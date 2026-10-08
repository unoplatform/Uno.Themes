// Regenerates the Styles and Lightweight Styling tables of the per-control style pages
// (doc/styles/*.md for Material v2, doc/styles/simple/*.md for Simple) from the theme XAML.
//
//   dotnet run build/scripts/GenerateStyleDocs.cs              rewrite the generated regions
//   dotnet run build/scripts/GenerateStyleDocs.cs -- --check   exit 1 when a page is out of date
//
// Only the region between the BEGIN/END GENERATED markers is owned by this script. The inclusion
// rules (which styles, which lightweight keys, how types are resolved) are documented in
// specs/11-style-docs-generator/progress.md.

using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

const string BeginMarker = "<!-- BEGIN GENERATED -->";
const string EndMarker = "<!-- END GENERATED -->";
const string GeneratedNotice = "<!-- This region is generated from the Styles XAML by `dotnet run build/scripts/GenerateStyleDocs.cs`; do not edit it by hand. -->";
const string DefaultStyleFootnote = "IsDefaultStyle\\*: Styles in this column will be set as the default implicit style for the matching control";

var check = args.Contains("--check");
var repoRoot = Path.GetFullPath(Path.Combine(ScriptDirectory(), "..", ".."));
string Repo(string relative) => Path.Combine(repoRoot, relative);

var libraries = new[]
{
	new Library(
		"Material",
		Repo("src/library/Uno.Material/Styles/Controls/v2"),
		Repo("doc/styles"),
		[
			Repo("src/library/Uno.Material/Styles/Controls/v2"),
			Repo("src/library/Uno.Material/Styles/Application/Common"),
			Repo("src/library/Uno.Material/Styles/Application/v2"),
			Repo("src/library/Uno.Themes/Styles/Applications/Common"),
		]),
	new Library(
		"Simple",
		Repo("src/library/Uno.Simple.WinUI/Styles/Controls"),
		Repo("doc/styles/simple"),
		[
			Repo("src/library/Uno.Simple.WinUI/Styles"),
			Repo("src/library/Uno.Themes/Styles/Applications/Common"),
		]),
};

// Pages whose shape does not fit the uniform layout; they stay entirely hand-owned.
var handOwnedPages = new HashSet<string>(StringComparer.Ordinal) { "TextBlock" };

// Pages whose styles target a control type with a different name than the page.
var pageTargetTypes = new Dictionary<string, string>(StringComparer.Ordinal) { ["FloatingActionButton"] = "Button" };

var errors = new List<string>();
var stale = new List<string>();

