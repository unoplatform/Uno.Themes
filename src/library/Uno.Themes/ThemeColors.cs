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
	private Action<bool> _onChanged;

	#region PrimarySeed (DP)
	/// <summary>
	/// Gets or sets the primary seed <see cref="Color"/> used to algorithmically generate
	/// the full color palette. When set, all semantic color roles are derived from this color.
	/// </summary>
	public Color? PrimarySeed
	{
		get => (Color?)GetValue(PrimarySeedProperty);
		set => SetValue(PrimarySeedProperty, value);
	}

	public static DependencyProperty PrimarySeedProperty { get; } =
		DependencyProperty.Register(
			nameof(PrimarySeed),
			typeof(Color?),
			typeof(ThemeColors),
			new PropertyMetadata(null, OnPropertyChanged));
	#endregion

	#region SecondarySeed (DP)
	/// <summary>
	/// Gets or sets the secondary seed <see cref="Color"/>. If not set, the Secondary
	/// palette is auto-derived from <see cref="PrimarySeed"/>.
	/// </summary>
	public Color? SecondarySeed
	{
		get => (Color?)GetValue(SecondarySeedProperty);
		set => SetValue(SecondarySeedProperty, value);
	}

	public static DependencyProperty SecondarySeedProperty { get; } =
		DependencyProperty.Register(
			nameof(SecondarySeed),
			typeof(Color?),
			typeof(ThemeColors),
			new PropertyMetadata(null, OnPropertyChanged));
	#endregion

	#region TertiarySeed (DP)
	/// <summary>
	/// Gets or sets the tertiary seed <see cref="Color"/>. If not set, the Tertiary
	/// palette is auto-derived from <see cref="PrimarySeed"/>.
	/// </summary>
	public Color? TertiarySeed
	{
		get => (Color?)GetValue(TertiarySeedProperty);
		set => SetValue(TertiarySeedProperty, value);
	}

	public static DependencyProperty TertiarySeedProperty { get; } =
		DependencyProperty.Register(
			nameof(TertiarySeed),
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
		if (d is not ThemeColors tc)
		{
			return;
		}

		if (e.NewValue is string sourceUri && !string.IsNullOrWhiteSpace(sourceUri))
		{
			tc.OverrideDictionary = new ResourceDictionary { Source = new Uri(sourceUri) };
		}
		else
		{
			tc.OverrideDictionary = null;
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
			bool isStructural = e.Property == OverrideDictionaryProperty;
			tc._onChanged?.Invoke(isStructural);
		}
	}

	/// <summary>
	/// Registers a callback that is invoked when any color property changes.
	/// The bool parameter indicates whether this is a structural change (true)
	/// or a seed color change (false).
	/// </summary>
	internal void SetChangedCallback(Action<bool> callback) => _onChanged = callback;
}
