using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>Provides semantic style resources for MenuFlyoutSubItem.</summary>
	public static partial class MenuFlyoutSubItem
	{
		/// <summary>Provides semantic MenuFlyoutSubItem styles.</summary>
		public static partial class Styles
		{
			/// <summary>Gets the default semantic MenuFlyoutSubItem style.</summary>
			[ResourceKeyDefinition(typeof(Style), "MenuFlyoutSubItemStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.MenuFlyoutSubItem))]
			public static StaticResourceKey<Style> Default => new("MenuFlyoutSubItemStyle");
		}
	}
}
