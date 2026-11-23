#nullable enable

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Interactive states of the Simple controls must stay on the control's own colour role: the
/// fill keeps the resting role and the state is a tint over it, so whatever sits on the fill (a
/// label, a check glyph, a switch knob) still pairs with it. Regression guard for
/// unoplatform/Uno.Themes#1784, where a coloured palette turned a filled button's hover fill dark
/// under a dark label, and a tonal button switched to the tertiary colour while pressed. The same
/// misuse of <c>PrimaryVariantDark</c> / <c>TertiaryContainer</c> as a state colour was swept from
/// the other Simple controls.
/// </summary>
[TestClass]
public class Given_InteractiveStateColors
{
	// The palette from the issue: three seeds of unrelated hues, plus a dark tertiary container
	// override, so a state that borrows another role shows up as a different colour.
	private static readonly Color PrimarySeed = Color.FromArgb(0xFF, 0x6F, 0x4E, 0x37);
	private static readonly Color SecondarySeed = Color.FromArgb(0xFF, 0xB0, 0x7D, 0x4F);
	private static readonly Color TertiarySeed = Color.FromArgb(0xFF, 0x4F, 0x6F, 0x52);
	private static readonly Color DarkTertiaryContainer = Color.FromArgb(0xFF, 0x34, 0x50, 0x3A);

	// WCAG AA for large text and UI components. A state layer may cost the label a little
	// contrast; the bug costs nearly all of it (about 1.2:1 on the filled button in dark).
	private const double MinimumStateContrast = 3.0;

	/// <summary>How to build a control, drive it into a state and read what it paints.</summary>
	/// <param name="Fill">Element whose fill must keep the resting role.</param>
	/// <param name="Layer">Element tinting the fill in the state, if any.</param>
	/// <param name="Label">Element drawn on the fill, if any; must stay readable on fill + tint.</param>
	/// <param name="RestState">State holding the resting fill, when it is not the one the control loads in
	/// (a selected item rests in Selected; a menu item's highlight is its PointerOver).</param>
	private sealed record Scenario(Func<Control> Create, string State, string Fill, string? Layer, string? Label, string? RestState = null);

	private static Style StyleOf(string key) => (Style)Application.Current.Resources[key];

