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
	public static partial class CalendarDatePicker
	{
		public static partial class Resources
		{
			public static partial class Background
			{
				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBackground")]
				public static ThemeResourceKey<Brush> Default => new("CalendarDatePickerBackground");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBackgroundPointerOver")]
				public static ThemeResourceKey<Brush> PointerOver => new("CalendarDatePickerBackgroundPointerOver");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBackgroundPressed")]
				public static ThemeResourceKey<Brush> Pressed => new("CalendarDatePickerBackgroundPressed");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBackgroundDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("CalendarDatePickerBackgroundDisabled");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBackgroundFocused")]
				public static ThemeResourceKey<Brush> Focused => new("CalendarDatePickerBackgroundFocused");
			}

			public static partial class BorderBrush
			{
				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBorderBrush")]
				public static ThemeResourceKey<Brush> Default => new("CalendarDatePickerBorderBrush");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBorderBrushPointerOver")]
				public static ThemeResourceKey<Brush> PointerOver => new("CalendarDatePickerBorderBrushPointerOver");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBorderBrushPressed")]
				public static ThemeResourceKey<Brush> Pressed => new("CalendarDatePickerBorderBrushPressed");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBorderBrushDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("CalendarDatePickerBorderBrushDisabled");
			}

			public static partial class Header
			{
				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerHeaderForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("CalendarDatePickerHeaderForeground");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerHeaderForegroundDisabled")]
				public static ThemeResourceKey<Brush> ForegroundDisabled => new("CalendarDatePickerHeaderForegroundDisabled");
			}

			public static partial class TextForeground
			{
				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerTextForeground")]
				public static ThemeResourceKey<Brush> Default => new("CalendarDatePickerTextForeground");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerTextForegroundDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("CalendarDatePickerTextForegroundDisabled");

				[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerTextForegroundSelected")]
				public static ThemeResourceKey<Brush> Selected => new("CalendarDatePickerTextForegroundSelected");
			}

			[ResourceKeyDefinition(typeof(CornerRadius), "CalendarDatePickerBackgroundCornerRadius")]
			public static ThemeResourceKey<CornerRadius> BackgroundCornerRadius => new("CalendarDatePickerBackgroundCornerRadius");

			[ResourceKeyDefinition(typeof(double), "CalendarDatePickerBackgroundMinHeight")]
			public static ThemeResourceKey<double> BackgroundMinHeight => new("CalendarDatePickerBackgroundMinHeight");

			[ResourceKeyDefinition(typeof(Thickness), "CalendarDatePickerBorderThemeThickness")]
			public static ThemeResourceKey<Thickness> BorderThemeThickness => new("CalendarDatePickerBorderThemeThickness");

			[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerBottomBorderBrush")]
			public static ThemeResourceKey<Brush> BottomBorderBrush => new("CalendarDatePickerBottomBorderBrush");

			[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerCalendarGlyphForegroundDisabled")]
			public static ThemeResourceKey<Brush> CalendarGlyphForegroundDisabled => new("CalendarDatePickerCalendarGlyphForegroundDisabled");

			[ResourceKeyDefinition(typeof(Thickness), "CalendarDatePickerContentMargin")]
			public static ThemeResourceKey<Thickness> ContentMargin => new("CalendarDatePickerContentMargin");

			[ResourceKeyDefinition(typeof(CornerRadius), "CalendarDatePickerCornerRadius")]
			public static ThemeResourceKey<CornerRadius> CornerRadius => new("CalendarDatePickerCornerRadius");

			[ResourceKeyDefinition(typeof(Brush), "CalendarDatePickerForeground")]
			public static ThemeResourceKey<Brush> Foreground => new("CalendarDatePickerForeground");

			[ResourceKeyDefinition(typeof(double), "CalendarDatePickerHeight")]
			public static ThemeResourceKey<double> Height => new("CalendarDatePickerHeight");
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialCalendarDatePickerStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.CalendarDatePicker))]
			public static StaticResourceKey<Style> Default => new("MaterialCalendarDatePickerStyle");
		}
	}
}
