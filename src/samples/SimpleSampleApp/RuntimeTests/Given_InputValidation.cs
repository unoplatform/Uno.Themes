#nullable enable

// Uno targets only: input validation is an Uno Platform feature. On WinAppSDK the Validation properties
// exist (Uno.UI.Extras) but have no effect: the Simple styles' validation setters and templates load unused.
#if HAS_UNO
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;
// Aliases, not `using Uno.UI.Xaml.Controls`: that namespace also declares a ComboBox, which would clash with
// Microsoft.UI.Xaml.Controls.ComboBox.
using InputValidationKind = Uno.UI.Xaml.Controls.InputValidationKind;
using InputValidationMode = Uno.UI.Xaml.Controls.InputValidationMode;
using Validation = Uno.UI.Xaml.Controls.Validation;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the input validation visuals of the Simple input styles: the error templates resolve, a control
/// whose INotifyDataErrorInfo source reports errors reaches the Compact/Inline error states and shows the
/// ErrorPresenter and error ring, clearing the errors hides them again, and a control with no errors lays
/// out exactly as one without validation.
/// </summary>
/// <remarks>Relies on FeatureConfiguration.InputValidation being enabled by the App.</remarks>
[TestClass]
public class Given_InputValidation
{
	private const string ErrorMessage = "Value is invalid.";

	// Leak guard state lives in fields: locals would keep the tracked control reachable while the GC runs.
	private ErrorSource? _longLivedSource;
	private WeakReference<Control>? _removedControl;

	private static Grid CreateThemedContainer(ElementTheme theme = ElementTheme.Default)
	{
		var container = new Grid { Width = 400, RequestedTheme = theme };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		return container;
	}

	private static Control CreateControl(string styleKey, ResourceDictionary resources)
	{
		Control control = styleKey switch
		{
			"SimpleOutlinedTextBoxStyle" or "SimpleFilledTextBoxStyle" => new TextBox(),
			"SimpleOutlinedPasswordBoxStyle" or "SimpleFilledPasswordBoxStyle" => new PasswordBox(),
			"SimpleAutoSuggestBoxStyle" => new AutoSuggestBox(),
			"SimpleComboBoxStyle" => new ComboBox(),
			_ => throw new ArgumentOutOfRangeException(nameof(styleKey), styleKey, null),
		};
		control.Style = (Style)resources[styleKey];
		return control;
	}

	/// <summary>Binds the property each control type validates, the way an app would: through its DataContext.</summary>
	private static void BindValidatedProperty(Control control, ErrorSource source)
	{
		var property = control switch
		{
			TextBox => TextBox.TextProperty,
			PasswordBox => PasswordBox.PasswordProperty,
			AutoSuggestBox => AutoSuggestBox.TextProperty,
			ComboBox => ComboBox.SelectedItemProperty,
			_ => throw new ArgumentOutOfRangeException(nameof(control)),
		};

		control.DataContext = source;
		control.SetBinding(property, new Binding { Path = new PropertyPath(nameof(ErrorSource.Value)), Mode = BindingMode.TwoWay });
	}

	private static FrameworkElement? FindPart(Control control, string name) =>
		control.EnumerateDescendants().OfType<FrameworkElement>().FirstOrDefault(x => x.Name == name);

