using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.UI.Xaml;
using Uno.Material;

namespace Uno.Material.WinUI.Markup
{
	public static partial class MaterialMarkup
	{
		public static T UseMaterial<T>(this T app,
			ResourceDictionary? colorOverride = null,
			ResourceDictionary? fontOverride = null) where T : Application
		{
			app.Resources(r => r.Merged(
				new MaterialColors().OverrideDictionary(colorOverride),
				new MaterialFonts().OverrideDictionary(fontOverride),
				new MaterialResources())
			);

			return app;
		}
	}
}
