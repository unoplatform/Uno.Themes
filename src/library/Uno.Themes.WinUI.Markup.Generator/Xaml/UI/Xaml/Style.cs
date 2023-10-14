using System.Xml.Linq;
using Uno.Markup.Xaml.Parsers;
using Uno.Markup.Xaml.UI.Xaml.Controls;

namespace Uno.Markup.Xaml.UI.Xaml;

public record Style(string? TargetType = null, string? BasedOn = null)
{
	public List<Setter> Setters = new();

#if LINQPAD
	private object ToDump() => new { TargetType, BasedOn, Setters };
#endif

	public static Style Parse(XElement e)
	{
		var result = new Style(
			e.Attribute("TargetType")?.Value,
			IKeyedResource.GetKeyFromMarkup(e.Attribute("BasedOn")?.Value)
		);
		ParseSetters(e.Elements(), wrapped: false);

		return result;

		void ParseSetters(IEnumerable<XElement> children, bool wrapped)
		{
			foreach (var child in children)
			{
				if (child.Name.IsMemberOf<Style>(nameof(Setters)) && !wrapped)
				{
					ParseSetters(child.Elements(), wrapped: true);
					continue;
				}

				var parsed = ScuffedXamlParser.Parse(child);
				if (parsed is Ignorable) continue;
				if (parsed is Setter setter)
				{
					result.Setters.Add(setter);
				}
				else
				{
					throw new NotImplementedException($"{(wrapped ? "Style > Style.Setters" : "Style > ")}{child.Name.Pretty()}").PreDump(child);
				}
			}
		}
	}

	public VisualTreeElement? GetTemplateRoot()
	{
		return Setters
			.FirstOrDefault(x => x.Property == "Template")
			?.Value.As<ControlTemplate>()
			?.TemplateRoot;
	}
}
