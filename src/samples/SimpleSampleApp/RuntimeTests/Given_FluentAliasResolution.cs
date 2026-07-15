using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Spike S1 for specs/05-fluent-theme (spec.md §14.1): validates the platform
/// mechanism the planned Fluent theme adapter relies on — a ResourceDictionary
/// loaded via an ms-appx Source URI whose <c>StaticResource</c> aliases target
/// keys provided by XamlControlsResources merged as an app-scope sibling.
/// specs/lessons.md documents eager cross-dictionary alias resolution as a
/// real failure mode on Uno; these tests are the permanent mechanism guard.
/// </summary>
[TestClass]
public class Given_FluentAliasResolution
{
	private const string SpikeDictionarySource =
		"ms-appx:///RuntimeTests/FluentSpike/FluentAliasSpikeDictionary.xaml";

	private static ResourceDictionary LoadSpikeDictionary()
		=> new ResourceDictionary { Source = new Uri(SpikeDictionarySource) };

	private static Grid CreateContainerWithSpikeDictionary()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(LoadSpikeDictionary());
		return container;
	}

	// ─────────────────────────────────────────────────────────────────────
	// S1 case 1 — style alias to an XCR key resolves to the XCR instance
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("SpikeFilledButtonStyle", "AccentButtonStyle")]
	[DataRow("SpikeOutlinedButtonStyle", "DefaultButtonStyle")]
	public void When_StyleAlias_ToXcrKey_ResolvesToSameInstance(string aliasKey, string xcrKey)
	{
		var container = CreateContainerWithSpikeDictionary();

		var aliased = container.Resources[aliasKey] as Style;
		Assert.IsNotNull(aliased, $"{aliasKey} should resolve to a Style");
		Assert.AreEqual(typeof(Button), aliased.TargetType);

		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(xcrKey, out var xcrValue),
			$"{xcrKey} should be available from XamlControlsResources");
		Assert.AreSame(xcrValue, aliased, $"{aliasKey} should reference the same instance as {xcrKey}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// S1 case 1 (visual) — the aliased style produces accent-button visuals
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_AliasApplied_RendersLikeAccentButton()
	{
		var container = CreateContainerWithSpikeDictionary();

		var aliasButton = new Button { Content = "alias", Style = (Style)container.Resources["SpikeFilledButtonStyle"] };
		var accentButton = new Button { Content = "accent", Style = (Style)Application.Current.Resources["AccentButtonStyle"] };
		var standardButton = new Button { Content = "standard", Style = (Style)container.Resources["SpikeOutlinedButtonStyle"] };

		var panel = new StackPanel();
		panel.Children.Add(aliasButton);
		panel.Children.Add(accentButton);
		panel.Children.Add(standardButton);
		container.Children.Add(panel);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(aliasButton);
		await UnitTestsUIContentHelper.WaitForLoaded(standardButton);
		await UnitTestsUIContentHelper.WaitForIdle();

		var aliasBg = aliasButton.Background as SolidColorBrush;
		var accentBg = accentButton.Background as SolidColorBrush;
		var standardBg = standardButton.Background as SolidColorBrush;
		Assert.IsNotNull(aliasBg, "aliased accent button should have a SolidColorBrush background");
		Assert.IsNotNull(accentBg, "accent button should have a SolidColorBrush background");
		Assert.IsNotNull(standardBg, "standard button should have a SolidColorBrush background");

		Assert.AreEqual(accentBg.Color, aliasBg.Color, "alias must render with the accent background");
		Assert.AreNotEqual(standardBg.Color, aliasBg.Color, "accent and standard backgrounds must differ");
	}

	// ─────────────────────────────────────────────────────────────────────
	// S1 case 2 — alias chaining is NOT supported on Uno (spike finding)
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_AliasOfAlias_DoesNotResolve()
	{
		var container = CreateContainerWithSpikeDictionary();

		var direct = container.Resources["SpikeFilledButtonStyle"] as Style;
		Assert.IsNotNull(direct, "direct alias to an XCR key must resolve");

		// Spike S1 finding (2026-07-14): an alias whose ResourceKey is itself an
		// alias in the same dictionary does NOT resolve on Uno ("Couldn't
		// statically resolve resource" at parse time). Design consequence
		// (spec D16): every semantic key must alias a concrete style key
		// directly — never another alias. If this assertion ever fails, the
		// platform constraint has been lifted; revisit D16 before relying on it.
		container.Resources.TryGetValue("SpikeAliasOfAlias", out var chained);
		Assert.IsFalse(chained is Style,
			"alias chaining unexpectedly resolved — revisit specs/05-fluent-theme/spec.md D16");
	}

	// ─────────────────────────────────────────────────────────────────────
	// S1 case 3 — setters-only bridge style BasedOn an XCR style
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_BasedOnXcrStyle_BridgeStyleApplies()
	{
		var container = CreateContainerWithSpikeDictionary();

		var bridge = container.Resources["SpikeTextButtonStyle"] as Style;
		Assert.IsNotNull(bridge, "bridge style should resolve");
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue("DefaultButtonStyle", out var defaultStyle),
			"DefaultButtonStyle should be available from XamlControlsResources");
		Assert.AreSame(defaultStyle, bridge.BasedOn, "bridge style should be BasedOn the XCR style instance");

		var button = new Button { Content = "subtle", Style = bridge };
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		var background = button.Background as SolidColorBrush;
		Assert.IsNotNull(background, "bridge-styled button should have a SolidColorBrush background");
		Assert.AreEqual(Colors.Transparent, background.Color, "bridge setter must win over the BasedOn background at rest");
	}

	// ─────────────────────────────────────────────────────────────────────
	// S1 case 4 — per-theme-branch color aliases do NOT pick up their own
	// branch's value (spike finding; the failure mode of specs/lessons.md)
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeBranchColorAlias_BranchesResolveAmbientTheme()
	{
		var dictionary = LoadSpikeDictionary();

		var light = (ResourceDictionary)dictionary.ThemeDictionaries["Light"];
		var dark = (ResourceDictionary)dictionary.ThemeDictionaries["Default"];

		Assert.IsTrue(light.TryGetValue("SpikeOnSurfaceColor", out var lightValue), "Light branch alias should resolve");
		Assert.IsTrue(dark.TryGetValue("SpikeOnSurfaceColor", out var darkValue), "Dark branch alias should resolve");

		// Spike S1 finding (2026-07-14), matching specs/lessons.md: per-branch
		// <StaticResource> aliases resolve eagerly against the AMBIENT theme,
		// so both branches carry the same value instead of each branch's own
		// value. Design consequence (spec D6): FluentTheme's color palette is
		// built in code (mechanism C), never via per-branch XAML aliases. If
		// this assertion ever fails, branch-correct aliasing has started
		// working; revisit D6 before relying on it.
		Assert.AreEqual((Windows.UI.Color)lightValue, (Windows.UI.Color)darkValue,
			"branch aliases unexpectedly resolved per-branch — revisit specs/05-fluent-theme/spec.md D6");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeBranchBrushReferencesAliasedColor_BrushResolves()
	{
		var container = CreateContainerWithSpikeDictionary();

		var brush = container.Resources["SpikeOnSurfaceBrush"] as SolidColorBrush;
		Assert.IsNotNull(brush, "brush built from an aliased color should resolve");
		Assert.AreNotEqual(default(Windows.UI.Color), brush.Color, "brush color should carry the aliased value");
	}

	// ─────────────────────────────────────────────────────────────────────
	// S1 case 5 — FontFamily alias to the platform-default Fluent font
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_FontFamilyAlias_ResolvesToContentControlThemeFontFamily()
	{
		var container = CreateContainerWithSpikeDictionary();

		var aliased = container.Resources["SpikeBodyFontFamily"] as FontFamily;
		Assert.IsNotNull(aliased, "FontFamily alias should resolve");

		Assert.IsTrue(
			Application.Current.Resources.TryGetValue("ContentControlThemeFontFamily", out var xcrValue),
			"ContentControlThemeFontFamily should be available");
		var target = xcrValue as FontFamily;
		Assert.IsNotNull(target);
		Assert.AreEqual(target.Source, aliased.Source, "alias should carry the platform-default font source");
	}

	// ─────────────────────────────────────────────────────────────────────
	// S1 case 6 — core XCR style keys the v1 mapping hard-depends on (S3-core)
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("AccentButtonStyle")]
	[DataRow("DefaultButtonStyle")]
	[DataRow("DefaultTextBoxStyle")]
	[DataRow("DefaultCheckBoxStyle")]
	[DataRow("DefaultContentDialogStyle")]
	public void When_CoreXcrStyleKey_IsPresent(string key)
	{
		Assert.IsTrue(
			Application.Current.Resources.TryGetValue(key, out var value),
			$"{key} should be provided by XamlControlsResources");
		Assert.IsInstanceOfType(value, typeof(Style), $"{key} should be a Style");
	}

	// ─────────────────────────────────────────────────────────────────────
	// S2/S3 discovery — probes every candidate key from spec §5.2/§6.3 and
	// writes a report consumed by specs/05-fluent-theme/spike-results.md.
	// Asserts only the report itself; per-key promotion to hard asserts
	// happens in the spec once availability is recorded (spike S3).
	// ─────────────────────────────────────────────────────────────────────

	private static readonly string[] _candidateStyleKeys =
	{
		"AccentButtonStyle", "DefaultButtonStyle", "DefaultToggleButtonStyle",
		"DefaultTextBoxStyle", "DefaultPasswordBoxStyle",
		"DefaultComboBoxStyle", "DefaultComboBoxItemStyle",
		"DefaultCheckBoxStyle", "DefaultRadioButtonStyle",
		"DefaultToggleSwitchStyle", "DefaultSliderStyle",
		"DefaultProgressBarStyle", "DefaultProgressRingStyle",
		"DefaultListViewStyle", "DefaultListViewItemStyle",
		"DefaultContentDialogStyle", "DefaultCommandBarStyle",
		"DefaultAppBarButtonStyle", "DefaultHyperlinkButtonStyle",
		"DefaultCalendarViewStyle", "DefaultCalendarDatePickerStyle",
		"DefaultDatePickerStyle", "DefaultTimePickerStyle",
		"DefaultPipsPagerStyle", "DefaultRatingControlStyle",
		"DefaultFlyoutPresenterStyle", "DefaultMenuFlyoutPresenterStyle",
		"DefaultMenuFlyoutItemStyle", "DefaultMenuFlyoutSeparatorStyle",
		"DefaultMenuFlyoutSubItemStyle", "DefaultToggleMenuFlyoutItemStyle",
		"DefaultRadioMenuFlyoutItemStyle",
		"DefaultNavigationViewStyle", "DefaultNavigationViewItemStyle",
		"DefaultMediaTransportControlsStyle", "DefaultAutoSuggestBoxStyle",
		"TextBlockButtonStyle",
		"CaptionTextBlockStyle", "BodyTextBlockStyle", "BodyStrongTextBlockStyle",
		"SubtitleTextBlockStyle", "TitleTextBlockStyle", "TitleLargeTextBlockStyle",
		"DisplayTextBlockStyle",
	};

	private static readonly string[] _candidateColorKeys =
	{
		"SystemAccentColor",
		"SystemAccentColorLight1", "SystemAccentColorLight2", "SystemAccentColorLight3",
		"SystemAccentColorDark1", "SystemAccentColorDark2", "SystemAccentColorDark3",
		"TextFillColorPrimary", "TextFillColorSecondary", "TextFillColorTertiary",
		"TextFillColorDisabled", "TextFillColorInverse",
		"TextOnAccentFillColorPrimary", "TextOnAccentFillColorSecondary",
		"AccentTextFillColorPrimary", "AccentTextFillColorSecondary", "AccentTextFillColorTertiary",
		"AccentFillColorDefaultBrush", "AccentFillColorSecondaryBrush", "AccentFillColorTertiaryBrush",
		"SolidBackgroundFillColorBase", "SolidBackgroundFillColorSecondary",
		"SolidBackgroundFillColorTertiary", "SolidBackgroundFillColorQuarternary",
		"CardBackgroundFillColorDefault", "CardBackgroundFillColorSecondary",
		"ControlFillColorDefault", "ControlFillColorSecondary", "ControlFillColorTertiary",
		"ControlStrokeColorDefault", "ControlStrongStrokeColorDefault",
		"DividerStrokeColorDefault", "CardStrokeColorDefault",
		"SubtleFillColorTransparent", "SubtleFillColorSecondary",
		"SystemFillColorCritical", "SystemFillColorCriticalBackground",
		"SystemFillColorSuccess", "SystemFillColorCaution", "SystemFillColorAttention",
		"ContentControlThemeFontFamily",
	};

	[TestMethod]
	[RunsOnUIThread]
	public void When_ProbingCandidateXcrKeys_ReportIsWritten()
	{
		var report = new StringBuilder();
		report.AppendLine("# Fluent spike discovery report (S2/S3)");
		report.AppendLine($"# Generated {DateTimeOffset.Now:O}; theme = {Application.Current.RequestedTheme}");
		report.AppendLine();
		report.AppendLine("## Style keys (S3)");

		foreach (var key in _candidateStyleKeys)
		{
			var found = Application.Current.Resources.TryGetValue(key, out var value);
			report.AppendLine($"{key} = {(found ? value?.GetType().Name ?? "null" : "MISSING")}");
		}

		report.AppendLine();
		report.AppendLine("## Color/brush/font tokens (S2)");

		foreach (var key in _candidateColorKeys)
		{
			var found = Application.Current.Resources.TryGetValue(key, out var value);
			var rendered = value switch
			{
				Windows.UI.Color color => color.ToString(),
				SolidColorBrush brush => $"SolidColorBrush({brush.Color}, opacity {brush.Opacity})",
				FontFamily font => $"FontFamily({font.Source})",
				null => "null",
				_ => value.GetType().Name,
			};
			report.AppendLine($"{key} = {(found ? rendered : "MISSING")}");
		}

		var path = Path.Combine(Path.GetTempPath(), "fluent-spike-report.txt");
		File.WriteAllText(path, report.ToString());

		Assert.IsTrue(File.Exists(path), $"discovery report should be written to {path}");
	}
}
