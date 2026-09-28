using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	/// <summary>Provides semantic style resources for MediaTransportControls.</summary>
	public static partial class MediaTransportControls
	{
		/// <summary>Provides semantic MediaTransportControls styles.</summary>
		public static partial class Styles
		{
			/// <summary>Gets the default semantic MediaTransportControls style.</summary>
			[ResourceKeyDefinition(typeof(Style), "MediaTransportControlsStyle", TargetType = typeof(global::Microsoft.UI.Xaml.Controls.MediaTransportControls))]
			public static StaticResourceKey<Style> Default => new("MediaTransportControlsStyle");
		}
	}
}
