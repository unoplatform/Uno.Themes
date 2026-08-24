using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the instance-based theme lookup: <c>application.GetTheme()</c>
/// resolves the <c>BaseTheme</c> merged into the given application's resources,
/// and <c>SemanticThemeHelper.GetTheme()</c> remains a pure delegation to it
/// on <c>Application.Current</c>.
/// </summary>
[TestClass]
public class Given_ApplicationExtensions
{
	[TestMethod]
	[RunsOnUIThread]
	public void When_GettingThemeFromApplicationInstance_Then_ReturnsMergedTheme()
	{
		var theme = Application.Current.GetTheme();

		Assert.IsInstanceOfType(theme, typeof(SimpleTheme),
			"The sample app merges a SimpleTheme at the application level");
		Assert.IsTrue(
			Application.Current.Resources.MergedDictionaries.Contains(theme),
			"The returned theme should be the instance merged into the application's resources");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ComparedToStaticHelper_Then_SameInstanceIsReturned()
	{
		var fromExtension = Application.Current.GetTheme();
		var fromHelper = SemanticThemeHelper.GetTheme();

		Assert.AreSame(fromHelper, fromExtension,
			"SemanticThemeHelper.GetTheme() should delegate to the extension on Application.Current");
	}

	[TestMethod]
	public void When_ApplicationIsNull_Then_ReturnsNull()
	{
		Assert.IsNull(ApplicationExtensions.GetTheme(null));
	}

	/// <summary>
	/// The head publishes itself on <c>NavigationHelper.CurrentApplication</c> so shared sample code
	/// can reach *this* application's theme without going through <c>Application.Current</c> — which
	/// returns the host, and no theme, when the sample is hosted in a secondary ALC (ThemesSampleApp).
	/// </summary>
	/// <remarks>
	/// Standalone the two are the same application, so this guards the hand-off from silently
	/// regressing (a head that stops publishing itself). The hosted path cannot be reached from here
	/// — <c>Application.Current</c> is the app under test by construction — and is gated by the
	/// wrapper's <c>--smoke</c> harness instead.
	/// </remarks>
	[TestMethod]
	[RunsOnUIThread]
	public void When_RunningStandalone_Then_CurrentApplicationIsPublishedAndResolvesSameTheme()
	{
		// Captured into a local so the null check below informs the compiler's nullable flow —
		// Assert.IsNotNull does not, and CurrentApplication is legitimately nullable.
		var published = NavigationHelper.CurrentApplication;

		Assert.IsNotNull(published,
			"The App class must publish itself on NavigationHelper.CurrentApplication");
		Assert.AreSame(Application.Current, published,
			"Standalone there is a single application, so the published one is Application.Current");
		Assert.AreSame(Application.Current.GetTheme(), published.GetTheme(),
			"Both paths must resolve the same BaseTheme instance when standalone");
	}
}
