#nullable enable

using System;
using System.Collections.Generic;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Themes.Helpers;

/// <summary>
/// Writes resolved semantic colors into the existing <see cref="SolidColorBrush"/> instances of
/// <c>SharedColors.xaml</c>.
/// </summary>
/// <remarks>
/// <para>
/// The brushes are declared as <c>&lt;SolidColorBrush Color="{StaticResource PrimaryColor}" /&gt;</c>.
/// <c>StaticResource</c> resolves <b>eagerly, at parse time</b>, against the ambient application
/// scope — not against the dictionaries the theme merges alongside it afterwards — so a
/// seed-generated or overridden color never reaches the brush on its own.
/// </para>
/// <para>
/// Replacing the brush dictionary on each rebuild does not help either: consumers bind with
/// <c>{ThemeResource PrimaryBrush}</c>, which resolves to a brush <i>instance</i> and re-evaluates
/// only on a theme change. Anything already rendered would keep the previous brush. Mutating
/// <see cref="SolidColorBrush.Color"/> on the instance the consumers already hold is therefore both
/// the correct fix and the only one that takes effect without re-navigation.
/// </para>
/// </remarks>
internal static class SemanticBrushUpdater
{
	private const string ColorSuffix = "Color";
	private const string BrushSuffix = "Brush";
	private const string OpacitySuffix = "Opacity";

	// The base (state-less) brush declares no Opacity in XAML, so it keeps the default.
	private const double OpaqueOpacity = 1.0;

	// role index -> brush keys for that role, one per interaction state. Precomputed because
	// Apply runs on every theme rebuild — including once per frame while a color picker is
	// dragged — and building ~288 key strings each pass would be pure garbage on WASM.
	private static readonly string[][] _brushKeys = BuildBrushKeys();

	// Opacity token per interaction state, parallel to ThemesConstants.BrushStateSuffixes.
	// null for the state-less base brush.
	private static readonly string[] _opacityKeys = BuildOpacityKeys();

	private static string[][] BuildBrushKeys()
	{
		var colorKeys = ThemesConstants.SemanticColorKeys;
		var states = ThemesConstants.BrushStateSuffixes;
		var keys = new string[colorKeys.Length][];

		for (int i = 0; i < colorKeys.Length; i++)
		{
			var role = colorKeys[i].Substring(0, colorKeys[i].Length - ColorSuffix.Length);
			keys[i] = new string[states.Length];
			for (int j = 0; j < states.Length; j++)
			{
				keys[i][j] = role + states[j] + BrushSuffix;
			}
		}

		return keys;
	}

	private static string[] BuildOpacityKeys()
	{
		var states = ThemesConstants.BrushStateSuffixes;
		var keys = new string[states.Length];

		for (int i = 0; i < states.Length; i++)
		{
			keys[i] = states[i].Length == 0 ? null : states[i] + OpacitySuffix;
		}

		return keys;
	}

	/// <summary>
	/// Resolves every semantic color role from <paramref name="colorLayers"/> and applies it to the
	/// matching brushes in <paramref name="brushes"/>.
	/// </summary>
	/// <param name="brushes">The <c>SharedColors.xaml</c> dictionary. Must be the same instance across rebuilds.</param>
	/// <param name="colorLayers">
	/// The color dictionaries in <b>increasing</b> precedence order (library palette, theme base,
	/// seed palette, consumer override). Later entries win.
	/// </param>
	internal static void Apply(ResourceDictionary brushes, IReadOnlyList<ResourceDictionary> colorLayers)
	{
		var colorKeys = ThemesConstants.SemanticColorKeys;
		var states = ThemesConstants.BrushStateSuffixes;
		Span<double> opacities = stackalloc double[states.Length];

		foreach (var themeKey in ThemesConstants.ThemeDictionaryKeys)
		{
			if (!TryGetThemeDictionary(brushes, themeKey, out var themedBrushes))
			{
				continue;
			}

			for (int j = 0; j < states.Length; j++)
			{
				opacities[j] = _opacityKeys[j] is { } opacityKey
					&& TryResolve<double>(colorLayers, themeKey, opacityKey, out var opacity)
						? opacity
						: OpaqueOpacity;
			}

			for (int i = 0; i < colorKeys.Length; i++)
			{
				if (!TryResolve<Color>(colorLayers, themeKey, colorKeys[i], out var color))
				{
					continue;
				}

				var roleBrushKeys = _brushKeys[i];
				for (int j = 0; j < roleBrushKeys.Length; j++)
				{
					if (!themedBrushes.TryGetValue(roleBrushKeys[j], out var value) || value is not SolidColorBrush brush)
					{
						continue;
					}

					if (!brush.Color.Equals(color))
					{
						brush.Color = color;
					}

					// Opacity is written too, not just Color: the XAML sets it with
					// {StaticResource HoverOpacity}, which — like the Color binding — resolves
					// eagerly against the ambient scope and silently yields 1.0 when the theme is
					// built before it is reachable from Application.Current.Resources.
					if (brush.Opacity != opacities[j])
					{
						brush.Opacity = opacities[j];
					}
				}
			}
		}
	}

	/// <summary>
	/// Walks <paramref name="colorLayers"/> from the highest precedence down and returns the first
	/// definition of <paramref name="key"/> for the requested theme. Theme-specific entries win over
	/// theme-agnostic ones within the same layer; the opacity tokens are declared theme-agnostically.
	/// </summary>
	private static bool TryResolve<T>(
		IReadOnlyList<ResourceDictionary> colorLayers, string themeKey, string key, out T resolved)
	{
		for (int i = colorLayers.Count - 1; i >= 0; i--)
		{
			var layer = colorLayers[i];

			if (TryGetThemeDictionary(layer, themeKey, out var themed)
				&& themed.TryGetValue(key, out var themedValue)
				&& themedValue is T themedResult)
			{
				resolved = themedResult;
				return true;
			}

			if (layer.TryGetValue(key, out var value) && value is T result)
			{
				resolved = result;
				return true;
			}
		}

		resolved = default;
		return false;
	}

	/// <summary>
	/// Gets a theme dictionary by key. Reading it through <c>TryGetValue</c> also materializes
	/// Uno's lazy initializer for XAML-backed dictionaries, which the indexer does not.
	/// </summary>
	private static bool TryGetThemeDictionary(ResourceDictionary dictionary, string themeKey, out ResourceDictionary themed)
	{
		if (dictionary.ThemeDictionaries.TryGetValue(themeKey, out var value) && value is ResourceDictionary result)
		{
			themed = result;
			return true;
		}

		themed = null!;
		return false;
	}
}
