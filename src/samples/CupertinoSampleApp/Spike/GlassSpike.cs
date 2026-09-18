// THROWAWAY — spec 10 Phase 0.5 Liquid Glass spike (specs/10-cupertino-liquid-glass/liquid-glass-rendering.md §7).
// Launch with --glass-spike (interactive) or --glass-spike=<dir> (scripted: writes captures + handshake files to <dir>).
// Not library code; delete once the findings are recorded.
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using SkiaSharp;
using Uno.WinUI.Graphics2DSK;

namespace Uno.Themes.Samples.Spike;

internal sealed class SpikeGlassBackplate : SKCanvasElement
{
	private const string NormalMapSksl = """
		uniform float2 size;
		uniform float radius;
		uniform float band;
		half4 main(float2 p) {
			float2 c = size * 0.5;
			float2 s = sign(p - c);
			float2 q = abs(p - c) - (c - radius);
			float d = length(max(q, 0.0)) + min(max(q.x, q.y), 0.0) - radius;
			float t = clamp(1.0 + d / band, 0.0, 1.0);
			float k = 1.0 - sqrt(1.0 - t * t);
			float2 m = max(q, 0.0);
			float2 n = (m.x + m.y > 0.0) ? normalize(m) : (q.x > q.y ? float2(1.0, 0.0) : float2(0.0, 1.0));
			float2 disp = -n * s * k;
			return half4(0.5 + 0.5 * disp.x, 0.5 + 0.5 * disp.y, 0.0, 1.0);
		}
		""";

	private static readonly SKRuntimeEffect? _normalMap = CreateNormalMap();

	public float Sigma { get; set; } = 12;
	public float Radius { get; set; } = 24;
	public float Refraction { get; set; }
	public bool Downsample { get; set; }
	public bool ClipAfterSaveLayer { get; set; }
	public static long RenderCount;

	private static SKRuntimeEffect? CreateNormalMap()
	{
		var effect = SKRuntimeEffect.CreateShader(NormalMapSksl, out var errors);
		if (effect is null)
		{
			Console.WriteLine($"[glass-spike] SkSL compile failed: {errors}");
		}
		return effect;
	}

	protected override void RenderOverride(SKCanvas canvas, Size area)
	{
		RenderCount++;
		var bounds = new SKRect(0, 0, (float)area.Width, (float)area.Height);
		using var rrect = new SKRoundRect(bounds, Radius);

		SKImageFilter? chain = null;
		if (Refraction > 0 && _normalMap is not null)
		{
			var uniforms = new SKRuntimeEffectUniforms(_normalMap)
			{
				["size"] = new[] { bounds.Width, bounds.Height },
				["radius"] = Radius,
				["band"] = Math.Min(Radius, 20f),
			};
			using var shader = _normalMap.ToShader(uniforms);
			using var map = SKImageFilter.CreateShader(shader, false, bounds);
			chain = SKImageFilter.CreateDisplacementMapEffect(SKColorChannel.R, SKColorChannel.G, Refraction, map, null, bounds);
		}

		if (Sigma > 0)
		{
			var k = Downsample && Sigma >= 16 ? Sigma / 8f : 1f;
			if (k > 1)
			{
				var down = SKMatrix.CreateScale(1 / k, 1 / k);
				var up = SKMatrix.CreateScale(k, k);
				chain = SKImageFilter.CreateMatrix(in down, new SKSamplingOptions(SKFilterMode.Linear), chain);
				chain = SKImageFilter.CreateBlur(Sigma / k, Sigma / k, SKShaderTileMode.Clamp, chain);
				chain = SKImageFilter.CreateMatrix(in up, new SKSamplingOptions(SKFilterMode.Linear), chain);
			}
			else
			{
				chain = SKImageFilter.CreateBlur(Sigma, Sigma, SKShaderTileMode.Clamp, chain, bounds);
			}
		}

		const float sat = 1.6f, ir = 0.2126f * (1 - sat), ig = 0.7152f * (1 - sat), ib = 0.0722f * (1 - sat);
		using var saturation = SKColorFilter.CreateColorMatrix(new[]
		{
			ir + sat, ig, ib, 0, 0,
			ir, ig + sat, ib, 0, 0,
			ir, ig, ib + sat, 0, 0,
			0, 0, 0, 1, 0,
		});
		chain = SKImageFilter.CreateColorFilter(saturation, chain);

		canvas.Save();
		if (!ClipAfterSaveLayer)
		{
			canvas.ClipRoundRect(rrect, SKClipOperation.Intersect, true);
		}
		canvas.SaveLayer(new SKCanvasSaveLayerRec { Bounds = bounds, Backdrop = chain });
		if (ClipAfterSaveLayer)
		{
			canvas.ClipRoundRect(rrect, SKClipOperation.Intersect, true);
		}

		using var tint = new SKPaint { Color = new SKColor(255, 255, 255, 56), IsAntialias = true };
		canvas.DrawRoundRect(rrect, tint);
		using var rim = new SKPaint { Color = new SKColor(255, 255, 255, 170), IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = 1.5f };
		canvas.DrawRoundRect(rrect, rim);

		canvas.Restore();
		canvas.Restore();
		chain?.Dispose();
	}
}

