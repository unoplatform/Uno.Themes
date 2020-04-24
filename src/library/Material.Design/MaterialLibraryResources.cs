using System;
using Windows.UI.Xaml;

namespace Material.Design
{
	public sealed class MaterialLibraryResources : ResourceDictionary
	{
		public MaterialLibraryResources()
		{
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Material.Design/Styles/_Styles.xaml") });
		}
	}
}