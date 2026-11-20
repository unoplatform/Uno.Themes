#nullable enable

using System;
using System.Collections.Generic;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Fluent;

/// <summary>
/// Turns a button using the built-in template into a Fluent "subtle button": the subtle fills on hover
/// and press, and no border. WinUI styles cannot set <see cref="FrameworkElement.Resources"/>, so the
/// button's own state resources are re-pointed from a dictionary merged into its resources.
/// </summary>
internal static class FluentSubtleButton
{
	public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
		"IsEnabled", typeof(bool), typeof(FluentSubtleButton), new PropertyMetadata(false, OnIsEnabledChanged));

	public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

	public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

	private static void OnIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is not FrameworkElement element)
		{
			return;
		}

		try
		{
			// Loaded re-applies the resources: a <Button.Resources> property element replaces the
			// dictionary after the Style attribute has set this property.
			element.Loaded -= OnLoaded;
			if (args.NewValue is true)
			{
				element.Loaded += OnLoaded;
				Apply(element);
			}
			else
			{
				Remove(element.Resources.MergedDictionaries);
			}
		}
		catch (Exception e)
		{
			// Property-changed callback: never let a failure escape into the consumer's layout.
			FluentDiagnostics.LogWarning("FluentTheme could not apply the subtle button resources.", e);
		}
	}

	private static void OnLoaded(object sender, RoutedEventArgs args)
	{
		try
		{
			if (sender is FrameworkElement element)
			{
				Apply(element);
			}
		}
		catch (Exception e)
		{
			FluentDiagnostics.LogWarning("FluentTheme could not apply the subtle button resources.", e);
		}
	}

	private static void Apply(FrameworkElement element)
	{
		var merged = element.Resources.MergedDictionaries;
		foreach (var dictionary in merged)
		{
			if (dictionary is SubtleButtonResources)
			{
				return;
			}
		}

		// One dictionary per element: WinUI rejects a dictionary with two parents. First, so the
		// element's own entries and later merges keep precedence.
		merged.Insert(0, new SubtleButtonResources());
	}

	private static void Remove(IList<ResourceDictionary> merged)
	{
		for (var i = merged.Count - 1; i >= 0; i--)
		{
			if (merged[i] is SubtleButtonResources)
			{
				merged.RemoveAt(i);
			}
		}
	}

	private sealed class SubtleButtonResources : ResourceDictionary
	{
		// The dictionaries are per element; their brushes are shared (per UI thread, as brushes are
		// thread-affine).
		[ThreadStatic]
		private static Brush[]? _brushes;

		public SubtleButtonResources()
		{
			// SubtleFillColorSecondary / SubtleFillColorTertiary per theme, then a transparent brush.
			// High contrast keeps the native button resources (the empty branch falls through to them).
			var brushes = _brushes ??= new Brush[]
			{
				new SolidColorBrush(Color.FromArgb(0x09, 0, 0, 0)),
				new SolidColorBrush(Color.FromArgb(0x06, 0, 0, 0)),
				new SolidColorBrush(Color.FromArgb(0x0F, 0xFF, 0xFF, 0xFF)),
				new SolidColorBrush(Color.FromArgb(0x0A, 0xFF, 0xFF, 0xFF)),
				new SolidColorBrush(Color.FromArgb(0, 0xFF, 0xFF, 0xFF)),
			};
			ThemeDictionaries["Light"] = CreateBranch(brushes[0], brushes[1], brushes[4]);
			ThemeDictionaries["Default"] = CreateBranch(brushes[2], brushes[3], brushes[4]);
			ThemeDictionaries["HighContrast"] = new ResourceDictionary();
		}

		private static ResourceDictionary CreateBranch(Brush pointerOver, Brush pressed, Brush transparent)
			=> new()
			{
				["ButtonBackgroundPointerOver"] = pointerOver,
				["ButtonBackgroundPressed"] = pressed,
				["ButtonBackgroundDisabled"] = transparent,
				["ButtonBorderBrushPointerOver"] = transparent,
				["ButtonBorderBrushPressed"] = transparent,
				["ButtonBorderBrushDisabled"] = transparent,
			};
	}
}
