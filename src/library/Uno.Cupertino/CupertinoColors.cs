using System;
using Uno.Themes.Helpers;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino;

/// <summary>
/// Legacy entry point for a Cupertino color override: it records <see cref="OverrideSource"/> for a
/// <see cref="CupertinoResources"/> declared after it. It still carries the default Cupertino colors and
/// brushes, so a dictionary that merges it to resolve <c>{StaticResource Cupertino*Brush}</c> keeps working.
/// </summary>
[Obsolete("Use CupertinoTheme with Colors.OverrideSource instead. This type will be removed in a future version.")]
public sealed class CupertinoColors : ResourceDictionary
{
	// Process-wide on purpose: the legacy App.xaml setup declares the three dictionaries as siblings, so the
	// only channel from this one to CupertinoResources is the order they are constructed in.
	internal static string RecordedOverrideSource { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoColors"/> class.
	/// </summary>
	public CupertinoColors()
	{
		// A recorder declared without an OverrideSource never fires the property-changed callback, so without
		// this an earlier recorder's URI would survive into the next CupertinoResources.
		RecordedOverrideSource = null;

		var palette = new ResourceDictionary { Source = new Uri(CupertinoConstants.ColorPalette) };
		var brushes = new ResourceDictionary { Source = new Uri(CupertinoConstants.Brushes) };
		SemanticBrushUpdater.Apply(brushes, new[] { palette }, CupertinoConstants.BrushColorKeys);
		MergedDictionaries.Add(palette);
		MergedDictionaries.Add(brushes);
	}

	/// <summary>
	/// (Optional) Gets or sets the URI of a dictionary overriding Cupertino colors.
	/// </summary>
	public string OverrideSource
	{
		get => (string)GetValue(OverrideSourceProperty);
		set => SetValue(OverrideSourceProperty, value);
	}

	/// <summary>Identifies the <see cref="OverrideSource"/> dependency property.</summary>
	public static DependencyProperty OverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(OverrideSource),
			typeof(string),
			typeof(CupertinoColors),
			new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

	private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		RecordedOverrideSource = args.NewValue as string;
	}
}
