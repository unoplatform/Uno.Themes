using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>Provides semantic style resources for ToggleMenuFlyoutItem.</summary>
	public static partial class ToggleMenuFlyoutItem
	{
		/// <summary>Provides semantic ToggleMenuFlyoutItem styles.</summary>
		public static partial class Styles
		{
			/// <summary>Gets the default semantic ToggleMenuFlyoutItem style.</summary>
			[ResourceKeyDefinition(typeof(Style), "ToggleMenuFlyoutItemStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ToggleMenuFlyoutItem))]
			public static StaticResourceKey<Style> Default => new("ToggleMenuFlyoutItemStyle");
		}
	}
}
