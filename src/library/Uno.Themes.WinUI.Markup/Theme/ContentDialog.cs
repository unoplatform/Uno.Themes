using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class ContentDialog
	{
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "ContentDialogStyle", TargetType = typeof(ContentDialog))]
			public static StaticResourceKey<Style> Default => new("ContentDialogStyle");
		}
	}
}
