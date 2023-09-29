using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static class TextBlock
		{
			public static class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "DisplayLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> DisplayLarge => new("DisplayLarge");

				[ResourceKeyDefinition(typeof(Style), "DisplayMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> DisplayMedium => new("DisplayMedium");

				[ResourceKeyDefinition(typeof(Style), "DisplaySmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> DisplaySmall => new("DisplaySmall");

				[ResourceKeyDefinition(typeof(Style), "HeadlineLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> HeadlineLarge => new("HeadlineLarge");

				[ResourceKeyDefinition(typeof(Style), "HeadlineMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> HeadlineMedium => new("HeadlineMedium");

				[ResourceKeyDefinition(typeof(Style), "HeadlineSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> HeadlineSmall => new("HeadlineSmall");

				[ResourceKeyDefinition(typeof(Style), "TitleLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> TitleLarge => new("TitleLarge");

				[ResourceKeyDefinition(typeof(Style), "TitleMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> TitleMedium => new("TitleMedium");

				[ResourceKeyDefinition(typeof(Style), "TitleSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> TitleSmall => new("TitleSmall");

				[ResourceKeyDefinition(typeof(Style), "LabelLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelLarge => new("LabelLarge");

				[ResourceKeyDefinition(typeof(Style), "LabelMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelMedium => new("LabelMedium");

				[ResourceKeyDefinition(typeof(Style), "LabelSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelSmall => new("LabelSmall");

				[ResourceKeyDefinition(typeof(Style), "LabelExtraSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> LabelExtraSmall => new("LabelExtraSmall");

				[ResourceKeyDefinition(typeof(Style), "BodyLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> BodyLarge => new("BodyLarge");

				[ResourceKeyDefinition(typeof(Style), "BodyMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> BodyMedium => new("BodyMedium");

				[ResourceKeyDefinition(typeof(Style), "BodySmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> BodySmall => new("BodySmall");

				[ResourceKeyDefinition(typeof(Style), "CaptionLarge", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> CaptionLarge => new("CaptionLarge");

				[ResourceKeyDefinition(typeof(Style), "CaptionMedium", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> CaptionMedium => new("CaptionMedium");

				[ResourceKeyDefinition(typeof(Style), "CaptionSmall", TargetType = typeof(TextBlock))]
				public static StaticResourceKey<Style> CaptionSmall => new("CaptionSmall");
			}
		}
	}
}
