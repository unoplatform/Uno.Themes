using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_CupertinoHigSelection
{
	[TestMethod]
	[DataRow(Orientation.Horizontal)]
	[DataRow(Orientation.Vertical)]
	[RunsOnUIThread]
	public async Task When_SliderUsesNaturalThickness_Then_TouchTargetIs44Points(Orientation orientation)
	{
		var slider = new Slider
		{
			Orientation = orientation,
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top,
		};
		try
		{
			await Load(slider, "SliderStyle");
			var thickness = orientation == Orientation.Horizontal ? slider.ActualHeight : slider.ActualWidth;
			Assert.IsTrue(thickness >= 44, $"Slider touch target thickness was {thickness}");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static T Find<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && match.Name == name) return match;
			if (FindOrNull<T>(child, name) is { } nested) return nested;
		}
		throw new AssertFailedException($"Missing {name}");
	}

	private static T? FindOrNull<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && match.Name == name) return match;
			if (FindOrNull<T>(child, name) is { } nested) return nested;
		}
		return null;
	}

	private static async Task Load(Control control, string key)
	{
		var host = new Grid { Width = 400, Height = 150 };
		host.Resources.MergedDictionaries.Add(new CupertinoTheme { GlassRenderingMode = GlassRenderingMode.Liquid });
		control.Style = (Style)host.Resources[key];
		host.Children.Add(control);
		UnitTestsUIContentHelper.Content = host;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SwitchToggles_Then_ContemporaryCapsuleStaysInsideTrack()
	{
		var toggle = new ToggleSwitch();
		try
		{
			await Load(toggle, "ToggleSwitchStyle");
			var track = Find<Rectangle>(toggle, "OuterBorder");
			Assert.AreEqual(64d, track.ActualWidth);
			Assert.AreEqual(28d, track.ActualHeight);
			var knob = Find<Border>(toggle, "SwitchKnobOff");
			Assert.AreEqual(40d, knob.ActualWidth);
			Assert.AreEqual(24d, knob.ActualHeight);
			Assert.AreEqual(12d, knob.CornerRadius.TopLeft);
			var movingKnob = Find<Grid>(toggle, "SwitchKnob");
			Assert.AreEqual(0d, ((TranslateTransform)movingKnob.RenderTransform).X);
			toggle.IsOn = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			await Task.Delay(500); // the knob glides over CupertinoToggleDuration
			Assert.AreEqual(20d, ((TranslateTransform)movingKnob.RenderTransform).X);
			Assert.AreEqual(1d, Find<Border>(toggle, "SwitchKnobOn").Opacity);
		}
		finally { UnitTestsUIContentHelper.Content = null; }
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SwitchPressed_Then_GlassIsTransientAndDetachesAtRest()
	{
		var toggle = new ToggleSwitch();
		try
		{
			await Load(toggle, "ToggleSwitchStyle");
			var glass = Find<GlassPanel>(toggle, "SwitchThumbGlass");
			var knob = Find<Grid>(toggle, "KnobVisual");
			AssertRest(glass);
			AssertAtRest(knob, Find<Grid>(toggle, "SolidKnob"));
			Assert.IsTrue(VisualStateManager.GoToState(toggle, "Pressed", false));
			await WaitForLift();
			Assert.AreEqual(Visibility.Visible, glass.Visibility);
			Assert.AreEqual(GlassRenderingMode.Auto, glass.RenderingMode);
			AssertLifted(knob, glass);
			Assert.IsTrue(VisualStateManager.GoToState(toggle, "Normal", false));
			await WaitForLift();
			AssertRest(glass);
			AssertAtRest(knob, Find<Grid>(toggle, "SolidKnob"));
		}
		finally { UnitTestsUIContentHelper.Content = null; }
	}

	[TestMethod]
	[DataRow(Orientation.Horizontal)]
	[DataRow(Orientation.Vertical)]
	[RunsOnUIThread]
	public async Task When_SliderThumbPressed_Then_CapsuleGlassIsTransient(Orientation orientation)
	{
		var slider = new Slider { Orientation = orientation, Value = 50 };
		try
		{
			await Load(slider, "SliderStyle");
			var thumb = Find<Thumb>(slider, orientation == Orientation.Horizontal ? "HorizontalThumb" : "VerticalThumb");
			Assert.AreEqual(orientation == Orientation.Horizontal ? 40d : 24d, thumb.ActualWidth);
			Assert.AreEqual(orientation == Orientation.Horizontal ? 24d : 40d, thumb.ActualHeight);
			var glass = Find<GlassPanel>(thumb, "ThumbGlass");
			var visual = Find<Grid>(thumb, "ThumbVisual");
			AssertRest(glass);
			AssertAtRest(visual, Find<Grid>(thumb, "SolidThumb"));
			Assert.IsTrue(VisualStateManager.GoToState(thumb, "Pressed", false));
			await WaitForLift();
			Assert.AreEqual(Visibility.Visible, glass.Visibility);
			Assert.AreEqual(GlassRenderingMode.Auto, glass.RenderingMode);
			AssertLifted(visual, glass);
			Assert.IsTrue(VisualStateManager.GoToState(thumb, "Normal", false));
			await WaitForLift();
			AssertRest(glass);
			AssertAtRest(visual, Find<Grid>(thumb, "SolidThumb"));
		}
		finally { UnitTestsUIContentHelper.Content = null; }
	}

	// The lift storyboard runs 0.15 s with an overshoot; give it room to settle before reading the transform.
	private static Task WaitForLift() => Task.Delay(500);

	/// <summary>
	/// HIG Materials: a pressed knob lifts off its track — scaled up, as the Clear lens, whose glass draws the
	/// shadow (Given_GlassPanel checks that it lands). ThemeShadow cannot: it derives its silhouette from shape
	/// visuals and the backdrop-sampling backplate has none.
	/// </summary>
	private static void AssertLifted(FrameworkElement visual, GlassPanel glass)
	{
		var scale = (ScaleTransform)visual.RenderTransform;
		Assert.IsTrue(scale.ScaleX > 1.3 && scale.ScaleY > 1.3, $"Knob scale was {scale.ScaleX}×{scale.ScaleY}");
		Assert.AreEqual(GlassMaterial.Clear, glass.Material);
	}

	// At rest the solid knob is a plain shape, so it sits on a ThemeShadow.
	private static void AssertAtRest(FrameworkElement visual, FrameworkElement solidKnob)
	{
		var scale = (ScaleTransform)visual.RenderTransform;
		Assert.AreEqual(1d, scale.ScaleX, 0.01);
		Assert.AreEqual(1d, scale.ScaleY, 0.01);
		Assert.IsInstanceOfType<ThemeShadow>(solidKnob.Shadow);
		Assert.IsTrue(solidKnob.Translation.Z > 0, $"Rest Translation.Z was {solidKnob.Translation.Z}");
	}

	private static void AssertRest(GlassPanel glass)
	{
		Assert.AreEqual(Visibility.Collapsed, glass.Visibility);
		Assert.AreEqual(GlassRenderingMode.Solid, glass.RenderingMode, "A hidden thumb must not retain a per-frame backplate");
		Assert.AreEqual(GlassRenderingMode.Solid, glass.ActualRenderingMode);
	}
}
