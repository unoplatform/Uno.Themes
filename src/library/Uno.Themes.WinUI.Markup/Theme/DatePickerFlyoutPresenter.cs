using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>Provides semantic style resources for DatePickerFlyoutPresenter.</summary>
	public static partial class DatePickerFlyoutPresenter
	{
		/// <summary>Provides semantic DatePickerFlyoutPresenter styles.</summary>
		public static partial class Styles
		{
			/// <summary>Gets the default semantic DatePickerFlyoutPresenter style.</summary>
			[ResourceKeyDefinition(typeof(Style), "DatePickerFlyoutPresenterStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.DatePickerFlyoutPresenter))]
			public static StaticResourceKey<Style> Default => new("DatePickerFlyoutPresenterStyle");
		}
	}
}
