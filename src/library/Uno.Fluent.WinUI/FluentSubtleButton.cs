#nullable enable

using System;

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
			var merged = element.Resources.MergedDictionaries;
			for (var i = merged.Count - 1; i >= 0; i--)
			{
				if (merged[i] is SubtleButtonResources)
				{
					merged.RemoveAt(i);
				}
			}

			// One dictionary per element: WinUI rejects a dictionary with two parents. First, so the
			// element's own entries and later merges keep precedence.
			if (args.NewValue is true)
			{
				merged.Insert(0, new SubtleButtonResources());
			}
		}
		catch (Exception e)
		{
			// Property-changed callback: never let a failure escape into the consumer's layout.
			FluentDiagnostics.LogWarning("FluentTheme could not apply the subtle button resources.", e);
		}
	}

	private sealed class SubtleButtonResources : ResourceDictionary
	{
		public SubtleButtonResources()
		{
			// SubtleFillColorSecondary / SubtleFillColorTertiary per theme. High contrast keeps the
			// native button resources (the empty branch falls through to them).
			ThemeDictionaries["Light"] = CreateBranch(Color.FromArgb(0x09, 0, 0, 0), Color.FromArgb(0x06, 0, 0, 0));
			ThemeDictionaries["Default"] = CreateBranch(Color.FromArgb(0x0F, 0xFF, 0xFF, 0xFF), Color.FromArgb(0x0A, 0xFF, 0xFF, 0xFF));
			ThemeDictionaries["HighContrast"] = new ResourceDictionary();
		}

		private static ResourceDictionary CreateBranch(Color pointerOver, Color pressed)
		{
			var transparent = Color.FromArgb(0, 0xFF, 0xFF, 0xFF);
			return new ResourceDictionary
			{
				["ButtonBackgroundPointerOver"] = new SolidColorBrush(pointerOver),
				["ButtonBackgroundPressed"] = new SolidColorBrush(pressed),
				["ButtonBackgroundDisabled"] = new SolidColorBrush(transparent),
				["ButtonBorderBrushPointerOver"] = new SolidColorBrush(transparent),
				["ButtonBorderBrushPressed"] = new SolidColorBrush(transparent),
				["ButtonBorderBrushDisabled"] = new SolidColorBrush(transparent),
			};
		}
	}
}
