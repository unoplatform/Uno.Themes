#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Omarchy;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the <see cref="SemanticResources"/> marker on <see cref="OmarchyTheme"/>: every semantic
/// (theme-agnostic) key is declared in a <see cref="SemanticResources"/> dictionary or one of its
/// theme dictionaries, and no Omarchy-prefixed key is. Omarchy's lightweight keys alias the brushes
/// <see cref="OmarchyTheme"/> generates from its palette, so these tests also guard that those
/// aliases still resolve — and still follow a palette change — from the standalone semantic file.
/// </summary>
[TestClass]
public class Given_SemanticResources
{
	/// <summary>One representative key per semantic layer: style alias, typography alias, colour, brush, spacing, shape, density, typeface.</summary>
	private static readonly string[] SemanticKeys =
	{
		"FilledButtonStyle",
		"BodyMedium",
		"PrimaryColor",
		"PrimaryBrush",
		"Space400Thickness",
		"Radius100CornerRadius",
		"ControlHeightSmall",
		"BodyMediumFontFamily",
	};

	/// <summary>Keys whose only declaration is the semantic file, so they must not exist in a plain dictionary either.</summary>
	private static readonly string[] SemanticOnlyKeys = { "FilledButtonStyle", "BodyMedium", "FilledButtonBackground", "HyperlinkUnderlineVisible" };

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_SemanticKeysAreInSemanticResources()
	{
		var (semantic, other) = Collect(new OmarchyTheme());

		foreach (var key in SemanticKeys)
		{
			Assert.IsTrue(semantic.Contains(key), $"'{key}' should be declared in a SemanticResources dictionary");
		}

		Assert.IsTrue(other.Contains("OmarchyFilledButtonStyle"), "the theme-specific style should stay in a plain dictionary");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_SemanticOnlyKeysAreNotInPlainDictionaries()
	{
		// Guards the regression this marker exists to prevent: an alias or lightweight block re-added
		// to the merged bundle would leave the keys present twice and every other assertion green.
		var (_, other) = Collect(new OmarchyTheme());

		foreach (var key in SemanticOnlyKeys)
		{
			Assert.IsFalse(other.Contains(key), $"'{key}' should only be declared in SemanticResources");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_NoThemePrefixedKeyIsInSemanticResources()
	{
		var (semantic, _) = Collect(new OmarchyTheme());

		AssertNoPrefixedKey(semantic);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultFontFamilySet_Then_NoThemePrefixedKeyIsInSemanticResources()
	{
		var (semantic, _) = Collect(new OmarchyTheme { DefaultFontFamily = new FontFamily("Inter") });

		Assert.IsTrue(semantic.Contains("BodyMediumFontFamily"));
		AssertNoPrefixedKey(semantic);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_HotReloadRebuilds_Then_SemanticDictionariesSurviveOnce()
	{
		// The alias and shared-typography dictionaries are merged at construction, outside the
		// rebuild lifecycle; a rebuild (hot reload or palette change) must neither drop nor duplicate them.
		var theme = new OmarchyTheme();
		var before = CountSemanticDictionaries(theme);

		typeof(BaseTheme)
			.GetMethod("UpdateApplication", BindingFlags.Static | BindingFlags.NonPublic)!
			.Invoke(null, new object[] { new[] { typeof(OmarchyTheme) } });
		theme.Palette = OmarchyPalettes.Nord;

		Assert.AreEqual(before, CountSemanticDictionaries(theme), "rebuild should keep the same SemanticResources dictionaries");
		Assert.IsTrue(theme.TryGetValue("FilledButtonStyle", out var style), "FilledButtonStyle should still resolve after a rebuild");
		Assert.IsInstanceOfType<Style>(style);
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Light")]
	[DataRow("Default")]
	public void When_LightweightResourcesWalked_Then_EachControlIsMarked(string appearance)
	{
		var (semantic, _) = Collect(new OmarchyTheme(), appearance);
		string[] keys =
		{
			"FilledButtonBackground", "FilledButtonForeground", "FilledButtonBorderBrush",
			"FilledTonalButtonBackground", "OutlinedButtonForeground", "TextButtonForeground", "IconButtonForeground",
			"CheckBoxBackgroundChecked", "ComboBoxBackground", "ContentDialogBackground", "FlyoutPresenterBackground",
			"HyperlinkButtonForeground", "ListViewItemForeground", "MenuFlyoutItemForeground",
			"NavigationViewButtonForeground", "FilledPasswordBoxBackground", "ProgressBarForeground",
			"ProgressRingForeground", "RadioButtonForeground", "SliderForeground", "FilledTextBoxBackground",
			"TextBlockForeground", "TextToggleButtonBackground", "ToggleSwitchFillOn", "ToolTipBackground",
			// Theme-agnostic scalars declared outside the theme dictionaries
			"HyperlinkUnderlineVisible", "NavigationViewCompactPaneLength", "FlyoutPresenterBorderThickness",
		};

		foreach (var key in keys)
		{
			Assert.IsTrue(semantic.Contains(key), $"'{key}' should be marked semantic under {appearance}");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_LightweightBrushesResolve_Then_TheyAliasThePaletteBrushes(ElementTheme appearance)
	{
		var container = new StackPanel { RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new OmarchyTheme());
		(string Semantic, string Omarchy)[] pairs =
		{
			("FilledButtonBackground", "OmarchyNormalBlueBrush"),
			("FilledButtonForeground", "OmarchyBrightBlueBrush"),
			("OutlinedButtonBorderBrush", "OmarchyNormalWhiteBrush"),
			("IconButtonForeground", "OmarchyBrightWhiteBrush"),
		};

		// Resolve through live ThemeResource expressions rather than materializing lazy entries
		// in the theme dictionaries, which would pin their values to the current app theme.
		foreach (var (semantic, omarchy) in pairs)
		{
			container.Children.Add(CreateResourceProbe(semantic));
			container.Children.Add(CreateResourceProbe(omarchy));
		}

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(container);
		await UnitTestsUIContentHelper.WaitForIdle();

		for (var index = 0; index < pairs.Length; index++)
		{
			var actual = ((Border)container.Children[index * 2]).Background;
			var expected = ((Border)container.Children[index * 2 + 1]).Background;
			Assert.AreSame(expected, actual, $"{pairs[index].Semantic} should alias {pairs[index].Omarchy} under {appearance}");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_PaletteChanges_Then_LightweightBrushRepaints()
	{
		// The semantic file is loaded once, outside the rebuild lifecycle, and its aliases point at
		// the palette brushes OmarchyTheme rewrites in place: a palette change must still reach them.
		var theme = new OmarchyTheme();
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		var probe = CreateResourceProbe("FilledButtonBackground");
		container.Children.Add(probe);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(probe);
		await UnitTestsUIContentHelper.WaitForIdle();

		var brush = (SolidColorBrush)probe.Background;
		Assert.AreEqual(OmarchyPalettes.TokyoNight.Normal.Blue, brush.Color);

		theme.Palette = OmarchyPalettes.Nord;
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreSame(brush, probe.Background);
		Assert.AreEqual(OmarchyPalettes.Nord.Normal.Blue, brush.Color);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_LightweightResourcesOverridden_Then_StyledButtonUsesLocalBrushes()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new OmarchyTheme());
		var background = new SolidColorBrush(Colors.DarkOrchid);
		var foreground = new SolidColorBrush(Colors.Crimson);
		container.Resources["FilledButtonBackground"] = background;
		container.Resources["FilledButtonForeground"] = foreground;
		var button = new Button
		{
			Content = "Local override",
			Style = (Style)container.Resources["FilledButtonStyle"],
		};
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreSame(background, button.Background, "local FilledButtonBackground");
		Assert.AreSame(foreground, button.Foreground, "local FilledButtonForeground");
	}

	private static Border CreateResourceProbe(string background)
		=> (Border)XamlReader.Load($$"""
			<Border xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
			        Width="4"
			        Height="4"
			        Background="{ThemeResource {{background}}}" />
			""");

	private static void AssertNoPrefixedKey(HashSet<string> semantic)
	{
		var prefixed = semantic.Where(k => k.StartsWith("Omarchy", System.StringComparison.Ordinal)).ToList();
		Assert.AreEqual(0, prefixed.Count, $"Omarchy-prefixed keys in SemanticResources: {string.Join(", ", prefixed)}");
	}

	private static int CountSemanticDictionaries(ResourceDictionary theme)
		=> theme.MergedDictionaries.Count(d => d is SemanticResources);

	/// <summary>
	/// Splits the keys reachable from <paramref name="theme"/> by whether they are declared in a
	/// <see cref="SemanticResources"/> dictionary or one of its theme dictionaries. The semantic flag
	/// travels down <see cref="ResourceDictionary.ThemeDictionaries"/> (a Source-loaded file copies
	/// those in as plain dictionaries) but not down <see cref="ResourceDictionary.MergedDictionaries"/>.
	/// Only <see cref="ResourceDictionary.Keys"/> is read for entries: the entry indexer would materialize
	/// lazy theme-aware values shared with the Source singleton and break theme switching process-wide.
	/// </summary>
	private static (HashSet<string> Semantic, HashSet<string> Other) Collect(ResourceDictionary theme, string? appearance = null)
	{
		var semantic = new HashSet<string>();
		var other = new HashSet<string>();
		Visit(theme, isSemantic: false);
		return (semantic, other);

		void Visit(ResourceDictionary dictionary, bool isSemantic)
		{
			isSemantic |= dictionary is SemanticResources;
			var target = isSemantic ? semantic : other;

			foreach (var name in dictionary.Keys.OfType<string>())
			{
				target.Add(name);
			}

			foreach (var merged in dictionary.MergedDictionaries)
			{
				Visit(merged, isSemantic: false);
			}

			// Through the indexer, not enumeration: a Source-copied dictionary holds its theme
			// dictionaries as lazy initializers until first indexed, and enumeration returns those.
			foreach (var themedDictionary in dictionary.ThemeDictionaries.Keys
				.Where(key => appearance is null || Equals(key, appearance))
				.ToList()
				.Select(themeKey => dictionary.ThemeDictionaries[themeKey])
				.OfType<ResourceDictionary>())
			{
				Visit(themedDictionary, isSemantic);
			}
		}
	}
}
