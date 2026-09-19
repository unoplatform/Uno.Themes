using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the Cupertino control styles on realized controls: what a style key ends up rendering, and that
/// a lightweight-styling override reaches it.
/// </summary>
[TestClass]
public class Given_CupertinoControls
{
	private static Grid CreateThemedContainer()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new CupertinoTheme());
		return container;
	}

	private static T Resource<T>(FrameworkElement scope, string key)
		=> scope.Resources.TryGetValue(key, out var value) && value is T typed
			? typed
			: throw new AssertFailedException($"Resource '{key}' not found or not a {typeof(T).Name}");

	// A null key stands for "transparent": the framework's transparent brush is not a stable instance.
	private static void AssertBrush(FrameworkElement scope, string? expectedKey, Brush? actual, string what)
	{
		if (expectedKey is null)
		{
			Assert.AreEqual(0, (actual as SolidColorBrush)?.Color.A ?? 0, $"{what} should be transparent");
		}
		else
		{
			Assert.AreSame(Resource<Brush>(scope, expectedKey), actual, what);
		}
	}

	private static Task<Button> LoadButton(Grid container, string styleKey, bool isEnabled = true)
		=> Load(container, new Button { Content = "Button", IsEnabled = isEnabled }, styleKey);

	// A null style key leaves the control unstyled, so the theme's implicit style applies.
	private static async Task<T> Load<T>(Grid container, T control, string? styleKey)
		where T : Control
	{
		if (styleKey is not null)
		{
			control.Style = Resource<Style>(container, styleKey);
		}

		container.Children.Add(control);
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();
		return control;
	}

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

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TextButtonStyle", "PrimaryBrush", null)]
	[DataRow("FilledButtonStyle", "OnPrimaryBrush", "PrimaryBrush")]
	[DataRow("FilledTonalButtonStyle", "PrimaryBrush", "PrimaryContainerBrush")]
	[DataRow("OutlinedButtonStyle", "PrimaryBrush", "SecondaryContainerBrush")]
	[DataRow("ElevatedButtonStyle", "PrimaryBrush", "SecondaryContainerBrush")]
	[DataRow("CupertinoDestructiveButtonStyle", "ErrorBrush", null)]
	[DataRow("CupertinoButtonStyle", "PrimaryBrush", null)]
	[DataRow("CupertinoContainedButtonStyle", "OnPrimaryBrush", "PrimaryBrush")]
	public async Task When_ButtonStyleApplied_Then_RendersItsRoles(string styleKey, string foregroundKey, string? backgroundKey)
	{
		try
		{
			var container = CreateThemedContainer();

			var button = await LoadButton(container, styleKey);

			Assert.AreSame(Resource<Brush>(container, foregroundKey), button.Foreground, "Foreground");
			var root = FindDescendant<Grid>(button, "Root") ?? throw new AssertFailedException("template root missing");
			AssertBrush(container, backgroundKey, root.Background, "rendered Background");
			Assert.IsTrue(button.ActualHeight >= 44, $"Apple's minimum hit target is 44, was {button.ActualHeight}");
			Assert.IsNull(FindDescendant<GlassPanel>(button), "glass is opt-in, never behind a semantic or legacy key");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButtonBackgroundOverridden_Then_ButtonFollows()
	{
		try
		{
			var container = CreateThemedContainer();
			var custom = new SolidColorBrush(Colors.Magenta);
			container.Resources["FilledButtonBackground"] = custom;

			var button = await LoadButton(container, "FilledButtonStyle");

			Assert.AreSame(custom, FindDescendant<Grid>(button, "Root")?.Background);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ButtonDisabled_Then_WholeControlDims()
	{
		try
		{
			var container = CreateThemedContainer();

			var button = await LoadButton(container, "FilledButtonStyle", isEnabled: false);

			var root = FindDescendant<Grid>(button, "Root") ?? throw new AssertFailedException("template root missing");
			Assert.AreEqual(Resource<double>(container, "CupertinoDisableStateOpacity"), root.Opacity, 0.001);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("CupertinoGlassButtonStyle", false)]
	[DataRow("CupertinoGlassProminentButtonStyle", true)]
	[DataRow("FabStyle", true)]
	[DataRow("SecondaryFabStyle", false)]
	public async Task When_GlassButtonStyleApplied_Then_GlassCarriesTheTint(string styleKey, bool tinted)
	{
		try
		{
			var container = CreateThemedContainer();

			var button = await LoadButton(container, styleKey);

			var glass = FindDescendant<GlassPanel>(button) ?? throw new AssertFailedException("no GlassPanel in the template");
			AssertBrush(container, tinted ? "CupertinoGlassProminentTintBrush" : null, glass.Tint, "Tint");
			Assert.IsNull(FindDescendant<Grid>(button, "Root")?.Background, "the glass template paints no fill of its own");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("IconButtonStyle", 44)]
	[DataRow("TertiaryFabStyle", 44)]
	[DataRow("SurfaceLargeFabStyle", 56)]
	[DataRow("LargeFabStyle", 56)]
	public async Task When_IconButtonStyleApplied_Then_ItIsACircleOfThatSize(string styleKey, double size)
	{
		try
		{
			var button = await LoadButton(CreateThemedContainer(), styleKey);

			Assert.AreEqual(size, button.ActualWidth, 0.5);
			Assert.AreEqual(size, button.ActualHeight, 0.5);
			Assert.IsTrue(button.CornerRadius.TopLeft >= size / 2, "a full radius makes the square a circle");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TextToggleButtonStyle")]
	[DataRow("IconToggleButtonStyle")]
	[DataRow(null)]
	public async Task When_ToggleButtonChecked_Then_TakesTheAccentTint(string? styleKey)
	{
		try
		{
			var container = CreateThemedContainer();
			var toggle = await Load(container, new ToggleButton { Content = "Toggle" }, styleKey);
			var root = FindDescendant<Grid>(toggle, "Root") ?? throw new AssertFailedException("template root missing");
			Assert.AreSame(Resource<Brush>(container, "SecondaryContainerBrush"), root.Background, "unchecked");

			toggle.IsChecked = true;
			await UnitTestsUIContentHelper.WaitForIdle();

			// By color: a visual-state setter resolves its ThemeResource against the application's theme, which
			// holds other brush instances than this scoped one.
			Assert.AreEqual(Resource<SolidColorBrush>(container, "PrimaryContainerBrush").Color, (root.Background as SolidColorBrush)?.Color, "checked Background");
			var presenter = FindDescendant<ContentPresenter>(toggle, "ContentPresenter");
			Assert.AreEqual(Resource<SolidColorBrush>(container, "PrimaryBrush").Color, (presenter?.Foreground as SolidColorBrush)?.Color, "checked Foreground");

			toggle.IsChecked = false;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreSame(Resource<Brush>(container, "SecondaryContainerBrush"), root.Background, "unchecked again");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("HyperlinkButtonStyle", "PrimaryBrush")]
	[DataRow("SecondaryHyperlinkButtonStyle", "OnSurfaceVariantBrush")]
	[DataRow(null, "PrimaryBrush")]
	public async Task When_HyperlinkButtonStyleApplied_Then_RendersItsRole(string? styleKey, string foregroundKey)
	{
		try
		{
			var container = CreateThemedContainer();

			var link = await Load(container, new HyperlinkButton { Content = "Link" }, styleKey);

			Assert.AreSame(Resource<Brush>(container, foregroundKey), link.Foreground);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledTextFieldStylesApplied_Then_FilledAndBorderless()
	{
		try
		{
			var container = CreateThemedContainer();
			var fill = Resource<Brush>(container, "SecondaryContainerBrush");

			var text = await Load(container, new TextBox(), "FilledTextBoxStyle");
			var password = await Load(container, new PasswordBox(), "FilledPasswordBoxStyle");

			Assert.AreSame(fill, text.Background, "TextBox Background");
			Assert.AreEqual(new Thickness(0), text.BorderThickness, "TextBox BorderThickness");
			Assert.AreSame(Resource<Brush>(container, "OnSurfaceVariantBrush"), text.PlaceholderForeground, "TextBox PlaceholderForeground");
			Assert.AreSame(fill, FindDescendant<Border>(text, "ContentBorder")?.Background, "rendered TextBox fill");
			Assert.AreSame(fill, password.Background, "PasswordBox Background");
			Assert.AreEqual(new Thickness(0), password.BorderThickness, "PasswordBox BorderThickness");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// The activity indicator is eight spokes turning in 45 degree steps; it no longer needs Lottie.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("ProgressRingStyle", 20)]
	[DataRow("CupertinoLargeProgressRingStyle", 37)]
	[DataRow(null, 20)]
	public async Task When_ProgressRingActive_Then_EightSpokesStepAround(string? styleKey, double size)
	{
		try
		{
			var ring = await Load(CreateThemedContainer(), new ProgressRing { IsActive = true }, styleKey);

			Assert.AreEqual(size, ring.ActualWidth, 0.5);
			var spokes = FindDescendant<Grid>(ring, "Spokes") ?? throw new AssertFailedException("no spokes in the template");
			Assert.AreEqual(8, spokes.Children.Count);
			var rotation = (RotateTransform)spokes.RenderTransform;

			var seen = new HashSet<double>();
			for (var i = 0; i < 20 && seen.Count < 3; i++)
			{
				seen.Add(rotation.Angle);
				await Task.Delay(60);
			}

			Assert.IsTrue(seen.Count >= 3, $"the ring should be turning, saw only {string.Join(", ", seen)}");
			Assert.IsTrue(seen.All(a => a % 45 == 0), $"the ring turns in whole spokes, saw {string.Join(", ", seen)}");

			ring.IsActive = false;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(0, spokes.Opacity, "an inactive ring is hidden");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("ProgressBarStyle")]
	[DataRow(null)]
	public async Task When_ProgressBarStyleApplied_Then_ThinAccentBarThatCanStretch(string? styleKey)
	{
		try
		{
			var container = CreateThemedContainer();
			container.Width = 400;

			var bar = await Load(container, new ProgressBar { Value = 50, HorizontalAlignment = HorizontalAlignment.Stretch }, styleKey);

			Assert.AreSame(Resource<Brush>(container, "PrimaryBrush"), bar.Foreground, "Foreground");
			Assert.AreSame(Resource<Brush>(container, "OutlineVariantBrush"), bar.Background, "Background");
			Assert.AreEqual(4, bar.ActualHeight, 0.5);
			Assert.AreEqual(400, bar.ActualWidth, 0.5, "the bar used to be pinned to 250 px");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
