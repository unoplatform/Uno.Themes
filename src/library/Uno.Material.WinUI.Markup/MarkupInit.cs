using Microsoft.UI.Xaml;

namespace Uno.Material.Markup;

public static class MarkupInit
{
	public static T UseMaterial<T>(this T app,
		ResourceDictionary colorOverride = null,
		ResourceDictionary fontOverride = null) where T : Application
		=> app.Resources(
			r => r.Merged(
				new MaterialTheme()
					.ColorOverrideDictionary(colorOverride)
					.FontOverrideDictionary(fontOverride)
				)
			);
}
