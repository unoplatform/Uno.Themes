using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class ProgressBar
		{
			public static class Resources
			{
				[ResourceKeyDefinition(typeof(Brush), "ProgressBarForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("ProgressBarForeground");

				[ResourceKeyDefinition(typeof(Brush), "ProgressBarBackground")]
				public static ThemeResourceKey<Brush> Background => new("ProgressBarBackground");

				[ResourceKeyDefinition(typeof(double), "ProgressBarMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("ProgressBarMinWidth");

				[ResourceKeyDefinition(typeof(double), "ProgressBarHeight")]
				public static ThemeResourceKey<double> Height => new("ProgressBarHeight");
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressBarStyle", TargetType = typeof(ProgressBar))]
				public static StaticResourceKey<Style> Default => new("ProgressBarStyle");
			}
		}
	}
}
