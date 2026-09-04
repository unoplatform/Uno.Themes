using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the font family seam: <see cref="BaseTheme.DefaultFontFamily"/> regenerates the root
/// typeface token and every type-scale family key derived from it, at construction and at runtime,
/// while a consumer font override still wins and a URI-backed override is re-read from its source
/// on each rebuild.
/// </summary>
/// <remarks>
/// Asserted through <c>SimpleTheme</c>; the unset expectations are Simple's own single-typeface
/// declaration (Inter through its variable-font entry point), which <c>Given_Fonts</c> pins
/// independently alongside the per-scale weight tokens.
/// </remarks>
[TestClass]
public class Given_DefaultFontFamily
{
	private const string ChosenSource = "ms-appx:///TestFonts/Chosen-Variable.ttf#ChosenTest";
	private const string OtherSource = "ms-appx:///TestFonts/Other-Variable.ttf#OtherTest";

	// A real, addressable dictionary standing in for a consumer font override: it declares
	// DefaultFontFamily as Segoe UI in its theme dictionaries, so "the override won" is a value
	// neither the Simple defaults nor the sources above can produce.
	private const string SharedTypographySource =
		"ms-appx:///Uno.Themes.WinUI/Styles/Applications/Common/SharedTypography.xaml";

	private const string SegoeUi = "Segoe UI";

	private const string SimpleInter = "Inter.ttf";

