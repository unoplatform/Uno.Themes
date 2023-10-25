using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class AppBarButton
	{
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "AppBarButtonStyle", TargetType = typeof(AppBarButton))]
			public static StaticResourceKey<Style> Default => new("AppBarButtonStyle");
		}
	}
}
