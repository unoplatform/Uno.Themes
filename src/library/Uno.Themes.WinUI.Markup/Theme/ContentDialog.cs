using System;
using Windows.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

using ResourceKeyDefinitionAttribute = Uno.Themes.WinUI.Markup.ResourceKeyDefinitionAttribute;

namespace Uno.Themes.Markup
{
	public static partial class Theme
	{
		public static partial class ContentDialog
		{
			public static partial class Resources
			{
				[ResourceKeyDefinition(typeof(Brush), "MaterialContentDialogBackground")]
				public static StaticResourceKey<Brush> Background => new("MaterialContentDialogBackground");

				[ResourceKeyDefinition(typeof(Thickness), "MaterialContentDialogCommandSpaceToContentMargin")]
				public static StaticResourceKey<Thickness> CommandSpaceToContentMargin => new("MaterialContentDialogCommandSpaceToContentMargin");

				[ResourceKeyDefinition(typeof(Brush), "MaterialContentDialogContentForeground")]
				public static StaticResourceKey<Brush> ContentForeground => new("MaterialContentDialogContentForeground");

				[ResourceKeyDefinition(typeof(CornerRadius), "MaterialContentDialogCornerRadius")]
				public static StaticResourceKey<CornerRadius> CornerRadius => new("MaterialContentDialogCornerRadius");

				[ResourceKeyDefinition(typeof(Thickness), "MaterialContentDialogEdgeMargin")]
				public static StaticResourceKey<Thickness> EdgeMargin => new("MaterialContentDialogEdgeMargin");

				[ResourceKeyDefinition(typeof(double), "MaterialContentDialogMaxHeight")]
				public static StaticResourceKey<double> MaxHeight => new("MaterialContentDialogMaxHeight");

				[ResourceKeyDefinition(typeof(double), "MaterialContentDialogMaxWidth")]
				public static StaticResourceKey<double> MaxWidth => new("MaterialContentDialogMaxWidth");

				[ResourceKeyDefinition(typeof(double), "MaterialContentDialogMinHeight")]
				public static StaticResourceKey<double> MinHeight => new("MaterialContentDialogMinHeight");

				[ResourceKeyDefinition(typeof(double), "MaterialContentDialogMinWidth")]
				public static StaticResourceKey<double> MinWidth => new("MaterialContentDialogMinWidth");

				[ResourceKeyDefinition(typeof(Thickness), "MaterialContentDialogPanelPadding")]
				public static StaticResourceKey<Thickness> PanelPadding => new("MaterialContentDialogPanelPadding");

				[ResourceKeyDefinition(typeof(Brush), "MaterialContentDialogSmokeLayerBackground")]
				public static StaticResourceKey<Brush> SmokeLayerBackground => new("MaterialContentDialogSmokeLayerBackground");

				[ResourceKeyDefinition(typeof(Brush), "MaterialContentDialogTitleForeground")]
				public static StaticResourceKey<Brush> TitleForeground => new("MaterialContentDialogTitleForeground");

				[ResourceKeyDefinition(typeof(Thickness), "MaterialContentDialogTitleToContentMargin")]
				public static StaticResourceKey<Thickness> TitleToContentMargin => new("MaterialContentDialogTitleToContentMargin");
			}

			public static partial class Styles
			{
				[ResourceKeyDefinition(typeof(Style), "MaterialContentDialogStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ContentDialog))]
				public static StaticResourceKey<Style> Default => new("MaterialContentDialogStyle");
			}
		}
	}
}
