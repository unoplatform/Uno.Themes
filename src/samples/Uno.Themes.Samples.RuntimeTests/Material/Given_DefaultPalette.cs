using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.Themes;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests.Material;

/// <summary>
/// Verifies that a default MaterialTheme (no seed configured) resolves the
/// default Material palette from SharedColorPalette.xaml, and that seed
/// generation only activates when a PrimarySeed is explicitly set.
/// This guards against regressions where an implicit default seed (or a
/// change to the generation algorithm) silently shifts the default colors
/// of every Material app — see unoplatform/uno.toolkit.ui#1606.
/// </summary>
[TestClass]
public class Given_DefaultPalette
{
	// Default Material primaries from SharedColorPalette.xaml.
	private static readonly Color DefaultPrimaryLight = Color.FromArgb(0xFF, 0x59, 0x46, 0xD2);
	private static readonly Color DefaultPrimaryDark = Color.FromArgb(0xFF, 0xC7, 0xBF, 0xFF);

	[TestMethod]
	[RunsOnUIThread]
	public void When_NoSeedConfigured_Then_PrimaryColorIsDefaultPalette()
	{
		var theme = new MaterialTheme();

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.IsTrue(
			container.Resources.TryGetValue("PrimaryColor", out var colorVal),
			"PrimaryColor should be resolvable from the theme");

		// The ambient theme (Light/Dark) at test time is host-dependent, so
		// accept either default tone; a seed-generated tone matches neither.
		var color = (Color)colorVal;
		Assert.IsTrue(
			color == DefaultPrimaryLight || color == DefaultPrimaryDark,
			$"A default MaterialTheme must resolve the default Material primary " +
			$"(#{DefaultPrimaryLight} or #{DefaultPrimaryDark}), but got #{color} — " +
			"a seed-generated tone must not apply without an explicit PrimarySeed.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_NoSeedConfigured_Then_ButtonUsesDefaultPalette()
	{
		var theme = new MaterialTheme();

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should resolve from theme");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Theme brushes only re-materialize when the root element's theme
		// changes — setting RequestedTheme on the container subtree is not
		// enough — so drive the XamlRoot content like an app theme switch.
		var root = container.XamlRoot?.Content as FrameworkElement;
		Assert.IsNotNull(root, "The loaded container should expose the XamlRoot content");
		var initialTheme = root.RequestedTheme;
		try
		{
			foreach (var (elementTheme, expected) in new[]
			{
				(ElementTheme.Light, DefaultPrimaryLight),
				(ElementTheme.Dark, DefaultPrimaryDark),
			})
			{
				root.RequestedTheme = elementTheme;
				await UnitTestsUIContentHelper.WaitForIdle();

				var bg = button.Background as SolidColorBrush;
				Assert.IsNotNull(bg, "Button should have a SolidColorBrush Background");

				Assert.AreEqual(expected, bg.Color,
					$"[{elementTheme}] Expected the default Material primary #{expected} but got #{bg.Color}. " +
					"A default MaterialTheme must not use seed-generated colors.");
			}
		}
		finally
		{
			root.RequestedTheme = initialTheme;
			await UnitTestsUIContentHelper.WaitForIdle();
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PrimarySeedExplicitlySet_Then_GenerationActivates()
	{
		// A seed far from the default palette guarantees the generated tones
		// differ from the default primaries. The exact generated value is
		// intentionally not pinned so the algorithm can evolve.
		var seed = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);
		var theme = new MaterialTheme
		{
			Colors = new ThemeColors { PrimarySeed = seed },
		};

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.IsTrue(
			container.Resources.TryGetValue("PrimaryColor", out var colorVal),
			"PrimaryColor should be resolvable from the theme");

		var color = (Color)colorVal;
		Assert.IsTrue(
			color != DefaultPrimaryLight && color != DefaultPrimaryDark,
			"Setting an explicit PrimarySeed should activate palette generation " +
			$"and replace the default Material primaries, but got #{color}.");
	}
}
