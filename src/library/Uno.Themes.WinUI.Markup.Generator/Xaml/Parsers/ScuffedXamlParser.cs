using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Uno.Markup.Xaml.UI;
using Uno.Markup.Xaml.UI.Xaml;
using Uno.Markup.Xaml.UI.Xaml.Controls;
using Uno.Markup.Xaml.UI.Xaml.Media;
using Uno.Markup.Xaml.UI.Xaml.Media.Animation;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Xaml.Parsers;

internal static class ScuffedXamlParser
{
	// fixme: make this configurable
	public static readonly string[] IgnoredXmlnsPrefixes = "todo,void".Split(',');

	public static object? Load(string path) => Load<object>(path);
	public static T? Load<T>(string path)
	{
		var document = XDocument.Load(path);

		return Parse<T>(document.Root!);
	}
	public static T? Parse<T>(XElement e) => (T?)Parse(e);

	public static object? Parse(XElement e)
	{
		if (IsExplicitlyIgnored(e)) return new Ignorable(e.Name.LocalName);
		if (TryParseSimpleValue(e, out var sv)) return sv;
		if (TryParseGenericValueObject(e, out var gvo)) return gvo;
		if (VisualTreeElement.TryParse(e, out var vte)) return vte;

		return e.GetNameParts() switch
		{
			(NSPresentation, nameof(ResourceDictionary)) => ResourceDictionary.Parse(e),
			(NSPresentation, nameof(Thickness)) => ParseThickness(e.Value),
			(NSPresentation, nameof(CornerRadius)) => ParseCornerRadius(e.Value),
			(NSPresentation, nameof(Color)) => ParseColor(e.Value),
			(NSPresentation, nameof(SolidColorBrush)) => ParseSolidColorBrush(e),
			(NSPresentation, nameof(StaticResource)) => ParseStaticResource(e),
			(NSPresentation, nameof(ThemeResource)) => ParseThemeResource(e),

			// common platform-conditional offenders
			(_/*NSPresentation*/, nameof(Style)) => Style.Parse(e),
			(_/*NSPresentation*/, nameof(Setter)) => Setter.Parse(e),
			(_/*NSPresentation*/, nameof(ControlTemplate)) => ControlTemplate.Parse(e),

			(NSPresentation, nameof(VisualStateGroup)) => VisualStateGroup.Parse(e),
			(NSPresentation, nameof(VisualTransition)) => VisualTransition.Parse(e),
			(NSPresentation, nameof(VisualState)) => VisualState.Parse(e),
			(NSPresentation, nameof(Storyboard)) => Storyboard.Parse(e),

			_ when e.Name.LocalName.EndsWith("Converter") => null,
			_ => UnhandledResult(),
		};

		object UnhandledResult()
		{
#if SKIP_ALL_NOTIMPLEMENTED_OBJECT
#if !SKIP_ALL_NOTIMPLEMENTED_OBJECT_NO_LOG_WARN
			Console.WriteLine($"Ignoring '{e.Name.LocalName}', as there is no parser implemented for it");
#endif
			return new NotParsedObject(e.Name.LocalName.ToString());
#else
			throw new NotImplementedException(e.Name.ToString());
#endif
		}
	}
	public static object ParseInlineValue(string value)
	{
		if (IKeyedResource.TryParseResource(value) is { } markup)
		{
			return markup;
		}

		// just returning raw text for now, since we dont have the type plugged in here
		return value;
	}

	private static bool TryParseSimpleValue(XElement e, out object? result) // for primitive, struct, POCO types, excluding DependencyObject
	{
		(var success, result) = e.GetNameParts() switch
		{
			(NSX, nameof(Boolean)) => (true, bool.Parse(e.Value)),
			(NSX, nameof(Int32)) => (true, int.Parse(e.Value)),
			(NSX, nameof(Double)) => (true, double.Parse(e.Value)),
			(NSX, nameof(String)) => (true, e.Value),

			_ => (false, default(object)),
		};
		return success;
	}
	private static bool TryParseGenericValueObject(XElement e, out object? result) // for types that we are too lazy to define
	{
		result = e.GetNameParts() switch
		{
#if ENABLE_GENERIC_VALUE_OBJECT_PARSING
			(NSPresentation, "FontFamily") => new GenericValueObject("FontFamily", e.Value),
			(NSPresentation, "Visibility") => new GenericValueObject("Visibility", e.Value),
			(NSPresentation, "FontWeight") => new GenericValueObject("FontWeight", e.Value),
			(NSPresentation, "GridLength") => new GenericValueObject("GridLength", e.Value),
			//(NSPresentation, "ControlTemplate") => new GenericValueObject("ControlTemplate", "parser-not-implemented"),
			(_, "LottieVisualSource") => new GenericValueObject("LottieVisualSource", e.Attribute("UriSource")?.Value),
#endif

			_ => null,
		};
		return result != null;
	}

