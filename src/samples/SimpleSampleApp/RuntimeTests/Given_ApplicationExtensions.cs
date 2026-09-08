using System;
using System.Linq;
using Microsoft.UI.Xaml;
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

	// ─────────────────────────────────────────────────────────────────────
	// Nested layouts (issue #1704). App.xaml itself receives no Hot Reload updates, so the
	// guidance is to move the design system into a ResourceDictionary of its own and merge
	// that — which puts the BaseTheme one or more levels below Application.Resources.
	// ─────────────────────────────────────────────────────────────────────

	/// <summary>
	/// Rearranges the application's resources into the layout the guidance recommends: the theme is
	/// removed from the top level and merged into a chain of <paramref name="depth"/> plain
	/// dictionaries, the outermost of which is merged into <c>Application.Resources</c>. The returned
	/// disposable puts the application's resources back exactly as they were.
	/// </summary>
	/// <param name="depth">How many dictionaries to interpose between the application and the theme.</param>
	/// <returns>A scope that restores the application's resources when disposed.</returns>
	private static IDisposable NestApplicationTheme(int depth)
	{
		var appResources = Application.Current.Resources;
		var original = appResources.MergedDictionaries.ToArray();
		var themes = original.OfType<BaseTheme>().ToArray();

		Assert.IsTrue(themes.Length > 0,
			"This fixture needs the sample app's theme at the top level to move it down a level.");

		// Detach before re-parenting: a ResourceDictionary may not be nested under two parents.
		foreach (var theme in themes)
		{
			appResources.MergedDictionaries.Remove(theme);
		}

		var innermost = new ResourceDictionary();
		foreach (var theme in themes)
		{
			innermost.MergedDictionaries.Add(theme);
		}

		var outermost = innermost;
		for (var i = 1; i < depth; i++)
		{
			var parent = new ResourceDictionary();
			parent.MergedDictionaries.Add(outermost);
			outermost = parent;
		}

		appResources.MergedDictionaries.Add(outermost);

		return new RestoreApplicationResources(outermost, innermost, themes, original);
	}

	private sealed class RestoreApplicationResources(
		ResourceDictionary outermost,
		ResourceDictionary innermost,
		BaseTheme[] themes,
		ResourceDictionary[] original) : IDisposable
	{
		public void Dispose()
		{
			var appResources = Application.Current.Resources;

			appResources.MergedDictionaries.Remove(outermost);

			foreach (var theme in themes)
			{
				innermost.MergedDictionaries.Remove(theme);
			}

			appResources.MergedDictionaries.Clear();
			foreach (var dictionary in original)
			{
				appResources.MergedDictionaries.Add(dictionary);
			}
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(1, DisplayName = "theme one level down")]
	[DataRow(2, DisplayName = "theme two levels down")]
	public void When_ThemeIsNestedBelowApplicationResources_Then_ThemeIsStillFound(int depth)
	{
		using var _ = NestApplicationTheme(depth);

		var theme = Application.Current.GetTheme();

		Assert.IsNotNull(theme,
			$"GetTheme() must walk MergedDictionaries recursively; the theme is {depth} level(s) below " +
			"Application.Resources, which is the layout hot-reload guidance recommends.");
		Assert.IsInstanceOfType(theme, typeof(SimpleTheme));
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(31, true, DisplayName = "at the deepest level the walk descends to")]
	[DataRow(32, false, DisplayName = "one level beyond the walk's bound")]
	public void When_ThemeIsNestedAtTheDepthBound_Then_TheBoundHolds(int depth, bool expectFound)
	{
		using var _ = NestApplicationTheme(depth);

		var theme = Application.Current.GetTheme();

		Assert.AreEqual(expectFound, theme is not null,
			$"A theme {depth} dictionaries below the application sits at level {depth + 1}; the walk " +
			"descends 32 levels, and the bound is a guard against a pathological graph rather than a " +
			"limit any real application meets.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeIsNestedInASourceLoadedDictionary_Then_ThemeIsStillFound()
	{
		var appResources = Application.Current.Resources;
		var original = appResources.MergedDictionaries.ToArray();
		var themes = original.OfType<BaseTheme>().ToArray();

		Assert.IsTrue(themes.Length > 0,
			"This test needs the sample app's theme at the top level to take it away.");

		ResourceDictionary? nested = null;

		try
		{
			foreach (var theme in themes)
			{
				appResources.MergedDictionaries.Remove(theme);
			}

			// Unlike the programmatic cases above, this dictionary's MergedDictionaries are
			// populated by the XAML parser from a Source, which is how a real application arrives
			// at this layout — and this repo has been bitten before by XAML-backed dictionaries
			// behaving differently from code-built ones (see specs/lessons.md).
			nested = new ResourceDictionary
			{
				Source = new Uri("ms-appx:///RuntimeTests/NestedThemeFixture.xaml"),
			};
			appResources.MergedDictionaries.Add(nested);

			var found = Application.Current.GetTheme();

			Assert.IsNotNull(found,
				"GetTheme() must find a theme merged by a Source-loaded dictionary of the application's own — " +
				"the layout reported in issue #1704.");
			Assert.IsInstanceOfType(found, typeof(SimpleTheme));
		}
		finally
		{
			if (nested is not null)
			{
				appResources.MergedDictionaries.Remove(nested);
			}

			appResources.MergedDictionaries.Clear();
			foreach (var dictionary in original)
			{
				appResources.MergedDictionaries.Add(dictionary);
			}
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeIsNested_Then_StaticHelperFindsItToo()
	{
		using var _ = NestApplicationTheme(depth: 1);

		Assert.IsNotNull(SemanticThemeHelper.GetTheme(),
			"SemanticThemeHelper.GetTheme() delegates to the extension, so it inherits the same reach.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_NestedGraphHasACycle_Then_GetThemeDoesNotHang()
	{
		var appResources = Application.Current.Resources;
		var original = appResources.MergedDictionaries.ToArray();
		var themes = original.OfType<BaseTheme>().ToArray();

		var first = new ResourceDictionary();
		var second = new ResourceDictionary();
		var shared = new ResourceDictionary();

		try
		{
			foreach (var theme in themes)
			{
				appResources.MergedDictionaries.Remove(theme);
			}

			// A diamond (both branches merge one shared dictionary) with the theme at the bottom of
			// the second branch, so a recursive walk meets the same dictionary twice and must not
			// revisit it or recurse forever.
			first.MergedDictionaries.Add(shared);
			second.MergedDictionaries.Add(shared);
			foreach (var theme in themes)
			{
				second.MergedDictionaries.Add(theme);
			}

			appResources.MergedDictionaries.Add(first);
			appResources.MergedDictionaries.Add(second);

			Assert.IsNotNull(Application.Current.GetTheme(),
				"A diamond in the resource graph must not stop the walk from reaching the theme.");
		}
		finally
		{
			appResources.MergedDictionaries.Remove(first);
			appResources.MergedDictionaries.Remove(second);
			first.MergedDictionaries.Clear();
			second.MergedDictionaries.Clear();

			foreach (var theme in themes)
			{
				second.MergedDictionaries.Remove(theme);
			}

			appResources.MergedDictionaries.Clear();
			foreach (var dictionary in original)
			{
				appResources.MergedDictionaries.Add(dictionary);
			}
		}
	}
}
