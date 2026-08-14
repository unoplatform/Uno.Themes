#nullable enable

using System.Collections.Generic;
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
/// Builds the reverse "primary → Fluent accent" mapping (spec 05 §9): when an
/// effective primary color is active — a seed, or an explicit
/// <c>PrimaryColor</c> consumer override from any channel
/// (<c>Colors.OverrideDictionary</c> / <c>Colors.OverrideSource</c> or the
/// obsolete <c>BaseTheme.ColorOverride*</c> properties, which funnel into it) —
/// the built-in Fluent controls must follow it, so the
/// <c>SystemAccentColor*</c> shades and the accent-derived token closure are
/// overridden per theme branch.
/// </summary>
/// <remarks>
/// <para>
/// Two driver modes, mirroring how Material/Simple behave (their templates
/// consume the semantic brushes directly, so both a seed and a PrimaryColor
/// override visibly recolor controls):
/// a <b>seed</b> is a generator input — the shades follow the tonal palette
/// (spec 05 §9.1: light fill = tone 30 / <c>Dark1</c>, dark fill = tone 70 /
/// <c>Light2</c>, matching Fluent's own accent usage — spike S2/S4); an
/// explicit <b>PrimaryColor override</b> is the highest-precedence statement
/// of what "Primary" IS, so it becomes the accent fill VERBATIM for its
/// branch (with the surrounding shades derived tonally from it). An override
/// takes precedence over the seed per branch, exactly as it does in the
/// semantic palette layer.
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
/// <para>
/// Any accent-family key the consumer override defines EXPLICITLY wins over
/// the derived value (the repo-wide override-precedence contract,
/// <c>Given_ColorOverridePrecedence</c>): those values are copied over the
/// derived entries, branch-aware ("Light" / "Dark" / "Default" / flat).
/// </para>
/// </remarks>
internal static class FluentAccentPalette
{
	private const string PrimaryColorKey = "PrimaryColor";

	private const string LightBranchKey = "Light";
	private const string DarkBranchKey = "Dark";
	private const string DefaultBranchKey = "Default";

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

	private static readonly string[] _accentShadeKeys =
	{
		"SystemAccentColor",
		"SystemAccentColorLight1",
		"SystemAccentColorLight2",
		"SystemAccentColorLight3",
		"SystemAccentColorDark1",
		"SystemAccentColorDark2",
		"SystemAccentColorDark3",
	};

	private static readonly string[] _closureKeys =
	{
		"AccentFillColorDefault",
		"AccentFillColorDefaultBrush",
		"AccentFillColorSecondary",
		"AccentFillColorSecondaryBrush",
		"AccentFillColorTertiary",
		"AccentFillColorTertiaryBrush",
		"AccentFillColorSelectedTextBackground",
		"AccentFillColorSelectedTextBackgroundBrush",
		"AccentTextFillColorPrimary",
		"AccentTextFillColorPrimaryBrush",
		"AccentTextFillColorSecondary",
		"AccentTextFillColorSecondaryBrush",
		"AccentTextFillColorTertiary",
		"AccentTextFillColorTertiaryBrush",
	};

	/// <summary>
	/// Resolves the per-branch accent basis from the consumer override's
	/// explicit <c>PrimaryColor</c>, honoring the native ThemeDictionaries
	/// semantics: the exact branch key first ("Light" / "Dark"), then the
	/// universal "Default" branch, then a flat (theme-invariant) entry.
	/// Reads OWN entries only — <c>TryGetValue</c> would also search the
	/// ambient theme branch and break branch fidelity.
	/// </summary>
	internal static (Color? Light, Color? Dark) ResolveAccentBasis(ResourceDictionary? consumerOverride)
	{
		if (consumerOverride is null)
		{
			return (null, null);
		}

		var flat = ReadOwnColor(consumerOverride, PrimaryColorKey);
		var fallback = ReadBranchColor(consumerOverride, DefaultBranchKey, PrimaryColorKey);
		var light = ReadBranchColor(consumerOverride, LightBranchKey, PrimaryColorKey) ?? fallback ?? flat;
		var dark = ReadBranchColor(consumerOverride, DarkBranchKey, PrimaryColorKey) ?? fallback ?? flat;
		return (light, dark);
	}

