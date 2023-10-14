using Uno.Markup.Misc;

namespace Uno.Markup.Xaml.UI.Xaml;

public record DependencyObject
{
	private EquitableDictionary<string, object?> _properties = new();

	public object? GetDP(string dp) => _properties.TryGetValue(dp, out var value) ? value : default;
	public void SetDP(string dp, object? value) => _properties[dp] = value;
}
