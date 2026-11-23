namespace Uno.Themes.Samples;

public partial class ClippedGrid : Grid
{
	public ClippedGrid()
	{
		Loaded += (s, e) => UpdateClippingArea();
		SizeChanged += (s, e) => UpdateClippingArea();
	}

	private void UpdateClippingArea()
	{
		var rect = new Rect(0, 0, ActualWidth, ActualHeight);

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
