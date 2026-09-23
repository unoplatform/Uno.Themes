using System;
using Uno.Themes;
using Uno.Themes.Helpers;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Cupertino;

/// <summary>
/// Cupertino Theme resources including colors, fonts, layout values, and styles.
/// </summary>
/// <param name="colorOverride">(Optional) A dictionary redefining Cupertino or shared <c>*Color</c> resources.</param>
/// <param name="fontOverride">(Optional) A dictionary redefining font resources, typically <c>DefaultFontFamily</c>.</param>
public class CupertinoTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	: BaseTheme(GetCupertinoColorOverride(colorOverride), fontOverride)
{
	/// <summary>
	/// Cupertino uses Apple's hand-crafted system palette by default (no seed).
	/// When a user explicitly sets <c>Colors.PrimarySeed</c>, the generated palette
	/// replaces it, following the M3 container recipe rather than Apple's tinted fills.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoTheme"/> class with no overrides.
	/// </summary>
	public CupertinoTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	#region GlassRenderingMode (DP)
	/// <summary>
	/// Gets or sets how every <see cref="GlassPanel"/> under this theme is drawn. Default is
	/// <see cref="Uno.Cupertino.GlassRenderingMode.Auto"/>. Set <see cref="Uno.Cupertino.GlassRenderingMode.Solid"/>
	/// to honor a Reduce Transparency preference: it wins over a panel's own <see cref="GlassPanel.RenderingMode"/>.
	/// </summary>
	/// <remarks>
	/// This is a <b>construction-time</b> setting: a panel reads it when it loads, so a later change reaches
	/// the panels loaded afterwards.
	/// </remarks>
	public GlassRenderingMode GlassRenderingMode
	{
		get => (GlassRenderingMode)GetValue(GlassRenderingModeProperty);
		set => SetValue(GlassRenderingModeProperty, value);
	}

	/// <summary>Identifies the <see cref="GlassRenderingMode"/> dependency property.</summary>
	public static DependencyProperty GlassRenderingModeProperty { get; } =
		DependencyProperty.Register(
			nameof(GlassRenderingMode),
			typeof(GlassRenderingMode),
			typeof(CupertinoTheme),
			new PropertyMetadata(GlassRenderingMode.Auto, (d, _) => (d as CupertinoTheme)?.UpdateSource()));
	#endregion

	private static ResourceDictionary GetCupertinoColorOverride(ResourceDictionary colorOverride)
	{
		// Load the Cupertino color palette (overrides the default SharedColorPalette values)
		var cupertinoColors = new ResourceDictionary { Source = new Uri(CupertinoConstants.ColorPalette) };

		if (colorOverride is { })
		{
			cupertinoColors.SafeMerge(colorOverride);
		}

		return cupertinoColors;
	}

	/// <inheritdoc />
	protected override string DefaultStylesSource => CupertinoConstants.MergedPages;

	// Created once and kept for the lifetime of the theme: consumers hold {ThemeResource Cupertino*Brush}
	// references to these instances, so they are rewritten in place, never replaced (see SemanticBrushUpdater).
	private ResourceDictionary _cupertinoBrushes;

	/// <inheritdoc />
	protected override void AddThemeSpecificResources()
	{
		_cupertinoBrushes ??= new ResourceDictionary { Source = new Uri(CupertinoConstants.Brushes) };
		SemanticBrushUpdater.Apply(_cupertinoBrushes, ColorLayers, CupertinoConstants.BrushColorKeys);
		AddThemeDictionary(_cupertinoBrushes);
		AddThemeDictionary(BuildAccentColors(_cupertinoBrushes));
		AddThemeDictionary(BuildSpacingResources());
		AddThemeDictionary(new ResourceDictionary { [CupertinoConstants.GlassRenderingModeKey] = GlassRenderingMode.ToString() });
	}

	// Asymmetric iOS paddings cannot be represented by a uniform shared Thickness token.
	// Regenerate the existing lightweight keys along with the shared spacing scale.
	private ResourceDictionary BuildSpacingResources()
	{
		// BaseTheme has already appended the validated, density-adjusted spacing scale.
		var spacing = TryGetValue(CupertinoConstants.SpacingBaseKey, out var value) && value is double unit ? unit : 4;
		var resources = new ResourceDictionary();
		foreach (var appearance in CupertinoConstants.AccentThemeKeys)
		{
			// Cupertino preserves the iOS 44-point hit target independently of spacing density.
			var themed = new ResourceDictionary { [CupertinoConstants.TouchTargetMinSizeKey] = 44d };
			foreach (var (key, left, top, right, bottom) in CupertinoConstants.SpacingMultipliers)
			{
				themed[key] = new Thickness(left * spacing, top * spacing, right * spacing, bottom * spacing);
			}
			resources.ThemeDictionaries[appearance] = themed;
		}
		return resources;
	}

	// The accent brushes follow the semantic primary (a seed, or a PrimaryColor override) while their *Color
	// keys are plain palette entries. Consumers read both (uno.toolkit.ui's Cupertino styles do), so the
	// colors are re-emitted from the painted brushes to keep the accent from splitting in two.
	private static ResourceDictionary BuildAccentColors(ResourceDictionary brushes)
	{
		var accents = new ResourceDictionary();
		foreach (var themeKey in CupertinoConstants.AccentThemeKeys)
		{
			if (!brushes.ThemeDictionaries.TryGetValue(themeKey, out var block) || block is not ResourceDictionary themedBrushes)
			{
				continue;
			}

			var themedColors = new ResourceDictionary();
			foreach (var (brushKey, colorKey) in CupertinoConstants.AccentColorKeys)
			{
				if (themedBrushes.TryGetValue(brushKey, out var value) && value is SolidColorBrush brush)
				{
					themedColors[colorKey] = brush.Color;
				}
			}

			accents.ThemeDictionaries[themeKey] = themedColors;
		}

		return accents;
	}

	/// <summary>
	/// The font family alias keys Cupertino declares — StaticResource aliases that snapshot at parse
	/// time and therefore need regenerating for a runtime <see cref="BaseTheme.DefaultFontFamily"/>
	/// change to reach the control templates. Only keys declared inside <c>ThemeDictionaries</c> can be
	/// listed: a generated layer is a merged dictionary and cannot shadow a top-level key.
	/// </summary>
	internal override string[] FontFamilyAliasKeys { get; } =
	{
		"CupertinoFontFamily",
	};
}
