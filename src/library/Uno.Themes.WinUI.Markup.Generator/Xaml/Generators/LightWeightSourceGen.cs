using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Uno.Markup.Misc;
using Uno.Markup.Xaml.Parsers;
using Uno.Markup.Xaml.UI.Xaml;

namespace Uno.Markup.Xaml.Generators;

internal static class LightWeightSourceGen
{
	public static string? GenerateCsMarkup(string file, ResourceDictionary context, SourceGenOptions? options = default)
	{
		if (options?.Skip == true) return null;

		options ??= new();
		options.TargetType ??= Path.GetFileNameWithoutExtension(file);

		var rd = ScuffedXamlParser.Load<ResourceDictionary>(file) ?? throw new InvalidOperationException();
		rd.ResolveWith(context);

		//rd.Values.Dump("ResourceDictionary", 0);
		//options.Dump("options", 0);

		var resources = new NamedResourceBag { Name = "Resources" };
		var styles = new NamedResourceBag { Name = "Styles", Sortable = false };
		foreach (var resource in rd)
		{
			if (resource.Value is StaticResource sr &&
				sr.Value is Style style &&
				style.TargetType?.Split(':', 2).LastOrDefault() == options.TargetType &&
				style.Setters.Any())
			{
				//Util.Metatext($"Style: {sr.Key}").Dump();
				//style.Dump($"Style: {sr.Key}", 0);
				var name = (sr.Key.Key ?? throw new InvalidOperationException("key not found"))
					.RemoveHead(options.TrimMaterialPrefix ? "Material" : null)
					.RemoveTail("Style")
					.RemoveTail(options.TargetType);
				if (name != "Base")
				{
					var srr = new StaticResourceRef(sr.Key.Key);
					styles.Resources.Add(new StaticResourceRef(sr.Key.Key));
					styles.ContextedResources.Add((srr, name.EmptyAsNull() ?? "Default"));
				}

				var wip = ExtractRelationship(options, rd, style);
				var styleResources = ReHierarchize(options, rd, style, wip);

				styleResources.Name = name;
				{
					// we need to remove the extra prefix from its resources+descendents before the context is lost.
					//styleResources.Dump("styleResources", 0);
					var prefixes = sr.Key.Key.RemoveTail("Style")
						.PermuteWords()
						.Concat(options.TrimMaterialPrefix && name?.Length > 0 ? name.PermuteWords() : Array.Empty<string>())
						.Concat(options.TrimMaterialPrefix ? new[] { "Material" } : Array.Empty<string>())
						.ToArray();
					styleResources.Flatten(x => x.Children).ForEach(item =>
					{
						//item.Dump(item.Name, 0);
						if (item != styleResources)
						{
							item.Name = UpdateName(item.Name);
						}
						item.ContextedResources = item.ContextedResources
							.Select(x => x with { Context = UpdateName(x.Context) })
							.ToHashSet();
					});

					string? UpdateName(string? name)
					{
						if (name is { })
						{
							foreach (var prefix in prefixes)
							{
								if (name.StartsWith(prefix))
								{
									return name[prefix.Length..];
								}
							}
						}

						return name;
					}
				}

				if (options.PromoteDefaultStyleResources && ",Base,Default".Split(',').Contains(styleResources.Name))
				{
					resources.Merge(styleResources);
				}
				else
				{
					resources.Children.Add(styleResources);
				}
			}
		}

		PreGenProcessing(options, resources);

		var control = new NamedResourceBag { Name = options.TargetType, Sortable = false, Children = { resources, styles } };
		var theme = new NamedResourceBag { Name = "Theme", Sortable = false, Children = { control } };
		var source = Generate(options, rd, theme);

		return source;
	}

