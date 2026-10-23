using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the sample heads' own theme handle: sample pages must reach the theme through
/// <see cref="SampleThemeHelper"/> rather than <c>Application.Current</c>, because a head hosted in
/// a secondary ALC by <c>ThemesSampleApp</c> is not the current application (Uno only assigns
/// <c>Application.Current</c> for the default-ALC app). These tests run standalone, where both
/// routes must agree — the hosted case is covered by running the wrapper.
/// </summary>
[TestClass]
public class Given_SampleThemeAccess
{
	[TestMethod]
	[RunsOnUIThread]
	public void When_HeadIsLoaded_Then_ApplicationHandleIsRegistered()
	{
		Assert.IsNotNull(SampleThemeHelper.CurrentApplication,
			"The head's App constructor must register itself, or hosted sample pages lose their theme");
		Assert.AreSame(Application.Current, SampleThemeHelper.CurrentApplication,
			"Running standalone, the registered application is the current one");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_GettingTheme_Then_ReturnsTheApplicationsMergedTheme()
	{
		var theme = SampleThemeHelper.GetTheme();

		Assert.IsInstanceOfType(theme, typeof(SimpleTheme),
			"The sample app merges a SimpleTheme at the application level");
		Assert.AreSame(Application.Current.GetTheme(), theme,
			"The head's handle must resolve the same instance as the current application does standalone");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_GettingColors_Then_ReturnsTheThemesColors()
	{
		var theme = SampleThemeHelper.GetTheme();

		var colors = SampleThemeHelper.GetColorsOrThrow();

		Assert.IsNotNull(colors, "A theme with no Colors gets one created on demand");
		Assert.AreSame(theme.Colors, colors,
			"The returned colors must be the ones the theme actually carries — a copy would silently drop seed changes");
	}
}
