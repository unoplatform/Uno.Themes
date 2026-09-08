using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the FluentTheme semantic color palette (specs/05-fluent-theme, §6):
/// the accent-derived roles are built in code per theme branch ("mechanism C",
/// D6) from the live XamlControlsResources token values — per-branch XAML
/// aliases are broken on Uno (see
/// Given_FluentAliasResolution.When_ThemeBranchColorAlias_*) — and the static
/// neutral roles are copied from the declarative ColorPalette.xaml.
///
/// Expected values are read from the live XCR resources (never baked hexes) so
/// the tests keep passing across Uno.UI accent/token value changes while still
/// failing fast if the mapping or the branch mechanics regress.
/// </summary>
[TestClass]
public class Given_FluentColorPalette
{
	// The semantic color roles the Fluent palette must carry in each theme branch.
	// ShadowColor is intentionally absent: Fluent has no shadow token, the shared
	// default applies (spec §6.3).
	private static readonly string[] _mappedRoles =
	{
		"PrimaryColor", "OnPrimaryColor", "PrimaryContainerColor", "OnPrimaryContainerColor",
		"PrimaryInverseColor", "PrimaryVariantDarkColor", "PrimaryVariantLightColor",
		"SecondaryColor", "OnSecondaryColor", "SecondaryContainerColor", "OnSecondaryContainerColor",
		"SecondaryVariantDarkColor", "SecondaryVariantLightColor",
		"TertiaryColor", "OnTertiaryColor", "TertiaryContainerColor", "OnTertiaryContainerColor",
		"ErrorColor", "OnErrorColor", "ErrorContainerColor", "OnErrorContainerColor",
		"BackgroundColor", "OnBackgroundColor",
		"SurfaceColor", "OnSurfaceColor", "SurfaceVariantColor", "OnSurfaceVariantColor",
		"SurfaceInverseColor", "OnSurfaceInverseColor", "SurfaceTintColor",
		"OutlineColor", "OutlineVariantColor",
	};

