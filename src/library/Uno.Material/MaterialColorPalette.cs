using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Default Material Color Palette
	/// </summary>
	public sealed class MaterialColorPalette : ResourceDictionary
	{
		public MaterialColorPalette()
		{
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/ColorPalette.xaml") });
		}
	}
}
