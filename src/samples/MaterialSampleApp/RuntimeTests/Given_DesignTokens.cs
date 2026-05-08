using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.Themes;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies design token APIs for MaterialTheme: spacing scale, shape scale,
/// thickness companions, fixed tokens, runtime switching, and visual integration.
/// </summary>
[TestClass]
public class Given_DesignTokens
{
	// ─────────────────────────────────────────────────────────────────────
	// Helpers
	// ─────────────────────────────────────────────────────────────────────

	private static (Grid container, MaterialTheme theme) CreateThemedContainer(
		Density density = Density.Regular,
		double cornerRadius = 4.0)
	{
		var theme = new MaterialTheme
		{
			DefaultDensity = density,
			DefaultCornerRadius = cornerRadius,
		};
		// Construction-time rebuild is dispatcher-deferred for coalescing; flush it
		// so the synchronous resource queries below see the fully-populated theme.
		theme.EnsureInitialized();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return (container, theme);
	}

	private static T GetResource<T>(Grid container, string key)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is T typed)
		{
			return typed;
		}

		Assert.Fail($"Resource '{key}' not found or not of type {typeof(T).Name}");
		return default!;
	}

	private static bool TryGetResource<T>(Grid container, string key, out T result)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is T typed)
		{
			result = typed;
			return true;
		}
		result = default!;
		return false;
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 1. SPACING SCALE — formula: base × multiplier (representative subset)
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(Density.Compact, "Space0", 0.0)]    // zero is always zero
	[DataRow(Density.Compact, "Space100", 3.0)]  // base=3 × 1
	[DataRow(Density.Compact, "Space400", 12.0)] // base=3 × 4
	[DataRow(Density.Regular, "Space100", 4.0)]  // base=4 × 1
	[DataRow(Density.Regular, "Space200", 8.0)]  // base=4 × 2
	[DataRow(Density.Regular, "Space4000", 160.0)] // base=4 × 40
	[DataRow(Density.Comfy, "Space100", 5.0)]    // base=5 × 1
	[DataRow(Density.Comfy, "Space050", 2.5)]    // base=5 × 0.5
	[DataRow(Density.Comfy, "Space800", 40.0)]   // base=5 × 8
	public void When_DensitySet_Then_SpaceTokenHasCorrectValue(
		Density density, string tokenKey, double expected)
	{
		var (container, _) = CreateThemedContainer(density);
		var actual = GetResource<double>(container, tokenKey);
		Assert.AreEqual(expected, actual, 0.001,
			$"{tokenKey} at {density}: expected {expected}, got {actual}");
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 2. THICKNESS COMPANIONS (uniform, horizontal, directional)
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public void When_DensitySet_Then_ThicknessCompanionsAreCorrect()
	{
		var (container, _) = CreateThemedContainer(Density.Regular);

		// Uniform: all sides equal
		Assert.AreEqual(new Thickness(8), GetResource<Thickness>(container, "Space200Thickness"));

		// Horizontal: left/right only
		Assert.AreEqual(new Thickness(16, 0, 16, 0), GetResource<Thickness>(container, "Space400HorizontalThickness"));

		// Vertical
		Assert.AreEqual(new Thickness(0, 4, 0, 4), GetResource<Thickness>(container, "Space100VerticalThickness"));

		// Directional singles
		Assert.AreEqual(new Thickness(0, 8, 0, 0), GetResource<Thickness>(container, "Space200TopThickness"));
		Assert.AreEqual(new Thickness(0, 0, 0, 8), GetResource<Thickness>(container, "Space200BottomThickness"));
		Assert.AreEqual(new Thickness(4, 0, 0, 0), GetResource<Thickness>(container, "Space100LeftThickness"));
		Assert.AreEqual(new Thickness(0, 0, 4, 0), GetResource<Thickness>(container, "Space100RightThickness"));
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_HighScaleVariants_Then_NoDirectionalCompanions()
	{
		var (container, _) = CreateThemedContainer(Density.Regular);

		// Variants above 800 have uniform Thickness but no directional ones
		Assert.IsTrue(TryGetResource<Thickness>(container, "Space1200Thickness", out _));
		Assert.IsFalse(TryGetResource<Thickness>(container, "Space1200HorizontalThickness", out _));
		Assert.IsFalse(TryGetResource<Thickness>(container, "Space1200VerticalThickness", out _));
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 3. SHAPE SCALE — formula: base × multiplier
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(4.0, "Radius0", 0.0)]
	[DataRow(4.0, "Radius100", 4.0)]
	[DataRow(4.0, "Radius200", 8.0)]
	[DataRow(4.0, "Radius500", 20.0)]
	[DataRow(6.0, "Radius100", 6.0)]
	[DataRow(6.0, "Radius300", 18.0)]
	[DataRow(4.0, "RadiusFull", 9999.0)]
	[DataRow(10.0, "RadiusFull", 9999.0)]
	public void When_CornerRadiusSet_Then_RadiusTokenHasCorrectValue(
		double baseRadius, string tokenKey, double expected)
	{
		var (container, _) = CreateThemedContainer(cornerRadius: baseRadius);
		var actual = GetResource<double>(container, tokenKey);
		Assert.AreEqual(expected, actual, 0.001,
			$"{tokenKey} at base={baseRadius}: expected {expected}, got {actual}");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_CornerRadiusSet_Then_CornerRadiusCompanionsExist()
	{
		var (container, _) = CreateThemedContainer(cornerRadius: 4.0);

		Assert.AreEqual(new CornerRadius(0), GetResource<CornerRadius>(container, "Radius0CornerRadius"));
		Assert.AreEqual(new CornerRadius(4), GetResource<CornerRadius>(container, "Radius100CornerRadius"));
		Assert.AreEqual(new CornerRadius(8), GetResource<CornerRadius>(container, "Radius200CornerRadius"));
		Assert.AreEqual(new CornerRadius(9999), GetResource<CornerRadius>(container, "RadiusFullCornerRadius"));
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 4. FIXED TOKENS — invariant across densities
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(Density.Compact)]
	[DataRow(Density.Regular)]
	[DataRow(Density.Comfy)]
	public void When_DensityChanges_Then_FixedTokensAreConstant(Density density)
	{
		var (container, _) = CreateThemedContainer(density);

		Assert.AreEqual(32.0, GetResource<double>(container, "ControlHeightSmall"), 0.001);
		Assert.AreEqual(40.0, GetResource<double>(container, "ControlHeightMedium"), 0.001);
		Assert.AreEqual(44.0, GetResource<double>(container, "ControlHeightMediumLarge"), 0.001);
		Assert.AreEqual(48.0, GetResource<double>(container, "ControlHeightLarge"), 0.001);
		Assert.AreEqual(16.0, GetResource<double>(container, "IconSizeSmall"), 0.001);
		Assert.AreEqual(24.0, GetResource<double>(container, "IconSizeMedium"), 0.001);
		Assert.AreEqual(32.0, GetResource<double>(container, "IconSizeLarge"), 0.001);
		Assert.AreEqual(48.0, GetResource<double>(container, "TouchTargetMinSize"), 0.001);
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 5. RUNTIME SWITCHING
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public void When_DensitySwitchedAtRuntime_Then_SpacingTokensUpdate()
	{
		var (container, theme) = CreateThemedContainer(Density.Regular);
		Assert.AreEqual(4.0, GetResource<double>(container, "Space100"), 0.001);

		theme.DefaultDensity = Density.Compact;
		Assert.AreEqual(3.0, GetResource<double>(container, "Space100"), 0.001);

		theme.DefaultDensity = Density.Comfy;
		Assert.AreEqual(5.0, GetResource<double>(container, "Space100"), 0.001);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_CornerRadiusSwitchedAtRuntime_Then_ShapeTokensUpdate()
	{
		var (container, theme) = CreateThemedContainer(cornerRadius: 4.0);
		Assert.AreEqual(8.0, GetResource<double>(container, "Radius200"), 0.001);

		theme.DefaultCornerRadius = 6.0;
		Assert.AreEqual(12.0, GetResource<double>(container, "Radius200"), 0.001);

		// RadiusFull remains constant
		Assert.AreEqual(9999.0, GetResource<double>(container, "RadiusFull"), 0.001);
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 6. INDEPENDENCE — changing one axis doesn't affect the other
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public void When_DensityChanges_Then_ShapeTokensAreUnaffected()
	{
		var (container, theme) = CreateThemedContainer(Density.Regular, cornerRadius: 4.0);
		Assert.AreEqual(8.0, GetResource<double>(container, "Radius200"), 0.001);

		theme.DefaultDensity = Density.Compact;
		Assert.AreEqual(8.0, GetResource<double>(container, "Radius200"), 0.001);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_CornerRadiusChanges_Then_SpacingTokensAreUnaffected()
	{
		var (container, theme) = CreateThemedContainer(Density.Regular, cornerRadius: 4.0);
		Assert.AreEqual(8.0, GetResource<double>(container, "Space200"), 0.001);

		theme.DefaultCornerRadius = 10.0;
		Assert.AreEqual(8.0, GetResource<double>(container, "Space200"), 0.001);
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 7. EDGE CASES
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public void When_InvalidDensityEnum_Then_FallsBackToRegular()
	{
		var theme = new MaterialTheme { DefaultDensity = (Density)99 };
		theme.EnsureInitialized();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(4.0, GetResource<double>(container, "Space100"), 0.001,
			"Invalid Density should fall back to base=4.0");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_CornerRadiusIsZero_Then_AllRadiusTokensAreZeroExceptFull()
	{
		var (container, _) = CreateThemedContainer(cornerRadius: 0.0);

		Assert.AreEqual(0.0, GetResource<double>(container, "Radius100"), 0.001);
		Assert.AreEqual(0.0, GetResource<double>(container, "Radius500"), 0.001);
		Assert.AreEqual(9999.0, GetResource<double>(container, "RadiusFull"), 0.001);
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 8. VISUAL INTEGRATION — render actual controls, verify layout values
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButton_AtRegularDensity_Then_PaddingAndCornerRadiusAreCorrect()
	{
		var (container, _) = CreateThemedContainer(Density.Regular, cornerRadius: 4.0);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should exist");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// ButtonPadding = Space400HorizontalThickness = Thickness(16,0,16,0) at Regular
		Assert.AreEqual(new Thickness(16, 0, 16, 0), button.Padding,
			"FilledButton Padding should be Space400HorizontalThickness at Regular density");

		// ButtonCornerRadius = Radius500CornerRadius = CornerRadius(20) at base=4
		Assert.AreEqual(new CornerRadius(20), button.CornerRadius,
			"FilledButton CornerRadius should be Radius500CornerRadius (4×5=20)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButton_AtCompactDensity_Then_PaddingIsTighter()
	{
		var (container, _) = CreateThemedContainer(Density.Compact, cornerRadius: 4.0);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should exist");

		var button = new Button { Content = "Compact", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// ButtonPadding = Space400HorizontalThickness at Compact: base=3 × 4 = 12
		Assert.AreEqual(new Thickness(12, 0, 12, 0), button.Padding,
			"FilledButton Padding should shrink to 12 at Compact density");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButton_WithCustomCornerRadius_Then_ButtonReflectsIt()
	{
		var (container, _) = CreateThemedContainer(Density.Regular, cornerRadius: 6.0);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should exist");

		var button = new Button { Content = "Rounded", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// ButtonCornerRadius = Radius500CornerRadius = CornerRadius(6×5=30)
		Assert.AreEqual(new CornerRadius(30), button.CornerRadius,
			"FilledButton CornerRadius should be 30 at base=6 (Radius500 = 6×5)");
	}

}
