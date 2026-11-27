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

	[TestMethod]
	public async Task When_ConstructedWithoutDispatcher_SystemAccentIsNotObserved()
	{
		// Off the UI thread there is no dispatcher to marshal a refresh to, and rebuilding the theme on
		// the event's thread would fail (WinUI) or race UI-thread reads (Uno): the theme must not subscribe.
		var publisher = await Task.Run(() =>
		{
			Assert.IsNull(Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread(), "the test needs a thread without a dispatcher");
			var theme = new FluentTheme();
			var field = Assert.IsInstanceOfType<FieldInfo>(typeof(FluentTheme).GetField("_uiSettings", BindingFlags.NonPublic | BindingFlags.Instance));
			return field.GetValue(theme);
		});

		Assert.IsNull(publisher, "a theme built without a dispatcher must not observe the system accent");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateObservedTheme()
	{
		var theme = new FluentTheme();
		_observedTheme = new WeakReference<FluentTheme>(theme);
		var field = Assert.IsInstanceOfType<FieldInfo>(typeof(FluentTheme).GetField("_uiSettings", BindingFlags.NonPublic | BindingFlags.Instance));
		_publisher = (UISettings?)field.GetValue(theme);
		Assert.IsNotNull(_publisher);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CollectObservedTheme()
	{
		// Intentional leak guard: force finalization in a separate frame from the
		// strong references created by CreateObservedTheme.
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

#if HAS_UNO // WinUI binds the built-in controls' accent aliases inside XamlControlsResources, so FluentTheme cannot recolor them there (spec 05 §9.2).
	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_ConstructorOverridesSupplied_NativeButtonUsesTheSameValues(ElementTheme appearance)
	{
		var theme = new FluentTheme(colorOverride: new ResourceDictionary
		{
			["PrimaryColor"] = Microsoft.UI.Colors.Green,
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
			// A PrimaryColor override is the accent fill, verbatim, in both appearances.
			AssertBrush(Microsoft.UI.Colors.Green, 1, button.Background);
			Assert.AreEqual(Microsoft.UI.Colors.Green, (Color)theme["PrimaryColor"]);
		}
		finally { dictionaries.Remove(theme); }
	}
#endif

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ConstructorOverrideAndSeed_SeedWinsAsInTheOtherThemes()
	{
		// As in SimpleTheme / MaterialTheme, the constructor override is the base palette, below a seed.
		var theme = new FluentTheme(colorOverride: new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Green });
		theme.Colors ??= new ThemeColors();
		theme.Colors.PrimarySeed = Microsoft.UI.Colors.Red;

		var fill = await GetAccentButtonFill(theme);

		Assert.AreNotEqual(Microsoft.UI.Colors.Green, (Color)theme["PrimaryColor"], "the seed must regenerate PrimaryColor");
		Assert.AreNotEqual(Microsoft.UI.Colors.Green, fill, "the built-in accent must follow the seed");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_ConstructorOverrideDeclaresAccentKeysAndSeed_ExplicitKeysWin()
	{
		// A seed outranks the constructor override's semantic roles, but not the platform accent keys it
		// declares by name: like a Colors override, those are explicit consumer values.
		var explicitBrush = new SolidColorBrush(Microsoft.UI.Colors.Orange);
		var theme = new FluentTheme(colorOverride: new ResourceDictionary
		{
			["SystemAccentColor"] = Microsoft.UI.Colors.Green,
			["AccentFillColorDefaultBrush"] = explicitBrush,
		});
		theme.Colors ??= new ThemeColors();
		theme.Colors.PrimarySeed = Microsoft.UI.Colors.Red;

		var host = new Grid { RequestedTheme = ElementTheme.Light };
		host.Resources.MergedDictionaries.Add(theme);

		Assert.IsTrue(host.Resources.TryGetValue("SystemAccentColor", out var accent));
		Assert.AreEqual(Microsoft.UI.Colors.Green, (Color)accent, "the seed must not replace an explicit SystemAccentColor");
		Assert.IsTrue(host.Resources.TryGetValue("AccentFillColorDefaultBrush", out var fill));
		Assert.AreSame(explicitBrush, fill, "the seed must not replace an explicit AccentFillColorDefaultBrush");
		Assert.AreNotEqual(Microsoft.UI.Colors.Green, (Color)theme["PrimaryColor"], "the seed still drives the semantic palette");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(true)]
	[DataRow(false)]
	public async Task When_AppDeclaresSystemAccentShades_PaletteFollowsThem(bool declaredBeforeTheme)
	{
		// The standard WinUI accent customization: SystemAccentColor* declared in the app resources. In
		// App.xaml they are parsed after the merged FluentTheme is constructed (declaredBeforeTheme: false).
		var shades = new (string Key, Color Value)[]
		{
			("SystemAccentColor", Color.FromArgb(0xFF, 0xE0, 0x60, 0x10)),
			("SystemAccentColorLight1", Color.FromArgb(0xFF, 0xF0, 0x80, 0x30)),
			("SystemAccentColorLight2", Color.FromArgb(0xFF, 0xF8, 0xA0, 0x50)),
			("SystemAccentColorLight3", Color.FromArgb(0xFF, 0xFF, 0xC0, 0x80)),
			("SystemAccentColorDark1", Color.FromArgb(0xFF, 0xB0, 0x40, 0x08)),
			("SystemAccentColorDark2", Color.FromArgb(0xFF, 0x80, 0x30, 0x04)),
			("SystemAccentColorDark3", Color.FromArgb(0xFF, 0x50, 0x20, 0x02)),
		};
		var app = Application.Current.Resources;
		var theme = declaredBeforeTheme ? null : new FluentTheme();
		foreach (var (key, value) in shades)
		{
			app[key] = value;
		}
		try
		{
			theme ??= new FluentTheme();
			var fill = await GetAccentButtonFill(theme);

			// The indexer resolves against the ambient app theme: Primary is the accent in Light, Light2 in Dark.
			var expectedPrimary = Application.Current.RequestedTheme == ApplicationTheme.Dark ? shades[2].Value : shades[0].Value;
			Assert.AreEqual(expectedPrimary, (Color)theme["PrimaryColor"], "PrimaryColor must follow the app's SystemAccentColor shades, like the built-in controls");
#if HAS_UNO // On WinUI the built-in accent aliases are bound inside XamlControlsResources at load (spec 05 §9.2).
			Assert.AreEqual(shades[4].Value, fill, "the built-in accent fill follows the app's SystemAccentColorDark1");
#endif
		}
		finally
		{
			foreach (var (key, _) in shades)
			{
				app.Remove(key);
			}
		}
	}

#if HAS_UNO // WinUI binds the built-in controls' accent aliases inside XamlControlsResources, so FluentTheme cannot recolor them there (spec 05 §9.2).
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ColorsAssignedWithoutSeed_ConstructorOverrideStillApplies()
	{
		var theme = new FluentTheme(colorOverride: new ResourceDictionary { ["PrimaryColor"] = Microsoft.UI.Colors.Green });
		theme.Colors = new ThemeColors();

		var fill = await GetAccentButtonFill(theme);

		Assert.AreEqual(Microsoft.UI.Colors.Green, (Color)theme["PrimaryColor"], "assigning Colors must not drop the constructor override");
		Assert.AreEqual(Microsoft.UI.Colors.Green, fill);
	}
#endif

	private static async Task<Color> GetAccentButtonFill(FluentTheme theme)
	{
		var host = new Grid { RequestedTheme = ElementTheme.Light };
		host.Resources.MergedDictionaries.Add(theme);
		var button = new Button { Content = "Accent", Style = (Style)host.Resources["FilledButtonStyle"] };
		host.Children.Add(button);
		UnitTestsUIContentHelper.Content = host;
		await UnitTestsUIContentHelper.WaitForLoaded(button);
		await UnitTestsUIContentHelper.WaitForIdle();
		return Assert.IsInstanceOfType<SolidColorBrush>(button.Background).Color;
	}

#if HAS_UNO // WinUI binds the built-in controls' accent aliases inside XamlControlsResources, so FluentTheme cannot recolor them there (spec 05 §9.2).
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
#endif

#if HAS_UNO
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_PlatformAccentEventRaised_SemanticPaletteRefreshes()
	{
		var theme = new FluentTheme();
		// The platform accent is read from XamlControlsResources, which FluentTheme cannot shadow.
		var xcr = Application.Current.Resources.MergedDictionaries.OfType<XamlControlsResources>().Single();
		var platform = new ResourceDictionary { ["SystemAccentColor"] = Microsoft.UI.Colors.Orange, ["SystemAccentColorLight2"] = Microsoft.UI.Colors.Orange };
		xcr.MergedDictionaries.Add(platform);
		try
		{
			var notify = Assert.IsInstanceOfType<MethodInfo>(
				typeof(UISettings).GetMethod("OnColorValuesChanged", BindingFlags.NonPublic | BindingFlags.Static),
				"The Uno platform event seam must be available.");
			notify.Invoke(null, null);
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreEqual(Microsoft.UI.Colors.Orange, (Color)theme["PrimaryColor"]);
		}
		finally { xcr.MergedDictionaries.Remove(platform); }
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
		var solid = Assert.IsInstanceOfType<SolidColorBrush>(brush);
		Assert.AreEqual(color, solid.Color);
		Assert.AreEqual(opacity, solid.Opacity, 0.001);
	}
}
