using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>Provides semantic style resources for RadioMenuFlyoutItem.</summary>
	public static partial class RadioMenuFlyoutItem
	{
		/// <summary>Provides semantic RadioMenuFlyoutItem styles.</summary>
		public static partial class Styles
		{
			/// <summary>Gets the default semantic RadioMenuFlyoutItem style.</summary>
			[ResourceKeyDefinition(typeof(Style), "RadioMenuFlyoutItemStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.RadioMenuFlyoutItem))]
			public static StaticResourceKey<Style> Default => new("RadioMenuFlyoutItemStyle");
		}
	}
}
