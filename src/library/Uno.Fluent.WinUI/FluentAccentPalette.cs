#nullable enable

using Uno.Themes.ColorGeneration;
using Uno.Themes.ColorGeneration.Hct;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Fluent;

/// <summary>
/// Builds the reverse seed → Fluent accent mapping (spec 05 §9): when a seed
/// color is active, the built-in Fluent controls must follow it, so the
/// <c>SystemAccentColor*</c> shades and the accent-derived token closure are
/// overridden per theme branch with tones from the seed's tonal palette.
/// </summary>
/// <remarks>
/// <para>
/// Tone assignments (spec 05 §9.1) are chosen so Fluent's own usage lands on
/// tones with correct contrast: the light-theme accent fill is
/// <c>SystemAccentColorDark1</c> and the dark-theme fill is
/// <c>SystemAccentColorLight2</c> (spike S2/S4).
/// </para>
/// <para>
/// On Uno targets, overriding <c>SystemAccentColor*</c> alone cascades into
/// every accent brush — XCR's accent resources re-resolve late-bound against
/// the ambient scope (spike S4, 2026-07-15). The closure (the
/// <c>AccentFillColor*</c> / <c>AccentTextFillColor*</c> colors and brushes)
/// is written as well for WinUI on Windows, where XCR resolves its internal
/// <c>{StaticResource SystemAccentColor*}</c> references eagerly at load and a
/// late shade override does not retro-propagate (spec 05 D12). The closure
/// values mirror XCR's own structure (S4 capture), so both paths agree.
/// <c>TextOnAccentFillColor*</c> is intentionally NOT overridden: its values
/// (white / near-black families) are seed-invariant, and the seeded fill tones
/// (30 light / 70 dark) preserve the platform contrast direction.
/// </para>
/// </remarks>
internal static class FluentAccentPalette
{
	// Spec 05 §9.1 — Fluent accent shade → tonal-palette tone.
	private const int AccentTone = 40;
	private const int Light1Tone = 60;
	private const int Light2Tone = 70;
	private const int Light3Tone = 80;
	private const int Dark1Tone = 30;
	private const int Dark2Tone = 20;
	private const int Dark3Tone = 10;

	// UWP-era accent brushes still referenced by some Uno templates; all carry
	// the base accent in both theme branches (S4 capture).
	private static readonly string[] _legacyAccentBrushKeys =
	{
		"SystemControlBackgroundAccentBrush",
		"SystemControlForegroundAccentBrush",
		"SystemControlHighlightAccentBrush",
		"SystemControlHighlightAltAccentBrush",
		"SystemControlHyperlinkTextBrush",
		"SystemColorControlAccentBrush",
	};

	/// <summary>
	/// Builds the accent override dictionary for <paramref name="seed"/>:
	/// theme-invariant <c>SystemAccentColor*</c> shades as flat entries, and the
	/// theme-varying accent closure per Light/Default branch.
	/// </summary>
	internal static ResourceDictionary Build(Color seed)
	{
		// High-fidelity generation (chroma preserved, no M3 minimum-chroma
		// floor) — must match FluentTheme.UseHighFidelityColors so the reverse
		// mapping and the seed-generated semantic palette agree on every tone.
		var hct = HctColor.FromArgb(ToArgb(seed));
		var palette = new TonalPalette(hct.Hue, hct.Chroma);

		var accent = ToneColor(palette, AccentTone);
		var light1 = ToneColor(palette, Light1Tone);
		var light2 = ToneColor(palette, Light2Tone);
		var light3 = ToneColor(palette, Light3Tone);
		var dark1 = ToneColor(palette, Dark1Tone);
		var dark2 = ToneColor(palette, Dark2Tone);
		var dark3 = ToneColor(palette, Dark3Tone);

		var dictionary = new ResourceDictionary
		{
			// The shade set is theme-invariant (like the platform's), so it
			// lives in flat entries visible from both theme branches.
			["SystemAccentColor"] = accent,
			["SystemAccentColorLight1"] = light1,
			["SystemAccentColorLight2"] = light2,
			["SystemAccentColorLight3"] = light3,
			["SystemAccentColorDark1"] = dark1,
			["SystemAccentColorDark2"] = dark2,
			["SystemAccentColorDark3"] = dark3,
		};

		foreach (var key in _legacyAccentBrushKeys)
		{
			dictionary[key] = new SolidColorBrush(accent);
		}

		// Fluent's accent usage per branch (S4 capture, matching WinUI):
		//   light: fill = Dark1; accent text = Dark2 / Dark3 / Dark1
		//   dark:  fill = Light2; accent text = Light3 / Light3 / Light2
		dictionary.ThemeDictionaries["Light"] = BuildBranch(
			fill: dark1, textPrimary: dark2, textSecondary: dark3, textTertiary: dark1, selectedTextBackground: accent);
		dictionary.ThemeDictionaries["Default"] = BuildBranch(
			fill: light2, textPrimary: light3, textSecondary: light3, textTertiary: light2, selectedTextBackground: accent);

		return dictionary;
	}

	private static ResourceDictionary BuildBranch(Color fill, Color textPrimary, Color textSecondary, Color textTertiary, Color selectedTextBackground)
	{
		var branch = new ResourceDictionary();

		branch["AccentFillColorDefault"] = fill;
		branch["AccentFillColorDefaultBrush"] = new SolidColorBrush(fill);
		// Secondary/Tertiary are the same fill at 90% / 80% BRUSH opacity —
		// XCR's own structure (S4 capture); the color entries stay opaque.
		branch["AccentFillColorSecondary"] = fill;
		branch["AccentFillColorSecondaryBrush"] = new SolidColorBrush(fill) { Opacity = 0.9 };
		branch["AccentFillColorTertiary"] = fill;
		branch["AccentFillColorTertiaryBrush"] = new SolidColorBrush(fill) { Opacity = 0.8 };
		branch["AccentFillColorSelectedTextBackground"] = selectedTextBackground;
		branch["AccentFillColorSelectedTextBackgroundBrush"] = new SolidColorBrush(selectedTextBackground);

		branch["AccentTextFillColorPrimary"] = textPrimary;
		branch["AccentTextFillColorPrimaryBrush"] = new SolidColorBrush(textPrimary);
		branch["AccentTextFillColorSecondary"] = textSecondary;
		branch["AccentTextFillColorSecondaryBrush"] = new SolidColorBrush(textSecondary);
		branch["AccentTextFillColorTertiary"] = textTertiary;
		branch["AccentTextFillColorTertiaryBrush"] = new SolidColorBrush(textTertiary);

		return branch;
	}

	private static Color ToneColor(TonalPalette palette, int tone)
		=> FromArgb(palette.GetArgb(tone));

	private static int ToArgb(Color color) =>
		(color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

	private static Color FromArgb(int argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
}
