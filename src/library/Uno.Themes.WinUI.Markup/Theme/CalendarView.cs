using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class CalendarView
	{
		public static partial class Resources
		{
			[ResourceKeyDefinition(typeof(Brush), "MaterialCalendarViewBlackoutForeground")]
			public static StaticResourceKey<Brush> BlackoutForeground => new("MaterialCalendarViewBlackoutForeground");

			[ResourceKeyDefinition(typeof(Brush), "MaterialCalendarViewTodayForeground")]
			public static StaticResourceKey<Brush> TodayForeground => new("MaterialCalendarViewTodayForeground");
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialCalendarViewStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.CalendarView))]
			public static StaticResourceKey<Style> Default => new("MaterialCalendarViewStyle");
		}
	}
}
