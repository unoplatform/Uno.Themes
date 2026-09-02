using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Regression tests for font bundling in Uno.Simple.
/// Verifies the single-typeface contract: every font family resource resolves to the bundled
/// Inter variable-font entry point (<c>Inter.ttf</c>, whose weights resolve via the font manifest
/// on static-font platforms), the per-scale weight nuance lives in the <c>*FontWeight</c>
/// tokens rather than in weight-specific font files, and overriding the single root token
/// cascades to the scales.
/// </summary>
[TestClass]
public class Given_Fonts
{
	private const string InterEntryPoint = "ms-appx:///Uno.Fonts.Inter/Fonts/Inter.ttf#Inter";

	// A family that does not exist anywhere: what a missing entry point degrades to.
	private const string MissingEntryPoint = "ms-appx:///Uno.Fonts.Inter/Fonts/DoesNotExist.ttf#Missing";

	// A distinctive override root; only its Source string is asserted, it never has to load.
	private const string OverrideEntryPoint = "ms-appx:///Assets/Fonts/Probe.ttf#Probe";

	private static Grid CreateThemedContainer(SimpleTheme? theme = null)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme ?? new SimpleTheme());
		return container;
	}

	private static FontFamily GetFontFamily(Grid container, string key)
		=> GetFontFamily(container.Resources, key);

	private static FontFamily GetFontFamily(ResourceDictionary resources, string key)
	{
		if (resources.TryGetValue(key, out var value) && value is FontFamily ff)
		{
			return ff;
		}

		Assert.Fail($"Resource '{key}' not found or not of type FontFamily");
		return null!;
	}

	private static string GetString(Grid container, string key)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is string s)
		{
			return s;
		}

		Assert.Fail($"Resource '{key}' not found or not of type string");
		return null!;
	}

	/// <summary>
	/// Builds a consumer-style font override that redefines the root token under both appearances.
	/// </summary>
	private static ResourceDictionary CreateRootOverride(string source)
	{
		var overrideDict = new ResourceDictionary();
		foreach (var themeKey in new[] { "Light", "Default" })
		{
			var themed = new ResourceDictionary();
			themed["DefaultFontFamily"] = new FontFamily(source);
			overrideDict.ThemeDictionaries[themeKey] = themed;
		}
		return overrideDict;
	}

	private static TextBlock CreateBodyMediumProbe(Grid container, ResourceDictionary styleSource)
	{
		var text = new TextBlock
		{
			Text = "probe",
			Style = (Style)styleSource["BodyMedium"],
		};
		container.Children.Add(text);
		return text;
	}

	/// <summary>
	/// Skia loads file fonts asynchronously and re-measures text when they land; polls until the
	/// condition holds or a bounded wait elapses, so the assertion that follows judges settled layout.
	/// </summary>
	private static async Task WaitForFontLoad(Func<bool> settled)
	{
		for (var attempt = 0; attempt < 50 && !settled(); attempt++)
		{
			await Task.Delay(100);
			await UnitTestsUIContentHelper.WaitForIdle();
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Font URI: the root token and the legacy key must use the ms-appx:/// URI of the
	// bundled Inter entry point (not a bare "Inter" system name, and not a weight-baked file).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DefaultFontFamily")]
	[DataRow("SimpleFontFamily")]
	public void When_SimpleThemeLoaded_Then_FontFamilyUsesInterEntryPoint(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		Assert.AreEqual(
			InterEntryPoint,
			fontFamily.Source,
			$"Font resource '{resourceKey}' must reference the bundled Inter entry point, " +
			$"so its weights resolve from the single family. Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Typography scale: every type-scale slot derives from the single root.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLargeFontFamily")]
	[DataRow("DisplayMediumFontFamily")]
	[DataRow("DisplaySmallFontFamily")]
	[DataRow("HeadlineLargeFontFamily")]
	[DataRow("HeadlineMediumFontFamily")]
	[DataRow("HeadlineSmallFontFamily")]
	[DataRow("TitleLargeFontFamily")]
	[DataRow("TitleMediumFontFamily")]
	[DataRow("TitleSmallFontFamily")]
	[DataRow("LabelLargeFontFamily")]
	[DataRow("LabelMediumFontFamily")]
	[DataRow("LabelSmallFontFamily")]
	[DataRow("LabelExtraSmallFontFamily")]
	[DataRow("BodyLargeFontFamily")]
	[DataRow("BodyMediumFontFamily")]
	[DataRow("BodySmallFontFamily")]
	[DataRow("CaptionLargeFontFamily")]
	[DataRow("CaptionMediumFontFamily")]
	[DataRow("CaptionSmallFontFamily")]
	public void When_SimpleThemeLoaded_Then_TypographyScaleDerivesFromRoot(string resourceKey)
	{
		var container = CreateThemedContainer();
		var fontFamily = GetFontFamily(container, resourceKey);

		Assert.AreEqual(
			InterEntryPoint,
			fontFamily.Source,
			$"Typography resource '{resourceKey}' must derive from the single DefaultFontFamily root. " +
			$"Actual Source: '{fontFamily.Source}'");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Weight tokens: Simple's per-scale weight nuance (Bold display, SemiBold titles)
	// is carried by the *FontWeight tokens, resolved from the single family.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DisplayLargeFontWeight", "Bold")]
	[DataRow("DisplayMediumFontWeight", "Bold")]
	[DataRow("DisplaySmallFontWeight", "Normal")]
	[DataRow("HeadlineMediumFontWeight", "SemiBold")]
	[DataRow("TitleLargeFontWeight", "SemiBold")]
	[DataRow("TitleMediumFontWeight", "SemiBold")]
	[DataRow("LabelLargeFontWeight", "SemiBold")]
	[DataRow("BodyLargeFontWeight", "Normal")]
	[DataRow("CaptionMediumFontWeight", "Normal")]
	// Control-level weight tokens: buttons kept the Medium the old Inter-Medium-baked family
	// carried, and the named weights consumed by Expander/CalendarView are now actually defined.
	[DataRow("SimpleButtonFontWeight", "Medium")]
	[DataRow("SimpleToggleButtonFontWeight", "Medium")]
	[DataRow("SimpleRegularFontWeight", "Normal")]
	[DataRow("SimpleSemiBoldFontWeight", "SemiBold")]
	public void When_SimpleThemeLoaded_Then_WeightTokenCarriesTheScaleNuance(
		string resourceKey, string expectedWeight)
	{
		var container = CreateThemedContainer();

		Assert.AreEqual(
			expectedWeight,
			GetString(container, resourceKey),
			$"'{resourceKey}' must carry Simple's per-scale weight nuance now that the family is shared.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Light and Dark both declare the root: a theme-dictionary entry dropped from
	// Simple's Fonts.xaml would let the scales fall through to the Segoe UI baseline
	// that SharedTypography.xaml declares for that appearance.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeChanges_Then_RootResolvesUnderBothAppearances()
	{
		var container = CreateThemedContainer();
		var text = CreateBodyMediumProbe(container, container.Resources);

		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(text);

			container.RequestedTheme = ElementTheme.Dark;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(InterEntryPoint, text.FontFamily?.Source,
				"The Default (dark) theme dictionary must declare the Inter root.");

			container.RequestedTheme = ElementTheme.Light;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(InterEntryPoint, text.FontFamily?.Source,
				"The Light theme dictionary must declare the Inter root.");
		}
		finally
		{
			container.RequestedTheme = ElementTheme.Default;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Root cascade: the documented single-key font swap. A consumer override that
	// redefines DefaultFontFamily on the application-level theme must reach the
	// type scales (whose *FontFamily keys are StaticResource aliases of the root)
	// under both appearances, and clearing it must hand the scales back to Inter.
	//
	// Application level is the scope that matters: an alias target is resolved
	// against the application scope, so the same override on a theme merged into
	// a scoped container does not reach the aliases (specs/lessons.md).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_RootOverriddenOnApplicationTheme_Then_ScaleFollowsAndClears()
	{
		var theme = Application.Current.GetTheme();
		Assert.IsNotNull(theme, "The sample app merges a SimpleTheme at the application level.");

		var appResources = Application.Current.Resources;
		var container = new Grid();
		var text = CreateBodyMediumProbe(container, appResources);

		try
		{
			theme.FontOverrideDictionary = CreateRootOverride(OverrideEntryPoint);

			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(text);

			Assert.AreEqual(OverrideEntryPoint, GetFontFamily(appResources, "BodyMediumFontFamily").Source,
				"Overriding DefaultFontFamily must cascade to the type-scale slot resource.");

			foreach (var appearance in new[] { ElementTheme.Dark, ElementTheme.Light })
			{
				container.RequestedTheme = appearance;
				await UnitTestsUIContentHelper.WaitForIdle();
				Assert.AreEqual(OverrideEntryPoint, text.FontFamily?.Source,
					$"A BodyMedium TextBlock must render with the overridden root under {appearance}.");
			}

			// Clear: the override layer is dropped on the rebuild; the appearance flip makes the
			// already-rendered TextBlock re-resolve its ThemeResource setters.
			theme.FontOverrideDictionary = null;
			container.RequestedTheme = ElementTheme.Dark;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(InterEntryPoint, GetFontFamily(appResources, "BodyMediumFontFamily").Source,
				"Clearing the font override must hand the type-scale slot back to the Inter root.");
			Assert.AreEqual(InterEntryPoint, text.FontFamily?.Source,
				"Clearing the font override must hand rendered text back to the Inter root.");
		}
		finally
		{
			theme.FontOverrideDictionary = null;
			container.RequestedTheme = ElementTheme.Default;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Rendered-weight guard: the whole single-family design hangs on FontWeight
	// resolving a real weight from the one entry point (variable font, or its
	// font manifest on static-font platforms). Every other test asserts resource
	// strings; this one asserts glyphs, and it is the merge gate on
	// unoplatform/uno.fonts#75 made visible: a missing Inter.ttf silently falls
	// back to the platform default, which also has a Bold face, so Bold-vs-Normal
	// alone stays green. The probe therefore first proves the entry point loaded
	// at all by measuring it against a family that is known not to exist.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_WeightsDiffer_Then_RenderedWidthsDiffer()
	{
		const string probe = "Weight probe 0123456789";
		var inter = new FontFamily(InterEntryPoint);

		var normal = new TextBlock { Text = probe, FontFamily = inter, FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Normal };
		var bold = new TextBlock { Text = probe, FontFamily = inter, FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Bold };
		var fallback = new TextBlock { Text = probe, FontFamily = new FontFamily(MissingEntryPoint), FontSize = 24, FontWeight = Microsoft.UI.Text.FontWeights.Normal };

		var container = CreateThemedContainer();
		container.Children.Add(new StackPanel { Children = { normal, bold, fallback } });

		try
		{
			UnitTestsUIContentHelper.Content = container;
			await UnitTestsUIContentHelper.WaitForLoaded(fallback);
			await WaitForFontLoad(() => normal.ActualWidth != fallback.ActualWidth && normal.ActualWidth != bold.ActualWidth);

			Assert.AreNotEqual(fallback.ActualWidth, normal.ActualWidth,
				"The Inter entry point did not load: its text measures exactly like a family that does not " +
				"exist, so both fell back to the platform default. The referenced Uno.Fonts.Inter package is " +
				"missing the Inter.ttf entry point (unoplatform/uno.fonts#75).");
			Assert.AreNotEqual(normal.ActualWidth, bold.ActualWidth,
				"Bold and Normal must select different faces from the single Inter reference. " +
				"Equal widths mean FontWeight is being ignored: the Inter.ttf entry point loaded " +
				"but its font manifest is missing.");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