foreach (var library in libraries)
{
	var resolver = new TypeResolver(library.TypeSources);
	var controlFiles = Directory.GetFiles(library.ControlsDir, "*.xaml").Order(StringComparer.Ordinal).ToArray();
	var allStyles = controlFiles.SelectMany(Xaml.ReadStyles)
		.GroupBy(s => s.Key, StringComparer.Ordinal)
		.ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

	// SemanticStyles.xaml was introduced by #1730; before it, the semantic style aliases live in
	// _Resources.xaml and the lightweight keys only in each control's own XAML.
	var semanticFile = Path.Combine(library.ControlsDir, "SemanticStyles.xaml");
	var resourcesFile = Path.Combine(library.ControlsDir, "_Resources.xaml");
	var semantic = File.Exists(semanticFile) ? Xaml.ReadResources(semanticFile).ToList() : [];
	var aliasesByStyle = semantic.Concat(Xaml.ReadResources(resourcesFile))
		.Where(r => r.Def.Theme is null && r.Def.Alias is { } target && allStyles.ContainsKey(target))
		.GroupBy(r => r.Def.Alias ?? "", StringComparer.Ordinal)
		.ToDictionary(g => g.Key, g => g.Select(r => r.Def.Key).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToList(), StringComparer.Ordinal);

	var defaultStyles = ResolveDefaultStyles(resourcesFile, allStyles);

	var pageCandidates = controlFiles
		.Select(f => Path.GetFileNameWithoutExtension(f))
		.Where(n => !n.StartsWith('_') && n != "SemanticStyles" && !n.EndsWith(".Base", StringComparison.Ordinal))
		.Where(n => !File.Exists(Path.Combine(library.DocsDir, n + ".md")))
		.Order(StringComparer.Ordinal);
	if (!check && pageCandidates.Any())
	{
		Console.Error.WriteLine($"info: {library.Name} controls with no page (not generated): {string.Join(", ", pageCandidates)}");
	}

	foreach (var pagePath in Directory.GetFiles(library.DocsDir, "*.md").Order(StringComparer.Ordinal))
	{
		var control = Path.GetFileNameWithoutExtension(pagePath);
		var relativePage = Path.GetRelativePath(repoRoot, pagePath).Replace('\\', '/');
		if (handOwnedPages.Contains(control))
		{
			continue;
		}

		var sources = new[] { control + ".xaml", control + ".Base.xaml" }
			.Select(n => Path.Combine(library.ControlsDir, n))
			.Where(File.Exists)
			.ToArray();
		if (sources.Length == 0)
		{
			errors.Add($"{relativePage}: no {control}.xaml in {Path.GetRelativePath(repoRoot, library.ControlsDir)}");
			continue;
		}

		var pageErrors = new List<string>();
		var region = RenderRegion(library, control, sources, semantic, allStyles, aliasesByStyle, defaultStyles, resolver, pageErrors);
		errors.AddRange(pageErrors.Select(e => $"{relativePage}: {e}"));
		if (pageErrors.Count > 0)
		{
			continue;
		}

		var original = File.ReadAllText(pagePath);
		string updated;
		try
		{
			updated = SplicePage(original, region, relativePage, check);
		}
		catch (InvalidOperationException ex)
		{
			errors.Add(ex.Message);
			continue;
		}

		if (updated == original)
		{
			continue;
		}

		stale.Add(relativePage);
		if (!check)
		{
			File.WriteAllText(pagePath, updated, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}
	}
}

if (errors.Count > 0)
{
	Console.Error.WriteLine($"error: {errors.Count} problem(s) while generating style docs:");
	errors.ForEach(e => Console.Error.WriteLine($"  {e}"));
	return 2;
}

if (check)
{
	if (stale.Count == 0)
	{
		Console.WriteLine("Style docs are up to date.");
		return 0;
	}

	Console.Error.WriteLine("error: these pages are out of date; run `dotnet run build/scripts/GenerateStyleDocs.cs`:");
	stale.ForEach(p => Console.Error.WriteLine($"  {p}"));
	return 1;
}

Console.WriteLine(stale.Count == 0 ? "Style docs are up to date." : $"Updated {stale.Count} page(s):");
stale.ForEach(p => Console.WriteLine($"  {p}"));
return 0;

// Walks each implicit style's BasedOn chain in _Resources.xaml; every style visited before a *Base* template is a default.
static HashSet<string> ResolveDefaultStyles(string resourcesFile, Dictionary<string, StyleDef> allStyles)
{
	var defaults = new HashSet<string>(StringComparer.Ordinal);
	foreach (var implicitStyle in XDocument.Load(resourcesFile).Root?.Elements().Where(e => e.Name.LocalName == "Style") ?? [])
	{
		var key = Xaml.MarkupKey(implicitStyle.Attribute("BasedOn")?.Value);
		while (key is not null && !key.Contains("Base", StringComparison.Ordinal) && defaults.Add(key))
		{
			key = allStyles.TryGetValue(key, out var style) ? style.BasedOn : null;
		}
	}

	return defaults;
}

string RenderRegion(
	Library library,
	string control,
	string[] sources,
	List<GroupedResource> semantic,
	Dictionary<string, StyleDef> allStyles,
	Dictionary<string, List<string>> aliasesByStyle,
	HashSet<string> defaultStyles,
	TypeResolver resolver,
	List<string> pageErrors)
{
	var md = new StringBuilder();
	md.AppendLine(BeginMarker).AppendLine(GeneratedNotice).AppendLine();

	// Styles: keyed styles targeting this control (or one of its parts named after it), minus *Base* templates and WinUI internals.
	var target = pageTargetTypes.GetValueOrDefault(control, control);
	var styleRows = new List<string[]>();
	foreach (var style in sources.SelectMany(Xaml.ReadStyles).DistinctBy(s => s.Key))
	{
		var included = style.TargetType.Contains(target, StringComparison.Ordinal)
			&& !style.Key.Contains("Base", StringComparison.Ordinal)
			&& !style.Key.StartsWith("MUX_", StringComparison.Ordinal);
		if (!included)
		{
			if (!check)
			{
				Console.Error.WriteLine($"info: {library.Name}/{control}: style `{style.Key}` (TargetType {style.TargetType}) excluded");
			}

			continue;
		}

		var aliases = aliasesByStyle.GetValueOrDefault(style.Key) ?? [];
		styleRows.Add([
			Code(style.Key),
			string.Join(", ", aliases.Select(Code)),
			defaultStyles.Contains(style.Key) ? "True" : "",
		]);
	}

	md.AppendLine("## Styles").AppendLine();
	AppendTable(md, ["Style Key", "Semantic Alias", "IsDefaultStyle\\*"], styleRows);
	md.AppendLine().AppendLine(DefaultStyleFootnote).AppendLine();

	// Lightweight keys: the SemanticStyles comment groups for this control, plus resources the control's own XAML defines.
	var groups = sources.Select(Path.GetFileNameWithoutExtension).ToHashSet(StringComparer.Ordinal);
	var sourceText = string.Concat(sources.Select(File.ReadAllText));
	var references = Xaml.References(sourceText);
	var ownResources = sources.SelectMany(s => Xaml.ReadResources(s).Select(r => r.Def)).ToList();
	var semanticResources = semantic.Select(r => r.Def).ToList();

	var keys = new SortedSet<string>(StringComparer.Ordinal);
	keys.UnionWith(semantic.Where(r => r.Group is { } g && groups.Contains(g)).Select(r => r.Def.Key));
	keys.UnionWith(ownResources.Where(d => !IsStyleAlias(d)).Select(d => d.Key));

	// Cross-check: a SemanticStyles key the control references must sit in that control's comment group.
	// Theme-agnostic aliases of styles (FilledButtonStyle, BodyMedium) are the semantic style layer, not lightweight keys.
	bool IsStyleAlias(ResourceDef d) => d.Theme is null && d.Alias is { } a && allStyles.ContainsKey(a);
	foreach (var stray in semanticResources.Where(d => !IsStyleAlias(d)).Select(d => d.Key).Distinct().Where(k => references.Contains(k) && !keys.Contains(k)).Order(StringComparer.Ordinal))
	{
		var owner = semantic.First(r => r.Def.Key == stray).Group ?? "no";
		pageErrors.Add($"`{stray}` is referenced by {control}.xaml but sits in the {owner} lightweight group of SemanticStyles.xaml");
	}

	var agnosticRows = new List<string[]>();
	var themedRows = new List<(string Key, string Type, string Light, string Dark, string? HighContrast)>();
	foreach (var key in keys)
	{
		// The control's own definitions win over SemanticStyles, mirroring dictionary precedence.
		var defs = ownResources.Where(d => d.Key == key).Concat(semanticResources.Where(d => d.Key == key)).ToList();
		var agnostic = defs.FirstOrDefault(d => d.Theme is null);
		var light = defs.FirstOrDefault(d => d.Theme == "Light");
		var dark = defs.FirstOrDefault(d => d.Theme == "Dark");
		var highContrast = defs.FirstOrDefault(d => d.Theme == "HighContrast");
		var primary = agnostic ?? light;
		if (primary is null)
		{
			pageErrors.Add($"`{key}` is themed but has no Light definition");
			continue;
		}

		if (agnostic is null && dark is null)
		{
			// A source gap rather than a generator failure: surface it on the page and in the log.
			Console.Error.WriteLine($"warning: {library.Name}/{control}: `{key}` has a Light but no Dark (Default) definition");
		}

		var type = resolver.Resolve(primary);
		if (type is null)
		{
			pageErrors.Add($"cannot resolve the type of `{key}` (alias chain from {Path.GetFileName(primary.File)} ends at an unknown key)");
			continue;
		}

		if (agnostic is not null)
		{
			agnosticRows.Add([Code(key), Code(type), agnostic.Value]);
		}
		else
		{
			themedRows.Add((key, type, primary.Value, dark?.Value ?? "(not defined)", highContrast?.Value));
		}
	}

	md.AppendLine("## Lightweight Styling").AppendLine();
	if (agnosticRows.Count == 0 && themedRows.Count == 0)
	{
		md.AppendLine("This control does not define lightweight styling resources.").AppendLine();
	}

	if (agnosticRows.Count > 0)
	{
		md.AppendLine("### Theme-agnostic").AppendLine();
		AppendTable(md, ["Key", "Type", "Value"], agnosticRows);
		md.AppendLine();
	}

	if (themedRows.Count > 0)
	{
		var showDark = themedRows.Any(r => r.Light != r.Dark);
		md.AppendLine("### Themed").AppendLine();
		AppendTable(
			md,
			showDark ? ["Key", "Type", "Light", "Dark"] : ["Key", "Type", "Light"],
			themedRows.Select(r => showDark
				? new[] { Code(r.Key), Code(r.Type), r.Light, r.Dark }
				: [Code(r.Key), Code(r.Type), r.Light]).ToList());
		md.AppendLine();
		if (!showDark)
		{
			md.AppendLine("The Dark theme uses the same values as Light.").AppendLine();
		}

		var highContrastDiffs = themedRows.Where(r => r.HighContrast is not null && r.HighContrast != r.Light).ToList();
		if (highContrastDiffs.Count > 0)
		{
			md.AppendLine("The HighContrast theme overrides these values:").AppendLine();
			highContrastDiffs.ForEach(r => md.AppendLine($"- {Code(r.Key)}: {r.HighContrast}"));
			md.AppendLine();
		}
	}

	md.Append(EndMarker);
	return md.ToString().Replace("\r\n", "\n");
}

// Replaces the generated region, or — on the first run — the hand-written `## Styles` … end-of-Lightweight-Styling range.
static string SplicePage(string original, string region, string relativePage, bool check)
{
	var eol = original.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
	var lines = original.Replace("\r\n", "\n").TrimEnd('\n').Split('\n').ToList();
	var regionLines = region.Split('\n');

	var begin = lines.IndexOf(BeginMarker);
	var end = lines.IndexOf(EndMarker);
	if (begin >= 0 && end > begin)
	{
		lines.RemoveRange(begin, end - begin + 1);
		lines.InsertRange(begin, regionLines);
	}
	else
	{
		var start = lines.IndexOf("## Styles");
		if (start < 0)
		{
			throw new InvalidOperationException($"{relativePage}: no generated markers and no `## Styles` heading to replace");
		}

		var stop = lines.FindIndex(start + 1, l => l.StartsWith("## ", StringComparison.Ordinal) && !l.StartsWith("## Lightweight Styling", StringComparison.Ordinal));
		if (stop < 0)
		{
			stop = lines.Count;
		}

		var preserved = ExtractProse(lines.GetRange(start, stop - start));
		foreach (var block in preserved)
		{
			if (!check)
			{
				Console.Error.WriteLine($"info: {relativePage}: moved below the generated region: {block[0]}");
			}
		}

		var replacement = new List<string>(regionLines);
		foreach (var block in preserved)
		{
			replacement.Add("");
			replacement.AddRange(block);
		}

		if (stop < lines.Count)
		{
			replacement.Add("");
		}

		lines.RemoveRange(start, stop - start);
		lines.InsertRange(start, replacement);
	}

	return string.Join(eol, lines) + eol;
}

// Paragraphs and `>` blocks in the replaced range that are neither tables, headings, nor the IsDefaultStyle footnote.
static List<List<string>> ExtractProse(List<string> range)
{
	var blocks = new List<List<string>>();
	List<string>? current = null;
	foreach (var line in range)
	{
		var isProse = line.Length > 0
			&& !line.StartsWith('|')
			&& !line.StartsWith('#')
			&& line != DefaultStyleFootnote;
		if (!isProse)
		{
			current = null;
			continue;
		}

		if (current is null)
		{
			current = [];
			blocks.Add(current);
		}

		current.Add(line);
	}

	return blocks;
}

static void AppendTable(StringBuilder md, string[] header, List<string[]> rows)
{
	var widths = header.Select((h, i) => Math.Max(3, rows.Select(r => r[i].Length).Prepend(h.Length).Max())).ToArray();
	void Row(IEnumerable<string> cells) => md.Append("| ").Append(string.Join(" | ", cells)).AppendLine(" |");
	Row(header.Select((h, i) => h.PadRight(widths[i])));
	Row(widths.Select(w => new string('-', w)));
	foreach (var row in rows)
	{
		Row(row.Select((c, i) => c.PadRight(widths[i])));
	}
}

static string Code(string value) => $"`{value}`";

static string ScriptDirectory([CallerFilePath] string path = "") => Path.GetDirectoryName(path) ?? ".";

record Library(string Name, string ControlsDir, string DocsDir, string[] TypeSources);

record StyleDef(string Key, string TargetType, string? BasedOn);

/// <summary>A keyed resource. <c>Theme</c> is null for theme-agnostic entries; <c>Alias</c> is the key it points at, if any.</summary>
record ResourceDef(string Key, string ElementType, string? Alias, string Value, string? Theme, string File);

record GroupedResource(ResourceDef Def, string? Group);

static class Xaml
{
	const string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
	const string PresentationNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

	static readonly XName KeyName = XName.Get("Key", XamlNamespace);
	static readonly HashSet<string> NonResourceElements = new(StringComparer.Ordinal) { "Style", "ControlTemplate", "DataTemplate", "ItemsPanelTemplate", "ResourceDictionary" };
	static readonly Regex GroupComment = new(@"^\s*([\w.]+)\.xaml lightweight resources\s*$", RegexOptions.CultureInvariant);
	static readonly Regex MarkupReference = new(@"\{(?:Theme|Static)Resource\s+(?:ResourceKey=)?([\w.]+)\s*\}|ResourceKey=""([\w.]+)""", RegexOptions.CultureInvariant);

	public static IEnumerable<StyleDef> ReadStyles(string file)
		=> (XDocument.Load(file).Root?.Elements() ?? [])
			.Where(e => e.Name.LocalName == "Style" && e.Attribute(KeyName) is not null)
			.Select(e => new StyleDef(
				e.Attribute(KeyName)?.Value ?? "",
				e.Attribute("TargetType")?.Value.Split(':').Last() ?? "",
				MarkupKey(e.Attribute("BasedOn")?.Value)));

	/// <summary>Top-level and ThemeDictionaries resources in document order, tagged with the `X.xaml lightweight resources` comment group they sit under.</summary>
	public static IEnumerable<GroupedResource> ReadResources(string file)
	{
		var root = XDocument.Load(file).Root;
		if (root is null)
		{
			yield break;
		}

		foreach (var resource in ReadScope(root, theme: null, file))
		{
			yield return resource;
		}

		foreach (var themeDictionary in root.Elements().Where(e => e.Name.LocalName == "ResourceDictionary.ThemeDictionaries").Elements())
		{
			var theme = themeDictionary.Attribute(KeyName)?.Value switch
			{
				"Default" or "Dark" => "Dark",
				var other => other,
			};
			foreach (var resource in ReadScope(themeDictionary, theme, file))
			{
				yield return resource;
			}
		}
	}

	static IEnumerable<GroupedResource> ReadScope(XElement scope, string? theme, string file)
	{
		string? group = null;
		foreach (var node in scope.Nodes())
		{
			if (node is XComment comment && GroupComment.Match(comment.Value) is { Success: true } match)
			{
				group = match.Groups[1].Value;
			}
			else if (node is XElement element && element.Attribute(KeyName)?.Value is { } key && IsResource(element))
			{
				yield return new GroupedResource(ToDef(element, key, theme, file), group);
			}
		}
	}

	static bool IsResource(XElement element)
	{
		var ns = element.Name.NamespaceName;
		var markupNamespace = ns == PresentationNamespace || ns == XamlNamespace || ns.StartsWith("http://uno.ui/", StringComparison.Ordinal);
		return markupNamespace && !NonResourceElements.Contains(element.Name.LocalName);
	}

	static ResourceDef ToDef(XElement element, string key, string? theme, string file)
	{
		var type = element.Name.LocalName;
		if (type == "StaticResource")
		{
			var target = element.Attribute("ResourceKey")?.Value ?? "";
			return new ResourceDef(key, type, target, $"`{target}`", theme, file);
		}

		if (type == "SolidColorBrush" && element.Attribute("Color")?.Value is { } color)
		{
			var colorKey = MarkupKey(color);
			var value = colorKey is null ? color : $"`{colorKey}`";
			if (element.Attribute("Opacity")?.Value is { } opacity)
			{
				value += $" (Opacity {opacity})";
			}

			return new ResourceDef(key, type, null, value, theme, file);
		}

		return new ResourceDef(key, type, null, Literal(element.Value), theme, file);
	}

	/// <summary>Literal values are shown unformatted (whitespace collapsed), with pipes escaped for the table.</summary>
	static string Literal(string text)
	{
		var value = Regex.Replace(text.Trim(), @"\s+", " ");
		if (value.Length == 0)
		{
			return "(empty)";
		}

		// Icon-font glyphs (private use area) and control characters render invisibly; show them as escapes.
		var escaped = new StringBuilder();
		foreach (var c in value)
		{
			escaped.Append(char.IsControl(c) || c is >= '\uE000' and <= '\uF8FF' ? $"\\u{(int)c:X4}" : c.ToString());
		}

		return escaped.Replace("|", "\\|").ToString();
	}

	public static string? MarkupKey(string? markup)
		=> markup is not null && MarkupReference.Match(markup) is { Success: true } m && m.Groups[1].Success ? m.Groups[1].Value : null;

	public static HashSet<string> References(string xamlText)
		=> MarkupReference.Matches(xamlText)
			.Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value)
			.ToHashSet(StringComparer.Ordinal);
}

