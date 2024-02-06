using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Themes;

namespace Uno.Material.Markup;

public static class MarkupInit
{
	public static ResourceDictionaryBuilder UseMaterial(
		this ResourceDictionaryBuilder builder,
		ResourceDictionary colorOverride = null,
		ResourceDictionary fontOverride = null)
		=> builder.Merged(
			new MaterialTheme()
				.ColorOverrideDictionary(colorOverride)
				.FontOverrideDictionary(fontOverride)
			);
}
