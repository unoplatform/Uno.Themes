using System;
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
public abstract class BaseTheme : ResourceDictionary
{
	private bool _isColorOverrideMuted;
	private bool _isFontOverrideMuted;
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
		if (d is BaseTheme theme && e.NewValue is string sourceUri)
		{
			theme.ColorOverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
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
		if (d is BaseTheme { _isColorOverrideMuted: false } theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region SeedColor (DP)
	/// <summary>
	/// (Optional) Gets or sets a seed <see cref="Color"/> that algorithmically generates
	/// the full color palette using the HCT (Hue-Chroma-Tone) color space.
	/// When set, all semantic color roles (Primary, Secondary, Tertiary, Error, Surface, etc.)
	/// are derived from this single color for both Light and Dark themes.
	/// Individual colors can still be overridden via <see cref="ColorOverrideDictionary"/>.
	/// </summary>
	public Color? SeedColor
	{
		get => (Color?)GetValue(SeedColorProperty);
		set => SetValue(SeedColorProperty, value);
	}

	public static DependencyProperty SeedColorProperty { get; } =
		DependencyProperty.Register(
			nameof(SeedColor),
			typeof(Color?),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnSeedColorChanged));

	private static void OnSeedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme { _isColorOverrideMuted: false } theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region SecondarySeedColor (DP)
	/// <summary>
	/// (Optional) Gets or sets a seed <see cref="Color"/> for the Secondary color role.
	/// If not set, the Secondary palette is auto-derived from <see cref="SeedColor"/>.
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
			typeof(BaseTheme),
			new PropertyMetadata(null, OnSeedColorChanged));
	#endregion

	#region TertiarySeedColor (DP)
	/// <summary>
	/// (Optional) Gets or sets a seed <see cref="Color"/> for the Tertiary color role.
	/// If not set, the Tertiary palette is auto-derived from <see cref="SeedColor"/>.
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
			typeof(BaseTheme),
			new PropertyMetadata(null, OnSeedColorChanged));
	#endregion

	public BaseTheme() : this(colorOverride: null, fontOverride: null)
	{

	}

	public BaseTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	{
		if (colorOverride is { })
		{
			SetColorOverrideSilently(colorOverride);
		}

		if (fontOverride is { })
		{
			SetFontOverrideSilently(fontOverride);
		}

		UpdateSource();
	}

	private void SetColorOverrideSilently(ResourceDictionary colorOverride)
	{
		try
		{
			_isColorOverrideMuted = true;
			ColorOverrideDictionary = colorOverride;
		}
		finally
		{
			_isColorOverrideMuted = false;
		}
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

	protected void UpdateSource()
	{
#if !HAS_UNO
		Source = null;
#endif
		ThemeDictionaries.Clear();
		MergedDictionaries.Clear();
		this.Clear();

		var converters = new ResourceDictionary { Source = new Uri(ThemesConstants.ConverterResourcePath) };
		var colors = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorsResourcePath) };

		colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorPaletteResourcePath) });

		if (SeedColor is { } seed)
		{
			var seedPalette = SeedColorPaletteGenerator.Generate(seed, SecondarySeedColor, TertiarySeedColor);
			colors.SafeMerge(seedPalette);
		}

		if (ColorOverrideDictionary is { } colorOverride)
		{
			colors.SafeMerge(colorOverride);
		}

		var typography = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedTypographyResourcePath) };

		var mergedPages = GenerateSpecificResources();

		mergedPages.MergedDictionaries.Add(colors);
		mergedPages.MergedDictionaries.Add(converters);

		MergedDictionaries.Add(typography);
		MergedDictionaries.Add(mergedPages);
	}

	protected abstract ResourceDictionary GenerateSpecificResources();
}
