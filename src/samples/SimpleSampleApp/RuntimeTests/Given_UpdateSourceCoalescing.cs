using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that <see cref="BaseTheme.UpdateSource"/> is invoked exactly once during
/// XAML-style initialization, even when the constructor runs followed by N property
/// setters. Without coalescing the constructor + each DP-changed callback would each
/// rebuild the resource tree (clearing and re-merging dictionaries, re-collecting
/// brush instances) — N+1 rebuilds per theme.
/// </summary>
[TestClass]
public class Given_UpdateSourceCoalescing
{
	[TestMethod]
	[RunsOnUIThread]
	public void When_NoProperties_Then_RebuildsOnceAfterEnsureInitialized()
	{
		var theme = new SimpleTheme();
		theme.EnsureInitialized();
		Assert.AreEqual(1, theme.RebuildCount,
			"A theme constructed with no properties should rebuild exactly once.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_MultiplePropertiesSetAtConstruction_Then_RebuildsOnce()
	{
		var theme = new SimpleTheme
		{
			PrimarySeedColor = Color.FromArgb(0xFF, 0x12, 0x34, 0x56),
			DefaultDensity = Density.Compact,
			DefaultCornerRadius = 6.0,
		};
		theme.EnsureInitialized();
		Assert.AreEqual(1, theme.RebuildCount,
			"Constructor + multiple DP setters should produce a single rebuild.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_RuntimePropertyChange_AfterInit_Then_RebuildsAgainSynchronously()
	{
		var theme = new SimpleTheme();
		theme.EnsureInitialized();
		Assert.AreEqual(1, theme.RebuildCount, "Initial rebuild should have flushed.");

		theme.PrimarySeedColor = Color.FromArgb(0xFF, 0xAB, 0xCD, 0xEF);
		Assert.AreEqual(2, theme.RebuildCount,
			"Runtime property changes after the initial rebuild should rebuild synchronously.");

		theme.DefaultDensity = Density.Comfy;
		Assert.AreEqual(3, theme.RebuildCount,
			"A second runtime property change should produce another synchronous rebuild.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_EnsureInitializedCalledTwice_Then_NoExtraRebuild()
	{
		var theme = new SimpleTheme { DefaultDensity = Density.Compact };
		theme.EnsureInitialized();
		var firstCount = theme.RebuildCount;

		theme.EnsureInitialized();
		Assert.AreEqual(firstCount, theme.RebuildCount,
			"EnsureInitialized() should be idempotent once the pending rebuild has flushed.");
	}
}
