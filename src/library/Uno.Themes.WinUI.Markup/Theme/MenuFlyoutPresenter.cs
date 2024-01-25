using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class MenuFlyoutPresenter
	{
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "MenuFlyoutPresenterStyle", TargetType = typeof(MenuFlyoutPresenter))]
			public static StaticResourceKey<Style> Default => new("MenuFlyoutPresenterStyle");
		}
	}
}
