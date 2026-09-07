using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class CommandBar
	{
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "CommandBarStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.CommandBar))]
			public static StaticResourceKey<Style> Default => new("CommandBarStyle");
		}
	}
}
