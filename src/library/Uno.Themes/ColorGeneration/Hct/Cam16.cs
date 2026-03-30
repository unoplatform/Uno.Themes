using System;

namespace Uno.Themes.ColorGeneration.Hct;

/// <summary>
/// CAM16 color appearance model. Converts between ARGB and perceptual
/// attributes (hue, chroma, lightness J) under given viewing conditions.
/// </summary>
internal readonly struct Cam16
{
	internal double Hue { get; }
	internal double Chroma { get; }
	internal double J { get; }
	internal double Q { get; }
	internal double M { get; }
	internal double S { get; }
	internal double JStar { get; }
	internal double AStar { get; }
	internal double BStar { get; }

	private Cam16(double hue, double chroma, double j, double q, double m, double s,
		double jStar, double aStar, double bStar)
	{
		Hue = hue;
		Chroma = chroma;
		J = j;
		Q = q;
		M = m;
		S = s;
		JStar = jStar;
		AStar = aStar;
		BStar = bStar;
	}

	/// <summary>Compute CAM16 appearance correlates from an ARGB color.</summary>
	internal static Cam16 FromArgb(int argb)
	{
		return FromArgb(argb, ViewingConditions.Default);
	}

	internal static Cam16 FromArgb(int argb, ViewingConditions vc)
	{
		int red = (argb >> 16) & 0xFF;
		int green = (argb >> 8) & 0xFF;
		int blue = argb & 0xFF;

		double redL = ColorMath.Linearized(red);
		double greenL = ColorMath.Linearized(green);
		double blueL = ColorMath.Linearized(blue);

		double rT = 0.401288 * redL + 0.650173 * greenL - 0.051461 * blueL;
		double gT = -0.250268 * redL + 1.204414 * greenL + 0.045854 * blueL;
		double bT = -0.002079 * redL + 0.048952 * greenL + 0.953127 * blueL;

		double rD = vc.RgbD[0] * rT;
		double gD = vc.RgbD[1] * gT;
		double bD = vc.RgbD[2] * bT;

		double rAF = Math.Pow(vc.Fl * Math.Abs(rD) / 100.0, 0.42);
		double gAF = Math.Pow(vc.Fl * Math.Abs(gD) / 100.0, 0.42);
		double bAF = Math.Pow(vc.Fl * Math.Abs(bD) / 100.0, 0.42);

		double rA = Math.Sign(rD) * 400.0 * rAF / (rAF + 27.13);
		double gA = Math.Sign(gD) * 400.0 * gAF / (gAF + 27.13);
		double bA = Math.Sign(bD) * 400.0 * bAF / (bAF + 27.13);

		double a = (11.0 * rA + -12.0 * gA + bA) / 11.0;
		double b = (rA + gA - 2.0 * bA) / 9.0;

		double u = (20.0 * rA + 20.0 * gA + 21.0 * bA) / 20.0;
		double p2 = (40.0 * rA + 20.0 * gA + bA) / 20.0;

		double atan2 = Math.Atan2(b, a);
		double atanDegrees = atan2 * 180.0 / Math.PI;
		double hue = MathHelper.SanitizeDegrees(atanDegrees);

		double ac = p2 * vc.Nbb;
		double j = 100.0 * Math.Pow(ac / vc.Aw, vc.C * vc.Z);

		double q = (4.0 / vc.C) * Math.Sqrt(j / 100.0) * (vc.Aw + 4.0) * vc.FlRoot;

		double huePrime = hue < 20.14 ? hue + 360.0 : hue;
		double eHue = 0.25 * (Math.Cos(huePrime * Math.PI / 180.0 + 2.0) + 3.8);
		double p1 = 50000.0 / 13.0 * eHue * vc.Nc * vc.Ncb;
		double t = p1 * Math.Sqrt(a * a + b * b) / (u + 0.305);
		double alpha = Math.Pow(t, 0.9) * Math.Pow(1.64 - Math.Pow(0.29, vc.N), 0.73);

		double c16 = alpha * Math.Sqrt(j / 100.0);
		double m16 = c16 * vc.FlRoot;
		double s16 = 50.0 * Math.Sqrt(alpha * vc.C / (vc.Aw + 4.0));

		double jStar = (1.0 + 100.0 * 0.007) * j / (1.0 + 0.007 * j);
		double mStar = 1.0 / 0.0228 * Math.Log(1.0 + 0.0228 * m16);
		double aStar = mStar * Math.Cos(hue * Math.PI / 180.0);
		double bStar = mStar * Math.Sin(hue * Math.PI / 180.0);

		return new Cam16(hue, c16, j, q, m16, s16, jStar, aStar, bStar);
	}

	/// <summary>
	/// Solve for the ARGB color with the given CAM16 J (lightness) and
	/// chroma at the specified hue, under default viewing conditions.
	/// Returns the closest in-gamut color.
	/// </summary>
	internal static int ToArgb(double hue, double chroma, double j)
	{
		return ToArgb(hue, chroma, j, ViewingConditions.Default);
	}

	internal static int ToArgb(double hue, double chroma, double j, ViewingConditions vc)
	{
		if (chroma < 0.5 || j < 0.0001)
		{
			// For near-achromatic colors, compute a gray from J via the reverse
			// of the achromatic response. This avoids the J≠L* confusion.
			return GrayArgbFromJ(j, vc);
		}

		double hueRadians = hue * Math.PI / 180.0;
		double alpha = chroma / Math.Sqrt(j / 100.0);
		double t = Math.Pow(alpha / Math.Pow(1.64 - Math.Pow(0.29, vc.N), 0.73), 1.0 / 0.9);

		double ac = vc.Aw * Math.Pow(j / 100.0, 1.0 / (vc.C * vc.Z));

		double huePrime = hue < 20.14 ? hue + 360.0 : hue;
		double eHue = 0.25 * (Math.Cos(huePrime * Math.PI / 180.0 + 2.0) + 3.8);
		double p1 = 50000.0 / 13.0 * eHue * vc.Nc * vc.Ncb;
		double p2 = ac / vc.Nbb;

		double cosHue = Math.Cos(hueRadians);
		double sinHue = Math.Sin(hueRadians);

		double gamma = 23.0 * (p2 + 0.305) * t / (23.0 * p1 + 11.0 * t * cosHue + 108.0 * t * sinHue);
		double a = gamma * cosHue;
		double b = gamma * sinHue;

		double rA = (460.0 * p2 + 451.0 * a + 288.0 * b) / 1403.0;
		double gA = (460.0 * p2 - 891.0 * a - 261.0 * b) / 1403.0;
		double bA = (460.0 * p2 - 220.0 * a - 6300.0 * b) / 1403.0;

		double rF = InverseAdapt(rA, vc.Fl);
		double gF = InverseAdapt(gA, vc.Fl);
		double bF = InverseAdapt(bA, vc.Fl);

		double rX = rF / vc.RgbD[0];
		double gX = gF / vc.RgbD[1];
		double bX = bF / vc.RgbD[2];

		// Inverse M16 to get linear RGB
		double linearR = 1.8620678 * rX - 1.0112547 * gX + 0.14918678 * bX;
		double linearG = 0.38752654 * rX + 0.62144744 * gX - 0.00897398 * bX;
		double linearB = -0.01584150 * rX - 0.03412294 * gX + 1.0499644 * bX;

		int ri = ColorMath.Delinearized(linearR);
		int gi = ColorMath.Delinearized(linearG);
		int bi = ColorMath.Delinearized(linearB);

		return ColorMath.ArgbFromRgb(ri, gi, bi);
	}

	private static double InverseAdapt(double adapted, double fl)
	{
		double abs = Math.Abs(adapted);
		double base_ = Math.Max(0, 27.13 * abs / (400.0 - abs));
		return Math.Sign(adapted) * 100.0 / fl * Math.Pow(base_, 1.0 / 0.42);
	}

	private static int GrayArgbFromJ(double j, ViewingConditions vc)
	{
		// For achromatic colors, reverse the J → achromatic signal path:
		// j = 100 * (ac / aw)^(c*z), so ac = aw * (j/100)^(1/(c*z))
		// For a gray: rA = gA = bA, and p2 = (40*rA + 20*rA + rA)/20 = 61*rA/20
		// ac = p2 * nbb
		// adapted = 400 * factor / (factor + 27.13)
		// factor = (fl * |rD| / 100)^0.42
		// rD = rgbD[0] * rT, for gray rT = (0.401288+0.650173-0.051461)*L = L
		if (j <= 0)
		{
			return ColorMath.ArgbFromRgb(0, 0, 0);
		}

		double ac = vc.Aw * Math.Pow(j / 100.0, 1.0 / (vc.C * vc.Z));
		double p2 = ac / vc.Nbb;
		// For achromatic: a=0, b=0, so rA = gA = bA = p2 * 20/61
		double adapted = p2 * 20.0 / 61.0;

		double rF = InverseAdapt(adapted, vc.Fl);
		// For gray: all three M16 channels are equal before chromatic adaptation
		// rD = rgbD * rT, and rT = gT = bT for a gray
		// After inverse M16: linearR = linearG = linearB
		// rT = rF / rgbD[0], but we need to un-adapt all three channels
		// and then inverse M16 to get linear RGB
		double rX = rF / vc.RgbD[0];
		double gX = rF / vc.RgbD[1];
		double bX = rF / vc.RgbD[2];

		double linearR = 1.8620678 * rX - 1.0112547 * gX + 0.14918678 * bX;
		double linearG = 0.38752654 * rX + 0.62144744 * gX - 0.00897398 * bX;
		double linearB = -0.01584150 * rX - 0.03412294 * gX + 1.0499644 * bX;

		int ri = ColorMath.Delinearized(linearR);
		int gi = ColorMath.Delinearized(linearG);
		int bi = ColorMath.Delinearized(linearB);

		return ColorMath.ArgbFromRgb(ri, gi, bi);
	}
}
