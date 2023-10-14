using System.Xml.Linq;
using Uno.Markup.Xaml.Parsers;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Xaml.UI.Xaml;

public record Setter(string? Target, string? Property, object? Value)
{
	public static Setter Parse(XElement e)
	{
		return new Setter
		(
			Property: e.Attribute("Property")?.Value,
			Target: e.Attribute("Target")?.Value,
			Value: GetDirectOrNestedValue()
		);

		object? GetDirectOrNestedValue()
		{
			if (e.Element(Presentation + "Setter.Value") is { } valueMember && valueMember.HasElements)
			{
				return ScuffedXamlParser.Parse(valueMember.Elements().Single());
			}
			if (e.Attribute("Value") is { Value: { Length: > 0 } value })
			{
				return ScuffedXamlParser.ParseInlineValue(value);
			}

			return null;
		}
	}
}
