using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Fluent;
using Uno.UI.RuntimeTests;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_FluentThemeLifecycle
{
	private WeakReference<FluentTheme>? _observedTheme;
	private UISettings? _publisher;
	private FluentTheme? _replacementTheme;

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeIsReplaced_SystemAccentSubscriptionDoesNotRetainIt()
	{
		CreateObservedTheme();
		// Uno caches the latest Source dictionary's merged children, whose parent points at
		// that latest theme. Replace it before checking the old theme's event subscription.
		// Keeping its UISettings publisher alive proves the observer itself is weak.
		_replacementTheme = new FluentTheme();
		await UnitTestsUIContentHelper.WaitForIdle();
		CollectObservedTheme();
		Assert.IsNotNull(_observedTheme);
		Assert.IsFalse(_observedTheme.TryGetTarget(out _), "The OS event publisher must not retain the theme dictionary.");
		GC.KeepAlive(_publisher);
		_publisher = null;
		_replacementTheme = null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateObservedTheme()
	{
		var theme = new FluentTheme();
		_observedTheme = new WeakReference<FluentTheme>(theme);
		var field = typeof(FluentTheme).GetField("_uiSettings", BindingFlags.NonPublic | BindingFlags.Instance);
		Assert.IsNotNull(field);
		_publisher = (UISettings?)field.GetValue(theme);
		Assert.IsNotNull(_publisher);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CollectObservedTheme()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_ConstructorOverridesSupplied_NativeButtonUsesTheSameValues(ElementTheme appearance)
	{
		var fill = new SolidColorBrush(Microsoft.UI.Colors.Crimson) { Opacity = 0.43 };
		var theme = new FluentTheme(colorOverride: new ResourceDictionary
		{
			["PrimaryColor"] = Microsoft.UI.Colors.Green,
			["FilledButtonBackground"] = fill,
		});
		var dictionaries = Application.Current.Resources.MergedDictionaries;
		dictionaries.Add(theme);
		try
		{
			var button = new Button { Content = "Constructor", Style = (Style)theme["FilledButtonStyle"] };
			var host = new Grid { RequestedTheme = appearance };
			host.Children.Add(button);
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			AssertBrush(fill.Color, fill.Opacity, button.Background);
			Assert.AreEqual(Microsoft.UI.Colors.Green, (Color)theme["PrimaryColor"]);
		}
		finally { dictionaries.Remove(theme); }
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_SeedChangesAndClears_TheSameNativeButtonUpdates(ElementTheme appearance)
	{
		var dictionaries = Application.Current.Resources.MergedDictionaries;
		var baselineButton = new Button { Content = "Baseline", Style = (Style)Application.Current.Resources["AccentButtonStyle"] };
		var host = new Grid { RequestedTheme = appearance };
		host.Children.Add(baselineButton);
		UnitTestsUIContentHelper.Content = host;
		await UnitTestsUIContentHelper.WaitForLoaded(baselineButton);
		await UnitTestsUIContentHelper.WaitForIdle();
		var baseline = (SolidColorBrush)baselineButton.Background;
		var baselineColor = baseline.Color;
		var baselineOpacity = baseline.Opacity;
		var theme = new FluentTheme { Colors = new ThemeColors { PrimarySeed = Microsoft.UI.Colors.Red } };
		dictionaries.Add(theme);
		try
		{
			var button = new Button { Content = "Live", Style = (Style)theme["FilledButtonStyle"] };
			host.Children.Add(button);
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			var initial = ((SolidColorBrush)button.Background).Color;
			theme.Colors.PrimarySeed = Microsoft.UI.Colors.Blue;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreNotEqual(initial, ((SolidColorBrush)button.Background).Color, "The existing button must follow a new seed without a theme toggle or navigation.");
			theme.Colors.PrimarySeed = Color.FromArgb(255, 70, 110, 120);
			await UnitTestsUIContentHelper.WaitForIdle();
			var fidelityFill = ((SolidColorBrush)button.Background).Color;
			theme.Colors.SeedColorMode = SeedColorMode.TonalSpot;
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreNotEqual(fidelityFill, ((SolidColorBrush)button.Background).Color, "The existing control must also follow generation-mode changes for a muted seed.");
			theme.Colors.PrimarySeed = null;
			await UnitTestsUIContentHelper.WaitForIdle();
			AssertBrush(baselineColor, baselineOpacity, button.Background);
		}
		finally { dictionaries.Remove(theme); }
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_LightweightOverrideChanges_TheSameNativeButtonUpdates()
	{
		var theme = new FluentTheme { Colors = new ThemeColors { OverrideDictionary = new ResourceDictionary { ["FilledButtonBackground"] = new SolidColorBrush(Microsoft.UI.Colors.Red) } } };
		var dictionaries = Application.Current.Resources.MergedDictionaries;
		dictionaries.Add(theme);
		try
		{
			var button = new Button { Content = "Live override", Style = (Style)theme["FilledButtonStyle"] };
			UnitTestsUIContentHelper.Content = button;
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			AssertBrush(Microsoft.UI.Colors.Red, 1, button.Background);
			theme.Colors.OverrideDictionary = new ResourceDictionary { ["FilledButtonBackground"] = new SolidColorBrush(Microsoft.UI.Colors.Blue) { Opacity = 0.4 } };
			await UnitTestsUIContentHelper.WaitForIdle();
			AssertBrush(Microsoft.UI.Colors.Blue, 0.4, button.Background);
		}
		finally { dictionaries.Remove(theme); }
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_RootFontOverrideChanges_SlotsAndNativeFontFollowAndClear()
	{
		var theme = new FluentTheme();
		var dictionaries = Application.Current.Resources.MergedDictionaries;
		dictionaries.Add(theme);
		try
		{
			var baseline = ((FontFamily)theme["BodyMediumFontFamily"]).Source;
			theme.FontOverrideDictionary = new ResourceDictionary { ["DefaultFontFamily"] = new FontFamily("Semantic root test") };
			foreach (var key in new[] { "DisplayLargeFontFamily", "BodyMediumFontFamily", "CaptionSmallFontFamily", "ContentControlThemeFontFamily" })
			{
				Assert.AreEqual("Semantic root test", ((FontFamily)theme[key]).Source, key);
			}
			theme.FontOverrideDictionary = new ResourceDictionary
			{
				["DefaultFontFamily"] = new FontFamily("Semantic root test"),
				["BodyMediumFontFamily"] = new FontFamily("Explicit slot"),
			};
			Assert.AreEqual("Explicit slot", ((FontFamily)theme["BodyMediumFontFamily"]).Source);
			theme.FontOverrideDictionary = null;
			Assert.AreEqual(baseline, ((FontFamily)theme["BodyMediumFontFamily"]).Source);
		}
		finally { dictionaries.Remove(theme); }
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PlatformAccentChanges_UnrelatedRebuildRefreshesSemanticPalette()
	{
		var resources = Application.Current.Resources;
		var theme = new FluentTheme();
		var platform = new ResourceDictionary { ["SystemAccentColor"] = Microsoft.UI.Colors.Orange, ["SystemAccentColorLight2"] = Microsoft.UI.Colors.Orange };
		resources.MergedDictionaries.Add(platform);
		try
		{
			theme.DefaultSpacing = 6;
			Assert.AreEqual(Microsoft.UI.Colors.Orange, (Color)theme["PrimaryColor"]);
			AssertBrush(Microsoft.UI.Colors.Orange, 1, (Brush)theme["PrimaryBrush"]);
		}
		finally { resources.MergedDictionaries.Remove(platform); }
	}

#if HAS_UNO
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_PlatformAccentEventRaised_SemanticPaletteRefreshes()
	{
		var theme = new FluentTheme();
		var resources = Application.Current.Resources;
		var platform = new ResourceDictionary { ["SystemAccentColor"] = Microsoft.UI.Colors.Orange, ["SystemAccentColorLight2"] = Microsoft.UI.Colors.Orange };
		resources.MergedDictionaries.Add(platform);
		try
		{
			var notify = typeof(UISettings).GetMethod("OnColorValuesChanged", BindingFlags.NonPublic | BindingFlags.Static);
			Assert.IsNotNull(notify, "The Uno platform event seam must be available.");
			notify.Invoke(null, null);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Microsoft.UI.Colors.Orange, (Color)theme["PrimaryColor"]);
		}
		finally { resources.MergedDictionaries.Remove(platform); }
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_PreviouslyValidOverrideSourceFails_RebuildRetainsAssignedValues()
	{
		const string source = "ms-appx:///RuntimeTests/FluentLifecycleOverride.xaml";
		var fail = false;
		global::Uno.UI.ResourceResolver.RegisterResourceDictionaryBySource(source, null, () => fail
			? throw new InvalidOperationException("Simulated hot-reload source failure")
			: new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Crimson });
		try
		{
			var theme = new FluentTheme { Colors = new ThemeColors { OverrideSource = source } };
			fail = true;
			theme.DefaultSpacing = 6;
			Assert.AreEqual(Microsoft.UI.Colors.Crimson, (Color)theme["PrimaryColor"]);
			AssertBrush(Microsoft.UI.Colors.Crimson, 1, (Brush)theme["AccentFillColorDefaultBrush"]);
		}
		finally
		{
			global::Uno.UI.ResourceResolver.RegisterResourceDictionaryBySource(source, null, () => new ResourceDictionary());
		}
	}
#endif

	private static void AssertBrush(Color color, double opacity, Brush brush)
	{
		Assert.IsInstanceOfType<SolidColorBrush>(brush);
		var solid = (SolidColorBrush)brush;
		Assert.AreEqual(color, solid.Color);
		Assert.AreEqual(opacity, solid.Opacity, 0.001);
	}
}
