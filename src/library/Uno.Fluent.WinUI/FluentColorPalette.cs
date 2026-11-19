#nullable enable

using System;
using Uno.Themes;

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
	// Loaded once and only copied from, never attached, so sharing it across themes is safe.
	private static ResourceDictionary? _neutralSource;

	/// <summary>Populates the Light/Default branches of <paramref name="palette"/>.</summary>
	/// <returns><c>false</c> when the platform accent or the neutrals are unavailable.</returns>
	internal static bool TryPopulate(ResourceDictionary palette)
	{
		if (FluentAccentPalette.GetPlatformShades() is not { } accent)
		{
			FluentDiagnostics.LogWarning(
				"FluentTheme could not resolve the Fluent accent; semantic colors keep the shared defaults. Ensure XamlControlsResources is merged before FluentTheme.");
			return false;
		}

		if (GetNeutralSource() is not { } neutrals)
		{
			return false;
		}

		palette.ThemeDictionaries["Light"] = BuildBranch("Light", isLight: true, accent, neutrals);
		palette.ThemeDictionaries["Default"] = BuildBranch("Default", isLight: false, accent, neutrals);
		return true;
	}

	private static ResourceDictionary? GetNeutralSource()
	{
		if (_neutralSource is { } cached)
		{
			return cached;
		}

		try
		{
			return _neutralSource = new ResourceDictionary { Source = new Uri(FluentConstants.ResourcePaths.ColorPalette) };
		}
		catch (Exception e)
		{
			// Theme initialization never throws: degrade to the shared defaults.
			FluentDiagnostics.LogWarning($"FluentTheme could not load its neutral color palette; semantic colors keep the shared defaults. {e.Message}");
			return null;
		}
	}

	private static ResourceDictionary BuildBranch(string branchKey, bool isLight, FluentAccentPalette.AccentShades accent, ResourceDictionary neutrals)
	{
		// Flat copy of the neutrals: the shape SafeMerge and the override precedence expect.
		var branch = new ResourceDictionary();
		if (neutrals.ThemeDictionaries.TryGetValue(branchKey, out var declared) && declared is ResourceDictionary declaredBranch)
		{
			foreach (var pair in declaredBranch)
			{
				branch[pair.Key] = pair.Value;
			}
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
}
