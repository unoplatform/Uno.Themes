// Ported from material-color-utilities by Google under the Apache License 2.0.
// Original source: https://github.com/material-foundation/material-color-utilities
// SPDX-License-Identifier: Apache-2.0

using System;

namespace Uno.Themes.ColorGeneration.Hct;

/// <summary>
/// A color in the HCT (Hue-Chroma-Tone) color space.
/// Hue is 0-360, Chroma is 0+, Tone (CIE L*) is 0-100.
/// This combines CAM16 hue and chroma with CIE L* lightness,
/// matching the Material Color Utilities approach.
/// </summary>
public readonly struct HctColor
{
	/// <summary>Hue in degrees (0-360).</summary>
	public double Hue { get; }

	/// <summary>Chroma (colorfulness). 0 = gray, higher = more vivid.</summary>
	public double Chroma { get; }

	/// <summary>Tone (CIE L*). 0 = black, 100 = white.</summary>
	public double Tone { get; }

	public HctColor(double hue, double chroma, double tone)
	{
		Hue = MathHelper.SanitizeDegrees(hue);
		Chroma = Math.Max(0, chroma);
		Tone = Math.Max(0, Math.Min(100, tone));
	}

	/// <summary>Create an HctColor from an ARGB int.</summary>
	public static HctColor FromArgb(int argb)
	{
		var cam = Cam16.FromArgb(argb);
		double tone = ColorMath.LstarFromArgb(argb);
		return new HctColor(cam.Hue, cam.Chroma, tone);
	}

	/// <summary>Convert this HCT color to an ARGB int.</summary>
	public int ToArgb()
	{
		return HctSolver.SolveToArgb(Hue, Chroma, Tone);
	}
}
