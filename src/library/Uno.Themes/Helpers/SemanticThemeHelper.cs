using System;
using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

/// <summary>
/// Provides static helpers for runtime theme configuration via <see cref="ThemeColors"/>.
/// </summary>
public static class SemanticThemeHelper
{
	/// <summary>
	/// Gets the <see cref="BaseTheme"/> instance from <see cref="Application.Current.Resources"/>.
	/// Returns <c>null</c> if no <see cref="BaseTheme"/> is found.
	/// </summary>
	public static BaseTheme GetTheme() =>
		Application.Current?.Resources?.MergedDictionaries.OfType<BaseTheme>().FirstOrDefault();

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

	private static ThemeColors GetColorsOrThrow()
	{
		var theme = GetTheme() ?? throw new InvalidOperationException(
			"No BaseTheme (MaterialTheme, SimpleTheme, etc.) found in Application.Current.Resources.MergedDictionaries.");

		if (theme.Colors is null)
		{
			theme.Colors = new ThemeColors();
		}

		return theme.Colors;
	}
}
