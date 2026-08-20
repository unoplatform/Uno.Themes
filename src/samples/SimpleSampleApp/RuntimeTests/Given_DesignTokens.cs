using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies design token APIs for SimpleTheme: spacing scale, shape scale,
/// thickness companions, fixed tokens, runtime switching, and visual integration.
/// </summary>
[TestClass]
public class Given_DesignTokens
{
	// ─────────────────────────────────────────────────────────────────────
	// Helpers
	// ─────────────────────────────────────────────────────────────────────

	private static (Grid container, SimpleTheme theme) CreateThemedContainer(
		Density density = Density.Regular,
		double cornerRadius = 4.0,
		double spacing = double.NaN)
	{
		var theme = new SimpleTheme
		{
			DefaultDensity = density,
			DefaultCornerRadius = cornerRadius,
			DefaultSpacing = spacing,
		};
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
	// 5. RUNTIME SWITCHING — of the *token resources* only.
	//
	// These assert that assigning DefaultDensity / DefaultCornerRadius regenerates the Space* and
	// Radius* resources. They deliberately do NOT assert anything about rendered controls, because
	// controls do not restyle: the per-control keys that consume these tokens (ButtonCornerRadius,
	// ButtonPadding, …) are resolved once when the theme's control-style dictionaries are parsed,
	// and CornerRadius/Thickness are values with no live instance to update. Both properties are
	// documented as construction-time settings (see BaseTheme and doc/design-tokens.md); to change
	// them at runtime an app must recreate its root content.
	//
	// Do not "extend" these into control-level assertions expecting them to pass.
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
		var theme = new SimpleTheme { DefaultDensity = (Density)99 };
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
	// 8. DEFAULT SPACING — base unit; the density mode scales it (×0.75/×1/×1.25)
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(6.0, "Space0", 0.0)]     // zero is always zero
	[DataRow(6.0, "Space050", 3.0)]   // base=6 × 0.5
	[DataRow(6.0, "Space100", 6.0)]   // base=6 × 1
	[DataRow(6.0, "Space400", 24.0)]  // base=6 × 4
	[DataRow(2.5, "Space200", 5.0)]   // fractional base
	[DataRow(0.0, "Space100", 0.0)]   // zero is a valid base
	public void When_DefaultSpacingSet_Then_SpaceTokenHasCorrectValue(
		double spacing, string tokenKey, double expected)
	{
		var (container, _) = CreateThemedContainer(Density.Regular, spacing: spacing);
		var actual = GetResource<double>(container, tokenKey);
		Assert.AreEqual(expected, actual, 0.001,
			$"{tokenKey} at DefaultSpacing={spacing}: expected {expected}, got {actual}");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(Density.Compact, 6.0, 4.5)]  // 6 × 0.75
	[DataRow(Density.Regular, 6.0, 6.0)]  // 6 × 1
	[DataRow(Density.Comfy, 6.0, 7.5)]    // 6 × 1.25
	[DataRow(Density.Compact, 8.0, 6.0)]  // 8 × 0.75
	public void When_DefaultSpacingAndDensitySet_Then_TheyCompose(
		Density density, double spacing, double expectedBase)
	{
		// Density is a mode over the spacing base unit, not a competing setting:
		// effective base = DefaultSpacing × density factor.
		var (container, _) = CreateThemedContainer(density, spacing: spacing);
		Assert.AreEqual(expectedBase, GetResource<double>(container, "Space100"), 0.001);
		Assert.AreEqual(expectedBase * 2, GetResource<double>(container, "Space200"), 0.001);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultSpacingSet_Then_ThicknessCompanionsDeriveFromIt()
	{
		var (container, _) = CreateThemedContainer(Density.Regular, spacing: 6.0);

		Assert.AreEqual(new Thickness(12), GetResource<Thickness>(container, "Space200Thickness"));
		Assert.AreEqual(new Thickness(6, 0, 6, 0), GetResource<Thickness>(container, "Space100HorizontalThickness"));
		Assert.AreEqual(new Thickness(0, 6, 0, 0), GetResource<Thickness>(container, "Space100TopThickness"));
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultSpacingChangedAtRuntime_Then_DensityModeStillApplies()
	{
		var (container, theme) = CreateThemedContainer(Density.Comfy);
		Assert.AreEqual(5.0, GetResource<double>(container, "Space100"), 0.001,
			"Default base (4) × Comfy (1.25) should be 5");

		theme.DefaultSpacing = 6.0;
		Assert.AreEqual(7.5, GetResource<double>(container, "Space100"), 0.001,
			"New base (6) × Comfy (1.25) should be 7.5");

		theme.DefaultSpacing = double.NaN;
		Assert.AreEqual(5.0, GetResource<double>(container, "Space100"), 0.001,
			"An invalid base (NaN) should restore the default base (4) × Comfy (1.25)");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(double.NaN)]
	[DataRow(-1.0)]
	[DataRow(double.PositiveInfinity)]
	[DataRow(double.NegativeInfinity)]
	public void When_DefaultSpacingInvalid_Then_FallsBackToDefaultBase(double invalid)
	{
		var (container, _) = CreateThemedContainer(Density.Comfy, spacing: invalid);
		Assert.AreEqual(5.0, GetResource<double>(container, "Space100"), 0.001,
			$"DefaultSpacing={invalid} should fall back to the default base (4) × Comfy (1.25)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultSpacingChanges_Then_ShapeTokensAreUnaffected()
	{
		var (container, theme) = CreateThemedContainer(spacing: 6.0, cornerRadius: 4.0);
		Assert.AreEqual(8.0, GetResource<double>(container, "Radius200"), 0.001);

		theme.DefaultSpacing = 10.0;
		Assert.AreEqual(8.0, GetResource<double>(container, "Radius200"), 0.001);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultSpacingSet_Then_FixedTokensAreConstant()
	{
		var (container, _) = CreateThemedContainer(spacing: 10.0);

		Assert.AreEqual(40.0, GetResource<double>(container, "ControlHeightMedium"), 0.001);
		Assert.AreEqual(24.0, GetResource<double>(container, "IconSizeMedium"), 0.001);
		Assert.AreEqual(48.0, GetResource<double>(container, "TouchTargetMinSize"), 0.001);
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 9. VISUAL INTEGRATION — render actual controls, verify layout values
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SimpleButton_AtRegularDensity_Then_PaddingAndCornerRadiusAreCorrect()
	{
		var (container, _) = CreateThemedContainer(Density.Regular, cornerRadius: 4.0);

		var style = container.Resources["SimpleBaseButtonStyle"] as Style;
		Assert.IsNotNull(style, "SimpleBaseButtonStyle should exist");

		var button = new Button { Content = "Test", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Padding = SimpleSpace300Thickness = Space300Thickness = Thickness(12) at Regular (4×3=12)
		Assert.AreEqual(new Thickness(12), button.Padding,
			"Simple button Padding should be Space300Thickness (uniform 12) at Regular density");

		// CornerRadius = SimpleRadius200CornerRadius = Radius200CornerRadius = CornerRadius(8) at base=4
		Assert.AreEqual(new CornerRadius(8), button.CornerRadius,
			"Simple button CornerRadius should be Radius200CornerRadius (4×2=8)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_SimpleButton_AtRegularDensity_Then_MinHeightMatchesToken()
	{
		var (container, _) = CreateThemedContainer(Density.Regular, cornerRadius: 4.0);

		var style = container.Resources["SimpleBaseButtonStyle"] as Style;
		Assert.IsNotNull(style, "SimpleBaseButtonStyle should exist");

		var button = new Button { Content = "Height", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// MinHeight = ControlHeightMedium = 40
		Assert.AreEqual(40.0, button.MinHeight, 0.001,
			"Simple button MinHeight should be ControlHeightMedium (40)");
	}

}
