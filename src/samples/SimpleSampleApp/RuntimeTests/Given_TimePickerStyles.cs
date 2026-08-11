#nullable enable

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
	/// <summary>Named parts the WinUI/Uno TimePicker looks up in its template.</summary>
	private static readonly string[] RequiredTimePickerParts =
	{
		"LayoutRoot",
		"PlaceholderText",
		"FlyoutButton",
		"FlyoutButtonContentGrid",
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
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
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

		var missing = new List<string>();
		foreach (var key in keys)
		{
			if (!container.Resources.TryGetValue(key, out var value) || value is null)
			{
				missing.Add(key);
			}
		}

		CollectionAssert.AreEqual(
			System.Array.Empty<string>(),
			missing,
			$"These TimePicker lightweight keys did not resolve under {theme}: {string.Join(", ", missing)}");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FlyoutPresenterHighlight_Resolved_ThenItContrastsWithTheFlyoutBackground()
	{
		// The selection band sits directly on the flyout background; when the two resolve to the
		// same color the selected row is invisible — PrimaryVariantLightBrush was #1E1E1E under
		// Dark, exactly SurfaceBrush. Note the ResourceDictionary indexer resolves theme
		// dictionaries against the *application* theme, so this covers the ambient theme only.
		var container = CreateThemedContainer();

		var highlight = container.Resources["TimePickerFlyoutPresenterHighlightFill"] as SolidColorBrush;
		var background = container.Resources["TimePickerFlyoutPresenterBackground"] as SolidColorBrush;

		Assert.IsNotNull(highlight, "TimePickerFlyoutPresenterHighlightFill should be a SolidColorBrush");
		Assert.IsNotNull(background, "TimePickerFlyoutPresenterBackground should be a SolidColorBrush");
		Assert.AreNotEqual(
			background!.Color,
			highlight!.Color,
			"The TimePicker selection band must be distinguishable from the flyout background");
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

		var missing = new List<string>();
		foreach (var part in RequiredTimePickerParts)
		{
			if (FindDescendantByName(picker, part) is null)
			{
				missing.Add(part);
			}
		}

		CollectionAssert.AreEqual(
			System.Array.Empty<string>(),
			missing,
			$"SimpleTimePickerStyle is missing these named template parts under {theme}: {string.Join(", ", missing)}");
	}

	// Field sizing (shared with DatePicker) lives in Given_PickerFieldSizing.

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

		var placeholder = FindDescendantByName(picker, "PlaceholderText") as TextBlock;
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
	public async Task When_TimeIsSelected_ThenTheFieldReadsAsOneCompactValue()
	{
		// The field should look like a single text field ("9:41 AM"), not three cells stretched
		// across the control's width, so the content run stays far narrower than the field itself.
		var container = CreateThemedContainer();
		var picker = new TimePicker
		{
			Width = 400,
			SelectedTime = new System.TimeSpan(9, 41, 0),
			Style = container.Resources["SimpleTimePickerStyle"] as Style,
		};
		container.Children.Add(picker);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(picker);
		await UnitTestsUIContentHelper.WaitForIdle();

		var content = FindDescendantByName(picker, "FlyoutButtonContentGrid");
		Assert.IsNotNull(content, "FlyoutButtonContentGrid should be part of the template");
		Assert.IsTrue(
			content!.ActualWidth < picker.ActualWidth / 2,
			$"The time value should stay a compact run, but it took {content.ActualWidth} of {picker.ActualWidth}px");

		// The separator is a TextBlock (First|SecondColumnDivider are UIElement in the control
		// contract), so a revert to a Rectangle rule fails this cast.
		var separator = FindDescendantByName(picker, "FirstColumnDivider") as TextBlock;
		Assert.IsNotNull(separator, "FirstColumnDivider should be the textual time separator");
		Assert.AreEqual(":", separator!.Text, "The hour and minute should be separated by a colon");
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
