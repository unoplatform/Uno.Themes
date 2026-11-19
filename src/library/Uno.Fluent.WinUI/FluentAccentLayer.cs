#nullable enable

using System.Collections.Generic;

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
		foreach (var pair in brushes)
		{
			if (entries is null || !entries.ContainsKey(pair.Key))
			{
				platform ??= FluentAccentPalette.BuildPlatform(isLight);
				if (platform?.TryGetValue(pair.Key, out var value) == true && value is SolidColorBrush source)
				{
					CopyTo(source, pair.Value);
				}
			}
		}

		branch.Clear();
		if (entries is null)
		{
			return;
		}

		foreach (var pair in entries)
		{
			if (pair.Value is SolidColorBrush source)
			{
				if (!brushes.TryGetValue(pair.Key, out var brush))
				{
					brushes[pair.Key] = brush = new SolidColorBrush();
				}
				CopyTo(source, brush);
				branch[pair.Key] = brush;
			}
			else
			{
				branch[pair.Key] = pair.Value;
			}
		}
	}

	private static void CopyTo(SolidColorBrush source, SolidColorBrush target)
	{
		target.Color = source.Color;
		target.Opacity = source.Opacity;
	}
}
