using System.Xml.Linq;
using Uno.Markup.Xaml.Parsers;

namespace Uno.Markup.Xaml.UI.Xaml.Controls;

public record ControlTemplate(string? TargetType = null)
{
	public VisualTreeElement? TemplateRoot { get; private set; }

	public static ControlTemplate Parse(XElement e)
	{
		var result = new ControlTemplate(e.Attribute("TargetType")?.Value);
		var elements = e.Elements().ToArray();

		result.TemplateRoot = elements.Length switch
		{
			0 => null,
			1 => ScuffedXamlParser.Parse(elements[0]) is { } parsed
				? parsed is VisualTreeElement vte
					? vte : throw new InvalidOperationException($"ControlTemplate can only accept a child of VisualTreeElement: parsed type is {parsed?.GetType().Name ?? "<null>"}").PreDump(parsed)
				: throw new InvalidOperationException(),
			_ => throw new InvalidOperationException($"ControlTemplate > multiple children are present").PreDump(e),
		};

		return result;
	}
}
