using System;
using System.Collections.Generic;
using System.ComponentModel;
using Uno.Themes.Helpers;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino;

/// <summary>
/// The frozen pre-<see cref="CupertinoTheme"/> Cupertino colors and brushes, instantiated by
/// <see cref="CupertinoResourcesV1"/>. Honors the override recorded by a <see cref="CupertinoColors"/>
/// declared before it.
/// </summary>
[Obsolete("Frozen pre-CupertinoTheme resources kept as a migration escape hatch. Use CupertinoTheme. This type will be removed in the next major version.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed partial class CupertinoColorsV1 : ResourceDictionary
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoColorsV1"/> class.
	/// </summary>
	public CupertinoColorsV1()
	{
		// The palette (then the override) must be merged before InitializeComponent: the brushes read
		// their colors with {StaticResource} while this dictionary is parsed.
		var layers = new List<ResourceDictionary> { new ResourceDictionary { Source = new Uri(CupertinoConstants.V1.ColorPalette) } };
#pragma warning disable CS0618 // The obsolete recorder is the only channel carrying the legacy override.
		var overrideSource = CupertinoColors.RecordedOverrideSource;
#pragma warning restore CS0618
		if (!string.IsNullOrWhiteSpace(overrideSource))
		{
			layers.Add(new ResourceDictionary { Source = new Uri(overrideSource) });
		}

		foreach (var layer in layers)
		{
			MergedDictionaries.Add(layer);
		}

		InitializeComponent();

		// {StaticResource} in this dictionary resolves against the application scope before its own merged
		// palette, and an app migrating through V1 usually has the new palette there. Paint the brushes from
		// the frozen layers explicitly so "frozen" holds whatever the ambient scope contains.
		SemanticBrushUpdater.Apply(this, layers, CupertinoConstants.BrushColorKeys);
	}
}
