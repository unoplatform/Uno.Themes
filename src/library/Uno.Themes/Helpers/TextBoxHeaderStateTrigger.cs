using Microsoft.UI.Xaml;

namespace Uno.Themes;

internal class TextBoxHeaderStateTrigger : StateTriggerBase
{
	public FocusState FocusState
	{
		get => (FocusState)GetValue(FocusStateProperty);
		set => SetValue(FocusStateProperty, value);
	}

	public static DependencyProperty FocusStateProperty { get; } =
		DependencyProperty.Register(
			nameof(FocusState), typeof(FocusState),
			typeof(TextBoxHeaderStateTrigger),
			new PropertyMetadata(
				defaultValue: default(FocusState),
				propertyChangedCallback: (s, e) => (s as TextBoxHeaderStateTrigger)?.Reevaluate()));

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static DependencyProperty TextProperty { get; } =
		DependencyProperty.Register(
			nameof(Text), typeof(string),
			typeof(TextBoxHeaderStateTrigger),
			new PropertyMetadata(
				defaultValue: string.Empty,
				propertyChangedCallback: (s, e) => (s as TextBoxHeaderStateTrigger)?.Reevaluate()));

	public string HeaderText
	{
		get => (string)GetValue(HeaderTextProperty);
		set => SetValue(HeaderTextProperty, value);
	}

	public static DependencyProperty HeaderTextProperty { get; } =
		DependencyProperty.Register(
			nameof(HeaderText), typeof(string),
			typeof(TextBoxHeaderStateTrigger),
			new PropertyMetadata(
				defaultValue: string.Empty,
				propertyChangedCallback: (s, e) => (s as TextBoxHeaderStateTrigger)?.Reevaluate()));

	public string PlaceholderText
	{
		get => (string)GetValue(PlaceholderTextProperty);
		set => SetValue(PlaceholderTextProperty, value);
	}

	public static DependencyProperty PlaceholderTextProperty { get; } =
		DependencyProperty.Register(
			nameof(PlaceholderText), typeof(string),
			typeof(TextBoxHeaderStateTrigger),
			new PropertyMetadata(
				defaultValue: string.Empty,
				propertyChangedCallback: (s, e) => (s as TextBoxHeaderStateTrigger)?.Reevaluate()));

	private void Reevaluate()
	{
		SetActive(!string.IsNullOrEmpty(HeaderText) && (!string.IsNullOrEmpty(PlaceholderText) || FocusState is not FocusState.Unfocused || !string.IsNullOrEmpty(Text)));
	}
}
