using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Omarchy;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the semantic abstraction layer of the Omarchy theme: every design-system-agnostic
/// style key resolves to its Omarchy-specific style, the Omarchy styles resolve under both the
/// Light and the Dark theme, and the default filled/outlined buttons carry the flutter_omarchy
/// colors (filled(blue): normal.blue tint + bright.blue text; outline(white): normal.white
/// border + bright.white text).
/// </summary>
[TestClass]
public class Given_OmarchySemanticStyles
{
	private static Grid CreateThemedContainer()
	{
		var theme = new OmarchyTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Semantic key -> Omarchy style (see _Resources.xaml)
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle", "OmarchyFilledButtonStyle")]
	[DataRow("FilledTonalButtonStyle", "OmarchyFilledButtonWhiteStyle")]
	[DataRow("OutlinedButtonStyle", "OmarchyOutlinedButtonStyle")]
	[DataRow("TextButtonStyle", "OmarchyTextButtonStyle")]
	[DataRow("IconButtonStyle", "OmarchyIconButtonStyle")]
	[DataRow("FabStyle", "OmarchyIconButtonStyle")]
	[DataRow("TextToggleButtonStyle", "OmarchyTextToggleButtonStyle")]
	[DataRow("IconToggleButtonStyle", "OmarchyIconToggleButtonStyle")]
	[DataRow("FilledTextBoxStyle", "OmarchyFilledTextBoxStyle")]
	[DataRow("OutlinedTextBoxStyle", "OmarchyOutlinedTextBoxStyle")]
	[DataRow("FilledPasswordBoxStyle", "OmarchyFilledPasswordBoxStyle")]
	[DataRow("OutlinedPasswordBoxStyle", "OmarchyOutlinedPasswordBoxStyle")]
	[DataRow("ComboBoxStyle", "OmarchyComboBoxStyle")]
	[DataRow("ComboBoxItemStyle", "OmarchyComboBoxItemStyle")]
	[DataRow("CheckBoxStyle", "OmarchyCheckBoxStyle")]
	[DataRow("RadioButtonStyle", "OmarchyRadioButtonStyle")]
	[DataRow("ToggleSwitchStyle", "OmarchyToggleSwitchStyle")]
	[DataRow("SliderStyle", "OmarchySliderStyle")]
	[DataRow("HyperlinkButtonStyle", "OmarchyHyperlinkButtonStyle")]
	[DataRow("SecondaryHyperlinkButtonStyle", "OmarchySecondaryHyperlinkButtonStyle")]
	[DataRow("ListViewStyle", "OmarchyListViewStyle")]
	[DataRow("ListViewItemStyle", "OmarchyListViewItemStyle")]
	[DataRow("ContentDialogStyle", "OmarchyContentDialogStyle")]
	[DataRow("NavigationViewStyle", "OmarchyNavigationViewStyle")]
	[DataRow("NavigationViewItemStyle", "OmarchyNavigationViewItemStyle")]
	[DataRow("ProgressBarStyle", "OmarchyProgressBarStyle")]
	[DataRow("ProgressRingStyle", "OmarchyProgressRingStyle")]
	[DataRow("FlyoutPresenterStyle", "OmarchyFlyoutPresenterStyle")]
	[DataRow("MenuFlyoutPresenterStyle", "OmarchyMenuFlyoutPresenterStyle")]
	[DataRow("MenuFlyoutItemStyle", "OmarchyMenuFlyoutItemStyle")]
	[DataRow("MenuFlyoutSeparatorStyle", "OmarchyMenuFlyoutSeparatorStyle")]
	[DataRow("MenuFlyoutSubItemStyle", "OmarchyMenuFlyoutSubItemStyle")]
	[DataRow("ToggleMenuFlyoutItemStyle", "OmarchyToggleMenuFlyoutItemStyle")]
	[DataRow("RadioMenuFlyoutItemStyle", "OmarchyRadioMenuFlyoutItemStyle")]
	[DataRow("BodyLarge", "OmarchyBodyLarge")]
	[DataRow("TitleMedium", "OmarchyTitleMedium")]
	[DataRow("CaptionLarge", "OmarchyCaptionLarge")]
	public void When_SemanticKey_IsResolved_Then_ItIsTheOmarchyStyle(string semanticKey, string omarchyKey)
	{
		var container = CreateThemedContainer();

		var semanticStyle = container.Resources[semanticKey] as Style;
		var omarchyStyle = container.Resources[omarchyKey] as Style;

		Assert.IsNotNull(semanticStyle, $"{semanticKey} should resolve to a Style");
		Assert.IsNotNull(omarchyStyle, $"{omarchyKey} should resolve to a Style");

		// A <StaticResource> alias resolves through the ambient (application) scope, so the
		// instance reached through the semantic key is not guaranteed to be the one this
		// container's theme holds (see specs/lessons.md); compare the style's identity instead.
		Assert.AreEqual(omarchyStyle.TargetType, semanticStyle.TargetType, $"{semanticKey} should target the same type as {omarchyKey}");
		Assert.AreEqual(omarchyStyle.Setters.Count, semanticStyle.Setters.Count, $"{semanticKey} should carry the setters of {omarchyKey}");
		Assert.AreEqual(omarchyStyle.BasedOn?.TargetType, semanticStyle.BasedOn?.TargetType, $"{semanticKey} should derive like {omarchyKey}");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("ElevatedButtonStyle")]
	[DataRow("AppBarButtonStyle")]
	[DataRow("CommandBarStyle")]
	[DataRow("CalendarViewStyle")]
	[DataRow("DatePickerStyle")]
	[DataRow("PipsPagerStyle")]
	[DataRow("RatingControlStyle")]
	public void When_SemanticKey_HasNoOmarchyWidget_Then_ItIsNotDefined(string semanticKey)
	{
		var container = CreateThemedContainer();

		Assert.IsFalse(container.Resources.TryGetValue(semanticKey, out _), $"{semanticKey} is a documented GAP and must stay undefined");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Light + Dark resolution of the Omarchy styles and their base keys
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_FilledAndOutlinedButtons_AreRendered_Then_TheyCarryTheOmarchyColors(ElementTheme requestedTheme)
	{
		var container = CreateThemedContainer();
		container.RequestedTheme = requestedTheme;
		var palette = OmarchyPalettes.TokyoNight;

		var filled = new Button { Content = "filled", Style = (Style)container.Resources["FilledButtonStyle"] };
		var outlined = new Button { Content = "outline", Style = (Style)container.Resources["OutlinedButtonStyle"] };
		var red = new Button { Content = "red", Style = (Style)container.Resources["OmarchyFilledButtonRedStyle"] };

		var panel = new StackPanel();
		panel.Children.Add(filled);
		panel.Children.Add(outlined);
		panel.Children.Add(red);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(red);
		await UnitTestsUIContentHelper.WaitForIdle();

		// filled(blue): the tint source is normal.blue, the text bright.blue (button.dart).
		Assert.AreEqual(palette.Normal.Blue, ((SolidColorBrush)filled.Background).Color);
		Assert.AreEqual(palette.Bright.Blue, ((SolidColorBrush)filled.Foreground).Color);

		// outline(white): normal.white border, bright.white text.
		Assert.AreEqual(palette.Normal.White, ((SolidColorBrush)outlined.BorderBrush).Color);
		Assert.AreEqual(palette.Bright.White, ((SolidColorBrush)outlined.Foreground).Color);
		Assert.AreEqual(new Thickness(2), outlined.BorderThickness);
		Assert.AreEqual(new CornerRadius(0), outlined.CornerRadius, "Omarchy corners are sharp");

		// An ANSI accent variant only swaps the two colors.
		Assert.AreEqual(palette.Normal.Red, ((SolidColorBrush)red.Background).Color);
		Assert.AreEqual(palette.Bright.Red, ((SolidColorBrush)red.Foreground).Color);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_Text_IsRendered_Then_ItUsesTheBundledMonospaceFace()
	{
		var container = CreateThemedContainer();
		var text = new TextBlock { Text = "omarchy" };
		container.Children.Add(text);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(text);
		await UnitTestsUIContentHelper.WaitForIdle();

		StringAssert.Contains(text.FontFamily.Source, "CaskaydiaMonoNerdFontMono-Regular.ttf");
		Assert.AreEqual(14, text.FontSize, "Omarchy body text is 14 px");
		Assert.AreEqual(OmarchyPalettes.TokyoNight.Foreground, ((SolidColorBrush)text.Foreground).Color);
	}
}
