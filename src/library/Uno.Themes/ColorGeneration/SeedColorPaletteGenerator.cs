// Based on the dynamic color scheme generation from material-color-utilities
// by Google under the Apache License 2.0.
// Original source: https://github.com/material-foundation/material-color-utilities
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using Uno.Themes.ColorGeneration.Hct;

#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
#endif

namespace Uno.Themes.ColorGeneration;

/// <summary>
/// Generates a complete semantic color palette (Light + Dark) from seed color(s),
/// following the Material Design 3 tonal palette system.
/// </summary>
internal sealed class SeedColorPaletteGenerator
{
	/// <summary>
	/// Shared default instance.
	/// </summary>
	internal static SeedColorPaletteGenerator Default { get; } = new();

	private readonly Dictionary<(int primary, int? secondary, int? tertiary), ResourceDictionary> _cache = new();

	internal ResourceDictionary Generate(
		Color primarySeedColor,
		Color? secondarySeedColor = null,
		Color? tertiarySeedColor = null)
	{
		var cacheKey = (
			ColorToArgb(primarySeedColor),
			secondarySeedColor is { } sec ? (int?)ColorToArgb(sec) : null,
			tertiarySeedColor is { } ter ? (int?)ColorToArgb(ter) : null);

		if (_cache.TryGetValue(cacheKey, out var cached))
		{
			return cached;
		}

		var result = GenerateCore(primarySeedColor, secondarySeedColor, tertiarySeedColor);
		_cache[cacheKey] = result;
		return result;
	}

	internal void ClearCache() => _cache.Clear();

	private static ResourceDictionary GenerateCore(
		Color seedColor,
		Color? secondarySeedColor,
		Color? tertiarySeedColor)
	{
		var seedHct = HctColor.FromArgb(ColorToArgb(seedColor));
		double hue = seedHct.Hue;
		double chroma = seedHct.Chroma;

		// Derive the six tonal palettes
		var primary = new TonalPalette(hue, Math.Max(chroma, 48));

		TonalPalette secondary;
		if (secondarySeedColor is { } secSeed)
		{
			var secHct = HctColor.FromArgb(ColorToArgb(secSeed));
			secondary = new TonalPalette(secHct.Hue, secHct.Chroma);
		}
		else
		{
			secondary = new TonalPalette(hue, chroma / 3.0);
		}

		TonalPalette tertiary;
		if (tertiarySeedColor is { } terSeed)
		{
			var terHct = HctColor.FromArgb(ColorToArgb(terSeed));
			tertiary = new TonalPalette(terHct.Hue, terHct.Chroma);
		}
		else
		{
			tertiary = new TonalPalette(hue + 60.0, chroma / 2.0);
		}

		var neutral = new TonalPalette(hue, Math.Min(chroma / 12.0, 4.0));
		var neutralVariant = new TonalPalette(hue, Math.Min(chroma / 6.0, 8.0));
		var error = new TonalPalette(25.0, 84.0);

		// Build the resource dictionary
		var lightDict = new ResourceDictionary();
		var darkDict = new ResourceDictionary();

		// Primary
		SetColor(lightDict, darkDict, "PrimaryColor", primary, 40, 80);
		SetColor(lightDict, darkDict, "OnPrimaryColor", primary, 100, 20);
		SetColor(lightDict, darkDict, "PrimaryContainerColor", primary, 90, 30);
		SetColor(lightDict, darkDict, "OnPrimaryContainerColor", primary, 10, 90);
		SetColor(lightDict, darkDict, "PrimaryInverseColor", primary, 80, 40);
		SetColor(lightDict, darkDict, "PrimaryVariantDarkColor", primary, 30, 30);
		SetColor(lightDict, darkDict, "PrimaryVariantLightColor", primary, 70, 80);

		// Secondary
		SetColor(lightDict, darkDict, "SecondaryColor", secondary, 40, 80);
		SetColor(lightDict, darkDict, "OnSecondaryColor", secondary, 100, 20);
		SetColor(lightDict, darkDict, "SecondaryContainerColor", secondary, 90, 30);
		SetColor(lightDict, darkDict, "OnSecondaryContainerColor", secondary, 10, 90);
		SetColor(lightDict, darkDict, "SecondaryVariantDarkColor", secondary, 30, 30);
		SetColor(lightDict, darkDict, "SecondaryVariantLightColor", secondary, 70, 80);

		// Tertiary
		SetColor(lightDict, darkDict, "TertiaryColor", tertiary, 40, 80);
		SetColor(lightDict, darkDict, "OnTertiaryColor", tertiary, 100, 20);
		SetColor(lightDict, darkDict, "TertiaryContainerColor", tertiary, 90, 30);
		SetColor(lightDict, darkDict, "OnTertiaryContainerColor", tertiary, 10, 90);

		// Error
		SetColor(lightDict, darkDict, "ErrorColor", error, 40, 80);
		SetColor(lightDict, darkDict, "OnErrorColor", error, 100, 20);
		SetColor(lightDict, darkDict, "ErrorContainerColor", error, 90, 30);
		SetColor(lightDict, darkDict, "OnErrorContainerColor", error, 10, 90);

		// Background
		SetColor(lightDict, darkDict, "BackgroundColor", neutral, 99, 10);
		SetColor(lightDict, darkDict, "OnBackgroundColor", neutral, 10, 90);

		// Surface
		SetColor(lightDict, darkDict, "SurfaceColor", neutral, 99, 20);
		SetColor(lightDict, darkDict, "OnSurfaceColor", neutral, 10, 90);
		SetColor(lightDict, darkDict, "SurfaceVariantColor", neutralVariant, 90, 30);
		SetColor(lightDict, darkDict, "OnSurfaceVariantColor", neutralVariant, 30, 80);
		SetColor(lightDict, darkDict, "SurfaceInverseColor", neutral, 20, 90);
		SetColor(lightDict, darkDict, "OnSurfaceInverseColor", neutral, 95, 20);
		SetColor(lightDict, darkDict, "SurfaceTintColor", primary, 40, 80);

		// Outline
		SetColor(lightDict, darkDict, "OutlineColor", neutralVariant, 50, 60);
		SetColor(lightDict, darkDict, "OutlineVariantColor", neutralVariant, 80, 30);

		// Shadow (fixed, not derived from seed)
		var shadowColor = Color.FromArgb(0x33, 0x00, 0x00, 0x00);
		lightDict["ShadowColor"] = shadowColor;
		darkDict["ShadowColor"] = shadowColor;

		var result = new ResourceDictionary();
		result.ThemeDictionaries["Light"] = lightDict;
		result.ThemeDictionaries["Default"] = darkDict;
		return result;
	}

	private static void SetColor(
		ResourceDictionary lightDict, ResourceDictionary darkDict,
		string key, TonalPalette palette, int lightTone, int darkTone)
	{
		lightDict[key] = ArgbToColor(palette.GetArgb(lightTone));
		darkDict[key] = ArgbToColor(palette.GetArgb(darkTone));
	}

	private static int ColorToArgb(Color color) =>
		(color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

	private static Color ArgbToColor(int argb) =>
		Color.FromArgb(
			(byte)((argb >> 24) & 0xFF),
			(byte)((argb >> 16) & 0xFF),
			(byte)((argb >> 8) & 0xFF),
			(byte)(argb & 0xFF));
}
