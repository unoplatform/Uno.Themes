#nullable enable

using System;
using Windows.Foundation.Metadata;
using Windows.UI;

namespace Uno.Omarchy;

/// <summary>
/// The eight base terminal colors of an Omarchy palette. Each exists in a <em>normal</em> and a
/// <em>bright</em> variant (<see cref="OmarchyPalette.Normal"/> / <see cref="OmarchyPalette.Bright"/>).
/// </summary>
public enum OmarchyAnsiColor : byte
{
	/// <summary>Normal: the theme's <c>lighter_background</c> (secondary surfaces, borders); bright: the muted color.</summary>
	Black,
	/// <summary>Normal: the foreground; bright: the <c>bright_foreground</c>.</summary>
	White,
	Red,
	Green,
	Blue,
	Yellow,
	Magenta,
	Cyan,
}

/// <summary>
/// One ANSI color set (normal or bright) of an <see cref="OmarchyPalette"/>.
/// </summary>
public readonly record struct OmarchyAnsiPalette(
	Color Black,
	Color White,
	Color Red,
	Color Green,
	Color Yellow,
	Color Blue,
	Color Magenta,
	Color Cyan)
{
	/// <summary>Gets the color for <paramref name="color"/>.</summary>
	public Color this[OmarchyAnsiColor color] => color switch
	{
		OmarchyAnsiColor.Black => Black,
		OmarchyAnsiColor.White => White,
		OmarchyAnsiColor.Red => Red,
		OmarchyAnsiColor.Green => Green,
		OmarchyAnsiColor.Yellow => Yellow,
		OmarchyAnsiColor.Blue => Blue,
		OmarchyAnsiColor.Magenta => Magenta,
		OmarchyAnsiColor.Cyan => Cyan,
		_ => throw new ArgumentOutOfRangeException(nameof(color), color, "Unknown ANSI color."),
	};
}

/// <summary>
/// An Omarchy color palette: the values of a theme's <c>colors.toml</c>, in the shape
/// <c>flutter_omarchy</c> exposes them (<c>OmarchyColorThemeData</c>).
/// </summary>
/// <remarks>
/// A palette is either light or dark (<see cref="IsLight"/>); it has no light/dark variants. The
/// theme therefore applies the same values to both XAML theme dictionaries — an app that wants
/// to follow the system theme switches <see cref="OmarchyTheme.Palette"/> instead.
/// In XAML, a palette can be written by name (<c>Palette="Nord"</c>), resolved through
/// <see cref="OmarchyPalettes.FromName"/>.
/// </remarks>
/// <param name="Name">The display name (e.g. <c>Tokyo Night</c>).</param>
/// <param name="IsLight">Whether this is a light palette (<c>mode = "light"</c>).</param>
/// <param name="Background">The primary background of surfaces.</param>
/// <param name="Foreground">The primary text and icon color.</param>
/// <param name="Accent">Active borders, focus, selected text and highlights.</param>
/// <param name="Selection">The text-selection background.</param>
/// <param name="Muted">De-emphasized text and icons.</param>
/// <param name="Normal">The normal ANSI colors.</param>
/// <param name="Bright">The bright ANSI colors.</param>
[CreateFromString(MethodName = "Uno.Omarchy.OmarchyPalettes.FromName")]
public sealed record OmarchyPalette(
	string Name,
	bool IsLight,
	Color Background,
	Color Foreground,
	Color Accent,
	Color Selection,
	Color Muted,
	OmarchyAnsiPalette Normal,
	OmarchyAnsiPalette Bright);
