using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Themes;

namespace Uno.Simple.Markup;

public static class MarkupInit
{
	public static ResourceDictionaryBuilder UseSimple(
		this ResourceDictionaryBuilder builder,
		ResourceDictionary colorOverride = null,
		ResourceDictionary fontOverride = null)
		=> builder.Merged(
			new SimpleTheme()
				.ColorOverrideDictionary(colorOverride)
				.FontOverrideDictionary(fontOverride)
			);
}
