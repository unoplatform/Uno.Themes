using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.Themes.ColorGeneration;
using Uno.Themes.ColorGeneration.Hct;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the FluentTheme lightweight-styling bridge for Button
/// (specs/05-fluent-theme, §10, goal G6): the documented semantic lightweight
/// keys resolve with Fluent default values, consumer overrides re-point the
/// built-in Fluent per-control resources so XCR-templated buttons reflect
/// them, and without an override the stock rendering is untouched.
/// </summary>
[TestClass]
public class Given_FluentLightweightStyling
{
	private static readonly Color OverrideRed = Color.FromArgb(0xFF, 0xB0, 0x00, 0x20);
	private static readonly Color SeedPurple = Color.FromArgb(0xFF, 0x59, 0x46, 0xD2);

	private static bool IsAmbientDark =>
		Application.Current.RequestedTheme == ApplicationTheme.Dark;

	private static Color GetAmbientColor(string key)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(key, out var value) && value is Color,
			$"{key} should be resolvable from XamlControlsResources");
		return (Color)value;
	}

	/// <summary>The live platform accent-button fill for the ambient branch (spike S4).</summary>
	private static Color AmbientAccentFill =>
		GetAmbientColor(IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColorDark1");

	private static Grid CreateThemedContainer(FluentTheme? theme = null)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme ?? new FluentTheme());
		return container;
	}

	private static SolidColorBrush GetBrush(ResourceDictionary resources, string key)
	{
		Assert.IsTrue(
			resources.TryGetValue(key, out var value),
			$"{key} should resolve under FluentTheme");
		var brush = value as SolidColorBrush;
		Assert.IsNotNull(brush, $"{key} should be a SolidColorBrush");
		return brush;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Semantic key defaults: every bridged Button key resolves, and the
	// value-bearing ones carry the live Fluent token values (drift guard for
	// the captured neutral values declared in LightweightDefaults.xaml).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonBackground")]
	[DataRow("FilledButtonBackgroundPointerOver")]
	[DataRow("FilledButtonBackgroundPressed")]
	[DataRow("FilledButtonForeground")]
	[DataRow("FilledButtonForegroundPointerOver")]
	[DataRow("FilledButtonForegroundPressed")]
	[DataRow("FilledButtonBorderBrush")]
	[DataRow("FilledButtonBorderBrushPointerOver")]
	[DataRow("FilledButtonBorderBrushPressed")]
	[DataRow("FilledButtonBorderBrushDisabled")]
	[DataRow("OutlinedButtonBackground")]
	[DataRow("OutlinedButtonForeground")]
	[DataRow("OutlinedButtonBorderBrush")]
	[DataRow("TextButtonForeground")]
	[DataRow("TextButtonForegroundPointerOver")]
	[DataRow("TextButtonForegroundPressed")]
	[DataRow("TextButtonBackground")]
	[DataRow("TextButtonBorderBrush")]
	[DataRow("IconButtonForeground")]
	public void When_SemanticButtonKey_ResolvesAsBrush(string key)
	{
		var container = CreateThemedContainer();
		GetBrush(container.Resources, key);
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("OutlinedButtonForeground", "TextFillColorPrimary")]
	[DataRow("TextButtonForeground", "TextFillColorPrimary")]
	[DataRow("TextButtonForegroundPressed", "TextFillColorSecondary")]
	[DataRow("IconButtonForeground", "TextFillColorSecondary")]
	[DataRow("OutlinedButtonBorderBrush", "ControlStrongStrokeColorDefault")]
	[DataRow("OutlinedButtonBackground", "ControlFillColorDefault")]
	[DataRow("FilledTextBoxBackground", "ControlFillColorDefault")]
	[DataRow("FilledTextBoxForeground", "TextFillColorPrimary")]
	[DataRow("FilledTextBoxPlaceholderForeground", "TextFillColorSecondary")]
	[DataRow("FilledTextBoxHeaderForeground", "TextFillColorPrimary")]
	[DataRow("FilledTextBoxBorderBrush", "ControlStrongStrokeColorDefault")]
	[DataRow("OutlinedTextBoxForeground", "TextFillColorPrimary")]
	[DataRow("OutlinedTextBoxBorderBrush", "ControlStrongStrokeColorDefault")]
	[DataRow("OutlinedTextBoxPlaceholderForeground", "TextFillColorSecondary")]
	[DataRow("OutlinedTextBoxHeaderForeground", "TextFillColorPrimary")]
	[DataRow("TextBoxDeleteButtonForeground", "TextFillColorSecondary")]
	[DataRow("CheckBoxGlyphForegroundChecked", "TextOnAccentFillColorPrimary")]
	[DataRow("ToggleSwitchKnobOnFill", "TextOnAccentFillColorPrimary")]
	public void When_AmbientDefaults_MatchLiveTokens(string key, string token)
	{
		var container = CreateThemedContainer();

		Assert.AreEqual(GetAmbientColor(token), GetBrush(container.Resources, key).Color,
			$"{key} (baked capture in LightweightDefaults.xaml) must match the live {token} value — " +
			"an Uno.UI update likely changed the token; re-capture the values");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_NoSeed_FilledBackgroundDefaultIsPlatformAccentFill()
	{
		var container = CreateThemedContainer();

		Assert.AreEqual(AmbientAccentFill, GetBrush(container.Resources, "FilledButtonBackground").Color,
			"FilledButtonBackground must default to the platform accent-button fill (light: Dark1, dark: Light2)");
		Assert.AreEqual(0.9, GetBrush(container.Resources, "FilledButtonBackgroundPointerOver").Opacity, 0.001,
			"the hover fill is the rest fill at 90% brush opacity (XCR structure)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_Seeded_FilledBackgroundDefaultFollowsSeed()
	{
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { PrimarySeed = SeedPurple };
		var container = CreateThemedContainer(theme);

		var hct = HctColor.FromArgb((SeedPurple.A << 24) | (SeedPurple.R << 16) | (SeedPurple.G << 8) | SeedPurple.B);
		var palette = new TonalPalette(hct.Hue, hct.Chroma);
		var argb = palette.GetArgb(IsAmbientDark ? 70 : 30);
		var expected = Color.FromArgb(
			(byte)((argb >> 24) & 0xFF), (byte)((argb >> 16) & 0xFF), (byte)((argb >> 8) & 0xFF), (byte)(argb & 0xFF));

		Assert.AreEqual(expected, GetBrush(container.Resources, "FilledButtonBackground").Color,
			"under a seed, FilledButtonBackground must agree with the reverse accent mapping (tones 30/70)");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Bridge styles: TextButtonForeground reaches the shipped styles via the
	// {ThemeResource} setter — element-scope resolution, so a subtree-scoped
	// consumer override works without any re-pointing.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TextButtonStyled_ForegroundFollowsScopedOverride()
	{
		var container = CreateThemedContainer();
		container.Resources["TextButtonForeground"] = new SolidColorBrush(OverrideRed);

		var button = new Button
		{
			Content = "subtle",
			Style = (Style)container.Resources["TextButtonStyle"],
		};
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var foreground = button.Foreground as SolidColorBrush;
		Assert.IsNotNull(foreground, "the subtle button should have a SolidColorBrush foreground");
		Assert.AreEqual(OverrideRed, foreground.Color,
			"a subtree-scoped TextButtonForeground override must reach the bridge style's foreground");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TextButtonStyled_DefaultForegroundIsNeutralText()
	{
		var container = CreateThemedContainer();

		var button = new Button
		{
			Content = "subtle",
			Style = (Style)container.Resources["TextButtonStyle"],
		};
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var foreground = button.Foreground as SolidColorBrush;
		Assert.IsNotNull(foreground, "the subtle button should have a SolidColorBrush foreground");
		Assert.AreEqual(GetAmbientColor("TextFillColorPrimary"), foreground.Color,
			"without an override, the Fluent subtle button keeps neutral text");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Re-pointing (spec §10 steps 2–3): a consumer override of a semantic key
	// via Colors.OverrideDictionary reaches XCR-templated buttons in the
	// documented consumer topology; unbridged variants stay untouched.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButtonBackgroundOverridden_FluentButtonFollows()
	{
		var overrideDict = new ResourceDictionary
		{
			["FilledButtonBackground"] = new SolidColorBrush(OverrideRed),
		};
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			var filled = new Button
			{
				Content = "filled",
				Style = (Style)Application.Current.Resources["FilledButtonStyle"],
			};
			var outlined = new Button
			{
				Content = "outlined",
				Style = (Style)Application.Current.Resources["OutlinedButtonStyle"],
			};
			var panel = new StackPanel();
			panel.Children.Add(filled);
			panel.Children.Add(outlined);
			var host = new Grid();
			host.Children.Add(panel);

			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(filled);
			await UnitTestsUIContentHelper.WaitForIdle();

			var filledBackground = filled.Background as SolidColorBrush;
			Assert.IsNotNull(filledBackground, "the filled button should have a SolidColorBrush background");
			Assert.AreEqual(OverrideRed, filledBackground.Color,
				"a FilledButtonBackground override must reach the XCR-templated accent button (G6)");

			var outlinedBackground = outlined.Background as SolidColorBrush;
			Assert.IsNotNull(outlinedBackground, "the outlined button should have a SolidColorBrush background");
			Assert.AreNotEqual(OverrideRed, outlinedBackground.Color,
				"the standard button must not be affected by a Filled* override");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultedSemanticKeyOverridden_DirectLookupSeesOverride()
	{
		// The bridge's default layers (LightweightDefaults.xaml + the code
		// accent defaults) are merged ABOVE the colors layer that carries
		// Colors.OverrideDictionary. Without mirroring the override onto the
		// semantic key, a defaulted key would render correctly (via the
		// re-pointed per-control resource) while a DIRECT
		// {ThemeResource <semantic key>} lookup in app XAML still saw the
		// default — diverging from Material/Simple, whose defaults sit below
		// the colors layer so overrides win both ways.
		var overrideDict = new ResourceDictionary
		{
			// Accent-derived default (code bridge layer).
			["FilledButtonBackground"] = new SolidColorBrush(OverrideRed),
			// Neutral default (LightweightDefaults.xaml layer).
			["TextButtonForeground"] = new SolidColorBrush(OverrideRed),
		};
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var container = CreateThemedContainer(theme);

		Assert.AreEqual(OverrideRed, GetBrush(container.Resources, "FilledButtonBackground").Color,
			"a direct lookup of an overridden accent-derived semantic key must see the override, not the bridge default");
		Assert.AreEqual(OverrideRed, GetBrush(container.Resources, "TextButtonForeground").Color,
			"a direct lookup of an overridden neutral semantic key must see the override, not the declarative default");
	}

	// ─────────────────────────────────────────────────────────────────────
	// TextBox / CheckBox / ToggleSwitch re-pointing (Fluent has divergent
	// per-control key names for these); RadioButton and Slider need no
	// bridging — their semantic key names ARE WinUI's per-control keys.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_ToggleSwitchOuterBorderFill_DefaultsToAccentFill()
	{
		var container = CreateThemedContainer();

		Assert.AreEqual(AmbientAccentFill, GetBrush(container.Resources, "ToggleSwitchOuterBorderFill").Color,
			"the semantic ON-track fill must default to the platform accent fill");
		Assert.AreEqual(AmbientAccentFill, GetBrush(container.Resources, "ToggleSwitchOuterBorderStroke").Color,
			"the semantic ON-track stroke must default to the platform accent fill");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledTextBoxBackgroundOverridden_FluentTextBoxFollows()
	{
		var overrideDict = new ResourceDictionary
		{
			["FilledTextBoxBackground"] = new SolidColorBrush(OverrideRed),
		};
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			// Explicitly Fluent-styled: the host app's implicit TextBox style is
			// Simple's, whose template does not consume TextControl* resources.
			var textBox = new TextBox
			{
				Text = "styled",
				Style = (Style)Application.Current.Resources["DefaultTextBoxStyle"],
			};
			var host = new Grid();
			host.Children.Add(textBox);

			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(textBox);
			await UnitTestsUIContentHelper.WaitForIdle();

			var background = textBox.Background as SolidColorBrush;
			Assert.IsNotNull(background, "the text box should have a SolidColorBrush background");
			Assert.AreEqual(OverrideRed, background.Color,
				"a FilledTextBoxBackground override must reach the XCR-templated TextBox (G6)");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_BothTextBoxFamiliesOverridden_OutlinedWins()
	{
		var overrideDict = new ResourceDictionary
		{
			["FilledTextBoxForeground"] = new SolidColorBrush(SeedPurple),
			["OutlinedTextBoxForeground"] = new SolidColorBrush(OverrideRed),
		};
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var container = CreateThemedContainer(theme);

		Assert.AreEqual(OverrideRed, GetBrush(container.Resources, "TextControlForeground").Color,
			"Fluent has a single TextBox: when both semantic families are overridden, the Outlined value wins (documented)");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CheckBoxGlyphForegroundChecked", "CheckBoxCheckGlyphForegroundChecked")]
	[DataRow("ToggleSwitchKnobOnFill", "ToggleSwitchKnobFillOn")]
	[DataRow("ToggleSwitchOffOuterBorderFill", "ToggleSwitchFillOff")]
	public void When_DivergentKeyOverridden_FluentPerControlKeyFollows(string semanticKey, string fluentKey)
	{
		var overrideDict = new ResourceDictionary
		{
			[semanticKey] = new SolidColorBrush(OverrideRed),
		};
		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var container = CreateThemedContainer(theme);

		Assert.AreEqual(OverrideRed, GetBrush(container.Resources, fluentKey).Color,
			$"an override of {semanticKey} must be re-pointed onto {fluentKey}");
	}

	// Permanent guard: these semantic key names must keep existing NATIVELY in
	// XamlControlsResources (the bridge deliberately ships nothing for them) —
	// an Uno.UI rename would silently break consumer overrides.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("RadioButtonForeground")]
	[DataRow("RadioButtonOuterEllipseStroke")]
	[DataRow("RadioButtonOuterEllipseCheckedStroke")]
	[DataRow("RadioButtonOuterEllipseCheckedFill")]
	[DataRow("SliderTrackFill")]
	[DataRow("SliderTrackValueFill")]
	[DataRow("SliderThumbBackground")]
	[DataRow("SliderTickBarFill")]
	[DataRow("CheckBoxCheckBackgroundFillChecked")]
	[DataRow("CheckBoxCheckBackgroundStrokeUnchecked")]
	[DataRow("ToggleSwitchKnobFillOn")]
	[DataRow("ToggleSwitchFillOff")]
	[DataRow("TextControlBackground")]
	[DataRow("TextControlPlaceholderForeground")]
	public void When_NativeSemanticKey_IsProvidedByXcr(string key)
	{
		var xcr = new Microsoft.UI.Xaml.Controls.XamlControlsResources();

		Assert.IsTrue(xcr.TryGetValue(key, out var value) && value is not null,
			$"{key} must be provided natively by XamlControlsResources — a rename in Uno.UI breaks consumer overrides");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Branch-key fidelity: consumer overrides use the "Dark" branch key (the
	// documented ColorPaletteOverride.xaml shape) and rely on "Default" as the
	// universal fallback — the re-pointing must honor the native
	// ThemeDictionaries semantics (exact branch first, then "Default").
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_OverrideUsesDarkBranchKey_DarkBranchIsRepointed()
	{
		var overrideDict = new ResourceDictionary();
		var darkBranch = new ResourceDictionary();
		darkBranch["FilledButtonBackground"] = new SolidColorBrush(OverrideRed);
		overrideDict.ThemeDictionaries["Dark"] = darkBranch;

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		// The rendered re-point pipeline is covered by the flat and
		// 'Default'-branch tests; what needs guarding here is branch fidelity
		// for the "Dark" consumer key, which the CI host cannot render (the
		// ambient theme is fixed and element-theme flips do not re-branch
		// app-scope template resources on this target). Assert the produced
		// resource graph instead: the theme IS a ResourceDictionary — its
		// dark branch must now carry the re-pointed per-control resource, and
		// its light branch must not.
		var (light, dark) = FindBridgeBranches(theme, "AccentButtonBackground");

		Assert.IsNotNull(dark, "a 'Dark'-branch override must re-point AccentButtonBackground in the theme's dark branch");
		Assert.AreEqual(OverrideRed, ((SolidColorBrush)dark!).Color,
			"the dark branch must carry the override value verbatim (the documented consumer branch key)");
		Assert.IsNull(light, "a 'Dark'-branch override must not leak into the light branch");
	}

	/// <summary>
	/// Finds <paramref name="key"/> in the Light / Default branches across the
	/// theme's merged dynamic layers (own entries only, later layers win) —
	/// i.e. what theme-aware resolution would see per branch.
	/// </summary>
	private static (object? Light, object? Dark) FindBridgeBranches(FluentTheme theme, string key)
	{
		object? light = null;
		object? dark = null;
		foreach (var merged in theme.MergedDictionaries)
		{
			if (merged.ThemeDictionaries.TryGetValue("Light", out var lb) && lb is ResourceDictionary lightBranch)
			{
				foreach (var pair in lightBranch)
				{
					if (pair.Key as string == key)
					{
						light = pair.Value;
					}
				}
			}

			if (merged.ThemeDictionaries.TryGetValue("Default", out var db) && db is ResourceDictionary darkBranch)
			{
				foreach (var pair in darkBranch)
				{
					if (pair.Key as string == key)
					{
						dark = pair.Value;
					}
				}
			}
		}

		return (light, dark);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_OverrideUsesDefaultBranchOnly_LightThemedButtonFollows()
	{
		var overrideDict = new ResourceDictionary();
		var defaultBranch = new ResourceDictionary();
		defaultBranch["FilledButtonBackground"] = new SolidColorBrush(OverrideRed);
		overrideDict.ThemeDictionaries["Default"] = defaultBranch;

		var theme = new FluentTheme();
		theme.Colors = new ThemeColors { OverrideDictionary = overrideDict };

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			var lightButton = new Button
			{
				Content = "light",
				RequestedTheme = ElementTheme.Light,
				Style = (Style)Application.Current.Resources["FilledButtonStyle"],
			};
			var host = new Grid();
			host.Children.Add(lightButton);

			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(lightButton);
			await UnitTestsUIContentHelper.WaitForIdle();

			var background = lightButton.Background as SolidColorBrush;
			Assert.IsNotNull(background, "the light-themed button should have a SolidColorBrush background");
			Assert.AreEqual(OverrideRed, background.Color,
				"a 'Default'-branch-only override is the universal fallback and must reach the light branch too");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_NoOverride_StockAccentButtonIsUntouched()
	{
		var theme = new FluentTheme();

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		appDictionaries.Add(theme);
		try
		{
			var button = new Button
			{
				Content = "stock",
				Style = (Style)Application.Current.Resources["AccentButtonStyle"],
			};
			var host = new Grid();
			host.Children.Add(button);

			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			var background = button.Background as SolidColorBrush;
			Assert.IsNotNull(background, "the stock accent button should have a SolidColorBrush background");
			Assert.AreEqual(AmbientAccentFill, background.Color,
				"without an override the bridge must not re-point anything — stock Fluent rendering stays the platform's");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}
}
