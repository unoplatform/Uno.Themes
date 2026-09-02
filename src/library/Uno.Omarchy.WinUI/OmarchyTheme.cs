#nullable enable

using System;
using System.Collections.Generic;
using Uno.Themes;
using Windows.UI;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Omarchy;

/// <summary>
/// The Omarchy theme: a terminal-inspired design system ported from
/// <see href="https://github.com/aloisdeniel/flutter_omarchy">flutter_omarchy</see>.
/// One monospace face, sharp corners, 2 px borders, translucent tinted fills and an
/// 8-color ANSI palette on top of background / foreground / accent / selection / muted.
/// </summary>
/// <remarks>
/// <para>
/// Colors come from a single <see cref="OmarchyPalette"/> (<see cref="Palette"/>, default
/// <see cref="OmarchyPalettes.TokyoNight"/>). A palette is either light or dark and has no
/// light/dark variants, so the same values are applied to both XAML theme dictionaries; to follow
/// the system theme, switch the palette (for example to <see cref="OmarchyPalettes.CatppuccinLatte"/>).
/// </para>
/// <para>
/// The palette is exposed as <c>Omarchy*Color</c> / <c>Omarchy*Brush</c> resources
/// (<c>OmarchyBackgroundBrush</c>, <c>OmarchyNormalRedBrush</c>, <c>OmarchyBrightBlueBrush</c>, …)
/// and mapped onto the shared semantic roles (<c>PrimaryBrush</c>, <c>SurfaceVariantBrush</c>, …).
/// Changing <see cref="Palette"/> at runtime rewrites the color of every brush in place, so
/// controls already on screen repaint — the same mechanism the seed-color stack uses.
/// </para>
/// </remarks>
public partial class OmarchyTheme : BaseTheme
{
	// The Omarchy tint alpha of a filled surface (button.dart: filled normal state, 0.15).
	// Container roles are the alpha-composited, opaque result of that tint over the background so
	// the shared semantic layer looks like Omarchy's filled widgets without translucent brushes.
	private const double ContainerTintAlpha = 0.15;

	private static readonly (OmarchyAnsiColor Color, string Name)[] _ansiColors =
	{
		(OmarchyAnsiColor.Black, "Black"),
		(OmarchyAnsiColor.White, "White"),
		(OmarchyAnsiColor.Red, "Red"),
		(OmarchyAnsiColor.Green, "Green"),
		(OmarchyAnsiColor.Yellow, "Yellow"),
		(OmarchyAnsiColor.Blue, "Blue"),
		(OmarchyAnsiColor.Magenta, "Magenta"),
		(OmarchyAnsiColor.Cyan, "Cyan"),
	};

	// The palette layer handed to BaseTheme as its base color override. It is one instance for
	// the theme's lifetime and is re-populated in place on palette changes, so it keeps its
	// position in the color-layer precedence (below the seed palette and consumer overrides).
	private readonly ResourceDictionary _paletteLayer;

	// Omarchy brushes are created once and their Color rewritten on every rebuild: consumers hold
	// the instances through {ThemeResource}, and only a mutation repaints what is already rendered.
	private ResourceDictionary? _brushes;

	/// <summary>
	/// Initializes a new Omarchy theme with the default palette (<see cref="OmarchyPalettes.TokyoNight"/>).
	/// </summary>
	public OmarchyTheme() : this(colorOverride: null, fontOverride: null)
	{
	}

	/// <summary>
	/// Initializes a new Omarchy theme.
	/// </summary>
	/// <param name="colorOverride">
	/// Optional consumer color overrides (<c>*Color</c> keys), applied through
	/// <see cref="ThemeColors.OverrideDictionary"/> so they take precedence over the palette.
	/// </param>
	/// <param name="fontOverride">Optional consumer font overrides (<c>*FontFamily</c> keys).</param>
	public OmarchyTheme(ResourceDictionary? colorOverride = null, ResourceDictionary? fontOverride = null)
		: base(CreatePaletteLayer(OmarchyPalettes.TokyoNight, out var paletteLayer), fontOverride)
	{
		_paletteLayer = paletteLayer;

		if (colorOverride is { })
		{
			Colors = new ThemeColors { OverrideDictionary = colorOverride };
		}
	}

