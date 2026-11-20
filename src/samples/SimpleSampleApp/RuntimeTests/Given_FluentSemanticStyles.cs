using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the FluentTheme semantic style layer (specs/05-fluent-theme, §5):
/// every semantic style key resolves under FluentTheme — XAML aliases resolve to
/// the very XamlControlsResources instances, the subtle button styles derive from them, and
/// the gap keys (no public XCR key on Uno) ship as declarative empty styles that
/// keep the built-in default appearance.
///
/// Each test creates a local FluentTheme scoped to the test container (spec D14),
/// with XamlControlsResources merged at app scope by the sample host.
/// </summary>
[TestClass]
public class Given_FluentSemanticStyles
{
	private WeakReference<Button>? _unloadedButton;

#if HAS_UNO // WinUI's DatePickerFlyoutPresenter has no public constructor, so it cannot be instantiated there.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_DatePickerFlyoutPresenterSemanticStyleApplied_PreservesNativeTemplate(ElementTheme appearance)
	{
		var theme = new FluentTheme();
		Assert.IsTrue(theme.TryGetValue("DatePickerFlyoutPresenterStyle", out var value), "The shared presenter style key must resolve under Fluent.");
		var style = Assert.IsInstanceOfType<Style>(value);
		Assert.AreEqual(typeof(DatePickerFlyoutPresenter), style.TargetType);
		var presenter = new DatePickerFlyoutPresenter { Style = style, RequestedTheme = appearance };
		var host = new Grid();
		host.Resources.MergedDictionaries.Add(theme);
		host.Children.Add(presenter);
		UnitTestsUIContentHelper.Content = host;
		await UnitTestsUIContentHelper.WaitForLoaded(presenter);
		await UnitTestsUIContentHelper.WaitForIdle();
		Assert.IsNotNull(presenter.Template, "An explicit semantic style must preserve the built-in picker template.");
	}
#endif

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

