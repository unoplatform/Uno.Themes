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
	/// Populates the Light/Default branches of <paramref name="palette"/> from the platform accent, unless it
	/// is still <paramref name="shades"/> (the accent the palette was last built from, updated on success). A
	/// constructor override is a separate layer above it (see <c>BaseTheme</c>), so nothing of the
	/// consumer's is written here.
	/// </summary>
	/// <returns><c>true</c> when the palette was (re)built; <c>false</c> when the accent is unchanged or the
	/// platform accent or the neutrals are unavailable.</returns>
	internal static bool TryPopulate(ResourceDictionary palette, ref FluentAccentPalette.AccentShades? shades)
	{
		try
		{
			if (FluentAccentPalette.GetPlatformShades() is not { } accent)
			{
				FluentDiagnostics.LogWarning(
					"FluentTheme could not resolve the Fluent accent; semantic colors keep the shared defaults. Ensure XamlControlsResources is merged before FluentTheme.");
				return false;
			}

			if (accent == shades)
			{
				return false;
			}

			var neutrals = _neutrals ??= LoadNeutrals();
			palette.ThemeDictionaries["Light"] = BuildBranch(isLight: true, accent, neutrals.Light);
			palette.ThemeDictionaries["Default"] = BuildBranch(isLight: false, accent, neutrals.Dark);
			shades = accent;
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
