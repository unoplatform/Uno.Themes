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
/// Builds the lightweight-styling bridge (spec 05 §10, goal G6): defines the
/// documented semantic lightweight keys with Fluent default values, and — when
/// a consumer override supplies a semantic key — re-points the corresponding
/// built-in Fluent per-control resource at the override's value so
/// Fluent-styled controls reflect it.
/// </summary>
/// <remarks>
/// <para>
/// Mechanism (spike S4(b), 2026-07-15): a per-control resource redefined in
/// FluentTheme's dynamic layer wins the <c>{ThemeResource}</c> lookups made
/// inside XCR templates, both at app scope and container scope, and layer
/// swaps while attached propagate to newly rendered controls. Re-pointing is
/// <b>override-driven only</b> — with no override, the stock per-control
/// resources are left untouched, so default Fluent rendering stays exactly
/// the platform's (including live system-accent tracking on Windows).
/// </para>
/// <para>
/// Per-control rollout (spec 05 D13) — all six controls covered:
/// <b>Button</b> — Filled* (accent button), Outlined* (standard button),
/// TextButtonForeground / IconButtonForeground (consumed by the shipped
/// bridge styles via <c>{ThemeResource}</c> setters — element-scope
/// resolution, so page-scoped overrides work for those without re-pointing).
/// FilledTonal*/Elevated* map to the same standard button as Outlined* and
/// are NOT re-pointed (indistinguishable without re-templating — D1).
/// <b>TextBox</b> — both semantic families map onto the single Fluent
/// TextBox (TextControl*); Outlined* wins when both are overridden.
/// <b>CheckBox</b> — only the glyph family diverges from WinUI's names;
/// everything else is native. <b>ToggleSwitch</b> — Material's key names are
/// mapped onto WinUI's (FillOn/Off, StrokeOn/Off, KnobFillOn/Off*).
/// <b>RadioButton / Slider</b> — the documented semantic keys ARE WinUI's
/// per-control keys; overrides work natively, nothing to bridge.
/// Keys with no Fluent equivalent (IconForeground variants, StateLayer,
/// Elevation, StateCircle, knob shadow/bounds, icon presenters, Focused knob
/// states) are not bridged. Disabled and hover/pressed keys outside the
/// accent families are re-point pass-through only (no defaults): their
/// platform values are not verified on every branch.
/// </para>
/// <para>
/// Override channels (documented in lightweight-styling.md): app-wide via
/// <c>Colors.OverrideDictionary</c> (triggers a rebuild pass, feeding this
/// bridge); page/subtree-scoped overrides target the Fluent per-control keys
/// (<c>AccentButtonBackground</c>, …) directly, or — for the bridge-style
/// keys — the semantic key itself.
/// </para>
/// </remarks>
internal static class FluentLightweightBridge
{
	private const string LightBranchKey = "Light";
	private const string DefaultBranchKey = "Default";