internal sealed class GlassSpikePage : Page
{
	private readonly string? _autoDir;
	private readonly TextBlock _hud = new() { FontSize = 14, Foreground = new SolidColorBrush(Colors.Yellow), Margin = new Thickness(8) };
	private readonly Button _flyoutButton = new() { Content = "Flyout" };
	private readonly Stopwatch _clock = Stopwatch.StartNew();
	private ScrollViewer? _scroller;
	private int _frames;
	private long _lastFpsTick;
	private double _fps;
	private double _scrollDelta = 6;
	private Grid? _root;

	private static bool NoGlass => Environment.GetCommandLineArgs().Contains("--glass-spike-noglass");

	public GlassSpikePage(string? autoDir)
	{
		_autoDir = autoDir;
		Content = _root = Build();
		Loaded += OnLoaded;
		Unloaded += (_, _) => CompositionTarget.Rendering -= OnRendering;
	}

	private static SpikeGlassBackplate Glass(float sigma, float refraction = 0, bool downsample = false, bool clipAfter = false, double w = 260, double h = 84) =>
		new() { Sigma = sigma, Refraction = refraction, Downsample = downsample, ClipAfterSaveLayer = clipAfter, Width = w, Height = h };

	private static FrameworkElement Labeled(string label, FrameworkElement glass)
	{
		var g = new Grid { Margin = new Thickness(12, 8, 12, 8), HorizontalAlignment = HorizontalAlignment.Left };
		g.Children.Add(glass);
		g.Children.Add(new TextBlock { Text = label, FontSize = 12, Foreground = new SolidColorBrush(Colors.Black), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, TextWrapping = TextWrapping.Wrap, MaxWidth = 220 });
		return g;
	}