		var semantic = Assert.IsInstanceOfType<Style>(container.Resources[semanticKey], $"{semanticKey} should resolve to a Style under FluentTheme");

		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(xcrKey, out var xcrValue),
			$"{xcrKey} should be provided by XamlControlsResources");
		Assert.AreSame(xcrValue, semantic,
			$"{semanticKey} should reference the same instance as {xcrKey}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Subtle button styles: setters-only, BasedOn the XCR standard button.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TextButtonStyle")]
	[DataRow("IconButtonStyle")]
	public void When_SubtleButtonStyle_DerivesFromTheBuiltInButtonStyle(string key)
	{
		// Container-scoped: the styles this library defines must resolve without an app-level theme.
		var container = CreateThemedContainer();

		var style = Assert.IsInstanceOfType<Style>(container.Resources[key], $"{key} should resolve");
		Assert.AreEqual(typeof(Button), style.TargetType);
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue("DefaultButtonStyle", out var defaultStyle),
			"DefaultButtonStyle should be provided by XamlControlsResources");

		var root = style;
		while (root.BasedOn is { } basedOn && !ReferenceEquals(basedOn, defaultStyle))
		{
			Assert.IsFalse(root.Setters.OfType<Setter>().Any(s => s.Property == Control.TemplateProperty), $"{key} must not re-template the button");
			root = basedOn;
		}
		Assert.AreSame(defaultStyle, root.BasedOn, $"{key} must derive from the XCR DefaultButtonStyle instance");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "#09000000")]
	[DataRow(ElementTheme.Dark, "#0FFFFFFF")]
	public async Task When_TextButtonPointerOver_UsesTheSubtleFill(ElementTheme appearance, string expected)
	{
		var container = CreateThemedContainer();
		container.RequestedTheme = appearance;
		var button = new Button { Content = "subtle", Style = (Style)container.Resources["TextButtonStyle"] };

		var fill = await GetPointerOverFill(container, button);

		Assert.AreEqual(expected, fill, "the subtle button hovers with SubtleFillColorSecondary, not the standard fill");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SeveralSubtleButtons_EachUsesTheSubtleFill()
	{
		// WinUI rejects a ResourceDictionary with two parents: every button must get its own.
		var container = CreateThemedContainer();
		container.RequestedTheme = ElementTheme.Light;
		var first = new Button { Content = "first", Style = (Style)container.Resources["TextButtonStyle"] };
		var second = new Button { Content = "second", Style = (Style)container.Resources["IconButtonStyle"] };
		container.Children.Add(first);

		var firstFill = await GetPointerOverFill(container, first);
		var secondFill = await GetPointerOverFill(container, second);

		Assert.AreEqual("#09000000", firstFill);
		Assert.AreEqual("#09000000", secondFill);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TextButtonResourcesAssignedAfterStyle_PointerOverUsesTheSubtleFill()
	{
		// XAML sets the Style attribute before a <Button.Resources> property element replaces the dictionary.
		var container = CreateThemedContainer();
		container.RequestedTheme = ElementTheme.Light;
		var button = new Button { Content = "subtle", Style = (Style)container.Resources["TextButtonStyle"] };
		button.Resources = new ResourceDictionary { ["UnrelatedKey"] = 1.0 };

		var fill = await GetPointerOverFill(container, button);

		Assert.AreEqual("#09000000", fill, "a replaced Resources dictionary must not drop the subtle fills");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_LoadedTextButtonSwitchesToTheStandardStyle_PointerOverUsesTheStandardFill()
	{
		var container = CreateThemedContainer();
		container.RequestedTheme = ElementTheme.Light;
		var standard = (Style)container.Resources["OutlinedButtonStyle"];
		var reference = new Button { Content = "reference", Style = standard };
		var expected = await GetPointerOverFill(container, reference);

		var button = new Button { Content = "switched", Style = (Style)container.Resources["TextButtonStyle"] };
		await GetPointerOverFill(container, button);
		VisualStateManager.GoToState(button, "Normal", false);
		button.Style = standard;

		var fill = await GetPointerOverFill(container, button);

		Assert.AreNotEqual("#09000000", expected, "precondition: the standard fill differs from the subtle one");
		Assert.AreEqual(expected, fill, "replacing the style on a loaded button must drop the subtle fills");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SubtleButtonUnloaded_ItIsCollected()
	{
		await LoadAndUnloadSubtleButton();
		await UnitTestsUIContentHelper.WaitForIdle();
		Collect();

		Assert.IsNotNull(_unloadedButton);
		Assert.IsFalse(_unloadedButton.TryGetTarget(out _), "the subtle button's Loaded subscription must not retain the button");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task LoadAndUnloadSubtleButton()
	{
		var container = CreateThemedContainer();
		var button = new Button { Content = "subtle", Style = (Style)container.Resources["TextButtonStyle"] };
		_unloadedButton = new WeakReference<Button>(button);
		await GetPointerOverFill(container, button);
		UnitTestsUIContentHelper.Content = new Grid();
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Collect()
	{
		// Leak guard: collect in a separate frame from the strong references.
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	private static async Task<string> GetPointerOverFill(Panel container, Button button)
	{
		if (!container.Children.Contains(button))
		{
			container.Children.Add(button);
		}

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();
		VisualStateManager.GoToState(button, "PointerOver", false);
		await UnitTestsUIContentHelper.WaitForIdle();

		var presenter = Assert.IsInstanceOfType<ContentPresenter>(VisualTreeHelper.GetChild(button, 0));
		var background = Assert.IsInstanceOfType<SolidColorBrush>(presenter.Background);
		return background.Color.ToString();
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

		var background = Assert.IsInstanceOfType<SolidColorBrush>(button.Background, "TextButtonStyle button should have a SolidColorBrush background");
		Assert.AreEqual(Colors.Transparent, background.Color,
			"the Fluent subtle button must be transparent at rest");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Gap keys (spec §5.2 Ⓜ set): no public XCR key on Uno — shipped as
	// declarative empty styles (SemanticStyles.xaml) that keep the control's
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
		var style = Assert.IsInstanceOfType<Style>(value, $"{semanticKey} should be a Style");
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

		var filledBg = Assert.IsInstanceOfType<SolidColorBrush>(filled.Background);
		var accentBg = Assert.IsInstanceOfType<SolidColorBrush>(accent.Background);
		var outlinedBg = Assert.IsInstanceOfType<SolidColorBrush>(outlined.Background);

		Assert.AreEqual(accentBg.Color, filledBg.Color,
			"FilledButtonStyle must render with the Fluent accent background");
		Assert.AreNotEqual(outlinedBg.Color, filledBg.Color,
			"accent and standard button backgrounds must differ");
	}
}
