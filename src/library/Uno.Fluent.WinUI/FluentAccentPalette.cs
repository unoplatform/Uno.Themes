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
			var hct = HctColor.FromArgb(SeedColorPaletteGenerator.ColorToArgb(seed));
			var palette = SeedColorPaletteGenerator.CreatePrimaryPalette(hct, mode);

			return mode == SeedColorMode.TonalSpot
				? From(ToneColor(palette, AccentTone), AccentTone, palette)
				: From(Color.FromArgb(0xFF, seed.R, seed.G, seed.B), hct.Tone, palette);
		}

		/// <summary>The shades of a verbatim accent (a <c>PrimaryColor</c> override): its own chroma and tone.</summary>
		internal static AccentShades FromBasis(Color basis)
		{
			var hct = HctColor.FromArgb(SeedColorPaletteGenerator.ColorToArgb(basis));
			return From(basis, hct.Tone, SeedColorPaletteGenerator.CreatePrimaryPalette(hct, SeedColorMode.Fidelity));
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
			FluentDiagnostics.LogWarning("FluentTheme could not read the platform accent.", e);
			return null;
		}
	}

	/// <summary>
	/// The accent resources of one appearance, driven by the effective primary in the semantic palette's
	/// precedence: the consumer override's <c>PrimaryBrush</c> (solid) or <c>PrimaryColor</c> — the accent
	/// verbatim — else <paramref name="seedShades"/>, else <paramref name="constructorOverride"/>'s; plus
	/// <c>OnPrimaryBrush</c> / <c>OnPrimaryColor</c> as the on-accent text, in the same order. Keys either
	/// override declares are left out whether or not it drives the accent, so an explicit consumer value
	/// always wins. <c>null</c> when nothing drives the accent.
	/// </summary>
	/// <returns>Colors and <see cref="SolidColorBrush"/> templates (color + opacity), by key.</returns>
	internal static Dictionary<string, object>? Build(bool isLight, AccentShades? seedShades, ResourceDictionary? consumerOverride, ResourceDictionary? constructorOverride = null)
	{
		// Read through the semantic brushes' resolver. Declared entries only: the platform defines the
		// accent keys too. Resolved once per layer: every key below is looked up in these snapshots.
		var themes = isLight ? ThemesConstants.LightColorThemes : ThemesConstants.DarkColorThemes;
		var consumer = Resolve(consumerOverride, themes);
		var constructor = Resolve(constructorOverride, themes);

		// A seed outranks the constructor override's semantic roles, as in the semantic palette.
		object? Find(string key)
			=> consumer is { } && consumer.TryGetValue(key, out var value) ? value
			: seedShades is null && constructor is { } && constructor.TryGetValue(key, out value) ? value
			: null;

		var primaryBrush = Find("PrimaryBrush") as SolidColorBrush;
		var basis = primaryBrush?.Color ?? Find(SemanticColorKeys.Primary) as Color?;

		var onPrimary = Find("OnPrimaryBrush") as SolidColorBrush;
		if (onPrimary is null && Find(SemanticColorKeys.OnPrimary) is Color onPrimaryColor)
		{
			onPrimary = new SolidColorBrush(onPrimaryColor);
		}

		var shades = basis is { } b ? AccentShades.FromBasis(b) : seedShades;
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

		RemoveDeclared(entries, consumer);
		RemoveDeclared(entries, constructor);
		return entries;
	}

	private static Dictionary<string, object>? Resolve(ResourceDictionary? layer, string[] themes)
		=> layer is { } ? SemanticBrushUpdater.ResolveDeclared(layer, themes) : null;

	private static void RemoveDeclared(Dictionary<string, object> entries, Dictionary<string, object>? declared)
	{
		if (declared is { })
		{
			foreach (var key in declared.Keys)
			{
				entries.Remove(key);
			}
		}
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
		var fillArgb = SeedColorPaletteGenerator.ColorToArgb(fill);
		return ColorMath.ContrastRatio(fillArgb, SeedColorPaletteGenerator.ColorToArgb(White)) >= ColorMath.ContrastRatio(fillArgb, SeedColorPaletteGenerator.ColorToArgb(Black))
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

	// Relative shades land on fractional tones; the palette is integer-toned.
	private static Color ToneColor(TonalPalette palette, double tone)
		=> SeedColorPaletteGenerator.ArgbToColor(palette.GetArgb((int)Math.Round(Math.Clamp(tone, 0, 100))));
}
