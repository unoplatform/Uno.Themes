#nullable enable

using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards the Simple TimePicker styles (<c>SimpleTimePickerStyle</c>,
/// <c>SimpleTimePickerFlyoutPresenterStyle</c> and their <c>SimpleDefault*</c> aliases).
///
/// The high-value assertion here is the template-part contract: Uno's
/// <c>TimePicker.OnApplyTemplate</c> reparents the hour / minute / period TextBlocks into
/// <c>First|Second|ThirdPickerHost</c> and drives <c>First|Second|ThirdTextBlockColumn</c> and
/// <c>First|SecondColumnDivider</c> to reorder per culture and collapse the period column for a
/// 24-hour <see cref="TimePicker.ClockIdentifier"/>. A template missing those named parts still
/// renders — it just silently loses ordering and 24-hour support — so the names are asserted
/// explicitly rather than inferred from "it looked fine".
/// </summary>
[TestClass]
public class Given_TimePickerStyles
{
	/// <summary>
	/// Parts the WinUI/Uno TimePicker itself looks up, taken from <c>TimePicker.partial.mux.cs</c>.
	/// Dropping one of these does not fail the build or throw — the control just silently loses
	/// culture ordering or 24-hour support — so they are asserted explicitly.
	///
	/// <c>First|Second|ThirdTextBlockColumn</c> belong to the same contract but are not listed here:
	/// a <c>ColumnDefinition</c> is not a visual-tree child, so it cannot be found by walking
	/// <see cref="VisualTreeHelper"/>. <see cref="When_TemplateApplied_ThenTheControlDrivesTheColumnWidths"/>
	/// covers them, and does it better — the control could only have assigned those widths if it
	/// resolved the names.
	/// </summary>
	private static readonly string[] ControlContractParts =
	{
		"LayoutRoot",
		"FlyoutButton",
		"FirstPickerHost",
		"SecondPickerHost",
		"ThirdPickerHost",
		"HourTextBlock",
		"MinuteTextBlock",
		"PeriodTextBlock",
		"FirstColumnDivider",
		"SecondColumnDivider",
	};

	/// <summary>
	/// Names this template owns. They are not part of the control contract, so a future retemplate
	/// may rename them — they are asserted only because the tests below address them.
	/// </summary>
	private static readonly string[] TemplateOwnedParts =
	{
		"PlaceholderText",
		"FlyoutButtonContentGrid",
	};

	private static Grid CreateThemedContainer(ElementTheme theme = ElementTheme.Default)
	{
		var container = new Grid { RequestedTheme = theme };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		return container;
	}

	/// <summary>
	/// Loads one <see cref="Border"/> per key, each bound to that key through <c>ThemeResource</c>,
	/// then reads the brushes back once per theme.
	///
	/// Two traps make the obvious version of this useless. The <see cref="ResourceDictionary"/>
	/// indexer resolves ThemeDictionaries against the *application* theme, so
	/// <c>container.Resources[key]</c> returns the same brush whatever theme is asked for; and theme
	/// brushes only re-materialize when the <b>XamlRoot content's</b> theme changes — setting
	/// <see cref="FrameworkElement.RequestedTheme"/> on the container subtree is not enough (the same
	/// approach <c>Given_DefaultPalette</c> uses in MaterialSampleApp).
	/// </summary>
	private static async Task<(SolidColorBrush? First, SolidColorBrush? Second)> ResolveThemeBrushPairAsync(
		ElementTheme theme,
		string firstKey,
		string secondKey)
	{
		var container = CreateThemedContainer();
		var probes = (Panel)XamlReader.Load(
			$$"""
			<StackPanel xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
				<Border Width="8" Height="8" Background="{ThemeResource {{firstKey}}}" />
				<Border Width="8" Height="8" Background="{ThemeResource {{secondKey}}}" />
			</StackPanel>
			""");

		container.Children.Add(probes);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(container);
		await UnitTestsUIContentHelper.WaitForIdle();

		var root = container.XamlRoot?.Content as FrameworkElement;
		Assert.IsNotNull(root, "The loaded container should expose the XamlRoot content");

		var initialTheme = root!.RequestedTheme;
		try
		{
			root.RequestedTheme = theme;
			await UnitTestsUIContentHelper.WaitForIdle();

			var first = ((Border)probes.Children[0]).Background as SolidColorBrush;
			var second = ((Border)probes.Children[1]).Background as SolidColorBrush;

			Assert.IsNotNull(first, $"{firstKey} should resolve to a SolidColorBrush under {theme}");
			Assert.IsNotNull(second, $"{secondKey} should resolve to a SolidColorBrush under {theme}");

			return (first, second);
		}
		finally
		{
			root.RequestedTheme = initialTheme;
			await UnitTestsUIContentHelper.WaitForIdle();
		}
	}

	private static FrameworkElement? FindDescendantByName(DependencyObject root, string name)
	{
		var count = VisualTreeHelper.GetChildrenCount(root);
		for (var i = 0; i < count; i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is FrameworkElement fe && fe.Name == name)
			{
				return fe;
			}

			if (FindDescendantByName(child, name) is { } match)
			{
				return match;
			}
		}

		return null;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Style keys resolve, in Light and Dark.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, "SimpleTimePickerStyle")]
	[DataRow(ElementTheme.Light, "SimpleDefaultTimePickerStyle")]
	[DataRow(ElementTheme.Light, "SimpleTimePickerFlyoutButtonStyle")]
	[DataRow(ElementTheme.Light, "SimpleTimePickerFlyoutPresenterStyle")]
	[DataRow(ElementTheme.Light, "SimpleDefaultTimePickerFlyoutPresenterStyle")]
	[DataRow(ElementTheme.Dark, "SimpleTimePickerStyle")]
	[DataRow(ElementTheme.Dark, "SimpleDefaultTimePickerStyle")]
	[DataRow(ElementTheme.Dark, "SimpleTimePickerFlyoutButtonStyle")]
	[DataRow(ElementTheme.Dark, "SimpleTimePickerFlyoutPresenterStyle")]
	[DataRow(ElementTheme.Dark, "SimpleDefaultTimePickerFlyoutPresenterStyle")]
	public void When_TimePickerStyleKey_Requested_ThenItResolves(ElementTheme theme, string styleKey)
	{
		var container = CreateThemedContainer(theme);

		Assert.IsTrue(
			container.Resources.TryGetValue(styleKey, out var resource),
			$"{styleKey} should be available under {theme}");
		Assert.IsInstanceOfType(resource, typeof(Style), $"{styleKey} should be a Style");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public void When_SemanticTimePickerKeys_Requested_ThenTheyAliasTheSimpleStyles(ElementTheme theme)
	{
		var container = CreateThemedContainer(theme);

		Assert.AreSame(
			container.Resources["SimpleTimePickerStyle"],
			container.Resources["TimePickerStyle"],
			$"TimePickerStyle should alias SimpleTimePickerStyle under {theme}");
		Assert.AreSame(
			container.Resources["SimpleTimePickerFlyoutPresenterStyle"],
			container.Resources["TimePickerFlyoutPresenterStyle"],
			$"TimePickerFlyoutPresenterStyle should alias SimpleTimePickerFlyoutPresenterStyle under {theme}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Lightweight styling keys resolve, in Light and Dark.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public void When_TimePickerLightweightKeys_Requested_ThenTheyResolve(ElementTheme theme)
	{
		var container = CreateThemedContainer(theme);

		var keys = new[]
		{
			"TimePickerButtonBackground",
			"TimePickerButtonBackgroundDisabled",
			"TimePickerButtonBorderBrush",
			"TimePickerButtonBorderBrushDisabled",
			"TimePickerButtonTimeTextForeground",
			"TimePickerButtonTimeTextForegroundDisabled",
			"TimePickerPlaceholderTextForeground",
			"TimePickerHeaderForeground",
			"TimePickerHeaderForegroundDisabled",
			"TimePickerSpacerFill",
			"TimePickerSpacerFillDisabled",
			"TimePickerFlyoutPresenterBackground",
			"TimePickerFlyoutPresenterBorderBrush",
			"TimePickerFlyoutPresenterSpacerFill",
			"TimePickerFlyoutPresenterHighlightFill",
			"TimePickerFlyoutPresenterCornerRadius",
			"TimePickerCornerRadius",
			"TimePickerMinHeight",
			"TimePickerContentMargin",
		};

		var missing = keys
			.Where(key => !container.Resources.TryGetValue(key, out var value) || value is null)
			.ToList();

		CollectionAssert.AreEqual(
			System.Array.Empty<string>(),
			missing,
			$"These TimePicker lightweight keys did not resolve under {theme}: {string.Join(", ", missing)}");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_FlyoutPresenterHighlight_Resolved_ThenItContrastsWithTheFlyoutBackground(ElementTheme theme)
	{
		// The selection band sits directly on the flyout background; when the two resolve to the
		// same color the selected row is invisible. That is the bug this guards: the band used to
		// be PrimaryVariantLightBrush, which is #1E1E1E under Dark — exactly SurfaceColor. Under
		// Light the two already differed (#F5F5F5 vs #FFFFFF), so the Dark row is the one that
		// reproduces it, and it only does so because the brushes are resolved through a themed
		// element rather than the theme-blind ResourceDictionary indexer.
		var (highlight, background) = await ResolveThemeBrushPairAsync(
			theme,
			"TimePickerFlyoutPresenterHighlightFill",
			"TimePickerFlyoutPresenterBackground");

		Assert.AreNotEqual(
			background!.Color,
			highlight!.Color,
			$"The TimePicker selection band must be distinguishable from the flyout background under {theme}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Template-part contract.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_TimePickerStyle_Applied_ThenRequiredTemplatePartsArePresent(ElementTheme theme)
	{
		var container = CreateThemedContainer(theme);
		var picker = new TimePicker
		{
			Header = "Pick a time",
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var missing = ControlContractParts
			.Concat(TemplateOwnedParts)
			.Where(part => FindDescendantByName(picker, part) is null)
			.ToList();

		CollectionAssert.AreEqual(
			System.Array.Empty<string>(),
			missing,
			$"SimpleTimePickerStyle is missing these named template parts under {theme}: {string.Join(", ", missing)}");
	}

	// Field sizing (shared with DatePicker) lives in Given_PickerFieldLayout.

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_HeaderIsSet_ThenItRendersOnceAsThePlaceholder()
	{
		// The header used to render twice — as a label above the field and again as the
		// header-bound placeholder inside it. It is the placeholder only.
		const string header = "Start Time";

		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			Header = header,
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var renderedHeaders = CountVisibleTextBlocksWithText(picker, header);
		Assert.AreEqual(1, renderedHeaders, $"'{header}' should be rendered exactly once, as the placeholder");

		var placeholder = FindDescendantByName(picker, "PlaceholderText");
		Assert.IsNotNull(placeholder, "PlaceholderText should be part of the template");
		Assert.AreEqual(Visibility.Visible, placeholder!.Visibility, "The header should be visible while no time is set");

		// The control's own "hour : minute AM" run would sit on top of the placeholder.
		var value = FindDescendantByName(picker, "FlyoutButtonContentGrid");
		Assert.AreEqual(
			Visibility.Collapsed,
			value!.Visibility,
			"With a header acting as the placeholder, the hour/minute run must stay hidden until a time is set");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_NoHeaderIsSet_ThenTheControlPlaceholderIsShown()
	{
		// Without a header there is nothing to use as the placeholder, so the control's own
		// "hour : minute AM" run stays visible rather than leaving an empty field.
		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var value = FindDescendantByName(picker, "FlyoutButtonContentGrid");
		Assert.IsNotNull(value, "FlyoutButtonContentGrid should be part of the template");
		Assert.AreEqual(Visibility.Visible, value!.Visibility, "The hour/minute run is the fallback placeholder");
	}

	private static int CountVisibleTextBlocksWithText(DependencyObject root, string text)
	{
		var count = 0;
		var children = VisualTreeHelper.GetChildrenCount(root);
		for (var i = 0; i < children; i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is TextBlock { Visibility: Visibility.Visible } textBlock && textBlock.Text == text)
			{
				count++;
			}

			if (child is not FrameworkElement { Visibility: Visibility.Collapsed })
			{
				count += CountVisibleTextBlocksWithText(child, text);
			}
		}

		return count;
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TimeIsSelected_ThenTheDividersCarryNoOrderDependentText()
	{
		// Regression guard for a culture bug, so it asserts the property that makes the template
		// culture-safe rather than a rendering that only holds for en-US.
		//
		// UpdateOrderAndLayout reparents the hour/minute/period TextBlocks between
		// First|Second|ThirdPickerHost to reorder per culture, but it only toggles the *visibility*
		// of the dividers — it never moves them. A literal ":" in FirstColumnDivider is therefore
		// correct only while the hour happens to come first. Under a period-first culture
		// (ko-KR / zh-CN / ja-JP put the period in FirstPickerHost) it renders "오전 : 9  41":
		// colon between period and hour, none between hour and minute.
		//
		// A neutral rule carries no ordering assumption, so the invariant to hold is "the dividers
		// are not text". Asserting it structurally keeps the test deterministic — swapping the
		// ambient culture mid-test is flaky and would not re-run the control's ordering pass.
		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			SelectedTime = new System.TimeSpan(9, 41, 0),
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		foreach (var dividerName in new[] { "FirstColumnDivider", "SecondColumnDivider" })
		{
			var divider = FindDescendantByName(picker, dividerName);
			Assert.IsNotNull(divider, $"{dividerName} should be part of the template");
			Assert.IsNotInstanceOfType(
				divider,
				typeof(TextBlock),
				$"{dividerName} must not be a TextBlock: the control never repositions the dividers, "
					+ "so any glyph in one is wrong as soon as the culture reorders the columns");
		}

		// The hour / minute / period text stays owned by the control, so culture ordering,
		// MinuteIncrement and ClockIdentifier all keep working.
		var hour = FindDescendantByName(picker, "HourTextBlock") as TextBlock;
		var minute = FindDescendantByName(picker, "MinuteTextBlock") as TextBlock;
		Assert.IsFalse(string.IsNullOrEmpty(hour?.Text), "The control should have filled HourTextBlock");
		Assert.IsFalse(string.IsNullOrEmpty(minute?.Text), "The control should have filled MinuteTextBlock");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TemplateApplied_ThenTheControlDrivesTheColumnWidths()
	{
		// The named columns are the control's, not the template's: UpdateOrderAndLayout assigns 1*
		// to every populated column and 0 to the rest on each pass, overwriting whatever width the
		// template declared. Declaring "Auto" there reads as a compact-layout mechanism that does
		// not exist — this pins the real behaviour so the template comment stays honest.
		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			SelectedTime = new System.TimeSpan(9, 41, 0),
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var grid = FindDescendantByName(picker, "FlyoutButtonContentGrid") as Grid;
		Assert.IsNotNull(grid, "FlyoutButtonContentGrid should be part of the template");

		var populated = grid!.ColumnDefinitions
			.Where(c => c.Width.GridUnitType == GridUnitType.Star)
			.ToList();

		Assert.AreEqual(
			3,
			populated.Count,
			"The control should have starred the three populated hour/minute/period columns");
		foreach (var column in populated)
		{
			// Delta rather than == : GridLength.Value is a double, and the control computes it
			// rather than echoing the literal the template declared.
			Assert.AreEqual(
				1.0,
				column.Width.Value,
				0.0001,
				"Every populated column should carry the control's 1* width");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ClockIdentifierIs24Hour_ThenPeriodColumnCollapses()
	{
		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			ClockIdentifier = "24HourClock",
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var secondDivider = FindDescendantByName(picker, "SecondColumnDivider");
		Assert.IsNotNull(secondDivider, "SecondColumnDivider should be part of the template");
		Assert.AreEqual(
			Visibility.Collapsed,
			secondDivider!.Visibility,
			"With a 24-hour clock there is no AM/PM column, so the second divider must collapse");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Lightweight-styling override precedence.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TimePickerButtonBackgroundOverridden_ThenFieldReflectsChange()
	{
		var expected = Colors.DarkOrchid;

		var container = CreateThemedContainer();
		container.Resources["TimePickerButtonBackground"] = new SolidColorBrush(expected);

		var picker = new TimePicker
		{
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var background = picker.Background as SolidColorBrush;
		Assert.IsNotNull(background, "TimePicker should have a Background brush from the style");
		Assert.AreEqual(
			expected,
			background!.Color,
			"Overriding TimePickerButtonBackground should change the TimePicker field background");
	}
}
