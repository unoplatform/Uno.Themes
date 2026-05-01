using System;
using Uno.Themes;


#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
#endif

namespace Uno.Simple;

/// <summary>
/// Simple Theme resources including colors, fonts, layout values, and styles.
/// </summary>
public class SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	: BaseTheme(GetSimpleColorOverride(colorOverride), fontOverride)
{
	/// <summary>
	/// Simple uses a hand-crafted grayscale palette by default (no seed).
	/// When a user explicitly sets <c>Colors.PrimarySeed</c>, high-fidelity
	/// mode preserves the source chroma so low-chroma seeds stay neutral
	/// instead of being boosted by the M3 minimum-chroma floor.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	/// <inheritdoc />
	protected override bool UseHighFidelityColors => true;

	#region DefaultDensity (DP)
	/// <summary>
	/// Gets or sets the density level for the Simple theme.
	/// <c>Compact</c> = tighter spacing (3px base),
	/// <c>Regular</c> = default (4px base),
	/// <c>Comfy</c> = looser spacing (5px base).
	/// This sets the underlying <see cref="BaseTheme.DefaultSpacing"/> automatically.
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
			typeof(SimpleTheme),
			new PropertyMetadata(Density.Regular, OnDefaultDensityChanged));

	private static void OnDefaultDensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is SimpleTheme theme)
		{
			theme.DefaultSpacing = DensityToSpacing((Density)e.NewValue);
		}
	}

	private static double DensityToSpacing(Density density) => density switch
	{
		Density.Compact => 3.0,
		Density.Comfy => 5.0,
		_ => 4.0,
	};
	#endregion

	public SimpleTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

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

		var fonts = new ResourceDictionary { Source = new Uri(SimpleConstants.ResourcePaths.Common.Fonts) };

		if (FontOverrideDictionary is { } fontOverride)
		{
			fonts.SafeMerge(fontOverride);
		}

		mergedPages.MergedDictionaries.Add(fonts);
		return mergedPages;
	}
}
