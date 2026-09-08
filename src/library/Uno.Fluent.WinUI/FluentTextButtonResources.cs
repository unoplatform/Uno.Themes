#nullable enable

using System;
using Uno.Themes;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Fluent;

/// <summary>
/// Supplies per-button native state resources while retaining the platform Button template.
/// The style's ThemeResource setters resolve semantic brushes in the element's resource scope.
/// </summary>
internal static class FluentTextButtonResources
{
	private static readonly DependencyProperty OverridesProperty = DependencyProperty.RegisterAttached(
		"Overrides", typeof(ResourceDictionary), typeof(FluentTextButtonResources), new PropertyMetadata(null));
	public static readonly DependencyProperty ForegroundPointerOverProperty = Register("ForegroundPointerOver", "ButtonForegroundPointerOver");
	public static Brush? GetForegroundPointerOver(DependencyObject obj) => (Brush?)obj.GetValue(ForegroundPointerOverProperty);
	public static void SetForegroundPointerOver(DependencyObject obj, Brush? value) => obj.SetValue(ForegroundPointerOverProperty, value);
	public static readonly DependencyProperty ForegroundPressedProperty = Register("ForegroundPressed", "ButtonForegroundPressed");
	public static Brush? GetForegroundPressed(DependencyObject obj) => (Brush?)obj.GetValue(ForegroundPressedProperty);
	public static void SetForegroundPressed(DependencyObject obj, Brush? value) => obj.SetValue(ForegroundPressedProperty, value);
	public static readonly DependencyProperty ForegroundDisabledProperty = Register("ForegroundDisabled", "ButtonForegroundDisabled");
	public static Brush? GetForegroundDisabled(DependencyObject obj) => (Brush?)obj.GetValue(ForegroundDisabledProperty);
	public static void SetForegroundDisabled(DependencyObject obj, Brush? value) => obj.SetValue(ForegroundDisabledProperty, value);
	public static readonly DependencyProperty BackgroundPointerOverProperty = Register("BackgroundPointerOver", "ButtonBackgroundPointerOver");
	public static Brush? GetBackgroundPointerOver(DependencyObject obj) => (Brush?)obj.GetValue(BackgroundPointerOverProperty);
	public static void SetBackgroundPointerOver(DependencyObject obj, Brush? value) => obj.SetValue(BackgroundPointerOverProperty, value);
	public static readonly DependencyProperty BackgroundPressedProperty = Register("BackgroundPressed", "ButtonBackgroundPressed");
	public static Brush? GetBackgroundPressed(DependencyObject obj) => (Brush?)obj.GetValue(BackgroundPressedProperty);
	public static void SetBackgroundPressed(DependencyObject obj, Brush? value) => obj.SetValue(BackgroundPressedProperty, value);
	public static readonly DependencyProperty BackgroundDisabledProperty = Register("BackgroundDisabled", "ButtonBackgroundDisabled");
	public static Brush? GetBackgroundDisabled(DependencyObject obj) => (Brush?)obj.GetValue(BackgroundDisabledProperty);
	public static void SetBackgroundDisabled(DependencyObject obj, Brush? value) => obj.SetValue(BackgroundDisabledProperty, value);
	public static readonly DependencyProperty BorderBrushPointerOverProperty = Register("BorderBrushPointerOver", "ButtonBorderBrushPointerOver");
	public static Brush? GetBorderBrushPointerOver(DependencyObject obj) => (Brush?)obj.GetValue(BorderBrushPointerOverProperty);
	public static void SetBorderBrushPointerOver(DependencyObject obj, Brush? value) => obj.SetValue(BorderBrushPointerOverProperty, value);
	public static readonly DependencyProperty BorderBrushPressedProperty = Register("BorderBrushPressed", "ButtonBorderBrushPressed");
	public static Brush? GetBorderBrushPressed(DependencyObject obj) => (Brush?)obj.GetValue(BorderBrushPressedProperty);
	public static void SetBorderBrushPressed(DependencyObject obj, Brush? value) => obj.SetValue(BorderBrushPressedProperty, value);
	public static readonly DependencyProperty BorderBrushDisabledProperty = Register("BorderBrushDisabled", "ButtonBorderBrushDisabled");
	public static Brush? GetBorderBrushDisabled(DependencyObject obj) => (Brush?)obj.GetValue(BorderBrushDisabledProperty);
	public static void SetBorderBrushDisabled(DependencyObject obj, Brush? value) => obj.SetValue(BorderBrushDisabledProperty, value);

	private static readonly (DependencyProperty Property, string ResourceKey)[] Mappings =
	{
		(ForegroundPointerOverProperty, "ButtonForegroundPointerOver"),
		(ForegroundPressedProperty, "ButtonForegroundPressed"),
		(ForegroundDisabledProperty, "ButtonForegroundDisabled"),
		(BackgroundPointerOverProperty, "ButtonBackgroundPointerOver"),
		(BackgroundPressedProperty, "ButtonBackgroundPressed"),
		(BackgroundDisabledProperty, "ButtonBackgroundDisabled"),
		(BorderBrushPointerOverProperty, "ButtonBorderBrushPointerOver"),
		(BorderBrushPressedProperty, "ButtonBorderBrushPressed"),
		(BorderBrushDisabledProperty, "ButtonBorderBrushDisabled"),
	};

	private static DependencyProperty Register(string propertyName, string resourceKey) =>
		DependencyProperty.RegisterAttached(propertyName, typeof(Brush), typeof(FluentTextButtonResources),
			CreateBrushMetadata((sender, args) => Apply(sender, resourceKey, args.NewValue as Brush)));

