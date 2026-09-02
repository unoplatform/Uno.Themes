#nullable enable

using System;

namespace Uno.Omarchy;

public static partial class OmarchyPalettes
{
	/// <summary>
	/// Finds a stock palette by name. Matching ignores case, spaces and dashes, so
	/// <c>"tokyo-night"</c> (the Omarchy theme slug), <c>"Tokyo Night"</c> and <c>"TokyoNight"</c>
	/// all resolve to <see cref="TokyoNight"/>. This is also the <c>CreateFromString</c> converter
	/// behind <c>&lt;OmarchyTheme Palette="Nord" /&gt;</c>.
	/// </summary>
	/// <returns>The palette, or <c>null</c> when no stock palette has that name.</returns>
	public static OmarchyPalette? FromName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}

		var wanted = Normalize(name);
		var all = All;
		for (int i = 0; i < all.Count; i++)
		{
			if (Normalize(all[i].Name).Equals(wanted, StringComparison.OrdinalIgnoreCase))
			{
				return all[i];
			}
		}

		return null;
	}

	private static string Normalize(string value)
	{
		// Names are short (≤ 20 chars) and this runs once per lookup, not on a hot path.
		return value.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("_", string.Empty);
	}
}
