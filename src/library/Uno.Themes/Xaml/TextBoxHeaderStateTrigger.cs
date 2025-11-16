#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

public class TextBoxHeaderStateTrigger : StateTriggerBase
{
	#region DependencyProperty: Text

	public static DependencyProperty TextProperty { get; } = DependencyProperty.Register(
		nameof(Text),
		typeof(string),
		typeof(TextBoxHeaderStateTrigger),
		new PropertyMetadata(string.Empty, OnTextChanged));

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	#endregion
	#region DependencyProperty: Password

	public static DependencyProperty PasswordProperty { get; } = DependencyProperty.Register(
		nameof(Password),
		typeof(string),
		typeof(TextBoxHeaderStateTrigger),
		new PropertyMetadata(string.Empty, OnPasswordChanged));

	public string Password
	{
		get => (string)GetValue(PasswordProperty);
		set => SetValue(PasswordProperty, value);
	}

	#endregion
	#region DependencyProperty: HeaderText

	public static DependencyProperty HeaderTextProperty { get; } = DependencyProperty.Register(
		nameof(HeaderText),
		typeof(string),
		typeof(TextBoxHeaderStateTrigger),
		new PropertyMetadata(string.Empty, OnHeaderTextChanged));

	public string HeaderText
	{
		get => (string)GetValue(HeaderTextProperty);
		set => SetValue(HeaderTextProperty, value);
	}

	#endregion
	#region DependencyProperty: PlaceholderText

	public static DependencyProperty PlaceholderTextProperty { get; } = DependencyProperty.Register(
		nameof(PlaceholderText),
		typeof(string),
		typeof(TextBoxHeaderStateTrigger),
		new PropertyMetadata(string.Empty, OnPlaceholderTextChanged));

	public string PlaceholderText
	{
		get => (string)GetValue(PlaceholderTextProperty);
		set => SetValue(PlaceholderTextProperty, value);
	}

	#endregion
	#region DependencyProperty: FocusState

	public static DependencyProperty FocusStateProperty { get; } = DependencyProperty.Register(
		nameof(FocusState),
		typeof(FocusState),
		typeof(TextBoxHeaderStateTrigger),
		new PropertyMetadata(default(FocusState), OnFocusStateChanged));

	public FocusState FocusState
	{
		get => (FocusState)GetValue(FocusStateProperty);
		set => SetValue(FocusStateProperty, value);
	}

	#endregion

	private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as TextBoxHeaderStateTrigger)?.Reevaluate();
	private static void OnPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as TextBoxHeaderStateTrigger)?.Reevaluate();
	private static void OnHeaderTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as TextBoxHeaderStateTrigger)?.Reevaluate();
	private static void OnPlaceholderTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as TextBoxHeaderStateTrigger)?.Reevaluate();
	private static void OnFocusStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as TextBoxHeaderStateTrigger)?.Reevaluate();

	private void Reevaluate()
	{
		var containsHeader = !string.IsNullOrEmpty(HeaderText);
		var shouldBeDisplaced =
			!string.IsNullOrEmpty(Text) ||
			!string.IsNullOrEmpty(Password) ||
			!string.IsNullOrEmpty(PlaceholderText) ||
			FocusState is not FocusState.Unfocused;

		SetActive(containsHeader && shouldBeDisplaced);
	}
}
