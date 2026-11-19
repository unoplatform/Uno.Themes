#nullable enable

using System;
using System.Collections.Generic;
using Uno.Themes;
using Uno.Themes.ColorGeneration;
using Uno.Themes.ColorGeneration.Hct;
using Uno.Themes.Helpers;
using Windows.UI.ViewManagement;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Fluent;

/// <summary>
/// The Fluent accent resources: the <c>SystemAccentColor*</c> shades and the accent-derived tokens
/// (<c>AccentFillColor*</c>, <c>AccentTextFillColor*</c>, <c>TextOnAccentFillColor*</c>) the built-in
/// templates read, derived from the effective primary so built-in controls follow a seed or a
/// <c>PrimaryColor</c> override like the semantic brushes do.
/// </summary>
/// <remarks>
/// The closure is written alongside the shades because WinUI resolves its accent tokens from the
/// shades once, at load; on Uno the shades alone would cascade.
/// </remarks>
internal static class FluentAccentPalette
{
	// M3's primary tone, which TonalSpot pins the light Primary to; Fidelity keeps the seed's own tone.
	private const int AccentTone = 40;

	// The dark-theme Primary tone. Light2 (Fluent's dark-theme accent fill) sits here, so the built-in
	// accent and the semantic PrimaryColor agree in the dark theme.
	private const double DarkThemePrimaryTone = 80;

	// Same chroma floor SeedColorPaletteGenerator applies under TonalSpot.
	private const double TonalSpotMinimumChroma = 48;

