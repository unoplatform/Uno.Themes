using System;
using Uno.Themes;


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
#endif

namespace Uno.Simple;

/// <summary>
/// Controls the default size variant used by Simple Theme control styles.
/// </summary>
public enum SimpleControlSize
{
	/// <summary>Compact sizing (32 px height for buttons).</summary>
	Small,

	/// <summary>Standard sizing (40 px height for buttons).</summary>
	Medium
}

/// <summary>
/// Simple Theme resources including colors, fonts, layout values, and styles.
/// </summary>
public class SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	: BaseTheme(GetSimpleColorOverride(colorOverride), fontOverride)
{
	/// <summary>
	/// Identifies the <see cref="DefaultSize"/> dependency property.
	/// </summary>
	public static readonly DependencyProperty DefaultSizeProperty =
		DependencyProperty.Register(
			nameof(DefaultSize),
			typeof(SimpleControlSize),
			typeof(SimpleTheme),
			new PropertyMetadata(SimpleControlSize.Small, OnDefaultSizeChanged));

	/// <summary>
	/// Gets or sets the default size variant for control styles.
	/// The default is <see cref="SimpleControlSize.Small"/>.
	/// </summary>
	public SimpleControlSize DefaultSize
	{
		get => (SimpleControlSize)GetValue(DefaultSizeProperty);
		set => SetValue(DefaultSizeProperty, value);
	}

	/// <summary>
	/// Default seed for Simple's neutral palette (mid-gray).
	/// The generator enforces minimum chroma, so Primary/Secondary/Tertiary
	/// will carry a slight tint; Neutral and NeutralVariant stay near-gray.
	/// </summary>
	protected override Color? DefaultPrimarySeed { get; } = Color.FromArgb(0xFF, 0x80, 0x80, 0x80);

	public SimpleTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	private static void OnDefaultSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		=> ((SimpleTheme)d).UpdateSource();

	private static ResourceDictionary GetSimpleColorOverride(ResourceDictionary colorOverride)
	{
		// Load the Simple color palette (overrides the default SharedColorPalette values)
		var simpleColors = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.ColorPalette) };

		if (colorOverride is { })
		{
			simpleColors.SafeMerge(colorOverride);
		}

		return simpleColors;
	}

	protected override ResourceDictionary GenerateSpecificResources()
	{
		var mergedPages = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.MergedPages) };

		var thickness = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Thickness) };
		mergedPages.MergedDictionaries.Add(thickness);

		// Load size defaults based on the DefaultSize configuration
		var sizeDefaultsPath = DefaultSize == SimpleControlSize.Medium
			? SimpleConstants.ResourcePaths.Common.SizeMediumDefaults
			: SimpleConstants.ResourcePaths.Common.SizeSmallDefaults;
		var sizeDefaults = new ResourceDictionary { Source = new Uri(sizeDefaultsPath) };
		mergedPages.MergedDictionaries.Add(sizeDefaults);

		var fonts = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Common.Fonts) };

		if (FontOverrideDictionary is { } fontOverride)
		{
			fonts.SafeMerge(fontOverride);
		}

		mergedPages.MergedDictionaries.Add(fonts);
		return mergedPages;
	}
}
