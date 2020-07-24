using System;
using Windows.UI.Xaml;

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
