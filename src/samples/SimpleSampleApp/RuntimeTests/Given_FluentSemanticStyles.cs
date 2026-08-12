using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the FluentTheme semantic style layer (specs/05-fluent-theme, §5):
/// every semantic style key resolves under FluentTheme — XAML aliases resolve to
/// the very XamlControlsResources instances, bridge styles derive from them, and
/// the gap keys (no public XCR key on Uno) ship as declarative empty styles that
/// keep the built-in default appearance.
///
/// Each test creates a local FluentTheme scoped to the test container (spec D14),
/// with XamlControlsResources merged at app scope by the sample host.
/// </summary>
[TestClass]
public class Given_FluentSemanticStyles
{
	private static Grid CreateThemedContainer()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new FluentTheme());
		return container;
	}

	// ─────────────────────────────────────────────────────────────────────
	// XAML aliases resolve to the same instance as the XCR style they target
	// (spec §5.2; mechanism A validated by Given_FluentAliasResolution).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle", "AccentButtonStyle")]
	[DataRow("ElevatedButtonStyle", "DefaultButtonStyle")]
	[DataRow("FilledTonalButtonStyle", "DefaultButtonStyle")]
	[DataRow("OutlinedButtonStyle", "DefaultButtonStyle")]
	[DataRow("FabStyle", "AccentButtonStyle")]
	[DataRow("SmallFabStyle", "AccentButtonStyle")]
	[DataRow("LargeFabStyle", "AccentButtonStyle")]
	[DataRow("SecondaryFabStyle", "DefaultButtonStyle")]
	[DataRow("SecondarySmallFabStyle", "DefaultButtonStyle")]
	[DataRow("SecondaryLargeFabStyle", "DefaultButtonStyle")]
	[DataRow("TertiaryFabStyle", "DefaultButtonStyle")]
	[DataRow("TertiarySmallFabStyle", "DefaultButtonStyle")]
	[DataRow("TertiaryLargeFabStyle", "DefaultButtonStyle")]
	[DataRow("SurfaceFabStyle", "DefaultButtonStyle")]
	[DataRow("SurfaceSmallFabStyle", "DefaultButtonStyle")]
	[DataRow("SurfaceLargeFabStyle", "DefaultButtonStyle")]
	[DataRow("TextToggleButtonStyle", "DefaultToggleButtonStyle")]
	[DataRow("IconToggleButtonStyle", "DefaultToggleButtonStyle")]
	[DataRow("FilledTextBoxStyle", "DefaultTextBoxStyle")]
	[DataRow("OutlinedTextBoxStyle", "DefaultTextBoxStyle")]
	[DataRow("FilledPasswordBoxStyle", "DefaultPasswordBoxStyle")]
	[DataRow("OutlinedPasswordBoxStyle", "DefaultPasswordBoxStyle")]
	[DataRow("ComboBoxStyle", "DefaultComboBoxStyle")]
	[DataRow("ComboBoxItemStyle", "DefaultComboBoxItemStyle")]
	[DataRow("CheckBoxStyle", "DefaultCheckBoxStyle")]
	[DataRow("RadioButtonStyle", "DefaultRadioButtonStyle")]
	[DataRow("ToggleSwitchStyle", "DefaultToggleSwitchStyle")]
	[DataRow("SliderStyle", "DefaultSliderStyle")]
	[DataRow("HyperlinkButtonStyle", "DefaultHyperlinkButtonStyle")]
	[DataRow("SecondaryHyperlinkButtonStyle", "DefaultHyperlinkButtonStyle")]
	[DataRow("ListViewItemStyle", "DefaultListViewItemStyle")]
	[DataRow("ContentDialogStyle", "DefaultContentDialogStyle")]
	[DataRow("AppBarButtonStyle", "DefaultAppBarButtonStyle")]
	[DataRow("CalendarViewStyle", "DefaultCalendarViewStyle")]
	[DataRow("DatePickerStyle", "DefaultDatePickerStyle")]
	[DataRow("ProgressBarStyle", "DefaultProgressBarStyle")]
	[DataRow("MediaTransportControlsStyle", "DefaultMediaTransportControlsStyle")]
	[DataRow("FlyoutPresenterStyle", "DefaultFlyoutPresenterStyle")]
	[DataRow("MenuFlyoutPresenterStyle", "DefaultMenuFlyoutPresenterStyle")]
	[DataRow("MenuFlyoutItemStyle", "DefaultMenuFlyoutItemStyle")]
	[DataRow("MenuFlyoutSubItemStyle", "DefaultMenuFlyoutSubItemStyle")]
	[DataRow("ToggleMenuFlyoutItemStyle", "DefaultToggleMenuFlyoutItemStyle")]
	[DataRow("RadioMenuFlyoutItemStyle", "DefaultRadioMenuFlyoutItemStyle")]
	public void When_SemanticAlias_ResolvesToXcrStyleInstance(string semanticKey, string xcrKey)
	{
		var container = CreateThemedContainer();

		var semantic = container.Resources[semanticKey] as Style;
		Assert.IsNotNull(semantic, $"{semanticKey} should resolve to a Style under FluentTheme");

		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(xcrKey, out var xcrValue),
			$"{xcrKey} should be provided by XamlControlsResources");
		Assert.AreSame(xcrValue, semantic,
			$"{semanticKey} should reference the same instance as {xcrKey}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Bridge styles (spec §5.4): setters-only, BasedOn the XCR standard button.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TextButtonStyle", "FluentTextButtonStyle")]
	[DataRow("IconButtonStyle", "FluentIconButtonStyle")]
	public void When_BridgeStyleAlias_ResolvesToShippedBridgeStyle(string semanticKey, string bridgeKey)
	{
		var container = CreateThemedContainer();

		var semantic = container.Resources[semanticKey] as Style;
		var bridge = container.Resources[bridgeKey] as Style;
		Assert.IsNotNull(semantic, $"{semanticKey} should resolve");
		Assert.IsNotNull(bridge, $"{bridgeKey} should resolve");
		Assert.AreSame(bridge, semantic, $"{semanticKey} should alias {bridgeKey}");

		Assert.AreEqual(typeof(Button), bridge.TargetType);
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue("DefaultButtonStyle", out var defaultStyle),
			"DefaultButtonStyle should be provided by XamlControlsResources");
		Assert.AreSame(defaultStyle, bridge.BasedOn,
			$"{bridgeKey} must be BasedOn the XCR DefaultButtonStyle instance (never a re-templated fork)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TextButtonStyleApplied_RestBackgroundIsTransparent()
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

		var background = button.Background as SolidColorBrush;
		Assert.IsNotNull(background, "TextButtonStyle button should have a SolidColorBrush background");
		Assert.AreEqual(Colors.Transparent, background.Color,
			"the Fluent subtle button must be transparent at rest");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Gap keys (spec §5.2 Ⓜ set): no public XCR key on Uno — shipped as
	// declarative empty styles (_Resources.xaml) that keep the control's
	// built-in default appearance (D8: nearest-match over GAP; the key must
	// resolve on every platform where the control exists).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("ProgressRingStyle", typeof(MUXC.ProgressRing))]
	[DataRow("ListViewStyle", typeof(ListView))]
	[DataRow("CommandBarStyle", typeof(CommandBar))]
	[DataRow("CalendarDatePickerStyle", typeof(CalendarDatePicker))]
	[DataRow("PipsPagerStyle", typeof(MUXC.PipsPager))]
	[DataRow("RatingControlStyle", typeof(RatingControl))]
	[DataRow("MenuFlyoutSeparatorStyle", typeof(MenuFlyoutSeparator))]
	[DataRow("NavigationViewStyle", typeof(MUXC.NavigationView))]
	[DataRow("NavigationViewItemStyle", typeof(MUXC.NavigationViewItem))]
	public void When_GapKey_ResolvesToStyleOfExpectedTargetType(string semanticKey, Type targetType)
	{
		var container = CreateThemedContainer();

		Assert.IsTrue(
			container.Resources.TryGetValue(semanticKey, out var value),
			$"{semanticKey} should resolve under FluentTheme");
		var style = value as Style;
		Assert.IsNotNull(style, $"{semanticKey} should be a Style");
		Assert.AreEqual(targetType, style.TargetType,
			$"{semanticKey} should target {targetType.Name}");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_GapKeyStyleApplied_ControlRendersWithDefaultTemplate()
	{
		var container = CreateThemedContainer();

		// The empty style must keep the built-in default template: an
		// explicitly styled ProgressRing should still load and realize a template.
		var ring = new MUXC.ProgressRing
		{
			IsActive = false,
			Style = (Style)container.Resources["ProgressRingStyle"],
		};
		container.Children.Add(ring);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(ring);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.IsTrue(ring.IsLoaded, "ProgressRing styled with ProgressRingStyle should load");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Accent / standard visual split (spec §12): the semantic Filled button is
	// the accent button, visually distinct from the standard (Outlined) one.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledAndOutlinedApplied_AccentSplitIsVisible()
	{
		var container = CreateThemedContainer();

		var filled = new Button { Content = "filled", Style = (Style)container.Resources["FilledButtonStyle"] };
		var accent = new Button { Content = "accent", Style = (Style)Application.Current.Resources["AccentButtonStyle"] };
		var outlined = new Button { Content = "outlined", Style = (Style)container.Resources["OutlinedButtonStyle"] };

		var panel = new StackPanel();
		panel.Children.Add(filled);
		panel.Children.Add(accent);
		panel.Children.Add(outlined);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(filled);
		await UnitTestsUIContentHelper.WaitForLoaded(outlined);
		await UnitTestsUIContentHelper.WaitForIdle();

		var filledBg = filled.Background as SolidColorBrush;
		var accentBg = accent.Background as SolidColorBrush;
		var outlinedBg = outlined.Background as SolidColorBrush;
		Assert.IsNotNull(filledBg);
		Assert.IsNotNull(accentBg);
		Assert.IsNotNull(outlinedBg);

		Assert.AreEqual(accentBg.Color, filledBg.Color,
			"FilledButtonStyle must render with the Fluent accent background");
		Assert.AreNotEqual(outlinedBg.Color, filledBg.Color,
			"accent and standard button backgrounds must differ");
	}
}
