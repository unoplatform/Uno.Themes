using System.Threading.Tasks;
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

	private static async Task<Button> LoadButton(Grid container, string styleKey, bool isEnabled = true)
	{
		var button = new Button { Content = "Button", Style = Resource<Style>(container, styleKey), IsEnabled = isEnabled };
		container.Children.Add(button);
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();
		return button;
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
}
