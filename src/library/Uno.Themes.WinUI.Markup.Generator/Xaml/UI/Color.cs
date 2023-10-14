namespace Uno.Markup.Xaml.UI;

public record Color(byte A, byte R, byte G, byte B)
{
	public override string ToString() => "#" + this.ToRgbText();
}