	public static Thickness ParseThickness(string raw)
	{
		try
		{
			return raw.Split(',') switch
			{
				[var uniform] when double.TryParse(uniform, out var u) => new(u, u, u, u),
				[var lr, var tb] when
					double.TryParse(lr, out var lrv) &&
					double.TryParse(tb, out var tbv)
					=> new(lrv, tbv, lrv, tbv),
				[var l, var t, var r, var b] when
					double.TryParse(l, out var lv) &&
					double.TryParse(t, out var tv) &&
					double.TryParse(r, out var rv) &&
					double.TryParse(b, out var bv)
					=> new(lv, tv, rv, bv),

				_ => throw new FormatException("Invalid corner-radius syntax"),
			};
		}
		catch (Exception ex)
		{
			throw new ArgumentException("Invalid corner-radius literal: " + raw, ex);
		}
	}
	public static CornerRadius ParseCornerRadius(string raw)
	{
		try
		{
			return raw.Split(',') switch
			{
				[var uniform] when double.TryParse(uniform, out var u) => new(u, u, u, u),
				[var tl, var tr, var br, var bl] when
					double.TryParse(tl, out var tlv) &&
					double.TryParse(tr, out var trv) &&
					double.TryParse(br, out var brv) &&
					double.TryParse(bl, out var blv)
					=> new(tlv, trv, brv, blv),

				_ => throw new FormatException("Invalid corner-radius syntax"),
			};
		}
		catch (Exception ex)
		{
			throw new ArgumentException("Invalid corner-radius literal: " + raw, ex);
		}
	}
	public static Color ParseColor(string raw)
	{
		// #rgb (need to check definition...), #rrggbb, #aarrggbb
		if (raw.StartsWith("#") && raw[1..] is { Length: /*3 or*/ 6 or 8 } hex && Regex.IsMatch(hex, "^[a-f0-9]+$", RegexOptions.IgnoreCase))
		{
			var parts = Enumerable.Range(0, hex.Length / 2).Select(x => byte.Parse(hex.Substring(x * 2, 2), NumberStyles.HexNumber)).ToArray();
			return parts.Length switch
			{
				3 => new Color(byte.MaxValue, parts[0], parts[1], parts[2]),
				4 => new Color(parts[0], parts[1], parts[2], parts[3]),

				_ => throw new ArgumentException("Invalid color literal: " + raw),
			};
		}

		uint color = raw.ToLowerInvariant() switch
		{
			"transparent" => 0x00FFFFFF,
			"aliceblue" => 0xFFF0F8FF,
			"antiquewhite" => 0xFFFAEBD7,
			"aqua" => 0xFF00FFFF,
			"aquamarine" => 0xFF7FFFD4,
			"azure" => 0xFFF0FFFF,
			"beige" => 0xFFF5F5DC,
			"bisque" => 0xFFFFE4C4,
			"black" => 0xFF000000,
			"blanchedalmond" => 0xFFFFEBCD,
			"blue" => 0xFF0000FF,
			"blueviolet" => 0xFF8A2BE2,
			"brown" => 0xFFA52A2A,
			"burlywood" => 0xFFDEB887,
			"cadetblue" => 0xFF5F9EA0,
			"chartreuse" => 0xFF7FFF00,
			"chocolate" => 0xFFD2691E,
			"coral" => 0xFFFF7F50,
			"cornflowerblue" => 0xFF6495ED,
			"cornsilk" => 0xFFFFF8DC,
			"crimson" => 0xFFDC143C,
			"cyan" => 0xFF00FFFF,
			"darkblue" => 0xFF00008B,
			"darkcyan" => 0xFF008B8B,
			"darkgoldenrod" => 0xFFB8860B,
			"darkgray" => 0xFFA9A9A9,
			"darkgreen" => 0xFF006400,
			"darkkhaki" => 0xFFBDB76B,
			"darkmagenta" => 0xFF8B008B,
			"darkolivegreen" => 0xFF556B2F,
			"darkorange" => 0xFFFF8C00,
			"darkorchid" => 0xFF9932CC,
			"darkred" => 0xFF8B0000,
			"darksalmon" => 0xFFE9967A,
			"darkseagreen" => 0xFF8FBC8F,
			"darkslateblue" => 0xFF483D8B,
			"darkslategray" => 0xFF2F4F4F,
			"darkturquoise" => 0xFF00CED1,
			"darkviolet" => 0xFF9400D3,
			"deeppink" => 0xFFFF1493,
			"deepskyblue" => 0xFF00BFFF,
			"dimgray" => 0xFF696969,
			"dodgerblue" => 0xFF1E90FF,
			"firebrick" => 0xFFB22222,
			"floralwhite" => 0xFFFFFAF0,
			"forestgreen" => 0xFF228B22,
			"fuchsia" => 0xFFFF00FF,
			"gainsboro" => 0xFFDCDCDC,
			"ghostwhite" => 0xFFF8F8FF,
			"gold" => 0xFFFFD700,
			"goldenrod" => 0xFFDAA520,
			"gray" => 0xFF808080,
			"green" => 0xFF008000,
			"greenyellow" => 0xFFADFF2F,
			"honeydew" => 0xFFF0FFF0,
			"hotpink" => 0xFFFF69B4,
			"indianred" => 0xFFCD5C5C,
			"indigo" => 0xFF4B0082,
			"ivory" => 0xFFFFFFF0,
			"khaki" => 0xFFF0E68C,
			"lavender" => 0xFFE6E6FA,
			"lavenderblush" => 0xFFFFF0F5,
			"lawngreen" => 0xFF7CFC00,
			"lemonchiffon" => 0xFFFFFACD,
			"lightblue" => 0xFFADD8E6,
			"lightcoral" => 0xFFF08080,
			"lightcyan" => 0xFFE0FFFF,
			"lightgoldenrodyellow" => 0xFFFAFAD2,
			"lightgray" => 0xFFD3D3D3,
			"lightgreen" => 0xFF90EE90,
			"lightpink" => 0xFFFFB6C1,
			"lightsalmon" => 0xFFFFA07A,
			"lightseagreen" => 0xFF20B2AA,
			"lightskyblue" => 0xFF87CEFA,
			"lightslategray" => 0xFF778899,
			"lightsteelblue" => 0xFFB0C4DE,
			"lightyellow" => 0xFFFFFFE0,
			"lime" => 0xFF00FF00,
			"limegreen" => 0xFF32CD32,
			"linen" => 0xFFFAF0E6,
			"magenta" => 0xFFFF00FF,
			"maroon" => 0xFF800000,
			"mediumaquamarine" => 0xFF66CDAA,
			"mediumblue" => 0xFF0000CD,
			"mediumorchid" => 0xFFBA55D3,
			"mediumpurple" => 0xFF9370DB,
			"mediumseagreen" => 0xFF3CB371,
			"mediumslateblue" => 0xFF7B68EE,
			"mediumspringgreen" => 0xFF00FA9A,
			"mediumturquoise" => 0xFF48D1CC,
			"mediumvioletred" => 0xFFC71585,
			"midnightblue" => 0xFF191970,
			"mintcream" => 0xFFF5FFFA,
			"mistyrose" => 0xFFFFE4E1,
			"moccasin" => 0xFFFFE4B5,
			"navajowhite" => 0xFFFFDEAD,
			"navy" => 0xFF000080,
			"oldlace" => 0xFFFDF5E6,
			"olive" => 0xFF808000,
			"olivedrab" => 0xFF6B8E23,
			"orange" => 0xFFFFA500,
			"orangered" => 0xFFFF4500,
			"orchid" => 0xFFDA70D6,
			"palegoldenrod" => 0xFFEEE8AA,
			"palegreen" => 0xFF98FB98,
			"paleturquoise" => 0xFFAFEEEE,
			"palevioletred" => 0xFFDB7093,
			"papayawhip" => 0xFFFFEFD5,
			"peachpuff" => 0xFFFFDAB9,
			"peru" => 0xFFCD853F,
			"pink" => 0xFFFFC0CB,
			"plum" => 0xFFDDA0DD,
			"powderblue" => 0xFFB0E0E6,
			"purple" => 0xFF800080,
			"red" => 0xFFFF0000,
			"rosybrown" => 0xFFBC8F8F,
			"royalblue" => 0xFF4169E1,
			"saddlebrown" => 0xFF8B4513,
			"salmon" => 0xFFFA8072,
			"sandybrown" => 0xFFF4A460,
			"seagreen" => 0xFF2E8B57,
			"seashell" => 0xFFFFF5EE,
			"sienna" => 0xFFA0522D,
			"silver" => 0xFFC0C0C0,
			"skyblue" => 0xFF87CEEB,
			"slateblue" => 0xFF6A5ACD,
			"slategray" => 0xFF708090,
			"snow" => 0xFFFFFAFA,
			"springgreen" => 0xFF00FF7F,
			"steelblue" => 0xFF4682B4,
			"tan" => 0xFFD2B48C,
			"teal" => 0xFF008080,
			"thistle" => 0xFFD8BFD8,
			"tomato" => 0xFFFF6347,
			"turquoise" => 0xFF40E0D0,
			"violet" => 0xFFEE82EE,
			"wheat" => 0xFFF5DEB3,
			"white" => 0xFFFFFFFF,
			"whitesmoke" => 0xFFF5F5F5,
			"yellow" => 0xFFFFFF00,
			"yellowgreen" => 0xFF9ACD32,

			_ => throw new ArgumentException("Invalid color literal: " + raw),
		};
		if (BitConverter.GetBytes(color) is [var a, var r, var g, var b])
		{
			return new Color(a, r, g, b);
		}
		else
		{
			// how?
			throw new InvalidOperationException();
		}
	}
	public static SolidColorBrush ParseSolidColorBrush(XElement e)
	{
		var result = new SolidColorBrush();
		if (result.MapDP(e, x => x.Color, out var color)) result = result with { Color = ParseColor(color) };
		if (result.MapDP(e, x => x.Opacity, out var opacity)) result = result with { Opacity = double.Parse(opacity) };

		return result;
	}
	private static object ParseStaticResource(XElement e)
	{
		return new StaticResourceRef(e.Attribute("ResourceKey")?.Value ?? throw new ArgumentException("Missing ResourceKey"));
	}
	private static object ParseThemeResource(XElement e)
	{
		return new ThemeResourceRef(e.Attribute("ResourceKey")?.Value ?? throw new ArgumentException("Missing ResourceKey"));
	}

