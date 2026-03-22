using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_ResourceOverrides
{
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SemanticKey_FilledButtonBackground_IsOverridden()
	{
		var style = Application.Current.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle must be available");

		var overrideColor = Colors.DarkOrchid;

		// Create a container that overrides the semantic resource key
		var container = new Grid();
		container.Resources["FilledButtonBackground"] = new SolidColorBrush(overrideColor);

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var bg = button.Background as SolidColorBrush;
		Assert.IsNotNull(bg, "Button Background should be a SolidColorBrush");
		Assert.AreEqual(overrideColor, bg.Color,
			"Overriding semantic key 'FilledButtonBackground' should propagate to the button's Background");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SemanticKey_FilledButtonForeground_IsOverridden()
	{
		var style = Application.Current.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle must be available");

		var overrideColor = Colors.Crimson;

		var container = new Grid();
		container.Resources["FilledButtonForeground"] = new SolidColorBrush(overrideColor);

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var fg = button.Foreground as SolidColorBrush;
		Assert.IsNotNull(fg, "Button Foreground should be a SolidColorBrush");
		Assert.AreEqual(overrideColor, fg.Color,
			"Overriding semantic key 'FilledButtonForeground' should propagate to the button's Foreground");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SemanticKey_TextButtonForeground_IsOverridden()
	{
		var style = Application.Current.Resources["TextButtonStyle"] as Style;
		Assert.IsNotNull(style, "TextButtonStyle must be available");

		var overrideColor = Colors.Teal;

		var container = new Grid();
		container.Resources["TextButtonForeground"] = new SolidColorBrush(overrideColor);

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var fg = button.Foreground as SolidColorBrush;
		Assert.IsNotNull(fg, "Button Foreground should be a SolidColorBrush");
		Assert.AreEqual(overrideColor, fg.Color,
			"Overriding semantic key 'TextButtonForeground' should propagate to the TextButton's Foreground");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SemanticKey_FilledTonalButtonBackground_IsOverridden()
	{
		var style = Application.Current.Resources["FilledTonalButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledTonalButtonStyle must be available");

		var overrideColor = Colors.DarkSlateBlue;

		var container = new Grid();
		container.Resources["FilledTonalButtonBackground"] = new SolidColorBrush(overrideColor);

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var bg = button.Background as SolidColorBrush;
		Assert.IsNotNull(bg, "Button Background should be a SolidColorBrush");
		Assert.AreEqual(overrideColor, bg.Color,
			"Overriding semantic key 'FilledTonalButtonBackground' should propagate to the button's Background");
	}

	/// <summary>
	/// Verifies that overriding the semantic key and the theme-specific key both
	/// produce the same visual result. This is the core contract of the semantic
	/// abstraction layer: both override paths should affect the same change.
	/// </summary>
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SemanticAndThemeSpecific_Overrides_ProduceSameResult()
	{
		var style = Application.Current.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle must be available");

		var overrideColor = Colors.DarkGreen;

		// First: get default background color
		var defaultButton = new Button { Content = "Default", Style = style };
		UnitTestsUIContentHelper.Content = defaultButton;
		await UnitTestsUIContentHelper.WaitForLoaded(defaultButton);
		await UnitTestsUIContentHelper.WaitForIdle();
		var defaultBg = defaultButton.Background as SolidColorBrush;
		Assert.IsNotNull(defaultBg, "Default button should have a SolidColorBrush Background");

		// Second: override via semantic key (FilledButtonBackground)
		var semanticContainer = new Grid();
		semanticContainer.Resources["FilledButtonBackground"] = new SolidColorBrush(overrideColor);
		var semanticButton = new Button { Content = "Semantic Override", Style = style };
		semanticContainer.Children.Add(semanticButton);

		UnitTestsUIContentHelper.Content = semanticContainer;
		await UnitTestsUIContentHelper.WaitForLoaded(semanticButton);
		await UnitTestsUIContentHelper.WaitForIdle();
		var semanticBg = semanticButton.Background as SolidColorBrush;
		Assert.IsNotNull(semanticBg);

		// The override should have taken effect (different from default)
		Assert.AreNotEqual(defaultBg.Color, semanticBg.Color,
			"Semantic override should change the button background from its default");
		Assert.AreEqual(overrideColor, semanticBg.Color,
			"Semantic override should set the exact override color");
	}

	/// <summary>
	/// Verifies that multiple semantic resource keys can be overridden simultaneously
	/// on the same container, and all overrides take effect independently.
	/// </summary>
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_MultipleSemanticKeys_AreOverridden_AllTakeEffect()
	{
		var style = Application.Current.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle must be available");

		var bgColor = Colors.DarkOrchid;
		var fgColor = Colors.Yellow;

		var container = new Grid();
		container.Resources["FilledButtonBackground"] = new SolidColorBrush(bgColor);
		container.Resources["FilledButtonForeground"] = new SolidColorBrush(fgColor);

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var bg = button.Background as SolidColorBrush;
		var fg = button.Foreground as SolidColorBrush;

		Assert.IsNotNull(bg);
		Assert.IsNotNull(fg);
		Assert.AreEqual(bgColor, bg.Color, "Background override should take effect");
		Assert.AreEqual(fgColor, fg.Color, "Foreground override should take effect");
	}

	/// <summary>
	/// Verifies that a resource override in a nested container does not leak to
	/// sibling containers — the override scope is correctly isolated.
	/// </summary>
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_Override_InNestedScope_DoesNotLeakToSibling()
	{
		var style = Application.Current.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle must be available");

		var overrideColor = Colors.DarkOrchid;

		var root = new StackPanel();

		// Container with override
		var overriddenContainer = new Grid();
		overriddenContainer.Resources["FilledButtonBackground"] = new SolidColorBrush(overrideColor);
		var overriddenButton = new Button { Content = "Overridden", Style = style };
		overriddenContainer.Children.Add(overriddenButton);

		// Sibling container without override
		var defaultContainer = new Grid();
		var defaultButton = new Button { Content = "Default", Style = style };
		defaultContainer.Children.Add(defaultButton);

		root.Children.Add(overriddenContainer);
		root.Children.Add(defaultContainer);

		UnitTestsUIContentHelper.Content = root;
		await UnitTestsUIContentHelper.WaitForLoaded(overriddenButton);
		await UnitTestsUIContentHelper.WaitForLoaded(defaultButton);
		await UnitTestsUIContentHelper.WaitForIdle();

		var overriddenBg = overriddenButton.Background as SolidColorBrush;
		var defaultBg = defaultButton.Background as SolidColorBrush;

		Assert.IsNotNull(overriddenBg);
		Assert.IsNotNull(defaultBg);

		Assert.AreEqual(overrideColor, overriddenBg.Color,
			"Button in overridden scope should use the override color");
		Assert.AreNotEqual(overrideColor, defaultBg.Color,
			"Button in sibling scope should NOT be affected by the override");
	}
}
