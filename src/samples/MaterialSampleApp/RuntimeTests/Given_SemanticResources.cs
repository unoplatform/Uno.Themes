#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies the <see cref="SemanticResources"/> marker: walking a <see cref="MaterialTheme"/>, every
/// semantic (theme-agnostic) key is declared in a <see cref="SemanticResources"/> dictionary or one
/// of its theme dictionaries, and no Material-prefixed key is. Tooling relies on the dictionary type
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
		var (semantic, other) = Collect(new MaterialTheme());

		foreach (var key in SemanticKeys)
		{
			Assert.IsTrue(semantic.Contains(key), $"'{key}' should be declared in a SemanticResources dictionary");
		}

		Assert.IsTrue(other.Contains("MaterialFilledButtonStyle"), "the theme-specific style should stay in a plain dictionary");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_AliasKeysAreNotInPlainDictionaries()
	{
		// Guards the regression this marker exists to prevent: an alias block re-added to the merged
		// bundle would leave the aliases present twice and every other assertion green.
		var (_, other) = Collect(new MaterialTheme());

		foreach (var key in AliasOnlyKeys)
		{
			Assert.IsFalse(other.Contains(key), $"'{key}' should only be declared in SemanticResources");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ThemeWalked_Then_NoThemePrefixedKeyIsInSemanticResources()
	{
		var (semantic, _) = Collect(new MaterialTheme());

		AssertNoPrefixedKey(semantic);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultFontFamilySet_Then_NoThemePrefixedKeyIsInSemanticResources()
	{
		// The generated typeface layer only exists when a family is set; the theme's per-control
		// *FontFamily aliases it regenerates are theme-prefixed and must stay out of the marker.
		var (semantic, _) = Collect(new MaterialTheme { DefaultFontFamily = new FontFamily("Inter") });

		Assert.IsTrue(semantic.Contains("BodyMediumFontFamily"));
		AssertNoPrefixedKey(semantic);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_AliasesMovedToSemanticResources_Then_TheyStillResolve()
	{
		var theme = new MaterialTheme();

		Assert.IsTrue(theme.TryGetValue("FilledButtonStyle", out var style), "FilledButtonStyle should resolve through the theme");
		Assert.IsInstanceOfType(style, typeof(Style));
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_HotReloadRebuilds_Then_SemanticDictionariesSurviveOnce()
	{
		// The alias and shared-typography dictionaries are merged at construction, outside the
		// rebuild lifecycle; a rebuild must neither drop nor duplicate them.
		var theme = new MaterialTheme();
		var before = CountSemanticDictionaries(theme);

		typeof(BaseTheme)
			.GetMethod("UpdateApplication", BindingFlags.Static | BindingFlags.NonPublic)!
			.Invoke(null, new object[] { new[] { typeof(MaterialTheme) } });

		Assert.AreEqual(before, CountSemanticDictionaries(theme), "rebuild should keep the same SemanticResources dictionaries");
		Assert.IsTrue(theme.TryGetValue("FilledButtonStyle", out _), "FilledButtonStyle should still resolve after a rebuild");
	}

	private static void AssertNoPrefixedKey(HashSet<string> semantic)
	{
		var prefixed = semantic.Where(k => k.StartsWith("Material", System.StringComparison.Ordinal)).ToList();
		Assert.AreEqual(0, prefixed.Count, $"Material-prefixed keys in SemanticResources: {string.Join(", ", prefixed)}");
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
	private static (HashSet<string> Semantic, HashSet<string> Other) Collect(ResourceDictionary theme)
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
				.ToList()
				.Select(themeKey => dictionary.ThemeDictionaries[themeKey])
				.OfType<ResourceDictionary>())
			{
				Visit(themedDictionary, isSemantic);
			}
		}
	}
}
