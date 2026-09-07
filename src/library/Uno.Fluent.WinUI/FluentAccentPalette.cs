#nullable enable

using System;
using System.Collections.Generic;
using Uno.Themes;
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
/// (spec 05 §9.1: light fill = <c>Dark1</c>, dark fill = <c>Light2</c>,
/// matching Fluent's own accent usage — spike S2/S4), derived RELATIVE to the
/// accent's tone (<see cref="AccentShades"/>) and with the theme's
/// <see cref="SeedColorMode"/> exactly as the semantic palette is: under
/// <see cref="SeedColorMode.Fidelity"/> (the default) the base
/// <c>SystemAccentColor</c> is the seed verbatim — the generated light
/// <c>PrimaryColor</c> — and the palette keeps the seed's chroma; under
/// <see cref="SeedColorMode.TonalSpot"/> it is tone 40 of the chroma-boosted
/// palette. <c>Light2</c> — Fluent's dark-theme fill — is anchored at the
/// dark-theme Primary tone, so the forward and reverse flows agree in both
/// branches and both modes (§9.3); an
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
/// <c>TextOnAccentFillColorPrimary/Secondary</c> (+ brushes) are written as
/// well, as the Fluent white or black family that contrasts best with the
/// branch's fill (<see cref="PickOnAccentText"/>): the stock families assume a
/// mid-tone platform accent, while a relative shade of a pale seed or a pale
/// verbatim override can produce a light fill that white text is unreadable
/// on. <c>TextOnAccentFillColorDisabled</c> stays stock (the disabled fill is a
/// seed-invariant neutral).
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
	private const string LightBranchKey = "Light";
	private const string DarkBranchKey = "Dark";
	private const string DefaultBranchKey = "Default";

	// Spec 05 §9.1 — the base accent tone under TonalSpot (M3's primary tone);
	// Fidelity pins the seed itself, at its own tone (see AccentShades).
	private const int AccentTone = 40;

	// The tone the semantic palette gives the DARK-theme PrimaryColor (M3: tone 80).
	// Light2 — Fluent's dark-theme accent fill — is anchored to it, so the built-in
	// accent and the semantic PrimaryColor agree in the dark branch too (§9.3).
	private const double DarkThemePrimaryTone = 80;

	// Material Design 3's tonal-spot minimum chroma on the primary palette — the
	// same floor SeedColorPaletteGenerator applies under SeedColorMode.TonalSpot.
	private const double TonalSpotMinimumChroma = 48;

	private static readonly Color White = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
	private static readonly Color Black = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);

	// Fluent's stock on-accent secondary text: 70% white / 50% black (S2 capture).
	private static readonly Color WhiteSecondary = Color.FromArgb(0xB3, 0xFF, 0xFF, 0xFF);
	private static readonly Color BlackSecondary = Color.FromArgb(0x80, 0x00, 0x00, 0x00);

	/// <summary>
	/// An accent color with its Fluent shade set, derived RELATIVE to the accent's
	/// own tone (spec 05 §9.1): the dark shades sit at 3/4, 1/2 and 1/4 of the way
	/// from the accent to black, so they stay darker than the accent whatever its
	/// tone (a navy brand color gets a navy light-theme fill, not a lighter one);
	/// <c>Light2</c> is anchored at the dark-theme Primary tone (80), <c>Light1</c>
	/// midway between the accent and it, <c>Light3</c> midway between it and white
	/// (tone 90 — where Fluent's own dark-theme accent text sits). At accent tone 40
	/// — TonalSpot's primary tone — the dark shades are the familiar 30/20/10 and
	/// Light1 is 60. Every shade is taken from the palette built with the
	/// generation mode's chroma, so it agrees with the seed-generated semantic
	/// palette. Known edge: for an accent lighter than tone 80 the light shades are
	/// not lighter than the accent — the dark-theme fill stays at tone 80, which
	/// keeps its black on-accent text readable.
	/// </summary>
	internal readonly record struct AccentShades(Color Accent, Color Light1, Color Light2, Color Light3, Color Dark1, Color Dark2, Color Dark3)
	{
		/// <summary>
		/// The shade set of a seed under <paramref name="mode"/> — the generator's own
		/// recipe for the light <c>PrimaryColor</c>: Fidelity pins the (opaque) seed
		/// verbatim at its own tone, TonalSpot takes tone 40 of the chroma-boosted palette.
		/// </summary>
		internal static AccentShades FromSeed(Color seed, SeedColorMode mode)
		{
			var hct = HctColor.FromArgb(ToArgb(seed));
			var palette = PaletteOf(hct, mode);

			return mode == SeedColorMode.TonalSpot
				? From(ToneColor(palette, AccentTone), AccentTone, palette)
				: From(Color.FromArgb(0xFF, seed.R, seed.G, seed.B), hct.Tone, palette);
		}

		/// <summary>
		/// The shade set of a verbatim accent (a <c>PrimaryColor</c> override basis):
		/// its own chroma and tone — the override is a statement, not a generator input.
		/// </summary>
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
	/// The Fluent on-accent text family — white or black, with Fluent's stock
	/// secondary opacity — that contrasts best with <paramref name="fill"/>.
	/// Fluent's stock values (white in the light theme, black in the dark) assume a
	/// mid-tone platform accent; a pale or very dark effective accent needs the
	/// other family, as Material picks <c>OnPrimary</c> by contrast.
	/// </summary>
	internal static (Color Primary, Color Secondary) PickOnAccentText(Color fill)
	{
		var fillArgb = ToArgb(fill);
		return ColorMath.ContrastRatio(fillArgb, ToArgb(White)) >= ColorMath.ContrastRatio(fillArgb, ToArgb(Black))
			? (White, WhiteSecondary)
			: (Black, BlackSecondary);
	}

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
		"TextOnAccentFillColorPrimary",
		"TextOnAccentFillColorPrimaryBrush",
		"TextOnAccentFillColorSecondary",
		"TextOnAccentFillColorSecondaryBrush",
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

		var flat = ReadOwnColor(consumerOverride, SemanticColorKeys.Primary);
		var fallback = ReadBranchColor(consumerOverride, DefaultBranchKey, SemanticColorKeys.Primary);
		var light = ReadBranchColor(consumerOverride, LightBranchKey, SemanticColorKeys.Primary) ?? fallback ?? flat;
		var dark = ReadBranchColor(consumerOverride, DarkBranchKey, SemanticColorKeys.Primary) ?? fallback ?? flat;
		return (light, dark);
	}

	/// <summary>
	/// Builds the accent override dictionary from the effective drivers: the
	/// per-branch override basis (verbatim accent) where present, else the
	/// <paramref name="seed"/>'s tonal mapping under <paramref name="seedColorMode"/>
	/// (the mode the semantic palette is generated with). At least one driver
	/// must be non-null. Consumer-explicit accent-family keys are copied over the
	/// derived values last.
	/// </summary>
	internal static ResourceDictionary Build(Color? seed, SeedColorMode seedColorMode, Color? lightBasis, Color? darkBasis, ResourceDictionary? consumerOverride)
	{
		var dictionary = new ResourceDictionary();
		var seedShades = seed is { } s ? AccentShades.FromSeed(s, seedColorMode) : (AccentShades?)null;

		if (lightBasis is null && darkBasis is null && seedShades is { } pure)
		{
			// Pure-seed mode: the shade set is theme-invariant (like the
			// platform's), so it lives in flat entries visible from both theme
			// branches; only the closure varies per branch.
			WriteShades(dictionary, pure);
			WriteLegacyBrushes(dictionary, pure.Accent);

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
			if (BuildBranchFor(isLight: true, lightBasis, seedShades) is { } light)
			{
				dictionary.ThemeDictionaries[LightBranchKey] = light;
			}

			if (BuildBranchFor(isLight: false, darkBasis, seedShades) is { } dark)
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

	private static ResourceDictionary? BuildBranchFor(bool isLight, Color? basis, AccentShades? seedShades)
	{
		if (basis is { } b)
		{
			// The override IS the accent for this branch (parity with
			// Material/Simple: the color the consumer set is the color they
			// see); the surrounding shades and accent-text tones derive from it,
			// relative to its own tone and keeping its exact chroma (the
			// override is a verbatim statement, not a generator input).
			var shades = AccentShades.FromBasis(b);
			var branch = new ResourceDictionary();

			WriteShades(branch, shades);
			WriteLegacyBrushes(branch, b);

			WriteClosure(
				branch,
				fill: b,
				textPrimary: isLight ? shades.Dark2 : shades.Light3,
				textSecondary: isLight ? shades.Dark3 : shades.Light3,
				textTertiary: isLight ? shades.Dark1 : shades.Light2,
				selectedTextBackground: b);

			return branch;
		}

		if (seedShades is { } st)
		{
			// Mixed mode (the OTHER branch has a basis): this branch follows the
			// seed, with the shade set branch-scoped instead of flat.
			var branch = BuildSeedClosure(st, isLight);
			WriteShades(branch, st);
			WriteLegacyBrushes(branch, st.Accent);
			return branch;
		}

		return null;
	}

	private static ResourceDictionary BuildSeedClosure(AccentShades shades, bool isLight)
	{
		// Fluent's accent usage per branch (S4 capture, matching WinUI):
		//   light: fill = Dark1; accent text = Dark2 / Dark3 / Dark1
		//   dark:  fill = Light2; accent text = Light3 / Light3 / Light2
		var branch = new ResourceDictionary();
		WriteClosure(
			branch,
			fill: isLight ? shades.Dark1 : shades.Light2,
			textPrimary: isLight ? shades.Dark2 : shades.Light3,
			textSecondary: isLight ? shades.Dark3 : shades.Light3,
			textTertiary: isLight ? shades.Dark1 : shades.Light2,
			selectedTextBackground: shades.Accent);
		return branch;
	}

	private static void WriteShades(ResourceDictionary target, AccentShades shades)
	{
		target["SystemAccentColor"] = shades.Accent;
		target["SystemAccentColorLight1"] = shades.Light1;
		target["SystemAccentColorLight2"] = shades.Light2;
		target["SystemAccentColorLight3"] = shades.Light3;
		target["SystemAccentColorDark1"] = shades.Dark1;
		target["SystemAccentColorDark2"] = shades.Dark2;
		target["SystemAccentColorDark3"] = shades.Dark3;
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

		// Text ON the accent fill: the white/black family that contrasts with THIS
		// branch's fill (the stock family assumes a mid-tone platform accent).
		var (onAccentPrimary, onAccentSecondary) = PickOnAccentText(fill);
		branch["TextOnAccentFillColorPrimary"] = onAccentPrimary;
		branch["TextOnAccentFillColorPrimaryBrush"] = new SolidColorBrush(onAccentPrimary);
		branch["TextOnAccentFillColorSecondary"] = onAccentSecondary;
		branch["TextOnAccentFillColorSecondaryBrush"] = new SolidColorBrush(onAccentSecondary);
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

	/// <summary>
	/// The primary tonal palette of <paramref name="hct"/> under
	/// <paramref name="mode"/> — the same recipe <c>SeedColorPaletteGenerator</c>
	/// applies, so every tone the reverse mapping (and the lightweight bridge)
	/// derives agrees with the seed-generated semantic palette:
	/// <see cref="SeedColorMode.Fidelity"/> keeps the color's own chroma,
	/// <see cref="SeedColorMode.TonalSpot"/> enforces M3's minimum of 48.
	/// </summary>
	private static TonalPalette PaletteOf(HctColor hct, SeedColorMode mode)
	{
		var chroma = mode == SeedColorMode.TonalSpot
			? Math.Max(hct.Chroma, TonalSpotMinimumChroma)
			: hct.Chroma;
		return new TonalPalette(hct.Hue, chroma);
	}

	private static Color ToneColor(TonalPalette palette, int tone)
		=> FromArgb(palette.GetArgb(tone));

	// Relative shades land on fractional tones; the palette is integer-toned.
	private static Color ToneColor(TonalPalette palette, double tone)
		=> ToneColor(palette, (int)Math.Round(Math.Clamp(tone, 0, 100)));

	private static int ToArgb(Color color) =>
		(color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

	private static Color FromArgb(int argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
}