	// Semantic key -> built-in Fluent per-control resource, re-pointed when a
	// consumer override defines the semantic key (spec 05 §10 step 2).
	private static readonly (string Semantic, string Fluent)[] _repointMap =
	{
		("FilledButtonBackground", "AccentButtonBackground"),
		("FilledButtonBackgroundPointerOver", "AccentButtonBackgroundPointerOver"),
		("FilledButtonBackgroundPressed", "AccentButtonBackgroundPressed"),
		("FilledButtonBackgroundDisabled", "AccentButtonBackgroundDisabled"),
		("FilledButtonForeground", "AccentButtonForeground"),
		("FilledButtonForegroundPointerOver", "AccentButtonForegroundPointerOver"),
		("FilledButtonForegroundPressed", "AccentButtonForegroundPressed"),
		("FilledButtonForegroundDisabled", "AccentButtonForegroundDisabled"),
		("FilledButtonBorderBrush", "AccentButtonBorderBrush"),
		("FilledButtonBorderBrushPointerOver", "AccentButtonBorderBrushPointerOver"),
		("FilledButtonBorderBrushPressed", "AccentButtonBorderBrushPressed"),
		("FilledButtonBorderBrushDisabled", "AccentButtonBorderBrushDisabled"),
		("OutlinedButtonBackground", "ButtonBackground"),
		("OutlinedButtonBackgroundPointerOver", "ButtonBackgroundPointerOver"),
		("OutlinedButtonBackgroundPressed", "ButtonBackgroundPressed"),
		("OutlinedButtonBackgroundDisabled", "ButtonBackgroundDisabled"),
		("OutlinedButtonForeground", "ButtonForeground"),
		("OutlinedButtonForegroundPointerOver", "ButtonForegroundPointerOver"),
		("OutlinedButtonForegroundPressed", "ButtonForegroundPressed"),
		("OutlinedButtonForegroundDisabled", "ButtonForegroundDisabled"),
		("OutlinedButtonBorderBrush", "ButtonBorderBrush"),
		("OutlinedButtonBorderBrushPointerOver", "ButtonBorderBrushPointerOver"),
		("OutlinedButtonBorderBrushPressed", "ButtonBorderBrushPressed"),
		("OutlinedButtonBorderBrushDisabled", "ButtonBorderBrushDisabled"),

		// TextBox — Fluent has a single TextBox, so BOTH semantic families
		// map onto the TextControl* resources; Outlined* is listed after
		// Filled* and wins when both are overridden (documented). The
		// Filled-only Background family has no Outlined counterpart.
		("FilledTextBoxBackground", "TextControlBackground"),
		("FilledTextBoxBackgroundPointerOver", "TextControlBackgroundPointerOver"),
		("FilledTextBoxBackgroundFocused", "TextControlBackgroundFocused"),
		("FilledTextBoxBackgroundDisabled", "TextControlBackgroundDisabled"),
		("FilledTextBoxForeground", "TextControlForeground"),
		("FilledTextBoxForegroundPointerOver", "TextControlForegroundPointerOver"),
		("FilledTextBoxForegroundFocused", "TextControlForegroundFocused"),
		("FilledTextBoxForegroundDisabled", "TextControlForegroundDisabled"),
		("FilledTextBoxBorderBrush", "TextControlBorderBrush"),
		("FilledTextBoxBorderBrushPointerOver", "TextControlBorderBrushPointerOver"),
		("FilledTextBoxBorderBrushFocused", "TextControlBorderBrushFocused"),
		("FilledTextBoxBorderBrushDisabled", "TextControlBorderBrushDisabled"),
		("FilledTextBoxPlaceholderForeground", "TextControlPlaceholderForeground"),
		("FilledTextBoxPlaceholderForegroundPointerOver", "TextControlPlaceholderForegroundPointerOver"),
		("FilledTextBoxPlaceholderForegroundFocused", "TextControlPlaceholderForegroundFocused"),
		("FilledTextBoxPlaceholderForegroundDisabled", "TextControlPlaceholderForegroundDisabled"),
		("FilledTextBoxHeaderForeground", "TextControlHeaderForeground"),
		("FilledTextBoxHeaderForegroundDisabled", "TextControlHeaderForegroundDisabled"),
		("OutlinedTextBoxForeground", "TextControlForeground"),
		("OutlinedTextBoxForegroundPointerOver", "TextControlForegroundPointerOver"),
		("OutlinedTextBoxForegroundFocused", "TextControlForegroundFocused"),
		("OutlinedTextBoxForegroundDisabled", "TextControlForegroundDisabled"),
		("OutlinedTextBoxBorderBrush", "TextControlBorderBrush"),
		("OutlinedTextBoxBorderBrushPointerOver", "TextControlBorderBrushPointerOver"),
		("OutlinedTextBoxBorderBrushFocused", "TextControlBorderBrushFocused"),
		("OutlinedTextBoxBorderBrushDisabled", "TextControlBorderBrushDisabled"),
		("OutlinedTextBoxPlaceholderForeground", "TextControlPlaceholderForeground"),
		("OutlinedTextBoxPlaceholderForegroundPointerOver", "TextControlPlaceholderForegroundPointerOver"),
		("OutlinedTextBoxPlaceholderForegroundFocused", "TextControlPlaceholderForegroundFocused"),
		("OutlinedTextBoxPlaceholderForegroundDisabled", "TextControlPlaceholderForegroundDisabled"),
		("OutlinedTextBoxHeaderForeground", "TextControlHeaderForeground"),
		("OutlinedTextBoxHeaderForegroundDisabled", "TextControlHeaderForegroundDisabled"),
		("TextBoxDeleteButtonForeground", "TextControlButtonForeground"),
		("TextBoxDeleteButtonForegroundPointerOver", "TextControlButtonForegroundPointerOver"),
		("TextBoxDeleteButtonForegroundPressed", "TextControlButtonForegroundPressed"),

		// CheckBox — every other documented CheckBox key name matches WinUI's
		// natively (no bridging needed); only the glyph family diverges.
		("CheckBoxGlyphForegroundUnchecked", "CheckBoxCheckGlyphForegroundUnchecked"),
		("CheckBoxGlyphForegroundChecked", "CheckBoxCheckGlyphForegroundChecked"),

		// ToggleSwitch — Material's key names diverge from WinUI's.
		// The unprefixed OuterBorder* family is the ON state.
		("ToggleSwitchOffOuterBorderFill", "ToggleSwitchFillOff"),
		("ToggleSwitchOffOuterBorderStroke", "ToggleSwitchStrokeOff"),
		("ToggleSwitchOuterBorderFill", "ToggleSwitchFillOn"),
		("ToggleSwitchOuterBorderStroke", "ToggleSwitchStrokeOn"),
		("ToggleSwitchKnobOffFill", "ToggleSwitchKnobFillOff"),
		("ToggleSwitchKnobOffFillPointerOver", "ToggleSwitchKnobFillOffPointerOver"),
		("ToggleSwitchKnobOffFillPressed", "ToggleSwitchKnobFillOffPressed"),
		("ToggleSwitchKnobOffFillDisabled", "ToggleSwitchKnobFillOffDisabled"),
		("ToggleSwitchKnobOnFill", "ToggleSwitchKnobFillOn"),
		("ToggleSwitchKnobOnFillPointerOver", "ToggleSwitchKnobFillOnPointerOver"),
		("ToggleSwitchKnobOnFillPressed", "ToggleSwitchKnobFillOnPressed"),
		("ToggleSwitchKnobOnFillDisabled", "ToggleSwitchKnobFillOnDisabled"),

		// RadioButton and Slider: the documented semantic key names ARE the
		// WinUI per-control keys (RadioButtonOuterEllipse*, SliderTrackFill,
		// SliderTrackValueFill*, SliderThumbBackground*, …) — consumer
		// overrides reach XCR templates natively, nothing to re-point.
		// Presence is guarded by Given_FluentLightweightStyling.
	};

