#nullable enable

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards the Material v2 <see cref="DatePicker"/> and <see cref="TimePicker"/> styles.
///
/// These templates previously had no runtime coverage at all: every picker test lived in
/// SimpleSampleApp and merged <c>SimpleTheme</c>, so the Material half of the same change —
/// the floating header, the culture-safe dividers and the whole <c>TimePicker*</c> key family —
/// was verified only by "it builds". The defects those tests protect against reproduce
/// identically in Material, hence this parallel suite.
/// </summary>
[TestClass]
public class Given_MaterialPickerStyles
{
	/// <summary>
	/// Parts the WinUI/Uno TimePicker itself looks up, taken from <c>TimePicker.partial.mux.cs</c>.
	/// A template missing one of these still renders — it just silently loses culture ordering or
	/// 24-hour support — so they are asserted explicitly rather than inferred from "it looked fine".
	/// </summary>
	private static readonly string[] TimePickerContractParts =
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

	private static Grid CreateThemedContainer(ElementTheme theme = ElementTheme.Default)
	{
		var container = new Grid { RequestedTheme = theme };
		container.Resources.MergedDictionaries.Add(new MaterialTheme());
		return container;
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

	/// <summary>
	/// Loads one <see cref="Border"/> per key, each bound to that key through <c>ThemeResource</c>,
	/// then reads the brushes back once per theme.
	///
	/// Two traps make the obvious version of this useless. The <see cref="ResourceDictionary"/>
	/// indexer resolves ThemeDictionaries against the *application* theme, so
	/// <c>container.Resources[key]</c> returns the same brush whatever theme is asked for; and theme
	/// brushes only re-materialize when the <b>XamlRoot content's</b> theme changes — setting
	/// <see cref="FrameworkElement.RequestedTheme"/> on the container subtree is not enough (the same
	/// approach <c>Given_DefaultPalette</c> uses).
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

	// ─────────────────────────────────────────────────────────────────────
	// Style keys resolve.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("MaterialTimePickerStyle")]
	[DataRow("MaterialDefaultTimePickerStyle")]
	[DataRow("MaterialTimePickerFlyoutButtonStyle")]
	[DataRow("MaterialTimePickerFlyoutPresenterStyle")]
	[DataRow("MaterialDefaultTimePickerFlyoutPresenterStyle")]
	[DataRow("MaterialDatePickerStyle")]
	[DataRow("MaterialDatePickerFlyoutPresenterStyle")]
	public void When_MaterialPickerStyleKey_Requested_ThenItResolves(string styleKey)
	{
		var container = CreateThemedContainer();

		Assert.IsTrue(
			container.Resources.TryGetValue(styleKey, out var resource),
			$"{styleKey} should be available from MaterialTheme");
		Assert.IsInstanceOfType(resource, typeof(Style), $"{styleKey} should be a Style");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_SemanticMaterialTimePickerKeys_Requested_ThenTheyAliasTheMaterialStyles()
	{
		var container = CreateThemedContainer();

		Assert.AreSame(
			container.Resources["MaterialTimePickerStyle"],
			container.Resources["TimePickerStyle"],
			"TimePickerStyle should alias MaterialTimePickerStyle");
		Assert.AreSame(
			container.Resources["MaterialTimePickerFlyoutPresenterStyle"],
			container.Resources["TimePickerFlyoutPresenterStyle"],
			"TimePickerFlyoutPresenterStyle should alias MaterialTimePickerFlyoutPresenterStyle");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_MaterialTimePickerLightweightKeys_Requested_ThenTheyResolve()
	{
		var container = CreateThemedContainer();

		var keys = new[]
		{
			"TimePickerButtonBackground",
			"TimePickerButtonBackgroundDisabled",
			"TimePickerButtonBorderBrush",
			"TimePickerButtonBorderBrushDisabled",
			"TimePickerButtonTimeTextForeground",
			"TimePickerButtonTimeTextForegroundDisabled",
			"TimePickerHeaderForeground",
			"TimePickerHeaderForegroundDisabled",
			"TimePickerPlaceholderTextForeground",
			"TimePickerSpacerFill",
			"TimePickerSpacerFillDisabled",
			"TimePickerFlyoutPresenterBackground",
			"TimePickerFlyoutPresenterBorderBrush",
			"TimePickerFlyoutPresenterSpacerFill",
			"TimePickerFlyoutPresenterHighlightFill",
			"TimePickerFlyoutPresenterCornerRadius",
			"TimePickerCornerRadius",
			"TimePickerHeight",
			"TimePickerColumnDividerWidth",
			"TimePickerColumnDividerMargin",
			"TimePickerButtonPlaceholderMargin",
			"TimePickerButtonContentMargin",
			"TimePickerHeaderFloatScale",
		};

		var missing = keys
			.Where(key => !container.Resources.TryGetValue(key, out var value) || value is null)
			.ToList();

		CollectionAssert.AreEqual(
			Array.Empty<string>(),
			missing,
			$"These Material TimePicker lightweight keys did not resolve: {string.Join(", ", missing)}");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TimePicker")]
	[DataRow("DatePicker")]
	public void When_HeaderFloatScale_Requested_ThenItResolvesOutsideThemeDictionaries(string control)
	{
		// The float scale is read by a Storyboard's To=, which resolves through StaticResource.
		// StaticResource does not re-resolve per ThemeDictionary, so a key defined only inside
		// ThemeDictionaries is documented as tunable but is not reliably overridable. Keeping it at
		// dictionary scope is what makes the documented lightweight-styling key actually work.
		var container = CreateThemedContainer();

		Assert.IsTrue(
			container.Resources.TryGetValue($"{control}HeaderFloatScale", out var value),
			$"{control}HeaderFloatScale should resolve");
		Assert.IsInstanceOfType(value, typeof(double), $"{control}HeaderFloatScale should be a Double");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Template-part contract and culture safety.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_MaterialTimePickerStyle_Applied_ThenRequiredTemplatePartsArePresent(ElementTheme theme)
	{
		var container = CreateThemedContainer(theme);
		var picker = new TimePicker
		{
			Header = "Pick a time",
			Style = container.Resources["MaterialTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var missing = TimePickerContractParts
			.Where(part => FindDescendantByName(picker, part) is null)
			.ToList();

		CollectionAssert.AreEqual(
			Array.Empty<string>(),
			missing,
			$"MaterialTimePickerStyle is missing these named template parts under {theme}: {string.Join(", ", missing)}");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TimeIsSelected_ThenTheDividersCarryNoOrderDependentText()
	{
		// UpdateOrderAndLayout reparents the hour/minute/period TextBlocks between the picker hosts
		// to reorder per culture, but it only toggles the *visibility* of the dividers — it never
		// moves them. A literal ":" in FirstColumnDivider is therefore correct only while the hour
		// comes first; in a period-first culture (ko-KR / zh-CN / ja-JP) it lands between the period
		// and the hour. Neutral rules carry no ordering assumption, so "not a TextBlock" is the
		// invariant worth pinning — and it is deterministic, unlike swapping the ambient culture.
		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			SelectedTime = new TimeSpan(9, 41, 0),
			Style = container.Resources["MaterialTimePickerStyle"] as Style,
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
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ClockIdentifierIs24Hour_ThenPeriodDividerCollapses()
	{
		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			ClockIdentifier = "24HourClock",
			Style = container.Resources["MaterialTimePickerStyle"] as Style,
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
	// Header placement — the floating label.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(typeof(TimePicker), "MaterialTimePickerStyle")]
	[DataRow(typeof(DatePicker), "MaterialDatePickerStyle")]
	public async Task When_HeaderIsSet_ThenItRendersOnce(Type pickerType, string styleKey)
	{
		// The header used to render twice — once as a label above the field and again as the
		// header-bound placeholder inside it.
		const string header = "Departure";

		var container = CreateThemedContainer();
		var picker = CreatePicker(pickerType, styleKey, container, header);
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(
			1,
			CountVisibleTextBlocksWithText(picker, header),
			$"{styleKey} should render '{header}' exactly once");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(typeof(TimePicker), "MaterialTimePickerStyle")]
	[DataRow(typeof(DatePicker), "MaterialDatePickerStyle")]
	public async Task When_NoHeaderIsSet_ThenTheValueIsVerticallyCentred(Type pickerType, string styleKey)
	{
		// A bare <TimePicker /> is the default usage. The header and value sit in two Auto rows so
		// that, with no header, row 0 collapses and the value centres. When the value instead
		// carried a fixed top inset sized for a header, a header-less field rendered its value
		// against the bottom edge.
		var container = CreateThemedContainer();
		var picker = CreatePicker(pickerType, styleKey, container);
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var header = FindDescendantByName(picker, "HeaderTextBlock");
		Assert.IsNotNull(header, "HeaderTextBlock should be part of the template");
		Assert.AreEqual(
			Visibility.Collapsed,
			header!.Visibility,
			"With no Header there is nothing to render, so the header row must collapse");

		var content = FindDescendantByName(picker, "FlyoutButtonContentGrid");
		Assert.IsNotNull(content, "FlyoutButtonContentGrid should be part of the template");

		var offset = content!.TransformToVisual(picker).TransformPoint(default).Y;
		var slack = picker.ActualHeight - content.ActualHeight;
		Assert.IsTrue(
			Math.Abs(offset - (slack / 2)) <= 2,
			$"With no Header the value should sit centred (expected ~{slack / 2:0.#}px from the top, "
				+ $"measured {offset:0.#}px of a {picker.ActualHeight:0.#}px field)");
	}

	private static FrameworkElement CreatePicker(Type pickerType, string styleKey, Grid container, string? header = null)
	{
		var picker = (FrameworkElement)Activator.CreateInstance(pickerType)!;
		switch (picker)
		{
			case TimePicker timePicker:
				timePicker.Header = header;
				break;
			case DatePicker datePicker:
				datePicker.Header = header;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(pickerType), pickerType, "Unsupported picker type");
		}

		picker.Style = container.Resources[styleKey] as Style;
		return picker;
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

	// ─────────────────────────────────────────────────────────────────────
	// Selection-band contrast, resolved per theme.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_FlyoutPresenterHighlight_Resolved_ThenItContrastsWithTheFlyoutBackground(ElementTheme theme)
	{
		// The selection band paints directly on the flyout background; when the two resolve to the
		// same color the selected row is invisible. Resolving through a themed element is what makes
		// the Dark row meaningful — the ResourceDictionary indexer would return the ambient theme's
		// brush for both rows.
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
	// Lightweight-styling override precedence.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TimePickerButtonBackgroundOverridden_ThenFieldReflectsChange()
	{
		var expected = Microsoft.UI.Colors.DarkOrchid;

		var container = CreateThemedContainer();
		container.Resources["TimePickerButtonBackground"] = new SolidColorBrush(expected);

		var picker = new TimePicker
		{
			Style = container.Resources["MaterialTimePickerStyle"] as Style,
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
			"Overriding TimePickerButtonBackground should change the Material TimePicker field background");
	}
}
