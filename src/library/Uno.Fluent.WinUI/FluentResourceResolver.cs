#nullable enable

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Fluent;

/// <summary>
/// Resolves consumer resources for an explicit appearance without consulting
/// the ambient application or system resources.
/// </summary>
internal static class FluentResourceResolver
{
	internal static bool TryResolve(ResourceDictionary? dictionary, string appearance, string key, out object? value)
	{
		value = Resolve(dictionary, appearance, key);
		return value is not null;
	}

	/// <summary>
	/// Follows resource dictionary precedence: own entries, merged dictionaries
	/// in reverse order, then the selected appearance dictionary. Default is used
	/// only when the requested appearance dictionary is absent, not when a key
	/// is absent from an existing appearance dictionary.
	/// </summary>
	internal static object? Resolve(ResourceDictionary? dictionary, string appearance, string key)
	{
		if (dictionary is null)
		{
			return null;
		}

		// TryGetValue also searches merged, themed and system resources. Enumerate
		// to distinguish the dictionary's own entries from those fallback scopes.
		foreach (var entry in dictionary)
		{
			if (entry.Key is string entryKey && entryKey == key)
			{
				return entry.Value;
			}
		}

		for (var i = dictionary.MergedDictionaries.Count - 1; i >= 0; i--)
		{
			if (Resolve(dictionary.MergedDictionaries[i], appearance, key) is { } mergedValue)
			{
				return mergedValue;
			}
		}

		if (dictionary.ThemeDictionaries.TryGetValue(appearance, out var themed)
			&& themed is ResourceDictionary branch)
		{
			return Resolve(branch, appearance, key);
		}

		return dictionary.ThemeDictionaries.TryGetValue("Default", out var fallback)
			&& fallback is ResourceDictionary defaultBranch
				? Resolve(defaultBranch, appearance, key)
				: null;
	}
}
