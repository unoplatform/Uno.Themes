using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the semantic abstraction layer: semantic style/resource keys
/// (e.g. FilledButtonStyle, FilledButtonBackground) properly map to their
/// Simple-specific counterparts (e.g. SimplePrimaryButtonStyle, SimplePrimaryButtonBackground)
/// and that overrides at either level flow through to styled controls.
///
/// Each test creates a local SimpleTheme scoped to the test container,
/// so colors are fully controlled and independent of app-level resources.
/// </summary>
[TestClass]
public class Given_SemanticStyles
{
	private static Grid CreateThemedContainer()
	{
		var theme = new SimpleTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Style equivalence: semantic key and Simple-specific key produce
	// identical visual output (same Background & Foreground).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle", "SimplePrimaryButtonStyle")]
	[DataRow("FilledTonalButtonStyle", "SimpleNeutralButtonStyle")]
	[DataRow("OutlinedButtonStyle", "SimpleNeutralButtonStyle")]
	[DataRow("TextButtonStyle", "SimpleSubtleButtonStyle")]
	[DataRow("IconButtonStyle", "SimpleIconButtonPrimaryStyle")]
	public async Task When_SemanticAndSimpleStyles_AreApplied_LookIdentical(
		string semanticStyleKey, string simpleStyleKey)
	{
		var container = CreateThemedContainer();

		var semanticStyle = container.Resources[semanticStyleKey] as Style;
		var simpleStyle = container.Resources[simpleStyleKey] as Style;
		Assert.IsNotNull(semanticStyle, $"{semanticStyleKey} should be available");
		Assert.IsNotNull(simpleStyle, $"{simpleStyleKey} should be available");

		var semanticButton = new Button { Content = "Semantic", Style = semanticStyle };
		var simpleButton = new Button { Content = "Simple", Style = simpleStyle };

		var panel = new StackPanel();
		panel.Children.Add(semanticButton);
		panel.Children.Add(simpleButton);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(semanticButton);
		await UnitTestsUIContentHelper.WaitForLoaded(simpleButton);
		await UnitTestsUIContentHelper.WaitForIdle();

		var semanticBg = semanticButton.Background as SolidColorBrush;
		var simpleBg = simpleButton.Background as SolidColorBrush;
		Assert.IsNotNull(semanticBg, $"Button with {semanticStyleKey} should have a Background");
		Assert.IsNotNull(simpleBg, $"Button with {simpleStyleKey} should have a Background");
		Assert.AreEqual(semanticBg.Color, simpleBg.Color,
			$"Background of {semanticStyleKey} and {simpleStyleKey} should match");

		var semanticFg = semanticButton.Foreground as SolidColorBrush;
		var simpleFg = simpleButton.Foreground as SolidColorBrush;
		Assert.IsNotNull(semanticFg, $"Button with {semanticStyleKey} should have a Foreground");
		Assert.IsNotNull(simpleFg, $"Button with {simpleStyleKey} should have a Foreground");
		Assert.AreEqual(semanticFg.Color, simpleFg.Color,
			$"Foreground of {semanticStyleKey} and {simpleStyleKey} should match");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Overriding semantic resource keys (FilledButton*, FilledTonalButton*,
	// TextButton*) flows to buttons using EITHER style key.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle", "SimplePrimaryButtonStyle", "FilledButtonBackground", "FilledButtonForeground")]
	[DataRow("FilledTonalButtonStyle", "SimpleNeutralButtonStyle", "FilledTonalButtonBackground", "FilledTonalButtonForeground")]
	[DataRow("TextButtonStyle", "SimpleSubtleButtonStyle", "TextButtonBackground", "TextButtonForeground")]
	public async Task When_SemanticResourceOverridden_BothStylesReflectChange(
		string semanticStyleKey, string simpleStyleKey,
		string bgResourceKey, string fgResourceKey)
	{
		var expectedBg = Colors.DarkOrchid;
		var expectedFg = Colors.Crimson;

		var container = CreateThemedContainer();
		container.Resources[bgResourceKey] = new SolidColorBrush(expectedBg);
		container.Resources[fgResourceKey] = new SolidColorBrush(expectedFg);

		var semanticStyle = container.Resources[semanticStyleKey] as Style;
		var simpleStyle = container.Resources[simpleStyleKey] as Style;
		Assert.IsNotNull(semanticStyle, $"{semanticStyleKey} should be available");
		Assert.IsNotNull(simpleStyle, $"{simpleStyleKey} should be available");

		var semanticButton = new Button { Content = "Semantic", Style = semanticStyle };
		var simpleButton = new Button { Content = "Simple", Style = simpleStyle };

		var panel = new StackPanel();
		panel.Children.Add(semanticButton);
		panel.Children.Add(simpleButton);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(semanticButton);
		await UnitTestsUIContentHelper.WaitForLoaded(simpleButton);
		await UnitTestsUIContentHelper.WaitForIdle();

		var semanticBg = semanticButton.Background as SolidColorBrush;
		var simpleBg = simpleButton.Background as SolidColorBrush;
		Assert.IsNotNull(semanticBg);
		Assert.IsNotNull(simpleBg);
		Assert.AreEqual(expectedBg, semanticBg.Color,
			$"Overriding {bgResourceKey} should change {semanticStyleKey} button Background");
		Assert.AreEqual(expectedBg, simpleBg.Color,
			$"Overriding {bgResourceKey} should also change {simpleStyleKey} button Background");

		var semanticFg = semanticButton.Foreground as SolidColorBrush;
		var simpleFg = simpleButton.Foreground as SolidColorBrush;
		Assert.IsNotNull(semanticFg);
		Assert.IsNotNull(simpleFg);
		Assert.AreEqual(expectedFg, semanticFg.Color,
			$"Overriding {fgResourceKey} should change {semanticStyleKey} button Foreground");
		Assert.AreEqual(expectedFg, simpleFg.Color,
			$"Overriding {fgResourceKey} should also change {simpleStyleKey} button Foreground");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Overriding Simple-specific resource keys (SimplePrimaryButton*,
	// SimpleNeutralButton*, SimpleSubtleButton*) flows to buttons using
	// EITHER style key.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FilledButtonStyle", "SimplePrimaryButtonStyle", "SimplePrimaryButtonBackground", "SimplePrimaryButtonForeground")]
	[DataRow("FilledTonalButtonStyle", "SimpleNeutralButtonStyle", "SimpleNeutralButtonBackground", "SimpleNeutralButtonForeground")]
	[DataRow("TextButtonStyle", "SimpleSubtleButtonStyle", "SimpleSubtleButtonBackground", "SimpleSubtleButtonForeground")]
	public async Task When_SimpleResourceOverridden_BothStylesReflectChange(
		string semanticStyleKey, string simpleStyleKey,
		string bgResourceKey, string fgResourceKey)
	{
		var expectedBg = Colors.Teal;
		var expectedFg = Colors.Yellow;

		var container = CreateThemedContainer();
		container.Resources[bgResourceKey] = new SolidColorBrush(expectedBg);
		container.Resources[fgResourceKey] = new SolidColorBrush(expectedFg);

		var semanticStyle = container.Resources[semanticStyleKey] as Style;
		var simpleStyle = container.Resources[simpleStyleKey] as Style;
		Assert.IsNotNull(semanticStyle, $"{semanticStyleKey} should be available");
		Assert.IsNotNull(simpleStyle, $"{simpleStyleKey} should be available");

		var semanticButton = new Button { Content = "Semantic", Style = semanticStyle };
		var simpleButton = new Button { Content = "Simple", Style = simpleStyle };

		var panel = new StackPanel();
		panel.Children.Add(semanticButton);
		panel.Children.Add(simpleButton);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(semanticButton);
		await UnitTestsUIContentHelper.WaitForLoaded(simpleButton);
		await UnitTestsUIContentHelper.WaitForIdle();

		var semanticBg = semanticButton.Background as SolidColorBrush;
		var simpleBg = simpleButton.Background as SolidColorBrush;
		Assert.IsNotNull(semanticBg);
		Assert.IsNotNull(simpleBg);
		Assert.AreEqual(expectedBg, semanticBg.Color,
			$"Overriding {bgResourceKey} should change {semanticStyleKey} button Background");
		Assert.AreEqual(expectedBg, simpleBg.Color,
			$"Overriding {bgResourceKey} should also change {simpleStyleKey} button Background");

		var semanticFg = semanticButton.Foreground as SolidColorBrush;
		var simpleFg = simpleButton.Foreground as SolidColorBrush;
		Assert.IsNotNull(semanticFg);
		Assert.IsNotNull(simpleFg);
		Assert.AreEqual(expectedFg, semanticFg.Color,
			$"Overriding {fgResourceKey} should change {semanticStyleKey} button Foreground");
		Assert.AreEqual(expectedFg, simpleFg.Color,
			$"Overriding {fgResourceKey} should also change {simpleStyleKey} button Foreground");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Scope isolation: overrides in one container don't leak to siblings.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_Override_InNestedScope_DoesNotLeakToSibling()
	{
		var overrideColor = Colors.DarkOrchid;
		var root = new StackPanel();

		// Sibling A: has overridden SimplePrimaryButtonBackground
		var overriddenContainer = new Grid();
		overriddenContainer.Resources.MergedDictionaries.Add(new SimpleTheme());
		overriddenContainer.Resources["SimplePrimaryButtonBackground"] = new SolidColorBrush(overrideColor);
		var overriddenButton = new Button
		{
			Content = "Overridden",
			Style = overriddenContainer.Resources["SimplePrimaryButtonStyle"] as Style
		};
		overriddenContainer.Children.Add(overriddenButton);

		// Sibling B: default theme colors (no override)
		var defaultContainer = new Grid();
		defaultContainer.Resources.MergedDictionaries.Add(new SimpleTheme());
		var defaultButton = new Button
		{
			Content = "Default",
			Style = defaultContainer.Resources["SimplePrimaryButtonStyle"] as Style
		};
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