	private static Scenario GetScenario(string id) => id switch
	{
		"FilledButton.PointerOver" => Button("FilledButtonStyle", "PointerOver"),
		"FilledButton.Pressed" => Button("FilledButtonStyle", "Pressed"),
		"FilledTonalButton.PointerOver" => Button("FilledTonalButtonStyle", "PointerOver"),
		"FilledTonalButton.Pressed" => Button("FilledTonalButtonStyle", "Pressed"),
		"IconButton.PointerOver" => Button("IconButtonStyle", "PointerOver"),
		"IconButton.Pressed" => Button("IconButtonStyle", "Pressed"),
		"IconButtonNeutral.PointerOver" => Button("SimpleIconButtonNeutralStyle", "PointerOver"),
		"IconButtonNeutral.Pressed" => Button("SimpleIconButtonNeutralStyle", "Pressed"),

		"TextToggleButton.Pressed" => Toggle(isChecked: false, "Pressed"),
		"IconToggleButton.CheckedPressed" => new(() => new ToggleButton { Content = "\uE734", IsChecked = true, Style = StyleOf("IconToggleButtonStyle") }, "CheckedPressed", "Root", "StateLayer", "ContentPresenter"),
		"TextToggleButton.CheckedPointerOver" => Toggle(isChecked: true, "CheckedPointerOver"),
		"TextToggleButton.CheckedPressed" => Toggle(isChecked: true, "CheckedPressed"),

		"CheckBox.CheckedPressed" => CheckBox(isChecked: true, "CheckedPressed", "CheckGlyph"),
		"CheckBox.UncheckedPressed" => CheckBox(isChecked: false, "UncheckedPressed", null),
		"CheckBox.IndeterminatePressed" => CheckBox(isChecked: null, "IndeterminatePressed", "IndeterminateGlyph"),

		"ComboBoxItem.SelectedPointerOver" => Item(() => new ComboBoxItem { Content = "Espresso", Style = StyleOf("SimpleComboBoxItemStyle") }, "SelectedPointerOver", "LayoutRoot", "Selected"),
		"ComboBoxItem.SelectedPressed" => Item(() => new ComboBoxItem { Content = "Espresso", Style = StyleOf("SimpleComboBoxItemStyle") }, "SelectedPressed", "LayoutRoot", "Selected"),
		"ComboBoxItem.PointerOver" => Item(() => new ComboBoxItem { Content = "Espresso", Style = StyleOf("SimpleComboBoxItemStyle") }, "PointerOver", "LayoutRoot", null),
		"ComboBoxItem.Pressed" => Item(() => new ComboBoxItem { Content = "Espresso", Style = StyleOf("SimpleComboBoxItemStyle") }, "Pressed", "LayoutRoot", null),
		"ListViewItem.Pressed" => Item(() => new ListViewItem { Content = "Espresso", Style = StyleOf("SimpleListViewItemStyle") }, "Pressed", "ContentBorder", null),
		"ListViewItem.PointerOverSelected" => Item(() => new ListViewItem { Content = "Espresso", Style = StyleOf("SimpleListViewItemStyle") }, "PointerOverSelected", "ContentBorder", "Selected"),
		"ListViewItem.PressedSelected" => Item(() => new ListViewItem { Content = "Espresso", Style = StyleOf("SimpleListViewItemStyle") }, "PressedSelected", "ContentBorder", "Selected"),

		"MenuFlyoutItem.Pressed" => new(() => new MenuFlyoutItem { Text = "Add to order", Style = StyleOf("SimpleMenuFlyoutItemStyle") }, "Pressed", "LayoutRoot", "StateLayer", "TextBlock", "PointerOver"),
		"ToggleMenuFlyoutItem.Pressed" => new(() => new ToggleMenuFlyoutItem { Text = "Oat milk", Style = StyleOf("SimpleToggleMenuFlyoutItemStyle") }, "Pressed", "LayoutRoot", "StateLayer", "TextBlock", "PointerOver"),
		"MenuFlyoutSubItem.Pressed" => new(() => new MenuFlyoutSubItem { Text = "Size", Style = StyleOf("SimpleMenuFlyoutSubItemStyle") }, "Pressed", "LayoutRoot", "StateLayer", "TextBlock", "PointerOver"),
		"RadioMenuFlyoutItem.Pressed" => new(() => new RadioMenuFlyoutItem { Text = "Large", Style = StyleOf("SimpleRadioMenuFlyoutItemStyle") }, "Pressed", "LayoutRoot", "StateLayer", "TextBlock", "PointerOver"),

		"ToggleSwitch.Pressed" => new(() => new ToggleSwitch { IsOn = true, Style = StyleOf("SimpleToggleSwitchStyle") }, "Pressed", "SwitchKnobBounds", "SwitchStateLayer", "KnobOn"),
		"ToggleSwitch.OffPressed" => new(() => new ToggleSwitch { IsOn = false, Style = StyleOf("SimpleToggleSwitchStyle") }, "Pressed", "OuterBorder", "SwitchStateLayer", null),

		"ExpanderHeader.PointerOver" => new(() => new ToggleButton { Content = "Details", Style = StyleOf("SimpleExpanderHeaderDownStyle") }, "PointerOver", "ToggleButtonGrid", "StateLayer", "ContentPresenter"),
		"ExpanderHeader.Pressed" => new(() => new ToggleButton { Content = "Details", Style = StyleOf("SimpleExpanderHeaderDownStyle") }, "Pressed", "ToggleButtonGrid", "StateLayer", "ContentPresenter"),
		"ExpanderHeader.CheckedPressed" => new(() => new ToggleButton { Content = "Details", IsChecked = true, Style = StyleOf("SimpleExpanderHeaderDownStyle") }, "CheckedPressed", "ToggleButtonGrid", "StateLayer", "ContentPresenter"),
		"ExpanderHeaderUp.Pressed" => new(() => new ToggleButton { Content = "Details", Style = StyleOf("SimpleExpanderHeaderUpStyle") }, "Pressed", "ToggleButtonGrid", "StateLayer", "ContentPresenter"),

		"Slider.Pressed" => new(() => new Slider { Value = 40, Width = 200, Style = StyleOf("SimpleSliderStyle") }, "Pressed", "HorizontalThumb", null, null),

		_ => throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown scenario"),
	};

	private static Scenario Button(string styleKey, string state)
		=> new(() => new Button { Content = "Add to order", Style = StyleOf(styleKey) }, state, "Root", "StateLayer", "ContentPresenter");

	private static Scenario Toggle(bool isChecked, string state)
		=> new(() => new ToggleButton { Content = "Oat milk", IsChecked = isChecked, Style = StyleOf("SimpleTextToggleButtonStyle") }, state, "Root", "StateLayer", "ContentPresenter");

