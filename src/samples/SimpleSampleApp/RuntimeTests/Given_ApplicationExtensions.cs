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
}
