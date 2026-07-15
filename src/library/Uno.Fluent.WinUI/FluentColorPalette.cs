#nullable enable

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
/// Builds the Fluent semantic color palette in code ("mechanism C", spec 05 D6):
/// maps the Fluent design tokens onto the semantic role keys, writing literal
/// <see cref="Color"/> values per theme branch.
/// </summary>
/// <remarks>
/// <para>
/// Two token sources are combined. The theme-invariant accent shades are resolved
/// LIVE from the ambient resources (so Windows tracks the user's real system
/// accent). The theme-varying neutrals are per-branch constants captured from the
/// running platform (spike S2, 2026-07-14 — identical to the documented WinUI 3
/// token values): Uno does not expose the WinUI 3 token set per theme branch
/// through any public dictionary API (the tokens live in an internal system
/// registry and ambient lookups only ever resolve the active theme's value), and
/// per-theme-branch XAML <c>&lt;StaticResource&gt;</c> aliases resolve against the
/// ambient theme (see specs/lessons.md and <c>Given_FluentAliasResolution</c>).
/// <c>Given_FluentColorPalette</c> asserts the ambient branch against the live
/// platform values on every run, failing fast if an Uno.UI update changes a token.
/// </para>
/// <para>
/// Population never throws: when the accent tokens are unreachable (e.g.
/// XamlControlsResources not merged) the palette is left untouched and the shared
/// (M3) defaults apply — graceful degradation from theme-initialization paths.
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

	private readonly record struct BranchValues(Color Light, Color Dark);

	// Theme-varying Fluent neutrals (token name -> per-branch value). Values are
	// the spike-S2 platform capture; Given_FluentColorPalette guards them against
	// the live ambient values.
	private static readonly Dictionary<string, BranchValues> _neutralTokens = new()
	{
		["TextFillColorPrimary"] = new(Rgba(0xE4000000), Rgba(0xFFFFFFFF)),
		["TextFillColorSecondary"] = new(Rgba(0x9E000000), Rgba(0xC5FFFFFF)),
		["TextFillColorInverse"] = new(Rgba(0xFFFFFFFF), Rgba(0xE4000000)),
		["TextOnAccentFillColorPrimary"] = new(Rgba(0xFFFFFFFF), Rgba(0xFF000000)),
		["SystemFillColorCritical"] = new(Rgba(0xFFC42B1C), Rgba(0xFFFF99A4)),
		["SystemFillColorCriticalBackground"] = new(Rgba(0xFFFDE7E9), Rgba(0xFF442726)),
		["SolidBackgroundFillColorBase"] = new(Rgba(0xFFF3F3F3), Rgba(0xFF202020)),
		["SolidBackgroundFillColorSecondary"] = new(Rgba(0xFFEEEEEE), Rgba(0xFF1C1C1C)),
		["SolidBackgroundFillColorTertiary"] = new(Rgba(0xFFF9F9F9), Rgba(0xFF282828)),
		["ControlStrongStrokeColorDefault"] = new(Rgba(0x72000000), Rgba(0x8BFFFFFF)),
		["DividerStrokeColorDefault"] = new(Rgba(0x0F000000), Rgba(0x15FFFFFF)),
	};

	/// <summary>
	/// Populates <paramref name="palette"/>'s Light/Default theme dictionaries with
	/// the semantic role colors mapped from the Fluent tokens (spec 05 §6.3).
	/// </summary>
	/// <returns>
	/// <c>false</c> when the accent tokens are unreachable; <paramref name="palette"/>
	/// is then left untouched so the shared (M3) palette defaults apply.
	/// </returns>
	internal static bool TryPopulate(ResourceDictionary palette)
	{
		if (Application.Current?.Resources is not { } resources)
		{
			LogTokensUnavailable();
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
			LogTokensUnavailable();
			return false;
		}

		palette.ThemeDictionaries[LightBranchKey] = BuildBranch(isLight: true, accent);
		palette.ThemeDictionaries[DefaultBranchKey] = BuildBranch(isLight: false, accent);
		return true;
	}

	private static ResourceDictionary BuildBranch(bool isLight, IReadOnlyDictionary<string, Color> accent)
	{
		var branch = new ResourceDictionary();

		// Skipped when an accent shade is missing on a platform: the role keeps
		// the shared (M3) default for this branch.
		void FromAccent(string role, string token)
		{
			if (accent.TryGetValue(token, out var color))
			{
				branch[role] = color;
			}
		}

		void FromNeutral(string role, string token)
			=> branch[role] = isLight ? _neutralTokens[token].Light : _neutralTokens[token].Dark;

		// SurfaceInverse carries the OTHER branch's value.
		void FromOppositeNeutral(string role, string token)
			=> branch[role] = isLight ? _neutralTokens[token].Dark : _neutralTokens[token].Light;

		// Primary — the accent itself; Fluent's dark-theme accent fill is
		// SystemAccentColorLight2, not the base accent (verified against
		// AccentFillColorDefaultBrush — spike S2). Secondary/Tertiary map to
		// darker/lighter shades of the single Fluent accent (not to neutrals) so
		// Secondary/Tertiary-styled UI stays visibly branded and hierarchical
		// (spec 05 §6.3 judgment calls).
		FromAccent("PrimaryColor", isLight ? "SystemAccentColor" : "SystemAccentColorLight2");
		FromNeutral("OnPrimaryColor", "TextOnAccentFillColorPrimary");
		FromAccent("PrimaryContainerColor", isLight ? "SystemAccentColorLight2" : "SystemAccentColorDark1");
		FromAccent("OnPrimaryContainerColor", isLight ? "SystemAccentColorDark2" : "SystemAccentColorLight3");
		FromAccent("PrimaryInverseColor", isLight ? "SystemAccentColorLight2" : "SystemAccentColor");
		FromAccent("PrimaryVariantDarkColor", "SystemAccentColorDark1");
		FromAccent("PrimaryVariantLightColor", "SystemAccentColorLight1");

		FromAccent("SecondaryColor", isLight ? "SystemAccentColorDark1" : "SystemAccentColorLight1");
		FromNeutral("OnSecondaryColor", "TextOnAccentFillColorPrimary");
		FromAccent("SecondaryContainerColor", isLight ? "SystemAccentColorLight3" : "SystemAccentColorDark2");
		FromAccent("OnSecondaryContainerColor", isLight ? "SystemAccentColorDark3" : "SystemAccentColorLight3");
		FromAccent("SecondaryVariantDarkColor", "SystemAccentColorDark2");
		FromAccent("SecondaryVariantLightColor", isLight ? "SystemAccentColor" : "SystemAccentColorLight2");

		FromAccent("TertiaryColor", isLight ? "SystemAccentColorDark2" : "SystemAccentColorLight3");
		FromNeutral("OnTertiaryColor", "TextOnAccentFillColorPrimary");
		FromAccent("TertiaryContainerColor", isLight ? "SystemAccentColorLight3" : "SystemAccentColorDark3");
		FromAccent("OnTertiaryContainerColor", isLight ? "SystemAccentColorDark3" : "SystemAccentColorLight2");

		// OnErrorContainer reuses SystemFillColorCritical: Fluent defines no
		// "on critical container" token; critical-on-critical-background is the
		// WinUI InfoBar pattern.
		FromNeutral("ErrorColor", "SystemFillColorCritical");
		FromNeutral("OnErrorColor", "TextOnAccentFillColorPrimary");
		FromNeutral("ErrorContainerColor", "SystemFillColorCriticalBackground");
		FromNeutral("OnErrorContainerColor", "SystemFillColorCritical");

		// Surface* maps to the solid background stack (no Mica/Acrylic — spec 05 N3).
		FromNeutral("BackgroundColor", "SolidBackgroundFillColorBase");
		FromNeutral("OnBackgroundColor", "TextFillColorPrimary");
		FromNeutral("SurfaceColor", "SolidBackgroundFillColorTertiary");
		FromNeutral("OnSurfaceColor", "TextFillColorPrimary");
		FromNeutral("SurfaceVariantColor", "SolidBackgroundFillColorSecondary");
		FromNeutral("OnSurfaceVariantColor", "TextFillColorSecondary");
		FromOppositeNeutral("SurfaceInverseColor", "SolidBackgroundFillColorBase");
		FromNeutral("OnSurfaceInverseColor", "TextFillColorInverse");
		FromAccent("SurfaceTintColor", isLight ? "SystemAccentColor" : "SystemAccentColorLight2");

		FromNeutral("OutlineColor", "ControlStrongStrokeColorDefault");
		FromNeutral("OutlineVariantColor", "DividerStrokeColorDefault");

		// ShadowColor keeps the shared default: Fluent has no shadow color token.

		return branch;
	}

	private static Color Rgba(uint argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));

	private static void LogTokensUnavailable()
	{
#if HAS_UNO
		var logger = global::Uno.Extensions.LogExtensionPoint.Log(typeof(FluentTheme));
		if (logger.IsEnabled(global::Microsoft.Extensions.Logging.LogLevel.Warning))
		{
			global::Microsoft.Extensions.Logging.LoggerExtensions.LogWarning(
				logger,
				"FluentTheme could not resolve the Fluent design tokens; semantic colors keep the shared defaults. Ensure XamlControlsResources is merged before FluentTheme.");
		}
#else
		global::System.Diagnostics.Debug.WriteLine(
			"FluentTheme could not resolve the Fluent design tokens; semantic colors keep the shared defaults. Ensure XamlControlsResources is merged before FluentTheme.");
#endif
	}
}
