using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that <see cref="CupertinoTheme"/> sits on the shared semantic system: Apple's palette
/// mapped onto the shared colour roles, live semantic brushes, seed colours and generated tokens.
/// </summary>
[TestClass]
public class Given_CupertinoTheme
{
	private const string ColorPalettePath = "ms-appx:///Uno.Cupertino.WinUI/Styles/Application/ColorPalette.xaml";

	private static Grid CreateThemedContainer(CupertinoTheme theme)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	private static T GetResource<T>(Grid container, string key)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is T typed)
		{
			return typed;
		}

		Assert.Fail($"Resource '{key}' not found or not of type {typeof(T).Name}");
		return default!;
	}

	private static Color Parse(string hex) =>
		Color.FromArgb(0xFF, Convert.ToByte(hex[1..3], 16), Convert.ToByte(hex[3..5], 16), Convert.ToByte(hex[5..7], 16));

	// ResourceDictionary.TryGetValue resolves ThemeDictionaries against the application theme, so the two
	// appearances are read from the palette's theme blocks directly (see specs/lessons.md).
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("PrimaryColor", "#0088FF", "#0091FF")]
	[DataRow("OnPrimaryColor", "#FFFFFF", "#FFFFFF")]
	[DataRow("PrimaryContainerColor", "#D9EDFF", "#001D33")]
	[DataRow("SecondaryColor", "#8E8E93", "#8E8E93")]
	[DataRow("TertiaryColor", "#6155F5", "#6D7CFF")]
	[DataRow("ErrorColor", "#FF383C", "#FF4245")]
	[DataRow("BackgroundColor", "#F2F2F7", "#000000")]
	[DataRow("SurfaceColor", "#FFFFFF", "#1C1C1E")]
	[DataRow("OnSurfaceVariantColor", "#8A8A8E", "#8D8D93")]
	[DataRow("OutlineColor", "#C6C6C8", "#38383A")]
	public void When_PaletteLoaded_Then_SharedRolesCarryAppleValues(string key, string light, string dark)
	{
		var palette = new ResourceDictionary { Source = new Uri(ColorPalettePath) };

		Assert.IsTrue(palette.ThemeDictionaries.TryGetValue("Light", out var lightBlock), "Light block missing");
		Assert.IsTrue(palette.ThemeDictionaries.TryGetValue("Default", out var darkBlock), "Default block missing");

		Assert.AreEqual(Parse(light), (Color)((ResourceDictionary)lightBlock)[key], $"{key} (Light)");
		Assert.AreEqual(Parse(dark), (Color)((ResourceDictionary)darkBlock)[key], $"{key} (Dark)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeLoaded_Then_SemanticBrushesFollowApplePalette()
	{
		var container = CreateThemedContainer(new CupertinoTheme());

		var primary = GetResource<Color>(container, "PrimaryColor");
		var brush = GetResource<SolidColorBrush>(container, "PrimaryBrush");

		// Light or Dark depending on the host; either way it is Apple's blue, not the shared M3 purple.
		Assert.IsTrue(primary == Parse("#0088FF") || primary == Parse("#0091FF"), $"PrimaryColor was {primary}");
		Assert.AreEqual(primary, brush.Color, "PrimaryBrush must be rewritten from the Cupertino palette");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimarySeedSet_Then_PrimaryBrushFollowsSeed()
	{
		var theme = new CupertinoTheme();
		var container = CreateThemedContainer(theme);
		var brush = GetResource<SolidColorBrush>(container, "PrimaryBrush");
		var unseeded = brush.Color;

		theme.Colors = new ThemeColors { PrimarySeed = Parse("#2E7D32") };

		Assert.AreNotEqual(unseeded, brush.Color, "the same brush instance must repaint from the seed palette");
		Assert.AreEqual(GetResource<Color>(container, "PrimaryColor"), brush.Color);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimarySeedSet_Then_AccentLegacyBrushFollowsOnTheSameInstance()
	{
		var theme = new CupertinoTheme();
		var container = CreateThemedContainer(theme);
		var accent = GetResource<SolidColorBrush>(container, "CupertinoBlueBrush");
		var green = GetResource<SolidColorBrush>(container, "CupertinoGreenBrush");
		var unseededGreen = green.Color;

		theme.Colors = new ThemeColors { PrimarySeed = Parse("#2E7D32") };

		Assert.AreSame(accent, GetResource<SolidColorBrush>(container, "CupertinoBlueBrush"), "the instance must survive a rebuild");
		Assert.AreEqual(GetResource<Color>(container, "PrimaryColor"), accent.Color, "the accent brush follows the seeded primary");
		Assert.AreEqual(unseededGreen, green.Color, "system colours that are not the accent stay put");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ConsumerOverridesCupertinoBlueColor_Then_ItBeatsTheSeed()
	{
		var overrides = new ResourceDictionary();
		overrides["CupertinoBlueColor"] = Parse("#123456");
		var theme = new CupertinoTheme { Colors = new ThemeColors { PrimarySeed = Parse("#2E7D32"), OverrideDictionary = overrides } };
		var container = CreateThemedContainer(theme);

		Assert.AreEqual(Parse("#123456"), GetResource<SolidColorBrush>(container, "CupertinoBlueBrush").Color);
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CupertinoBlueColor", "#0088FF", "#0091FF")]
	[DataRow("CupertinoRedColor", "#FF383C", "#FF4245")]
	[DataRow("CupertinoOrangeColor", "#FF8D28", "#FF9230")]
	[DataRow("CupertinoMintColor", "#00C8B3", "#00DAC3")]
	[DataRow("CupertinoCyanColor", "#00C0E8", "#3CD3FE")]
	[DataRow("CupertinoBrownColor", "#AC7F5E", "#B78A66")]
	[DataRow("LinkColor", "#0088FF", "#0091FF")]
	public void When_PaletteLoaded_Then_SystemColoursCarryThe2025Values(string key, string light, string dark) =>
		When_PaletteLoaded_Then_SharedRolesCarryAppleValues(key, light, dark);

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeLoaded_Then_DesignTokensGenerate()
	{
		var container = CreateThemedContainer(new CupertinoTheme { DefaultSpacing = 6, DefaultCornerRadius = 5 });

		Assert.AreEqual(12d, GetResource<double>(container, "Space200"));
		Assert.AreEqual(new CornerRadius(5), GetResource<CornerRadius>(container, "Radius100CornerRadius"));
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CupertinoButtonStyle")]
	[DataRow("CupertinoTextBoxStyle")]
	[DataRow("CupertinoToggleSwitchStyle")]
	[DataRow("CupertinoToolTipStyle")]
	public void When_ThemeLoaded_Then_ExistingStyleKeysResolve(string key)
	{
		var container = CreateThemedContainer(new CupertinoTheme());

		Assert.IsNotNull(GetResource<Style>(container, key).TargetType);
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle", typeof(Button))]
	[DataRow("TextButtonStyle", typeof(Button))]
	[DataRow("OutlinedTextBoxStyle", typeof(TextBox))]
	[DataRow("OutlinedPasswordBoxStyle", typeof(PasswordBox))]
	[DataRow("ComboBoxStyle", typeof(ComboBox))]
	[DataRow("ComboBoxItemStyle", typeof(ComboBoxItem))]
	[DataRow("CheckBoxStyle", typeof(CheckBox))]
	[DataRow("RadioButtonStyle", typeof(RadioButton))]
	[DataRow("ToggleSwitchStyle", typeof(ToggleSwitch))]
	[DataRow("SliderStyle", typeof(Slider))]
	[DataRow("HyperlinkButtonStyle", typeof(HyperlinkButton))]
	[DataRow("CalendarViewStyle", typeof(CalendarView))]
	[DataRow("CalendarDatePickerStyle", typeof(CalendarDatePicker))]
	[DataRow("DatePickerStyle", typeof(DatePicker))]
	[DataRow("ProgressBarStyle", typeof(ProgressBar))]
	public void When_ThemeLoaded_Then_SemanticStyleKeysResolve(string key, Type targetType)
	{
		var container = CreateThemedContainer(new CupertinoTheme());

		Assert.AreEqual(targetType, GetResource<Style>(container, key).TargetType);
	}

	// Sizes and weights are literals inside ThemeDictionaries, so a scoped lookup is a valid read of them;
	// the *FontFamily aliases are not (they resolve against the application scope) and are not asserted here.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLarge", 40d, "Bold")]
	[DataRow("DisplaySmall", 34d, "Normal")]
	[DataRow("HeadlineLarge", 28d, "Normal")]
	[DataRow("TitleMedium", 17d, "SemiBold")]
	[DataRow("BodyLarge", 17d, "Normal")]
	[DataRow("LabelLarge", 17d, "Medium")]
	[DataRow("CaptionSmall", 11d, "Normal")]
	public void When_ThemeLoaded_Then_TypeScaleCarriesAppleTextStyles(string slot, double size, string weight)
	{
		var container = CreateThemedContainer(new CupertinoTheme());

		Assert.AreEqual(size, GetResource<double>(container, slot + "FontSize"), "shadowed by SharedTypography?");
		Assert.AreEqual(weight, GetResource<string>(container, slot + "FontWeight"));
		Assert.AreEqual(0, GetResource<int>(container, slot + "CharacterSpacing"));
	}

	// The semantic contract: every key Simple aliases in its _Resources.xaml, plus the two Material-only keys
	// specs/10-cupertino-liquid-glass/token-and-style-mapping.md §6 commits Cupertino to. The keys live in
	// each theme's XAML, not in code, so this list is the head's copy — extend it when Simple gains a key.
	// MediaTransportControlsStyle is a documented gap, as it is in Simple.
	private static readonly string[] SemanticKeys =
	{
		"FilledButtonStyle", "FilledTonalButtonStyle", "OutlinedButtonStyle", "TextButtonStyle", "IconButtonStyle", "ElevatedButtonStyle",
		"TextToggleButtonStyle", "IconToggleButtonStyle",
		"FilledTextBoxStyle", "OutlinedTextBoxStyle", "FilledPasswordBoxStyle", "OutlinedPasswordBoxStyle",
		"ComboBoxStyle", "ComboBoxItemStyle",
		"CheckBoxStyle", "RadioButtonStyle", "ToggleSwitchStyle", "SliderStyle",
		"HyperlinkButtonStyle", "SecondaryHyperlinkButtonStyle",
		"ListViewStyle", "ListViewItemStyle", "ContentDialogStyle",
		"AppBarButtonStyle", "CommandBarStyle", "NavigationViewStyle", "NavigationViewItemStyle",
		"CalendarViewStyle", "CalendarDatePickerStyle", "DatePickerStyle",
		"ProgressBarStyle", "ProgressRingStyle", "PipsPagerStyle", "RatingControlStyle",
		"FlyoutPresenterStyle", "MenuFlyoutPresenterStyle", "MenuFlyoutItemStyle", "MenuFlyoutSeparatorStyle",
		"MenuFlyoutSubItemStyle", "ToggleMenuFlyoutItemStyle", "RadioMenuFlyoutItemStyle",
		"FabStyle", "SmallFabStyle", "LargeFabStyle",
		"SecondaryFabStyle", "SecondarySmallFabStyle", "SecondaryLargeFabStyle",
		"TertiaryFabStyle", "TertiarySmallFabStyle", "TertiaryLargeFabStyle",
		"SurfaceFabStyle", "SurfaceSmallFabStyle", "SurfaceLargeFabStyle",
		"DisplayLarge", "DisplayMedium", "DisplaySmall",
		"HeadlineLarge", "HeadlineMedium", "HeadlineSmall",
		"TitleLarge", "TitleMedium", "TitleSmall",
		"BodyLarge", "BodyMedium", "BodySmall",
		"LabelLarge", "LabelMedium", "LabelSmall", "LabelExtraSmall",
		"CaptionLarge", "CaptionMedium", "CaptionSmall",
	};

	// Keys Cupertino has no style for yet. This list only ever shrinks: the test below fails as soon as one
	// of these starts resolving, so an entry cannot outlive the work that makes it obsolete, and a key that
	// is neither resolvable nor listed here fails it too. Empty means the contract is met.
	private static readonly string[] PendingSemanticKeys =
	{
		// Phase 3 — core controls

		// Phase 4 — containers and navigation
		"ContentDialogStyle",
		"AppBarButtonStyle", "CommandBarStyle", "NavigationViewStyle", "NavigationViewItemStyle",
		"PipsPagerStyle", "RatingControlStyle",
	};

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeLoaded_Then_EverySemanticKeyResolvesOrIsTrackedAsPending()
	{
		var container = CreateThemedContainer(new CupertinoTheme());
		bool Resolves(string key) => container.Resources.TryGetValue(key, out var value) && value is Style;

		var missing = SemanticKeys.Except(PendingSemanticKeys).Where(k => !Resolves(k)).ToArray();
		var landed = PendingSemanticKeys.Where(Resolves).ToArray();
		var unknown = PendingSemanticKeys.Except(SemanticKeys).ToArray();

		Assert.AreEqual(0, missing.Length, $"semantic keys that neither resolve nor are tracked as pending: {string.Join(", ", missing)}");
		Assert.AreEqual(0, landed.Length, $"now resolve - remove them from PendingSemanticKeys: {string.Join(", ", landed)}");
		Assert.AreEqual(0, unknown.Length, $"pending keys that are not part of the contract: {string.Join(", ", unknown)}");
	}

	// Alias cascades resolve against the application scope (specs/lessons.md), so the typeface is asserted
	// on the application theme and on a realized control, never from a scoped container.
	[TestMethod]
	[RunsOnUIThread]
	public void When_AppUsesCupertinoTheme_Then_TypeScaleDerivesFromInterRoot()
	{
		Assert.IsInstanceOfType(Application.Current.GetTheme(), typeof(CupertinoTheme));

		foreach (var key in new[] { "DefaultFontFamily", "CupertinoFontFamily", "BodyLargeFontFamily", "DisplayLargeFontFamily", "LabelSmallFontFamily" })
		{
			Assert.IsTrue(Application.Current.Resources.TryGetValue(key, out var value), $"{key} missing");
			StringAssert.Contains(((FontFamily)value).Source, "Inter", $"{key} must derive from the Inter root");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_AppUsesCupertinoTheme_Then_RealizedButtonRendersInter()
	{
		var button = new Button { Content = "Inter", Style = (Style)Application.Current.Resources["CupertinoButtonStyle"] };
		try
		{
			UnitTestsUIContentHelper.Content = button;
			await UnitTestsUIContentHelper.WaitForLoaded(button);

			StringAssert.Contains(button.FontFamily.Source, "Inter");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// Rendered, not looked up: an implicit style is proven by what an unstyled control ends up with.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeLoaded_Then_UnstyledControlsPickUpCupertinoStyles()
	{
		var button = new Button { Content = "Plain" };
		var toggle = new ToggleSwitch();
		var container = CreateThemedContainer(new CupertinoTheme());
		container.Children.Add(new StackPanel { Children = { button, toggle } });

		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(button);

			var accent = GetResource<SolidColorBrush>(container, "CupertinoBlueBrush").Color;
			Assert.AreEqual(accent, (button.Foreground as SolidColorBrush)?.Color, "implicit Button must be the plain, accent-text style");
			Assert.IsNull((button.Background as SolidColorBrush)?.Color is { A: > 0 } ? button.Background : null, "plain buttons have no fill");

			Assert.IsTrue(container.Resources.TryGetValue(typeof(ToggleSwitch), out var implicitStyle) && implicitStyle is Style, "implicit ToggleSwitch style missing");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// Every key uno.toolkit.ui's Cupertino styles (TabBar, segmented controls) read from this library
	// without defining it themselves. Inventory taken from uno.toolkit.ui@d88a0c12: referenced keys minus
	// the keys its own Cupertino dictionaries declare, minus WinUI's system brushes. Resource keys are
	// public API; this pins the ones a shipped dependent is known to need.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CupertinoBlueBrush", typeof(SolidColorBrush))]
	[DataRow("CupertinoLabelBrush", typeof(SolidColorBrush))]
	[DataRow("CupertinoSystemBackgroundBrush", typeof(SolidColorBrush))]
	[DataRow("CupertinoTertiarySystemFillBrush", typeof(SolidColorBrush))]
	[DataRow("CupertinoBlueColor", typeof(Color))]
	[DataRow("CupertinoQuaternaryGrayColor", typeof(Color))]
	[DataRow("LabelColor", typeof(Color))]
	[DataRow("SystemBackgroundColor", typeof(Color))]
	public void When_ThemeLoaded_Then_KeysTheToolkitDependsOnResolve(string key, Type type)
	{
		var container = CreateThemedContainer(new CupertinoTheme());

		Assert.IsTrue(container.Resources.TryGetValue(key, out var value), $"{key} missing");
		Assert.IsInstanceOfType(value, type, key);
	}

	// Review finding (skeptic): an implicit TextBlock style that sets metrics overrides the FontSize a
	// template TextBlock would otherwise inherit from its control.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ControlSetsFontSize_Then_ImplicitTextBlockStyleDoesNotOverrideIt()
	{
		var label = new TextBlock { Text = "inherits" };
		var host = new ContentControl { FontSize = 13, Content = label };
		var container = CreateThemedContainer(new CupertinoTheme());
		container.Children.Add(host);

		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(label);

			Assert.AreEqual(13d, label.FontSize, "the implicit TextBlock style must not carry text metrics");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// Review finding (contract + skeptic): a constructor color override must reach the brushes, not only the colors.
	[TestMethod]
	[RunsOnUIThread]
	public void When_ColorOverridePassedToConstructor_Then_BrushesFollow()
	{
		var overrides = new ResourceDictionary();
		overrides["PrimaryColor"] = Parse("#123456");
		overrides["CupertinoGreenColor"] = Parse("#654321");
		var container = CreateThemedContainer(new CupertinoTheme(colorOverride: overrides));

		Assert.AreEqual(Parse("#123456"), GetResource<SolidColorBrush>(container, "PrimaryBrush").Color);
		Assert.AreEqual(Parse("#123456"), GetResource<SolidColorBrush>(container, "CupertinoBlueBrush").Color, "the accent follows PrimaryColor");
		Assert.AreEqual(Parse("#654321"), GetResource<SolidColorBrush>(container, "CupertinoGreenBrush").Color);
	}

	// Review finding (skeptic): with a seed the accent must not split between the color and the brush —
	// uno.toolkit.ui reads both CupertinoBlueColor and CupertinoBlueBrush.
	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimarySeedSet_Then_AccentColorKeysAgreeWithTheirBrushes()
	{
		var theme = new CupertinoTheme { Colors = new ThemeColors { PrimarySeed = Parse("#2E7D32") } };
		var container = CreateThemedContainer(theme);

		Assert.AreEqual(GetResource<SolidColorBrush>(container, "CupertinoBlueBrush").Color, GetResource<Color>(container, "CupertinoBlueColor"));
		Assert.AreEqual(GetResource<SolidColorBrush>(container, "CupertinoLinkBrush").Color, GetResource<Color>(container, "LinkColor"));
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SlotStylesApplied_Then_TextRendersTheTypeScaleAndFollowsTokenOverrides()
	{
		var container = CreateThemedContainer(new CupertinoTheme());
		var body = new TextBlock { Text = "Body", Style = GetResource<Style>(container, "BodyLarge") };
		var title = new TextBlock { Text = "Title", Style = GetResource<Style>(container, "TitleMedium") };
		var headline = new TextBlock { Text = "Headline", Style = GetResource<Style>(container, "CupertinoHeadline") };

		// A lightweight override of the slot token, scoped to one panel.
		var overridden = new TextBlock { Text = "Big body", Style = GetResource<Style>(container, "BodyLarge") };
		var scope = new StackPanel { Children = { overridden } };
		scope.Resources["BodyLargeFontSize"] = 21d;

		container.Children.Add(new StackPanel { Children = { body, title, headline, scope } });

		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(overridden);

			Assert.AreEqual(17d, body.FontSize);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Normal.Weight, body.FontWeight.Weight);
			Assert.AreEqual(17d, title.FontSize);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.SemiBold.Weight, title.FontWeight.Weight);

			// The Apple-named style is the slot plus its leading: nothing about it may have moved.
			Assert.AreEqual(17d, headline.FontSize);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.SemiBold.Weight, headline.FontWeight.Weight);
			Assert.AreEqual(22d, headline.LineHeight);

			Assert.AreEqual(21d, overridden.FontSize, "a scoped *FontSize override must reach the slot style");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
