using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class ProgressRing
	{
		public static partial class Resources
		{
			public static partial class Default
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "ProgressRingBackground")]
					public static ThemeResourceKey<Brush> Default => new("ProgressRingBackground");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "ProgressRingForeground")]
					public static ThemeResourceKey<Brush> Default => new("ProgressRingForeground");
				}
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "ProgressRingStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ProgressRing))]
			public static StaticResourceKey<Style> Default => new("ProgressRingStyle");
		}
	}
}
