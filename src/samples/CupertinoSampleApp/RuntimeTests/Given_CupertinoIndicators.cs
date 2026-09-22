using System.Threading.Tasks;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the page dots and rating glyphs on realized Cupertino controls.
/// </summary>
[TestClass]
public class Given_CupertinoIndicators
{
	private static Grid CreateThemedContainer(ElementTheme theme)
	{
		var container = new Grid { RequestedTheme = theme };
		container.Resources.MergedDictionaries.Add(new CupertinoTheme());
		return container;
	}

	private static T Resource<T>(FrameworkElement scope, string key)
		=> scope.Resources.TryGetValue(key, out var value) && value is T typed
			? typed
			: throw new AssertFailedException($"Resource '{key}' not found or not a {typeof(T).Name}");

	private static T? FindDescendant<T>(DependencyObject root, string? name = null)
		where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && (name is null || match.Name == name))
			{
				return match;
			}

			if (FindDescendant<T>(child, name) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}

	private static Color? ColorOf(Brush? brush) => (brush as SolidColorBrush)?.Color;

	[TestMethod]
	[DataRow(ElementTheme.Light, Orientation.Horizontal)]
	[DataRow(ElementTheme.Dark, Orientation.Horizontal)]
	[DataRow(ElementTheme.Light, Orientation.Vertical)]
	[DataRow(ElementTheme.Dark, Orientation.Vertical)]
	[RunsOnUIThread]
	public async Task When_PagerSelectionChanges_Then_SevenPointDotsFollowSelection(ElementTheme theme, Orientation orientation)
	{
		var container = CreateThemedContainer(theme);
		var pager = new PipsPager
		{
			Style = Resource<Style>(container, "PipsPagerStyle"),
			NumberOfPages = 5,
			SelectedPageIndex = 2,
			Orientation = orientation,
		};
		container.Children.Add(pager);
		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(pager);
			await UnitTestsUIContentHelper.WaitForIdle();

			var repeater = FindDescendant<ItemsRepeater>(pager, "PipsPagerItemsRepeater")
				?? throw new AssertFailedException("The pager did not realize its pips.");
			AssertPip(2, "OnSurfaceBrush");
			AssertPip(0, "OnSurfaceLowBrush");

			pager.SelectedPageIndex = 4;
			await UnitTestsUIContentHelper.WaitForIdle();
			AssertPip(2, "OnSurfaceLowBrush");
			AssertPip(4, "OnSurfaceBrush");

			void AssertPip(int index, string brushKey)
			{
				var pip = repeater.TryGetElement(index) as Button
					?? throw new AssertFailedException($"Pip {index} was not realized.");
				var dot = FindDescendant<Ellipse>(pip)
					?? throw new AssertFailedException($"Pip {index} has no dot.");
				Assert.AreEqual(16d, pip.ActualWidth, "pip width");
				Assert.AreEqual(16d, pip.ActualHeight, "pip height");
				Assert.AreEqual(7d, dot.ActualWidth, "dot width");
				Assert.AreEqual(7d, dot.ActualHeight, "dot height");
				// Direct dictionary lookup uses the application appearance, not the container's.
				Assert.AreEqual(theme == ElementTheme.Light ? Colors.Black : Colors.White, ColorOf(dot.Fill), "dot colour");
				Assert.AreEqual(Resource<SolidColorBrush>(container, brushKey).Opacity, dot.Fill.Opacity, "dot opacity distinguishes selection");
			}
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	[RunsOnUIThread]
	public async Task When_RatingStatesChange_Then_StockGlyphsUseCupertinoBrushes(ElementTheme theme)
	{
		var container = CreateThemedContainer(theme);
		var rating = new RatingControl
		{
			Style = Resource<Style>(container, "RatingControlStyle"),
			Value = 3,
			Caption = "Three stars",
		};
		// Resolve expectations through ThemeResource on loaded elements, so they follow the
		// container's appearance rather than ResourceDictionary.TryGetValue's application scope.
		var probes = (Grid)Microsoft.UI.Xaml.Markup.XamlReader.Load("""
			<Grid xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
			      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" Width="0" Height="0">
				<Border x:Name="PrimaryBrush" Background="{ThemeResource PrimaryBrush}" />
				<Border x:Name="SecondaryBrush" Background="{ThemeResource SecondaryBrush}" />
				<Border x:Name="OnSurfaceVariantBrush" Background="{ThemeResource OnSurfaceVariantBrush}" />
				<Border x:Name="OnSurfaceDisabledBrush" Background="{ThemeResource OnSurfaceDisabledBrush}" />
			</Grid>
			""");
		container.Children.Add(probes);
		container.Children.Add(rating);
		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(rating);
			await UnitTestsUIContentHelper.WaitForIdle();

			// Uno ResetControlSize assigns a local Height of 32; its glyphs render at half scale.
			Assert.AreEqual(32d, rating.ActualHeight);
			var foreground = FindDescendant<ContentPresenter>(rating, "ForegroundContentPresenter")
				?? throw new AssertFailedException("The rating did not realize its foreground presenter.");
			AssertColour("PrimaryBrush");
			var background = FindDescendant<StackPanel>(rating, "RatingBackgroundStackPanel")
				?? throw new AssertFailedException("The rating did not realize its background glyphs.");
			var glyph = FindDescendant<TextBlock>(background)
				?? throw new AssertFailedException("The rating has no unselected glyph.");
			Assert.AreEqual(ColorOf(ExpectedBrush("SecondaryBrush")), ColorOf(glyph.Foreground));

			Assert.IsTrue(VisualStateManager.GoToState(rating, "PointerOverUnselected", false));
			await UnitTestsUIContentHelper.WaitForIdle();
			AssertColour("OnSurfaceVariantBrush");
			rating.IsEnabled = false;
			await UnitTestsUIContentHelper.WaitForIdle();
			AssertColour("OnSurfaceDisabledBrush");

			Brush ExpectedBrush(string brushKey) => ((Border)probes.FindName(brushKey)).Background;

			void AssertColour(string brushKey)
			{
				var expected = ExpectedBrush(brushKey);
				Assert.AreEqual(ColorOf(expected), ColorOf(foreground.Foreground), brushKey);
				Assert.AreEqual(expected.Opacity, foreground.Foreground.Opacity, brushKey + " opacity");
			}
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
