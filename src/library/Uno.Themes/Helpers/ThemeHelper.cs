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
/// Provides static helpers for runtime theme configuration.
/// </summary>
public static class ThemeHelper
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
	public static Color? PrimarySeedColor
	{
		get => GetThemeOrThrow().PrimarySeedColor;
		set => GetThemeOrThrow().PrimarySeedColor = value;
	}

	/// <summary>
	/// Gets or sets the secondary seed color on the active theme.
	/// If <c>null</c>, the secondary palette is auto-derived from <see cref="PrimarySeedColor"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">No <see cref="BaseTheme"/> found in application resources.</exception>
	public static Color? SecondarySeedColor
	{
		get => GetThemeOrThrow().SecondarySeedColor;
		set => GetThemeOrThrow().SecondarySeedColor = value;
	}

	/// <summary>
	/// Gets or sets the tertiary seed color on the active theme.
	/// If <c>null</c>, the tertiary palette is auto-derived from <see cref="PrimarySeedColor"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">No <see cref="BaseTheme"/> found in application resources.</exception>
	public static Color? TertiarySeedColor
	{
		get => GetThemeOrThrow().TertiarySeedColor;
		set => GetThemeOrThrow().TertiarySeedColor = value;
	}

	private static BaseTheme GetThemeOrThrow() =>
		GetTheme() ?? throw new InvalidOperationException(
			"No BaseTheme (MaterialTheme, SimpleTheme, etc.) found in Application.Current.Resources.MergedDictionaries.");
}
