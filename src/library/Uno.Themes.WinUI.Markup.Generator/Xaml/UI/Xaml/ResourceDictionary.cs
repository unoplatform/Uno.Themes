using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Uno.Markup.Xaml.Parsers;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Xaml.UI.Xaml;

public class ResourceDictionary : Dictionary<ResourceKey, IKeyedResource>
{
	private static readonly ILogger _logger = typeof(ResourceDictionary).Log();

	public static Dictionary<string, string> ThemeMapping = new()
	{
		["Light"] = "Light",
		["Dark"] = "Dark",
	};

	// fixme: do not merge themed-values into values, instead keep them inside ThemeDictionaries, and concat them when looking for expansion
	// (for debug enumerating, DO NOT concat into to IEnumerable.GetEnumerator)
	// also remove Merge() and use MergedDictionaries
	// that way we can preserve the windows hierarchy

	public ResourceDictionary() { }
	public ResourceDictionary(ResourceDictionary rd) : base(rd) { }

	public void Add(IKeyedResource resource)
	{
		if (!TryAdd(resource.Key, resource))
		{
#if ALLOW_DUPLICATED_KEYS
#if !ALLOW_DUPLICATED_KEYS_WITHOUT_WARNING
#if LINQPAD
			Util.WithStyle($"Duplicated key: {resource.Key}", "color: orange").Dump();
#else
			_logger.LogWarning($"Duplicated key: {resource.Key}");
#endif
#endif
#else
			throw new ArgumentException($"Duplicated key: {resource.Key}");
#endif
		}
	}
	public void AddRange(IEnumerable<IKeyedResource> resources)
	{
		foreach (var resource in resources)
			Add(resource);
	}
	public ResourceDictionary Merge(ResourceDictionary other)
	{
		AddRange(other.Values);
		return this;
	}

	public object ToDump() => Values;

	public IKeyedResource? this[string? Key = null, string? TargetType = null]
	{
		get => TryGetValue(new(Key, TargetType), out var value) ? value : default;
	}

	public static string? GetKey(XElement element)
	{
		// https://docs.microsoft.com/en-us/windows/apps/design/style/xaml-resource-dictionary
		// x:Name can be used instead of x:Key. However, [...less optimal].
		return
			element.Attribute(X + "Key")?.Value ??
			element.Attribute(X + "Name")?.Value;
	}
	private static ResourceKey GetResourceKey(XElement element)
	{
		if (GetKey(element) is string key)
		{
			return key;
		}
		if (element.Attribute("TargetType")?.Value is { } targetType)
		{
			return new(null, targetType);
		}

		throw new KeyNotFoundException().PreDump(element);
	}

	public static ResourceDictionary Parse(XElement e)
	{
		var result = new ResourceDictionary();
		foreach (var child in e.Elements())
		{
			if (child.Name.IsMemberOf<ResourceDictionary>(out var property))
			{
				_ = property switch
				{
					"ThemeDictionaries" => ParseThemeDictionaries(result, child),
					"MergedDictionaries" => ParseMergedDictionaries(result, child),

					_ => throw new NotImplementedException(property),
				};
			}
			else
			{
				result.Add(new StaticResource(ResourceDictionary.GetResourceKey(child), ScuffedXamlParser.Parse(child)));
			}
		}

		return result;
	}
	protected static ResourceDictionary?[] ParseThemeDictionaries(ResourceDictionary rd, XElement e)
	{
		var themes = Parse(e);
		var light = (themes.FirstOrDefault(x => ThemeMapping["Light"].Split(',').Contains(x.Key.Key)).Value as StaticResource)?.Value as ResourceDictionary;
		var dark = (themes.FirstOrDefault(x => ThemeMapping["Dark"].Split(',').Contains(x.Key.Key)).Value as StaticResource)?.Value as ResourceDictionary;

		object? GetResourceUnwrapped(ResourceDictionary? rd, ResourceKey key)
			//=> ((StaticResource)rd[key]).Value;
			=> rd?.TryGetValue(key, out var value) == true && value is StaticResource sr ? sr.Value : default;
		foreach (var key in Enumerable.Union((light?.Keys).Safe(), (dark?.Keys).Safe()))
			rd.Add(new ThemeResource(key, GetResourceUnwrapped(light, key), GetResourceUnwrapped(dark, key)));

		return new[] { light, dark };
	}
	protected static ResourceDictionary[] ParseMergedDictionaries(ResourceDictionary rd, XElement e)
	{
		var buffer = new List<ResourceDictionary>();
		foreach (var item in e.Elements())
		{
			var nested = Parse(item);
			rd.Merge(nested);
		}

		return buffer.ToArray();
	}

	/// <summary>Resolve or unwrap static/theme-resources with the provided context.</summary>
	internal void ResolveWith(ResourceDictionary context, int maxDepth = 100)
	{
		context = new ResourceDictionary(context).Merge(this);

		foreach (var key in Keys.ToArray())
		{
			var value = this[key];
			for (int i = 0; i < maxDepth; i++)
			{
				var referenced = value.GetResourceValue();
				if (referenced is IResourceRef rr && context.TryGetValue(rr.ResourceKey, out var result))
				{
					value = result;
				}
				else break;
			}

			this[key] = value;
		}
	}
}