	private Grid Build()
	{
		var root = new Grid { Background = new SolidColorBrush(Colors.White) };
		root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(620) });
		root.ColumnDefinitions.Add(new ColumnDefinition());

		// Backdrop: hard black/white stripes, colour blocks, and a bar that animates *behind* the glass (item 2).
		var stripes = new StackPanel { Orientation = Orientation.Horizontal };
		for (var i = 0; i < 16; i++)
		{
			stripes.Children.Add(new Rectangle { Width = 40, Fill = new SolidColorBrush(i % 2 == 0 ? Colors.Black : Colors.White) });
		}
		root.Children.Add(stripes);

		var mover = new Rectangle { Width = 50, Fill = new SolidColorBrush(Colors.Magenta), HorizontalAlignment = HorizontalAlignment.Left, RenderTransform = new TranslateTransform() };
		root.Children.Add(mover);
		var anim = new DoubleAnimation { From = 0, To = 300, Duration = new Duration(TimeSpan.FromSeconds(3)), AutoReverse = true, RepeatBehavior = RepeatBehavior.Forever };
		Storyboard.SetTarget(anim, mover.RenderTransform);
		Storyboard.SetTargetProperty(anim, "X");
		var sb = new Storyboard();
		sb.Children.Add(anim);
		Loaded += (_, _) => sb.Begin();

		var panels = new StackPanel { Margin = new Thickness(0, 40, 0, 0), Visibility = NoGlass ? Visibility.Collapsed : Visibility.Visible };
		panels.Children.Add(Labeled("A blur 12, clip BEFORE SaveLayer", Glass(12)));
		panels.Children.Add(Labeled("B blur 30 downsampled", Glass(30, downsample: true)));
		panels.Children.Add(Labeled("C blur 3 + refraction 24", Glass(3, refraction: 24)));
		var faded = Labeled("D = A at Opacity 0.3 (item 3)", Glass(12));
		faded.Opacity = 0.3;
		panels.Children.Add(faded);
		panels.Children.Add(Labeled("E blur 12, clip AFTER SaveLayer (spec order)", Glass(12, clipAfter: true)));
		root.Children.Add(panels);

		// Item 5: glass bar over a scrolling list.
		var listHost = new Grid();
		Grid.SetColumn(listHost, 1);
		var rows = new StackPanel();
		for (var i = 0; i < 300; i++)
		{
			rows.Children.Add(new Border { Height = 44, Background = new SolidColorBrush(i % 2 == 0 ? Colors.DodgerBlue : Colors.Gold), Child = new TextBlock { Text = $"Row {i} - the quick brown fox", Foreground = new SolidColorBrush(Colors.Black), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(12, 0, 0, 0) } });
		}
		_scroller = new ScrollViewer { Content = rows };
		listHost.Children.Add(_scroller);
		listHost.Children.Add(new Grid
		{
			VerticalAlignment = VerticalAlignment.Top,
			Margin = new Thickness(12),
			Visibility = NoGlass ? Visibility.Collapsed : Visibility.Visible,
			Children = { Glass(20, refraction: 12, downsample: true, w: double.NaN, h: 64) },
		});
		root.Children.Add(listHost);

		var buttons = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Left, Spacing = 8, Margin = new Thickness(8) };
		_flyoutButton.Flyout = new Flyout { Content = Labeled("glass in Flyout", Glass(12, refraction: 12)) };
		var dialog = new Button { Content = "Dialog" };
		dialog.Click += async (_, _) => await ShowDialogAsync(null);
		buttons.Children.Add(_flyoutButton);
		buttons.Children.Add(dialog);
		root.Children.Add(buttons);

		root.Children.Add(_hud);
		return root;
	}

	private async Task ShowDialogAsync(Func<Task>? whileOpen)
	{
		var dlg = new ContentDialog { XamlRoot = XamlRoot, Content = Labeled("glass in ContentDialog", Glass(12, refraction: 12)), CloseButtonText = "Close", Background = new SolidColorBrush(Colors.Transparent) };
		var shown = dlg.ShowAsync();
		if (whileOpen is not null)
		{
			await whileOpen();
			dlg.Hide();
		}
		await shown;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		CompositionTarget.Rendering += OnRendering;
		if (_autoDir is not null)
		{
			_ = RunScriptAsync(_autoDir);
		}
	}

	private void OnRendering(object? sender, object e)
	{
		_frames++;
		var now = _clock.ElapsedMilliseconds;
		if (now - _lastFpsTick >= 1000)
		{
			_fps = _frames * 1000.0 / (now - _lastFpsTick);
			_frames = 0;
			_lastFpsTick = now;
			_hud.Text = $"{_fps:F0} fps | backplate renders {SpikeGlassBackplate.RenderCount} | {Capabilities()}";
		}

		if (_scroller is { } sv)
		{
			if (sv.VerticalOffset >= sv.ScrollableHeight - 1) _scrollDelta = -6;
			else if (sv.VerticalOffset <= 1) _scrollDelta = 6;
			sv.ChangeView(null, sv.VerticalOffset + _scrollDelta, null, disableAnimation: true);
		}
	}

	private static string Capabilities() =>
		$"SKCanvasElement supported={SKCanvasElement.IsSupportedOnCurrentPlatform()} effectsFast={CompositionCapabilities.GetForCurrentView().AreEffectsFast()} effectsSupported={CompositionCapabilities.GetForCurrentView().AreEffectsSupported()}";

	private async Task RunScriptAsync(string dir)
	{
		try
		{
			Directory.CreateDirectory(dir);
			await Task.Delay(4000);
			File.WriteAllText(System.IO.Path.Combine(dir, "info.txt"), $"{Capabilities()}\nfps(4 panels + bar + scrolling list)={_fps:F1}\nrenders={SpikeGlassBackplate.RenderCount}\nos={Environment.OSVersion}\n");

			// Item 7: does RenderTargetBitmap see the backdrop SaveLayer? Captured from the topmost visual so the popup layer is included.
			await CaptureAsync(dir, "main-1");
			await Task.Delay(700);
			await CaptureAsync(dir, "main-2");
			await HandshakeAsync(dir, "main");

			_flyoutButton.Flyout.ShowAt(_flyoutButton);
			await Task.Delay(1000);
			await CaptureAsync(dir, "flyout");
			await HandshakeAsync(dir, "flyout");
			_flyoutButton.Flyout.Hide();

			await ShowDialogAsync(async () =>
			{
				await Task.Delay(1000);
				await CaptureAsync(dir, "dialog");
				await HandshakeAsync(dir, "dialog");
			});

			File.AppendAllText(System.IO.Path.Combine(dir, "info.txt"), $"fps(end)={_fps:F1}\nrenders(end)={SpikeGlassBackplate.RenderCount}\n");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[glass-spike] script failed: {ex}");
			try { File.WriteAllText(System.IO.Path.Combine(dir, "error.txt"), ex.ToString()); } catch (IOException) { }
		}
		finally
		{
			Application.Current.Exit();
		}
	}

	private async Task CaptureAsync(string dir, string name)
	{
		DependencyObject top = _root!;
		while (VisualTreeHelper.GetParent(top) is { } parent)
		{
			top = parent;
		}
		var rtb = new RenderTargetBitmap();
		await rtb.RenderAsync((UIElement)top);
		var pixels = (await rtb.GetPixelsAsync()).ToArray();
		using var bmp = new SKBitmap(new SKImageInfo(rtb.PixelWidth, rtb.PixelHeight, SKColorType.Bgra8888, SKAlphaType.Premul));
		System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmp.GetPixels(), pixels.Length);
		using var data = bmp.Encode(SKEncodedImageFormat.Png, 100);
		File.WriteAllBytes(System.IO.Path.Combine(dir, name + ".png"), data.ToArray());
		File.AppendAllText(System.IO.Path.Combine(dir, "info.txt"), $"capture {name}: root={top.GetType().Name} {rtb.PixelWidth}x{rtb.PixelHeight} scale={XamlRoot?.RasterizationScale}{Environment.NewLine}");
	}

	// Tells the external capture script the stage is on screen, then waits for it to finish grabbing the window.
	private static async Task HandshakeAsync(string dir, string stage)
	{
		File.WriteAllText(System.IO.Path.Combine(dir, stage + ".ready"), "");
		for (var i = 0; i < 100 && !File.Exists(System.IO.Path.Combine(dir, stage + ".done")); i++)
		{
			await Task.Delay(200);
		}
	}

}