	private static void InvokeHotReloadHandler(Type[] updatedTypes)
	{
		var method = typeof(BaseTheme).GetMethod(
			"UpdateApplication",
			System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
		Assert.IsNotNull(method, "BaseTheme.UpdateApplication(Type[]?) must exist for the runtime to invoke.");
		method!.Invoke(null, new object[] { updatedTypes });
	}

	private static Grid CreateContainer(SimpleTheme theme)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	private static string GetFontSource(Grid container, string key)
	{
		if (container.Resources.TryGetValue(key, out var value) && value is FontFamily family)
		{
			return family.Source;
		}

		Assert.Fail($"Resource '{key}' not found or not a FontFamily");
		return null!;
	}

	// ─────────────────────────────────────────────────────────────────────
	// One family reaches the root token and every scale generated from it.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DefaultFontFamily")]
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
	public void When_DefaultFontFamilySet_Then_EveryTypeScaleKeyFollows(string resourceKey)
	{
		var container = CreateContainer(new SimpleTheme { DefaultFontFamily = new FontFamily(ChosenSource) });

		Assert.AreEqual(ChosenSource, GetFontSource(container, resourceKey),
			$"'{resourceKey}' is generated from DefaultFontFamily and must follow it.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Unset is not the same as set-to-null: with no family the theme generates no key at all,
	// or it would shadow the design system's own declarations with nothing.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("DefaultFontFamily")]
	[DataRow("DisplayLargeFontFamily")]
	[DataRow("TitleMediumFontFamily")]
	[DataRow("BodyLargeFontFamily")]
	public void When_NoFontFamilySet_Then_ThemeDefaultsStand(string resourceKey)
	{
		var container = CreateContainer(new SimpleTheme());

		StringAssert.Contains(GetFontSource(container, resourceKey), SimpleInter,
			$"'{resourceKey}' must keep Simple's own declaration when no font family is set.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// The runtime seam: assigning after construction rebuilds the layer, and clearing
	// it hands the keys back to the design system's own declarations.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontFamilyAssignedAfterConstruction_Then_ScalesFollow()
	{
		var theme = new SimpleTheme();
		var container = CreateContainer(theme);

		StringAssert.Contains(GetFontSource(container, "DisplayLargeFontFamily"), SimpleInter);

		theme.DefaultFontFamily = new FontFamily(ChosenSource);

		Assert.AreEqual(ChosenSource, GetFontSource(container, "DisplayLargeFontFamily"),
			"Assigning the family after construction must regenerate the derived keys.");
		Assert.AreEqual(ChosenSource, GetFontSource(container, "DefaultFontFamily"),
			"The root token is generated from the one family.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontFamilyReplaced_Then_TheNewOneWins()
	{
		var theme = new SimpleTheme { DefaultFontFamily = new FontFamily(ChosenSource) };
		var container = CreateContainer(theme);

		Assert.AreEqual(ChosenSource, GetFontSource(container, "BodyLargeFontFamily"));

		theme.DefaultFontFamily = new FontFamily(OtherSource);

		Assert.AreEqual(OtherSource, GetFontSource(container, "BodyLargeFontFamily"),
			"A second assignment must replace the generated layer, not layer over it.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontFamilyCleared_Then_ThemeDefaultsReturn()
	{
		var theme = new SimpleTheme { DefaultFontFamily = new FontFamily(ChosenSource) };
		var container = CreateContainer(theme);

		Assert.AreEqual(ChosenSource, GetFontSource(container, "DisplayLargeFontFamily"));

		theme.DefaultFontFamily = null;

		StringAssert.Contains(GetFontSource(container, "DisplayLargeFontFamily"), SimpleInter,
			"Clearing the family must remove the generated keys, not leave a null shadowing them.");
		StringAssert.Contains(GetFontSource(container, "DefaultFontFamily"), SimpleInter,
			"The root token returns to the design system's own declaration.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Theme-declared alias keys: the design system's per-control lightweight keys are
	// StaticResource aliases that snapshot at parse time, so the generated layer must
	// regenerate them for a runtime change to reach control templates.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SimpleButtonFontFamily")]
	[DataRow("SimpleToggleButtonFontFamily")]
	public void When_DefaultFontFamilySet_Then_ThemeAliasKeysFollow(string resourceKey)
	{
		var theme = new SimpleTheme();
		var container = CreateContainer(theme);

		theme.DefaultFontFamily = new FontFamily(ChosenSource);

		Assert.AreEqual(ChosenSource, GetFontSource(container, resourceKey),
			$"'{resourceKey}' is an alias of the root and must follow a runtime DefaultFontFamily change.");

		theme.DefaultFontFamily = null;

		StringAssert.Contains(GetFontSource(container, resourceKey), SimpleInter,
			$"Clearing the root must hand '{resourceKey}' back to the design system's own alias.");
	}

	/// <summary>
	/// Regenerating an alias key only reaches a control if the style reads it through
	/// <c>{ThemeResource}</c>: a <c>{StaticResource}</c> setter snapshots the family at parse time
	/// and never re-resolves, so the key follows the property while the rendered button does not.
	/// </summary>
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DefaultFontFamilyChanges_Then_StyledButtonFollows()
	{
		var theme = new SimpleTheme();
		var container = CreateContainer(theme);
		var stack = new StackPanel();
		var button = new Button
		{
			Content = "probe",
			Style = (Style)container.Resources["SimpleFilledButtonStyle"],
		};
		var toggle = new ToggleButton
		{
			Content = "probe",
			Style = (Style)container.Resources["SimpleTextToggleButtonStyle"],
		};
		stack.Children.Add(button);
		stack.Children.Add(toggle);
		container.Children.Add(stack);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);

		try
		{
			StringAssert.Contains(button.FontFamily?.Source, SimpleInter,
				"The button starts on the design system's own typeface.");

			theme.DefaultFontFamily = new FontFamily(ChosenSource);

			// Setters re-resolve on a theme-change pass, the public route this seam has to content
			// that is already realized (the Design Tokens page's tuner drives the same flip).
			container.RequestedTheme = ElementTheme.Light;
			await UnitTestsUIContentHelper.WaitForIdle();
			container.RequestedTheme = ElementTheme.Dark;
			await UnitTestsUIContentHelper.WaitForIdle();

			Assert.AreEqual(ChosenSource, button.FontFamily?.Source,
				"A button styled from SimpleButtonFontFamily must re-resolve it after the family changes.");
			Assert.AreEqual(ChosenSource, toggle.FontFamily?.Source,
				"SimpleToggleButtonFontFamily is the same seam and must re-resolve too.");
		}
		finally
		{
			container.RequestedTheme = ElementTheme.Default;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Drift guard: the generated key list must cover every *FontFamily slot
	// SharedTypography.xaml declares — enumerated from the XAML itself so a slot
	// added there cannot silently escape the generated layer.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultFontFamilySet_Then_EverySharedTypographySlotFollows()
	{
		var shared = new ResourceDictionary { Source = new Uri(SharedTypographySource) };
		Assert.IsTrue(shared.ThemeDictionaries.TryGetValue("Default", out var defaultDictObj)
			&& defaultDictObj is ResourceDictionary,
			"SharedTypography.xaml must declare a Default theme dictionary.");

		var slotKeys = ((ResourceDictionary)defaultDictObj!).Keys
			.OfType<string>()
			.Where(key => key.EndsWith("FontFamily", StringComparison.Ordinal))
			.ToList();

		Assert.IsTrue(slotKeys.Count >= 20,
			$"Expected the root token plus at least 19 slot keys, found {slotKeys.Count}.");

		var container = CreateContainer(new SimpleTheme { DefaultFontFamily = new FontFamily(ChosenSource) });

		foreach (var key in slotKeys)
		{
			Assert.AreEqual(ChosenSource, GetFontSource(container, key),
				$"'{key}' is declared by SharedTypography.xaml and must follow DefaultFontFamily; " +
				"if this fails for a newly added slot, add it to ThemesConstants.TypefaceScaleKeys.");
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Appearance coverage: the generated layer must declare the root under every theme
	// dictionary a design system's own Fonts.xaml declares it under. Material and
	// Cupertino carry a HighContrast font dictionary; a generated layer without one
	// would lose to it under high contrast. Light and Dark are asserted on rendered
	// text, since ambient-theme lookups pass green with one dictionary missing.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Default")]
	[DataRow("Light")]
	[DataRow("HighContrast")]
	public void When_DefaultFontFamilySet_Then_GeneratedLayerCoversAppearance(string themeKey)
	{
		var theme = new SimpleTheme { DefaultFontFamily = new FontFamily(ChosenSource) };

		var generated = theme.MergedDictionaries.FirstOrDefault(dictionary =>
			dictionary.ThemeDictionaries.TryGetValue(themeKey, out var themed)
			&& themed is ResourceDictionary themedDictionary
			&& themedDictionary.TryGetValue("DefaultFontFamily", out var value)
			&& value is FontFamily family
			&& family.Source == ChosenSource);

		Assert.IsNotNull(generated,
			$"The generated typeface layer must declare DefaultFontFamily under '{themeKey}', " +
			"or a design system's own font dictionary for that appearance wins over it.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeChanges_Then_GeneratedFamilyResolvesUnderBothAppearances()
	{
		var container = CreateContainer(new SimpleTheme { DefaultFontFamily = new FontFamily(ChosenSource) });
		var text = new TextBlock
		{
			Text = "probe",
			Style = (Style)container.Resources["BodyMedium"],
		};
		container.Children.Add(text);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(text);

		try
		{
			container.RequestedTheme = ElementTheme.Dark;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(ChosenSource, text.FontFamily?.Source,
				"The generated Default (dark) dictionary must carry the family.");

			container.RequestedTheme = ElementTheme.Light;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(ChosenSource, text.FontFamily?.Source,
				"The generated Light dictionary must carry the family.");
		}
		finally
		{
			container.RequestedTheme = ElementTheme.Default;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Precedence: a consumer font override is merged above the generated tokens, matching
	// how a colour override beats the generated seed palette. That override is also the
	// route to changing only some scales, which the single property does not express.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontOverrideDeclaresTheToken_Then_OverrideWins()
	{
		var theme = new SimpleTheme
		{
			DefaultFontFamily = new FontFamily(ChosenSource),
			FontOverrideSource = SharedTypographySource,
		};
		var container = CreateContainer(theme);

		Assert.AreEqual(SegoeUi, GetFontSource(container, "DefaultFontFamily"),
			"A font override declaring DefaultFontFamily must beat the generated token.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontOverrideSetsOneScaleOnly_Then_TheOthersKeepTheGeneratedFamily()
	{
		var overrideDictionary = new ResourceDictionary();
		foreach (var themeKey in new[] { "Light", "Default" })
		{
			overrideDictionary.ThemeDictionaries[themeKey] = new ResourceDictionary
			{
				["DisplayLargeFontFamily"] = new FontFamily(OtherSource),
			};
		}

		var theme = new SimpleTheme
		{
			DefaultFontFamily = new FontFamily(ChosenSource),
			FontOverrideDictionary = overrideDictionary,
		};
		var container = CreateContainer(theme);

		Assert.AreEqual(OtherSource, GetFontSource(container, "DisplayLargeFontFamily"),
			"The override is the route to changing only some scales.");
		Assert.AreEqual(ChosenSource, GetFontSource(container, "BodyLargeFontFamily"),
			"A scale the override is silent about keeps the generated family.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontOverrideIsSilentOnTheToken_Then_GeneratedFamilyStands()
	{
		var overrideDictionary = new ResourceDictionary();
		foreach (var themeKey in new[] { "Light", "Default" })
		{
			overrideDictionary.ThemeDictionaries[themeKey] = new ResourceDictionary
			{
				["SimpleButtonFontFamily"] = new FontFamily("ms-appx:///TestFonts/Unrelated.ttf#Unrelated"),
			};
		}

		var theme = new SimpleTheme
		{
			DefaultFontFamily = new FontFamily(ChosenSource),
			FontOverrideDictionary = overrideDictionary,
		};
		var container = CreateContainer(theme);

		Assert.AreEqual(ChosenSource, GetFontSource(container, "DefaultFontFamily"),
			"An override that does not declare the token must leave the generated one in place.");
		Assert.AreEqual(ChosenSource, GetFontSource(container, "DisplayLargeFontFamily"));
	}

	// ─────────────────────────────────────────────────────────────────────
	// Uno.Themes#1705: a URI-backed font override is re-read from its Source on every
	// rebuild, the way the colour override already is, so a hot-reload edit to the file
	// reaches the theme instead of the load-time snapshot being re-merged.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontOverrideHasSource_Then_ItIsReResolvedOnRebuild()
	{
		var theme = new SimpleTheme { FontOverrideSource = SharedTypographySource };
		var container = CreateContainer(theme);

		Assert.AreEqual(SegoeUi, GetFontSource(container, "DefaultFontFamily"),
			"The override file declares DefaultFontFamily, so it must apply.");

		// Setting Source copies the file's entries into the instance, so the instance is a snapshot.
		// Writing a key the file does not declare stands in for the snapshot and the file having
		// diverged - which is what a hot-reload edit to the file produces.
		theme.FontOverrideDictionary["DefaultFontFamily"] = new FontFamily(ChosenSource);

		// A hot-reload pass rebuilds the dynamic layers and invalidates the resolved-override
		// cache; drive it through the real MetadataUpdateHandler entry point.
		InvokeHotReloadHandler(new[] { typeof(SimpleTheme) });

		Assert.AreEqual(SegoeUi, GetFontSource(container, "DefaultFontFamily"),
			"The hot-reload rebuild must re-read the override from its Source rather than re-merging the snapshot.");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontOverrideHasNoSource_Then_TheAssignedDictionaryIsKept()
	{
		var overrideDictionary = new ResourceDictionary
		{
			["DefaultFontFamily"] = new FontFamily(ChosenSource),
		};

		var theme = new SimpleTheme { FontOverrideDictionary = overrideDictionary };
		var container = CreateContainer(theme);

		Assert.AreEqual(ChosenSource, GetFontSource(container, "DefaultFontFamily"));

		overrideDictionary["DefaultFontFamily"] = new FontFamily(OtherSource);
		theme.DefaultCornerRadius = 8;

		Assert.AreEqual(OtherSource, GetFontSource(container, "DefaultFontFamily"),
			"An override with no Source has no file to re-read, so the assigned instance is merged as-is.");
	}
}
