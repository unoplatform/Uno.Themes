using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static partial class CalendarView
		{
			public static partial class Resources
			{
				public static partial class Calendar
				{
					[ResourceKeyDefinition(typeof(Brush), "MaterialCalendarBlackoutForeground")]
					public static ThemeResourceKey<Brush> BlackoutForeground => new("MaterialCalendarBlackoutForeground");

					[ResourceKeyDefinition(typeof(Brush), "MaterialCalendarTodayForeground")]
					public static StaticResourceKey<Brush> TodayForeground => new("MaterialCalendarTodayForeground");
				}
			}

			public static partial class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialCalendarViewStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.CalendarView))]
				public static StaticResourceKey<Style> Default => new("MaterialCalendarViewStyle");
			}
		}
	}
}