	/// <summary>Gets the name of the current state of a visual state group declared on the control's template root.</summary>
	private static string? GetCurrentState(Control control, string groupName) =>
		VisualTreeHelper.GetChild(control, 0) is FrameworkElement root
			? VisualStateManager.GetVisualStateGroups(root).FirstOrDefault(x => x.Name == groupName)?.CurrentState?.Name
			: null;

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_ThemeApplied_Then_ErrorTemplatesResolve(ElementTheme theme)
	{
		// Arrange
		var container = CreateThemedContainer(theme);

		// Act
		var errorTemplate = container.Resources["SimpleInputValidationErrorTemplate"] as DataTemplate;
		var iconTemplate = container.Resources["DefaultCompactErrorIconTemplate"] as DataTemplate;

		// Assert
		Assert.IsNotNull(errorTemplate, "SimpleInputValidationErrorTemplate should resolve");
		Assert.IsNotNull(iconTemplate, "DefaultCompactErrorIconTemplate should resolve");
		Assert.IsInstanceOfType(errorTemplate.LoadContent(), typeof(ItemsControl));

		var icon = iconTemplate.LoadContent() as FontIcon;
		Assert.IsNotNull(icon, "The compact error icon should be a FontIcon");
		Assert.IsInstanceOfType(ToolTipService.GetToolTip(icon), typeof(ToolTip), "The framework fills the icon's ToolTip with the errors");

		// The icon is painted with the theme's own ErrorBrush, in either theme.
		var probe = (Border)XamlReader.Load("""
			<Border xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
					Width="1"
					Height="1"
					Background="{ThemeResource ErrorBrush}" />
			""");
		container.Children.Add(probe);
		container.Children.Add(icon);
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(icon);
		await UnitTestsUIContentHelper.WaitForIdle();

		var expected = (probe.Background as SolidColorBrush)?.Color;
		Assert.IsNotNull(expected, "ErrorBrush should resolve to a SolidColorBrush");
		Assert.AreEqual(expected, (icon.Foreground as SolidColorBrush)?.Color, "The icon should use the theme's ErrorBrush");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleOutlinedTextBoxStyle", InputValidationKind.Compact)]
	[DataRow("SimpleOutlinedTextBoxStyle", InputValidationKind.Inline)]
	[DataRow("SimpleFilledTextBoxStyle", InputValidationKind.Compact)]
	[DataRow("SimpleFilledTextBoxStyle", InputValidationKind.Inline)]
	[DataRow("SimpleOutlinedPasswordBoxStyle", InputValidationKind.Compact)]
	[DataRow("SimpleOutlinedPasswordBoxStyle", InputValidationKind.Inline)]
	[DataRow("SimpleFilledPasswordBoxStyle", InputValidationKind.Compact)]
	[DataRow("SimpleFilledPasswordBoxStyle", InputValidationKind.Inline)]
	[DataRow("SimpleAutoSuggestBoxStyle", InputValidationKind.Compact)]
	[DataRow("SimpleAutoSuggestBoxStyle", InputValidationKind.Inline)]
	[DataRow("SimpleComboBoxStyle", InputValidationKind.Compact)]
	[DataRow("SimpleComboBoxStyle", InputValidationKind.Inline)]
	public async Task When_SourceReportsErrors_Then_ErrorStateShowsAndClears(string styleKey, InputValidationKind kind)
	{
		// Arrange
		var container = CreateThemedContainer();
		var control = CreateControl(styleKey, container.Resources);
		var source = new ErrorSource(ErrorMessage);
		Validation.SetMode(control, InputValidationMode.Auto);
		Validation.SetKind(control, kind);
		BindValidatedProperty(control, source);
		container.Children.Add(control);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert: in error
		Assert.IsTrue(Validation.GetHasErrors(control), "The source's error should reach the control");

		var presenter = FindPart(control, "ErrorPresenter");
		var ring = FindPart(control, "ErrorBorderElement");
		Assert.IsNotNull(presenter, "ErrorPresenter should be realized while in error");
		Assert.IsNotNull(ring, "ErrorBorderElement should be part of the template");
		Assert.AreEqual(Visibility.Visible, presenter.Visibility, "ErrorPresenter should show while in error");
		Assert.AreEqual(Visibility.Visible, ring.Visibility, "The error ring should show while in error");

		var presenterRoot = (Grid)VisualTreeHelper.GetParent(presenter);
		if (kind == InputValidationKind.Compact)
		{
			Assert.AreEqual(1, Grid.GetColumn(presenter), "Compact: the icon sits in the icon column");
			Assert.IsTrue(presenter.ActualWidth > 0, "Compact: the icon column should have opened");

			var icon = (presenter as ContentPresenter)?.Content as FrameworkElement;
			Assert.IsNotNull(icon, "Compact: ErrorPresenter should hold the error icon");
			var toolTip = ToolTipService.GetToolTip(icon) as ToolTip;
			Assert.IsNotNull(toolTip?.Content, "Compact: the icon's tooltip should carry the errors");
		}
		else
		{
			Assert.AreEqual(presenterRoot.RowDefinitions.Count - 1, Grid.GetRow(presenter), "Inline: the errors sit in the last row");
			Assert.AreEqual(0, Grid.GetColumn(presenter), "Inline: the errors span from the first column");
		}

		if (control is AutoSuggestBox)
		{
			var innerTextBox = control.EnumerateDescendants().OfType<TextBox>().First();
			Assert.IsNotNull(innerTextBox.Tag, "The error states signal the inner TextBox through its Tag");
		}

		// Act: the source clears its errors
		source.SetErrors();
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert: back to clean
		Assert.IsFalse(Validation.GetHasErrors(control), "The control should no longer be in error");
		Assert.AreEqual(Visibility.Collapsed, presenter.Visibility, "ErrorPresenter should hide once the errors clear");
		Assert.AreEqual(Visibility.Collapsed, ring.Visibility, "The error ring should hide once the errors clear");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(false, InputValidationKind.Compact)]
	[DataRow(false, InputValidationKind.Inline)]
	[DataRow(true, InputValidationKind.Compact)]
	[DataRow(true, InputValidationKind.Inline)]
	public async Task When_AutoSuggestBoxWithHeaderInError_Then_ErrorRingFitsTheField(bool elementHeader, InputValidationKind kind)
	{
		// Arrange: an AutoSuggestBox in error, with a text header or an element header
		var container = CreateThemedContainer();
		var control = (AutoSuggestBox)CreateControl("SimpleAutoSuggestBoxStyle", container.Resources);
		control.Header = elementHeader ? new StackPanel { Children = { new TextBlock { Text = "Header" }, new TextBlock { Text = "Second line" } } } : "Header";
		Validation.SetMode(control, InputValidationMode.Auto);
		Validation.SetKind(control, kind);
		BindValidatedProperty(control, new ErrorSource(ErrorMessage));
		container.Children.Add(control);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert: the ring has the bounds of the field (the inner TextBox's BorderElement), not of field and header
		var ring = FindPart(control, "ErrorBorderElement");
		var field = FindPart(control, "BorderElement");
		Assert.IsNotNull(ring, "ErrorBorderElement should be part of the template");
		Assert.IsNotNull(field, "The inner TextBox should have its BorderElement");
		Assert.AreEqual(Visibility.Visible, ring.Visibility, "The error ring should show while in error");

		var ringBounds = GetBounds(ring, control);
		var fieldBounds = GetBounds(field, control);
		Assert.AreEqual(fieldBounds.Y, ringBounds.Y, 0.5, "The error ring should start where the field starts");
		Assert.AreEqual(fieldBounds.Height, ringBounds.Height, 0.5, "The error ring should be as tall as the field");
		Assert.AreEqual(fieldBounds.X, ringBounds.X, 0.5, "The error ring should start at the field's left edge");
		Assert.AreEqual(fieldBounds.Width, ringBounds.Width, 0.5, "The error ring should be as wide as the field");
	}

	private static Rect GetBounds(FrameworkElement element, UIElement relativeTo) =>
		element.TransformToVisual(relativeTo).TransformBounds(new Rect(0, 0, element.ActualWidth, element.ActualHeight));

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(InputValidationKind.Compact)]
	[DataRow(InputValidationKind.Inline)]
	public async Task When_ShownAfterBeingCollapsed_Then_ErrorStateShows(InputValidationKind kind)
	{
		// Arrange: a control already in error, loaded inside a collapsed panel
		var container = CreateThemedContainer();
		var control = CreateControl("SimpleOutlinedTextBoxStyle", container.Resources);
		Validation.SetMode(control, InputValidationMode.Auto);
		Validation.SetKind(control, kind);
		BindValidatedProperty(control, new ErrorSource(ErrorMessage));
		var panel = new StackPanel { Visibility = Visibility.Collapsed };
		panel.Children.Add(control);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForIdle();
		Assert.IsTrue(panel.IsLoaded, "The collapsed panel should be loaded");
		await UnitTestsUIContentHelper.WaitForIdle();

		// Act
		panel.Visibility = Visibility.Visible;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert
		Assert.IsTrue(Validation.GetHasErrors(control));
		Assert.AreEqual(Visibility.Visible, FindPart(control, "ErrorPresenter")?.Visibility, "ErrorPresenter should show once the control is shown");
		Assert.AreEqual(Visibility.Visible, FindPart(control, "ErrorBorderElement")?.Visibility, "The error ring should show once the control is shown");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleOutlinedTextBoxStyle")]
	[DataRow("SimpleFilledTextBoxStyle")]
	[DataRow("SimpleOutlinedPasswordBoxStyle")]
	[DataRow("SimpleFilledPasswordBoxStyle")]
	[DataRow("SimpleAutoSuggestBoxStyle")]
	[DataRow("SimpleComboBoxStyle")]
	public async Task When_NoErrors_Then_LayoutMatchesUnvalidatedControl(string styleKey)
	{
		// Arrange: the same style twice, one validating against a clean source, one not validating at all
		var container = CreateThemedContainer();
		var validated = CreateControl(styleKey, container.Resources);
		var plain = CreateControl(styleKey, container.Resources);
		Validation.SetMode(validated, InputValidationMode.Auto);
		BindValidatedProperty(validated, new ErrorSource());

		var panel = new StackPanel();
		panel.Children.Add(validated);
		panel.Children.Add(plain);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(validated);
		await UnitTestsUIContentHelper.WaitForLoaded(plain);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert
		Assert.IsFalse(Validation.GetHasErrors(validated));
		Assert.AreEqual(plain.ActualWidth, validated.ActualWidth, 0.5, "A clean validated control should be as wide as an unvalidated one");
		Assert.AreEqual(plain.ActualHeight, validated.ActualHeight, 0.5, "A clean validated control should be as tall as an unvalidated one");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ModeDisabledThenAuto_Then_ValidationDisablesAndReturns()
	{
		// Arrange: a control in error
		var container = CreateThemedContainer();
		var control = CreateControl("SimpleOutlinedTextBoxStyle", container.Resources);
		Validation.SetMode(control, InputValidationMode.Auto);
		BindValidatedProperty(control, new ErrorSource(ErrorMessage));
		container.Children.Add(control);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();
		Assert.IsTrue(Validation.GetHasErrors(control));

		// Act: validation turned off
		Validation.SetMode(control, InputValidationMode.Disabled);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert: the control leaves the enabled states. Like WinUI (CControl::EnsureValidationVisuals), the framework
		// leaves the error states group where it was, so the error visuals stay until validation is turned back on.
		// The ErrorPresenter is not asserted: WinUI unloads it (DeferErrors), while Uno cannot and keeps it visible.
		Assert.IsFalse(Validation.GetHasErrors(control), "A control that does not validate has no errors");
		Assert.AreEqual("ValidationDisabled", GetCurrentState(control, "InputValidationEnabledStates"));
		Assert.AreEqual("CompactErrors", GetCurrentState(control, "InputValidationErrorStates"), "The error states group is left as it was");

		// Act: validation turned back on
		Validation.SetMode(control, InputValidationMode.Auto);
		await UnitTestsUIContentHelper.WaitForIdle();

		// Assert: the source's error is presented again
		Assert.IsTrue(Validation.GetHasErrors(control), "The source's error should be picked up again");
		Assert.AreEqual("CompactValidationEnabled", GetCurrentState(control, "InputValidationEnabledStates"));
		Assert.AreEqual("CompactErrors", GetCurrentState(control, "InputValidationErrorStates"));
		Assert.AreEqual(Visibility.Visible, FindPart(control, "ErrorPresenter")?.Visibility, "ErrorPresenter should show again");
		Assert.AreEqual(Visibility.Visible, FindPart(control, "ErrorBorderElement")?.Visibility, "The error ring should show again");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ControlRemoved_Then_LongLivedSourceDoesNotKeepItAlive()
	{
		// Arrange: a source that outlives the control, as a view model typically does
		_longLivedSource = new ErrorSource(ErrorMessage);
		var container = CreateThemedContainer();
		UnitTestsUIContentHelper.Content = container;
		await AddValidatedControl(container, _longLivedSource);

		// Act: the control leaves the tree
		container.Children.Clear();
		await UnitTestsUIContentHelper.WaitForIdle();
		await CollectGarbage();

		// Assert
		Assert.IsNotNull(_removedControl);
		Assert.IsFalse(_removedControl.TryGetTarget(out _), "The source's ErrorsChanged subscription should not keep a removed control alive");
		GC.KeepAlive(_longLivedSource);
	}

	/// <summary>Creates, binds and loads the control in its own frame, so no local outlives it.</summary>
	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task AddValidatedControl(Grid container, ErrorSource source)
	{
		var control = CreateControl("SimpleOutlinedTextBoxStyle", container.Resources);
		Validation.SetMode(control, InputValidationMode.Auto);
		BindValidatedProperty(control, source);
		container.Children.Add(control);
		await UnitTestsUIContentHelper.WaitForLoaded(control);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.IsTrue(Validation.GetHasErrors(control), "The control should be subscribed to the source");
		_removedControl = new WeakReference<Control>(control);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static async Task CollectGarbage()
	{
		for (var i = 0; i < 3; i++)
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			await UnitTestsUIContentHelper.WaitForIdle();
		}
	}

	/// <summary>A one-property INotifyDataErrorInfo source whose errors are set by the test.</summary>
	private sealed class ErrorSource : INotifyPropertyChanged, INotifyDataErrorInfo
	{
		private string[] _errors;

		public ErrorSource(params string[] errors) => _errors = errors;

		public string? Value
		{
			get;
			set
			{
				field = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}

		public void SetErrors(params string[] errors)
		{
			_errors = errors;
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Value)));
		}

		public bool HasErrors => _errors.Length != 0;

		public IEnumerable GetErrors(string? propertyName) =>
			string.IsNullOrEmpty(propertyName) || propertyName == nameof(Value) ? _errors : Array.Empty<string>();

		public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
#endif
