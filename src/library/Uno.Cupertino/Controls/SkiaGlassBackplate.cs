#if HAS_UNO
#nullable enable
using System;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using Uno.Extensions;
using Uno.WinUI.Graphics2DSK;
using Windows.Foundation;

namespace Uno.Cupertino;

/// <summary>
/// The Liquid tier of <see cref="GlassPanel"/>: reads what is already drawn beneath it through a backdrop
/// <c>SaveLayer</c>, then refracts, blurs and saturates it.
/// </summary>
internal sealed partial class SkiaGlassBackplate : SKCanvasElement
{
	// Encodes the outward normal of a rounded rectangle, weighted by a lens profile over the edge band,
	// into R / G for SKImageFilter.CreateDisplacementMapEffect. 0.5 is "no displacement".
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

	// The refraction band never gets wider than this, however large the corner radius is.
	private const float MaxRefractionBand = 20f;

	// Blurs at or above this sigma are computed on a downsampled layer (k = sigma / 8), as Uno's acrylic does.
	private const float DownsampleSigma = 16f;

	private const float RimStrokeWidth = 1f;

	private static readonly Lazy<SKRuntimeEffect?> _normalMap = new(CreateNormalMap);

	private SKImageFilter? _chain;
	private (float Width, float Height, float Radius, GlassPreset Preset) _chainKey;
	private float _layerOpacity = 1f;
	private bool _isSubscribed;

	public SkiaGlassBackplate()
	{
		Loaded += OnLoaded;
		Unloaded += OnUnloaded;
	}

	/// <summary>The material parameters to draw with.</summary>
	public GlassPreset Preset { get; set; } = GlassPreset.For(GlassMaterial.Regular);

	/// <summary>The corner radius, in pixels. Clamped to half the shorter side when drawing.</summary>
	public float Radius { get; set; }

	/// <summary>How many times <see cref="RenderOverride"/> ran; lets a test observe the per-frame repaint.</summary>
	internal int RenderCount { get; private set; }

	internal bool IsSubscribed => _isSubscribed;

	private static SKRuntimeEffect? CreateNormalMap()
	{
		var effect = SKRuntimeEffect.CreateShader(NormalMapSksl, out var errors);
		if (effect is null && typeof(SkiaGlassBackplate).Log().IsEnabled(LogLevel.Warning))
		{
			typeof(SkiaGlassBackplate).Log().LogWarning("The glass refraction shader did not compile; glass renders without refraction. {Errors}", errors);
		}

		return effect;
	}

	// The panel must repaint on every frame while it is on screen. With damage-region rendering, a backplate
	// outside the damaged strip reads its OWN previous output back as "backdrop" and blurs it again, which
	// smears anything moving behind it into a trail (measured on a GPU, spec 10 §9). Invalidating damages the
	// whole bounds, so the content beneath is redrawn before it is read. Cost: the render loop does not idle
	// while glass is visible.
	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (!_isSubscribed)
		{
			_isSubscribed = true;
			CompositionTarget.Rendering += OnRendering;
		}
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		if (_isSubscribed)
		{
			_isSubscribed = false;
			CompositionTarget.Rendering -= OnRendering;
		}

