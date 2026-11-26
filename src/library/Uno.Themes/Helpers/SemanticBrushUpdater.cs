#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
/// Creates a theme's semantic brush dictionary — the code-built equivalent of
/// <c>SharedColors.xaml</c> — and writes resolved semantic colors into its existing
/// <see cref="SolidColorBrush"/> instances.
/// </summary>
/// <remarks>
/// <para>
/// <c>SharedColors.xaml</c> declares each brush as
/// <c>&lt;SolidColorBrush Color="{StaticResource PrimaryColor}" /&gt;</c>. <c>StaticResource</c>
/// resolves <b>eagerly, at parse time</b>, against the ambient application scope — not against
/// the dictionaries the theme merges alongside it afterwards — so a seed-generated or overridden
/// color never reaches such a brush on its own. The brushes <see cref="CreateBrushes"/> builds
/// reference nothing at all: every value they hold is written by <see cref="Apply"/>.
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
	private static readonly string?[] _opacityKeys = BuildOpacityKeys();

	private static string[][] BuildBrushKeys()
	{
		var colorKeys = SemanticColorKeys.All;
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

	private static string?[] BuildOpacityKeys()
	{
		var states = ThemesConstants.BrushStateSuffixes;
		var keys = new string?[states.Length];

		for (int i = 0; i < states.Length; i++)
		{
			keys[i] = states[i].Length == 0 ? null : states[i] + OpacitySuffix;
		}

		return keys;
	}

	/// <summary>
	/// Creates the brush dictionary <see cref="Apply"/> writes into: the same keys and theme
	/// dictionaries as <c>SharedColors.xaml</c>, holding blank brushes. Loading that file instead
	/// would resolve two <c>StaticResource</c> references per brush only for <see cref="Apply"/> to
	/// overwrite both values.
	/// </summary>
	/// <returns>A <see cref="SemanticResources"/> dictionary; pass it to <see cref="Apply"/> before use.</returns>
	internal static ResourceDictionary CreateBrushes()
	{
		var colorKeys = SemanticColorKeys.All;
		var brushes = new SemanticResources();

		foreach (var (brushTheme, _) in ThemesConstants.BrushThemeSources)
		{
			// Plain, as the theme dictionaries of a Source-loaded file are (see SemanticResources).
			var themed = new ResourceDictionary();

			for (int i = 0; i < colorKeys.Length; i++)
			{
				if (colorKeys[i] == ThemesConstants.BrushlessColorKey)
				{
					continue;
				}

				var roleBrushKeys = _brushKeys[i];

				// States are ordered base-first (BrushStateSuffixes), so a stateless role keeps only its first key.
				var count = colorKeys[i] == ThemesConstants.StatelessBrushColorKey ? 1 : roleBrushKeys.Length;
				for (int j = 0; j < count; j++)
				{
					themed[roleBrushKeys[j]] = new SolidColorBrush();
				}
			}

			brushes.ThemeDictionaries[brushTheme] = themed;
		}

		return brushes;
	}

	/// <summary>
	/// Resolves every semantic color role from <paramref name="colorLayers"/> and applies it to the
	/// matching brushes in <paramref name="brushes"/>.
	/// </summary>
	/// <param name="brushes">The dictionary from <see cref="CreateBrushes"/>. Must be the same instance across rebuilds.</param>
	/// <param name="colorLayers">
	/// The color dictionaries in <b>increasing</b> precedence order (library palette, theme base,
	/// seed palette, consumer override). Later entries win.
	/// </param>
	internal static void Apply(ResourceDictionary brushes, IReadOnlyList<ResourceDictionary> colorLayers)
	{
		var colorKeys = SemanticColorKeys.All;
		var states = ThemesConstants.BrushStateSuffixes;
		Span<double> opacities = stackalloc double[states.Length];

		foreach (var (brushTheme, colorThemes) in ThemesConstants.BrushThemeSources)
		{
			if (!TryGetThemeDictionary(brushes, brushTheme, out var themedBrushes))
			{
				continue;
			}

			for (int j = 0; j < states.Length; j++)
			{
				opacities[j] = _opacityKeys[j] is { } opacityKey
					&& TryResolve<double>(colorLayers, colorThemes, opacityKey, out var opacity)
						? opacity
						: OpaqueOpacity;
			}

			for (int i = 0; i < colorKeys.Length; i++)
			{
				if (!TryResolve<Color>(colorLayers, colorThemes, colorKeys[i], out var color))
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

	// All theme keys a color layer may declare. Used to detect that a key is theme-scoped in a
	// layer whose dictionaries for the requested theme did not carry it.
	private static readonly string[] _allThemeKeys = { "Light", "Dark", "Default", "HighContrast" };

	/// <summary>
	/// Walks <paramref name="colorLayers"/> from the highest precedence down and returns the first
	/// definition of <paramref name="key"/> for the requested theme. Within a layer the candidate
	/// theme keys are probed in order (mirroring the framework's <c>Dark</c> → <c>Default</c>
	/// fallback). When the layer declares the key only under <em>other</em> theme dictionaries, the
	/// layer is skipped rather than falling through to the plain lookup:
	/// <see cref="ResourceDictionary.TryGetValue"/> resolves themed entries against the
	/// <em>application</em> theme, which would bleed one theme's value into another's brushes.
	/// The plain lookup remains for genuinely theme-agnostic entries (the opacity tokens).
	/// </summary>
	/// <param name="colorLayers">The dictionaries in <b>increasing</b> precedence order.</param>
	/// <param name="themeKeys">The theme dictionary keys to probe in each layer, in order.</param>
	/// <param name="key">The resource key.</param>
	/// <param name="resolved">The resolved value.</param>
	/// <param name="declaredOnly">
	/// <c>true</c> to accept only entries a layer declares itself. Required for a key the platform
	/// also defines (Fluent's accent tokens): <see cref="ResourceDictionary.TryGetValue"/> answers
	/// such a key from the system resources of the ambient theme even when the layer lacks it. Slower
	/// (it enumerates the layer), and it throws where a dictionary cannot be enumerated, so the
	/// semantic roles, which no platform defines, keep the keyed lookup.
	/// </param>
	internal static bool TryResolve<T>(
		IReadOnlyList<ResourceDictionary> colorLayers, string[] themeKeys, string key, [MaybeNullWhen(false)] out T resolved, bool declaredOnly = false)
	{
		for (int i = colorLayers.Count - 1; i >= 0; i--)
		{
			var layer = colorLayers[i];

			if (declaredOnly)
			{
				if (TryResolveDeclared(layer, themeKeys, key, out var declared) && declared is T declaredResult)
				{
					resolved = declaredResult;
					return true;
				}

				continue;
			}

			for (int t = 0; t < themeKeys.Length; t++)
			{
				if (TryGetThemeDictionary(layer, themeKeys[t], out var themed)
					&& themed.TryGetValue(key, out var themedValue)
					&& themedValue is T themedResult)
				{
					resolved = themedResult;
					return true;
				}
			}

			if (IsThemeScoped(layer, key))
			{
				continue;
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
	/// <c>true</c> when <paramref name="key"/> is declared under any of <paramref name="layer"/>'s
	/// theme dictionaries — i.e. the key is theme-scoped in this layer and the theme-agnostic
	/// fallback must not resolve it.
	/// </summary>
	private static bool IsThemeScoped(ResourceDictionary layer, string key)
	{
		for (int t = 0; t < _allThemeKeys.Length; t++)
		{
			if (TryGetThemeDictionary(layer, _allThemeKeys[t], out var themed)
				&& themed.TryGetValue(key, out _))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// The <c>declaredOnly</c> lookup of one dictionary, in the same order as the keyed one: the
	/// requested theme dictionaries, then the dictionary's own entries unless another of its theme
	/// dictionaries declares the key, then its merged dictionaries (last merged first, as the
	/// framework searches them). The keyed lookup gets the merged dictionaries from
	/// <see cref="ResourceDictionary.TryGetValue"/>; here they are walked by hand, which also reads
	/// their theme dictionaries for the requested theme.
	/// </summary>
	private static bool TryResolveDeclared(ResourceDictionary dictionary, string[] themeKeys, string key, out object? value)
	{
		for (int t = 0; t < themeKeys.Length; t++)
		{
			if (TryGetThemeDictionary(dictionary, themeKeys[t], out var themed)
				&& TryResolveDeclared(themed, themeKeys, key, out value))
			{
				return true;
			}
		}

		for (int t = 0; t < _allThemeKeys.Length; t++)
		{
			if (TryGetThemeDictionary(dictionary, _allThemeKeys[t], out var themed)
				&& TryGetOwnEntry(themed, key, out _))
			{
				value = null;
				return false;
			}
		}

		if (TryGetOwnEntry(dictionary, key, out value))
		{
			return true;
		}

		var merged = dictionary.MergedDictionaries;
		for (int i = merged.Count - 1; i >= 0; i--)
		{
			if (TryResolveDeclared(merged[i], themeKeys, key, out value))
			{
				return true;
			}
		}

		value = null;
		return false;
	}

	// Enumerated, not looked up: TryGetValue and ContainsKey both fall back to the system resources.
	private static bool TryGetOwnEntry(ResourceDictionary dictionary, string key, out object? value)
	{
		foreach (var entry in dictionary)
		{
			if (entry.Key is string entryKey && entryKey == key)
			{
				value = entry.Value;
				return true;
			}
		}

		value = null;
		return false;
	}

	/// <summary>
	/// Gets a theme dictionary by key. Reading it through <c>TryGetValue</c> also materializes
	/// Uno's lazy initializer for XAML-backed dictionaries, which the indexer does not.
	/// </summary>
	private static bool TryGetThemeDictionary(ResourceDictionary dictionary, string themeKey, [NotNullWhen(true)] out ResourceDictionary? themed)
	{
		if (dictionary.ThemeDictionaries.TryGetValue(themeKey, out var value) && value is ResourceDictionary result)
		{
			themed = result;
			return true;
		}

		themed = null;
		return false;
	}
}
