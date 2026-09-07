using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.UI.Xaml.Controls.Primitives;
using Uno.Simple;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_SimpleSemanticOverrideStates
{
	private static readonly Color ForegroundColor = Colors.Magenta;
	private static readonly Color BackgroundColor = Colors.Lime;
	private static readonly Color BorderColor = Colors.Blue;

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "Normal")]
	[DataRow(ElementTheme.Light, "PointerOver")]
	[DataRow(ElementTheme.Light, "Pressed")]
	[DataRow(ElementTheme.Light, "Disabled")]
	[DataRow(ElementTheme.Dark, "Normal")]
	[DataRow(ElementTheme.Dark, "PointerOver")]
	[DataRow(ElementTheme.Dark, "Pressed")]
	[DataRow(ElementTheme.Dark, "Disabled")]
	public async Task When_OutlinedButtonOverridden_UsesOwnStateKeys(ElementTheme appearance, string state)
	{
		var host = CreateHost(appearance);
		var suffix = state == "Normal" ? "" : state;
		SetPaint(host.Resources, "OutlinedButton", suffix);
		var outlined = new Button { Content = "outlined", Style = GetStyle(host, "OutlinedButtonStyle") };
		var tonal = new Button { Content = "tonal", Style = GetStyle(host, "FilledTonalButtonStyle") };
		await Show(host, outlined, tonal);
		await GoToState(outlined, state);
		await GoToState(tonal, state);
		AssertPaint(FindPart<ContentPresenter>(outlined, "ContentPresenter").Foreground,
			FindPart<Grid>(outlined, "Root").Background, FindPart<Grid>(outlined, "Root").BorderBrush, state);
		Assert.AreNotEqual(ForegroundColor, BrushColor(FindPart<ContentPresenter>(tonal, "ContentPresenter").Foreground),
			"An outlined override must not change the tonal sibling.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "Normal")]
	[DataRow(ElementTheme.Light, "PointerOver")]
	[DataRow(ElementTheme.Light, "Pressed")]
	[DataRow(ElementTheme.Light, "Disabled")]
	[DataRow(ElementTheme.Light, "Checked")]
	[DataRow(ElementTheme.Light, "CheckedPointerOver")]
	[DataRow(ElementTheme.Light, "CheckedPressed")]
	[DataRow(ElementTheme.Light, "CheckedDisabled")]
	[DataRow(ElementTheme.Light, "IndeterminateNormal")]
	[DataRow(ElementTheme.Light, "IndeterminatePointerOver")]
	[DataRow(ElementTheme.Light, "IndeterminatePressed")]
	[DataRow(ElementTheme.Light, "IndeterminateDisabled")]
	[DataRow(ElementTheme.Dark, "Normal")]
	[DataRow(ElementTheme.Dark, "PointerOver")]
	[DataRow(ElementTheme.Dark, "Pressed")]
	[DataRow(ElementTheme.Dark, "Disabled")]
	[DataRow(ElementTheme.Dark, "Checked")]
	[DataRow(ElementTheme.Dark, "CheckedPointerOver")]
	[DataRow(ElementTheme.Dark, "CheckedPressed")]
	[DataRow(ElementTheme.Dark, "CheckedDisabled")]
	[DataRow(ElementTheme.Dark, "IndeterminateNormal")]
	[DataRow(ElementTheme.Dark, "IndeterminatePointerOver")]
	[DataRow(ElementTheme.Dark, "IndeterminatePressed")]
	[DataRow(ElementTheme.Dark, "IndeterminateDisabled")]
	public async Task When_IconToggleOverridden_UsesIndependentStateKeys(ElementTheme appearance, string state)
	{
		var host = CreateHost(appearance);
		var suffix = state == "Normal" ? "" : state == "IndeterminateNormal" ? "Indeterminate" : state;
		SetPaint(host.Resources, "IconToggleButton", suffix);
		var icon = new ToggleButton { Content = "icon", Style = GetStyle(host, "IconToggleButtonStyle") };
		var text = new ToggleButton { Content = "text", Style = GetStyle(host, "TextToggleButtonStyle") };
		await Show(host, icon, text);
		await GoToState(icon, state);
		await GoToState(text, state);
		var presenter = FindPart<ContentPresenter>(icon, "ContentPresenter");
		AssertPaint(presenter.Foreground, presenter.Background, presenter.BorderBrush, state);
		Assert.AreNotEqual(ForegroundColor, BrushColor(FindPart<ContentPresenter>(text, "ContentPresenter").Foreground),
			"An icon-toggle override must not change the text-toggle sibling.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "Checked", "Normal")]
	[DataRow(ElementTheme.Light, "Checked", "PointerOver")]
	[DataRow(ElementTheme.Light, "Checked", "Pressed")]
	[DataRow(ElementTheme.Light, "Checked", "Disabled")]
	[DataRow(ElementTheme.Light, "Indeterminate", "Normal")]
	[DataRow(ElementTheme.Light, "Indeterminate", "PointerOver")]
	[DataRow(ElementTheme.Light, "Indeterminate", "Pressed")]
	[DataRow(ElementTheme.Light, "Indeterminate", "Disabled")]
	[DataRow(ElementTheme.Dark, "Checked", "Normal")]
	[DataRow(ElementTheme.Dark, "Checked", "PointerOver")]
	[DataRow(ElementTheme.Dark, "Checked", "Pressed")]
	[DataRow(ElementTheme.Dark, "Checked", "Disabled")]
	[DataRow(ElementTheme.Dark, "Indeterminate", "Normal")]
	[DataRow(ElementTheme.Dark, "Indeterminate", "PointerOver")]
	[DataRow(ElementTheme.Dark, "Indeterminate", "Pressed")]
	[DataRow(ElementTheme.Dark, "Indeterminate", "Disabled")]
	public async Task When_CheckBoxLabelOverridden_UsesMatchingCombinedState(ElementTheme appearance, string checkState, string state)
	{
		var host = CreateHost(appearance);
		var suffix = state == "Normal" ? "" : state;
		host.Resources["CheckBoxForeground" + checkState + suffix] = new SolidColorBrush(ForegroundColor);
		host.Resources["CheckBoxForegroundUnchecked" + suffix] = new SolidColorBrush(Colors.Cyan);
		var checkBox = new CheckBox { Content = "label", IsThreeState = true, Style = GetStyle(host, "CheckBoxStyle") };
		await Show(host, checkBox);
		await GoToState(checkBox, checkState + state);
		Assert.AreEqual(ForegroundColor, BrushColor(FindPart<ContentPresenter>(checkBox, "ContentPresenter").Foreground),
			"The label must use the foreground for its checked or indeterminate state.");
		await GoToState(checkBox, "Unchecked" + state);
		Assert.AreEqual(Colors.Cyan, BrushColor(FindPart<ContentPresenter>(checkBox, "ContentPresenter").Foreground),
			"Returning to unchecked must restore the matching unchecked foreground.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "Filled", "PointerOver")]
	[DataRow(ElementTheme.Light, "Filled", "Focused")]
	[DataRow(ElementTheme.Light, "Filled", "Disabled")]
	[DataRow(ElementTheme.Light, "Outlined", "PointerOver")]
	[DataRow(ElementTheme.Light, "Outlined", "Focused")]
	[DataRow(ElementTheme.Light, "Outlined", "Disabled")]
	[DataRow(ElementTheme.Dark, "Filled", "PointerOver")]
	[DataRow(ElementTheme.Dark, "Filled", "Focused")]
	[DataRow(ElementTheme.Dark, "Filled", "Disabled")]
	[DataRow(ElementTheme.Dark, "Outlined", "PointerOver")]
	[DataRow(ElementTheme.Dark, "Outlined", "Focused")]
	[DataRow(ElementTheme.Dark, "Outlined", "Disabled")]
	public async Task When_TextBoxLabelOverridden_HeaderAndPlaceholderFollowState(ElementTheme appearance, string variant, string state)
	{
		var host = CreateHost(appearance);
		host.Resources[variant + "TextBoxHeaderForeground" + state] = new SolidColorBrush(ForegroundColor);
		host.Resources[variant + "TextBoxPlaceholderForeground" + state] = new SolidColorBrush(BackgroundColor);
		host.Resources[variant + "TextBoxHeaderForeground"] = new SolidColorBrush(Colors.Cyan);
		host.Resources[variant + "TextBoxPlaceholderForeground"] = new SolidColorBrush(Colors.Yellow);
		var textBox = new TextBox { Header = "header", PlaceholderText = "placeholder", Style = GetStyle(host, variant + "TextBoxStyle") };
		await Show(host, textBox);
		await GoToState(textBox, state);
		Assert.AreEqual(ForegroundColor, BrushColor(FindPart<ContentPresenter>(textBox, "HeaderContentPresenter").Foreground), "Header foreground");
		Assert.AreEqual(BackgroundColor, BrushColor(FindPart<ContentControl>(textBox, "PlaceholderElement").Foreground), "Placeholder foreground");
		await GoToState(textBox, "Normal");
		Assert.AreEqual(Colors.Cyan, BrushColor(FindPart<ContentPresenter>(textBox, "HeaderContentPresenter").Foreground), "Normal header must be restored.");
		Assert.AreEqual(Colors.Yellow, BrushColor(FindPart<ContentControl>(textBox, "PlaceholderElement").Foreground), "Normal placeholder must be restored.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "Normal")]
	[DataRow(ElementTheme.Light, "PointerOver")]
	[DataRow(ElementTheme.Light, "Pressed")]
	[DataRow(ElementTheme.Light, "Disabled")]
	[DataRow(ElementTheme.Dark, "Normal")]
	[DataRow(ElementTheme.Dark, "PointerOver")]
	[DataRow(ElementTheme.Dark, "Pressed")]
	[DataRow(ElementTheme.Dark, "Disabled")]
	public async Task When_SecondaryHyperlinkOverridden_ContentAndUnderlineUseOwnState(ElementTheme appearance, string state)
	{
		var host = CreateHost(appearance);
		var suffix = state == "Normal" ? "" : state;
		host.Resources["SecondaryHyperlinkButtonForeground" + suffix] = new SolidColorBrush(ForegroundColor);
		host.Resources["HyperlinkButtonForeground" + suffix] = new SolidColorBrush(Colors.Cyan);
		var secondary = new HyperlinkButton { Content = "secondary", Style = GetStyle(host, "SecondaryHyperlinkButtonStyle") };
		var primary = new HyperlinkButton { Content = "primary", Style = GetStyle(host, "HyperlinkButtonStyle") };
		await Show(host, secondary, primary);
		await GoToState(secondary, state);
		await GoToState(primary, state);
		Assert.AreEqual(ForegroundColor, BrushColor(FindPart<ContentPresenter>(secondary, "ContentPresenter").Foreground));
		Assert.AreEqual(ForegroundColor, BrushColor(FindPart<Microsoft.UI.Xaml.Shapes.Rectangle>(secondary, "UnderlineRect").Fill));
		Assert.AreEqual(Colors.Cyan, BrushColor(FindPart<ContentPresenter>(primary, "ContentPresenter").Foreground),
			"Primary hyperlinks must retain their independent foreground.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "ElevatedButtonStyle")]
	[DataRow(ElementTheme.Light, "CommandBarStyle")]
	[DataRow(ElementTheme.Light, "MediaTransportControlsStyle")]
	[DataRow(ElementTheme.Light, "DatePickerFlyoutPresenterStyle")]
	[DataRow(ElementTheme.Dark, "ElevatedButtonStyle")]
	[DataRow(ElementTheme.Dark, "CommandBarStyle")]
	[DataRow(ElementTheme.Dark, "MediaTransportControlsStyle")]
	[DataRow(ElementTheme.Dark, "DatePickerFlyoutPresenterStyle")]
	public async Task When_MissingSemanticStyleApplied_ResolvesOwnStyleAndTemplate(ElementTheme appearance, string key)
	{
		var theme = new SimpleTheme();
		var style = FindOwnStyle(theme, key);
		Assert.IsNotNull(style, "Simple must declare " + key + " instead of relying on ambient theme resources.");
		Control control = key switch
		{
			"ElevatedButtonStyle" => new Button { Content = "elevated" },
			"CommandBarStyle" => new CommandBar
			{
				PrimaryCommands = { new AppBarButton { Label = "Add", Icon = new SymbolIcon(Symbol.Add) } },
			},
			"MediaTransportControlsStyle" => new MediaTransportControls(),
			"DatePickerFlyoutPresenterStyle" => new DatePickerFlyoutPresenter(),
			_ => throw new AssertFailedException("Unexpected style key."),
		};
		Assert.AreEqual(control.GetType(), style.TargetType);
		control.Style = style;
		var host = CreateHost(appearance, theme);
		await Show(host, control);
		Assert.IsNotNull(control.Template, key + " must preserve a usable template.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "FilledButtonStyle")]
	[DataRow(ElementTheme.Light, "FilledTonalButtonStyle")]
	[DataRow(ElementTheme.Light, "OutlinedButtonStyle")]
	[DataRow(ElementTheme.Light, "TextButtonStyle")]
	[DataRow(ElementTheme.Dark, "FilledButtonStyle")]
	[DataRow(ElementTheme.Dark, "FilledTonalButtonStyle")]
	[DataRow(ElementTheme.Dark, "OutlinedButtonStyle")]
	[DataRow(ElementTheme.Dark, "TextButtonStyle")]
	public async Task When_ButtonMeasurementsOverridden_PortableBorderAndFontAreConsumed(ElementTheme appearance, string styleKey)
	{
		var host = CreateHost(appearance);
		host.Resources["ButtonBorderThickness"] = new Thickness(3, 4, 5, 6);
		host.Resources["LabelLargeFontSize"] = 27d;
		host.Resources["SimpleButtonCornerRadius"] = new CornerRadius(13);
		host.Resources["SimpleSpace300Thickness"] = new Thickness(17);
		var button = new Button { Content = "measure", Style = GetStyle(host, styleKey) };
		await Show(host, button);
		Assert.AreEqual(new Thickness(3, 4, 5, 6), button.BorderThickness, "Portable ButtonBorderThickness override");
		Assert.AreEqual(27d, button.FontSize, "Scoped semantic typography override");
		Assert.AreEqual(new CornerRadius(13), button.CornerRadius, "Existing Simple corner override must be preserved.");
		Assert.AreEqual(new Thickness(17), button.Padding, "Existing Simple spacing override must be preserved.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_SeedlessSimplePaletteUsed_LegacySecondaryBrushesAreGrayscale(ElementTheme appearance)
	{
		var host = CreateHost(appearance);
		var dark = (Border)XamlReader.Load("<Border xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Background=\"{ThemeResource SecondaryVariantDarkBrush}\" />");
		var light = (Border)XamlReader.Load("<Border xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Background=\"{ThemeResource SecondaryVariantLightBrush}\" />");
		dark.Width = light.Width = 20;
		dark.Height = light.Height = 20;
		await Show(host, dark, light);
		foreach (var (brush, key) in new[] { (dark.Background, "SecondaryVariantDarkBrush"), (light.Background, "SecondaryVariantLightBrush") })
		{
			var color = BrushColor(brush);
			Assert.AreEqual(color.R, color.G, key + " must use Simple's grayscale palette.");
			Assert.AreEqual(color.G, color.B, key + " must use Simple's grayscale palette.");
			Assert.AreEqual((byte)255, color.A, key + " must remain opaque.");
		}
	}

	private static StackPanel CreateHost(ElementTheme appearance, SimpleTheme? theme = null)
	{
		var host = new StackPanel { RequestedTheme = appearance };
		host.Resources.MergedDictionaries.Add(theme ?? new SimpleTheme());
		return host;
	}

	private static Style GetStyle(FrameworkElement host, string key) => (Style)host.Resources[key];

	private static void SetPaint(ResourceDictionary resources, string prefix, string suffix)
	{
		resources[prefix + "Foreground" + suffix] = new SolidColorBrush(ForegroundColor);
		resources[prefix + "Background" + suffix] = new SolidColorBrush(BackgroundColor);
		resources[prefix + "BorderBrush" + suffix] = new SolidColorBrush(BorderColor);
	}

	private static async Task Show(Panel host, params FrameworkElement[] controls)
	{
		foreach (var control in controls)
		{
			host.Children.Add(control);
		}
		UnitTestsUIContentHelper.Content = host;
		foreach (var control in controls)
		{
			await UnitTestsUIContentHelper.WaitForLoaded(control);
		}
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	private static async Task GoToState(Control control, string state)
	{
		Assert.IsTrue(VisualStateManager.GoToState(control, state, false), control.GetType().Name + " must support " + state);
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	private static void AssertPaint(Brush foreground, Brush background, Brush border, string state)
	{
		Assert.AreEqual(ForegroundColor, BrushColor(foreground), state + " foreground");
		Assert.AreEqual(BackgroundColor, BrushColor(background), state + " background");
		Assert.AreEqual(BorderColor, BrushColor(border), state + " border");
	}

	private static Color BrushColor(Brush brush)
	{
		Assert.IsInstanceOfType<SolidColorBrush>(brush);
		return ((SolidColorBrush)brush).Color;
	}

	private static T FindPart<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		if (FindPartCore<T>(root, name) is { } part)
		{
			return part;
		}
		throw new AssertFailedException("The realized template must contain " + name + ".");
	}

	private static T? FindPartCore<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		if (root is T element && element.Name == name)
		{
			return element;
		}
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			if (FindPartCore<T>(VisualTreeHelper.GetChild(root, i), name) is { } child)
			{
				return child;
			}
		}
		return null;
	}

	private static Style? FindOwnStyle(ResourceDictionary dictionary, string key)
	{
		foreach (var entry in dictionary)
		{
			if (Equals(entry.Key, key))
			{
				return entry.Value as Style;
			}
		}
		foreach (var merged in dictionary.MergedDictionaries)
		{
			if (FindOwnStyle(merged, key) is { } style)
			{
				return style;
			}
		}
		return null;
	}
}
