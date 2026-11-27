#nullable enable

using System.Collections.Generic;
using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Fluent;

/// <summary>
/// The accent resources FluentTheme merges. Its brushes are stable instances updated in place, so
/// built-in controls already holding one follow a seed or override change and return to the platform
/// value when it clears (as <c>SemanticBrushUpdater</c> does for the semantic brushes).
/// </summary>
internal sealed class FluentAccentLayer
{
	private readonly Dictionary<string, SolidColorBrush> _lightBrushes = new();
	private readonly Dictionary<string, SolidColorBrush> _darkBrushes = new();

	public FluentAccentLayer()
	{
		// Both branches always exist: a Dark-only entry in Default must not become the Light fallback.
		// The empty HighContrast branch keeps the platform's high-contrast accent resources.
		Resources.ThemeDictionaries["Light"] = new ResourceDictionary();
		Resources.ThemeDictionaries["Default"] = new ResourceDictionary();
		Resources.ThemeDictionaries["HighContrast"] = new ResourceDictionary();
	}

	public ResourceDictionary Resources { get; } = new();

	/// <param name="isLight">The Light branch, else the Default (dark) branch.</param>
	/// <param name="entries">The generated accent resources, or <c>null</c> to fall back to the platform.</param>
	public void Update(bool isLight, Dictionary<string, object>? entries)
	{
		var branch = (ResourceDictionary)Resources.ThemeDictionaries[isLight ? "Light" : "Default"];
		var brushes = isLight ? _lightBrushes : _darkBrushes;

		// Brushes no longer generated return to the platform value for the controls still holding them.
		Dictionary<string, object>? platform = null;
		foreach (var pair in brushes.Where(pair => entries?.ContainsKey(pair.Key) != true))
		{
			platform ??= FluentAccentPalette.BuildPlatform(isLight);
			if (platform?.TryGetValue(pair.Key, out var value) == true && value is FluentAccentPalette.BrushSpec spec)
			{
				Apply(spec, pair.Value);
			}
		}

		branch.Clear();
		if (entries is null)
		{
			return;
		}

		foreach (var pair in entries)
		{
			if (pair.Value is FluentAccentPalette.BrushSpec spec)
			{
				if (!brushes.TryGetValue(pair.Key, out var brush))
				{
					brushes[pair.Key] = brush = new SolidColorBrush();
				}
				Apply(spec, brush);
				branch[pair.Key] = brush;
			}
			else
			{
				branch[pair.Key] = pair.Value;
			}
		}
	}

	private static void Apply(FluentAccentPalette.BrushSpec spec, SolidColorBrush target)
	{
		target.Color = spec.Color;
		target.Opacity = spec.Opacity;
	}
}
