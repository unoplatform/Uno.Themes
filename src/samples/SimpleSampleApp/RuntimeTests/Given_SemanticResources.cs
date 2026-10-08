#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the <see cref="SemanticResources"/> marker: walking a <see cref="SimpleTheme"/>, every
/// semantic (theme-agnostic) key is declared in a <see cref="SemanticResources"/> dictionary or one
/// of its theme dictionaries, and no Simple-prefixed key is. Tooling relies on the dictionary type
/// alone to tell the two apart.
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

	/// <summary>Keys whose only declaration is the alias file, so they must not exist in a plain dictionary either.</summary>
	private static readonly string[] AliasOnlyKeys = { "FilledButtonStyle", "BodyMedium" };

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_SemanticKeysAreInSemanticResources()
	{
		var (semantic, other) = Collect(new SimpleTheme());

		foreach (var key in SemanticKeys)
		{
			Assert.IsTrue(semantic.Contains(key), $"'{key}' should be declared in a SemanticResources dictionary");
		}

		Assert.IsTrue(other.Contains("SimpleFilledButtonStyle"), "the theme-specific style should stay in a plain dictionary");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_AliasKeysAreNotInPlainDictionaries()
	{
		// Guards the regression this marker exists to prevent: an alias block re-added to the merged
		// bundle would leave the aliases present twice and every other assertion green.
		var (_, other) = Collect(new SimpleTheme());

		foreach (var key in AliasOnlyKeys)
		{
			Assert.IsFalse(other.Contains(key), $"'{key}' should only be declared in SemanticResources");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_NoThemePrefixedKeyIsInSemanticResources()
	{
		var (semantic, _) = Collect(new SimpleTheme());

		AssertNoPrefixedKey(semantic);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultFontFamilySet_Then_NoThemePrefixedKeyIsInSemanticResources()
	{
		// The generated typeface layer only exists when a family is set; the theme's per-control
		// *FontFamily aliases it regenerates are theme-prefixed and must stay out of the marker.
		var (semantic, _) = Collect(new SimpleTheme { DefaultFontFamily = new FontFamily("Inter") });

		Assert.IsTrue(semantic.Contains("BodyMediumFontFamily"));
		AssertNoPrefixedKey(semantic);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_AliasesMovedToSemanticResources_Then_TheyStillResolve()
	{
		var theme = new SimpleTheme();

		Assert.IsTrue(theme.TryGetValue("FilledButtonStyle", out var style), "FilledButtonStyle should resolve through the theme");
		Assert.IsInstanceOfType(style, typeof(Style));
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_HotReloadRebuilds_Then_SemanticDictionariesSurviveOnce()
	{
		// The alias and shared-typography dictionaries are merged at construction, outside the
		// rebuild lifecycle; a rebuild must neither drop nor duplicate them.
		var theme = new SimpleTheme();
		var before = CountSemanticDictionaries(theme);

		typeof(BaseTheme)
			.GetMethod("UpdateApplication", BindingFlags.Static | BindingFlags.NonPublic)!
			.Invoke(null, new object[] { new[] { typeof(SimpleTheme) } });

		Assert.AreEqual(before, CountSemanticDictionaries(theme), "rebuild should keep the same SemanticResources dictionaries");
		Assert.IsTrue(theme.TryGetValue("FilledButtonStyle", out _), "FilledButtonStyle should still resolve after a rebuild");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Light")]
	[DataRow("Default")]
	public void When_LightweightResourcesWalked_Then_EachControlIsMarked(string appearance)
	{
		// These are public, design-system-agnostic customization keys, just like the style aliases.
		var (semantic, _) = Collect(new SimpleTheme(), appearance);
		string[] keys =
		{
			"FilledButtonBackground", "FilledButtonForeground", "FilledButtonBorderBrush",
			"FilledTonalButtonBackground", "OutlinedButtonForeground", "TextButtonForeground",
			"CalendarDatePickerBackground", "CheckBoxBackgroundChecked", "ComboBoxBackground",
			"DatePickerButtonBackground", "HyperlinkButtonForeground", "NavigationViewButtonForeground",
			"FilledPasswordBoxBackground", "PipsPagerNavigationButtonBackground", "ProgressBarForeground",
			"ProgressRingForeground", "RadioButtonForeground", "RatingControlCaptionForeground",
			"SliderThumbBackground", "FilledTextBoxBackground", "TextToggleButtonBackground",
			"ToggleSwitchKnobBoundsFill", "SliderThumbWidth",
		};

		foreach (var key in keys)
		{
			Assert.IsTrue(semantic.Contains(key), $"'{key}' should be marked semantic under {appearance}");
		}

		foreach (var property in new[] { "Background", "Foreground", "BorderBrush" })
		{
			foreach (var key in new[] { "", "PointerOver", "Pressed", "Disabled" }.Select(state => $"FilledButton{property}{state}"))
			{
				Assert.IsTrue(semantic.Contains(key), $"'{key}' should be marked semantic under {appearance}");
			}
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_LightweightBrushesResolve_Then_ButtonStateValuesArePreserved(ElementTheme appearance)
	{
		var container = new StackPanel { RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		(string State, string Background, string Foreground)[] states =
		{
			("", "PrimaryBrush", "OnPrimaryBrush"),
			("PointerOver", "PrimaryVariantDarkBrush", "OnPrimaryBrush"),
			("Pressed", "PrimaryVariantDarkBrush", "OnPrimaryBrush"),
			("Disabled", "OnSurfaceDisabledBrush", "OnSurfaceDisabledBrush"),
		};

		// Resolve through live ThemeResource expressions rather than materializing lazy entries
		// in the theme dictionaries, which would pin their values to the current app theme.
		foreach (var (state, background, foreground) in states)
		{
			var actual = CreateResourceProbe($"FilledButtonBackground{state}", $"FilledButtonForeground{state}");
			var expected = CreateResourceProbe(background, foreground);
			container.Children.Add(actual);
			container.Children.Add(expected);
		}

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(container);
		await UnitTestsUIContentHelper.WaitForIdle();

		for (var index = 0; index < states.Length; index++)
		{
			var actual = (Button)container.Children[index * 2];
			var expected = (Button)container.Children[index * 2 + 1];
			AssertBrushEqual(expected.Background, actual.Background, $"FilledButtonBackground{states[index].State} under {appearance}");
			AssertBrushEqual(expected.Foreground, actual.Foreground, $"FilledButtonForeground{states[index].State} under {appearance}");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_LightweightResourcesOverridden_Then_StyledButtonUsesLocalBrushes(ElementTheme appearance)
	{
		var container = new Grid { RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
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

		AssertBrushEqual(background, button.Background, $"local FilledButtonBackground under {appearance}");
		AssertBrushEqual(foreground, button.Foreground, $"local FilledButtonForeground under {appearance}");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_LightweightScalarResolves_Then_SliderThumbWidthIsPreserved(ElementTheme appearance)
	{
		var container = new Grid { RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		var probe = (Border)XamlReader.Load("""
			<Border xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
			        Height="20"
			        Width="{ThemeResource SliderThumbWidth}" />
			""");
		container.Children.Add(probe);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(probe);
		await UnitTestsUIContentHelper.WaitForIdle();

		Assert.AreEqual(16.0, probe.Width, 0.001, $"SliderThumbWidth under {appearance}");
	}

	private static Button CreateResourceProbe(string background, string foreground)
		=> (Button)XamlReader.Load($$"""
			<Button xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
			        Background="{ThemeResource {{background}}}"
			        Foreground="{ThemeResource {{foreground}}}" />
			""");

	private static void AssertBrushEqual(Brush expected, Brush actual, string message)
	{
		Assert.IsInstanceOfType<SolidColorBrush>(expected, message);
		Assert.IsInstanceOfType<SolidColorBrush>(actual, message);
		Assert.AreEqual(((SolidColorBrush)expected).Color, ((SolidColorBrush)actual).Color, message);
		Assert.AreEqual(expected.Opacity, actual.Opacity, 0.001, message);
	}

	private static void AssertNoPrefixedKey(HashSet<string> semantic)
	{
		var prefixed = semantic.Where(k => k.StartsWith("Simple", System.StringComparison.Ordinal)).ToList();
		Assert.AreEqual(0, prefixed.Count, $"Simple-prefixed keys in SemanticResources: {string.Join(", ", prefixed)}");
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
