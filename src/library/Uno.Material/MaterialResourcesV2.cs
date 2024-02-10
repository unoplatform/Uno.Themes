using System;
using Windows.Foundation.Metadata;
using Uno.Themes;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material;

/// <summary>
/// Material resources including colors, layout values and styles
/// </summary>
[Deprecated("Resource initialization for the Uno.Material theme should now be done using the MaterialTheme class instead.", DeprecationType.Deprecate, 3)]
public class MaterialResourcesV2 : ResourceDictionary
{
	public MaterialResourcesV2()
	{
		Source = new Uri(MaterialConstants.ResourcePaths.Version2.MergedPages);
		MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(ThemesConstants.ConverterResourcePath) });
		MergedDictionaries.Add(new MaterialFonts());
		MergedDictionaries.Add(new MaterialColorsV2());
	}
}
