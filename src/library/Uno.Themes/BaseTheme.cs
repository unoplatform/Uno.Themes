using System;
using System.Collections.Generic;
using Uno.Themes.ColorGeneration;


#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif


namespace Uno.Themes;

/// <summary>
/// Controls the spacing density applied to all controls.
/// Drives the base spacing unit used by all Space* tokens.
/// </summary>
public enum Density
{
	/// <summary>Compact — base spacing unit = 3 px, tighter padding for data-dense UIs.</summary>
	Compact = 3,

	/// <summary>Regular (default) — base spacing unit = 4 px, balanced spacing.</summary>
	Regular = 4,

	/// <summary>Comfortable — base spacing unit = 5 px, more generous padding.</summary>
	Comfy = 5,
}

public abstract partial class BaseTheme : ResourceDictionary
{
	private bool _isUpdatingColorOverrides;
	private bool _isFontOverrideMuted;
	private ResourceDictionary _baseColorOverride;

	// Tracks the dictionaries this theme appends to its own MergedDictionaries during
	// UpdateSource() so a subsequent rebuild (theme-property change or hot reload) removes
	// only those, leaving any other entries in place rather than wiping MergedDictionaries.
	private readonly List<ResourceDictionary> _dynamicDictionaries = new();
	#region FontOverrideSource (DP)
	/// <summary>
	/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
	/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
	/// </summary>
	public string FontOverrideSource
	{
		get => (string)GetValue(FontOverrideSourceProperty);
		set => SetValue(FontOverrideSourceProperty, value);
	}

