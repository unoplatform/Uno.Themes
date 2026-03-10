using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests.Helpers;

/// <summary>
/// Shared utilities for style and template runtime tests.
/// </summary>
internal static class StyleTestHelper
{
	/// <summary>
	/// Places a control into the test render zone and waits for it to load.
	/// </summary>
	public static async Task LoadAndWait(FrameworkElement element)
	{
		UnitTestsUIContentHelper.Content = element;
		await UnitTestsUIContentHelper.WaitForIdle();
		await UnitTestsUIContentHelper.WaitForLoaded(element);
	}

	/// <summary>
	/// Finds a named descendant element within a visual tree.
	/// </summary>
	public static T FindTemplatePart<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		return root.FindFirstDescendant<T>(x => x.Name == name);
	}

	/// <summary>
	/// Asserts that a ThemeResource key resolves to a non-null value.
	/// </summary>
	public static void AssertThemeResourceResolves(string resourceKey)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(resourceKey, out var value),
			$"ThemeResource '{resourceKey}' not found in Application.Current.Resources");
		Assert.IsNotNull(value, $"ThemeResource '{resourceKey}' resolved to null");
	}

	/// <summary>
	/// Asserts that a ThemeResource key resolves to a <see cref="Brush"/> instance.
	/// </summary>
	public static void AssertBrushResource(string resourceKey)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(resourceKey, out var value),
			$"ThemeResource '{resourceKey}' not found in Application.Current.Resources");
		Assert.IsInstanceOfType(value, typeof(Brush),
			$"ThemeResource '{resourceKey}' resolved to {value?.GetType().Name} instead of Brush");
	}
}
