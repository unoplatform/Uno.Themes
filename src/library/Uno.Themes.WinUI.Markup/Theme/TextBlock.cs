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
	public static partial class TextBlock
	{
		public static partial class Resources
		{
		}

		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MaterialDisplayLarge", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> DisplayLarge => new("MaterialDisplayLarge");

			[ResourceKeyDefinition(typeof(Style), "MaterialDisplayMedium", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> DisplayMedium => new("MaterialDisplayMedium");

			[ResourceKeyDefinition(typeof(Style), "MaterialDisplaySmall", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> DisplaySmall => new("MaterialDisplaySmall");

			[ResourceKeyDefinition(typeof(Style), "MaterialHeadlineLarge", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> HeadlineLarge => new("MaterialHeadlineLarge");

			[ResourceKeyDefinition(typeof(Style), "MaterialHeadlineMedium", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> HeadlineMedium => new("MaterialHeadlineMedium");

			[ResourceKeyDefinition(typeof(Style), "MaterialHeadlineSmall", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> HeadlineSmall => new("MaterialHeadlineSmall");

			[ResourceKeyDefinition(typeof(Style), "MaterialTitleLarge", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> TitleLarge => new("MaterialTitleLarge");

			[ResourceKeyDefinition(typeof(Style), "MaterialTitleMedium", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> TitleMedium => new("MaterialTitleMedium");

			[ResourceKeyDefinition(typeof(Style), "MaterialTitleSmall", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> TitleSmall => new("MaterialTitleSmall");

			[ResourceKeyDefinition(typeof(Style), "MaterialLabelLarge", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> LabelLarge => new("MaterialLabelLarge");

			[ResourceKeyDefinition(typeof(Style), "MaterialLabelMedium", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> LabelMedium => new("MaterialLabelMedium");

			[ResourceKeyDefinition(typeof(Style), "MaterialLabelSmall", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> LabelSmall => new("MaterialLabelSmall");

			[ResourceKeyDefinition(typeof(Style), "MaterialLabelExtraSmall", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> LabelExtraSmall => new("MaterialLabelExtraSmall");

			[ResourceKeyDefinition(typeof(Style), "MaterialBodyLarge", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> BodyLarge => new("MaterialBodyLarge");

			[ResourceKeyDefinition(typeof(Style), "MaterialBodyMedium", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> BodyMedium => new("MaterialBodyMedium");

			[ResourceKeyDefinition(typeof(Style), "MaterialBodySmall", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> BodySmall => new("MaterialBodySmall");

			[ResourceKeyDefinition(typeof(Style), "MaterialCaptionLarge", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> CaptionLarge => new("MaterialCaptionLarge");

			[ResourceKeyDefinition(typeof(Style), "MaterialCaptionMedium", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> CaptionMedium => new("MaterialCaptionMedium");

			[ResourceKeyDefinition(typeof(Style), "MaterialCaptionSmall", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.TextBlock))]
			public static StaticResourceKey<Style> CaptionSmall => new("MaterialCaptionSmall");
		}
	}
}
