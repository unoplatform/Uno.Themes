using System;
using System.ComponentModel;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino;

/// <summary>
/// The frozen pre-<see cref="CupertinoTheme"/> Cupertino fonts, instantiated by
/// <see cref="CupertinoResourcesV1"/>. Honors the override recorded by a <see cref="CupertinoFonts"/>
/// declared before it.
/// </summary>
[Obsolete("Frozen pre-CupertinoTheme resources kept as a migration escape hatch. Use CupertinoTheme. This type will be removed in the next major version.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class CupertinoFontsV1 : ResourceDictionary
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoFontsV1"/> class.
	/// </summary>
	public CupertinoFontsV1()
	{
		MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(CupertinoConstants.V1.Fonts) });

#pragma warning disable CS0618 // The obsolete recorder is the only channel carrying the legacy override.
		var overrideSource = CupertinoFonts.RecordedOverrideSource;
#pragma warning restore CS0618
		if (!string.IsNullOrWhiteSpace(overrideSource))
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(overrideSource) });
		}
	}
}
