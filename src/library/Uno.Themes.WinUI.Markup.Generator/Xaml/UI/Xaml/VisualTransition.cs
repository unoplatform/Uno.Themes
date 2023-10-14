using System.Xml.Linq;
using Uno.Markup.Xaml.UI.Xaml.Media.Animation;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Xaml.UI.Xaml;

public record VisualTransition(string? Name, string? From, string? To)
{
	public Storyboard? Storyboard { get; private set; }

	public static VisualTransition Parse(XElement e)
	{
		var result = new VisualTransition(e.Attribute(X + "Name")?.Value, e.Attribute("From")?.Value, e.Attribute("To")?.Value);

		foreach (var child in e.Elements())
		{
			if (child.Name.Is("Storyboard"))
			{
				result.Storyboard = Storyboard.Parse(child);
			}
			else
			{
				throw new NotImplementedException($"Unexpected {nameof(VisualTransition)} child element: {child.Name}").PreDump(child);
			}
		}

		return result;
	}
}
