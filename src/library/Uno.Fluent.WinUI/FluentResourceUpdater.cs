#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Fluent;

/// <summary>
/// Reconciles generated resources while preserving the brushes already held by native controls.
/// </summary>
internal static class FluentResourceUpdater
{
	// A removed resource can still be held by a realized control. Keep only a weak reference to
	// that brush, so a later override can update the same control without retaining unused brushes.
	private static readonly ConditionalWeakTable<ResourceDictionary, DictionaryState> States = new();

	/// <summary>
	/// Updates a stable, updater-owned dictionary from a generated dictionary. Solid brushes are
	/// copied into owned instances with one-way bindings; consumer-owned brushes are never mutated.
	/// </summary>
	/// <param name="target">The stable, initially empty, code-built output dictionary.</param>
	/// <param name="replacement">Generated own entries and explicit appearance dictionaries, or null to clear.</param>
	/// <param name="baseline">
	/// Resources below this generated layer. Supply the removed brush keys here so existing controls
	/// return to their baseline before the corresponding entries are removed. Appearance-specific
	/// brushes must use explicit theme dictionaries in both the replacement and baseline.
	/// </param>
	internal static void Update(ResourceDictionary target, ResourceDictionary? replacement, ResourceDictionary? baseline = null)
		=> UpdateDictionary(target, replacement, baseline, "Default");

	private static void UpdateDictionary(
		ResourceDictionary target,
		ResourceDictionary? replacement,
		ResourceDictionary? baseline,
		string appearance)
	{
		var state = States.GetValue(target, static _ => new DictionaryState());
		var entries = ReadEntries(replacement);

		foreach (var pair in entries)
		{
			if (pair.Value is SolidColorBrush source)
			{
				if (!state.Brushes.TryGetValue(pair.Key, out var entry)
					|| !entry.Brush.TryGetTarget(out var brush))
				{
					brush = new SolidColorBrush();
					entry = new BrushEntry(brush);
					state.Brushes[pair.Key] = entry;
				}

				BindSource(brush, entry, source);
				target[pair.Key] = brush;
			}
			else
			{
				target[pair.Key] = pair.Value;
			}
		}

		List<string>? expiredBrushes = null;
		foreach (var pair in state.Brushes)
		{
			if (entries.TryGetValue(pair.Key, out var replacementValue) && replacementValue is SolidColorBrush)
			{
				continue;
			}

			if (!pair.Value.Brush.TryGetTarget(out var brush))
			{
				(expiredBrushes ??= new()).Add(pair.Key);
			}
			else if (FluentResourceResolver.Resolve(baseline, appearance, pair.Key) is SolidColorBrush fallback)
			{
				// Update even previously removed entries: their brushes may still belong to controls,
				// and the platform baseline can change while no explicit override is present.
				BindSource(brush, pair.Value, fallback);
			}
		}

		if (expiredBrushes is { })
		{
			foreach (var key in expiredBrushes)
			{
				state.Brushes.Remove(key);
			}
		}

		List<object>? removedKeys = null;
		foreach (var pair in target)
		{
			if (pair.Key is not string key || !entries.ContainsKey(key))
			{
				(removedKeys ??= new()).Add(pair.Key);
			}
		}
		if (removedKeys is { })
		{
			foreach (var key in removedKeys)
			{
				target.Remove(key);
			}
		}

		if (replacement is { })
		{
			foreach (var pair in replacement.ThemeDictionaries)
			{
				if (pair.Key is string key && pair.Value is ResourceDictionary && !state.Branches.ContainsKey(key))
				{
					state.Branches[key] = new ResourceDictionary();
				}
			}
		}

		foreach (var pair in state.Branches)
		{
			var nextBranch = replacement is { }
				&& replacement.ThemeDictionaries.TryGetValue(pair.Key, out var value)
				? value as ResourceDictionary
				: null;
			// The generated Default branch represents Dark. Keeping that explicit prevents a
			// baseline with distinct Light/Dark resources from following the ambient app theme.
			var branchAppearance = pair.Key == "Default" ? "Dark" : pair.Key;
			UpdateDictionary(pair.Value, nextBranch, baseline, branchAppearance);
			if (nextBranch is { })
			{
				target.ThemeDictionaries[pair.Key] = pair.Value;
			}
			else
			{
				target.ThemeDictionaries.Remove(pair.Key);
			}
		}
	}

	private static Dictionary<string, object> ReadEntries(ResourceDictionary? dictionary)
	{
		var entries = new Dictionary<string, object>(StringComparer.Ordinal);
		if (dictionary is { })
		{
			foreach (var pair in dictionary)
			{
				if (pair.Key is string key)
				{
					entries[key] = pair.Value;
				}
			}
		}
		return entries;
	}

	private static void BindSource(SolidColorBrush brush, BrushEntry entry, SolidColorBrush source)
	{
		if (ReferenceEquals(brush, source)
			|| (entry.Source is { } previous && previous.TryGetTarget(out var previousSource) && ReferenceEquals(previousSource, source)))
		{
			return;
		}

		BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, CreateBinding(source, nameof(SolidColorBrush.Color)));
		BindingOperations.SetBinding(brush, Brush.OpacityProperty, CreateBinding(source, nameof(Brush.Opacity)));
		BindingOperations.SetBinding(brush, Brush.TransformProperty, CreateBinding(source, nameof(Brush.Transform)));
		BindingOperations.SetBinding(brush, Brush.RelativeTransformProperty, CreateBinding(source, nameof(Brush.RelativeTransform)));
		entry.Source = new WeakReference<SolidColorBrush>(source);
	}

	private static Binding CreateBinding(SolidColorBrush source, string property)
		=> new() { Source = source, Path = new PropertyPath(property), Mode = BindingMode.OneWay };

	private sealed class DictionaryState
	{
		public Dictionary<string, BrushEntry> Brushes { get; } = new(StringComparer.Ordinal);

		// Retain empty branch containers so a removed/reintroduced branch can reuse the weak
		// brush trackers. This holds no visual elements and is bounded by appearance names.
		public Dictionary<string, ResourceDictionary> Branches { get; } = new(StringComparer.Ordinal);
	}

	private sealed class BrushEntry(SolidColorBrush brush)
	{
		public WeakReference<SolidColorBrush> Brush { get; } = new(brush);
		public WeakReference<SolidColorBrush>? Source { get; set; }
	}
}
