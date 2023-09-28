using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class ProgressRing
		{
			public static class Resources
			{
				public static class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "ProgressRingForeground")]
					public static ThemeResourceKey<Brush> Default => new("ProgressRingForeground");
				}

				public static class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "ProgressRingBackground")]
					public static ThemeResourceKey<Brush> Default => new("ProgressRingBackground");
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressRingStyle", TargetType = typeof(ProgressRing))]
				public static StaticResourceKey<Style> Default => new("ProgressRingStyle");
			}
		}
	}
}
