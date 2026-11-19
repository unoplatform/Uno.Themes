#nullable enable

using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Fluent;

/// <summary>
/// Lets a style re-point a control's own lightweight resources (e.g. <c>ButtonBackgroundPointerOver</c>):
/// WinUI styles cannot set <see cref="FrameworkElement.Resources"/>, so a setter assigns a dictionary
/// that is merged into the element's resources instead. Keeps the built-in template untouched.
/// </summary>
internal static class FluentResources
{
	public static readonly DependencyProperty MergedProperty = DependencyProperty.RegisterAttached(
		"Merged", typeof(ResourceDictionary), typeof(FluentResources), new PropertyMetadata(null, OnMergedChanged));

	public static ResourceDictionary? GetMerged(DependencyObject obj) => (ResourceDictionary?)obj.GetValue(MergedProperty);

	public static void SetMerged(DependencyObject obj, ResourceDictionary? value) => obj.SetValue(MergedProperty, value);

	private static void OnMergedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is not FrameworkElement element)
		{
			return;
		}

		try
		{
			var merged = element.Resources.MergedDictionaries;
			if (args.OldValue is ResourceDictionary previous)
			{
				merged.Remove(previous);
			}

			// First, so the element's own entries and later merges keep precedence.
			if (args.NewValue is ResourceDictionary next)
			{
				merged.Insert(0, next);
			}
		}
		catch (Exception e)
		{
			// Property-changed callback: never let a failure escape into the consumer's layout.
			FluentDiagnostics.LogWarning($"FluentTheme could not merge control resources. {e.Message}");
		}
	}
}
