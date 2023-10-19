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
	public static partial class DatePicker
	{
		public static partial class Resources
		{
			public static partial class Button
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackground")]
					public static ThemeResourceKey<Brush> Default => new("DatePickerButtonBackground");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonBackgroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonBackgroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBackgroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonBackgroundDisabled");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForeground")]
					public static ThemeResourceKey<Brush> Default => new("DatePickerButtonForeground");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundPointerOver")]
					public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonForegroundPointerOver");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundPressed")]
					public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonForegroundPressed");

					[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonForegroundDisabled")]
					public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonForegroundDisabled");
				}

				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrush")]
				public static ThemeResourceKey<Brush> BorderBrush => new("DatePickerButtonBorderBrush");
			}

			public static partial class ButtonBorderBrush
			{
				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushPointerOver")]
				public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonBorderBrushPointerOver");

				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushPressed")]
				public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonBorderBrushPressed");

				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonBorderBrushDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonBorderBrushDisabled");
			}

			public static partial class ButtonContent
			{
				[ResourceKeyDefinition(typeof(double), "DatePickerButtonContentHeight")]
				public static ThemeResourceKey<double> Height => new("DatePickerButtonContentHeight");

				[ResourceKeyDefinition(typeof(Thickness), "DatePickerButtonContentMargin")]
				public static ThemeResourceKey<Thickness> Margin => new("DatePickerButtonContentMargin");
			}

			public static partial class ButtonDateTextForeground
			{
				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForeground")]
				public static ThemeResourceKey<Brush> Default => new("DatePickerButtonDateTextForeground");

				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundPointerOver")]
				public static ThemeResourceKey<Brush> PointerOver => new("DatePickerButtonDateTextForegroundPointerOver");

				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundPressed")]
				public static ThemeResourceKey<Brush> Pressed => new("DatePickerButtonDateTextForegroundPressed");

				[ResourceKeyDefinition(typeof(Brush), "DatePickerButtonDateTextForegroundDisabled")]
				public static ThemeResourceKey<Brush> Disabled => new("DatePickerButtonDateTextForegroundDisabled");
			}

			[ResourceKeyDefinition(typeof(double), "DatePickerButtonBottomBorderHeight")]
			public static ThemeResourceKey<double> ButtonBottomBorderHeight => new("DatePickerButtonBottomBorderHeight");

			[ResourceKeyDefinition(typeof(Thickness), "DatePickerButtonHeaderMargin")]
			public static ThemeResourceKey<Thickness> ButtonHeaderMargin => new("DatePickerButtonHeaderMargin");

			[ResourceKeyDefinition(typeof(Thickness), "DatePickerButtonPlaceholderMargin")]
			public static ThemeResourceKey<Thickness> ButtonPlaceholderMargin => new("DatePickerButtonPlaceholderMargin");

			[ResourceKeyDefinition(typeof(CornerRadius), "DatePickerCornerRadius")]
			public static ThemeResourceKey<CornerRadius> CornerRadius => new("DatePickerCornerRadius");

			[ResourceKeyDefinition(typeof(double), "DatePickerHeight")]
			public static ThemeResourceKey<double> Height => new("DatePickerHeight");

			[ResourceKeyDefinition(typeof(Brush), "DatePickerPlaceholderTextForeground")]
			public static ThemeResourceKey<Brush> PlaceholderTextForeground => new("DatePickerPlaceholderTextForeground");
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialDatePickerStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.DatePicker))]
			public static StaticResourceKey<Style> Default => new("MaterialDatePickerStyle");
		}
	}
}