	private readonly record struct BranchValues(Color Light, Color Dark);

	// Per-branch Fluent neutrals used by the semantic defaults. Values are
	// platform captures (spike S2 dark / S4 light), drift-guarded by
	// Given_FluentLightweightStyling.When_AmbientDefaults_MatchLiveTokens.
	// TextFillColor* intentionally mirrors FluentColorPalette's capture —
	// the two tables serve different layers; keep both in sync on re-capture.
	private static readonly BranchValues _textPrimary = new(Rgba(0xE4000000), Rgba(0xFFFFFFFF));
	private static readonly BranchValues _textSecondary = new(Rgba(0x9E000000), Rgba(0xC5FFFFFF));
	private static readonly BranchValues _onAccentPrimary = new(Rgba(0xFFFFFFFF), Rgba(0xFF000000));
	private static readonly BranchValues _onAccentSecondary = new(Rgba(0xB3FFFFFF), Rgba(0x80000000));
	private static readonly BranchValues _controlFillDefault = new(Rgba(0xB3FFFFFF), Rgba(0x0FFFFFFF));
	private static readonly BranchValues _strongStroke = new(Rgba(0x72000000), Rgba(0x8BFFFFFF));

	/// <summary>
	/// Builds the bridge dictionary: semantic Button lightweight keys with
	/// Fluent default values per theme branch, plus per-control re-pointing
	/// entries for every semantic key found in <paramref name="consumerOverride"/>.
	/// </summary>
	/// <param name="seed">The effective primary seed, when one is active — the accent-fill defaults follow it (tones 30/70, matching FluentAccentPalette).</param>
	/// <param name="consumerOverride">The consumer's <c>Colors.OverrideDictionary</c>, when set.</param>
	internal static ResourceDictionary Build(Color? seed, ResourceDictionary? consumerOverride)
	{
		var (lightFill, darkFill) = ResolveAccentFill(seed);

		var light = BuildBranchDefaults(isLight: true, lightFill);
		var dark = BuildBranchDefaults(isLight: false, darkFill);

		if (consumerOverride is { })
		{
			ApplyRepointing(light, dark, consumerOverride);
		}

		var dictionary = new ResourceDictionary();
		dictionary.ThemeDictionaries[LightBranchKey] = light;
		dictionary.ThemeDictionaries[DefaultBranchKey] = dark;
		return dictionary;
	}

