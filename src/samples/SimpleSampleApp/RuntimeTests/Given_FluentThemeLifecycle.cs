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
	[DataRow("FilledButtonBackground", "AccentButtonBackground", "FilledButtonStyle", "AccentButtonStyle", false)]
	[DataRow("OutlinedButtonBackground", "ButtonBackground", "OutlinedButtonStyle", "DefaultButtonStyle", false)]
	[DataRow("FilledButtonBackground", "AccentButtonBackground", "FilledButtonStyle", "AccentButtonStyle", true)]
	public async Task When_LightweightOverrideClears_ExistingNativeButtonAndRetainedBrushReturnToBaseline(
		string semanticKey, string nativeKey, string semanticStyle, string nativeStyle, bool seeded)
	{
		var dictionaries = Application.Current.Resources.MergedDictionaries;
		var theme = new FluentTheme { Colors = new ThemeColors { PrimarySeed = seeded ? Microsoft.UI.Colors.Blue : null } };
		dictionaries.Add(theme);
		try
		{
			var host = new StackPanel();
			var baselineButton = new Button { Content = "Baseline", Style = (Style)Application.Current.Resources[nativeStyle] };
			host.Children.Add(baselineButton);
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(baselineButton);
			await UnitTestsUIContentHelper.WaitForIdle();
			var baseline = Assert.IsInstanceOfType<SolidColorBrush>(baselineButton.Background);
			var baselineColor = baseline.Color;
			var baselineOpacity = baseline.Opacity;
			var overrideBrush = new SolidColorBrush(Microsoft.UI.Colors.Red) { Opacity = 0.43 };
			theme.Colors.OverrideDictionary = new ResourceDictionary { [semanticKey] = overrideBrush };
			var button = new Button { Content = "Clear override", Style = (Style)theme[semanticStyle] };
			host.Children.Add(button);
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			var retainedBrush = Assert.IsInstanceOfType<SolidColorBrush>(theme[nativeKey]);
			AssertBrush(overrideBrush.Color, overrideBrush.Opacity, retainedBrush);
			AssertBrush(overrideBrush.Color, overrideBrush.Opacity, button.Background);

			theme.Colors.OverrideDictionary = null;
			await UnitTestsUIContentHelper.WaitForIdle();

			AssertBrush(baselineColor, baselineOpacity, button.Background);
			AssertBrush(baselineColor, baselineOpacity, retainedBrush);
			AssertBrush(Microsoft.UI.Colors.Red, 0.43, overrideBrush);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
			dictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ScopedLightweightOverrideClears_ExistingButtonAndRetainedBrushReturnToScopedSeed()
	{
		var platformColor = Assert.IsInstanceOfType<SolidColorBrush>(Application.Current.Resources["AccentFillColorDefaultBrush"]).Color;
		var theme = new FluentTheme { Colors = new ThemeColors { PrimarySeed = Microsoft.UI.Colors.Blue } };
		var baseline = Assert.IsInstanceOfType<SolidColorBrush>(theme["AccentFillColorDefaultBrush"]);
		if (baseline.Color == platformColor)
		{
			// The application may already use the candidate accent. Choose another
			// scoped seed so the fallback assertion stays meaningful.
			theme.Colors.PrimarySeed = Microsoft.UI.Colors.Orange;
			baseline = Assert.IsInstanceOfType<SolidColorBrush>(theme["AccentFillColorDefaultBrush"]);
		}
		var baselineColor = baseline.Color;
		var baselineOpacity = baseline.Opacity;
		Assert.AreNotEqual(platformColor, baselineColor, "The scoped seed must differ from the app accent so restoring the app fallback cannot pass.");
		theme.Colors.OverrideDictionary = new ResourceDictionary
		{
			["FilledButtonBackground"] = new SolidColorBrush(Microsoft.UI.Colors.Red) { Opacity = 0.43 },
		};
		var host = new Grid();
		host.Resources.MergedDictionaries.Add(theme);
		var button = new Button { Content = "Scoped clear", Style = (Style)theme["FilledButtonStyle"] };
		host.Children.Add(button);
		UnitTestsUIContentHelper.Content = host;
		try
		{
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			var retainedBrush = Assert.IsInstanceOfType<SolidColorBrush>(theme["AccentButtonBackground"]);
			AssertBrush(Microsoft.UI.Colors.Red, 0.43, button.Background);
			AssertBrush(Microsoft.UI.Colors.Red, 0.43, retainedBrush);

			theme.Colors.OverrideDictionary = null;
			await UnitTestsUIContentHelper.WaitForIdle();

			AssertBrush(baselineColor, baselineOpacity, retainedBrush);
			AssertBrush(baselineColor, baselineOpacity, button.Background);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
			host.Resources.MergedDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow("Background", "AccentFillColorDefaultBrush")]
	[DataRow("BackgroundPointerOver", "AccentFillColorSecondaryBrush")]
	[DataRow("BackgroundPressed", "AccentFillColorTertiaryBrush")]
	[DataRow("Foreground", "TextOnAccentFillColorPrimaryBrush")]
	[DataRow("ForegroundPointerOver", "TextOnAccentFillColorPrimaryBrush")]
	[DataRow("ForegroundPressed", "TextOnAccentFillColorSecondaryBrush")]
	public void When_ScopedButtonOverrideClears_RetainedStateBrushUsesScopedAccent(string suffix, string accentKey)
	{
		var theme = new FluentTheme { Colors = new ThemeColors { PrimarySeed = Microsoft.UI.Colors.Orange } };
		theme.Colors.OverrideDictionary = new ResourceDictionary
		{
			["FilledButton" + suffix] = new SolidColorBrush(Microsoft.UI.Colors.Magenta) { Opacity = 0.43 },
		};
		var retained = Assert.IsInstanceOfType<SolidColorBrush>(theme["AccentButton" + suffix]);
		AssertBrush(Microsoft.UI.Colors.Magenta, 0.43, retained);

		theme.Colors.OverrideDictionary = null;

		var expected = Assert.IsInstanceOfType<SolidColorBrush>(theme[accentKey]);
		AssertBrush(expected.Color, expected.Opacity, retained);
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ScopedSemanticMappingClears_ExplicitNativeOverrideRemains()
	{
		var native = new SolidColorBrush(Microsoft.UI.Colors.Magenta) { Opacity = 0.43 };
		var theme = new FluentTheme
		{
			Colors = new ThemeColors
			{
				PrimarySeed = Microsoft.UI.Colors.Orange,
				OverrideDictionary = new ResourceDictionary
				{
					["FilledButtonBackground"] = new SolidColorBrush(Microsoft.UI.Colors.Red),
					["AccentButtonBackground"] = native,
				},
			},
		};
		var host = new Grid();
		host.Resources.MergedDictionaries.Add(theme);
		var button = new Button { Content = "Native scoped override", Style = (Style)theme["FilledButtonStyle"] };
		host.Children.Add(button);
		UnitTestsUIContentHelper.Content = host;
		try
		{
			await UnitTestsUIContentHelper.WaitForLoaded(button);
			await UnitTestsUIContentHelper.WaitForIdle();
			var retained = Assert.IsInstanceOfType<SolidColorBrush>(theme["AccentButtonBackground"]);
			AssertBrush(native.Color, native.Opacity, button.Background);
			AssertBrush(native.Color, native.Opacity, retained);

			theme.Colors.OverrideDictionary = new ResourceDictionary { ["AccentButtonBackground"] = native };
			await UnitTestsUIContentHelper.WaitForIdle();

			AssertBrush(native.Color, native.Opacity, button.Background);
			AssertBrush(native.Color, native.Opacity, retained);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
			host.Resources.MergedDictionaries.Remove(theme);
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_NativeAndSemanticLightweightKeysAreOverridden_ExplicitNativeResourceWins()
	{
		var semantic = new SolidColorBrush(Microsoft.UI.Colors.Red);
		var native = new SolidColorBrush(Microsoft.UI.Colors.Blue) { Opacity = 0.43 };
		var theme = new FluentTheme
		{
			Colors = new ThemeColors
			{
				OverrideDictionary = new ResourceDictionary
				{
					["FilledButtonBackground"] = semantic,
					["AccentButtonBackground"] = native,
				},
			},
		};
		AssertBrush(native.Color, native.Opacity, (Brush)theme["AccentButtonBackground"]);
		AssertBrush(semantic.Color, semantic.Opacity, (Brush)theme["FilledButtonBackground"]);
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
			var notify = Assert.IsInstanceOfType<MethodInfo>(
				typeof(UISettings).GetMethod("OnColorValuesChanged", BindingFlags.NonPublic | BindingFlags.Static),
				"The Uno platform event seam must be available.");
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
		var solid = Assert.IsInstanceOfType<SolidColorBrush>(brush);
		Assert.AreEqual(color, solid.Color);
		Assert.AreEqual(opacity, solid.Opacity, 0.001);
	}
}
