#nullable enable

// Uno targets only: on WinAppSDK the Material style keeps WinUI's own ProgressRing animation and
// declares no Lottie sources, and CommunityToolkit.WinUI.Lottie is not referenced there.
#if HAS_UNO
using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Lottie;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards that the Material ProgressRing plays the theme's own Lottie animations, which ship as
/// embedded resources (<c>embedded://Uno.Material.WinUI/...</c>) and are loaded by
/// <see cref="LottieVisualSource"/> from Uno.UI. The libraries no longer declare the removed
/// <c>Lottie</c> Uno feature (unoplatform/uno#25117), and nothing at build time checks that the
/// resource resolves: a failure only shows up at run time, as a ring that never animates.
/// </summary>
[TestClass]
public class Given_ProgressRing
{
	private const string StyleKey = "MaterialProgressRingStyle";

	// Loading reads and parses the embedded JSON off the UI thread; generous for slow CI agents.
	private static readonly TimeSpan LoadTimeout = TimeSpan.FromSeconds(10);

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(true, "MaterialIndeterminate.json")]
	[DataRow(false, "MaterialDeterminate.json")]
	public async Task When_Active_Then_ThemeAnimationIsLoaded(bool isIndeterminate, string expectedAsset)
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new MaterialTheme());

		var style = container.Resources[StyleKey] as Style;
		Assert.IsNotNull(style, $"{StyleKey} should be resolvable from the theme.");

		var ring = new ProgressRing
		{
			Style = style,
			IsIndeterminate = isIndeterminate,
			Value = 50,
			IsActive = true,
			Width = 48,
			Height = 48,
		};
		container.Children.Add(ring);

		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(ring);
		await UnitTestsUIContentHelper.WaitForIdle();

		var player = ring.EnumerateDescendants().OfType<AnimatedVisualPlayer>().FirstOrDefault();
		Assert.IsNotNull(player, "The ProgressRing template should host an AnimatedVisualPlayer.");

		var source = player.Source as LottieVisualSource;
		Assert.IsNotNull(source, $"The player should use the theme's LottieVisualSource, not {player.Source?.GetType().Name ?? "null"}.");
		StringAssert.EndsWith(source.UriSource?.OriginalString, expectedAsset);

		await TestHelper.TryWaitFor(() => player.IsAnimatedVisualLoaded, LoadTimeout);
		Assert.IsTrue(player.IsAnimatedVisualLoaded,
			$"The theme animation {source.UriSource} should load (diagnostics: {player.Diagnostics ?? "none"}).");
	}
}
#endif
