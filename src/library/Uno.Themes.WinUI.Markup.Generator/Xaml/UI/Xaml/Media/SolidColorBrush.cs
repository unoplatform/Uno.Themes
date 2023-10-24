namespace Uno.Markup.Xaml.UI.Xaml.Media;

public record SolidColorBrush(Color? Color = default, double Opacity = 1) : DependencyObject
{
	public override string ToString()
	{
		var color = GetDP(nameof(Color)) switch
		{
			IResourceRef rf => rf.ResourceKey,
			null => Color?.ToString(),
			_ => throw new ArgumentOutOfRangeException(),
		};
		var opacity = GetDP(nameof(Opacity)) switch
		{
			IResourceRef rf => $"*{rf.ResourceKey}",
			null when Opacity != 1 => $"*{Opacity}",
			null => "",
			_ => throw new ArgumentOutOfRangeException(),
		};

		return $"{color}{opacity}";
	}
}
