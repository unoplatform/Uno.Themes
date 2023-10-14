using System.Xml.Linq;
using Uno.Markup.Xaml.Parsers;
using static Uno.Markup.Xaml.XamlNamespaces;

namespace Uno.Markup.Xaml.UI.Xaml;

public record VisualStateGroup(string? Name)
{
	public List<VisualState> VisualStates { get; } = new();
	public List<VisualTransition> Transitions { get; } = new();

	public static VisualStateGroup Parse(XElement e)
	{
		var result = new VisualStateGroup(e.Attribute(X + "Name")?.Value);
		foreach (var child in e.Elements())
		{
			if (child.Name.Is<VisualState>())
			{
				result.VisualStates.Add(VisualState.Parse(child));
			}
			else if (child.Name.IsMemberOf<VisualStateGroup>(nameof(Transitions)))
			{
				result.Transitions.AddRange(ParseTransitions(child));
			}
			else
			{
				throw new NotImplementedException($"Unexpected {nameof(VisualStateGroup)} child element: {child.Name}").PreDump(child);
			}
		}

		return result;
	}
	public static IEnumerable<VisualTransition> ParseTransitions(XElement transitions)
	{
		foreach (var child in transitions.Elements())
		{
			var item = ScuffedXamlParser.Parse(child);
			if (item is VisualTransition transition)
			{
				yield return transition;
			}
			else
			{
				throw new NotImplementedException($"Unexpected {transitions.Name.LocalName} child element: {child.Name}").PreDump(child);
			}
		}
	}
}
