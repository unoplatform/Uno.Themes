using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Uno.Markup.Xaml.UI.Xaml.Media.Animation;


public record Storyboard // doesnt make sense to inherit from Timeline here (while it was the case in uwp)
{
	public List<Timeline> Children { get; } = new();

	public static Storyboard Parse(XElement e)
	{
		var result = new Storyboard();
		foreach (var child in e.Elements())
		{
			if (Timeline.Parse(child) is { } timeline)
			{
				result.Children.Add(timeline);
			}
			else if (child.Name.Is<Storyboard>()) // hack: quick workaround for doubly-nested Storyboard (eg: Uno\Fluent\ProgressBar...)
			{
				return Parse(child);
			}
			else
			{
				throw new NotImplementedException($"Storyboard > {child.Name.Pretty()}").PreDump(child);
			}
		}
		return result;
	}

	public object ToDump() => Children;
}