	private static PropertyMetadata CreateBrushMetadata(PropertyChangedCallback callback)
	{
#if HAS_UNO
		// Shared theme brushes must not inherit a button's data context: Uno's
		// inheritance association otherwise keeps that button alive after style removal.
		return new FrameworkPropertyMetadata(null,
			FrameworkPropertyMetadataOptions.ValueDoesNotInheritDataContext, callback);
#else
		return new PropertyMetadata(null, callback);
#endif
	}

	private static void Apply(DependencyObject sender, string resourceKey, Brush? value)
	{
		if (sender is not Button button)
		{
			return;
		}

		try
		{
			var overrides = button.GetValue(OverridesProperty) as ResourceDictionary;
			if (value is null)
			{
				if (overrides is { })
				{
					overrides.Remove(resourceKey);
					if (overrides.Count == 0)
					{
						button.Resources.MergedDictionaries.Remove(overrides);
						button.Loaded -= OnLoaded;
						button.ActualThemeChanged -= OnActualThemeChanged;
						button.ClearValue(OverridesProperty);
					}
				}
				return;
			}

			if (overrides is null)
			{
				overrides = new ResourceDictionary();
				button.SetValue(OverridesProperty, overrides);
				// Own consumer entries and later merged dictionaries retain precedence.
				button.Resources.MergedDictionaries.Insert(0, overrides);
				button.Loaded += OnLoaded;
				button.ActualThemeChanged += OnActualThemeChanged;
			}

			value = FindExplicitOverride(button, resourceKey, overrides) ?? value;
			if (value is SolidColorBrush source)
			{
				// Keep the native brush stable when a semantic resource is replaced. Bind rather
				// than mutating a consumer brush or subscribing to its property changes ourselves.
				SolidColorBrush? target = null;
				foreach (var pair in overrides)
				{
					if (Equals(pair.Key, resourceKey))
					{
						target = pair.Value as SolidColorBrush;
						break;
					}
				}
				target ??= new SolidColorBrush();
				BindingOperations.SetBinding(target, SolidColorBrush.ColorProperty,
					new Binding { Source = source, Path = new PropertyPath(nameof(SolidColorBrush.Color)), Mode = BindingMode.OneWay });
				BindingOperations.SetBinding(target, Brush.OpacityProperty,
					new Binding { Source = source, Path = new PropertyPath(nameof(Brush.Opacity)), Mode = BindingMode.OneWay });
				overrides[resourceKey] = target;
			}
			else
			{
				overrides[resourceKey] = value;
			}
		}
		catch (ArgumentException ex)
		{
			FluentDiagnostics.LogWarning($"Could not apply Fluent text-button resource '{resourceKey}': {ex.Message}");
		}
		catch (InvalidOperationException ex)
		{
			FluentDiagnostics.LogWarning($"Could not apply Fluent text-button resource '{resourceKey}': {ex.Message}");
		}
		catch (NotSupportedException ex)
		{
			FluentDiagnostics.LogWarning($"Could not apply Fluent text-button resource '{resourceKey}': {ex.Message}");
		}
		catch (Exception ex)
		{
			FluentDiagnostics.LogWarning($"Could not apply Fluent text-button resource '{resourceKey}': {ex.Message}");
		}
	}

	private static void OnLoaded(object sender, RoutedEventArgs args)
	{
		if (sender is Button button)
		{
			Refresh(button);
		}
	}

	private static void OnActualThemeChanged(FrameworkElement sender, object args)
	{
		if (sender is Button button)
		{
			Refresh(button);
		}
	}

	private static void Refresh(Button button)
	{
		foreach (var (property, resourceKey) in Mappings)
		{
			Apply(button, resourceKey, button.GetValue(property) as Brush);
		}
	}

	private static Brush? FindExplicitOverride(Button button, string resourceKey, ResourceDictionary generated)
	{
		var appearance = button.ActualTheme == ElementTheme.Dark ? "Dark" : "Light";
		var semanticKey = "Text" + resourceKey;
		for (FrameworkElement? scope = button; scope is { }; scope = scope.Parent as FrameworkElement)
		{
			if (ResolveExplicit(scope.Resources, semanticKey) is Brush semantic)
			{
				return semantic;
			}
			if (ResolveExplicit(scope.Resources, resourceKey) is Brush native)
			{
				return native;
			}
		}
		if (Application.Current?.Resources is { } resources)
		{
			return ResolveExplicit(resources, semanticKey) as Brush ?? ResolveExplicit(resources, resourceKey) as Brush;
		}
		return null;

		object? ResolveExplicit(ResourceDictionary dictionary, string key)
		{
			// Exclude generated/default resources: they must not shadow a consumer's
			// native override higher in the tree. Theme override channels remain explicit.
			if (ReferenceEquals(dictionary, generated) || dictionary is XamlControlsResources)
			{
				return null;
			}
			if (dictionary is BaseTheme theme)
			{
				return FluentResourceResolver.Resolve(theme.ResolvedColorOverride, appearance, key);
			}
			foreach (var entry in dictionary)
			{
				if (Equals(entry.Key, key))
				{
					return entry.Value;
				}
			}
			for (var i = dictionary.MergedDictionaries.Count - 1; i >= 0; i--)
			{
				if (ResolveExplicit(dictionary.MergedDictionaries[i], key) is { } merged)
				{
					return merged;
				}
			}
			if (dictionary.ThemeDictionaries.TryGetValue(appearance, out var themed) && themed is ResourceDictionary branch)
			{
				return ResolveExplicit(branch, key);
			}
			return dictionary.ThemeDictionaries.TryGetValue("Default", out var fallback) && fallback is ResourceDictionary defaultBranch
				? ResolveExplicit(defaultBranch, key)
				: null;
		}
	}
}
