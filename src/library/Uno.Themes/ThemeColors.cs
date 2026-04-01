using System;

#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

/// <summary>
/// Groups all color-related configuration for a theme: seed colors
/// and override dictionaries.
/// Used as the value for <see cref="BaseTheme.Colors"/>.
/// </summary>
public sealed partial class ThemeColors : DependencyObject
{
	private Action _onChanged;

	#region PrimarySeedColor (DP)
	/// <summary>
	/// Gets or sets the primary seed <see cref="Color"/> used to algorithmically generate
	/// the full color palette. When set, all semantic color roles are derived from this color.
	/// </summary>
	public Color? PrimarySeedColor
	{
		get => (Color?)GetValue(PrimarySeedColorProperty);
		set => SetValue(PrimarySeedColorProperty, value);
	}

	public static DependencyProperty PrimarySeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(PrimarySeedColor),
			typeof(Color?),
			typeof(ThemeColors),
			new PropertyMetadata(null, OnPropertyChanged));
	#endregion

	#region SecondarySeedColor (DP)
	/// <summary>
	/// Gets or sets the secondary seed <see cref="Color"/>. If not set, the Secondary
	/// palette is auto-derived from <see cref="PrimarySeedColor"/>.
	/// </summary>
	public Color? SecondarySeedColor
	{
		get => (Color?)GetValue(SecondarySeedColorProperty);
		set => SetValue(SecondarySeedColorProperty, value);
	}

	public static DependencyProperty SecondarySeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(SecondarySeedColor),
			typeof(Color?),
			typeof(ThemeColors),
			new PropertyMetadata(null, OnPropertyChanged));
	#endregion

	#region TertiarySeedColor (DP)
	/// <summary>
	/// Gets or sets the tertiary seed <see cref="Color"/>. If not set, the Tertiary
	/// palette is auto-derived from <see cref="PrimarySeedColor"/>.
	/// </summary>
	public Color? TertiarySeedColor
	{
		get => (Color?)GetValue(TertiarySeedColorProperty);
		set => SetValue(TertiarySeedColorProperty, value);
	}

	public static DependencyProperty TertiarySeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(TertiarySeedColor),
			typeof(Color?),
			typeof(ThemeColors),
			new PropertyMetadata(null, OnPropertyChanged));
	#endregion

	#region OverrideSource (DP)
	/// <summary>
	/// Gets or sets a URI to a <see cref="ResourceDictionary"/> containing color overrides.
	/// </summary>
	public string OverrideSource
	{
		get => (string)GetValue(OverrideSourceProperty);
		set => SetValue(OverrideSourceProperty, value);
	}

	public static DependencyProperty OverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(OverrideSource),
			typeof(string),
			typeof(ThemeColors),
			new PropertyMetadata(null, OnOverrideSourceChanged));

	private static void OnOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ThemeColors tc && e.NewValue is string sourceUri)
		{
			tc.OverrideDictionary = new ResourceDictionary { Source = new Uri(sourceUri) };
		}
	}
	#endregion

	#region OverrideDictionary (DP)
	/// <summary>
	/// Gets or sets a <see cref="ResourceDictionary"/> containing direct color overrides.
	/// These take highest precedence, overriding both defaults and seed-generated colors.
	/// </summary>
	public ResourceDictionary OverrideDictionary
	{
		get => (ResourceDictionary)GetValue(OverrideDictionaryProperty);
		set => SetValue(OverrideDictionaryProperty, value);
	}

	public static DependencyProperty OverrideDictionaryProperty { get; } =
		DependencyProperty.Register(
			nameof(OverrideDictionary),
			typeof(ResourceDictionary),
			typeof(ThemeColors),
			new PropertyMetadata(null, OnPropertyChanged));
	#endregion

	private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is ThemeColors tc)
		{
			tc._onChanged?.Invoke();
		}
	}

	/// <summary>
	/// Registers a callback that is invoked when any color property changes.
	/// </summary>
	internal void SetChangedCallback(Action callback) => _onChanged = callback;
}
