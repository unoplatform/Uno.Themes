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
			=> app.Resources(
				r => r.Merged(
					new MaterialTheme()
						.ColorOverrideDictionary(colorOverride)
						.FontOverrideDictionary(fontOverride)
					)
				);
	}
}
