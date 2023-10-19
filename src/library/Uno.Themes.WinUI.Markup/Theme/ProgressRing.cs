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
	public static partial class ProgressRing
	{
		public static partial class Resources
		{
			public static partial class ProgressRing
			{
				[ResourceKeyDefinition(typeof(Brush), "ProgressRingBackground")]
				public static ThemeResourceKey<Brush> Background => new("ProgressRingBackground");

				[ResourceKeyDefinition(typeof(Brush), "ProgressRingForeground")]
				public static ThemeResourceKey<Brush> Foreground => new("ProgressRingForeground");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialProgressRingStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ProgressRing))]
			public static StaticResourceKey<Style> Default => new("MaterialProgressRingStyle");
		}
	}
}
