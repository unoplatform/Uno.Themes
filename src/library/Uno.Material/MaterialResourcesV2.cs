using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Material.Helpers;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Material resources including colors, layout values and styles
	/// </summary>
	public class MaterialResourcesV2 : ResourceDictionary
	{
		private static MaterialColorsV2 _colors;
		public static MaterialColorsV2 Colors => _colors;

		public MaterialResourcesV2()
		{
			Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml");

			MergedDictionaries.Add(_colors = new MaterialColorsV2());
		}
	}
}
