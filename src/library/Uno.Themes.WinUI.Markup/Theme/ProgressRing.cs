using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
					public static ResourceValue<Brush> Default => new("ProgressRingForeground", true);
				}

				public static class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "ProgressRingBackground")]
					public static ResourceValue<Brush> Default => new("ProgressRingBackground", true);
				}
			}

			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "ProgressRingStyle", TargetType = typeof(ProgressRing))]
				public static ResourceValue<Style> Default => new("ProgressRingStyle");
			}
		}
	}
}