	// we dont actually want to just take Ignorable as is (due to uno specific xmlns), so we will use a custom list here
	// we dont want to introduce the notion of "platform" yet...
	public static bool IsExplicitlyIgnored(XElement e)
	{
		var prefix = e.GetPrefixOfNamespace(e.Name.NamespaceName);
		return IgnoredXmlnsPrefixes.Contains(prefix);
	}
	public static bool IsExplicitlyIgnored(XAttribute attribute)
	{
		var prefix = attribute?.Parent?.GetPrefixOfNamespace(attribute.Name.NamespaceName);
		return IgnoredXmlnsPrefixes.Contains(prefix);
	}

	
	private static bool MapDP<T, TProperty>(this T obj, XElement e, Expression<Func<T, TProperty>> memberSelector, [NotNullWhen(true)]out string? value)
		where T : DependencyObject
	{
		var property = memberSelector.Body switch
		{
			MemberExpression m => m.Member.Name,

			_ => throw new ArgumentOutOfRangeException(memberSelector.Body.Type.ToString()),
		};
		if (e.Attribute(property) is { } attribute)
		{
			if (IKeyedResource.TryParseResource(attribute.Value) is { } resRef)
			{
				obj.SetDP(property, resRef);
			}
			else
			{
				value = attribute.Value;
				return true;
			}
		}

		value = default;
		return false;
	}
}
