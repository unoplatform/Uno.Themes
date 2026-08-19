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
/// Selects the recipe used to derive the generated tonal palettes from
/// <see cref="ThemeColors.PrimarySeed"/>.
/// </summary>
public enum SeedColorMode
{
	/// <summary>
	/// The generated scheme stays true to the seed (default). Every derived palette is scaled
	/// from the seed's own chroma — a muted seed yields a muted scheme, so a gray seed produces
	/// a neutral palette — and the light <c>PrimaryColor</c> is the seed verbatim, with
	/// <c>OnPrimaryColor</c> picked for contrast against it. The dark <c>PrimaryColor</c> is
	/// always derived (tone 80) so it stays legible on a dark surface.
	/// Matches Material Design 3's "content" chroma recipe, plus the exact-seed Primary.
	/// </summary>
	Fidelity = 0,

	/// <summary>
	/// Material Design 3's standard "tonal spot" recipe — the same recipe as before version 8.0,
	/// though its output differs from 7.x because the HCT color solver was also corrected in 8.0.
	/// A minimum chroma of 48 is enforced on Primary and the supporting palettes use fixed
	/// chromas, which guarantees vibrant output but does not reproduce the seed color exactly.
	/// </summary>
	TonalSpot = 1,
}

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

	#region SeedColorMode (DP)
	/// <summary>
	/// Gets or sets the recipe used to derive the generated palettes from <see cref="PrimarySeed"/>.
	/// Default is <see cref="SeedColorMode.Fidelity"/>: the generated scheme stays true to the
	/// seed, and the light <c>PrimaryColor</c> is the seed verbatim. Select
	/// <see cref="SeedColorMode.TonalSpot"/> for Material Design 3's standard vibrant recipe,
	/// which does not reproduce the seed color exactly. See <see cref="Uno.Themes.SeedColorMode"/>
	/// for the full description of each mode.
	/// </summary>
	public SeedColorMode SeedColorMode
	{
		get => (SeedColorMode)GetValue(SeedColorModeProperty);
		set => SetValue(SeedColorModeProperty, value);
	}

	/// <summary>Identifies the <see cref="SeedColorMode"/> dependency property.</summary>
	public static DependencyProperty SeedColorModeProperty { get; } =
		DependencyProperty.Register(
			nameof(SeedColorMode),
			typeof(SeedColorMode),
			typeof(ThemeColors),
			new PropertyMetadata(SeedColorMode.Fidelity, OnPropertyChanged));
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

		// A malformed or unresolvable URI must not throw: this is a property-changed callback, and an
		// exception here takes down the consuming app (on WASM, the runtime). Fall back to no
		// override instead.
		if (e.NewValue is string sourceUri
			&& !string.IsNullOrWhiteSpace(sourceUri)
			&& Uri.TryCreate(sourceUri, UriKind.Absolute, out var source))
		{
			try
			{
				tc.OverrideDictionary = new ResourceDictionary { Source = source };
			}
			catch (Exception)
			{
				// Well-formed but not loadable (missing resource, parse failure — the exception
				// type varies by platform). Same contract as a malformed URI: no override.
				tc.OverrideDictionary = null;
			}
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
	/// <c>true</c> when a consumer explicitly assigned <see cref="SeedColorMode"/>. Lets
	/// <see cref="BaseTheme"/> keep honoring the obsolete <c>UseHighFidelityColors</c> override
	/// of an existing subclass while an explicit assignment here still wins.
	/// </summary>
	internal bool HasExplicitSeedColorMode =>
		ReadLocalValue(SeedColorModeProperty) != DependencyProperty.UnsetValue;

	/// <summary>
	/// Registers a callback that is invoked when any color property changes.
	/// The bool parameter indicates whether this is a structural change (true)
	/// or a seed color change (false).
	/// </summary>
	internal void SetChangedCallback(Action<bool> callback) => _onChanged = callback;
}
