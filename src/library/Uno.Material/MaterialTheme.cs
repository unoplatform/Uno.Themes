using System;
using Uno.Themes;


#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Material Theme resources including colors, fonts, layout values, and styles
	/// </summary>
	public class MaterialTheme : BaseTheme
	{
		public MaterialTheme()
		{ }

		public MaterialTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
			: base(colorOverride, fontOverride)
		{ }

		protected override (ResourceDictionary mergedPages, ResourceDictionary[] resources) GenerateSpecificResources()
		{
			var mergedPages = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml") };

			var fonts = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/Common/Fonts.xaml") };

			if (FontOverrideDictionary is { } fontOverride)
			{
				fonts.SafeMerge(fontOverride);
			}

			return (mergedPages, new[] { fonts });
		}
	}
}
