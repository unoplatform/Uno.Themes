#if HAS_UNO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using Uno.UI;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

[assembly: ElementMetadataUpdateHandler(typeof(Uno.Themes.BaseThemeHotReloadHandler))]

namespace Uno.Themes;

public abstract partial class BaseTheme
{
	internal void RefreshForHotReload() => UpdateSource();
}

/// <summary>
/// Hot-reload bridge for <see cref="BaseTheme"/>.
///
/// Uno's built-in HR refresh only walks <see cref="Application"/>.Resources.MergedDictionaries
/// one level deep, so nested merged dictionaries inside a <see cref="BaseTheme"/> instance
/// (typography, colors, palette, etc.) never get updated when their source XAML is edited.
///
/// This handler runs once per HR cycle, collects the Source URIs of every updated
/// <see cref="IXamlResourceDictionaryProvider"/>, and if any of those sources is reachable
/// from a live <see cref="BaseTheme"/>'s nested tree, calls <see cref="BaseTheme.RefreshForHotReload"/>
/// to rebuild from the (already-updated) ResourceResolver factories.
/// </summary>
internal static class BaseThemeHotReloadHandler
{
	private const string MsResourcePrefix = "ms-resource:///Files/";
	private const string MsAppxPrefix = "ms-appx:///";

	private static readonly object _gate = new();
	private static readonly List<WeakReference<BaseTheme>> _live = new();

	internal static void Register(BaseTheme theme)
	{
		Console.WriteLine($"[STEVE] BaseThemeHotReloadHandler: Registering theme instance {theme?.GetHashCode():X8}");
		if (theme is null)
		{
			return;
		}

		lock (_gate)
		{
			// Opportunistic compaction so the list doesn't grow unbounded across reloads
			for (int i = _live.Count - 1; i >= 0; i--)
			{
				if (!_live[i].TryGetTarget(out var existing))
				{
					_live.RemoveAt(i);
					continue;
				}

				if (ReferenceEquals(existing, theme))
				{
					return;
				}
			}

			_live.Add(new WeakReference<BaseTheme>(theme));
		}
	}

	/// <summary>
	/// Invoked by the Uno HR agent (<see cref="ElementMetadataUpdateHandlerAttribute"/>)
	/// once per <c>UpdateApplication</c> cycle, on the UI thread, after the agent's own
	/// dictionary-refresh pass and after the per-element visual-tree update.
	/// </summary>
	public static void AfterVisualTreeUpdate(Type[] updatedTypes)
	{
		Console.WriteLine($"[STEVE] BaseThemeHotReloadHandler: AfterVisualTreeUpdate with {updatedTypes?.Length ?? 0} updated types");
		if (updatedTypes is null || updatedTypes.Length == 0)
		{
			return;
		}

		var updatedSources = CollectUpdatedDictionaryKeys(updatedTypes);
		if (updatedSources.Count == 0)
		{
			return;
		}

		BaseTheme[] snapshot;
		lock (_gate)
		{
			snapshot = _live
				.Select(wr => wr.TryGetTarget(out var t) ? t : null)
				.Where(t => t is not null)
				.ToArray()!;
		}

		foreach (var theme in snapshot)
		{
			if (ContainsAnySource(theme, updatedSources))
			{
				theme.RefreshForHotReload();
			}
		}
	}

	private static HashSet<string> CollectUpdatedDictionaryKeys(Type[] updatedTypes)
	{
		var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		foreach (var type in updatedTypes)
		{
			if (type is null)
			{
				continue;
			}

			try
			{
				if (!typeof(IXamlResourceDictionaryProvider).IsAssignableFrom(type))
				{
					continue;
				}

				var instanceProp = type.GetProperty(
					"Instance",
					BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

				if (instanceProp?.GetValue(null) is not IXamlResourceDictionaryProvider provider)
				{
					continue;
				}

				if (provider.GetResourceDictionary() is { Source: { } src } &&
					CanonicalizeSource(src) is { Length: > 0 } key)
				{
					keys.Add(key);
				}
			}
			catch
			{
				// Best-effort: a type whose Instance property throws is simply skipped.
				// HR ought to be robust against partially-initialised generated types.
			}
		}

		return keys;
	}

	private static bool ContainsAnySource(ResourceDictionary root, HashSet<string> targets)
	{
		if (root is null)
		{
			return false;
		}

		foreach (var merged in root.MergedDictionaries)
		{
			if (merged.Source is { } s && targets.Contains(CanonicalizeSource(s)))
			{
				return true;
			}

			if (ContainsAnySource(merged, targets))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Normalises a ResourceDictionary <c>Source</c> URI to a comparable form.
	/// Uno's generated code emits dictionary providers with <c>ms-resource:///Files/...</c>
	/// while consumer XAML and BaseTheme itself use <c>ms-appx:///...</c>. Both schemes are
	/// registered for the same factory, so equating them by relative path is the right unit
	/// of comparison.
	/// </summary>
	private static string CanonicalizeSource(Uri source)
	{
		if (source is null)
		{
			return string.Empty;
		}

		var s = source.OriginalString;

		if (s.StartsWith(MsResourcePrefix, StringComparison.OrdinalIgnoreCase))
		{
			s = s.Substring(MsResourcePrefix.Length);
		}
		else if (s.StartsWith(MsAppxPrefix, StringComparison.OrdinalIgnoreCase))
		{
			s = s.Substring(MsAppxPrefix.Length);
		}

		return s.Replace('\\', '/');
	}
}
#endif
