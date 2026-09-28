using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>Provides semantic style resources for ComboBoxItem.</summary>
	public static partial class ComboBoxItem
	{
		/// <summary>Provides semantic ComboBoxItem styles.</summary>
		public static partial class Styles
		{
			/// <summary>Gets the default semantic ComboBoxItem style.</summary>
			[ResourceKeyDefinition(typeof(Style), "ComboBoxItemStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.ComboBoxItem))]
			public static StaticResourceKey<Style> Default => new("ComboBoxItemStyle");
		}
	}
}
