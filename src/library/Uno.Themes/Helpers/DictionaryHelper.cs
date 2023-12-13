using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes.Helpers;

internal class DictionaryHelper
{
	internal static object CreateLazyResource(Func<Style> styleSelector)
	{
#if HAS_UNO
		return new ResourceDictionary.ResourceInitializer(styleSelector);
#else
		return styleSelector();
#endif
	}
}
