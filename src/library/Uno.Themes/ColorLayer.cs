#nullable enable

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

/// <summary>What a <see cref="ColorLayer"/> holds, which decides how a design-system adapter reads it.</summary>
internal enum ColorLayerKind : byte
{
	/// <summary>The shared semantic palette every theme starts from.</summary>
	SharedPalette,

	/// <summary>The design system's own base palette (<see cref="BaseTheme.ThemePalette"/>).</summary>
	ThemePalette,

	/// <summary>The constructor color override: consumer values, below a seed.</summary>
	ConstructorOverride,

	/// <summary>The palette generated from <see cref="BaseTheme.EffectivePrimarySeed"/>.</summary>
	Seed,

	/// <summary>The <see cref="ThemeColors"/> override: consumer values, above everything else.</summary>
	ConsumerOverride,
}

/// <summary>
/// One dictionary of a theme's color layer, as resolved by the last rebuild pass. The layers of
/// <see cref="BaseTheme.ColorLayers"/> are in increasing precedence, so the order is defined once,
/// by the base theme, for the semantic brushes and for any adapter deriving resources from it.
/// </summary>
internal readonly record struct ColorLayer(ResourceDictionary Dictionary, ColorLayerKind Kind);
