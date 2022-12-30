using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;

namespace Uno.Material
{
	public static class MarkupInit
	{
		public static T UseMaterial<T>(this T app,
			ResourceDictionary colorOverride = null,
			ResourceDictionary fontOverride = null) where T : Application
		{
			app.Resources(r => r.Merged(
				colorOverride is { } ? new MaterialColors().OverrideDictionary(colorOverride) : new MaterialColors(),
				fontOverride is { } ? new MaterialFonts().OverrideDictionary(fontOverride) : new MaterialFonts(),
				new MaterialResources())
			);

			return app;
		}
	}
}
