#nullable enable

using System;
using System.Collections.Generic;

#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
#endif

namespace Uno.Fluent;

/// <summary>
/// Builds the Fluent semantic color palette: the accent-derived roles are written
/// in code per theme branch ("mechanism C", spec 05 D6) from the live
/// <c>XamlControlsResources</c> accent shades, and the static per-branch neutrals
/// are copied verbatim from the declarative <c>ColorPalette.xaml</c>.
/// </summary>
/// <remarks>
/// <para>
/// Only the accent-derived roles are code-built: per-theme-branch XAML
/// <c>&lt;StaticResource&gt;</c> color aliases resolve against the ambient theme
/// on Uno (see specs/lessons.md and <c>Given_FluentAliasResolution</c>), and
/// resolving the shades live is also what lets Windows track the user's real
/// system accent. Every static per-branch value lives declaratively in
/// <c>Styles/Application/ColorPalette.xaml</c> (single source of truth; platform
/// captures from spike S2). <c>Given_FluentColorPalette</c> asserts the ambient
/// branch against the live platform values on every run, failing fast if an
/// Uno.UI update changes a token.
/// </para>
/// <para>
/// Population never throws and is all-or-nothing: when the accent tokens are
/// unreachable (e.g. XamlControlsResources not merged) the palette is left
/// untouched — neutrals included — and the shared (M3) defaults apply; applying
/// Fluent neutrals around a non-Fluent primary (the shared M3 purple) would
/// produce an incoherent mixed palette. Graceful degradation from
/// theme-initialization paths.
/// </para>
/// </remarks>
internal static class FluentColorPalette
{
	private const string LightBranchKey = "Light";
	private const string DefaultBranchKey = "Default";

	// Theme-invariant accent shades, resolved from the ambient resources.
	private static readonly string[] _accentTokens =
	{
		"SystemAccentColor",
		"SystemAccentColorLight1",
		"SystemAccentColorLight2",
		"SystemAccentColorLight3",
		"SystemAccentColorDark1",
		"SystemAccentColorDark2",
		"SystemAccentColorDark3",
	};

	// The declarative neutral values (ColorPalette.xaml), loaded once per process.
	// Never attached to a visual tree — only copied from — so sharing the instance
	// across theme instances is safe.
	private static ResourceDictionary? _neutralSource;

	/// <summary>
	/// Populates <paramref name="palette"/>'s Light/Default theme dictionaries with
	/// the semantic role colors mapped from the Fluent tokens (spec 05 §6.3).
	/// </summary>
	/// <returns>
	/// <c>false</c> when the accent tokens (or the neutral dictionary) are
	/// unreachable; <paramref name="palette"/> is then left untouched so the shared
	/// (M3) palette defaults apply.
	/// </returns>
	internal static bool TryPopulate(ResourceDictionary palette)
	{
		if (Application.Current?.Resources is not { } resources)
		{
			LogPaletteUnavailable();
			return false;
		}

		var accent = new Dictionary<string, Color>(_accentTokens.Length);
		foreach (var token in _accentTokens)
		{
			if (resources.TryGetValue(token, out var value) && value is Color color)
			{
				accent[token] = color;
			}
		}

		// All-or-nothing: applying Fluent neutrals around a non-Fluent primary
		// (the shared M3 purple) would produce an incoherent mixed palette.
		if (!accent.ContainsKey("SystemAccentColor"))
		{
			LogPaletteUnavailable();
			return false;
		}

		if (GetNeutralSource() is not { } neutrals)
		{
			return false;
		}

		palette.ThemeDictionaries[LightBranchKey] = BuildBranch(LightBranchKey, isLight: true, accent, neutrals);
		palette.ThemeDictionaries[DefaultBranchKey] = BuildBranch(DefaultBranchKey, isLight: false, accent, neutrals);
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
			// The packaged dictionary failed to load — degrade to the shared (M3)
			// defaults rather than throwing from theme initialization.
			FluentDiagnostics.LogWarning(
				$"FluentTheme could not load its neutral color palette (ColorPalette.xaml); semantic colors keep the shared defaults. {e.Message}");
			return null;
		}
	}

	private static ResourceDictionary BuildBranch(string branchKey, bool isLight, IReadOnlyDictionary<string, Color> accent, ResourceDictionary neutrals)
	{
		var branch = new ResourceDictionary();

		// The declarative neutrals first, copied verbatim so the branch carries
		// the full role set in a single flat dictionary (the shape SafeMerge and
		// the override-precedence contract are proven against).
		if (neutrals.ThemeDictionaries.TryGetValue(branchKey, out var declared) && declared is ResourceDictionary declaredBranch)
		{
			foreach (var pair in declaredBranch)
			{
				branch[pair.Key] = pair.Value;
			}
		}

		// Then the accent-derived roles. Skipped when an accent shade is missing
		// on a platform: the role keeps the shared (M3) default for this branch.
		void FromAccent(string role, string token)
		{
			if (accent.TryGetValue(token, out var color))
			{
				branch[role] = color;
			}
		}

		// Primary — the accent itself; Fluent's dark-theme accent fill is
		// SystemAccentColorLight2, not the base accent (verified against
		// AccentFillColorDefaultBrush — spike S2). Secondary/Tertiary map to
		// darker/lighter shades of the single Fluent accent (not to neutrals) so
		// Secondary/Tertiary-styled UI stays visibly branded and hierarchical
		// (spec 05 §6.3 judgment calls).
		FromAccent("PrimaryColor", isLight ? "SystemAccentColor" : "SystemAccentColorLight2");
		FromAccent("PrimaryContainerColor", isLight ? "SystemAccentColorLight2" : "SystemAccentColorDark1");
		FromAccent("OnPrimaryContainerColor", isLight ? "SystemAccentColorDark2" : "SystemAccentColorLight3");
		FromAccent("PrimaryInverseColor", isLight ? "SystemAccentColorLight2" : "SystemAccentColor");
		FromAccent("PrimaryVariantDarkColor", "SystemAccentColorDark1");
		FromAccent("PrimaryVariantLightColor", "SystemAccentColorLight1");

		FromAccent("SecondaryColor", isLight ? "SystemAccentColorDark1" : "SystemAccentColorLight1");
		FromAccent("SecondaryContainerColor", isLight ? "SystemAccentColorLight3" : "SystemAccentColorDark2");
		FromAccent("OnSecondaryContainerColor", isLight ? "SystemAccentColorDark3" : "SystemAccentColorLight3");
		FromAccent("SecondaryVariantDarkColor", "SystemAccentColorDark2");
		FromAccent("SecondaryVariantLightColor", isLight ? "SystemAccentColor" : "SystemAccentColorLight2");

		FromAccent("TertiaryColor", isLight ? "SystemAccentColorDark2" : "SystemAccentColorLight3");
		FromAccent("TertiaryContainerColor", isLight ? "SystemAccentColorLight3" : "SystemAccentColorDark3");
		FromAccent("OnTertiaryContainerColor", isLight ? "SystemAccentColorDark3" : "SystemAccentColorLight2");

		FromAccent("SurfaceTintColor", isLight ? "SystemAccentColor" : "SystemAccentColorLight2");

		return branch;
	}

	private static void LogPaletteUnavailable()
		=> FluentDiagnostics.LogWarning(
			"FluentTheme could not resolve the Fluent design tokens; semantic colors keep the shared defaults. Ensure XamlControlsResources is merged before FluentTheme.");
}