	private static Scenario CheckBox(bool? isChecked, string state, string? glyph)
		=> new(() => new CheckBox { Content = "Extra shot", IsThreeState = true, IsChecked = isChecked, Style = StyleOf("SimpleCheckBoxStyle") }, state, "CheckBoxBorder", "CheckBoxStateLayer", glyph);

	private static Scenario Item(Func<Control> create, string state, string root, string? restState)
		=> new(create, state, root, "StateLayer", "ContentPresenter", restState);

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButton.PointerOver", ElementTheme.Light)]
	[DataRow("FilledButton.PointerOver", ElementTheme.Dark)]
	[DataRow("FilledButton.Pressed", ElementTheme.Light)]
	[DataRow("FilledButton.Pressed", ElementTheme.Dark)]
	[DataRow("FilledTonalButton.PointerOver", ElementTheme.Light)]
	[DataRow("FilledTonalButton.PointerOver", ElementTheme.Dark)]
	[DataRow("FilledTonalButton.Pressed", ElementTheme.Light)]
	[DataRow("FilledTonalButton.Pressed", ElementTheme.Dark)]
	[DataRow("IconButton.PointerOver", ElementTheme.Dark)]
	[DataRow("IconButton.Pressed", ElementTheme.Dark)]
	[DataRow("IconButtonNeutral.PointerOver", ElementTheme.Dark)]
	[DataRow("IconButtonNeutral.Pressed", ElementTheme.Dark)]
	[DataRow("TextToggleButton.Pressed", ElementTheme.Dark)]
	[DataRow("IconToggleButton.CheckedPressed", ElementTheme.Dark)]
	[DataRow("TextToggleButton.CheckedPointerOver", ElementTheme.Light)]
	[DataRow("TextToggleButton.CheckedPointerOver", ElementTheme.Dark)]
	[DataRow("TextToggleButton.CheckedPressed", ElementTheme.Dark)]
	[DataRow("CheckBox.CheckedPressed", ElementTheme.Light)]
	[DataRow("CheckBox.CheckedPressed", ElementTheme.Dark)]
	[DataRow("CheckBox.IndeterminatePressed", ElementTheme.Dark)]
	[DataRow("CheckBox.UncheckedPressed", ElementTheme.Dark)]
	[DataRow("ComboBoxItem.PointerOver", ElementTheme.Dark)]
	[DataRow("ComboBoxItem.Pressed", ElementTheme.Dark)]
	[DataRow("ListViewItem.Pressed", ElementTheme.Dark)]
	[DataRow("ComboBoxItem.SelectedPointerOver", ElementTheme.Dark)]
	[DataRow("ComboBoxItem.SelectedPressed", ElementTheme.Light)]
	[DataRow("ComboBoxItem.SelectedPressed", ElementTheme.Dark)]
	[DataRow("ListViewItem.PointerOverSelected", ElementTheme.Dark)]
	[DataRow("ListViewItem.PressedSelected", ElementTheme.Light)]
	[DataRow("ListViewItem.PressedSelected", ElementTheme.Dark)]
	[DataRow("MenuFlyoutItem.Pressed", ElementTheme.Light)]
	[DataRow("MenuFlyoutItem.Pressed", ElementTheme.Dark)]
	[DataRow("ToggleMenuFlyoutItem.Pressed", ElementTheme.Dark)]
	[DataRow("MenuFlyoutSubItem.Pressed", ElementTheme.Dark)]
	[DataRow("RadioMenuFlyoutItem.Pressed", ElementTheme.Dark)]
	[DataRow("ToggleSwitch.Pressed", ElementTheme.Light)]
	[DataRow("ToggleSwitch.Pressed", ElementTheme.Dark)]
	[DataRow("ToggleSwitch.OffPressed", ElementTheme.Light)]
	[DataRow("ToggleSwitch.OffPressed", ElementTheme.Dark)]
	[DataRow("ExpanderHeader.PointerOver", ElementTheme.Dark)]
	[DataRow("ExpanderHeader.Pressed", ElementTheme.Light)]
	[DataRow("ExpanderHeader.Pressed", ElementTheme.Dark)]
	[DataRow("ExpanderHeader.CheckedPressed", ElementTheme.Dark)]
	[DataRow("ExpanderHeaderUp.Pressed", ElementTheme.Dark)]
	[DataRow("Slider.Pressed", ElementTheme.Light)]
	[DataRow("Slider.Pressed", ElementTheme.Dark)]
	public async Task When_ColouredPaletteAndInteractiveState_Then_FillKeepsItsRoleAndContentStaysReadable(
		string scenarioId, ElementTheme appearance)
	{
		// Arrange: seed the application theme, as an app sets it in App.xaml. Visual-state setters
		// resolve their {ThemeResource} from there, not from a theme scoped to the test container.
		var theme = SemanticThemeHelper.GetTheme();
		Assert.IsNotNull(theme, "The Simple sample app must have a BaseTheme in its application resources.");
		var originalColors = theme.Colors;
		var appPrimaryBefore = AppPrimaryColor();

		try
		{
			theme.Colors = CreateColouredPalette();

			var scenario = GetScenario(scenarioId);
			var control = scenario.Create();
			var container = new Grid { RequestedTheme = appearance };
			container.Children.Add(control);

			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(control);
			await UnitTestsUIContentHelper.WaitForIdle();

			if (scenario.RestState is { } restState)
			{
				Assert.IsTrue(VisualStateManager.GoToState(control, restState, useTransitions: false),
					$"{scenarioId}: the template should define the '{restState}' state");
				await UnitTestsUIContentHelper.WaitForIdle();
			}

			var resting = ReadRendering(control, scenario);

			// Act
			Assert.IsTrue(VisualStateManager.GoToState(control, scenario.State, useTransitions: false),
				$"{scenarioId}: the template should define the '{scenario.State}' state");
			await UnitTestsUIContentHelper.WaitForIdle();
			var active = ReadRendering(control, scenario);

			// Assert
			var context = $"{scenarioId} under {appearance}";
			Assert.AreEqual(resting.Fill, active.Fill,
				$"{context}: {scenario.Fill} should stay on the resting role ({resting.Fill}) and show the state as a tint, but became {active.Fill}.");

			if (scenario.Layer is not null)
			{
				Assert.AreNotEqual(resting.Composite, active.Composite, $"{context}: the state should still be visible.");
			}

			// Contrast is only meaningful on an opaque fill; a transparent one shows whatever is behind the control.
			if (active.Label is { } label && active.Fill.Opacity >= 0.999)
			{
				var contrast = ContrastRatio(active.Composite, label);
				Assert.IsTrue(contrast >= MinimumStateContrast,
					$"{context}: {scenario.Label} {label} on {active.Composite} has contrast {contrast:F2}, below {MinimumStateContrast}.");
			}
		}
		finally
		{
			theme.Colors = originalColors;
		}

		Assert.AreEqual(appPrimaryBefore, AppPrimaryColor(), "Restoring the app theme's Colors should bring back its own palette.");
	}

