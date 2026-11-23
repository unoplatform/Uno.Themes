#nullable enable

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Interactive states of the Simple button styles must stay on the button's own colour role:
/// the fill keeps the resting role and the state is a tint over it, so the label still pairs
/// with what it sits on. Regression guard for unoplatform/Uno.Themes#1784, where a coloured
/// palette turned a filled button's hover fill dark under a dark label, and a tonal button
/// switched to the tertiary colour while pressed.
/// </summary>
[TestClass]
public class Given_ButtonStateColors
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

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle", "PointerOver", ElementTheme.Light)]
	[DataRow("FilledButtonStyle", "PointerOver", ElementTheme.Dark)]
	[DataRow("FilledButtonStyle", "Pressed", ElementTheme.Light)]
	[DataRow("FilledButtonStyle", "Pressed", ElementTheme.Dark)]
	[DataRow("FilledTonalButtonStyle", "PointerOver", ElementTheme.Light)]
	[DataRow("FilledTonalButtonStyle", "PointerOver", ElementTheme.Dark)]
	[DataRow("FilledTonalButtonStyle", "Pressed", ElementTheme.Light)]
	[DataRow("FilledTonalButtonStyle", "Pressed", ElementTheme.Dark)]
	[DataRow("IconButtonStyle", "PointerOver", ElementTheme.Dark)]
	[DataRow("IconButtonStyle", "Pressed", ElementTheme.Dark)]
	[DataRow("SimpleIconButtonNeutralStyle", "PointerOver", ElementTheme.Dark)]
	[DataRow("SimpleIconButtonNeutralStyle", "Pressed", ElementTheme.Dark)]
	public async Task When_ColouredPaletteAndInteractiveState_Then_FillKeepsItsRoleAndLabelStaysReadable(
		string styleKey, string state, ElementTheme appearance)
	{
		// Arrange: seed the application theme, as an app sets it in App.xaml. Visual-state setters
		// resolve their {ThemeResource} from there, not from a theme scoped to the test container.
		var theme = SemanticThemeHelper.GetTheme();
		Assert.IsNotNull(theme, "The Simple sample app must have a BaseTheme in its application resources.");
		var originalColors = theme.Colors;

		try
		{
			theme.Colors = CreateColouredPalette();

			var container = new Grid { RequestedTheme = appearance };
			var button = new Button
			{
				Content = "Add to order",
				Style = (Style)Application.Current.Resources[styleKey],
			};
			container.Children.Add(button);

			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();

			var resting = ReadRendering(button);

			// Act
			VisualStateManager.GoToState(button, state, useTransitions: false);
			await UnitTestsUIContentHelper.WaitForIdle();
			var active = ReadRendering(button);

			// Assert
			var context = $"{styleKey} {state} under {appearance}";
			Assert.AreEqual(resting.Fill, active.Fill,
				$"{context}: the fill should stay on the resting role ({resting.Fill}) and show the state as a tint, but became {active.Fill}.");
			Assert.AreNotEqual(resting.Composite, active.Composite,
				$"{context}: the state should still be visible.");

			var contrast = ContrastRatio(active.Composite, active.Foreground);
			Assert.IsTrue(contrast >= MinimumStateContrast,
				$"{context}: label {active.Foreground} on {active.Composite} has contrast {contrast:F2}, below {MinimumStateContrast}.");
		}
		finally
		{
			theme.Colors = originalColors;
		}
	}

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
			var actual = (SolidColorBrush)((Border)container.Children[index * 2]).Background;
			var expected = (SolidColorBrush)((Border)container.Children[index * 2 + 1]).Background;
			var context = $"{keys[index].Key} under {appearance}";
			Assert.IsNotNull(actual, $"{context} should resolve to a SolidColorBrush");
			Assert.AreEqual(expected.Color, actual.Color, context);
			Assert.AreEqual(expected.Opacity, actual.Opacity, 0.001, context);
			Assert.IsTrue(actual.Opacity < 1.0, $"{context} should be a translucent tint, not a fill");
		}
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

	/// <summary>What the button paints: its fill, the fill with any state layer over it, and the label colour.</summary>
	private static (Color Fill, Color Composite, Color Foreground) ReadRendering(Button button)
	{
		var root = VisualTreeHelper.GetChild(button, 0) as FrameworkElement;
		Assert.IsNotNull(root, "The button template should have a root element");

		var fill = SolidColor(BackgroundOf(root), "template root background");
		var composite = fill;
		if (FindByName(root, "StateLayer") is Border stateLayer && stateLayer.Background is SolidColorBrush layer)
		{
			composite = Over(layer.Color, layer.Opacity, fill);
		}

		var presenter = FindByName(root, "ContentPresenter") as ContentPresenter;
		Assert.IsNotNull(presenter, "The button template should have a ContentPresenter");
		var foreground = SolidColor(presenter.Foreground, "label foreground");

		return (fill, composite, foreground);
	}

	private static Brush? BackgroundOf(FrameworkElement element) => element switch
	{
		Panel panel => panel.Background,
		Border border => border.Background,
		ContentPresenter presenter => presenter.Background,
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
		if (parent is FrameworkElement { Name: var current } element && current == name)
		{
			return element;
		}

		for (var index = 0; index < VisualTreeHelper.GetChildrenCount(parent); index++)
		{
			if (FindByName(VisualTreeHelper.GetChild(parent, index), name) is { } found)
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
