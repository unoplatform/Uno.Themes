using System;
using System.Collections.Generic;
using Uno.Themes.ColorGeneration.Hct;

namespace Uno.Themes.ColorGeneration;

/// <summary>
/// A tonal palette: a fixed hue and chroma that can produce colors at any tone (0-100).
/// </summary>
public sealed class TonalPalette
{
	private readonly double _hue;
	private readonly double _chroma;
	private readonly Dictionary<int, int> _cache = new();

	public TonalPalette(double hue, double chroma)
	{
		_hue = hue;
		_chroma = chroma;
	}

	/// <summary>
	/// Create a TonalPalette from a seed ARGB color.
	/// Uses the seed's hue and the specified chroma (or the seed's chroma if not specified).
	/// </summary>
	public static TonalPalette FromHct(HctColor hct, double? overrideChroma = null)
	{
		return new TonalPalette(hct.Hue, overrideChroma ?? hct.Chroma);
	}

	/// <summary>Get the ARGB color at the specified tone (0-100).</summary>
	public int GetArgb(int tone)
	{
		if (!_cache.TryGetValue(tone, out int argb))
		{
			argb = new HctColor(_hue, _chroma, tone).ToArgb();
			_cache[tone] = argb;
		}
		return argb;
	}
}
