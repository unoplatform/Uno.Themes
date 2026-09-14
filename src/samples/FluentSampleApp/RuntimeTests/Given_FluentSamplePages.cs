using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Inflates every shared sample page that declares <see cref="Design.Fluent"/> inside the Fluent
/// head — where <c>FluentTheme</c> is the app-level theme — and checks that the page's
/// <c>FluentTemplate</c> exists, is the template presented, and realizes without throwing.
/// </summary>
/// <remarks>
/// Lives in this head rather than the CI host (SimpleSampleApp) on purpose: the assertion is about
/// the Fluent template resolving against an app-level <c>FluentTheme</c>, which only this head
/// provides (AGENTS §5 placement rule; <c>MaterialSampleApp/RuntimeTests</c> is the precedent).
/// A page opted into <c>Design.Fluent</c> without a Fluent template would otherwise fall back to
/// the first defined template (Material) and log unresolved <c>Material*</c> keys at runtime — the
/// state the Overview page shipped in before 2026-09-06.
/// </remarks>
[TestClass]
public class Given_FluentSamplePages
{
	private const string FluentPresenterName = "FluentContentContainer";

	private static IEnumerable<(Type Type, SamplePageAttribute Attribute)> FluentPages()
		=> typeof(SamplePageLayout).Assembly.DefinedTypes
			.Where(t => t.Namespace?.StartsWith("Uno.Themes.Samples") == true)
			.Select(t => (Type: t.AsType(), Attribute: t.GetCustomAttribute<SamplePageAttribute>()))
			.Where(x => x.Attribute is { } attribute && attribute.SupportedDesigns.Contains(Design.Fluent))
			.OrderBy(x => x.Type.Name);

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_PageDeclaresFluent_ItsFluentTemplateIsPresented()
	{
		var pages = FluentPages().ToList();
		Assert.IsTrue(pages.Count > 0, "the shared sample assembly should declare Fluent-enabled pages");

		// SeedColorSamplePage applies its last seed to the app theme when it loads; restore afterwards.
		var seedBefore = SemanticThemeHelper.PrimarySeed;
		var failures = new StringBuilder();
		try
		{
			foreach (var (type, attribute) in pages)
			{
				try
				{
					if (Activator.CreateInstance(type) is not Page page)
					{
						failures.AppendLine($"{type.Name}: could not be instantiated as a Page");
						continue;
					}

					page.DataContext = new Sample(attribute, type);

					UnitTestsUIContentHelper.Content = page;
					await UnitTestsUIContentHelper.WaitForLoaded(page);
					await UnitTestsUIContentHelper.WaitForIdle();

					// Pages without a SamplePageLayout (e.g. SeedColorSamplePage) are design-agnostic:
					// inflating under the Fluent theme is the whole check.
					if (page.FindFirstDescendant<SamplePageLayout>() is { } layout)
					{
						Assert.IsNotNull(layout.FluentTemplate,
							$"{type.Name} declares Design.Fluent but defines no FluentTemplate");

						var presenter = layout.FindFirstDescendant<ContentPresenter>(x => x.Name == FluentPresenterName);
						Assert.IsNotNull(presenter, $"{type.Name}: the SamplePageLayout template should expose {FluentPresenterName}");
						Assert.AreEqual(Visibility.Visible, presenter.Visibility,
							$"{type.Name}: the Fluent presenter must be the one shown (Fluent visual state)");
						Assert.IsTrue(VisualTreeHelper.GetChildrenCount(presenter) > 0,
							$"{type.Name}: the Fluent template should have realized content");
					}
				}
				catch (Exception e) when (e is not OutOfMemoryException)
				{
					failures.AppendLine($"{type.Name}: {e.GetType().Name}: {e.Message}");
				}
				finally
				{
					UnitTestsUIContentHelper.Content = null;
				}
			}
		}
		finally
		{
			SemanticThemeHelper.PrimarySeed = seedBefore;
		}

		Assert.AreEqual(0, failures.Length,
			$"{pages.Count} Fluent-enabled pages inflated; failures:\n{failures}");
	}

	[TestMethod]
	public void When_CorePagesDeclareFluent()
	{
		// The pages this head must always show: the landing page and the four style pages.
		var names = FluentPages().Select(x => x.Type.Name).ToHashSet();
		foreach (var expected in new[] { "OverviewPage", "SemanticStylingSamplePage", "ColorsSamplePage", "SeedColorSamplePage", "DesignTokensSamplePage" })
		{
			Assert.IsTrue(names.Contains(expected), $"{expected} should declare Design.Fluent");
		}
	}
}
