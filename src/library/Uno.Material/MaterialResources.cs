using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Themes.Common;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public sealed class MaterialResources : ResourceDictionary
	{
		public MaterialResources()
		{
			this.MergedDictionaries.Add(new Colors());
		}
    }
}
