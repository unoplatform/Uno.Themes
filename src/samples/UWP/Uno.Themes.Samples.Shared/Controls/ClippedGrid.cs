using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
