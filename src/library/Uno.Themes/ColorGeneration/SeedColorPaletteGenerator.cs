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

	// Single-entry cache: stores only the most recent seed → palette result.
	// Runtime color picking only uses one seed at a time, so this avoids
	// unbounded growth from dragging a color picker.
	private (int primary, int? secondary, int? tertiary, bool preserveSeedColor) _cacheKey;
	private ResourceDictionary _cacheValue;

	internal ResourceDictionary Generate(
		Color primarySeedColor,
		Color? secondarySeedColor = null,
		Color? tertiarySeedColor = null,
		bool preserveSeedColor = true)
	{
		var key = (
			ColorToArgb(primarySeedColor),
			secondarySeedColor is { } sec ? (int?)ColorToArgb(sec) : null,
			tertiarySeedColor is { } ter ? (int?)ColorToArgb(ter) : null,
			preserveSeedColor);

		if (_cacheValue is not null && _cacheKey == key)
		{
			return _cacheValue;
		}

		var result = GenerateCore(primarySeedColor, secondarySeedColor, tertiarySeedColor, preserveSeedColor);
		_cacheKey = key;
		_cacheValue = result;
		return result;
	}

	private static ResourceDictionary GenerateCore(
		Color seedColor,
		Color? secondarySeedColor,
		Color? tertiarySeedColor,
		bool preserveSeedColor)
	{
		int seedArgb = ColorToArgb(seedColor);
		var seedHct = HctColor.FromArgb(seedArgb);
		double hue = seedHct.Hue;
		double chroma = seedHct.Chroma;

		// Derive the six tonal palettes. Two variants, matching material-color-utilities'
		// CorePalette.of(argb, isContent:):
		//  - preserveSeedColor (default, "content"): every palette is scaled from the seed's
		//    own chroma, so the generated ramp stays faithful to the seed — a gray seed
		//    produces a gray palette rather than a colored one.
		//  - !preserveSeedColor ("tonal spot", the pre-8.0 behavior): a minimum chroma of 48
		//    on Primary and fixed chromas on the supporting palettes, which guarantees vibrant
		//    output at the cost of distorting the seed.
		double primaryChroma = preserveSeedColor ? chroma : Math.Max(chroma, 48);
		var primary = new TonalPalette(hue, primaryChroma);

		TonalPalette secondary;
		if (secondarySeedColor is { } secSeed)
		{
			// An explicit seed always uses its own hue and chroma verbatim, in either variant.
			var secHct = HctColor.FromArgb(ColorToArgb(secSeed));
			secondary = new TonalPalette(secHct.Hue, secHct.Chroma);
		}
		else
		{
			secondary = new TonalPalette(hue, preserveSeedColor ? chroma / 3.0 : 16);
		}

		TonalPalette tertiary;
		if (tertiarySeedColor is { } terSeed)
		{
			var terHct = HctColor.FromArgb(ColorToArgb(terSeed));
			tertiary = new TonalPalette(terHct.Hue, terHct.Chroma);
		}
		else
		{
			tertiary = new TonalPalette(hue + 60.0, preserveSeedColor ? chroma / 2.0 : 24);
		}

		var neutral = new TonalPalette(hue, preserveSeedColor ? Math.Min(chroma / 12.0, 4.0) : 4);
		var neutralVariant = new TonalPalette(hue, preserveSeedColor ? Math.Min(chroma / 6.0, 8.0) : 8);

		// Error palette is intentionally omitted — it uses fixed values (hue=25, chroma=84)
		// independent of the seed color, matching the Material Theme Builder behavior.
		// The built-in SharedColorPalette.xaml already defines the correct Error colors.

		// Build the resource dictionary
		var lightDict = new ResourceDictionary();
		var darkDict = new ResourceDictionary();

		// Primary. In preserve-seed mode the light role *is* the seed, verbatim — the promise
		// being that PrimaryColor renders the exact hex the consumer supplied. Dark stays
		// derived at tone 80: a dark brand color pinned onto a dark surface is unreadable.
		int lightPrimary = preserveSeedColor ? seedArgb : primary.GetArgb(40);
		lightDict["PrimaryColor"] = ArgbToColor(lightPrimary);
		darkDict["PrimaryColor"] = ArgbToColor(primary.GetArgb(80));

		lightDict["OnPrimaryColor"] = ArgbToColor(
			preserveSeedColor ? PickOnColor(lightPrimary, primary) : primary.GetArgb(100));
		darkDict["OnPrimaryColor"] = ArgbToColor(primary.GetArgb(20));

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
		// SurfaceTint is M3's alias of Primary, so it follows the pinned value.
		lightDict["SurfaceTintColor"] = ArgbToColor(lightPrimary);
		darkDict["SurfaceTintColor"] = ArgbToColor(primary.GetArgb(80));

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

	/// <summary>
	/// Choose the foreground for a pinned Primary. Because the seed's tone is whatever the
	/// consumer supplied, M3's fixed tone-100 foreground is not always legible on it; pick
	/// whichever palette extreme contrasts best. Tone 0 is included because tone 10 tops out
	/// near 3.8:1 against a mid-tone (~L*50) background, which would leave the default pairing
	/// below WCAG AA — white/black always reaches at least 4.58:1.
	/// </summary>
	private static int PickOnColor(int backgroundArgb, TonalPalette palette)
	{
		int onLight = palette.GetArgb(100);
		int onDark = palette.GetArgb(10);
		int black = palette.GetArgb(0);

		double lightContrast = ColorMath.ContrastRatio(backgroundArgb, onLight);
		double darkContrast = ColorMath.ContrastRatio(backgroundArgb, onDark);
		double blackContrast = ColorMath.ContrastRatio(backgroundArgb, black);

		if (lightContrast >= darkContrast && lightContrast >= blackContrast)
		{
			return onLight;
		}

		return darkContrast >= blackContrast ? onDark : black;
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
