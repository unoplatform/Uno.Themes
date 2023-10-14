using Uno.Markup.Xaml.UI;
using Uno.Markup.Xaml.UI.Xaml.Media;

namespace Uno.Markup.Extensions;

public static class ColorExtensions
{
	public static string ToRgbText(this Color color) => $"{color.R:X2}{color.G:X2}{color.B:X2}";
	public static string ToRgbText(this SolidColorBrush brush) => brush.Color.ToRgbText();

#if LINQPAD
	public static object ToColoredBlock(this Color color)
	{
		var background = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
		return Util.RawHtml($"<div style='height: 20px; width: 20px; background: #{color.ToRgbText()};' />");
	}
	public static object ToColoredBlock(this SolidColorBrush brush) => brush.Color.ToColoredBlock();
#endif

	public static string ToPrettyString(this Color color)
	{
		return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2} (rgb: {color.R} {color.G} {color.B}, alpha: {color.A})";
	}
	public static string ToPrettyString(this SolidColorBrush brush) => brush.Color.ToPrettyString();
}
