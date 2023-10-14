namespace Uno.Markup.Xaml.UI.Xaml;

public record CornerRadius(double TopLeft, double TopRight, double BottomRight, double BottomLeft)
{
	public override string ToString()
	{
		// format: uniform, [left,top,right,bottom]
		if (TopLeft == TopRight && TopRight == BottomRight && BottomRight == BottomLeft) return $"{TopLeft:0.#}";
		return $"{TopLeft:0.#},{TopRight:0.#},{BottomRight:0.#},{BottomLeft:0.#}";
	}
}
