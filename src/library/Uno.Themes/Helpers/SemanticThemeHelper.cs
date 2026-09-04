using System;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Themes;

/// <summary>
/// Provides static helpers for runtime theme configuration: the seed colours and override channels
/// on <see cref="ThemeColors"/>, and the font family on <see cref="BaseTheme"/>.
/// </summary>
public static class SemanticThemeHelper
{
	/// <summary>
	/// Gets the <see cref="BaseTheme"/> instance from <see cref="Application.Current.Resources"/>.
	/// Returns <c>null</c> if no <see cref="BaseTheme"/> is found.
	/// </summary>
	public static BaseTheme GetTheme() => Application.Current.GetTheme();

	/// <summary>
	/// Gets or sets the primary seed color on the active theme.
	/// Setting this regenerates the full color palette at runtime.
	/// </summary>
	/// <exception cref="InvalidOperationException">No <see cref="BaseTheme"/> found in application resources.</exception>
	public static Color? PrimarySeed
	{
		get => GetColorsOrThrow().PrimarySeed;
		set => GetColorsOrThrow().PrimarySeed = value;
	}

	/// <summary>
	/// Gets or sets the secondary seed color on the active theme.
	/// If <c>null</c>, the secondary palette is auto-derived from <see cref="PrimarySeed"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">No <see cref="BaseTheme"/> found in application resources.</exception>
	public static Color? SecondarySeed
	{
		get => GetColorsOrThrow().SecondarySeed;
		set => GetColorsOrThrow().SecondarySeed = value;
	}

	/// <summary>
	/// Gets or sets the tertiary seed color on the active theme.
	/// If <c>null</c>, the tertiary palette is auto-derived from <see cref="PrimarySeed"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">No <see cref="BaseTheme"/> found in application resources.</exception>
	public static Color? TertiarySeed
	{
		get => GetColorsOrThrow().TertiarySeed;
		set => GetColorsOrThrow().TertiarySeed = value;
	}

	/// <summary>
	/// Gets or sets the recipe used to derive the generated palettes from <see cref="PrimarySeed"/>
	/// on the active theme. See <see cref="Uno.Themes.SeedColorMode"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">No <see cref="BaseTheme"/> found in application resources.</exception>
	public static SeedColorMode SeedColorMode
	{
		get => GetColorsOrThrow().SeedColorMode;
		set => GetColorsOrThrow().SeedColorMode = value;
	}

	/// <summary>
	/// Gets or sets the font family the active theme's whole type scale is generated from.
	/// <c>null</c> leaves the theme's own typeface in place. Setting this regenerates the
	/// <c>DefaultFontFamily</c> root token and every derived type-scale family key at runtime; see
	/// <see cref="BaseTheme.DefaultFontFamily"/> for what it does and does not move.
	/// </summary>
	/// <exception cref="InvalidOperationException">No <see cref="BaseTheme"/> found in application resources.</exception>
	public static FontFamily DefaultFontFamily
	{
		get => GetThemeOrThrow().DefaultFontFamily;
		set => GetThemeOrThrow().DefaultFontFamily = value;
	}

	private static BaseTheme GetThemeOrThrow()
		=> GetTheme() ?? throw new InvalidOperationException(
			"No BaseTheme (MaterialTheme, SimpleTheme, etc.) found in Application.Current.Resources.MergedDictionaries.");

	private static ThemeColors GetColorsOrThrow()
	{
		var theme = GetThemeOrThrow();

		if (theme.Colors is null)
		{
			theme.Colors = new ThemeColors();
		}

		return theme.Colors;
	}
}
