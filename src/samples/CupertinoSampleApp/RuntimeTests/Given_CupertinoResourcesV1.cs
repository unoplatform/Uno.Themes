using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// <c>CupertinoResourcesV1</c> is the frozen pre-<see cref="CupertinoTheme"/> generation: it must load on its
/// own, from its own palette, and still honor a legacy <c>CupertinoColors.OverrideSource</c>.
/// </summary>
[TestClass]
public class Given_CupertinoResourcesV1
{
	private const string ColorsOverride = "ms-appx:///RuntimeTests/Assets/LegacyColorsOverride.xaml";

	// Styles/Application/v1/ColorPalette.xaml, frozen: Light / Default (dark).
	private static readonly Color FrozenLightBlue = Color.FromArgb(0xFF, 0x00, 0x7B, 0xFF);
	private static readonly Color FrozenDarkBlue = Color.FromArgb(0xFF, 0x0A, 0x84, 0xFF);

	// Styles/Application/ColorPalette.xaml, the current generation: Light / Default (dark).
	private static readonly Color CurrentLightBlue = Color.FromArgb(0xFF, 0x00, 0x88, 0xFF);
	private static readonly Color CurrentDarkBlue = Color.FromArgb(0xFF, 0x00, 0x91, 0xFF);

	private static readonly Color OverrideBlue = Color.FromArgb(0xFF, 0x12, 0x34, 0x56);

	// A container outside the visual tree resolves its theme dictionaries for the application theme.
	private static Color FrozenBlue =>
		Application.Current.RequestedTheme == ApplicationTheme.Dark ? FrozenDarkBlue : FrozenLightBlue;

#pragma warning disable CS0618 // The obsolete escape hatch is the subject under test.
	private static Grid CreateContainer()
	{
		var container = new Grid();
		container.Resources.MergedDictionaries.Add(new CupertinoResourcesV1());
		return container;
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_Loaded_Then_ButtonStyleResolves()
	{
		var container = CreateContainer();

		Assert.IsTrue(container.Resources.TryGetValue("CupertinoButtonStyle", out var value), "CupertinoButtonStyle missing");
		Assert.IsInstanceOfType(value, typeof(Style));
		Assert.AreEqual(typeof(Button), ((Style)value).TargetType);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_Loaded_Then_BlueBrushComesFromTheFrozenPalette()
	{
		var container = CreateContainer();

		Assert.IsTrue(container.Resources.TryGetValue("CupertinoBlueBrush", out var value), "CupertinoBlueBrush missing");
		Assert.IsInstanceOfType(value, typeof(SolidColorBrush));

		var color = ((SolidColorBrush)value).Color;
		Assert.AreEqual(FrozenBlue, color);
		Assert.AreNotEqual(CurrentLightBlue, color, "V1 must not read the current palette");
		Assert.AreNotEqual(CurrentDarkBlue, color, "V1 must not read the current palette");
	}

	// Asserted on the color CupertinoColorsV1 parses its brushes against, not on CupertinoBlueBrush through
	// CupertinoResourcesV1: Uno builds a Source-loaded dictionary graph once per process, so the nested
	// CupertinoColorsV1 (and the override it read) is shared by every CupertinoResourcesV1. Brush-level
	// override and no-override assertions therefore cannot coexist in one test run, in either order.
	[TestMethod]
	[RunsOnUIThread]
	public void When_ColorsOverrideSource_Then_OverrideWinsOverTheFrozenPalette()
	{
		var colors = new CupertinoColors { OverrideSource = ColorsOverride };
		CupertinoColorsV1 sut;
		try
		{
			sut = new CupertinoColorsV1();
		}
		finally
		{
			// The override URI is recorded process-wide; never let it leak into the next test.
			colors.OverrideSource = null;
		}

		Assert.IsTrue(sut.TryGetValue("CupertinoBlueColor", out var value), "CupertinoBlueColor missing");
		Assert.AreEqual(OverrideBlue, (Color)value);
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_NoColorsOverrideSource_Then_FrozenPaletteColorIsKept()
	{
		var sut = new CupertinoColorsV1();

		Assert.IsTrue(sut.TryGetValue("CupertinoBlueColor", out var value), "CupertinoBlueColor missing");
		Assert.AreEqual(FrozenBlue, (Color)value);
	}

	// Review finding (contract): V1 must paint its brushes from its OWN frozen palette. {StaticResource} in
	// an x:Class dictionary resolves against the application scope first, where the new palette lives.
	[TestMethod]
	[RunsOnUIThread]
	public void When_ColorsV1BuiltOnItsOwn_Then_BrushesUseTheFrozenPaletteNotTheAmbientOne()
	{
		var sut = new CupertinoColorsV1();

		Assert.IsTrue(sut.TryGetValue("CupertinoBlueColor", out var color) && color is Color);
		Assert.IsTrue(sut.TryGetValue("CupertinoBlueBrush", out var brush) && brush is SolidColorBrush);
		Assert.AreEqual((Color)color, ((SolidColorBrush)brush).Color);
	}

#pragma warning restore CS0618
}
