using System.Xml.Linq;
using Uno.Markup.Xaml.Parsers;
using Uno.Markup.Xaml.UI.Xaml.Media.Animation;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Xaml.UI.Xaml;

public record VisualState(string? Name)
{
	public Storyboard? Storyboard { get; private set; }
	public List<Setter> Setters { get; private set; } = new();

	public static VisualState Parse(XElement e)
	{
		var result = new VisualState(e.Attribute(X + "Name")?.Value);

		foreach (var child in e.Elements())
		{
			if (child.Name.IsMemberOf<VisualState>(out var memberName))
			{
				if (memberName == "Setters")
				{
					result.Setters.AddRange(ParseSetters(child));
				}
			}
			else if (child.Name.Is("Storyboard"))
			{
				result.Storyboard = Storyboard.Parse(child);
			}
			else
			{
				throw new NotImplementedException($"Unexpected {nameof(VisualState)} child element: {child.Name}").PreDump(child);
			}
		}

		return result;
	}
	public static IEnumerable<Setter> ParseSetters(XElement setters)
	{
		foreach (var child in setters.Elements())
		{
			var item = ScuffedXamlParser.Parse(child);
			if (item is Ignorable) continue;
			if (item is Setter setter)
			{
				yield return setter;
			}
			else
			{
				throw new NotImplementedException($"Unexpected {setters.Name.LocalName} child element: {child.Name}").PreDump(child);
			}
		}
	}
}
