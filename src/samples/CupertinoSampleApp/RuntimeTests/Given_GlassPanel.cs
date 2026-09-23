using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Uno.WinUI.Graphics2DSK;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies <see cref="GlassPanel"/>: which tier it picks, that the theme can force the Solid tier, that the
/// Liquid tier really blurs what is beneath it, and that an unloaded panel is collectable even though its
/// backplate subscribes to a static per-frame event.
/// </summary>
[TestClass]
public class Given_GlassPanel
{
	// Fields, not locals, so nothing on the test method's stack frame keeps the panel alive (AGENTS.md §2).
	private WeakReference? _panelRef;
	private WeakReference? _backplateRef;

	private static Grid CreateThemedContainer(CupertinoTheme? theme = null)
	{
		var container = new Grid { Width = 200, Height = 100 };
		container.Resources.MergedDictionaries.Add(theme ?? new CupertinoTheme());
		return container;
	}

	private static async Task<GlassPanel> LoadPanel(Grid container, GlassRenderingMode mode)
	{
		var panel = new GlassPanel { RenderingMode = mode, Width = 160, Height = 40 };
		container.Children.Add(panel);
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(panel);
		await UnitTestsUIContentHelper.WaitForIdle();
		return panel;
	}

	private static SKCanvasElement? FindBackplate(DependencyObject root)
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if ((child as SKCanvasElement ?? FindBackplate(child)) is { } found)
			{
				return found;
			}
		}

		return null;
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ModeAuto_Then_TierMatchesCapabilities()
	{
		try
		{
			var panel = await LoadPanel(CreateThemedContainer(), GlassRenderingMode.Auto);

			var liquid = SKCanvasElement.IsSupportedOnCurrentPlatform()
				&& CompositionCapabilities.GetForCurrentView().AreEffectsFast();
			Assert.AreEqual(liquid ? GlassRenderingMode.Liquid : GlassRenderingMode.Solid, panel.ActualRenderingMode);
			Assert.AreEqual(liquid, FindBackplate(panel) is not null, "the backplate exists only in the Liquid tier");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeModeSolid_Then_PanelAskingForLiquidStaysSolid()
	{
		try
		{
			var container = CreateThemedContainer(new CupertinoTheme { GlassRenderingMode = GlassRenderingMode.Solid });

			var panel = await LoadPanel(container, GlassRenderingMode.Liquid);

			Assert.AreEqual(GlassRenderingMode.Solid, panel.ActualRenderingMode);
			Assert.IsNull(FindBackplate(panel));
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ModeSwitchesFromLiquidToSolid_Then_BackplateRemoved()
	{
		try
		{
			var panel = await LoadPanel(CreateThemedContainer(), GlassRenderingMode.Liquid);
			Assert.AreEqual(GlassRenderingMode.Liquid, panel.ActualRenderingMode);
			Assert.IsNotNull(FindBackplate(panel));

			panel.RenderingMode = GlassRenderingMode.Solid;

			Assert.AreEqual(GlassRenderingMode.Solid, panel.ActualRenderingMode);
			Assert.IsNull(FindBackplate(panel));
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// The Liquid tier over a hard black / white edge: outside the panel the edge is one pixel wide, inside
	// it the backdrop blur spreads it out. The software renderer runs the whole filter chain, so this needs
	// no GPU; Auto would pick Solid there, which is why the mode is forced.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_Liquid_Then_BackdropIsBlurred()
	{
		try
		{
			var container = CreateThemedContainer();
			container.ColumnDefinitions.Add(new ColumnDefinition());
			container.ColumnDefinitions.Add(new ColumnDefinition());
			var black = new Border { Background = new SolidColorBrush(Colors.Black) };
			var white = new Border { Background = new SolidColorBrush(Colors.White) };
			Grid.SetColumn(white, 1);
			container.Children.Add(black);
			container.Children.Add(white);

			var panel = await LoadPanel(container, GlassRenderingMode.Liquid);
			Grid.SetColumnSpan(panel, 2);
			panel.VerticalAlignment = VerticalAlignment.Top;
			panel.Margin = new Thickness(0, 10, 0, 0);
			await UnitTestsUIContentHelper.WaitForIdle();

			var bitmap = new RenderTargetBitmap();
			await bitmap.RenderAsync(container);
			var pixels = (await bitmap.GetPixelsAsync()).ToArray();
			var scale = bitmap.PixelWidth / container.ActualWidth;

			// The panel covers y 10–50 and x 20–180; x 40–160 stays clear of its refracting edge band.
			var inside = SharpestStep(pixels, bitmap.PixelWidth, (int)(30 * scale), (int)(40 * scale), (int)(160 * scale));
			var outside = SharpestStep(pixels, bitmap.PixelWidth, (int)(80 * scale), (int)(40 * scale), (int)(160 * scale));

			// Not vacuous: the Solid tier would also have no step, but it shows no backdrop either.
			Assert.AreEqual(GlassRenderingMode.Liquid, panel.ActualRenderingMode);
			var row = (int)(30 * scale) * bitmap.PixelWidth;
			var across = Math.Abs(pixels[(row + (int)(160 * scale)) * 4] - pixels[(row + (int)(40 * scale)) * 4]);
			Assert.IsTrue(across > 100, $"the backdrop should show through the glass, left and right differ by {across}");

			Assert.IsTrue(outside > 200, $"the bare edge should be a hard step, was {outside}");
			Assert.IsTrue(inside < 64, $"the edge under the glass should be blurred, sharpest step was {inside}");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	// The Clear material is the lifted knob lens: it must cast a visible shadow below itself on both tiers —
	// drawn by the backplate in Liquid (ThemeShadow cannot see a backdrop-sampling canvas), by ThemeShadow on
	// the panel in Solid. Regular casts none, so the comparison is not vacuous.
	[TestMethod]
	[DataRow(GlassRenderingMode.Liquid)]
	[DataRow(GlassRenderingMode.Solid)]
	[RunsOnUIThread]
	public async Task When_ClearMaterial_Then_ShadowFallsBelow(GlassRenderingMode mode)
	{
		try
		{
			var container = CreateThemedContainer();
			container.Background = new SolidColorBrush(Colors.White);
			var panel = await LoadPanel(container, mode);
			Assert.AreEqual(mode, panel.ActualRenderingMode);

			// The panel is centered: x 20–180, y 30–70. Sample 2 px below its bottom edge, mid-width.
			var regular = await LumaBelowAsync(container, panel, GlassMaterial.Regular);
			var clear = await LumaBelowAsync(container, panel, GlassMaterial.Clear);

			Assert.AreEqual(255, regular, "Regular glass casts no shadow");
			Assert.IsTrue(clear < 245, $"Clear glass should darken the surface below it, luma was {clear}");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	private static async Task<int> LumaBelowAsync(Grid container, GlassPanel panel, GlassMaterial material)
	{
		panel.Material = material;
		await UnitTestsUIContentHelper.WaitForIdle();
		var bitmap = new RenderTargetBitmap();
		await bitmap.RenderAsync(container);
		var pixels = (await bitmap.GetPixelsAsync()).ToArray();
		var scale = bitmap.PixelWidth / container.ActualWidth;
		var i = (((int)(72 * scale) * bitmap.PixelWidth) + (int)(100 * scale)) * 4;
		return (pixels[i] + pixels[i + 1] + pixels[i + 2]) / 3;
	}

	// Largest luminance difference between two neighboring pixels of a BGRA row.
	private static int SharpestStep(byte[] bgra, int stride, int y, int fromX, int toX)
	{
		static int Luma(byte[] p, int i) => (p[i] + p[i + 1] + p[i + 2]) / 3;

		var sharpest = 0;
		for (var x = fromX; x < toX; x++)
		{
			var i = ((y * stride) + x) * 4;
			sharpest = Math.Max(sharpest, Math.Abs(Luma(bgra, i + 4) - Luma(bgra, i)));
		}

		return sharpest;
	}

	// The backplate subscribes to the static CompositionTarget.Rendering; if Unloaded did not unsubscribe,
	// that event would keep every glass panel ever shown alive.
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_LiquidPanelUnloaded_Then_Collected()
	{
		try
		{
			await LoadAndTrack();
			UnitTestsUIContentHelper.Content = null;
			await UnitTestsUIContentHelper.WaitForIdle();

			for (var i = 0; i < 10 && (_panelRef!.IsAlive || _backplateRef!.IsAlive); i++)
			{
				await Task.Delay(50);
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}

			Assert.IsFalse(_panelRef!.IsAlive, "the panel leaked");
			Assert.IsFalse(_backplateRef!.IsAlive, "the backplate leaked");
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task LoadAndTrack()
	{
		var panel = await LoadPanel(CreateThemedContainer(), GlassRenderingMode.Liquid);
		_panelRef = new WeakReference(panel);
		_backplateRef = new WeakReference(FindBackplate(panel) ?? throw new AssertFailedException("no backplate to track"));
	}
}
