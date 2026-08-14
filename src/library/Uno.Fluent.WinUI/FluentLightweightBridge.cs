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
/// Builds the code-built half of the lightweight-styling bridge (spec 05 §10,
/// goal G6): the accent-derived semantic key defaults, and — when a consumer
/// override supplies a semantic key — the re-pointing of the corresponding
/// built-in Fluent per-control resource at the override's value so
/// Fluent-styled controls reflect it. The static neutral key defaults ship
/// declaratively in <c>Styles/Application/LightweightDefaults.xaml</c>, merged
/// by <see cref="FluentTheme"/> below this layer.
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
	private const string DarkBranchKey = "Dark";
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

	// Semantic keys with defaults but no per-control Fluent resource to
	// re-point (consumed by the shipped bridge styles via {ThemeResource}
	// setters): overrides are mirrored onto the semantic key only.
	private static readonly string[] _bridgeStyleKeys =
	{
		"TextButtonForeground",
		"TextButtonForegroundPointerOver",
		"TextButtonForegroundPressed",
		"TextButtonBackground",
		"TextButtonBackgroundPointerOver",
		"TextButtonBackgroundPressed",
		"TextButtonBorderBrush",
		"IconButtonForeground",
	};

	/// <summary>
	/// Builds the bridge dictionary: the accent-derived semantic key defaults
	/// per theme branch (the static neutral defaults ship declaratively in
	/// LightweightDefaults.xaml), plus per-control re-pointing entries for every
	/// semantic key found in <paramref name="consumerOverride"/>.
	/// </summary>
	/// <param name="seed">The effective primary seed, when one is active — the accent-fill defaults follow it (tones 30/70, matching FluentAccentPalette).</param>
	/// <param name="lightAccentBasis">The light-branch PrimaryColor override, when set — becomes the accent fill verbatim, taking precedence over the seed (matching FluentAccentPalette).</param>
	/// <param name="darkAccentBasis">The dark-branch PrimaryColor override, when set.</param>
	/// <param name="consumerOverride">The consumer's <c>Colors.OverrideDictionary</c>, when set.</param>
	internal static ResourceDictionary Build(Color? seed, Color? lightAccentBasis, Color? darkAccentBasis, ResourceDictionary? consumerOverride)
	{
		var (lightFill, darkFill) = ResolveAccentFill(seed, lightAccentBasis, darkAccentBasis);

		var light = BuildAccentDefaults(lightFill);
		var dark = BuildAccentDefaults(darkFill);

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
	/// The accent-button fill per branch: an explicit PrimaryColor override
	/// basis verbatim when present (agreeing with FluentAccentPalette's
	/// override-driven mode), else seed tones 30/70 when a seed is active,
	/// else the live platform shades (light fill = Dark1, dark fill = Light2 —
	/// spike S4). Null when the platform shades are unreachable (no XCR): the
	/// Filled background defaults are then skipped, graceful degradation.
	/// </summary>
	private static (Color? Light, Color? Dark) ResolveAccentFill(Color? seed, Color? lightBasis, Color? darkBasis)
	{
		Color? seedLight = null;
		Color? seedDark = null;
		if (seed is { } s && (lightBasis is null || darkBasis is null))
		{
			var hct = HctColor.FromArgb((s.A << 24) | (s.R << 16) | (s.G << 8) | s.B);
			var palette = new TonalPalette(hct.Hue, hct.Chroma);
			seedLight = FromArgb(palette.GetArgb(30));
			seedDark = FromArgb(palette.GetArgb(70));
		}

		var light = lightBasis ?? seedLight;
		var dark = darkBasis ?? seedDark;
		if (light is { } && dark is { })
		{
			return (light, dark);
		}

		if (Application.Current?.Resources is { } resources
			&& resources.TryGetValue("SystemAccentColorDark1", out var dark1) && dark1 is Color platformLight
			&& resources.TryGetValue("SystemAccentColorLight2", out var light2) && light2 is Color platformDark)
		{
			return (light ?? platformLight, dark ?? platformDark);
		}

		return (light, dark);
	}

	private static ResourceDictionary BuildAccentDefaults(Color? accentFill)
	{
		var branch = new ResourceDictionary();

		if (accentFill is { } fill)
		{
			// Filled (accent button). Hover/pressed are the fill at 0.9/0.8 BRUSH
			// opacity — XCR's own structure (spike S4).
			branch["FilledButtonBackground"] = new SolidColorBrush(fill);
			branch["FilledButtonBackgroundPointerOver"] = new SolidColorBrush(fill) { Opacity = 0.9 };
			branch["FilledButtonBackgroundPressed"] = new SolidColorBrush(fill) { Opacity = 0.8 };

			// ToggleSwitch ON track (OFF-state neutrals are re-point
			// pass-through only — unverified per branch).
			branch["ToggleSwitchOuterBorderFill"] = new SolidColorBrush(fill);
			branch["ToggleSwitchOuterBorderStroke"] = new SolidColorBrush(fill);
		}

		return branch;
	}

	/// <summary>
	/// Mirrors every semantic key found in the consumer override onto its
	/// Fluent per-control resource (spec 05 §10 steps 2–3): flat entries reach
	/// both branches; theme-branch entries follow the native ThemeDictionaries
	/// semantics — the exact branch key first ("Light" / "Dark"), then the
	/// universal "Default" fallback. Values are written verbatim, onto the
	/// Fluent key AND back onto the semantic key itself: this layer is merged
	/// above both the colors layer (where the override lives) and the
	/// declarative defaults, so without the semantic-key mirror a defaulted
	/// key would shadow the consumer's value on direct lookups — Material and
	/// Simple, whose defaults sit below the colors layer, let overrides win
	/// both ways (guarded by
	/// Given_FluentLightweightStyling.When_DefaultedSemanticKeyOverridden_DirectLookupSeesOverride).
	/// </summary>
	private static void ApplyRepointing(ResourceDictionary light, ResourceDictionary dark, ResourceDictionary consumerOverride)
	{
		// Enumeration reads OWN entries only — TryGetValue would also search
		// the ambient theme branch and break branch fidelity.
		var flat = ToOwnEntries(consumerOverride);
		var lightOverrides = BranchEntries(consumerOverride, LightBranchKey);
		var darkOverrides = BranchEntries(consumerOverride, DarkBranchKey);
		var fallbackOverrides = BranchEntries(consumerOverride, DefaultBranchKey);

		foreach (var (semantic, fluent) in _repointMap)
		{
			Apply(semantic, fluent);
		}

		foreach (var semantic in _bridgeStyleKeys)
		{
			Apply(semantic, fluent: null);
		}

		void Apply(string semantic, string? fluent)
		{
			if (flat.TryGetValue(semantic, out var flatValue))
			{
				Write(light, flatValue);
				Write(dark, flatValue);
			}

			var lightValue = OwnValue(lightOverrides, semantic) ?? OwnValue(fallbackOverrides, semantic);
			if (lightValue is { })
			{
				Write(light, lightValue);
			}

			var darkValue = OwnValue(darkOverrides, semantic) ?? OwnValue(fallbackOverrides, semantic);
			if (darkValue is { })
			{
				Write(dark, darkValue);
			}

			void Write(ResourceDictionary branch, object value)
			{
				if (fluent is { })
				{
					branch[fluent] = value;
				}

				branch[semantic] = value;
			}
		}
	}

	private static Dictionary<string, object>? BranchEntries(ResourceDictionary dictionary, string branchKey)
		=> dictionary.ThemeDictionaries.TryGetValue(branchKey, out var value) && value is ResourceDictionary branch
			? ToOwnEntries(branch)
			: null;

	private static object? OwnValue(Dictionary<string, object>? entries, string key)
		=> entries is { } && entries.TryGetValue(key, out var value) ? value : null;

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

	private static Color FromArgb(int argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
}
