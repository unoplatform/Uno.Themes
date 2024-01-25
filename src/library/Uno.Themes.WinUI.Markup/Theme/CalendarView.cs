using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class CalendarView
	{
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "CalendarViewStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.CalendarView))]
			public static StaticResourceKey<Style> Default => new("CalendarViewStyle");
		}
	}
}