	private static Grid CreateThemedContainer(out FluentTheme theme)
	{
		theme = new FluentTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	private static Color GetAmbientColor(string key)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(key, out var value) && value is Color,
			$"{key} should be resolvable from XamlControlsResources");
		return (Color)value;
	}

	/// <summary>
	/// Locates the Fluent palette's theme branch inside the theme's (public)
	/// merged-dictionaries structure: the dictionary whose Light/Default branches
	/// carry the semantic role colors written by mechanism C.
	/// </summary>
	private static ResourceDictionary FindPaletteBranch(ResourceDictionary root, string branchKey)
		=> TryFindPaletteBranch(root, branchKey)
			?? throw new AssertFailedException($"the Fluent palette should expose a populated '{branchKey}' theme branch");

	private static ResourceDictionary? TryFindPaletteBranch(ResourceDictionary dictionary, string branchKey)
	{
		if (dictionary.ThemeDictionaries.TryGetValue(branchKey, out var branchCandidate)
			&& branchCandidate is ResourceDictionary branch
			&& branch.TryGetValue("PrimaryColor", out var primary)
			&& primary is Color)
		{
			return branch;
		}

		// Search LATER siblings first: they win at lookup time, and the Fluent
		// palette is merged above the shared (M3) palette which also carries
		// PrimaryColor branches.
		for (var i = dictionary.MergedDictionaries.Count - 1; i >= 0; i--)
		{
			if (TryFindPaletteBranch(dictionary.MergedDictionaries[i], branchKey) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}

	// ─────────────────────────────────────────────────────────────────────
	// Branch correctness (the D6 core claim): each branch carries ITS OWN
	// accent mapping — light PrimaryColor is the base accent, dark PrimaryColor
	// is SystemAccentColorLight2 (Fluent's dark-theme accent fill).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeCreated_BranchesCarryBranchCorrectAccents()
	{
		CreateThemedContainer(out var theme);

		var light = FindPaletteBranch(theme, "Light");
		var dark = FindPaletteBranch(theme, "Default");

		Assert.AreEqual(GetAmbientColor("SystemAccentColor"), (Color)light["PrimaryColor"],
			"light-branch PrimaryColor must be the base system accent");
		Assert.AreEqual(GetAmbientColor("SystemAccentColorLight2"), (Color)dark["PrimaryColor"],
			"dark-branch PrimaryColor must be SystemAccentColorLight2 (Fluent's dark accent fill)");

		Assert.AreEqual(GetAmbientColor("SystemAccentColorDark1"), (Color)light["SecondaryColor"],
			"light-branch SecondaryColor must be SystemAccentColorDark1");
		Assert.AreEqual(GetAmbientColor("SystemAccentColorLight1"), (Color)dark["SecondaryColor"],
			"dark-branch SecondaryColor must be SystemAccentColorLight1");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeCreated_NeutralRolesDifferPerBranch()
	{
		CreateThemedContainer(out var theme);

		var light = FindPaletteBranch(theme, "Light");
		var dark = FindPaletteBranch(theme, "Default");

		// The exact failure mode of per-branch XAML aliases (S1 case 4) was both
		// branches carrying the AMBIENT theme's value — neutrals must differ.
		Assert.AreNotEqual((Color)light["BackgroundColor"], (Color)dark["BackgroundColor"],
			"light and dark BackgroundColor must carry their own branch's value");
		Assert.AreNotEqual((Color)light["OnSurfaceColor"], (Color)dark["OnSurfaceColor"],
			"light and dark OnSurfaceColor must carry their own branch's value");

		// SurfaceInverse crosses branches: each branch carries the OTHER branch's base.
		Assert.AreEqual((Color)dark["BackgroundColor"], (Color)light["SurfaceInverseColor"],
			"light SurfaceInverseColor must be the dark-branch background");
		Assert.AreEqual((Color)light["BackgroundColor"], (Color)dark["SurfaceInverseColor"],
			"dark SurfaceInverseColor must be the light-branch background");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeCreated_EveryMappedRoleIsPresentInBothBranches()
	{
		CreateThemedContainer(out var theme);

		foreach (var branchKey in new[] { "Light", "Default" })
		{
			var branch = FindPaletteBranch(theme, branchKey);
			foreach (var role in _mappedRoles)
			{
				Assert.IsTrue(
					branch.TryGetValue(role, out var value) && value is Color,
					$"{role} should be mapped in the '{branchKey}' branch — a Fluent token disappeared or the mapping regressed");
			}
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Drift guard: the neutral role values are S2-captured literals declared in
	// ColorPalette.xaml (Uno exposes no per-branch token API — see
	// FluentColorPalette). The ambient branch is asserted against the LIVE
	// platform values on every run, so an Uno.UI update that changes a Fluent
	// token fails fast here.
	// ─────────────────────────────────────────────────────────────────────

	private static bool IsAmbientDark =>
		Application.Current.RequestedTheme == ApplicationTheme.Dark;

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("OnSurfaceColor", "TextFillColorPrimary")]
	[DataRow("OnBackgroundColor", "TextFillColorPrimary")]
	[DataRow("OnSurfaceVariantColor", "TextFillColorSecondary")]
	[DataRow("OnSurfaceInverseColor", "TextFillColorInverse")]
	[DataRow("OnPrimaryColor", "TextOnAccentFillColorPrimary")]
	[DataRow("ErrorColor", "SystemFillColorCritical")]
	[DataRow("ErrorContainerColor", "SystemFillColorCriticalBackground")]
	[DataRow("BackgroundColor", "SolidBackgroundFillColorBase")]
	[DataRow("SurfaceVariantColor", "SolidBackgroundFillColorSecondary")]
	[DataRow("SurfaceColor", "SolidBackgroundFillColorTertiary")]
	[DataRow("OutlineColor", "ControlStrongStrokeColorDefault")]
	[DataRow("OutlineVariantColor", "DividerStrokeColorDefault")]
	public void When_AmbientBranch_MatchesLivePlatformTokens(string role, string token)
	{
		CreateThemedContainer(out var theme);

		var branch = FindPaletteBranch(theme, IsAmbientDark ? "Default" : "Light");

		Assert.AreEqual(GetAmbientColor(token), (Color)branch[role],
			$"{role} (baked S2 capture) must match the live {token} value — an Uno.UI update likely " +
			"changed the token; re-capture the values in ColorPalette.xaml");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_Resolving_PrimaryColor_MatchesAmbientAccentMapping()
	{
		var container = CreateThemedContainer(out _);

		var expected = GetAmbientColor(IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColor");

		Assert.IsTrue(
			container.Resources.TryGetValue("PrimaryColor", out var value) && value is Color,
			"PrimaryColor should resolve under FluentTheme");
		Assert.AreEqual(expected, (Color)value,
			"PrimaryColor must carry the ambient branch's Fluent accent mapping");
	}

	// The generated semantic brushes (SharedColors.xaml) materialize their
	// {StaticResource *Color} reference ONCE, against the app-level scope (see
	// the architecture note atop Given_ColorOverridePrecedence) — under a
	// container-scoped theme they carry the app-level theme's palette. The
	// value assertion therefore runs in the documented consumer topology:
	// FluentTheme merged at app scope, after XamlControlsResources.
	[TestMethod]
	[RunsOnUIThread]
	[DataRow("PrimaryBrush", null)]
	[DataRow("OnSurfaceBrush", "TextFillColorPrimary")]
	[DataRow("OutlineBrush", "ControlStrongStrokeColorDefault")]
	public void When_Resolving_SemanticBrush_CarriesMappedTokenValue(string brushKey, string? token)
	{
		var expected = token is null
			? GetAmbientColor(IsAmbientDark ? "SystemAccentColorLight2" : "SystemAccentColor")
			: GetAmbientColor(token);

		var appDictionaries = Application.Current.Resources.MergedDictionaries;
		var theme = new FluentTheme();
		appDictionaries.Add(theme);
		try
		{
			Assert.IsTrue(
				Application.Current.Resources.TryGetValue(brushKey, out var value),
				$"{brushKey} should resolve under an app-scope FluentTheme");
			var brush = value as SolidColorBrush;
			Assert.IsNotNull(brush, $"{brushKey} should be a SolidColorBrush");
			Assert.AreEqual(expected, brush.Color,
				$"{brushKey} must carry the mapped Fluent token value for the ambient theme");
		}
		finally
		{
			appDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_FilledButtonRendered_BackgroundIsFluentAccent()
	{
		var container = CreateThemedContainer(out _);

		var button = new Button
		{
			Content = "accent",
			Style = (Style)container.Resources["FilledButtonStyle"],
		};
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.IsTrue(
			Application.Current.Resources.TryGetValue("AccentFillColorDefaultBrush", out var accentValue)
				&& accentValue is SolidColorBrush,
			"AccentFillColorDefaultBrush should be provided by XamlControlsResources");

		var background = button.Background as SolidColorBrush;
		Assert.IsNotNull(background, "FilledButtonStyle button should have a SolidColorBrush background");
		Assert.AreEqual(((SolidColorBrush)accentValue).Color, background.Color,
			"the semantic Filled button must render with Fluent's accent fill");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Consumer constructor override layers above the Fluent palette
	// (same topology as SimpleTheme's grayscale palette).
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_ConstructorColorOverrideProvided_ItWinsOverFluentPalette()
	{
		var overrideColor = Color.FromArgb(0xFF, 0x21, 0x96, 0xF3);
		var overrideDict = new ResourceDictionary();
		foreach (var themeKey in new[] { "Light", "Default" })
		{
			var themed = new ResourceDictionary();
			themed["PrimaryColor"] = overrideColor;
			overrideDict.ThemeDictionaries[themeKey] = themed;
		}

		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new FluentTheme(colorOverride: overrideDict));

		Assert.IsTrue(
			container.Resources.TryGetValue("PrimaryColor", out var value) && value is Color,
			"PrimaryColor should resolve under FluentTheme");
		Assert.AreEqual(overrideColor, (Color)value,
			"a constructor color override must win over the code-built Fluent palette");
	}
}
