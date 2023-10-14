using System.Text.RegularExpressions;

namespace Uno.Markup.Xaml;

public partial interface IKeyedResource
{
	ResourceKey Key { get; }

	public static string? GetKeyFromMarkup(string? markup)
	{
		if (markup is null) return null;

		return TryGetKeyFromMarkup(markup, out var key)
			? key
			: throw new ArgumentException("Invalid resource markup: " + markup);
	}
	public static bool TryGetKeyFromMarkup(string? maybeMarkup, out string? key)
	{
		if (maybeMarkup is not null && Regex.Match(maybeMarkup, @"^{(?<type>(Static|Theme|Dynamic)Resource) (?<key>\w+)}$") is { Success: true } match)
		{
			key = match.Groups["key"].Value;
			return true;
		}
		else
		{
			key = default;
			return false;
		}
	}
	public static IResourceRef? TryParseResource(string? maybeMarkup)
	{
		if (maybeMarkup is not null &&
			Regex.Match(maybeMarkup, @"^{(?<type>(Static|Theme|Dynamic)Resource) (?<key>\w+)}$") is { Success: true, Groups: var g })
		{
			return g["type"].Value switch
			{
				"StaticResource" => new StaticResourceRef(g["key"].Value),
				"ThemeResource" => new ThemeResourceRef(g["key"].Value),
				"DynamicResource" => throw new NotImplementedException("DynamicResource"),

				_ => throw new ArgumentOutOfRangeException($"Invalid resource markup: {g["type"].Value}"),
			};
		}

		return default;
	}

	public object GetResourceValue()
	{
		// favoring Light value when resolving for simplicity
		return this switch
		{
			StaticResource sr => sr.Value,
			ThemeResource tr => tr.LightValue,

			_ => throw new ArgumentOutOfRangeException(),
		};
	}

	[GeneratedRegex(@"^{(?<type>(Static|Theme|Dynamic)Resource) (?<key>\w+)}$")]
	private static partial Regex ResourceMarkupExtensionPattern();
}

// Static/ThemeResource indicates the resources was stored whether under RD or under RD.ThemeDictionaries
// Static/ThemeResourceRef indicates the type of resource alias(<StaticResource ResourceKey="..." />) or markup({StaticResource ...})

public record StaticResourceRef(string ResourceKey) : IResourceRef
{
#if LINQPAD && DUMP_RESOURCEREF_WITH_KEY_INLINE
	private object ToDump() => Util.OnDemand($"{{StaticResource {ResourceKey}}}", () => this);
#endif
}

public record ThemeResourceRef(string ResourceKey) : IResourceRef
{
#if LINQPAD && DUMP_RESOURCEREF_WITH_KEY_INLINE
	private object ToDump() => Util.OnDemand($"{{ThemeResource {ResourceKey}}}", () => this);
#endif
}