	#region Palette (DP)
	/// <summary>
	/// Gets or sets the active palette. Defaults to <see cref="OmarchyPalettes.TokyoNight"/>; setting
	/// <c>null</c> (or an unknown name from XAML) falls back to it.
	/// </summary>
	/// <remarks>
	/// In XAML a stock palette can be named: <c>&lt;OmarchyTheme Palette="Nord" /&gt;</c>
	/// (see <see cref="OmarchyPalettes.FromName"/>). Can be changed at runtime.
	/// </remarks>
	public OmarchyPalette Palette
	{
		get => (OmarchyPalette?)GetValue(PaletteProperty) ?? OmarchyPalettes.TokyoNight;
		set => SetValue(PaletteProperty, value);
	}

	/// <summary>Identifies the <see cref="Palette"/> dependency property.</summary>
	public static DependencyProperty PaletteProperty { get; } =
		DependencyProperty.Register(
			nameof(Palette),
			typeof(OmarchyPalette),
			typeof(OmarchyTheme),
			new PropertyMetadata(OmarchyPalettes.TokyoNight, OnPaletteChanged));

	private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not OmarchyTheme theme)
		{
			return;
		}

		// A property-changed callback must not throw: a failure here would leave the consuming
		// app without a theme. UpdateSource builds every layer before committing, so on failure the
		// previous palette stays in effect.
		try
		{
			theme.ApplyPalette(e.NewValue as OmarchyPalette ?? OmarchyPalettes.TokyoNight);
		}
		catch (Exception)
		{
			// Graceful degradation: keep the last good palette.
		}
	}
	#endregion

	/// <summary>Omarchy has no seed: the palette is hand-picked, so no tonal palette is generated.</summary>
	protected override Color? DefaultPrimarySeed => null;

	protected override string DefaultStylesSource => OmarchyConstants.ResourcePaths.MergedPages;

	private void ApplyPalette(OmarchyPalette palette)
	{
		PopulatePaletteLayer(_paletteLayer, palette);
		UpdateSource();
	}

	protected override void AddThemeSpecificResources()
	{
		base.AddThemeSpecificResources();

		// Palette resolved through the DP: during the base constructor's first UpdateSource the
		// field initializers have not run, but the DP default (TokyoNight) is already available.
		var palette = Palette;
		_brushes ??= CreateBrushes(palette);
		UpdateBrushes(_brushes, palette);
		AddThemeDictionary(_brushes);

		// Fonts and tokens ship in the Source bundle (BaseDictionaries.xaml); only a
		// consumer-supplied font override is layered dynamically on top to shadow them.
		if (FontOverrideDictionary is { } fontOverride)
		{
			AddThemeDictionary(fontOverride);
		}
	}

	#region Palette layer (*Color resources + semantic role mapping)
	private static ResourceDictionary CreatePaletteLayer(OmarchyPalette palette, out ResourceDictionary layer)
	{
		layer = new ResourceDictionary();
		foreach (var themeKey in ThemesConstants.ThemeDictionaryKeys)
		{
			layer.ThemeDictionaries[themeKey] = new ResourceDictionary();
		}

		PopulatePaletteLayer(layer, palette);
		return layer;
	}

	private static void PopulatePaletteLayer(ResourceDictionary layer, OmarchyPalette palette)
	{
		foreach (var themeKey in ThemesConstants.ThemeDictionaryKeys)
		{
			if (layer.ThemeDictionaries.TryGetValue(themeKey, out var value) && value is ResourceDictionary themed)
			{
				WriteColors(themed, palette);
			}
		}
	}

	/// <summary>
	/// Writes the palette's own <c>Omarchy*Color</c> keys and the semantic role mapping described in
	/// <c>specs/08-omarchy-theme/progress.md</c> into <paramref name="target"/>.
	/// </summary>
	private static void WriteColors(ResourceDictionary target, OmarchyPalette palette)
	{
		const string color = OmarchyConstants.ResourceKeys.ColorSuffix;

		target[OmarchyConstants.ResourceKeys.Background + color] = palette.Background;
		target[OmarchyConstants.ResourceKeys.Foreground + color] = palette.Foreground;
		target[OmarchyConstants.ResourceKeys.Accent + color] = palette.Accent;
		target[OmarchyConstants.ResourceKeys.Selection + color] = palette.Selection;
		target[OmarchyConstants.ResourceKeys.Muted + color] = palette.Muted;

		foreach (var (ansi, name) in _ansiColors)
		{
			target[OmarchyConstants.ResourceKeys.NormalPrefix + name + color] = palette.Normal[ansi];
			target[OmarchyConstants.ResourceKeys.BrightPrefix + name + color] = palette.Bright[ansi];
		}

		var background = palette.Background;
		var accent = palette.Accent;
		var normal = palette.Normal;
		var bright = palette.Bright;

		// Primary = the accent (active borders, focus, selected text).
		target["PrimaryColor"] = accent;
		target["OnPrimaryColor"] = background;
		target["PrimaryContainerColor"] = Composite(accent, ContainerTintAlpha, background);
		target["OnPrimaryContainerColor"] = accent;
		target["PrimaryInverseColor"] = accent;
		target["PrimaryVariantDarkColor"] = accent;
		target["PrimaryVariantLightColor"] = accent;

		// Secondary / tertiary: the two ANSI hues the accent (blue by default) is not.
		target["SecondaryColor"] = normal.Magenta;
		target["OnSecondaryColor"] = background;
		target["SecondaryContainerColor"] = Composite(normal.Magenta, ContainerTintAlpha, background);
		target["OnSecondaryContainerColor"] = bright.Magenta;
		target["SecondaryVariantDarkColor"] = normal.Magenta;
		target["SecondaryVariantLightColor"] = bright.Magenta;

		target["TertiaryColor"] = normal.Cyan;
		target["OnTertiaryColor"] = background;
		target["TertiaryContainerColor"] = Composite(normal.Cyan, ContainerTintAlpha, background);
		target["OnTertiaryContainerColor"] = bright.Cyan;

		target["ErrorColor"] = normal.Red;
		target["OnErrorColor"] = background;
		target["ErrorContainerColor"] = Composite(normal.Red, ContainerTintAlpha, background);
		target["OnErrorContainerColor"] = bright.Red;

		target["BackgroundColor"] = background;
		target["OnBackgroundColor"] = palette.Foreground;
		target["SurfaceColor"] = background;
		target["OnSurfaceColor"] = palette.Foreground;
		// lighter_background: the secondary surface of tab bars, status bar and dividers.
		target["SurfaceVariantColor"] = normal.Black;
		target["OnSurfaceVariantColor"] = palette.Muted;
		target["SurfaceInverseColor"] = palette.Foreground;
		target["OnSurfaceInverseColor"] = background;
		target["SurfaceTintColor"] = accent;
		// Input borders are drawn in normal.white (the foreground); subtle borders in lighter_background.
		target["OutlineColor"] = normal.White;
		target["OutlineVariantColor"] = normal.Black;
	}

	/// <summary>Alpha-composites <paramref name="tint"/> at <paramref name="alpha"/> over the opaque <paramref name="background"/>.</summary>
	private static Color Composite(Color tint, double alpha, Color background)
	{
		static byte Mix(byte over, byte under, double a) => (byte)Math.Round(over * a + under * (1 - a));

		return Color.FromArgb(
			255,
			Mix(tint.R, background.R, alpha),
			Mix(tint.G, background.G, alpha),
			Mix(tint.B, background.B, alpha));
	}
	#endregion

	#region Brushes (stable SolidColorBrush instances)
	private static ResourceDictionary CreateBrushes(OmarchyPalette palette)
	{
		var brushes = new ResourceDictionary();
		foreach (var (key, color) in EnumerateBrushColors(palette))
		{
			brushes[key] = new SolidColorBrush(color);
		}

		return brushes;
	}

	private static void UpdateBrushes(ResourceDictionary brushes, OmarchyPalette palette)
	{
		foreach (var (key, color) in EnumerateBrushColors(palette))
		{
			if (brushes.TryGetValue(key, out var value) && value is SolidColorBrush brush && !brush.Color.Equals(color))
			{
				brush.Color = color;
			}
		}
	}

	private static IEnumerable<(string Key, Color Color)> EnumerateBrushColors(OmarchyPalette palette)
	{
		const string brush = OmarchyConstants.ResourceKeys.BrushSuffix;

		yield return (OmarchyConstants.ResourceKeys.Background + brush, palette.Background);
		yield return (OmarchyConstants.ResourceKeys.Foreground + brush, palette.Foreground);
		yield return (OmarchyConstants.ResourceKeys.Accent + brush, palette.Accent);
		yield return (OmarchyConstants.ResourceKeys.Selection + brush, palette.Selection);
		yield return (OmarchyConstants.ResourceKeys.Muted + brush, palette.Muted);

		foreach (var (ansi, name) in _ansiColors)
		{
			yield return (OmarchyConstants.ResourceKeys.NormalPrefix + name + brush, palette.Normal[ansi]);
			yield return (OmarchyConstants.ResourceKeys.BrightPrefix + name + brush, palette.Bright[ansi]);
		}
	}
	#endregion
}
