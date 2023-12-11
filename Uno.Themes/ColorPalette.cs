using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endif
using Windows.UI;

namespace Uno.Themes
{
	public class ColorPalette : ResourceDictionary
	{
		public Color Primary { get; set; }

		public Color Background { get; set; }

		public Color Surface { get; set; }
	}
}
