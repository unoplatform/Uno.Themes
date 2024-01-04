using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
#if IS_WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Themes.Samples
{
	public partial class ClippedGrid : Grid
	{
		public ClippedGrid()
		{
			Loaded += (s, e) => UpdateClippingArea();
			SizeChanged += (s, e) => UpdateClippingArea();
		}

		private void UpdateClippingArea()
		{
			var rect = new Rect(0, 0, ActualHeight, ActualWidth);

			if (Clip != null)
			{
				Clip.Rect = rect;
			}
			else
			{
				Clip = new RectangleGeometry { Rect = rect };
			}
		}
	}
}
