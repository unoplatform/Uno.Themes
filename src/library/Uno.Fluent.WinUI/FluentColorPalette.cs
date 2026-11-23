#nullable enable

using System;
using System.Collections.Generic;
using Uno.Themes;
using Windows.UI;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Fluent;

/// <summary>
/// The Fluent semantic color palette: the accent-derived roles from the platform accent shades, the
/// neutrals from <c>ColorPalette.xaml</c> (captured Fluent token values, drift-guarded by
/// <c>Given_FluentColorPalette</c>). Built in code because per-branch XAML color aliases resolve
/// against the ambient theme on Uno, not the branch they sit in.
/// </summary>
/// <remarks>
/// All-or-nothing and never throws: when the accent is unavailable the palette is left untouched and
/// the shared defaults apply, rather than mixing Fluent neutrals with a non-Fluent primary.
/// </remarks>
internal static class FluentColorPalette
{
	// Plain values, so any UI thread can read them: a ResourceDictionary is thread-affine on WinUI.
	private static Neutrals? _neutrals;

	/// <summary>
	/// Populates the Light/Default branches of <paramref name="palette"/>, with <paramref name="colorOverride"/>'s
	/// colors written into them.
	/// </summary>
	/// <returns><c>false</c> when the platform accent or the neutrals are unavailable.</returns>
	internal static bool TryPopulate(ResourceDictionary palette, ResourceDictionary? colorOverride)
	{
		try
		{
			if (FluentAccentPalette.GetPlatformShades() is not { } accent)
			{
				FluentDiagnostics.LogWarning(
					"FluentTheme could not resolve the Fluent accent; semantic colors keep the shared defaults. Ensure XamlControlsResources is merged before FluentTheme.");
				return false;
			}

			var neutrals = _neutrals ??= LoadNeutrals();
			var light = BuildBranch(isLight: true, accent, neutrals.Light);
			var dark = BuildBranch(isLight: false, accent, neutrals.Dark);
			if (colorOverride is { })
			{
				// The brush sweep reads a layer's theme branches before its merged dictionaries, so the
				// override merged into the palette alone would reach the *Color keys but not the brushes.
				ApplyOverride(light, colorOverride, isLight: true);
				ApplyOverride(dark, colorOverride, isLight: false);
			}
			palette.ThemeDictionaries["Light"] = light;
			palette.ThemeDictionaries["Default"] = dark;
			return true;
		}
		catch (Exception e)
		{
			// Runs from the theme constructor (App.xaml parsing): degrade to the shared defaults.
			FluentDiagnostics.LogWarning("FluentTheme could not build its color palette; semantic colors keep the shared defaults.", e);
			return false;
		}
	}

	private static Neutrals LoadNeutrals()
	{
		var source = new ResourceDictionary { Source = new Uri(FluentConstants.ResourcePaths.ColorPalette) };
		return new Neutrals(Read("Light"), Read("Default"));

		KeyValuePair<object, object>[] Read(string branchKey)
		{
			var pairs = new List<KeyValuePair<object, object>>();
			if (source.ThemeDictionaries.TryGetValue(branchKey, out var declared) && declared is ResourceDictionary branch)
			{
				foreach (var pair in branch)
				{
					pairs.Add(pair);
				}
			}
			return pairs.ToArray();
		}
	}

	/// <summary>
	/// Writes the override's colors for one appearance into <paramref name="branch"/>, with the brush sweep's
	/// precedence: the appearance's theme dictionaries (e.g. Dark, then Default) over theme-agnostic entries.
	/// </summary>
	private static void ApplyOverride(ResourceDictionary branch, ResourceDictionary colorOverride, bool isLight)
	{
		foreach (var pair in colorOverride)
		{
			if (pair.Value is Color)
			{
				branch[pair.Key] = pair.Value;
			}
		}

		var themeKeys = ThemesConstants.BrushThemeSources[isLight ? 0 : 1].ColorThemes;
		for (var i = themeKeys.Length - 1; i >= 0; i--)
		{
			if (colorOverride.ThemeDictionaries.TryGetValue(themeKeys[i], out var themed) && themed is ResourceDictionary themedBranch)
			{
				foreach (var pair in themedBranch)
				{
					if (pair.Value is Color)
					{
						branch[pair.Key] = pair.Value;
					}
				}
			}
		}
	}

	private static ResourceDictionary BuildBranch(bool isLight, FluentAccentPalette.AccentShades accent, KeyValuePair<object, object>[] neutrals)
	{
		// Flat copy of the neutrals: the shape SafeMerge and the override precedence expect.
		var branch = new ResourceDictionary();
		foreach (var pair in neutrals)
		{
			branch[pair.Key] = pair.Value;
		}

		// Primary is the accent in the light theme and Light2 (Fluent's accent fill) in the dark theme;
		// the light accent fill is the darker Dark1. Secondary and Tertiary take darker / lighter shades
		// of the same accent, not neutrals, so they stay branded.
		branch[SemanticColorKeys.Primary] = isLight ? accent.Accent : accent.Light2;
		branch[SemanticColorKeys.PrimaryContainer] = isLight ? accent.Light2 : accent.Dark1;
		branch[SemanticColorKeys.OnPrimaryContainer] = isLight ? accent.Dark2 : accent.Light3;
		branch[SemanticColorKeys.PrimaryInverse] = isLight ? accent.Light2 : accent.Accent;
		branch[SemanticColorKeys.PrimaryVariantDark] = accent.Dark1;
		branch[SemanticColorKeys.PrimaryVariantLight] = accent.Light1;

		branch[SemanticColorKeys.Secondary] = isLight ? accent.Dark1 : accent.Light1;
		branch[SemanticColorKeys.SecondaryContainer] = isLight ? accent.Light3 : accent.Dark2;
		branch[SemanticColorKeys.OnSecondaryContainer] = isLight ? accent.Dark3 : accent.Light3;
		branch[SemanticColorKeys.SecondaryVariantDark] = accent.Dark2;
		branch[SemanticColorKeys.SecondaryVariantLight] = isLight ? accent.Accent : accent.Light2;

		branch[SemanticColorKeys.Tertiary] = isLight ? accent.Dark2 : accent.Light3;
		branch[SemanticColorKeys.TertiaryContainer] = isLight ? accent.Light3 : accent.Dark3;
		branch[SemanticColorKeys.OnTertiaryContainer] = isLight ? accent.Dark3 : accent.Light2;

		branch[SemanticColorKeys.SurfaceTint] = isLight ? accent.Accent : accent.Light2;

		return branch;
	}

	private sealed record Neutrals(KeyValuePair<object, object>[] Light, KeyValuePair<object, object>[] Dark);
}
