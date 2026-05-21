using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that <see cref="BaseTheme.UpdateSource"/> runs synchronously on
/// construction and on every DP change, so {StaticResource …} lookups against
/// the theme dictionary resolve correctly without needing the dispatcher to pump.
/// </summary>
[TestClass]
public class Given_UpdateSourceRebuild
{
	[TestMethod]
	[RunsOnUIThread]
	public void When_NoProperties_Then_RebuildsOnceDuringConstruction()
	{
		var theme = new SimpleTheme();
		Assert.AreEqual(1, theme.RebuildCount,
			"A theme constructed with no properties should rebuild exactly once.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_RuntimePropertyChange_Then_RebuildsSynchronously()
	{
		var theme = new SimpleTheme();
		var baseline = theme.RebuildCount;

		theme.PrimarySeedColor = Color.FromArgb(0xFF, 0xAB, 0xCD, 0xEF);
		Assert.AreEqual(baseline + 1, theme.RebuildCount,
			"Runtime property changes should rebuild synchronously.");

		theme.DefaultDensity = Density.Comfy;
		Assert.AreEqual(baseline + 2, theme.RebuildCount,
			"A second runtime property change should produce another synchronous rebuild.");
	}
}
