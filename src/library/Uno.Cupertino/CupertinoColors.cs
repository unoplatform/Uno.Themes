using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino;

/// <summary>
/// Legacy entry point for a Cupertino color override. It no longer carries any resource: it records
/// <see cref="OverrideSource"/> for a <see cref="CupertinoResources"/> declared after it.
/// </summary>
[Obsolete("Use CupertinoTheme with ColorOverrideSource instead. This type will be removed in a future version.")]
public sealed class CupertinoColors : ResourceDictionary
{
	// Process-wide on purpose: the legacy App.xaml setup declares the three dictionaries as siblings, so the
	// only channel from this one to CupertinoResources is the order they are constructed in.
	internal static string RecordedOverrideSource { get; private set; }

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
