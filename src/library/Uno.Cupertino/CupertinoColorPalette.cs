using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Uno.Cupertino
{
	public sealed class CupertinoColorPalette : ResourceDictionary
	{
		public CupertinoColorPalette()
		{
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/ColorPalette.xaml") });
		}
	}
}
