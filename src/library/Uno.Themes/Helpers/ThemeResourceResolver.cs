#nullable enable

using System;
using System.Collections.Generic;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes.Helpers;

/// <summary>
/// Resolves consumer resources for an explicit appearance without consulting
/// the ambient application or system resources.
/// </summary>
internal static class ThemeResourceResolver
{
	internal static bool TryResolve(ResourceDictionary? dictionary, string appearance, string key, out object? value)
	{
		value = Resolve(dictionary, appearance, key);
		return value is not null;
	}

	/// <summary>
	/// Every non-null value <see cref="Resolve"/> (without appearance fallbacks) answers from
	/// <paramref name="dictionary"/> for <paramref name="appearance"/>, by key, in one pass: for a caller
	/// reading many keys, each dictionary is enumerated once instead of once per key.
	/// </summary>
	internal static Dictionary<string, object> ResolveAll(ResourceDictionary dictionary, string appearance)
	{
		var resolved = new Dictionary<string, object>(StringComparer.Ordinal);
		foreach (var pair in Collect(dictionary, appearance))
		{
			if (pair.Value is { } value)
			{
				resolved.Add(pair.Key, value);
			}
		}
		return resolved;
	}

	// Resolve for every key at once. Earlier steps win (TryAdd), in Resolve's order: own entries (a null
	// one included: Resolve returns it), merged dictionaries last first (a null answer falls through to
	// the next, as in Resolve), then the appearance branch, else Default when that branch is absent.
	private static Dictionary<string, object?> Collect(ResourceDictionary dictionary, string appearance)
	{
		var declared = new Dictionary<string, object?>(StringComparer.Ordinal);
		foreach (var entry in dictionary)
		{
			if (entry.Key is string key)
			{
				declared.TryAdd(key, entry.Value);
			}
		}

		for (var i = dictionary.MergedDictionaries.Count - 1; i >= 0; i--)
		{
			foreach (var pair in Collect(dictionary.MergedDictionaries[i], appearance))
			{
				if (pair.Value is { })
				{
					declared.TryAdd(pair.Key, pair.Value);
				}
			}
		}

		ResourceDictionary? branch = null;
		if (dictionary.ThemeDictionaries.TryGetValue(appearance, out var themed) && themed is ResourceDictionary appearanceBranch)
		{
			branch = appearanceBranch;
		}
		else if (dictionary.ThemeDictionaries.TryGetValue("Default", out var fallback) && fallback is ResourceDictionary defaultBranch)
		{
			branch = defaultBranch;
		}

		if (branch is { })
		{
			foreach (var pair in Collect(branch, appearance))
			{
				declared.TryAdd(pair.Key, pair.Value);
			}
		}
		return declared;
	}

	/// <summary>
	/// Follows resource dictionary precedence: own entries, merged dictionaries
	/// in reverse order, then the selected appearance dictionary. Default is used
	/// only when the requested appearance dictionary is absent, not when a key
	/// is absent from an existing appearance dictionary.
	/// </summary>
	internal static object? Resolve(ResourceDictionary? dictionary, string appearance, string key, string[]? appearanceFallbacks = null)
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
			if (Resolve(dictionary.MergedDictionaries[i], appearance, key, appearanceFallbacks) is { } mergedValue)
			{
				return mergedValue;
			}
		}

		// Shared brushes additionally preserve their explicit HighContrast → Dark → Default
		// palette policy. Select the first existing branch, never fill missing keys from siblings.
		if (appearanceFallbacks is not null)
		{
			foreach (var candidate in appearanceFallbacks)
			{
				if (dictionary.ThemeDictionaries.TryGetValue(candidate, out var selected)
					&& selected is ResourceDictionary selectedBranch)
				{
					return Resolve(selectedBranch, appearance, key, appearanceFallbacks);
				}
			}
			return null;
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