		_chain?.Dispose();
		_chain = null;
	}

	private void OnRendering(object? sender, object e)
	{
		// Uno does not apply Opacity to what an SKCanvasElement draws into a backdrop layer, so the
		// effective opacity is folded into the layer paint instead.
		var opacity = 1.0;
		for (DependencyObject? d = this; d is not null; d = VisualTreeHelper.GetParent(d))
		{
			if (d is UIElement element)
			{
				opacity *= element.Opacity;
			}
		}

		_layerOpacity = (float)Math.Clamp(opacity, 0, 1);
		Invalidate();
	}

	protected override void RenderOverride(SKCanvas canvas, Size area)
	{
		RenderCount++;

		var width = (float)area.Width;
		var height = (float)area.Height;
		if (width <= 0 || height <= 0 || _layerOpacity <= 0)
		{
			return;
		}

		var bounds = new SKRect(0, 0, width, height);
		var radius = Math.Clamp(Radius, 0, Math.Min(width, height) / 2);
		using var shape = new SKRoundRect(bounds, radius);

		var key = (width, height, radius, Preset);
		if (_chain is null || key != _chainKey)
		{
			_chain?.Dispose();
			_chain = CreateChain(bounds, radius, Preset);
			_chainKey = key;
		}

		// The clip has to come first: the layer is filled with the filtered backdrop when it is created and
		// composited through the clip active at that point. Clipping afterwards only trims the rim, and the
		// blurred backdrop leaks into the square corners.
		canvas.Save();
		canvas.ClipRoundRect(shape, SKClipOperation.Intersect, true);

		using var layerPaint = new SKPaint { Color = SKColors.White.WithAlpha((byte)(_layerOpacity * 255)) };
		canvas.SaveLayer(new SKCanvasSaveLayerRec { Bounds = bounds, Backdrop = _chain, Paint = layerPaint });

		if (Preset.Rim > 0)
		{
			// Inset by half the stroke so the clip does not cut the rim in two.
			using var rimShape = new SKRoundRect(bounds, radius);
			rimShape.Deflate(RimStrokeWidth / 2, RimStrokeWidth / 2);
			using var rim = new SKPaint
			{
				Color = SKColors.White.WithAlpha((byte)(Preset.Rim * 255)),
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = RimStrokeWidth,
			};
			canvas.DrawRoundRect(rimShape, rim);
		}

		canvas.Restore();
		canvas.Restore();
	}

	// Refraction, then blur, then saturation. Kept in one method: when SkiaSharp exposes runtime-effect
	// image filters the whole chain collapses into one shader, and the swap stays local.
	private static SKImageFilter CreateChain(SKRect bounds, float radius, GlassPreset preset)
	{
		SKImageFilter? chain = null;

		if (preset.Refraction > 0 && radius > 0 && _normalMap.Value is { } normalMap)
		{
			var uniforms = new SKRuntimeEffectUniforms(normalMap)
			{
				["size"] = new[] { bounds.Width, bounds.Height },
				["radius"] = radius,
				["band"] = Math.Min(radius, MaxRefractionBand),
			};
			using var shader = normalMap.ToShader(uniforms);
			using var map = SKImageFilter.CreateShader(shader, false, bounds);
			chain = SKImageFilter.CreateDisplacementMapEffect(SKColorChannel.R, SKColorChannel.G, preset.Refraction, map, null, bounds);
		}

		if (preset.Sigma > 0)
		{
			// Clamp tiling throughout, so the blur never samples outside its own bounds.
			if (preset.Sigma >= DownsampleSigma)
			{
				var k = preset.Sigma / 8f;
				var down = SKMatrix.CreateScale(1 / k, 1 / k);
				var up = SKMatrix.CreateScale(k, k);
				var sampling = new SKSamplingOptions(SKFilterMode.Linear);
				chain = SKImageFilter.CreateMatrix(in down, sampling, chain);
				chain = SKImageFilter.CreateBlur(preset.Sigma / k, preset.Sigma / k, SKShaderTileMode.Clamp, chain);
				chain = SKImageFilter.CreateMatrix(in up, sampling, chain);
			}
			else
			{
				chain = SKImageFilter.CreateBlur(preset.Sigma, preset.Sigma, SKShaderTileMode.Clamp, chain, bounds);
			}
		}

		// Rec. 709 luma weights; a saturation above 1 is what makes content behind the glass look vibrant.
		var s = preset.Saturation;
		float r = 0.2126f * (1 - s), g = 0.7152f * (1 - s), b = 0.0722f * (1 - s);
		using var saturation = SKColorFilter.CreateColorMatrix(new[]
		{
			r + s, g, b, 0, 0,
			r, g + s, b, 0, 0,
			r, g, b + s, 0, 0,
			0, 0, 0, 1, 0,
		});
		return SKImageFilter.CreateColorFilter(saturation, chain);
	}
}
#endif
