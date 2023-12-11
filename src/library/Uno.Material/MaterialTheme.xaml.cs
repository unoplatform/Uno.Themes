using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Material.Helpers;
using Uno.Themes;


#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Material
{
	public sealed partial class MaterialTheme : BaseTheme
	{
		public MaterialTheme()
		{
			this.InitializeComponent();

			Palettes = this.MergedDictionaries.OfType<PaletteCollection>().FirstOrDefault()?.MergedDictionaries;
		}
	}
}
