using System;
using Windows.UI.Xaml;

namespace Uno.Material
{
	public sealed class MaterialLibraryResources : ResourceDictionary
	{
		public MaterialLibraryResources()
		{
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/_Styles.xaml") });
		}
	}
}