using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.Themes;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies <see cref="SemanticResourceKeys"/>: every list holds exactly the keys the shared layer
/// declares (in XAML) or generates (in <see cref="BaseTheme"/>), every listed key resolves, and no
/// list can be written through.
/// </summary>
/// <remarks>
/// The XAML dictionaries and the generated layers are the oracles, so these tests fail when a key is
/// added to either side without the other. See specs/11-public-semantic-keys/progress.md.
/// </remarks>
[TestClass]
public class Given_SemanticResourceKeys
{
	private const string CommonDictionariesPath = "ms-appx:///Uno.Themes.WinUI/Styles/Applications/Common/";

	private static IEnumerable<(string Name, IReadOnlyList<string> Keys)> AllLists()
	{
		yield return (nameof(SemanticResourceKeys.Colors), SemanticResourceKeys.Colors);
		yield return (nameof(SemanticResourceKeys.Opacities), SemanticResourceKeys.Opacities);
		yield return (nameof(SemanticResourceKeys.Brushes), SemanticResourceKeys.Brushes);
		yield return (nameof(SemanticResourceKeys.FontFamilies), SemanticResourceKeys.FontFamilies);
		yield return (nameof(SemanticResourceKeys.FontSizes), SemanticResourceKeys.FontSizes);
		yield return (nameof(SemanticResourceKeys.FontWeights), SemanticResourceKeys.FontWeights);
		yield return (nameof(SemanticResourceKeys.CharacterSpacings), SemanticResourceKeys.CharacterSpacings);
		yield return (nameof(SemanticResourceKeys.Spacing), SemanticResourceKeys.Spacing);
		yield return (nameof(SemanticResourceKeys.Shape), SemanticResourceKeys.Shape);
		yield return (nameof(SemanticResourceKeys.Density), SemanticResourceKeys.Density);
	}

	private static ResourceDictionary LoadCommonDictionary(string fileName)
		=> new() { Source = new Uri(CommonDictionariesPath + fileName) };

	private static IEnumerable<string> ThemedKeys(ResourceDictionary dictionary, string themeKey)
	{
		Assert.IsTrue(dictionary.ThemeDictionaries.TryGetValue(themeKey, out var themed) && themed is ResourceDictionary,
			$"Expected a '{themeKey}' theme dictionary.");

		return ((ResourceDictionary)themed!).Keys.OfType<string>();
	}

	private static void AssertSameKeys(IEnumerable<string> declared, IReadOnlyList<string> listed, string context)
	{
		var declaredSet = declared.ToHashSet(StringComparer.Ordinal);
		var missing = declaredSet.Except(listed, StringComparer.Ordinal).OrderBy(key => key).ToList();
		var extra = listed.Except(declaredSet, StringComparer.Ordinal).OrderBy(key => key).ToList();

		Assert.AreEqual(0, missing.Count + extra.Count,
			$"{context}: declared but not listed [{string.Join(", ", missing)}]; " +
			$"listed but not declared [{string.Join(", ", extra)}].");
		Assert.AreEqual(listed.Count, listed.Distinct(StringComparer.Ordinal).Count(), $"{context}: the list holds duplicates.");
	}

