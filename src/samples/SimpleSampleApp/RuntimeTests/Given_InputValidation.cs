#nullable enable

using System.Collections;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Extras.Input;
using Uno.Simple;
using Uno.UI.RuntimeTests;

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
		VisualTreeHelperEx.EnumerateDescendants(control).OfType<FrameworkElement>().FirstOrDefault(x => x.Name == name);

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
			var innerTextBox = VisualTreeHelperEx.EnumerateDescendants(control).OfType<TextBox>().First();
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
