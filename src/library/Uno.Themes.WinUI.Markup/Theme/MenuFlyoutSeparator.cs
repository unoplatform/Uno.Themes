using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>Provides semantic style resources for MenuFlyoutSeparator.</summary>
	public static partial class MenuFlyoutSeparator
	{
		/// <summary>Provides semantic MenuFlyoutSeparator styles.</summary>
		public static partial class Styles
		{
			/// <summary>Gets the default semantic MenuFlyoutSeparator style.</summary>
			[ResourceKeyDefinition(typeof(Style), "MenuFlyoutSeparatorStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.MenuFlyoutSeparator))]
			public static StaticResourceKey<Style> Default => new("MenuFlyoutSeparatorStyle");
		}
	}
}
