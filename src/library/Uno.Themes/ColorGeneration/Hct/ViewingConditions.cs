using System;

namespace Uno.Themes.ColorGeneration.Hct;

/// <summary>
/// Pre-computed constants for the CAM16 color appearance model under
/// standard sRGB viewing conditions (D65, 200 cd/m^2, average surround).
/// </summary>
internal sealed class ViewingConditions
{
	internal static readonly ViewingConditions Default = Create(
		whitePoint: ColorMath.WhitePointD65,
		adaptingLuminance: 11.725676537,  // ~200 cd/m² monitor
		backgroundLstar: 50.0,
		surround: 2.0,                    // average surround
		discountingIlluminant: false);

	internal double N { get; }
	internal double Aw { get; }
	internal double Nbb { get; }
	internal double Ncb { get; }
	internal double C { get; }
	internal double Nc { get; }
	internal double Fl { get; }
	internal double FlRoot { get; }
	internal double Z { get; }
	internal double[] RgbD { get; }

	private ViewingConditions(double n, double aw, double nbb, double ncb,
		double c, double nc, double fl, double flRoot, double z, double[] rgbD)
	{
		N = n;
		Aw = aw;
		Nbb = nbb;
		Ncb = ncb;
		C = c;
		Nc = nc;
		Fl = fl;
		FlRoot = flRoot;
		Z = z;
		RgbD = rgbD;
	}

	private static ViewingConditions Create(
		double[] whitePoint, double adaptingLuminance,
		double backgroundLstar, double surround, bool discountingIlluminant)
	{
		// M16 transform: XYZ → sharpened RGB (Hunt-Pointer-Estevez adapted)
		double rW = 0.401288 * whitePoint[0] + 0.650173 * whitePoint[1] - 0.051461 * whitePoint[2];
		double gW = -0.250268 * whitePoint[0] + 1.204414 * whitePoint[1] + 0.045854 * whitePoint[2];
		double bW = -0.002079 * whitePoint[0] + 0.048952 * whitePoint[1] + 0.953127 * whitePoint[2];

		double f = 0.8 + surround / 10.0;
		double c = f >= 0.9 ? MathHelper.Lerp(0.59, 0.69, (f - 0.9) * 10.0)
				 : MathHelper.Lerp(0.525, 0.59, (f - 0.8) * 10.0);

		double d = discountingIlluminant
			? 1.0
			: f * (1.0 - 1.0 / 3.6 * Math.Exp((-adaptingLuminance - 42.0) / 92.0));
		d = Math.Max(0.0, Math.Min(1.0, d));

		double[] rgbD =
		{
			d * (100.0 / rW) + 1.0 - d,
			d * (100.0 / gW) + 1.0 - d,
			d * (100.0 / bW) + 1.0 - d,
		};

		double k = 1.0 / (5.0 * adaptingLuminance + 1.0);
		double k4 = k * k * k * k;
		double k4F = 1.0 - k4;
		double fl = k4 * adaptingLuminance + 0.1 * k4F * k4F * Math.Pow(5.0 * adaptingLuminance, 1.0 / 3.0);

		double n = ColorMath.YFromLstar(backgroundLstar) / whitePoint[1];
		double z = 1.48 + Math.Sqrt(n);
		double nbb = 0.725 / Math.Pow(n, 0.2);
		double nc = f;

		double[] rgbAFactors =
		{
			Math.Pow(fl * rgbD[0] * rW / 100.0, 0.42),
			Math.Pow(fl * rgbD[1] * gW / 100.0, 0.42),
			Math.Pow(fl * rgbD[2] * bW / 100.0, 0.42),
		};
		double[] rgbA =
		{
			400.0 * rgbAFactors[0] / (rgbAFactors[0] + 27.13),
			400.0 * rgbAFactors[1] / (rgbAFactors[1] + 27.13),
			400.0 * rgbAFactors[2] / (rgbAFactors[2] + 27.13),
		};

		double aw = (2.0 * rgbA[0] + rgbA[1] + 0.05 * rgbA[2]) * nbb;

		return new ViewingConditions(n, aw, nbb, nbb, c, nc, fl, Math.Pow(fl, 0.25), z, rgbD);
	}
}

internal static class MathHelper
{
	internal static double Lerp(double a, double b, double t) => a + (b - a) * t;

	internal static double SanitizeDegrees(double degrees)
	{
		degrees %= 360.0;
		if (degrees < 0) degrees += 360.0;
		return degrees;
	}
}
