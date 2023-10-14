using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Uno.Markup.Extensions;

public static class XAttributeExtensions
{
	public static string? ResourceKey(this XAttribute attribute)
	{
		var match = Regex.Match(attribute.Value, @"^{(StaticResource|ThemeResource) (?<key>.+)}$");

		return match.Success ? match.Groups["key"].Value : null;
	}
}