	private static Color AppPrimaryColor() => ((SolidColorBrush)Application.Current.Resources["PrimaryBrush"]).Color;

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_StateLayerKeysResolve_Then_TheyTintWithTheLabelRole(ElementTheme appearance)
	{
		// Arrange
		var container = new StackPanel { RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new Uno.Simple.SimpleTheme());
		(string Key, string Expected)[] keys =
		{
			("FilledButtonStateLayerBackgroundPointerOver", "OnPrimaryHoverBrush"),
			("FilledButtonStateLayerBackgroundPressed", "OnPrimaryPressedBrush"),
			("FilledTonalButtonStateLayerBackgroundPointerOver", "OnSurfaceHoverBrush"),
			("FilledTonalButtonStateLayerBackgroundPressed", "OnSurfacePressedBrush"),
			("OutlinedButtonStateLayerBackgroundPointerOver", "OnSurfaceHoverBrush"),
			("OutlinedButtonStateLayerBackgroundPressed", "OnSurfacePressedBrush"),
			("IconButtonStateLayerBackgroundPointerOver", "OnPrimaryHoverBrush"),
			("IconButtonStateLayerBackgroundPressed", "OnPrimaryPressedBrush"),
			("TextToggleButtonStateLayerBackgroundPointerOver", "OnSurfaceHoverBrush"),
			("TextToggleButtonStateLayerBackgroundPressed", "OnSurfacePressedBrush"),
			("TextToggleButtonStateLayerBackgroundCheckedPointerOver", "OnPrimaryHoverBrush"),
			("TextToggleButtonStateLayerBackgroundCheckedPressed", "OnPrimaryPressedBrush"),
			("TextToggleButtonStateLayerBackgroundIndeterminatePointerOver", "OnSurfaceHoverBrush"),
			("TextToggleButtonStateLayerBackgroundIndeterminatePressed", "OnSurfacePressedBrush"),
			("CheckBoxStateLayerBackgroundUncheckedPointerOver", "OnSurfaceHoverBrush"),
			("CheckBoxStateLayerBackgroundUncheckedPressed", "OnSurfacePressedBrush"),
			("CheckBoxStateLayerBackgroundCheckedPointerOver", "OnPrimaryHoverBrush"),
			("CheckBoxStateLayerBackgroundCheckedPressed", "OnPrimaryPressedBrush"),
			("CheckBoxStateLayerBackgroundIndeterminatePointerOver", "OnPrimaryHoverBrush"),
			("CheckBoxStateLayerBackgroundIndeterminatePressed", "OnPrimaryPressedBrush"),
			("ComboBoxItemStateLayerBackgroundPointerOver", "OnSurfaceHoverBrush"),
			("ComboBoxItemStateLayerBackgroundPressed", "OnSurfacePressedBrush"),
			("ComboBoxItemStateLayerBackgroundSelectedPointerOver", "OnPrimaryHoverBrush"),
			("ComboBoxItemStateLayerBackgroundSelectedPressed", "OnPrimaryPressedBrush"),
			("ListViewItemStateLayerBackgroundPointerOver", "OnSurfaceHoverBrush"),
			("ListViewItemStateLayerBackgroundPressed", "OnSurfacePressedBrush"),
			("ListViewItemStateLayerBackgroundPointerOverSelected", "OnPrimaryHoverBrush"),
			("ListViewItemStateLayerBackgroundPressedSelected", "OnPrimaryPressedBrush"),
			("ToggleSwitchStateLayerFillPressed", "OnSurfacePressedBrush"),
		};

		// Resolve through live ThemeResource expressions so each probe follows the container's theme.
		foreach (var (key, expected) in keys)
		{
			container.Children.Add(CreateBrushProbe(key));
			container.Children.Add(CreateBrushProbe(expected));
		}

		// Act
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(container);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert
		for (var index = 0; index < keys.Length; index++)
		{
			var actual = ((Border)container.Children[index * 2]).Background as SolidColorBrush;
			var expected = (SolidColorBrush)((Border)container.Children[index * 2 + 1]).Background;
			var context = $"{keys[index].Key} under {appearance}";
			Assert.IsNotNull(actual, $"{context} should resolve to a SolidColorBrush");
			Assert.AreEqual(expected.Color, actual.Color, context);
			Assert.AreEqual(expected.Opacity, actual.Opacity, 0.001, context);
			Assert.IsTrue(actual.Opacity < 1.0, $"{context} should be a translucent tint, not a fill");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_SelectedValueIsHovered_Then_ItStaysOnThePrimaryRole(ElementTheme appearance)
	{
		// RatingControl's hovered selected stars and CalendarView's hovered / pressed selected day
		// border used PrimaryVariantDark, a dark role in dark mode, so they faded into a dark surface.
		var container = new StackPanel { RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new Uno.Simple.SimpleTheme());
		container.Children.Add(CreateBrushProbe("RatingControlSelectedForegroundPointerOver"));
		container.Children.Add(CreateBrushProbe("PrimaryBrush"));
		var calendar = new CalendarView { Style = (Style)container.Resources["SimpleCalendarViewStyle"] };
		container.Children.Add(calendar);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(calendar);
		await UnitTestsUIContentHelper.WaitForIdle();

		var primary = ((SolidColorBrush)((Border)container.Children[1]).Background).Color;
		Assert.AreEqual(primary, ((SolidColorBrush)((Border)container.Children[0]).Background).Color,
			$"RatingControlSelectedForegroundPointerOver under {appearance}");
		Assert.AreEqual(primary, ((SolidColorBrush)calendar.SelectedHoverBorderBrush).Color,
			$"CalendarView SelectedHoverBorderBrush under {appearance}");
		Assert.AreEqual(primary, ((SolidColorBrush)calendar.SelectedPressedBorderBrush).Color,
			$"CalendarView SelectedPressedBorderBrush under {appearance}");
	}

	private static Border CreateBrushProbe(string key)
		=> (Border)XamlReader.Load($$"""
			<Border xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
			        Width="8"
			        Height="8"
			        Background="{ThemeResource {{key}}}" />
			""");

	private static ThemeColors CreateColouredPalette()
	{
		var overrides = new ResourceDictionary();
		overrides.ThemeDictionaries["Dark"] = new ResourceDictionary { ["TertiaryContainerColor"] = DarkTertiaryContainer };

		return new ThemeColors
		{
			PrimarySeed = PrimarySeed,
			SecondarySeed = SecondarySeed,
			TertiarySeed = TertiarySeed,
			OverrideDictionary = overrides,
		};
	}

	/// <summary>What the control paints: the fill, the fill with any state layer over it, and the colour of what sits on it.</summary>
	private static ((Color Color, double Opacity) Fill, Color Composite, Color? Label) ReadRendering(Control control, Scenario scenario)
	{
		var fillElement = FindByName(control, scenario.Fill);
		Assert.IsNotNull(fillElement, $"The template should have an element named {scenario.Fill}");
		Assert.IsInstanceOfType<SolidColorBrush>(FillOf(fillElement), $"The {scenario.Fill} fill should be a SolidColorBrush");
		var fillBrush = (SolidColorBrush)FillOf(fillElement)!;
		var fill = (fillBrush.Color, Math.Round(fillBrush.Opacity * fillBrush.Color.A / 255.0, 3));

		// On a translucent fill (a transparent item) the composite is the tint over an opaque copy of the fill:
		// enough to tell whether a state paints anything.
		var composite = Color.FromArgb(0xFF, fillBrush.Color.R, fillBrush.Color.G, fillBrush.Color.B);
		if (scenario.Layer is { } layerName
			&& FindByName(control, layerName) is { } layerElement
			&& FillOf(layerElement) is SolidColorBrush layer)
		{
			composite = Over(layer.Color, layer.Opacity * layerElement.Opacity, composite);
		}

		Color? label = null;
		if (scenario.Label is { } labelName)
		{
			var labelElement = FindByName(control, labelName);
			Assert.IsNotNull(labelElement, $"The template should have an element named {labelName}");
			label = SolidColor(ForegroundOf(labelElement), $"{labelName} foreground");
		}

		return (fill, composite, label);
	}

	private static Brush? FillOf(FrameworkElement element) => element switch
	{
		Shape shape => shape.Fill,
		Panel panel => panel.Background,
		Border border => border.Background,
		ContentPresenter presenter => presenter.Background,
		Control control => control.Background,
		_ => null,
	};

	private static Brush? ForegroundOf(FrameworkElement element) => element switch
	{
		Shape shape => shape.Fill,
		TextBlock text => text.Foreground,
		ContentPresenter presenter => presenter.Foreground,
		Control control => control.Foreground,
		_ => null,
	};

	private static Color SolidColor(Brush? brush, string what)
	{
		Assert.IsInstanceOfType<SolidColorBrush>(brush, $"The {what} should be a SolidColorBrush");
		var solid = (SolidColorBrush)brush;
		Assert.AreEqual(1.0, solid.Opacity * solid.Color.A / 255.0, 0.001, $"The {what} should be opaque");
		return solid.Color;
	}

	private static FrameworkElement? FindByName(DependencyObject parent, string name)
	{
		for (var index = 0; index < VisualTreeHelper.GetChildrenCount(parent); index++)
		{
			var child = VisualTreeHelper.GetChild(parent, index);
			if (child is FrameworkElement { Name: var current } element && current == name)
			{
				return element;
			}

			if (FindByName(child, name) is { } found)
			{
				return found;
			}
		}

		return null;
	}

	/// <summary>Source-over compositing of a translucent colour onto an opaque one.</summary>
	private static Color Over(Color top, double opacity, Color bottom)
	{
		var alpha = top.A / 255.0 * opacity;
		byte Blend(byte front, byte back) => (byte)Math.Round(front * alpha + back * (1 - alpha));
		return Color.FromArgb(0xFF, Blend(top.R, bottom.R), Blend(top.G, bottom.G), Blend(top.B, bottom.B));
	}

	private static double ContrastRatio(Color first, Color second)
	{
		var y1 = RelativeLuminance(first);
		var y2 = RelativeLuminance(second);
		return (Math.Max(y1, y2) + 0.05) / (Math.Min(y1, y2) + 0.05);
	}

	private static double RelativeLuminance(Color color)
		=> 0.2126 * Linearized(color.R) + 0.7152 * Linearized(color.G) + 0.0722 * Linearized(color.B);

	private static double Linearized(byte component)
	{
		var normalized = component / 255.0;
		return normalized <= 0.04045 ? normalized / 12.92 : Math.Pow((normalized + 0.055) / 1.055, 2.4);
	}
}
