using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_CupertinoSemanticTypography
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TextBox", "BodyLarge")]
	[DataRow("PasswordBox", "BodyLarge")]
	[DataRow("NumberBox", "BodyLarge")]
	[DataRow("ComboBox", "BodyLarge")]
	[DataRow("DatePicker", "BodyLarge")]
	[DataRow("CalendarDatePicker", "BodyLarge")]
	[DataRow("TimePicker", "BodyLarge")]
	[DataRow("Button", "LabelLarge")]
	[DataRow("ToggleButton", "LabelLarge")]
	[DataRow("ToggleSwitch", "BodySmall")]
	[DataRow("MenuFlyoutItem", "BodyLarge")]
	[DataRow("ListViewItem", "BodyLarge")]
	[DataRow("AppBarButton", "BodyLarge")]
	public async Task When_SlotOverridden_Then_DefaultControlUsesAllTypographyMetrics(string controlName, string slot)
	{
		foreach (var appearance in new[] { ElementTheme.Light, ElementTheme.Dark })
		{
			var control = CreateControl(controlName);
			var scope = new StackPanel { RequestedTheme = appearance, Children = { control } };
			scope.Resources[slot + "FontSize"] = 23d;
			if (controlName is "Button" or "ToggleButton")
			{
				scope.Resources["CupertinoButtonFontSize"] = 23d;
			}
			scope.Resources[slot + "FontFamily"] = new FontFamily("Arial");
			scope.Resources[slot + "FontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
			scope.Resources[slot + "CharacterSpacing"] = 37;
			try
			{
				UnitTestsUIContentHelper.Content = scope;
				await UnitTestsUIContentHelper.WaitForLoaded(control);
				Assert.AreEqual(23d, control.FontSize, controlName + " " + appearance + " size");
				Assert.AreEqual("Arial", control.FontFamily.Source, controlName + " family");
				Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, control.FontWeight.Weight, controlName + " weight");
				Assert.AreEqual(37, control.CharacterSpacing, controlName + " tracking");
				if (controlName == "ToggleSwitch")
				{
					var headerText = FindHeader(control);
					Assert.IsNotNull(headerText);
					Assert.AreEqual(23d, headerText.FontSize);
					Assert.AreEqual("Arial", headerText.FontFamily.Source);
					Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, headerText.FontWeight.Weight);
					Assert.AreEqual(37, headerText.CharacterSpacing);
				}
				if (controlName is "Button" or "ToggleButton" or "AppBarButton" or "ListViewItem" or "MenuFlyoutItem")
				{
					var contentText = FindHeader(control, "Body");
					Assert.IsNotNull(contentText, controlName + " rendered content");
					Assert.AreEqual(23d, contentText.FontSize, controlName + " rendered size");
					Assert.AreEqual("Arial", contentText.FontFamily.Source, controlName + " rendered family");
					Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, contentText.FontWeight.Weight, controlName + " rendered weight");
					Assert.AreEqual(37, contentText.CharacterSpacing, controlName + " rendered tracking");
				}
				if (control is NumberBox)
				{
					var input = FindInput(control);
					Assert.IsNotNull(input);
					Assert.AreEqual(23d, input.FontSize);
					Assert.AreEqual("Arial", input.FontFamily.Source);
					Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, input.FontWeight.Weight);
					Assert.AreEqual(37, input.CharacterSpacing);
				}
			}
			finally
			{
				UnitTestsUIContentHelper.Content = null;
			}
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("TextBox")]
	[DataRow("PasswordBox")]
	[DataRow("NumberBox")]
	[DataRow("ComboBox")]
	[DataRow("DatePicker")]
	[DataRow("CalendarDatePicker")]
	[DataRow("TimePicker")]
	public async Task When_CaptionOverridden_Then_FieldHeaderUsesCaptionMetrics(string controlName)
	{
		var control = CreateControl(controlName);
		var scope = new StackPanel { Children = { control } };
		scope.Resources["CaptionLargeFontSize"] = 19d;
		scope.Resources["CupertinoHeaderFontSize"] = 19d;
		scope.Resources["CaptionLargeFontFamily"] = new FontFamily("Arial");
		scope.Resources["CaptionLargeFontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
		scope.Resources["CaptionLargeCharacterSpacing"] = 31;
		try
		{
			UnitTestsUIContentHelper.Content = scope;
			await UnitTestsUIContentHelper.WaitForLoaded(control);
			var header = FindHeader(control);
			Assert.IsNotNull(header, controlName + " header presenter");
			Assert.AreEqual(19d, header.FontSize, controlName + " header size");
			Assert.AreEqual("Arial", header.FontFamily.Source, controlName + " header family");
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, header.FontWeight.Weight, controlName + " header weight");
			Assert.AreEqual(31, header.CharacterSpacing, controlName + " header tracking");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_RootFontChangedAndCleared_Then_RenderedControlRefreshes()
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var original = theme.DefaultFontFamily;
		var control = new TextBox { Text = "Root font" };
		var scope = new StackPanel { RequestedTheme = ElementTheme.Light, Children = { control } };
		try
		{
			UnitTestsUIContentHelper.Content = scope;
			await UnitTestsUIContentHelper.WaitForLoaded(control);
			var initial = control.FontFamily.Source;
			theme.DefaultFontFamily = new FontFamily("Arial");
			await Refresh(scope);
			Assert.AreEqual("Arial", control.FontFamily.Source);
			theme.DefaultFontFamily = original;
			await Refresh(scope);
			Assert.AreEqual(initial, control.FontFamily.Source);
		}
		finally
		{
			theme.DefaultFontFamily = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_EachTextSlotOverridden_Then_RenderedTextUsesEveryMetric(ElementTheme appearance)
	{
		var slots = new[]
		{
			"DisplayLarge", "DisplayMedium", "DisplaySmall", "HeadlineLarge", "HeadlineMedium", "HeadlineSmall",
			"TitleLarge", "TitleMedium", "TitleSmall", "BodyLarge", "BodyMedium", "BodySmall",
			"LabelLarge", "LabelMedium", "LabelSmall", "LabelExtraSmall", "CaptionLarge", "CaptionMedium", "CaptionSmall",
		};
		foreach (var slot in slots)
		{
			var text = new TextBlock { Text = slot, Style = (Style)Application.Current.Resources[slot] };
			var scope = new StackPanel { RequestedTheme = appearance, Children = { text } };
			scope.Resources[slot + "FontSize"] = 29d;
			scope.Resources[slot + "FontFamily"] = new FontFamily("Arial");
			scope.Resources[slot + "FontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
			scope.Resources[slot + "CharacterSpacing"] = 43;
			try
			{
				UnitTestsUIContentHelper.Content = scope;
				await UnitTestsUIContentHelper.WaitForLoaded(text);
				Assert.AreEqual(29d, text.FontSize, slot);
				Assert.AreEqual("Arial", text.FontFamily.Source, slot);
				Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, text.FontWeight.Weight, slot);
				Assert.AreEqual(43, text.CharacterSpacing, slot);
			}
			finally
			{
				UnitTestsUIContentHelper.Content = null;
			}
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ApplicationTypographyChanges_Then_LegacySizeAliasesFollowAndRestore()
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var original = theme.FontOverrideDictionary;
		var button = new Button { Content = "Button" };
		var number = new NumberBox { Header = "Header", Value = 1 };
		var scope = new StackPanel { RequestedTheme = ElementTheme.Light, Children = { button, number } };
		try
		{
			UnitTestsUIContentHelper.Content = scope;
			await UnitTestsUIContentHelper.WaitForLoaded(number);
			var header = FindHeader(number);
			Assert.IsNotNull(header);
			var initialButtonSize = button.FontSize;
			var initialHeaderSize = header.FontSize;
			theme.FontOverrideDictionary = new ResourceDictionary
			{
				["LabelLargeFontSize"] = 23d,
				["CaptionLargeFontSize"] = 19d,
			};
			await Refresh(scope);
			Assert.AreEqual(23d, button.FontSize, "button alias follows semantic application override");
			Assert.AreEqual(19d, header.FontSize, "header alias follows semantic application override");
			theme.FontOverrideDictionary = original;
			await Refresh(scope);
			Assert.AreEqual(initialButtonSize, button.FontSize, "button alias restores");
			Assert.AreEqual(initialHeaderSize, header.FontSize, "header alias restores");
		}
		finally
		{
			theme.FontOverrideDictionary = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_CalendarSlotsOverridden_Then_DayAndMonthMetricsFollow(ElementTheme appearance)
	{
		var calendar = new CalendarView();
		var scope = new StackPanel { RequestedTheme = appearance, Children = { calendar } };
		scope.Resources["TitleMediumFontSize"] = 27d;
		scope.Resources["TitleMediumFontFamily"] = new FontFamily("Arial");
		scope.Resources["TitleMediumFontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
		scope.Resources["TitleMediumCharacterSpacing"] = 39;
		scope.Resources["HeadlineSmallFontSize"] = 25d;
		scope.Resources["HeadlineSmallFontFamily"] = new FontFamily("Arial");
		scope.Resources["HeadlineSmallFontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
		scope.Resources["BodyLargeFontSize"] = 23d;
		scope.Resources["BodyLargeFontFamily"] = new FontFamily("Arial");
		scope.Resources["BodyLargeFontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
		try
		{
			UnitTestsUIContentHelper.Content = scope;
			await UnitTestsUIContentHelper.WaitForLoaded(calendar);
			Assert.AreEqual(25d, calendar.DayItemFontSize);
			Assert.AreEqual("Arial", calendar.DayItemFontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, calendar.DayItemFontWeight.Weight);
			Assert.AreEqual(23d, calendar.MonthYearItemFontSize);
			Assert.AreEqual("Arial", calendar.MonthYearItemFontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, calendar.MonthYearItemFontWeight.Weight);
			var heading = FindCalendarHeading(calendar);
			Assert.IsNotNull(heading);
			Assert.AreEqual(27d, heading.FontSize);
			Assert.AreEqual("Arial", heading.FontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, heading.FontWeight.Weight);
			Assert.AreEqual(39, heading.CharacterSpacing);
			var headingText = FindHeader(heading, heading.Content?.ToString() ?? string.Empty);
			Assert.IsNotNull(headingText);
			Assert.AreEqual(27d, headingText.FontSize);
			Assert.AreEqual("Arial", headingText.FontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, headingText.FontWeight.Weight);
			Assert.AreEqual(39, headingText.CharacterSpacing);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ToolTipCaptionOverridden_Then_RenderedHintUsesFullSlot()
	{
		var target = new Button { Content = "Target" };
		var scope = new StackPanel { Children = { target } };
		var tip = new ToolTip { Content = "Header" };
		tip.Resources["CaptionLargeFontSize"] = 19d;
		tip.Resources["CaptionLargeFontFamily"] = new FontFamily("Arial");
		tip.Resources["CaptionLargeFontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
		tip.Resources["CaptionLargeCharacterSpacing"] = 31;
		ToolTipService.SetToolTip(target, tip);
		try
		{
			UnitTestsUIContentHelper.Content = scope;
			await UnitTestsUIContentHelper.WaitForLoaded(target);
			tip.IsOpen = true;
			await UnitTestsUIContentHelper.WaitForIdle();
			var text = FindHeader(tip);
			Assert.IsNotNull(text);
			Assert.AreEqual(19d, text.FontSize);
			Assert.AreEqual("Arial", text.FontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, text.FontWeight.Weight);
			Assert.AreEqual(31, text.CharacterSpacing);
		}
		finally
		{
			tip.IsOpen = false;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DialogSlotsOverridden_Then_RenderedTitleAndBodyUseAllMetrics()
	{
		var scope = new Grid { Width = 600, Height = 500, Children = { new Button { Content = "Host" } } };
		var dialog = new ContentDialog { Title = "Header", Content = "Body", CloseButtonText = "Close" };
		foreach (var slot in new[] { "TitleMedium", "CaptionLarge" })
		{
			dialog.Resources[slot + "FontSize"] = 23d;
			dialog.Resources[slot + "FontFamily"] = new FontFamily("Arial");
			dialog.Resources[slot + "FontWeight"] = Microsoft.UI.Text.FontWeights.Bold;
			dialog.Resources[slot + "CharacterSpacing"] = 37;
		}
		Windows.Foundation.IAsyncOperation<ContentDialogResult>? showing = null;
		try
		{
			UnitTestsUIContentHelper.Content = scope;
			await UnitTestsUIContentHelper.WaitForLoaded(scope);
			dialog.XamlRoot = scope.XamlRoot;
			showing = dialog.ShowAsync();
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(23d, dialog.FontSize);
			Assert.AreEqual("Arial", dialog.FontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, dialog.FontWeight.Weight);
			Assert.AreEqual(37, dialog.CharacterSpacing);
			var body = FindHeader(dialog, "Body");
			Assert.IsNotNull(body);
			Assert.AreEqual(23d, body.FontSize);
			Assert.AreEqual("Arial", body.FontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, body.FontWeight.Weight);
			Assert.AreEqual(37, body.CharacterSpacing);
			var title = FindHeader(dialog);
			Assert.IsNotNull(title);
			Assert.AreEqual(23d, title.FontSize);
			Assert.AreEqual("Arial", title.FontFamily.Source);
			Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold.Weight, title.FontWeight.Weight);
			Assert.AreEqual(37, title.CharacterSpacing);
		}
		finally
		{
			dialog.Hide();
			if (showing is not null)
			{
				await showing;
			}
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ContentIsReplaced_Then_TrackingFollowsWithoutChangingCustomContent()
	{
		var custom = new TextBlock { Text = "Custom", CharacterSpacing = 71 };
		var button = new Button { Content = custom, CharacterSpacing = 37 };
		var scope = new StackPanel { Children = { button } };
		try
		{
			UnitTestsUIContentHelper.Content = scope;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			Assert.AreEqual(71, custom.CharacterSpacing);
			button.Content = "Body";
			await UnitTestsUIContentHelper.WaitForIdle();
			var text = FindHeader(button, "Body");
			Assert.IsNotNull(text);
			Assert.AreEqual(37, text.CharacterSpacing);
			button.CharacterSpacing = 45;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(45, text.CharacterSpacing, "existing generated text follows a live control property");
			button.ContentTemplate = (DataTemplate)Microsoft.UI.Xaml.Markup.XamlReader.Load("""
				<DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
				    <TextBlock Text="Template" CharacterSpacing="83" />
				</DataTemplate>
				""");
			await UnitTestsUIContentHelper.WaitForIdle();
			var templated = FindHeader(button, "Template");
			Assert.IsNotNull(templated);
			Assert.AreEqual(83, templated.CharacterSpacing);
			button.ContentTemplate = null;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(45, FindHeader(button, "Body")?.CharacterSpacing);
			scope.Children.Remove(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			scope.Children.Add(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(45, FindHeader(button, "Body")?.CharacterSpacing, "reload restores binding");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private WeakReference<Button>? _releasedButton;

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_TrackedPresenterUnloaded_Then_ControlCanBeCollected()
	{
		await CreateAndReleaseTrackedButton();
		await UnitTestsUIContentHelper.WaitForIdle();
		CollectReleasedButton();
		Assert.IsNotNull(_releasedButton);
		Assert.IsFalse(_releasedButton.TryGetTarget(out _), "tracking bridge must not retain its unloaded control");
	}

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
	private async Task CreateAndReleaseTrackedButton()
	{
		var button = new Button { Content = "Body", CharacterSpacing = 37 };
		_releasedButton = new WeakReference<Button>(button);
		UnitTestsUIContentHelper.Content = new StackPanel { Children = { button } };
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		UnitTestsUIContentHelper.Content = null;
	}

	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
	private static void CollectReleasedButton()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	private static Button? FindCalendarHeading(DependencyObject root)
	{
		if (root is Button button && button.Name == "HeaderButton")
		{
			return button;
		}
		for (var index = 0; index < VisualTreeHelper.GetChildrenCount(root); index++)
		{
			if (FindCalendarHeading(VisualTreeHelper.GetChild(root, index)) is { } heading)
			{
				return heading;
			}
		}
		return null;
	}

	private static async Task Refresh(FrameworkElement root)
	{
		root.RequestedTheme = ElementTheme.Dark;
		await UnitTestsUIContentHelper.WaitForIdle();
		root.RequestedTheme = ElementTheme.Light;
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	private static TextBox? FindInput(DependencyObject root)
	{
		if (root is TextBox input && input.Name == "InputBox")
		{
			return input;
		}
		for (var index = 0; index < VisualTreeHelper.GetChildrenCount(root); index++)
		{
			if (FindInput(VisualTreeHelper.GetChild(root, index)) is { } child)
			{
				return child;
			}
		}
		return null;
	}

	private static TextBlock? FindHeader(DependencyObject root, string text = "Header")
	{
		if (root is TextBlock presenter && presenter.Text == text)
		{
			return presenter;
		}
		for (var index = 0; index < VisualTreeHelper.GetChildrenCount(root); index++)
		{
			if (FindHeader(VisualTreeHelper.GetChild(root, index), text) is { } header)
			{
				return header;
			}
		}
		return null;
	}

	private static Control CreateControl(string name) => name switch
	{
		"TextBox" => new TextBox { Header = "Header", Text = "Body" },
		"PasswordBox" => new PasswordBox { Header = "Header", Password = "Body" },
		"NumberBox" => new NumberBox { Header = "Header", Value = 1 },
		"ComboBox" => new ComboBox { Header = "Header", Items = { "Body" }, SelectedIndex = 0 },
		"DatePicker" => new DatePicker { Header = "Header" },
		"CalendarDatePicker" => new CalendarDatePicker { Header = "Header" },
		"TimePicker" => new TimePicker { Header = "Header" },
		"Button" => new Button { Content = "Body" },
		"ToggleButton" => new Microsoft.UI.Xaml.Controls.Primitives.ToggleButton { Content = "Body" },
		"ToggleSwitch" => new ToggleSwitch { Header = "Header" },
		"MenuFlyoutItem" => new MenuFlyoutItem { Text = "Body" },
		"ListViewItem" => new ListViewItem { Content = "Body" },
		"AppBarButton" => new AppBarButton { Label = "Body" },
		_ => throw new ArgumentOutOfRangeException(nameof(name)),
	};
}