	private static NamedResourceBag ExtractRelationship(SourceGenOptions options, ResourceDictionary rd, Style style)
	{
		var templateRoot = style.GetTemplateRoot();
		var vsgs = (templateRoot?.GetVisualStateGroups())
			.Safe()
			.ToArray()
			//.Dump("VisualStateGroups", 0)
			;

		// Visual Tree
		templateRoot
			?.TreeGraph(x => x.Children, x => x.Name is { Length: > 0 } name ? $"{x.Type}#{name}" : x.Type)
			//.OnDemand("Click to expand")
			//.Dump("Visual Tree")
			;
		templateRoot
			?.Flatten()
			//.Dump("Flattened VisualTree", 0)
			;

		var propertiesContainingReferences = Enumerable
			.Concat(
				style.Setters.Select(x => new { Path = "", x.Property, x.Value }),
				(templateRoot ?? new VisualTreeElement("empty"))
					.Flatten()
					.SelectMany(x => Enumerable
						.Concat(x.Element.Properties, x.Element.AttachedProperties)
						.Select(y => new { x.Path, Property = (string?)y.Key, y.Value })
					)
			)
			.Where(x => x.Value is IResourceRef)
			.Where(x => x.Property != "Style")
			.OrderBy(x => x.Property)
			.Select(x => new
			{
				Path = $"{x.Path}/@{x.Property}",
				TargetName = x.Path.Split('/').Last().Split('#') is [var type, var name] ? name : null,
				PropertyName = x.Property,
				//Value = y.Value,
				Reference = (IResourceRef)x.Value!,
			})
			//.GroupBy(x => x.Value, (k, g) => new { Key = k, Properties = g.Select(x => x.Path).JoinBy("\n") })
			.Where(x => rd.ContainsKey(x.Reference.ResourceKey))
			//.Dump("Referential Properties by XPath", 0)
			;

		var propertiesAffectedByReferentialVSGs = vsgs
			.SelectMany(vsg => new[]
				{
					vsg.VisualStates.SelectMany(vs => vs.Setters.Select(x => new
					{
						Source = vs.Name, Target = (string?)x.Target, x.Value, IsTimeline = false
					})),
					vsg.VisualStates.SelectMany(vs => (vs.Storyboard?.Children).Safe().Select(x => new
					{
						Source = vs.Name, Target = (string?)(x.TargetName + x.TargetProperty?.Prefix(".")), Value = (object?)x.Value, IsTimeline = true
					})),
					vsg.Transitions.SelectMany(t => (t.Storyboard?.Children).Safe().Select(x => new
					{
						Source = (string?)$"{t.From}->{t.To}", Target = (string?)(x.TargetName + x.TargetProperty?.Prefix(".")), Value = (object?)x.Value, IsTimeline = true
					})),
				}
				.SelectMany(row => row.Safe().Select(x => new { Source = $"{vsg.Name}\\{x.Source}", x.Target, x.Value, x.IsTimeline }))
			)
			// Timelines have simplified value, its references are expressed as S[Key] or T[Key]
			.Where(x => !x.IsTimeline ? x.Value is IResourceRef : x.Value is string simplified && Regex.IsMatch(simplified, @"\[\w+\]"))
			// extract references used
			.Select(x => new
			{
				x.Source,
				//x.Target, 
				TargetName = x.Target?.Split('.')[0],
				PropertyName = x.Target?.Split('.', 2)[^1]?.StripPair("()"),
				x.Value,
				References = (x.Value switch
				{
					IResourceRef rr => new[] { rr },
					string simplified when x.IsTimeline => Regex.Matches(simplified, @"(S|T)\[(\w+)\]").Cast<Match>()
						.Select(x => (IResourceRef)(x.Groups[1].Value switch
						{
							"S" => new StaticResourceRef(x.Groups[2].Value),
							"T" => new ThemeResourceRef(x.Groups[2].Value),
							_ => throw new ArgumentOutOfRangeException(),
						}))
						.ToArray(),
					_ => Array.Empty<IResourceRef>(),
				}).Where(y => rd.ContainsKey(y.ResourceKey))
			})
			.Where(x => x.References?.Any() ?? false)
			.ToArray()
			//.Dump("Properties affected by visual-state (that also reference local resources)", 0)
			;

		// note: since we did filter both left and right sides by "containing reference",
		// here we need to do a full-join
		// fixme: perhaps it is better to not pre-filter sources, and perform a inner-join and then filter?
		//		^ we will still need to left/right-join, since VSG can modified a locally undefined property...
		var tmp = (templateRoot ?? new VisualTreeElement("empty"))
			.Flatten()
			.Select(x => new
			{
				x.Path,
				TargetName = x.Path.Split('/').Last().Split('#') is [var type, var name] ? name : null,
			})
			.Prepend(new { Path = "", TargetName = default(string) })
			.Select(x => new
			{
				x.Path,
				Properties = Enumerable.Concat(
					propertiesContainingReferences
						.Where(y => y.Path.Split("/@", 2)[0] == x.Path) // note: we can have referential properties on x:name-less element, and they need to be preserved
						.Select(y => new { y.PropertyName, y.Reference, Context = "Default" }),
					propertiesAffectedByReferentialVSGs
						.Where(y => y.TargetName == x.TargetName)
						.SelectMany(y => y.References.Select(z => new { y.PropertyName, Reference = z, Context = y.Source }))
				)
			})
			.SelectMany(x => x.Properties.Select(y => new { x.Path, y.PropertyName, y.Reference, y.Context }))
			.GroupBy(x => $"{x.Path}/@{x.PropertyName}", (k, g) => new
			{
				Path = k,
				References = g.Select(x => (x.Reference, Context: (string?)x.Context)).ToArray()
			})
			//.Dump("query: pre-group 1", 0)
			// ---
			.GroupBy(x => x.References.OrderByDescending(x => x.Context == "Default").FirstOrDefault(), (k, g) => new
			{
				//k.Reference,
				Reference = k.Reference.ResourceKey,
				k.Context,
				//ResourceGroupAndPaths = g.GroupBy(x => x.References, new SequentialEqualityComparer<(IResourceRef, string)>()),
				//ResourceGroupAndPaths2 = g.GroupBy(x => x.References, (k, g) => (Keys: k, Paths: g.Select(x => x.Path)), new SequentialEqualityComparer<(IResourceRef Reference, string Context)>()).SingleOrDefault(),
				ResourceGroupAndPaths = (
					Keys: g.SelectMany(x => x.References),
					Paths: g.Select(x => x.Path)
				)
			})
			// consolidate by key, favoring non-visual-state-based value
			.OrderByDescending(x => x.Context == "Default").DistinctBy(x => x.Reference)
			.OrderBy(x => x.Reference)
			.OrderByDescending(x => x.ResourceGroupAndPaths.Keys.Count()).ThenBy(x => x.Reference) // debug ordering
			.Select(x => new
			{
				x.Reference,
				x.Context,
				ResourceGroup = x.ResourceGroupAndPaths.Keys,
				Paths = x.ResourceGroupAndPaths.Paths,
			})
			.ToArray()
			//.Dump("tmp", 0)
			;

		var result = new NamedResourceBag();
		result.Children.AddRange(tmp.Select(x => new NamedResourceBag
		{
			Name = x.Reference,
			Resources = x.ResourceGroup.Select(y => y.Reference).ToHashSet(),
			ContextedResources = x.ResourceGroup.ToHashSet(),
			Paths = x.Paths.ToHashSet(),
		}));

		return result;
	}
	private static NamedResourceBag ReHierarchize(SourceGenOptions options, ResourceDictionary rd, Style style, NamedResourceBag wip)
	{
		// inject context
		wip.Children.ForEach(x => InferRawBagContext(options, style.TargetType, x));
		//if (wip.Children.Any(x => x.Resources.Any(y => y.ResourceKey.StartsWith(""))))
		//{
		//	wip.Children
		//		.OrderBy(x => x.Name)
		//		//.Where(x => x.Name.Contains("Elevation"))
		//		.Dump("wip.Children (pre-hierarchy)", 1, exclude: "Resources,Children,Sortable"); // note: depth=0 will not snapshot the state
		//	//throw new NotImplementedException();
		//}

		// reduce perfect resource subset
		foreach (var item in wip.Children.ToArray())
		{
			if (wip.Children.FirstOrDefault(x => x != item && item.Resources.IsSubsetOf(x.Resources)) is { } superset)
			{
				wip.Children.Remove(item);
				superset.Merge(item);
			}
		}
		//if (wip.Children.SelectMany(x => x.Resources).Count() != wip.Children.SelectMany(x => x.Resources).Distinct().Count())
		//{
		//	wip.Children.SelectMany(x => x.Resources).Select(x => x.ResourceKey).Dump().Distinct().Dump();
		//	Debug.Assert(wip.Children.SelectMany(x => x.Resources).Count() == wip.Children.SelectMany(x => x.Resources).Distinct().Count(), "from here on, there is should be no duplicate");
		//}

		var resources = new NamedResourceBag { Name = "Resources" };
		//wip.Children.OrderBy(x => x.Name).ToArray().Dump("wip.Children", 0);

		IEnumerable<NamedResourceBag> CombineByLeftNameIfPossible(IEnumerable<NamedResourceBag> bags)
		{
			var results = new List<NamedResourceBag>();
			foreach (var g in bags.GroupBy(x => x.LeftName))
			{
				var children = g.ToList();

				if (children is [var singleEntry])
				{
					// linear
					results.Add(singleEntry);
				}
				else if (string.IsNullOrEmpty(g.Key) ||
					style.Setters.Any(x => x.Property == g.Key))
				{
					// linear
					results.AddRange(children);
				}
				else
				{
					// hierarchical
					var bag = new NamedResourceBag { Name = g.Key, Children = children };
					foreach (var item in bag.Children)
					{
						item.Name = item.Name?.RemoveHead(g.Key);
					}

					results.Add(bag);
				}
			}

			// setter property and vsg will not be grouped by .LeftName, so we have to handle them separately at the end
			foreach (var setterProperty in results.Where(x => x.Paths.JustOneOrDefault()?.StartsWith("/@") == true).ToArray())
			{
				if (results
					.Flatten(x => x.Children)
					.FirstOrDefault(x => x != setterProperty && setterProperty.Name == (x.LeftName + x.RightName)) is { } matchingVsg)
				{
					results.Remove(setterProperty);
					matchingVsg.Merge(setterProperty);
				}
			}

			//results.Dump("results (after setters re-insert)", 1);
			return results;
		}

		// 1. extract forced groups
		// we are extracting forced groups before setter properties,
		// because otherwise it could result in resources belonging to the same property
		// placed under different nodes (setter vs visual-state-group), thus prevent them from being combined.
		wip.Children.GroupBy(x => options.ForcedGroupings?.FirstOrNull(kvp => (kvp.Value ?? kvp.Key).Split(',').Any(y => x.Name?.Contains(y) ?? false))?.Key)
			//.OrderBy(x => x.Key).Dump("ForcedGroupings", 1)
			.Where(g => g.Key != null)
			.ForEach(g =>
			{
				foreach (var item in g)
				{
					wip.Children.Remove(item);
					item.Name = item.Name?.RemoveOnce(g.Key);
					item.LeftName = item.LeftName?.RemoveOnce(g.Key);
				}

				var bag = new NamedResourceBag
				{
					Name = g.Key,
					Children = CombineByLeftNameIfPossible(g).ToList(),
				};
				resources.Children.Add(bag);
			});

		// ~~2. extract setter(control-level) properties~~ merged into next part
		//wip.Children.Where(x => x.DebugPaths.All(y => y.StartsWith("/@")))
		//	.ToArray()
		//	.ForEach(x =>
		//	{
		//		wip.Children.Remove(x);
		//		resources.Children.Add(x);
		//	});

		// 3. extract remainders
		resources.Children.AddRange(CombineByLeftNameIfPossible(wip.Children));

		//resources.Dump("resources", 0);
		//resources.Children.Dump("resources.Children", 1);
		//resources.Flatten(x => x.Children).MustAll(x => x.Resources.Count * x.Children.Count == 0, x => $"{x.Name} contains both resource-group and children");

		return resources;
	}
	private static void InferRawBagContext(SourceGenOptions options, string? targetType, NamedResourceBag bag)
	{
		bag.Name = bag.Name?.RemoveHead(options.TrimControlName ? targetType : null);
		bag.RightName = InferVisualStateBagName(bag);
		bag.LeftName = InferLeftName(bag, bag.RightName);

		string? InferVisualStateBagName(NamedResourceBag bag)
		{
			var properties = bag.Paths.Select(x => x.Split("/@", 2).ElementAtOrDefault(1)).Distinct().ToArray();
			if (properties.Length == 1)
			{
				return properties[0];
			}
			// exceptions...
			if (properties.Length == 2)
			{
				var exceptions = new List<(string Properties, string MergedNames)>
				{
					("Width,Height", "Length,Thickness,Size"),
					("RadiusX,RadiusY", "Radius"),
				};
				foreach (var (propertyGroup, aliases) in exceptions)
				{
					if (properties.IsSubset(propertyGroup.Split(',')) &&
						aliases.Split(',').FirstOrDefault(x => bag.Name?.EndsWith(x) ?? false) is { } property)
					{
						return property;
					}
				}
			}

			return null;
		}
		static string? InferLeftName(in NamedResourceBag bag, string? rightName) // 2nd argument used to suggest order
		{
			var visualStateNames = bag.ContextedResources
				.Select(x => x.Context?.Split('\\', 2) is [var vsg, var state] ? state : null)
				.OfType<string>() // filter null
				.ToArray();
			var suffixesOptions = new List<string[]> { visualStateNames };
			if (rightName != null)
				suffixesOptions.Add(new[] { rightName });

			var result = bag.Name;
			var hit = true;
			do
			{
				hit = false;
				foreach (var options in suffixesOptions)
				{
					if (options.FirstOrDefault(x => result?.EndsWith(x) ?? false) is { } tail)
					{
						result = result?.RemoveTail(tail);

						suffixesOptions.Remove(options);
						hit = true;
						break;
					}
				}
			} while (hit && suffixesOptions.Count > 0);

			return result;
		}
	}
	private static NamedResourceBag PreGenProcessing(SourceGenOptions options, NamedResourceBag resources)
	{
		resources.Flatten(x => x.Children).ForEach(AdjustVisualStateGroupedResources);
		TrimAndPromote();
		PromoteSingleResource(resources);

		void AdjustVisualStateGroupedResources(NamedResourceBag bag)
		{
			// flag vsg-bag, so they are non-sortable
			if (bag.ContextedResources.Select(x => x.Context).Where(x => x != "Default").ToArray() is { Length: > 0 } contexts &&
				contexts.All(x => x?.Contains(@"\") ?? false))
			{
				bag.IsVSG = true;
			}

			// remove vsg name from vs name
			bag.ContextedResources = bag.ContextedResources
				.Select(x => x with { Context = x.Context?.Split('\\', 2)[^1] })
				.ToHashSet();

			// correct vsg names that doesnt have normal/default state
			if (bag.Resources.Count > 1 &&
				bag.ContextedResources.Select(x => x.Context).FirstOrDefault(x => x != null && bag.Name?.Contains(x) == true) is { } stateName)
			{
				bag.Name = bag.Name?.RegexReplace(Regex.Unescape(stateName), "");
			}
		}
		void PromoteSingleResource(NamedResourceBag node)
		{
			foreach (var item in node.Children)
			{
				PromoteSingleResource(item);
			}

			foreach (var item in node.Children.Where(x => x.Resources.Count == 1 && x.Children.Count == 0).ToArray())
			{
				node.Children.Remove(item);

				if ((item.Paths.FirstOrDefault()?.StartsWith("/@") ?? false) &&
					node.Children.FirstOrDefault(x => x != item && (x.Name == item.Name)) is { } vsg)
				{
					// merge with relevant visual state group
					vsg.Merge(item);
				}
				else
				{
					// otherwise promote
					if (item.Resources.FirstOrDefault() is { } resource)
					{
						node.Resources.Add(resource);
						node.ContextedResources.Add((resource, Context: item.Name));
						node.Paths.AddRange(item.Paths);
					}
				}
			}
		}
		void TrimAndPromote()
		{
			foreach (var style in resources.Children)
			{
				style.ContextedResources = style.ContextedResources.Select(x => x with { Context = UpdateName(x.Context) }).ToHashSet();
				style.Children.ForEach(x => x.Name = UpdateName(x.Name));

				string? UpdateName(string? name) => name
					?.RemoveHead(style.Name)
					.RemoveHead(options.TargetType);
			}

			TryPromote(resources);

			void TryPromote(NamedResourceBag bag)
			{
				foreach (var item in bag.Children.ToArray())
				{
					TryPromote(item);

					// if we fully trimmed the name of a bag or if its name is same as parent's, it should just be merged into its parent
					if (string.IsNullOrEmpty(item.Name) || item.Name.PermuteWords().Contains(bag.Name))
					{
						// prevent generation of .Default property when its Name is fully trimmed
						if (string.IsNullOrEmpty(item.Name) && item.Resources.Count == 1)
						{
							item.ContextedResources = item.ContextedResources
								.Select(x => x with { Context = item.Paths.FirstOrDefault()?.Split("/@", 2)[^1] })
								.ToHashSet();
						}

						bag.Children.Remove(item);
						bag.Merge(item);
					}
				}
			}
		}

		return resources;
	}
	private static string Generate(SourceGenOptions options, ResourceDictionary rd, NamedResourceBag theme)
	{
		//theme.Dump("theme bag", 0);
		//theme.Children.FirstOrDefault(x => x.Name == "Resources").Dump("Resources bag", 0);
		//theme.Children.FirstOrDefault(x => x.Name == "Styles").Dump("Styles bag", 0);

		var buffer = new IndentedTextBuilder("\t") { BlockConsecutiveEmptyLines = true };

		using (WriteFileHeader())
		{
			WriteBag(theme);
		}

		return buffer.ToString();

		bool WriteBag(NamedResourceBag bag)
		{
			//bag.Dump(bag.Name ?? "<null>", 0);
			var children = GetChildrenSorted(bag).ToArray();
			var resources = bag.ContextedResources
				.Where(x => !(options.IgnoredResourceTypes?.Contains(ResolveResourceNiceTypeName(x.Resource)) ?? false))
				// trim duplicate resource keys
				.OrderByDescending(y => y.Context == "Default")
				.ThenByDescending(y => y.Context == "Normal")
				.DistinctBy(y => y.Context)
				.DistinctBy(y => y.Resource)
				.Apply(sequence =>
					!bag.Sortable ? sequence :
					!bag.IsVSG ? sequence.OrderBy(x => x.Context) : sequence
						.OrderByDescending(y => y.Context == "Default")
						.ThenByDescending(y => y.Context == "Normal")
				)
				.ToArray();

			if (children.Length == 0 && resources.Length == 0) return false;

			var hadSibling = false;
			// .default syntax: // used to skip 2nd part of... (Background.Default/Normal)
			//		public static readonly BackgroundVSG Background = new();
			//		public class BackgroundVSG
			//			public ThemeResourceKey<Brush> Default { get; } = ...;
			//			public ThemeResourceKey<Brush> PointerOver { get; } = ...;
			//			public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
			var vsgDefaultState = options.Production && bag.IsVSG
				? resources.FirstOrNull(x => x.Context is "Default" or "Normal")
				: null;
			var useDefaultShortcutSyntax = vsgDefaultState is { };
			var implClassName = useDefaultShortcutSyntax ? $"{bag.Name}VSG" : bag.Name;

			// write .default syntax alias
			if (useDefaultShortcutSyntax)
			{
				// public static readonly BackgroundVSG Background = new();
				buffer.AppendLine($"public static readonly {implClassName} {bag.Name} = new();");
				// alias and the class should be grouped, without empty line inbetween
			}

			// write class header
			buffer.AppendLine(options.Production
				? $"public {(!useDefaultShortcutSyntax ? "static " : "")}partial class {implClassName}"
				: $"class {implClassName} // Sortable={bag.Sortable}, IsVSG={bag.IsVSG}");
			using var _ = options.Production ? buffer.Block("{", "}") : buffer.Block();

			// write nested
			foreach (var item in children)
			{
				if (options.Production && hadSibling) buffer.AppendEmptyLine();
				if (WriteBag(item))
				{
					hadSibling = true;
				}
			}

			// write resources
			foreach (var item in resources)
			{
				var type = ResolveResourceNiceTypeName(item.Resource);
				if (bag.Name != "Styles" && type == "Style") continue; // ignore nested style unless we are under 'Styles'

				if (options.Production && hadSibling) buffer.AppendEmptyLine();

				if (options.Production) buffer
					.AppendLine($"[ResourceKeyDefinition(typeof({type}), \"{item.Resource.ResourceKey}\"{(type == "Style" && ResolveStyleTargetType(item.Resource) is { } targetType ? $", TargetType = typeof({ResolveStyleTargetType(item.Resource)})" : "")})]");
				buffer
					   .AppendLine(options.Production
						? $"public {(!useDefaultShortcutSyntax ? "static " : "")}{item.Resource.GetTypename()}Key<{type}> {item.Context} {(useDefaultShortcutSyntax ? "=" : "=>")} new(\"{item.Resource.ResourceKey}\");"
						: $"{item.Resource.GetTypename()}<{type}> {item.Context} => new(\"{item.Resource.ResourceKey}\");"
					);

				hadSibling = true;
			}

			// write .default syntax implicit operator
			if (useDefaultShortcutSyntax)
			{
				if (options.Production && hadSibling) buffer.AppendEmptyLine();

				// public static implicit operator ThemeResourceKey<Brush>(BackgroundVSG self) => self.Default;
				var type = ResolveResourceNiceTypeName(vsgDefaultState?.Resource!);
				buffer.AppendLine($"public static implicit operator ThemeResourceKey<{type}>({implClassName} self) => self.{vsgDefaultState?.Context};");

				hadSibling = true;
			}

			return true;
		}
		IDisposable WriteFileHeader()
		{
			if (!options.Production) return Disposable.Empty;

			foreach (var import in options.NamespaceImports.Safe())
			{
				if (!string.IsNullOrEmpty(import))
				{
					buffer.AppendLine($"using {import};");
				}
				else
				{
					buffer.AppendEmptyLine();
				}
			}
			buffer.AppendEmptyLine();

			if (options.UseFileScopedNamespace)
			{
				buffer.AppendLine($"namespace {options.Namespace};");
				buffer.AppendEmptyLine();
				return Disposable.Empty;
			}
			else
			{
				buffer.AppendLine("namespace Uno.Themes.Markup");
				return buffer.Block("{", "}");
			}
		}

		IEnumerable<NamedResourceBag> GetChildrenSorted(NamedResourceBag bag)
		{
			var results = bag.Children
				.OrderByDescending(x => x.Resources.Any() || x.Children.Any());

			if (bag.Sortable)
			{
				results = results.ThenBy(x => x.Name);
			}

			return results;
		}
		string? ResolveStyleTargetType(IResourceRef rr)
		{
			return (rd.TryGetValue(rr.ResourceKey, out var resolved) ? resolved.GetResourceValue() : null) switch
			{
				Style style => ResolveXamlControlType(style.TargetType),
				_ => null,
			};

			string? ResolveXamlControlType(string? xamltype)
			{
				return //xamltype?.Split(':', 2)[^1];
					options.XamlControlTypeResolver?.Invoke(xamltype) ??
					xamltype?.Split(':', 2) switch // guestimate
					{
						[.., var typename] when "Popup,ToggleButton".Split(',').Contains(typename)
							=> $"global::Microsoft.UI.Xaml.Controls.Primitives.{typename}",
						[var typename] => $"global::Microsoft.UI.Xaml.Controls.{typename}",
						["muxc", var typename] => $"global::Microsoft.UI.Xaml.Controls.{typename}",
						[var xmlns, var typename] => $"{xmlns}:{typename}",

						_ => xamltype,
					};
			}
		}
		string? ResolveResourceType(IResourceRef rr)
		{
			return (rd.TryGetValue(rr.ResourceKey, out var resolved) ? resolved.GetResourceValue() : null) switch
			{
				GenericValueObject npo => npo.Typename,
				VisualTreeElement vte => vte.Type,
				var x => x?.GetType().Name,
			};
		}
		string? ResolveResourceNiceTypeName(IResourceRef rr)
		{
			return ResolveResourceType(rr) switch
			{
				"Double" => "double",
				"Int32" => "int",
				"String" => "string",
				"Boolean" => "bool",

				"SolidColorBrush" => "Brush",

				var x => x,
			};
		}
	}
}
