using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Shapes;
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
	[DataRow(Density.Compact, "Space100", 6.0)]  // base=6 × 1
	[DataRow(Density.Compact, "Space400", 24.0)] // base=6 × 4
	[DataRow(Density.Regular, "Space100", 8.0)]  // base=8 × 1 (M3 space100 = 8dp)
	[DataRow(Density.Regular, "Space200", 16.0)] // base=8 × 2
	[DataRow(Density.Regular, "Space025", 2.0)]  // base=8 × 0.25
	[DataRow(Density.Regular, "Space125", 10.0)] // base=8 × 1.25
	[DataRow(Density.Regular, "Space900", 72.0)] // base=8 × 9
	[DataRow(Density.Regular, "Space2000", 160.0)] // base=8 × 20
	[DataRow(Density.Comfy, "Space100", 10.0)]   // base=10 × 1
	[DataRow(Density.Comfy, "Space050", 5.0)]    // base=10 × 0.5
	[DataRow(Density.Comfy, "Space800", 80.0)]   // base=10 × 8
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
		Assert.AreEqual(new Thickness(16), GetResource<Thickness>(container, "Space200Thickness"));

		// Horizontal: left/right only
		Assert.AreEqual(new Thickness(32, 0, 32, 0), GetResource<Thickness>(container, "Space400HorizontalThickness"));

		// Vertical
		Assert.AreEqual(new Thickness(0, 8, 0, 8), GetResource<Thickness>(container, "Space100VerticalThickness"));

		// Directional singles
		Assert.AreEqual(new Thickness(0, 16, 0, 0), GetResource<Thickness>(container, "Space200TopThickness"));
		Assert.AreEqual(new Thickness(0, 0, 0, 16), GetResource<Thickness>(container, "Space200BottomThickness"));
		Assert.AreEqual(new Thickness(8, 0, 0, 0), GetResource<Thickness>(container, "Space100LeftThickness"));
		Assert.AreEqual(new Thickness(0, 0, 8, 0), GetResource<Thickness>(container, "Space100RightThickness"));
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_HighScaleVariants_Then_NoDirectionalCompanions()
	{
		var (container, _) = CreateThemedContainer(Density.Regular);

		// Variants above 900 have uniform Thickness but no directional ones
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
		Assert.AreEqual(8.0, GetResource<double>(container, "Space100"), 0.001);

		theme.DefaultDensity = Density.Compact;
		Assert.AreEqual(6.0, GetResource<double>(container, "Space100"), 0.001);

		theme.DefaultDensity = Density.Comfy;
		Assert.AreEqual(10.0, GetResource<double>(container, "Space100"), 0.001);
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
		Assert.AreEqual(16.0, GetResource<double>(container, "Space200"), 0.001);

		theme.DefaultCornerRadius = 10.0;
		Assert.AreEqual(16.0, GetResource<double>(container, "Space200"), 0.001);
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 7. EDGE CASES
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public void When_InvalidDensityEnum_Then_FallsBackToRegular()
	{
		var theme = new MaterialTheme { DefaultDensity = (Density)99 };
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		Assert.AreEqual(8.0, GetResource<double>(container, "Space100"), 0.001,
			"Invalid Density should fall back to base=8.0");
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
	// SHAPE — directional companions, validation and size-capped radii
	// (unoplatform/Uno.Themes#1747)
	// ═══════════════════════════════════════════════════════════════════════

	private static readonly double[] ShapeBases = { 0.0, 2.0, 4.0, 8.0, 16.0 };

	// A per-control key is a StaticResource alias, which resolves from the scope doing the lookup:
	// read from a detached dictionary it reaches the application's theme, not this one. So these
	// are read the way a template reads them, by an element loaded under the themed container.
	private static async Task<T> LoadProbe<T>(double baseRadius, string xaml)
		where T : FrameworkElement
	{
		var (container, _) = CreateThemedContainer(cornerRadius: baseRadius);
		var probe = (T)XamlReader.Load(xaml);
		container.Children.Add(probe);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(probe);
		await UnitTestsUIContentHelper.WaitForIdle();

		return probe;
	}

	private static async Task<CornerRadius> ResolveCornerRadius(double baseRadius, string key)
		=> (await LoadProbe<Border>(baseRadius,
			$"<Border xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Width='40' Height='40' CornerRadius='{{ThemeResource {key}}}' />")).CornerRadius;

	private static async Task<double> ResolveRadius(double baseRadius, string key)
		=> (await LoadProbe<Rectangle>(baseRadius,
			$"<Rectangle xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Width='40' Height='40' RadiusX='{{ThemeResource {key}}}' />")).RadiusX;

	[TestMethod]
	[RunsOnUIThread]
	public void When_CornerRadiusSet_Then_DirectionalCompanionsExist()
	{
		var (container, _) = CreateThemedContainer(cornerRadius: 6.0);

		Assert.AreEqual(new CornerRadius(12, 12, 0, 0), GetResource<CornerRadius>(container, "Radius200TopCornerRadius"));
		Assert.AreEqual(new CornerRadius(12, 0, 0, 12), GetResource<CornerRadius>(container, "Radius200LeftCornerRadius"));
		Assert.AreEqual(new CornerRadius(0, 0, 12, 12), GetResource<CornerRadius>(container, "Radius200BottomCornerRadius"));
		Assert.AreEqual(new CornerRadius(12, 0, 0, 0), GetResource<CornerRadius>(container, "Radius200TopLeftCornerRadius"));
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(double.NaN)]
	[DataRow(double.PositiveInfinity)]
	[DataRow(double.NegativeInfinity)]
	[DataRow(-4.0)]
	public void When_CornerRadiusIsInvalid_Then_FallsBackToDefault(double invalid)
	{
		var (container, _) = CreateThemedContainer(cornerRadius: invalid);

		Assert.AreEqual(4.0, GetResource<double>(container, "Radius100"), 0.001);
		Assert.AreEqual(new CornerRadius(8), GetResource<CornerRadius>(container, "Radius200CornerRadius"));
		Assert.AreEqual(20.0, GetResource<double>(container, "MaterialShapeButton"), 0.001,
			"A capped radius must fall back to its base-4 value too.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("MaterialShapeButton", 5.0, 20.0)]
	[DataRow("MaterialShapeCheckBox", 0.5, 4.5)]
	[DataRow("MaterialShapeFab", 4.0, 16.8)]
	[DataRow("MaterialShapeFabSmall", 3.0, 12.0)]
	[DataRow("MaterialShapeFabLarge", 7.0, 28.8)]
	[DataRow("MaterialShapeDialog", 7.0, 32.0)]
	[DataRow("MaterialShapeNavigationItem", 7.0, 28.0)]
	[DataRow("MaterialShapeNavigationItemCompact", 4.0, 16.0)]
	[DataRow("MaterialShapeSwitchTrack", 4.0, 16.0)]
	[DataRow("MaterialShapeSliderFill", 1.0, 4.0)]
	[DataRow("MaterialShapeIndicator", 0.5, 2.0)]
	public void When_CornerRadiusChanges_Then_CappedRadiusKeepsControlShape(string key, double multiplier, double cap)
	{
		Assert.IsTrue(4.0 * multiplier <= cap, $"{key}: the cap must not change what the default base of 4 renders.");

		foreach (var baseRadius in ShapeBases)
		{
			var (container, _) = CreateThemedContainer(cornerRadius: baseRadius);
			var expected = Math.Min(baseRadius * multiplier, cap);

			Assert.AreEqual(expected, GetResource<double>(container, key), 0.001, $"{key} at base={baseRadius}");
			Assert.AreEqual(new CornerRadius(expected), GetResource<CornerRadius>(container, key + "CornerRadius"),
				$"{key}CornerRadius at base={baseRadius}");
		}
	}

	// The per-control keys keep their name and type and follow the capped radius: today's value at
	// 4, square at 0, and never past the control's size at 16.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("ButtonCornerRadius", 20.0, 20.0)]
	[DataRow("CheckBoxCheckAreaCornerRadius", 2.0, 4.5)]
	[DataRow("MaterialFabCornerRadius", 16.0, 16.8)]
	[DataRow("MaterialSmallFabCornerRadius", 12.0, 12.0)]
	[DataRow("MaterialLargeFabCornerRadius", 28.0, 28.8)]
	[DataRow("MaterialContentDialogCornerRadius", 28.0, 32.0)]
	[DataRow("MaterialNavigationViewItemCornerRadius", 28.0, 28.0)]
	[DataRow("MaterialNavigationViewItemCornerRadiusCompact", 16.0, 16.0)]
	[DataRow("MaterialNavigationViewButtonCornerRadius", 16.0, 16.0)]
	[DataRow("OutlinedPasswordBoxCornerRadius", 4.0, 16.0)]
	public async Task When_CornerRadiusChanges_Then_ControlKeyKeepsItsShape(string key, double atDefault, double atLarge)
	{
		Assert.AreEqual(new CornerRadius(0), await ResolveCornerRadius(0.0, key), $"{key} at base=0");
		Assert.AreEqual(new CornerRadius(atDefault), await ResolveCornerRadius(4.0, key), $"{key} at base=4");
		Assert.AreEqual(new CornerRadius(atLarge), await ResolveCornerRadius(16.0, key), $"{key} at base=16");
	}

	// Literals that now follow the scale: the same value at 4, square at 0.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CornerRadiusChanges_Then_FormerLiteralsFollowTheScale()
	{
		Assert.AreEqual(new CornerRadius(4, 4, 0, 0), await ResolveCornerRadius(4.0, "FilledTextBoxCornerRadius"));
		Assert.AreEqual(new CornerRadius(4, 4, 0, 0), await ResolveCornerRadius(4.0, "FilledPasswordBoxCornerRadius"));
		Assert.AreEqual(new CornerRadius(4, 4, 0, 0), await ResolveCornerRadius(4.0, "DatePickerCornerRadius"));
		Assert.AreEqual(new CornerRadius(4, 4, 0, 0), await ResolveCornerRadius(4.0, "CalendarDatePickerBackgroundCornerRadius"));
		Assert.AreEqual(new CornerRadius(4, 0, 0, 4), await ResolveCornerRadius(4.0, "SliderHorizontalFillCornerRadius"));
		Assert.AreEqual(new CornerRadius(0, 0, 4, 4), await ResolveCornerRadius(4.0, "SliderVerticalFillCornerRadius"));
		Assert.AreEqual(new CornerRadius(8, 0, 0, 0), await ResolveCornerRadius(4.0, "NavigationViewContentGridCornerRadius"));
		Assert.AreEqual(new CornerRadius(28, 0, 0, 28), await ResolveCornerRadius(4.0, "MaterialNavigationViewItemCornerRadiusLeftOnly"));
		Assert.AreEqual(16.0, await ResolveRadius(4.0, "SwitchKnobRadius"), 0.001);

		Assert.AreEqual(new CornerRadius(0), await ResolveCornerRadius(0.0, "FilledTextBoxCornerRadius"));
		Assert.AreEqual(new CornerRadius(0), await ResolveCornerRadius(0.0, "MaterialNavigationViewItemCornerRadiusLeftOnly"));
		Assert.AreEqual(0.0, await ResolveRadius(0.0, "SwitchKnobRadius"), 0.001);

		Assert.AreEqual(new CornerRadius(28, 0, 0, 28), await ResolveCornerRadius(16.0, "MaterialNavigationViewItemCornerRadiusLeftOnly"),
			"The left-only radius is capped at half the 56px item, like the full one.");
		Assert.AreEqual(16.0, await ResolveRadius(16.0, "SwitchKnobRadius"), 0.001);
		Assert.AreEqual(new CornerRadius(4, 0, 0, 4), await ResolveCornerRadius(16.0, "SliderHorizontalFillCornerRadius"),
			"The slider fill keeps its rounded end: its radius does not grow past the literal it replaced.");
		Assert.AreEqual(2.0, await ResolveRadius(16.0, "NavigationViewSelectionIndicatorRadius"), 0.001);
		Assert.AreEqual(2.0, await ResolveRadius(4.0, "NavigationViewSelectionIndicatorRadius"), 0.001);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_CornerRadiusIsLarge_Then_FabStaysARoundedSquare()
	{
		var (container, _) = CreateThemedContainer(cornerRadius: 16.0);
		var fab = new Button { Content = "+", Style = (Style)container.Resources["MaterialFabStyle"] };
		container.Children.Add(fab);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(fab);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Radius400 would be 64 here, past half of any FAB: a circle. Capped at 0.3 × the 56px side.
		Assert.AreEqual(new CornerRadius(16.8), fab.CornerRadius);
		Assert.IsTrue(fab.CornerRadius.TopLeft < fab.ActualHeight / 2,
			$"The FAB radius ({fab.CornerRadius.TopLeft}) must stay under half its height ({fab.ActualHeight}).");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButton_WithLargeCornerRadius_Then_ButtonStaysAPill()
	{
		var (container, _) = CreateThemedContainer(Density.Regular, cornerRadius: 16.0);
		var button = new Button { Content = "Pill", Style = (Style)container.Resources["FilledButtonStyle"] };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Radius500 would be 80; half the 40px height keeps a true pill instead of an ellipse.
		Assert.AreEqual(new CornerRadius(20), button.CornerRadius);
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

		// ButtonPadding = Space200HorizontalThickness = Thickness(16,0,16,0) at Regular
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

		// ButtonPadding = Space200HorizontalThickness at Compact: base=6 × 2 = 12
		Assert.AreEqual(new Thickness(12, 0, 12, 0), button.Padding,
			"FilledButton Padding should shrink to 12 at Compact density");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButton_WithCustomCornerRadius_Then_ButtonReflectsIt()
	{
		var (container, _) = CreateThemedContainer(Density.Regular, cornerRadius: 3.0);

		var style = container.Resources["FilledButtonStyle"] as Style;
		Assert.IsNotNull(style, "FilledButtonStyle should exist");

		var button = new Button { Content = "Rounded", Style = style };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		// ButtonCornerRadius = min(Radius500, 20) = CornerRadius(3×5=15); the cap is asserted in
		// When_FilledButton_WithLargeCornerRadius_Then_ButtonStaysAPill.
		Assert.AreEqual(new CornerRadius(15), button.CornerRadius,
			"FilledButton CornerRadius should be 15 at base=3 (Radius500 = 3×5)");
	}

	// ═══════════════════════════════════════════════════════════════════════
	// 9. RUNTIME SEAM — the scale measures move controls, not only tokens.
	// Regenerated Space*/Radius* keys reach a control only if its template reads them through
	// {ThemeResource}; a {StaticResource} attribute snapshots the value at parse time. Asserted on
	// the application theme (the scope a designer edits), restored in finally. ContentDialog is
	// the Material v2 control whose panel padding and corner radius were read statically.
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ScaleMeasuresChangeAtRuntime_Then_RealizedContentDialogFollows()
	{
		var theme = SemanticThemeHelper.GetTheme();
		Assert.IsNotNull(theme, "The Material sample app must have a BaseTheme in its application resources.");

		var originalSpacing = theme.DefaultSpacing;
		var originalRadius = theme.DefaultCornerRadius;

		// Declared in the tree like a page-level dialog; its style applies without showing it.
		var root = new Grid();
		var dialog = new ContentDialog
		{
			Title = "probe",
			Content = "probe",
			Style = (Style)Application.Current.Resources["MaterialContentDialogStyle"],
		};
		root.Children.Add(dialog);

		UnitTestsUIContentHelper.Content = root;
		await UnitTestsUIContentHelper.WaitForLoaded(dialog);
		await UnitTestsUIContentHelper.WaitForIdle();

		try
		{
			var radiusBefore = dialog.CornerRadius;

			theme.DefaultSpacing = 10;      // Space600Thickness = 60
			theme.DefaultCornerRadius = 1;  // Radius700CornerRadius = 7

			root.RequestedTheme = ElementTheme.Light;
			await UnitTestsUIContentHelper.WaitForIdle();
			root.RequestedTheme = ElementTheme.Dark;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(new CornerRadius(7), dialog.CornerRadius,
				$"A realized ContentDialog must re-resolve MaterialContentDialogCornerRadius after the radius changes (was {radiusBefore}).");
		}
		finally
		{
			theme.DefaultSpacing = originalSpacing;
			theme.DefaultCornerRadius = originalRadius;
			root.RequestedTheme = ElementTheme.Default;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// ═══════════════════════════════════════════════════════════════════════
	// CONTROL SIZES — spacing tokens are for padding, gaps and margins only
	// ═══════════════════════════════════════════════════════════════════════

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("AppBarButtonHeight", 64.0)]
	[DataRow("AppBarButtonWidth", 64.0)]
	[DataRow("PipsPagerHorizontalOrientationButtonWidth", 12.0)]
	[DataRow("PipsPagerNavigationVisualStatesEllipseWidth", 12.0)]
	[DataRow("PipsPagerSelectedEllipseSize", 8.0)]
	[DataRow("PipsPagerNormalEllipseSize", 8.0)]
	[DataRow("ProgressBarHeight", 4.0)]
	[DataRow("SliderFillThickness", 6.0)]
	[DataRow("SliderTopTickBarHeight", 4.0)]
	[DataRow("SliderHorizontalInlineTickBarHeight", 2.0)]
	[DataRow("SwitchKnobRadius", 16.0)]
	public void When_DefaultSpacingChanges_Then_ControlSizeKeysStayFixed(string key, double expected)
	{
		foreach (var spacing in new[] { 0.0, double.NaN, 64.0 })
		{
			var theme = new MaterialTheme { DefaultSpacing = spacing };
			var container = new Grid();
			container.Resources.MergedDictionaries.Add(theme);

			Assert.AreEqual(expected, GetResource<double>(container, key), 0.001,
				$"{key} must not follow DefaultSpacing (DefaultSpacing={spacing})");
		}
	}
}