	/// <summary>
	/// Builds the accent override dictionary from the effective drivers: the
	/// per-branch override basis (verbatim accent) where present, else the
	/// <paramref name="seed"/>'s tonal mapping. At least one driver must be
	/// non-null. Consumer-explicit accent-family keys are copied over the
	/// derived values last.
	/// </summary>
	internal static ResourceDictionary Build(Color? seed, Color? lightBasis, Color? darkBasis, ResourceDictionary? consumerOverride)
	{
		var dictionary = new ResourceDictionary();
		var seedPalette = seed is { } s ? PaletteOf(s) : null;

		if (lightBasis is null && darkBasis is null && seedPalette is { } pure)
		{
			// Pure-seed mode: the shade set is theme-invariant (like the
			// platform's), so it lives in flat entries visible from both theme
			// branches; only the closure varies per branch.
			var accent = ToneColor(pure, AccentTone);
			WriteShades(dictionary, pure, accent);
			WriteLegacyBrushes(dictionary, accent);

			dictionary.ThemeDictionaries[LightBranchKey] = BuildSeedClosure(pure, isLight: true);
			dictionary.ThemeDictionaries[DefaultBranchKey] = BuildSeedClosure(pure, isLight: false);
		}
		else
		{
			// Override-driven mode: the basis can differ per branch, so
			// EVERYTHING is branch-scoped (never mixing flat and branch entries
			// for the same key — their relative precedence is not portable).
			// A branch with neither a basis nor a seed gets no entries at all:
			// the platform accent stays in effect for it.
			if (BuildBranchFor(isLight: true, lightBasis, seedPalette) is { } light)
			{
				dictionary.ThemeDictionaries[LightBranchKey] = light;
			}

			if (BuildBranchFor(isLight: false, darkBasis, seedPalette) is { } dark)
			{
				dictionary.ThemeDictionaries[DefaultBranchKey] = dark;
			}
		}

		if (consumerOverride is { })
		{
			ApplyConsumerAccentOverrides(dictionary, consumerOverride);
		}

		return dictionary;
	}

	private static ResourceDictionary? BuildBranchFor(bool isLight, Color? basis, TonalPalette? seedPalette)
	{
		if (basis is { } b)
		{
			// The override IS the accent for this branch (parity with
			// Material/Simple: the color the consumer set is the color they
			// see); the surrounding shades and accent-text tones derive from it.
			var palette = PaletteOf(b);
			var branch = new ResourceDictionary();

			branch["SystemAccentColor"] = b;
			branch["SystemAccentColorLight1"] = ToneColor(palette, Light1Tone);
			branch["SystemAccentColorLight2"] = ToneColor(palette, Light2Tone);
			branch["SystemAccentColorLight3"] = ToneColor(palette, Light3Tone);
			branch["SystemAccentColorDark1"] = ToneColor(palette, Dark1Tone);
			branch["SystemAccentColorDark2"] = ToneColor(palette, Dark2Tone);
			branch["SystemAccentColorDark3"] = ToneColor(palette, Dark3Tone);
			WriteLegacyBrushes(branch, b);

			WriteClosure(
				branch,
				fill: b,
				textPrimary: ToneColor(palette, isLight ? Dark2Tone : Light3Tone),
				textSecondary: ToneColor(palette, isLight ? Dark3Tone : Light3Tone),
				textTertiary: ToneColor(palette, isLight ? Dark1Tone : Light2Tone),
				selectedTextBackground: b);

			return branch;
		}

		if (seedPalette is { } sp)
		{
			// Mixed mode (the OTHER branch has a basis): this branch follows the
			// seed, with the shade set branch-scoped instead of flat.
			var branch = BuildSeedClosure(sp, isLight);
			var accent = ToneColor(sp, AccentTone);
			WriteShades(branch, sp, accent);
			WriteLegacyBrushes(branch, accent);
			return branch;
		}

		return null;
	}

	private static ResourceDictionary BuildSeedClosure(TonalPalette palette, bool isLight)
	{
		// Fluent's accent usage per branch (S4 capture, matching WinUI):
		//   light: fill = Dark1; accent text = Dark2 / Dark3 / Dark1
		//   dark:  fill = Light2; accent text = Light3 / Light3 / Light2
		var branch = new ResourceDictionary();
		WriteClosure(
			branch,
			fill: ToneColor(palette, isLight ? Dark1Tone : Light2Tone),
			textPrimary: ToneColor(palette, isLight ? Dark2Tone : Light3Tone),
			textSecondary: ToneColor(palette, isLight ? Dark3Tone : Light3Tone),
			textTertiary: ToneColor(palette, isLight ? Dark1Tone : Light2Tone),
			selectedTextBackground: ToneColor(palette, AccentTone));
		return branch;
	}

	private static void WriteShades(ResourceDictionary target, TonalPalette palette, Color accent)
	{
		target["SystemAccentColor"] = accent;
		target["SystemAccentColorLight1"] = ToneColor(palette, Light1Tone);
		target["SystemAccentColorLight2"] = ToneColor(palette, Light2Tone);
		target["SystemAccentColorLight3"] = ToneColor(palette, Light3Tone);
		target["SystemAccentColorDark1"] = ToneColor(palette, Dark1Tone);
		target["SystemAccentColorDark2"] = ToneColor(palette, Dark2Tone);
		target["SystemAccentColorDark3"] = ToneColor(palette, Dark3Tone);
	}