/// <summary>Resolves a resource's element type, following StaticResource alias chains across the theme's dictionaries.</summary>
sealed class TypeResolver
{
	// Keys generated at runtime by BaseTheme.ScaleGeneration.cs (spacing, shape, density) and the typeface layer
	// (ThemesConstants.TypefaceScaleKeys); they have no XAML definition to walk.
	static readonly (Regex Pattern, string Type)[] GeneratedTokens =
	[
		(new(@"^Space(0|050|100|150|200|300|400|500|600|800|1200|1600|2400|4000)$"), "Double"),
		(new(@"^Space(0|050|100|150|200|300|400|500|600|800|1200|1600|2400|4000)Thickness$"), "Thickness"),
		(new(@"^Space(0|050|100|150|200|300|400|500|600|800)(Horizontal|Vertical|Top|Bottom|Left|Right)Thickness$"), "Thickness"),
		(new(@"^Radius(0|050|100|200|300|400|500|700|Full)$"), "Double"),
		(new(@"^Radius(0|050|100|200|300|400|500|700|Full)CornerRadius$"), "CornerRadius"),
		(new(@"^(TouchTargetMinSize|IconSize(Small|Medium|Large)|ControlHeight(Small|Medium|MediumLarge|Large))$"), "Double"),
		(new(@"^(Default|(Display|Headline|Title|Body|Caption)(Large|Medium|Small)|Label(Large|Medium|Small|ExtraSmall))FontFamily$"), "FontFamily"),
	];