	public static DependencyProperty FontOverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(FontOverrideSource),
			typeof(string),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnFontOverrideSourceChanged));

	private static void OnFontOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme && e.NewValue is string sourceUri)
		{
			theme.FontOverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
		}
	}
	#endregion

	#region ColorOverrideSource (DP)
	/// <summary>
	/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
	/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
	[Obsolete("Use Colors.OverrideSource on ThemeColors instead. This property will be removed in a future version.")]
	public string ColorOverrideSource
	{
		get => (string)GetValue(ColorOverrideSourceProperty);
		set => SetValue(ColorOverrideSourceProperty, value);
	}

	public static DependencyProperty ColorOverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(ColorOverrideSource),
			typeof(string),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorOverrideSourceChanged));

	private static void OnColorOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			try
			{
				theme._isUpdatingColorOverrides = true;
				if (e.NewValue is string sourceUri)
				{
					var tc = theme.EnsureColors();
					tc.OverrideSource = sourceUri;
				}
				else if (theme.Colors is { } tc)
				{
					tc.OverrideDictionary = null;
				}

				theme.UpdateSource();
			}
			finally
			{
				theme._isUpdatingColorOverrides = false;
			}
		}
	}
	#endregion

	#region FontOverrideDictionary (DP)
	/// <summary>
	/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
	/// </summary>
	public ResourceDictionary FontOverrideDictionary
	{
		get => (ResourceDictionary)GetValue(FontOverrideDictionaryProperty);
		set => SetValue(FontOverrideDictionaryProperty, value);
	}

	public static DependencyProperty FontOverrideDictionaryProperty { get; } =
		DependencyProperty.Register(
			nameof(FontOverrideDictionary),
			typeof(ResourceDictionary),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnFontOverrideChanged));

	private static void OnFontOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme { _isFontOverrideMuted: false } theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region ColorOverrideDictionary (DP)
	/// <summary>
	/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
	[Obsolete("Use Colors.OverrideDictionary on ThemeColors instead. This property will be removed in a future version.")]
	public ResourceDictionary ColorOverrideDictionary
	{
		get => (ResourceDictionary)GetValue(ColorOverrideDictionaryProperty);
		set => SetValue(ColorOverrideDictionaryProperty, value);
	}

	public static DependencyProperty ColorOverrideDictionaryProperty { get; } =
		DependencyProperty.Register(
			nameof(ColorOverrideDictionary),
			typeof(ResourceDictionary),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorOverrideChanged));

	private static void OnColorOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme { _isUpdatingColorOverrides: false } theme)
		{
			try
			{
				theme._isUpdatingColorOverrides = true;
				if (e.NewValue is ResourceDictionary dict)
				{
					var tc = theme.EnsureColors();
					tc.OverrideDictionary = dict;
				}
				else
				{
					if (theme.Colors is { } tc)
					{
						tc.OverrideDictionary = null;
					}
				}

				theme.UpdateSource();
			}
			finally
			{
				theme._isUpdatingColorOverrides = false;
			}
		}
	}
	#endregion

	#region Colors (DP)
	/// <summary>
	/// Gets or sets a <see cref="ThemeColors"/> object that groups all color-related configuration
	/// including seed colors, overrides, and the palette generation algorithm.
	/// This is the recommended way to configure theme colors.
	/// </summary>
	public ThemeColors Colors
	{
		get => (ThemeColors)GetValue(ColorsProperty);
		set => SetValue(ColorsProperty, value);
	}

	public static DependencyProperty ColorsProperty { get; } =
		DependencyProperty.Register(
			nameof(Colors),
			typeof(ThemeColors),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorsChanged));

	private static void OnColorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			if (e.OldValue is ThemeColors old)
			{
				old.SetChangedCallback(null);
			}

			if (e.NewValue is ThemeColors tc)
			{
				tc.SetChangedCallback((_) =>
				{
					if (theme._isUpdatingColorOverrides)
					{
						return;
					}

					theme.UpdateSource();
				});
			}

			if (!theme._isUpdatingColorOverrides)
			{
				theme.UpdateSource();
			}
		}
	}
	#endregion

	#region DefaultCornerRadius (DP)
	/// <summary>
	/// Gets or sets the base corner radius unit (in pixels). Default is 4.
	/// All shape scale tokens (Radius100, Radius200, …) are computed
	/// as multiples of this value — e.g. <c>DefaultCornerRadius="6"</c> makes
	/// Radius100=6, Radius200=12, Radius400=24, etc.
	/// <c>RadiusFull</c> always remains 9999 (pill shape).
	/// Individual tokens can still be overridden via lightweight styling.
	/// </summary>
	public double DefaultCornerRadius
	{
		get => (double)GetValue(DefaultCornerRadiusProperty);
		set => SetValue(DefaultCornerRadiusProperty, value);
	}

	public static DependencyProperty DefaultCornerRadiusProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultCornerRadius),
			typeof(double),
			typeof(BaseTheme),
			new PropertyMetadata(4.0, OnDefaultCornerRadiusChanged));

	private static void OnDefaultCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region DefaultDensity (DP)
	/// <summary>
	/// Gets or sets the density preset for the theme. Default is <see cref="Density.Regular"/>.
	/// This drives the base spacing unit used by all Space* tokens:
	/// Compact = 3, Regular = 4, Comfy = 5.
	/// Control heights and icon sizes remain constant across densities.
	/// </summary>
	public Density DefaultDensity
	{
		get => (Density)GetValue(DefaultDensityProperty);
		set => SetValue(DefaultDensityProperty, value);
	}

	public static DependencyProperty DefaultDensityProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultDensity),
			typeof(Density),
			typeof(BaseTheme),
			new PropertyMetadata(Density.Regular, OnDefaultDensityChanged));

	private static void OnDefaultDensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	/// <summary>
	/// Gets the default primary seed color for this theme.
	/// When not <c>null</c>, seed color generation is always active — even
	/// without an explicit <see cref="ThemeColors.PrimarySeed"/> — so the
	/// theme uses algorithmically-derived colors by default.
	/// Override in subclasses to provide a theme-specific default.
	/// </summary>
	protected virtual Color? DefaultPrimarySeed => null;

	/// <summary>
	/// When <c>true</c>, seed color generation preserves the source color's
	/// actual chroma (high-fidelity / "color match" mode). When <c>false</c>
	/// (default), the standard M3 minimum chroma of 48 is enforced, which
	/// guarantees vibrant colors but distorts low-chroma seeds like gray.
	/// </summary>
	protected virtual bool UseHighFidelityColors => false;

	public BaseTheme() : this(colorOverride: null, fontOverride: null)
	{

	}

	public BaseTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	{
		// Opt this instance into hot-reload tracking so BaseTheme.UpdateApplication can
		// rebuild it when a XAML resource edit is applied (see BaseTheme.HotReload.cs).
		RegisterInstance(this);

		if (colorOverride is { })
		{
			_baseColorOverride = colorOverride;
		}

		if (fontOverride is { })
		{
			SetFontOverrideSilently(fontOverride);
		}

		// Set once: populates this dictionary with the concrete theme's static base layer
		// (control styles) from the URI it provides. UpdateSource() appends the dynamic
		// layers (colors/typography/spacing/…) on top and rebuilds them on every pass.
		Source = new Uri(DefaultStylesSource);
		UpdateSource();
	}

	private void SetFontOverrideSilently(ResourceDictionary fontOverride)
	{
		try
		{
			_isFontOverrideMuted = true;
			FontOverrideDictionary = fontOverride;
		}
		finally
		{
			_isFontOverrideMuted = false;
		}
	}

	private ThemeColors EnsureColors()
	{
		var colors = Colors;
		if (colors is null)
		{
			colors = new ThemeColors();
			Colors = colors;
		}
		return colors;
	}

	protected void UpdateSource()
	{
		// Remove only the dictionaries this theme appended on a previous pass. The URI-backed
		// base layer populated when Source was set (the concrete theme's control styles) stays
		// in place so it is not rebuilt on every theme-property change or hot reload.
		foreach (var dictionary in _dynamicDictionaries)
		{
			MergedDictionaries.Remove(dictionary);
		}
		_dynamicDictionaries.Clear();

		// Colour layer (always dynamic so it rebuilds on seed-color / override changes):
		// SharedColors brushes resolve their {StaticResource *Color} against the shared
		// palette, the theme's own base palette (e.g. SimpleTheme's grayscale, supplied via
		// _baseColorOverride), the seed palette and finally the consumer override — each
		// merged in increasing precedence. This layer is intentionally NOT baked into the
		// Source bundle (BaseDictionaries.xaml) so colour edits never require reloading the
		// static base.
		var colors = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorsResourcePath) };

		colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorPaletteResourcePath) });

		// Theme-specific base colors (e.g. SimpleTheme's grayscale palette) are merged
		// before the seed so that seed-generated colors take precedence.
		if (_baseColorOverride is { } baseColorOverride)
		{
			colors.SafeMerge(baseColorOverride);
		}

		// Resolve seed colors from Colors property, falling back to theme default
		var effectivePrimary = Colors?.PrimarySeed ?? DefaultPrimarySeed;
		var effectiveSecondary = Colors?.SecondarySeed;
		var effectiveTertiary = Colors?.TertiarySeed;

		if (effectivePrimary is { } seed)
		{
			var seedPalette = SeedColorPaletteGenerator.Default.Generate(seed, effectiveSecondary, effectiveTertiary, UseHighFidelityColors);
			colors.SafeMerge(seedPalette);
		}

		// Explicit user overrides from Colors.OverrideDictionary take highest precedence.
		// URI-backed overrides are re-resolved from their Source on each rebuild so that
		// hot-reload edits to the underlying XAML propagate: the in-memory key/value pairs of
		// the original instance were loaded at init time and would otherwise be stale.
		if (Colors?.OverrideDictionary is { } userOverride)
		{
			colors.SafeMerge(userOverride.Source is { } overrideSource
				? new ResourceDictionary { Source = overrideSource }
				: userOverride);
		}

		// Generate spacing, shape, and density scales from code
		var baseSpacing = Enum.IsDefined(DefaultDensity) ? (double)DefaultDensity : 4.0;
		var spacing = GenerateSpacingScale(baseSpacing);
		var shape = GenerateShapeScale(DefaultCornerRadius);
		var density = GenerateDensityDefaults();

		AddThemeDictionary(spacing);
		AddThemeDictionary(shape);
		AddThemeDictionary(density);
		AddThemeDictionary(colors);

		// Let the concrete theme append its own dynamic dictionaries (e.g. a consumer font
		// override) on top of the generated layers. The static converter/typography/font/
		// thickness defaults live in the Source bundle (BaseDictionaries.xaml), below the
		// theme's own overrides; only resources that must shadow that base belong here.
		AddThemeSpecificResources();
	}

	/// <summary>
	/// Adds <paramref name="dictionary"/> to this theme's <see cref="ResourceDictionary.MergedDictionaries"/>
	/// and tracks it so the next <see cref="UpdateSource"/> removes it. Use this instead of adding to
	/// <c>MergedDictionaries</c> directly so the entry participates in the rebuild lifecycle.
	/// </summary>
	protected void AddThemeDictionary(ResourceDictionary dictionary)
	{
		MergedDictionaries.Add(dictionary);
		_dynamicDictionaries.Add(dictionary);
	}

	/// <summary>
	/// URI of the concrete theme's static base layer (its merged control-style pages).
	/// Set as this dictionary's <see cref="ResourceDictionary.Source"/> once during construction;
	/// the dynamic layers generated by <see cref="UpdateSource"/> are appended on top.
	/// </summary>
	protected abstract string DefaultStylesSource { get; }

	/// <summary>
	/// Appends design-system-specific resource dictionaries (fonts, thickness, …) on top of the
	/// layers generated by <see cref="UpdateSource"/>. Called at the end of every rebuild pass.
	/// Override in a concrete theme and call <see cref="AddThemeDictionary(ResourceDictionary)"/>
	/// for each dictionary so it participates in the dynamic rebuild lifecycle.
	/// </summary>
	protected virtual void AddThemeSpecificResources()
	{
	}
}