	private static void WriteLegacyBrushes(ResourceDictionary target, Color accent)
	{
		foreach (var key in _legacyAccentBrushKeys)
		{
			target[key] = new SolidColorBrush(accent);
		}
	}

	private static void WriteClosure(ResourceDictionary branch, Color fill, Color textPrimary, Color textSecondary, Color textTertiary, Color selectedTextBackground)
	{
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
	}

	/// <summary>
	/// Copies every accent-family key the consumer override defines EXPLICITLY
	/// over the derived entries, branch-aware, so the consumer keeps the last
	/// word on any key it names (override-precedence contract).
	/// </summary>
	private static void ApplyConsumerAccentOverrides(ResourceDictionary dictionary, ResourceDictionary consumerOverride)
	{
		var flat = ToOwnEntries(consumerOverride);
		var light = BranchEntries(consumerOverride, LightBranchKey);
		var dark = BranchEntries(consumerOverride, DarkBranchKey);
		var fallback = BranchEntries(consumerOverride, DefaultBranchKey);
		var derivedFlat = ToOwnEntries(dictionary);

		foreach (var key in EnumerateManagedKeys())
		{
			if (flat.TryGetValue(key, out var flatValue))
			{
				// Replace the derived flat entry when one exists (pure-seed
				// shades), and mirror into both branches so the consumer value
				// wins regardless of flat-vs-branch lookup order.
				if (derivedFlat.ContainsKey(key))
				{
					dictionary[key] = flatValue;
				}

				WriteToBranch(dictionary, LightBranchKey, key, flatValue);
				WriteToBranch(dictionary, DefaultBranchKey, key, flatValue);
			}

			var lightValue = OwnValue(light, key) ?? OwnValue(fallback, key);
			if (lightValue is { })
			{
				WriteToBranch(dictionary, LightBranchKey, key, lightValue);
			}

			var darkValue = OwnValue(dark, key) ?? OwnValue(fallback, key);
			if (darkValue is { })
			{
				WriteToBranch(dictionary, DefaultBranchKey, key, darkValue);
			}
		}
	}

	private static IEnumerable<string> EnumerateManagedKeys()
	{
		foreach (var key in _accentShadeKeys)
		{
			yield return key;
		}
		foreach (var key in _legacyAccentBrushKeys)
		{
			yield return key;
		}
		foreach (var key in _closureKeys)
		{
			yield return key;
		}
	}

	private static void WriteToBranch(ResourceDictionary dictionary, string branchKey, string key, object value)
	{
		if (dictionary.ThemeDictionaries.TryGetValue(branchKey, out var existing) && existing is ResourceDictionary branch)
		{
			branch[key] = value;
		}
		else
		{
			var created = new ResourceDictionary();
			created[key] = value;
			dictionary.ThemeDictionaries[branchKey] = created;
		}
	}

	private static Color? ReadBranchColor(ResourceDictionary dictionary, string branchKey, string key)
	{
		if (dictionary.ThemeDictionaries.TryGetValue(branchKey, out var value) && value is ResourceDictionary branch)
		{
			return ReadOwnColor(branch, key);
		}

		return null;
	}

	private static Color? ReadOwnColor(ResourceDictionary dictionary, string key)
		=> ToOwnEntries(dictionary).TryGetValue(key, out var value) && value is Color color ? color : null;

	private static object? OwnValue(Dictionary<string, object>? entries, string key)
		=> entries is { } && entries.TryGetValue(key, out var value) ? value : null;

	private static Dictionary<string, object>? BranchEntries(ResourceDictionary dictionary, string branchKey)
		=> dictionary.ThemeDictionaries.TryGetValue(branchKey, out var value) && value is ResourceDictionary branch
			? ToOwnEntries(branch)
			: null;

	private static Dictionary<string, object> ToOwnEntries(ResourceDictionary dictionary)
	{
		var entries = new Dictionary<string, object>();
		foreach (var pair in dictionary)
		{
			if (pair.Key is string key)
			{
				entries[key] = pair.Value;
			}
		}
		return entries;
	}

	private static TonalPalette PaletteOf(Color color)
	{
		// High-fidelity generation (chroma preserved, no M3 minimum-chroma
		// floor): for a seed this must match FluentTheme.UseHighFidelityColors
		// so the reverse mapping and the seed-generated semantic palette agree
		// on every tone; for an override basis it keeps the derived shades
		// faithful to the exact color the consumer chose.
		var hct = HctColor.FromArgb(ToArgb(color));
		return new TonalPalette(hct.Hue, hct.Chroma);
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
