namespace Uno.Markup.Xaml.UI.Xaml;
public record Thickness(double Left, double Top, double Right, double Bottom)
{
	public override string ToString()
	{
		// format: uniform, [same-left-right,same-top-bottom], [left,top,right,bottom]
		if (Left == Top && Top == Right && Right == Bottom) return $"{Left:0.#}";
		if (Left == Right && Top == Bottom) return $"{Left:0.#},{Top:0.#}";
		return $"{Left:0.#},{Top:0.#},{Right:0.#},{Bottom:0.#}";
	}
}
