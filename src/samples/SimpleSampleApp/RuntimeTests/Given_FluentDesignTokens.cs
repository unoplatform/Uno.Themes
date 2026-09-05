using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that the BaseTheme shape setting reaches the BUILT-IN Fluent
/// controls under FluentTheme (specs/05-fluent-theme, §8, goal G4): the XCR
/// templates read ControlCornerRadius / OverlayCornerRadius through
/// {ThemeResource}, so FluentTheme re-points those platform tokens when the
/// consumer sets DefaultCornerRadius — and leaves them untouched otherwise, so
/// stock rendering stays the platform's.
/// </summary>
[TestClass]
public class Given_FluentDesignTokens
{
	private static Grid CreateThemedContainer(FluentTheme theme)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(theme);
		return container;
	}

	private static CornerRadius GetCornerRadius(ResourceDictionary resources, string key)
	{
		Assert.IsTrue(resources.TryGetValue(key, out var value) && value is { }, $"{key} should resolve");
		Assert.IsInstanceOfType(value, typeof(CornerRadius), $"{key} should resolve to a CornerRadius (got {value.GetType().Name})");
		return (CornerRadius)value;
	}

	/// <summary>
	/// Whether any dictionary the theme merged (its dynamic layers) holds
	/// <paramref name="key"/> as an OWN entry. "Untouched" is asserted
	/// structurally: in this host <c>Application.Current.Resources.TryGetValue</c>
	/// does not resolve XCR's corner-radius tokens at all (the templates'
	/// <c>{ThemeResource}</c> lookups do), so a value comparison against the stock
	/// is not available, and <c>TryGetValue</c> would in any case fall back to the
	/// system resources. XAML-backed dictionaries are not enumerable on Uno and
	/// are skipped — the platform tokens are only ever written to code-built layers.
	/// </summary>
	private static bool ThemeDefines(ResourceDictionary dictionary, string key)
	{
		try
		{
			foreach (var pair in dictionary)
			{
				if (pair.Key is string entryKey && entryKey == key)
				{
					return true;
				}
			}
		}
		catch (NotSupportedException)
		{
			// XAML-backed: not enumerable, and never a holder of the platform tokens.
		}

		foreach (var merged in dictionary.MergedDictionaries)
		{
			if (ThemeDefines(merged, key))
			{
				return true;
			}
		}

		return false;
	}

	private static async Task<Button> LoadStockButtonAsync(Grid container)
	{
		// The CI host's implicit Button style is Simple's — apply the Fluent
		// default style explicitly; its CornerRadius setter reads
		// {ThemeResource ControlCornerRadius}.
		var button = new Button
		{
			Content = "stock",
			Style = (Style)Application.Current.Resources["DefaultButtonStyle"],
		};
		container.Children.Add(button);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();
		return button;
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultCornerRadiusSet_PlatformCornerRadiusTokensFollow()
	{
		var container = CreateThemedContainer(new FluentTheme { DefaultCornerRadius = 8 });

		Assert.AreEqual(new CornerRadius(8), GetCornerRadius(container.Resources, "ControlCornerRadius"),
			"ControlCornerRadius must follow DefaultCornerRadius (the Radius100 unit) under FluentTheme");
		Assert.AreEqual(new CornerRadius(16), GetCornerRadius(container.Resources, "OverlayCornerRadius"),
			"OverlayCornerRadius must follow twice DefaultCornerRadius (Radius200) under FluentTheme");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DefaultCornerRadiusUnset_PlatformCornerRadiusTokensAreStock()
	{
		var theme = new FluentTheme();
		var container = CreateThemedContainer(theme);

		Assert.IsFalse(ThemeDefines(theme, "ControlCornerRadius"),
			"without a DefaultCornerRadius, FluentTheme must not write ControlCornerRadius (stock rendering untouched)");
		Assert.IsFalse(ThemeDefines(theme, "OverlayCornerRadius"),
			"without a DefaultCornerRadius, FluentTheme must not write OverlayCornerRadius (stock rendering untouched)");

		// Rendered: the platform's ControlCornerRadius (Fluent: 4px) stands.
		var button = await LoadStockButtonAsync(container);
		Assert.AreEqual(new CornerRadius(4), button.CornerRadius,
			"a stock Fluent button under an unset FluentTheme must keep the platform corner radius (4)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_DefaultCornerRadiusChangedAfterConstruction_TokenFollowsAndClearRestoresStock()
	{
		var theme = new FluentTheme();
		var container = CreateThemedContainer(theme);
		Assert.IsFalse(ThemeDefines(theme, "ControlCornerRadius"), "sanity: nothing written while unset");

		theme.DefaultCornerRadius = 12;
		Assert.AreEqual(new CornerRadius(12), GetCornerRadius(container.Resources, "ControlCornerRadius"),
			"a DefaultCornerRadius assigned after construction must re-point ControlCornerRadius");

		theme.ClearValue(BaseTheme.DefaultCornerRadiusProperty);
		Assert.IsFalse(ThemeDefines(theme, "ControlCornerRadius"),
			"clearing DefaultCornerRadius must drop the re-pointed ControlCornerRadius (stock restored)");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_DefaultCornerRadiusSet_StockButtonFollows()
	{
		var container = CreateThemedContainer(new FluentTheme { DefaultCornerRadius = 8 });

		var button = await LoadStockButtonAsync(container);
		Assert.AreEqual(new CornerRadius(8), button.CornerRadius,
			"a stock Fluent button must render with the theme's DefaultCornerRadius (G4)");
	}
}
