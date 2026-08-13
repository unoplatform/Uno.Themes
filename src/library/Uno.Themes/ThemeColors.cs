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

	#region PreserveSeedColor (DP)
	/// <summary>
	/// Gets or sets whether generated palettes stay faithful to <see cref="PrimarySeed"/>.
	/// Default is <c>true</c>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When <c>true</c>, the light <c>PrimaryColor</c> resource is the seed color verbatim and
	/// every derived palette is scaled from the seed's own chroma, so a low-chroma seed such as
	/// gray produces a neutral palette. The light <c>OnPrimaryColor</c> is picked for contrast
	/// against the pinned seed rather than being fixed at tone 100.
	/// </para>
	/// <para>
	/// When <c>false</c>, the Material Design 3 "tonal spot" behavior applies instead: a minimum
	/// chroma of 48 is enforced on Primary and the supporting palettes use fixed chromas. This
	/// guarantees vibrant output but does not reproduce the seed color exactly. This was the
	/// behavior before version 8.0.
	/// </para>
	/// <para>The dark <c>PrimaryColor</c> is always derived (tone 80) so it stays legible on a dark surface.</para>
	/// </remarks>
	public bool PreserveSeedColor
	{
		get => (bool)GetValue(PreserveSeedColorProperty);
		set => SetValue(PreserveSeedColorProperty, value);
	}

	/// <summary>Identifies the <see cref="PreserveSeedColor"/> dependency property.</summary>
	public static DependencyProperty PreserveSeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(PreserveSeedColor),
			typeof(bool),
			typeof(ThemeColors),
			new PropertyMetadata(true, OnPropertyChanged));
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
	/// <c>true</c> when a consumer explicitly assigned <see cref="PreserveSeedColor"/>. Lets
	/// <see cref="BaseTheme"/> keep honoring the obsolete <c>UseHighFidelityColors</c> override
	/// of an existing subclass while an explicit assignment here still wins.
	/// </summary>
	internal bool HasExplicitPreserveSeedColor =>
		ReadLocalValue(PreserveSeedColorProperty) != DependencyProperty.UnsetValue;

	/// <summary>
	/// Registers a callback that is invoked when any color property changes.
	/// The bool parameter indicates whether this is a structural change (true)
	/// or a seed color change (false).
	/// </summary>
	internal void SetChangedCallback(Action<bool> callback) => _onChanged = callback;
}
