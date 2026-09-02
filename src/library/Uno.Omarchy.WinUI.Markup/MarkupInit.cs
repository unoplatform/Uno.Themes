using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;

namespace Uno.Omarchy.Markup;

public static class MarkupInit
{
	/// <summary>
	/// Merges an <see cref="OmarchyTheme"/> into the application resources.
	/// </summary>
	/// <param name="builder">The resource dictionary builder.</param>
	/// <param name="palette">The palette to apply; <c>null</c> keeps the default (<see cref="OmarchyPalettes.TokyoNight"/>).</param>
	/// <param name="colorOverride">Optional consumer color overrides (<c>*Color</c> keys).</param>
	/// <param name="fontOverride">Optional consumer font overrides (<c>*FontFamily</c> keys).</param>
	public static ResourceDictionaryBuilder UseOmarchy(
		this ResourceDictionaryBuilder builder,
		OmarchyPalette palette = null,
		ResourceDictionary colorOverride = null,
		ResourceDictionary fontOverride = null)
	{
		var theme = new OmarchyTheme(colorOverride, fontOverride);
		if (palette is { })
		{
			theme.Palette = palette;
		}

		return builder.Merged(theme);
	}
}
