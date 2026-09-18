using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino;

/// <summary>
/// Legacy entry point for a Cupertino font override: it records <see cref="OverrideSource"/> for a
/// <see cref="CupertinoResources"/> declared after it. It still carries the default Cupertino fonts.
/// </summary>
[Obsolete("Use CupertinoTheme with DefaultFontFamily or FontOverrideSource instead. This type will be removed in a future version.")]
public sealed class CupertinoFonts : ResourceDictionary
{
	// Process-wide on purpose, see CupertinoColors.RecordedOverrideSource.
	internal static string RecordedOverrideSource { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoFonts"/> class.
	/// </summary>
	public CupertinoFonts()
	{
		// See CupertinoColors: a recorder without an OverrideSource must not inherit an earlier one.
		RecordedOverrideSource = null;

		MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(CupertinoConstants.Fonts) });
	}

	/// <summary>
	/// (Optional) Gets or sets the URI of a dictionary overriding Cupertino fonts.
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
			typeof(CupertinoFonts),
			new PropertyMetadata(null, OnFontOverrideSourcePropertyChanged));

	private static void OnFontOverrideSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		RecordedOverrideSource = args.NewValue as string;
	}
}
