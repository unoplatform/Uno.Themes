using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uno.Themes.Samples.RuntimeTests.Helpers;

/// <summary>
/// Utilities for validating Light/Dark theme dictionary parity.
/// </summary>
internal static class ThemeDictionaryTestHelper
{
	/// <summary>
	/// Loads a XAML resource dictionary and asserts that Light and Default (Dark)
	/// ThemeDictionaries contain the same set of resource keys.
	/// </summary>
	public static void AssertThemeParity(string xamlUri)
	{
		var dict = new ResourceDictionary { Source = new Uri(xamlUri) };

		if (!dict.ThemeDictionaries.TryGetValue("Light", out var lightObj) ||
			!dict.ThemeDictionaries.TryGetValue("Default", out var darkObj))
		{
			// No ThemeDictionaries or missing one side — skip (not all files have them)
			return;
		}

		if (lightObj is not ResourceDictionary light || darkObj is not ResourceDictionary dark)
		{
			Assert.Fail($"ThemeDictionaries in '{xamlUri}' did not contain ResourceDictionary entries");
			return;
		}

		var lightKeys = GetKeys(light);
		var darkKeys = GetKeys(dark);

		var missingInDark = lightKeys.Except(darkKeys).ToList();
		var missingInLight = darkKeys.Except(lightKeys).ToList();

		var errors = new List<string>();
		if (missingInDark.Count > 0)
		{
			errors.Add($"Keys in Light but missing in Default (Dark): {string.Join(", ", missingInDark)}");
		}
		if (missingInLight.Count > 0)
		{
			errors.Add($"Keys in Default (Dark) but missing in Light: {string.Join(", ", missingInLight)}");
		}

		Assert.AreEqual(0, errors.Count,
			$"Theme parity violation in '{xamlUri}':\n{string.Join("\n", errors)}");
	}

	/// <summary>
	/// Extracts sorted resource keys from a ResourceDictionary.
	/// </summary>
	public static IReadOnlyList<string> GetKeys(ResourceDictionary dict)
	{
		return dict.Keys.Cast<string>().OrderBy(k => k).ToList();
	}
}