	// WinUI framework resources the styles alias; they are defined by the platform, not by this repo.
	static readonly Dictionary<string, string> SystemResources = new(StringComparer.Ordinal)
	{
		["ControlContentThemeFontSize"] = "Double",
		["ControlCornerRadius"] = "CornerRadius",
		["OverlayCornerRadius"] = "CornerRadius",
		["SymbolThemeFontFamily"] = "FontFamily",
		["SystemControlDisabledChromeDisabledHighBrush"] = "SolidColorBrush",
		["SystemControlPageBackgroundMediumAltMediumBrush"] = "SolidColorBrush",
		["SystemControlTransparentBrush"] = "SolidColorBrush",
	};

	readonly Dictionary<string, List<ResourceDef>> _index = new(StringComparer.Ordinal);
	readonly HashSet<string> _styles = new(StringComparer.Ordinal);

	public TypeResolver(IEnumerable<string> directories)
	{
		foreach (var file in directories.SelectMany(d => Directory.GetFiles(d, "*.xaml", SearchOption.AllDirectories).Order(StringComparer.Ordinal)))
		{
			_styles.UnionWith(Xaml.ReadStyles(file).Select(s => s.Key));
			foreach (var resource in Xaml.ReadResources(file))
			{
				if (!_index.TryGetValue(resource.Def.Key, out var defs))
				{
					_index[resource.Def.Key] = defs = [];
				}

				defs.Add(resource.Def);
			}
		}
	}

	public string? Resolve(ResourceDef def) => Resolve(def, new HashSet<string>(StringComparer.Ordinal));

	string? Resolve(ResourceDef def, HashSet<string> visited)
	{
		if (def.Alias is null)
		{
			return def.ElementType;
		}

		if (!visited.Add(def.Key))
		{
			return null;
		}

		var target = def.Alias;
		if (_styles.Contains(target))
		{
			return "Style";
		}

		if (SystemResources.TryGetValue(target, out var systemType))
		{
			return systemType;
		}

		if (GeneratedTokens.FirstOrDefault(t => t.Pattern.IsMatch(target)) is { Type: { } generatedType })
		{
			return generatedType;
		}

		// Prefer theme-agnostic, then Light definitions, so the chain follows what a Light lookup would.
		var candidates = _index.GetValueOrDefault(target) ?? [];
		return candidates
			.OrderBy(d => d.Theme switch { null => 0, "Light" => 1, _ => 2 })
			.Select(d => Resolve(d, new HashSet<string>(visited, StringComparer.Ordinal)))
			.FirstOrDefault(t => t is not null);
	}
}
