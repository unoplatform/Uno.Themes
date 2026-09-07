#nullable enable

using System;
using Uno.Themes;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Fluent;

public partial class FluentTheme
{
	private readonly ResourceDictionary _liveAccentResources = new();
	private readonly ResourceDictionary _liveLightweightResources = new();

	internal override void PrepareThemeResources()
	{
		// Read the platform without our previous reverse-accent mapping shadowing it.
		// Restore the layer before BaseTheme builds its replacement transaction.
		var index = MergedDictionaries.IndexOf(_liveAccentResources);
		if (index >= 0)
		{
			MergedDictionaries.RemoveAt(index);
		}
		try
		{
			if (_palette is { })
			{
				FluentColorPalette.TryPopulate(_palette);
			}
		}
		finally
		{
			if (index >= 0)
			{
				MergedDictionaries.Insert(index, _liveAccentResources);
			}
		}
	}

	private void AddFontRootOverrides()
	{
		if (DefaultFontFamily is not null || ResolvedFontOverride is not { } fontOverride)
		{
			return;
		}

		var dictionary = new ResourceDictionary();
		foreach (var appearance in new[] { "Light", "Dark", "HighContrast" })
		{
			var branch = new ResourceDictionary();
			if (FluentResourceResolver.Resolve(fontOverride, appearance, "DefaultFontFamily") is FontFamily family)
			{
				foreach (var key in ThemesConstants.TypefaceScaleKeys)
				{
					branch[key] = family;
				}
				branch["ContentControlThemeFontFamily"] = family;
			}
			dictionary.ThemeDictionaries[appearance == "Dark" ? "Default" : appearance] = branch;
		}
		AddThemeDictionary(dictionary);
	}

	private void UpdateAccentResources(ResourceDictionary replacement)
	{
		var baseline = new ResourceDictionary();
		baseline.ThemeDictionaries["Light"] = FluentAccentPalette.BuildPlatformClosure(isLight: true) ?? new ResourceDictionary();
		baseline.ThemeDictionaries["Default"] = FluentAccentPalette.BuildPlatformClosure(isLight: false) ?? new ResourceDictionary();
		FluentResourceUpdater.Update(_liveAccentResources, replacement, baseline);
		AddThemeDictionary(_liveAccentResources);
	}

	private void UpdateLightweightResources(ResourceDictionary replacement)
	{
		var baseline = new ResourceDictionary();
		baseline.MergedDictionaries.Add(_lightweightDefaults);
		baseline.MergedDictionaries.Add(FluentLightweightBridge.Build(null, SeedColorMode.Fidelity, null, null, null));
		FluentResourceUpdater.Update(_liveLightweightResources, replacement, baseline);
		baseline.MergedDictionaries.Remove(_lightweightDefaults);
		AddThemeDictionary(_liveLightweightResources);
	}
}
