#nullable enable

using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the <see cref="Uno.Themes.BaseTheme.AddThemeSpecificResources"/> extension seam.
/// Subclasses (e.g. the Uno.Toolkit Material/Simple themes) override it to stack their own
/// resource dictionaries on top of the generated theme layers. The contract guarded here:
/// resources added via the hook resolve, and — because the hook runs inside every rebuild
/// pass — they are removed and re-added on each rebuild rather than accumulating or being
/// orphaned after the first pass.
/// </summary>
[TestClass]
public class Given_ThemeSpecificResources
{
	private const string MarkerKey = "TestThemeSpecificMarker";

	/// <summary>
	/// Stand-in for a toolkit theme: appends an in-memory dictionary carrying a marker key.
	/// A fresh dictionary is created on each pass, mirroring the toolkit's URI-backed pattern
	/// (a new <c>ResourceDictionary { Source = … }</c> per rebuild for hot-reload fidelity).
	/// </summary>
	private sealed class TestThemeWithExtraResources : SimpleTheme
	{
		protected override void AddThemeSpecificResources()
		{
			base.AddThemeSpecificResources();
			AddThemeDictionary(new ResourceDictionary { [MarkerKey] = 42.0 });
		}
	}

	private static int CountMarkerDictionaries(ResourceDictionary theme) =>
		theme.MergedDictionaries.Count(d => d.ContainsKey(MarkerKey));

	[TestMethod]
	[RunsOnUIThread]
	public void When_SubclassAddsResources_Then_TheyResolveFromTheTheme()
	{
		// Arrange
		var theme = new TestThemeWithExtraResources();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);

		// Act / Assert: the marker added via the hook is resolvable through the theme.
		Assert.IsTrue(
			container.Resources.TryGetValue(MarkerKey, out var value),
			"Resource added via AddThemeSpecificResources should resolve from the theme");
		Assert.AreEqual(42.0, value);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeRebuilds_Then_HookResourcesAreReappliedExactlyOnce()
	{
		// Arrange: a single marker dictionary after the constructor's initial rebuild.
		var theme = new TestThemeWithExtraResources();
		Assert.AreEqual(
			1,
			CountMarkerDictionaries(theme),
			"Exactly one hook-added dictionary should be present after construction");

		// Act: trigger several rebuilds via a theme-property change.
		theme.DefaultCornerRadius = 8.0;
		theme.DefaultCornerRadius = 12.0;
		theme.DefaultDensity = Density.Compact;

		// Assert: the hook re-ran each pass but the dynamic layer is cleared first, so the
		// marker dictionary never accumulates and is never orphaned.
		Assert.AreEqual(
			1,
			CountMarkerDictionaries(theme),
			"Hook-added dictionaries must be removed and re-added on rebuild, not accumulated");

		// And the resource still resolves after rebuilds.
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		Assert.IsTrue(
			container.Resources.TryGetValue(MarkerKey, out var value) && (double)value == 42.0,
			"Hook-added resource should still resolve after a rebuild");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_BaseTheme_Then_HookIsNoOpByDefault()
	{
		// A theme that does not override the hook must behave exactly as before — no marker.
		var theme = new SimpleTheme();
		Assert.AreEqual(
			0,
			CountMarkerDictionaries(theme),
			"The default AddThemeSpecificResources is a no-op and must not add dictionaries");
	}
}