	// ─────────────────────────────────────────────────────────────────────
	// XAML-declared families: each list matches its dictionary exactly,
	// under every theme dictionary the file declares.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Light")]
	[DataRow("Default")]
	public void When_ColorPaletteInspected_Then_ColorsMatchItsThemedKeys(string themeKey)
	{
		var palette = LoadCommonDictionary("SharedColorPalette.xaml");

		AssertSameKeys(ThemedKeys(palette, themeKey), SemanticResourceKeys.Colors, $"SharedColorPalette.xaml [{themeKey}]");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ColorPaletteInspected_Then_OpacitiesMatchItsThemeIndependentKeys()
	{
		var palette = LoadCommonDictionary("SharedColorPalette.xaml");

		AssertSameKeys(palette.Keys.OfType<string>(), SemanticResourceKeys.Opacities, "SharedColorPalette.xaml [top level]");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Light")]
	[DataRow("Default")]
	[DataRow("HighContrast")]
	public void When_BrushDictionaryInspected_Then_BrushesMatchItsThemedKeys(string themeKey)
	{
		var brushes = LoadCommonDictionary("SharedColors.xaml");

		AssertSameKeys(ThemedKeys(brushes, themeKey), SemanticResourceKeys.Brushes, $"SharedColors.xaml [{themeKey}]");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("FontFamily", "Light")]
	[DataRow("FontFamily", "Default")]
	[DataRow("FontSize", "Light")]
	[DataRow("FontSize", "Default")]
	[DataRow("FontWeight", "Light")]
	[DataRow("FontWeight", "Default")]
	[DataRow("CharacterSpacing", "Light")]
	[DataRow("CharacterSpacing", "Default")]
	public void When_TypographyInspected_Then_EachFacetMatchesItsThemedKeys(string facet, string themeKey)
	{
		var listed = facet switch
		{
			"FontFamily" => SemanticResourceKeys.FontFamilies,
			"FontSize" => SemanticResourceKeys.FontSizes,
			"FontWeight" => SemanticResourceKeys.FontWeights,
			_ => SemanticResourceKeys.CharacterSpacings,
		};
		var declared = ThemedKeys(LoadCommonDictionary("SharedTypography.xaml"), themeKey)
			.Where(key => key.EndsWith(facet, StringComparison.Ordinal));

		AssertSameKeys(declared, listed, $"SharedTypography.xaml {facet} [{themeKey}]");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Light")]
	[DataRow("Default")]
	public void When_TypographyInspected_Then_EveryKeyBelongsToAFacet(string themeKey)
	{
		var typographyLists = SemanticResourceKeys.FontFamilies
			.Concat(SemanticResourceKeys.FontSizes)
			.Concat(SemanticResourceKeys.FontWeights)
			.Concat(SemanticResourceKeys.CharacterSpacings)
			.ToList();

		AssertSameKeys(ThemedKeys(LoadCommonDictionary("SharedTypography.xaml"), themeKey), typographyLists,
			$"SharedTypography.xaml [{themeKey}]");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Generated families: each list matches the dictionary BaseTheme
	// generated for it, under Light and Default.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(nameof(SemanticResourceKeys.Spacing), "Space100", "Light")]
	[DataRow(nameof(SemanticResourceKeys.Spacing), "Space100", "Default")]
	[DataRow(nameof(SemanticResourceKeys.Shape), "Radius100", "Light")]
	[DataRow(nameof(SemanticResourceKeys.Shape), "Radius100", "Default")]
	[DataRow(nameof(SemanticResourceKeys.Density), "ControlHeightMedium", "Light")]
	[DataRow(nameof(SemanticResourceKeys.Density), "ControlHeightMedium", "Default")]
	public void When_GeneratedScaleInspected_Then_ListMatchesItsKeys(string listName, string probeKey, string themeKey)
	{
		var theme = new SimpleTheme();
		var listed = AllLists().Single(list => list.Name == listName).Keys;

		var generated = theme.MergedDictionaries
			.Select(dictionary => dictionary.ThemeDictionaries.TryGetValue(themeKey, out var themed) ? themed as ResourceDictionary : null)
			.SingleOrDefault(themed => themed is not null && themed.ContainsKey(probeKey));

		Assert.IsNotNull(generated, $"Expected one generated '{themeKey}' dictionary carrying '{probeKey}'.");
		AssertSameKeys(generated.Keys.OfType<string>(), listed, $"Generated {listName} [{themeKey}]");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Every listed key resolves through a themed container.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeMerged_Then_EveryListedKeyResolves()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new SimpleTheme());

		var unresolved = AllLists()
			.SelectMany(list => list.Keys.Select(key => (list.Name, Key: key)))
			.Where(entry => !container.Resources.TryGetValue(entry.Key, out var value) || value is null)
			.Select(entry => $"{entry.Name}.{entry.Key}")
			.ToList();

		Assert.AreEqual(0, unresolved.Count, $"Unresolved keys: {string.Join(", ", unresolved)}");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(nameof(SemanticResourceKeys.Colors), typeof(Color))]
	[DataRow(nameof(SemanticResourceKeys.Opacities), typeof(double))]
	[DataRow(nameof(SemanticResourceKeys.Brushes), typeof(SolidColorBrush))]
	[DataRow(nameof(SemanticResourceKeys.FontFamilies), typeof(FontFamily))]
	[DataRow(nameof(SemanticResourceKeys.FontSizes), typeof(double))]
	[DataRow(nameof(SemanticResourceKeys.FontWeights), typeof(string))]
	[DataRow(nameof(SemanticResourceKeys.CharacterSpacings), typeof(int))]
	[DataRow(nameof(SemanticResourceKeys.Density), typeof(double))]
	public void When_ThemeMerged_Then_EachKeyResolvesToItsDocumentedType(string listName, Type expectedType)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new SimpleTheme());
		var listed = AllLists().Single(list => list.Name == listName).Keys;

		var mistyped = listed
			.Where(key => !container.Resources.TryGetValue(key, out var value) || !expectedType.IsInstanceOfType(value))
			.ToList();

		Assert.AreEqual(0, mistyped.Count, $"{listName} keys not resolving to {expectedType.Name}: {string.Join(", ", mistyped)}");
	}

	// ─────────────────────────────────────────────────────────────────────
	// Edge case: a consumer cannot write through a list.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	public void When_ListCastToMutableList_Then_WritesThrow()
	{
		foreach (var (name, keys) in AllLists())
		{
			Assert.IsInstanceOfType<IList<string>>(keys, $"{name} is expected to expose IList<string>.");
			var mutable = (IList<string>)keys;

			Assert.ThrowsExactly<NotSupportedException>(() => mutable[0] = "Tampered", $"{name} indexer write");
			Assert.ThrowsExactly<NotSupportedException>(() => mutable.Add("Tampered"), $"{name}.Add");
			Assert.ThrowsExactly<NotSupportedException>(() => mutable.Clear(), $"{name}.Clear");
		}
	}
}
