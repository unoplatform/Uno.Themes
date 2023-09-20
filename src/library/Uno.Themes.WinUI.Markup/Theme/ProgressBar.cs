using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
				public static ResourceValue<Brush> Foreground => new("ProgressBarForeground", true);

				[ResourceKeyDefinition(typeof(Brush), "ProgressBarBackground")]
				public static ResourceValue<Brush> Background => new("ProgressBarBackground", true);

				[ResourceKeyDefinition(typeof(double), "ProgressBarMinWidth")]
				public static ResourceValue<double> MinWidth => new("ProgressBarMinWidth", true);

				[ResourceKeyDefinition(typeof(double), "ProgressBarHeight")]
				public static ResourceValue<double> Height => new("ProgressBarHeight", true);
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressBarStyle", TargetType = typeof(ProgressBar))]
				public static ResourceValue<Style> Default => new("ProgressBarStyle");
			}
		}
	}
}
