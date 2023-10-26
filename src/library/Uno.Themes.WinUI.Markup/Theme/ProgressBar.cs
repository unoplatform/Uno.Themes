﻿using Microsoft.UI.Xaml;
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
			public static partial class Default
			{
				public static partial class Background
				{
					[ResourceKeyDefinition(typeof(Brush), "ProgressBarBackground")]
					public static ThemeResourceKey<Brush> Default => new("ProgressBarBackground");
				}

				public static partial class Foreground
				{
					[ResourceKeyDefinition(typeof(Brush), "ProgressBarForeground")]
					public static ThemeResourceKey<Brush> Default => new("ProgressBarForeground");
				}

				[ResourceKeyDefinition(typeof(double), "ProgressBarHeight")]
				public static ThemeResourceKey<double> Height => new("ProgressBarHeight");

				[ResourceKeyDefinition(typeof(double), "ProgressBarMinWidth")]
				public static ThemeResourceKey<double> MinWidth => new("ProgressBarMinWidth");
			}
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "ProgressBarStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ProgressBar))]
			public static StaticResourceKey<Style> Default => new("ProgressBarStyle");
		}
	}
}
