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
	public static partial class ProgressBar
	{
		public static partial class Resources
		{
			public static partial class ProgressBar
			{
				[ResourceKeyDefinition(typeof(Brush), "ProgressBarBackground")]
				public static ThemeResourceKey<Brush> Background => new("ProgressBarBackground");

				[ResourceKeyDefinition(typeof(Brush), "ProgressBarForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("ProgressBarForeground");

				[ResourceKeyDefinition(typeof(double), "ProgressBarHeight")]
				public static ThemeResourceKey<double> Height => new("ProgressBarHeight");

				[ResourceKeyDefinition(typeof(double), "ProgressBarMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("ProgressBarMinWidth");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialProgressBarStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ProgressBar))]
			public static StaticResourceKey<Style> Default => new("MaterialProgressBarStyle");
		}
	}
}