	/// <summary>
	/// The accent-button fill per branch: seed tones 30/70 when a seed is
	/// active (agreeing with FluentAccentPalette), else the live platform
	/// shades (light fill = Dark1, dark fill = Light2 — spike S4). Null when
	/// the platform shades are unreachable (no XCR): the Filled background
	/// defaults are then skipped, graceful degradation.
	/// </summary>
	private static (Color? Light, Color? Dark) ResolveAccentFill(Color? seed)
	{
		if (seed is { } s)
		{
			var hct = HctColor.FromArgb((s.A << 24) | (s.R << 16) | (s.G << 8) | s.B);
			var palette = new TonalPalette(hct.Hue, hct.Chroma);
			return (FromArgb(palette.GetArgb(30)), FromArgb(palette.GetArgb(70)));
		}

		if (Application.Current?.Resources is { } resources
			&& resources.TryGetValue("SystemAccentColorDark1", out var dark1) && dark1 is Color lightFill
			&& resources.TryGetValue("SystemAccentColorLight2", out var light2) && light2 is Color darkFill)
		{
			return (lightFill, darkFill);
		}

		return (null, null);
	}

	private static ResourceDictionary BuildBranchDefaults(bool isLight, Color? accentFill)
	{
		var branch = new ResourceDictionary();

		Color Of(BranchValues values) => isLight ? values.Light : values.Dark;
		var transparent = Color.FromArgb(0, 0xFF, 0xFF, 0xFF);

		// Filled (accent button). Hover/pressed are the fill at 0.9/0.8 BRUSH
		// opacity — XCR's own structure (spike S4).
		if (accentFill is { } fill)
		{
			branch["FilledButtonBackground"] = new SolidColorBrush(fill);
			branch["FilledButtonBackgroundPointerOver"] = new SolidColorBrush(fill) { Opacity = 0.9 };
			branch["FilledButtonBackgroundPressed"] = new SolidColorBrush(fill) { Opacity = 0.8 };
		}
		branch["FilledButtonForeground"] = new SolidColorBrush(Of(_onAccentPrimary));
		branch["FilledButtonForegroundPointerOver"] = new SolidColorBrush(Of(_onAccentPrimary));
		branch["FilledButtonForegroundPressed"] = new SolidColorBrush(Of(_onAccentSecondary));
		// The stock accent-button border is an elevation gradient that cannot
		// be expressed as a semantic default; transparent is the documented
		// nearest value (rest/hover/pressed/disabled are all near-invisible).
		branch["FilledButtonBorderBrush"] = new SolidColorBrush(transparent);
		branch["FilledButtonBorderBrushPointerOver"] = new SolidColorBrush(transparent);
		branch["FilledButtonBorderBrushPressed"] = new SolidColorBrush(transparent);
		branch["FilledButtonBorderBrushDisabled"] = new SolidColorBrush(transparent);

		// Outlined (standard button), rest state.
		branch["OutlinedButtonBackground"] = new SolidColorBrush(Of(_controlFillDefault));
		branch["OutlinedButtonForeground"] = new SolidColorBrush(Of(_textPrimary));
		branch["OutlinedButtonBorderBrush"] = new SolidColorBrush(Of(_strongStroke));

		// Text / Icon (consumed by the shipped bridge styles via
		// {ThemeResource} setters). Fluent's subtle button uses neutral text.
		branch["TextButtonForeground"] = new SolidColorBrush(Of(_textPrimary));
		branch["TextButtonForegroundPointerOver"] = new SolidColorBrush(Of(_textPrimary));
		branch["TextButtonForegroundPressed"] = new SolidColorBrush(Of(_textSecondary));
		branch["TextButtonBackground"] = new SolidColorBrush(transparent);
		branch["TextButtonBackgroundPointerOver"] = new SolidColorBrush(transparent);
		branch["TextButtonBackgroundPressed"] = new SolidColorBrush(transparent);
		branch["TextButtonBorderBrush"] = new SolidColorBrush(transparent);
		branch["IconButtonForeground"] = new SolidColorBrush(Of(_textSecondary));

		// TextBox (both families — one Fluent TextBox), rest state.
		branch["FilledTextBoxBackground"] = new SolidColorBrush(Of(_controlFillDefault));
		branch["FilledTextBoxForeground"] = new SolidColorBrush(Of(_textPrimary));
		branch["FilledTextBoxPlaceholderForeground"] = new SolidColorBrush(Of(_textSecondary));
		branch["FilledTextBoxBorderBrush"] = new SolidColorBrush(Of(_strongStroke));
		branch["FilledTextBoxHeaderForeground"] = new SolidColorBrush(Of(_textPrimary));
		branch["OutlinedTextBoxForeground"] = new SolidColorBrush(Of(_textPrimary));
		branch["OutlinedTextBoxPlaceholderForeground"] = new SolidColorBrush(Of(_textSecondary));
		branch["OutlinedTextBoxBorderBrush"] = new SolidColorBrush(Of(_strongStroke));
		branch["OutlinedTextBoxHeaderForeground"] = new SolidColorBrush(Of(_textPrimary));
		branch["TextBoxDeleteButtonForeground"] = new SolidColorBrush(Of(_textSecondary));

		// CheckBox glyph (checked glyph sits on the accent fill).
		branch["CheckBoxGlyphForegroundChecked"] = new SolidColorBrush(Of(_onAccentPrimary));

		// ToggleSwitch ON family (accent-based; OFF-state neutrals are
		// re-point pass-through only — unverified per branch).
		if (accentFill is { } switchFill)
		{
			branch["ToggleSwitchOuterBorderFill"] = new SolidColorBrush(switchFill);
			branch["ToggleSwitchOuterBorderStroke"] = new SolidColorBrush(switchFill);
		}
		branch["ToggleSwitchKnobOnFill"] = new SolidColorBrush(Of(_onAccentPrimary));

		return branch;
	}