	private static readonly Color White = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
	private static readonly Color Black = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);

	// Fluent's on-accent secondary text: 70% white / 50% black.
	private static readonly Color WhiteSecondary = Color.FromArgb(0xB3, 0xFF, 0xFF, 0xFF);
	private static readonly Color BlackSecondary = Color.FromArgb(0x80, 0x00, 0x00, 0x00);

	private static readonly string[] ShadeKeys =
	{
		"SystemAccentColor",
		"SystemAccentColorLight1",
		"SystemAccentColorLight2",
		"SystemAccentColorLight3",
		"SystemAccentColorDark1",
		"SystemAccentColorDark2",
		"SystemAccentColorDark3",
	};

	private static readonly UIColorType[] ShadeColorTypes =
	{
		UIColorType.Accent,
		UIColorType.AccentLight1,
		UIColorType.AccentLight2,
		UIColorType.AccentLight3,
		UIColorType.AccentDark1,
		UIColorType.AccentDark2,
		UIColorType.AccentDark3,
	};

	// UWP-era accent brushes still referenced by some templates; they carry the base accent.
	private static readonly string[] LegacyAccentBrushKeys =
	{
		"SystemControlBackgroundAccentBrush",
		"SystemControlForegroundAccentBrush",
		"SystemControlHighlightAccentBrush",
		"SystemControlHighlightAltAccentBrush",
		"SystemControlHyperlinkTextBrush",
		"SystemColorControlAccentBrush",
	};

	/// <summary>
	/// An accent and its Fluent shade ramp. Shades are relative to the accent's tone: the dark shades
	/// sit 3/4, 1/2 and 1/4 of the way to black (so they stay darker than any accent), Light2 at the
	/// dark-theme Primary tone, Light1 between the accent and it, Light3 between it and white.
	/// </summary>
	internal readonly record struct AccentShades(Color Accent, Color Light1, Color Light2, Color Light3, Color Dark1, Color Dark2, Color Dark3)
	{
		/// <summary>
		/// The shades of a seed, generated like the light <c>PrimaryColor</c>: Fidelity keeps the
		/// (opaque) seed verbatim, TonalSpot takes tone 40 of the chroma-boosted palette.
		/// </summary>
		internal static AccentShades FromSeed(Color seed, SeedColorMode mode)
		{
			var hct = HctColor.FromArgb(ToArgb(seed));
			var palette = PaletteOf(hct, mode);

			return mode == SeedColorMode.TonalSpot
				? From(ToneColor(palette, AccentTone), AccentTone, palette)
				: From(Color.FromArgb(0xFF, seed.R, seed.G, seed.B), hct.Tone, palette);
		}

		/// <summary>The shades of a verbatim accent (a <c>PrimaryColor</c> override): its own chroma and tone.</summary>
		internal static AccentShades FromBasis(Color basis)
		{
			var hct = HctColor.FromArgb(ToArgb(basis));
			return From(basis, hct.Tone, PaletteOf(hct, SeedColorMode.Fidelity));
		}

		private static AccentShades From(Color accent, double accentTone, TonalPalette palette)
			=> new(
				accent,
				Light1: ToneColor(palette, (accentTone + DarkThemePrimaryTone) / 2),
				Light2: ToneColor(palette, DarkThemePrimaryTone),
				Light3: ToneColor(palette, (DarkThemePrimaryTone + 100) / 2),
				Dark1: ToneColor(palette, accentTone * 0.75),
				Dark2: ToneColor(palette, accentTone * 0.5),
				Dark3: ToneColor(palette, accentTone * 0.25));
	}

	/// <summary>
	/// The platform accent shades, read where FluentTheme's own accent layer cannot shadow them:
	/// the app's <c>XamlControlsResources</c>, else <see cref="UISettings"/> (the source of the system
	/// accent on WinUI, where the shades are framework resources). <c>null</c> when unavailable.
	/// </summary>
	internal static AccentShades? GetPlatformShades()
	{
		var colors = new Color[ShadeKeys.Length];
		if (FindXamlControlsResources(Application.Current?.Resources) is { } xcr && TryReadShades(xcr, colors))
		{
			return new AccentShades(colors[0], colors[1], colors[2], colors[3], colors[4], colors[5], colors[6]);
		}

		try
		{
			var settings = new UISettings();
			for (var i = 0; i < ShadeColorTypes.Length; i++)
			{
				colors[i] = settings.GetColorValue(ShadeColorTypes[i]);
			}
			return new AccentShades(colors[0], colors[1], colors[2], colors[3], colors[4], colors[5], colors[6]);
		}
		catch (Exception e)
		{
			FluentDiagnostics.LogWarning($"FluentTheme could not read the platform accent. {e.Message}");
			return null;
		}
	}

	/// <summary>
	/// The accent resources of one appearance, driven by the consumer override's <c>PrimaryBrush</c>
	/// (solid) or <c>PrimaryColor</c> — the accent verbatim — else by <paramref name="seed"/>; plus
	/// <c>OnPrimaryBrush</c> / <c>OnPrimaryColor</c> as the on-accent text. Keys the override defines
	/// explicitly are left out, so the consumer's value wins. <c>null</c> when nothing drives the accent.
	/// </summary>
	/// <returns>Colors and <see cref="SolidColorBrush"/> templates (color + opacity), by key.</returns>
	internal static Dictionary<string, object>? Build(bool isLight, Color? seed, SeedColorMode mode, ResourceDictionary? consumerOverride)
	{
		var appearance = isLight ? "Light" : "Dark";
		var primaryBrush = ThemeResourceResolver.Resolve(consumerOverride, appearance, "PrimaryBrush") as SolidColorBrush;
		var basis = primaryBrush?.Color ?? ThemeResourceResolver.Resolve(consumerOverride, appearance, SemanticColorKeys.Primary) as Color?;

		var onPrimary = ThemeResourceResolver.Resolve(consumerOverride, appearance, "OnPrimaryBrush") as SolidColorBrush;
		if (onPrimary is null && ThemeResourceResolver.Resolve(consumerOverride, appearance, SemanticColorKeys.OnPrimary) is Color onPrimaryColor)
		{
			onPrimary = new SolidColorBrush(onPrimaryColor);
		}

		AccentShades? shades = basis is { } b ? AccentShades.FromBasis(b)
			: seed is { } s ? AccentShades.FromSeed(s, mode)
			: null;
		if (shades is null && onPrimary is null)
		{
			return null;
		}

		var entries = new Dictionary<string, object>(StringComparer.Ordinal);
		if (shades is { } accent)
		{
			// A PrimaryColor override is the color the consumer sees on the fill; a seed follows
			// Fluent's own usage (light fill Dark1, dark fill Light2).
			var fill = basis is { } ? accent.Accent : isLight ? accent.Dark1 : accent.Light2;
			WriteAccent(entries, accent, isLight, fill, primaryBrush?.Opacity ?? 1, PickOnAccentText(fill));
		}

		if (onPrimary is { })
		{
			WriteColorAndBrush(entries, "TextOnAccentFillColorPrimary", onPrimary.Color, onPrimary.Opacity);
			WriteColorAndBrush(entries, "TextOnAccentFillColorSecondary", onPrimary.Color, onPrimary.Opacity);
		}

		if (consumerOverride is { })
		{
			foreach (var key in new List<string>(entries.Keys))
			{
				if (ThemeResourceResolver.Resolve(consumerOverride, appearance, key) is { })
				{
					entries.Remove(key);
				}
			}
		}

		return entries;
	}

	/// <summary>
	/// The platform's own accent resources of one appearance, to restore built-in controls when the
	/// driver clears; <c>null</c> when the platform accent is unavailable.
	/// </summary>
	internal static Dictionary<string, object>? BuildPlatform(bool isLight)
	{
		if (GetPlatformShades() is not { } shades)
		{
			return null;
		}

		var entries = new Dictionary<string, object>(StringComparer.Ordinal);
		// The stock on-accent text is white in the light theme, black in the dark.
		WriteAccent(entries, shades, isLight, isLight ? shades.Dark1 : shades.Light2, 1, isLight ? (White, WhiteSecondary) : (Black, BlackSecondary));
		return entries;
	}

	/// <summary>
	/// The on-accent text family (white or black, with Fluent's secondary opacity) that contrasts best
	/// with <paramref name="fill"/>: the stock family assumes a mid-tone accent.
	/// </summary>
	internal static (Color Primary, Color Secondary) PickOnAccentText(Color fill)
	{
		var fillArgb = ToArgb(fill);
		return ColorMath.ContrastRatio(fillArgb, ToArgb(White)) >= ColorMath.ContrastRatio(fillArgb, ToArgb(Black))
			? (White, WhiteSecondary)
			: (Black, BlackSecondary);
	}

	private static void WriteAccent(Dictionary<string, object> entries, AccentShades shades, bool isLight, Color fill, double fillOpacity, (Color Primary, Color Secondary) onAccent)
	{
		entries["SystemAccentColor"] = shades.Accent;
		entries["SystemAccentColorLight1"] = shades.Light1;
		entries["SystemAccentColorLight2"] = shades.Light2;
		entries["SystemAccentColorLight3"] = shades.Light3;
		entries["SystemAccentColorDark1"] = shades.Dark1;
		entries["SystemAccentColorDark2"] = shades.Dark2;
		entries["SystemAccentColorDark3"] = shades.Dark3;

		foreach (var key in LegacyAccentBrushKeys)
		{
			entries[key] = new SolidColorBrush(shades.Accent);
		}

		// Secondary / Tertiary fills are the same color at 90% / 80% brush opacity, as in Fluent.
		WriteColorAndBrush(entries, "AccentFillColorDefault", fill, fillOpacity);
		WriteColorAndBrush(entries, "AccentFillColorSecondary", fill, fillOpacity * 0.9);
		WriteColorAndBrush(entries, "AccentFillColorTertiary", fill, fillOpacity * 0.8);
		WriteColorAndBrush(entries, "AccentFillColorSelectedTextBackground", shades.Accent, 1);

		// Fluent's accent text: light theme Dark2 / Dark3 / Dark1, dark theme Light3 / Light3 / Light2.
		WriteColorAndBrush(entries, "AccentTextFillColorPrimary", isLight ? shades.Dark2 : shades.Light3, 1);
		WriteColorAndBrush(entries, "AccentTextFillColorSecondary", isLight ? shades.Dark3 : shades.Light3, 1);
		WriteColorAndBrush(entries, "AccentTextFillColorTertiary", isLight ? shades.Dark1 : shades.Light2, 1);

		WriteColorAndBrush(entries, "TextOnAccentFillColorPrimary", onAccent.Primary, 1);
		WriteColorAndBrush(entries, "TextOnAccentFillColorSecondary", onAccent.Secondary, 1);
	}

	private static void WriteColorAndBrush(Dictionary<string, object> entries, string colorKey, Color color, double opacity)
	{
		entries[colorKey] = color;
		entries[colorKey + "Brush"] = new SolidColorBrush(color) { Opacity = opacity };
	}

	private static bool TryReadShades(ResourceDictionary resources, Color[] colors)
	{
		for (var i = 0; i < ShadeKeys.Length; i++)
		{
			if (!resources.TryGetValue(ShadeKeys[i], out var value) || value is not Color color)
			{
				return false;
			}
			colors[i] = color;
		}
		return true;
	}

	private static XamlControlsResources? FindXamlControlsResources(ResourceDictionary? dictionary)
	{
		if (dictionary is null)
		{
			return null;
		}

		if (dictionary is XamlControlsResources xcr)
		{
			return xcr;
		}

		foreach (var merged in dictionary.MergedDictionaries)
		{
			if (FindXamlControlsResources(merged) is { } found)
			{
				return found;
			}
		}

		return null;
	}

	private static TonalPalette PaletteOf(HctColor hct, SeedColorMode mode)
		=> new(hct.Hue, mode == SeedColorMode.TonalSpot ? Math.Max(hct.Chroma, TonalSpotMinimumChroma) : hct.Chroma);

	// Relative shades land on fractional tones; the palette is integer-toned.
	private static Color ToneColor(TonalPalette palette, double tone)
		=> FromArgb(palette.GetArgb((int)Math.Round(Math.Clamp(tone, 0, 100))));

	private static int ToArgb(Color color) =>
		(color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

	private static Color FromArgb(int argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
}