	/// <summary>
	/// Mirrors every semantic key found in the consumer override onto its
	/// Fluent per-control resource (spec 05 §10 steps 2–3): flat entries reach
	/// both branches, theme-branch entries only theirs. Values are written
	/// verbatim.
	/// </summary>
	private static void ApplyRepointing(ResourceDictionary light, ResourceDictionary dark, ResourceDictionary consumerOverride)
	{
		// Enumeration reads OWN entries only — TryGetValue would also search
		// the ambient theme branch and break branch fidelity.
		var flat = ToOwnEntries(consumerOverride);
		var lightOverrides = consumerOverride.ThemeDictionaries.TryGetValue(LightBranchKey, out var lo) && lo is ResourceDictionary lod
			? ToOwnEntries(lod)
			: null;
		var darkOverrides = consumerOverride.ThemeDictionaries.TryGetValue(DefaultBranchKey, out var dv) && dv is ResourceDictionary dod
			? ToOwnEntries(dod)
			: null;

		foreach (var (semantic, fluent) in _repointMap)
		{
			if (flat.TryGetValue(semantic, out var flatValue))
			{
				light[fluent] = flatValue;
				dark[fluent] = flatValue;
			}

			if (lightOverrides is { } && lightOverrides.TryGetValue(semantic, out var lightValue))
			{
				light[fluent] = lightValue;
			}

			if (darkOverrides is { } && darkOverrides.TryGetValue(semantic, out var darkValue))
			{
				dark[fluent] = darkValue;
			}
		}
	}

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

	private static Color Rgba(uint argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));

	private static Color FromArgb(int argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
}
