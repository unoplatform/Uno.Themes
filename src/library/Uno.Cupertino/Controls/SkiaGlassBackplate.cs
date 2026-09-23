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
	// into R / G for SKImageFilter.CreateDisplacementMapEffect. 0.5 is "no displacement". The map is
	// evaluated in canvas coordinates, so the shape's origin is a uniform. The sample point moves OUTWARD
	// along the normal, strongest at the rim: what lies just outside the glass is pulled in across the edge
	// band, the way Liquid Glass shows a shrunken copy of the track edge inside a knob.
	private const string NormalMapSksl = """
		uniform float2 origin;
		uniform float2 size;
		uniform float radius;
		uniform float band;
		half4 main(float2 p) {
			p -= origin;
			float2 c = size * 0.5;
			float2 s = sign(p - c);
			float2 q = abs(p - c) - (c - radius);
			float d = length(max(q, 0.0)) + min(max(q.x, q.y), 0.0) - radius;
			float t = clamp(1.0 + d / band, 0.0, 1.0);
			// A bevelled slab, not a sphere: the outer half of the band is displaced by the full amount, so
			// what shows through it is a crisp, inset copy of the surroundings; the inner half ramps to zero.
			float k = smoothstep(0.0, 0.5, t);
			float2 m = max(q, 0.0);
			float2 n = (m.x + m.y > 0.0) ? normalize(m) : (q.x > q.y ? float2(1.0, 0.0) : float2(0.0, 1.0));
			float2 disp = n * s * k;
			return half4(0.5 + 0.5 * disp.x, 0.5 + 0.5 * disp.y, 0.0, 1.0);
		}
		""";

	// The refraction band never gets wider than this, however large the corner radius is.
	private const float MaxRefractionBand = 20f;

	// Blurs at or above this sigma are computed on a downsampled layer (k = sigma / 8), as Uno's acrylic does.
	private const float DownsampleSigma = 16f;


	// Drop shadow of a preset with Shadow > 0: offset down, blurred. The element is inflated by ShadowMargin
	// on every side (a negative Margin set by GlassPanel) so the shadow has room outside the glass shape.
	private const float ShadowDy = 5f;
	private const float ShadowSigma = 6f;
	internal const float ShadowMargin = 20f;

	// Inner shadow: a soft dark fill over the lens body, inset by the refracting band, so the band stays bright
	// while what shows through the flat centre of the lens reads darker — a thick slab, not a film.
	private const float InnerShadowSigma = 1f;

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

		var inset = Preset.Shadow > 0 ? ShadowMargin : 0;
		var bounds = new SKRect(inset, inset, width - inset, height - inset);
		if (bounds.Width <= 0 || bounds.Height <= 0)
		{
			return;
		}

		var radius = Math.Clamp(Radius, 0, Math.Min(bounds.Width, bounds.Height) / 2);
		using var shape = new SKRoundRect(bounds, radius);

		if (Preset.Shadow > 0)
		{
			// Only outside the shape: the lens stays clear and the shadow does not darken its own backdrop.
			canvas.Save();
			canvas.ClipRoundRect(shape, SKClipOperation.Difference, true);
			canvas.Translate(0, ShadowDy);
			using var shadowFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, ShadowSigma);
			using var shadow = new SKPaint
			{
				Color = SKColors.Black.WithAlpha((byte)(Preset.Shadow * _layerOpacity * 255)),
				IsAntialias = true,
				MaskFilter = shadowFilter,
			};
			canvas.DrawRoundRect(shape, shadow);
			canvas.Restore();
		}

		// The backdrop the filters read is wider than the shape when the element is inflated, so that the
		// refraction can sample what lies just outside the glass instead of reading transparent.
		var sampleBounds = bounds;
		sampleBounds.Inflate(inset, inset);

		var key = (width, height, radius, Preset);
		if (_chain is null || key != _chainKey)
		{
			_chain?.Dispose();
			_chain = CreateChain(bounds, sampleBounds, radius, Preset);
			_chainKey = key;
		}

		// The clip has to come first: the layer is filled with the filtered backdrop when it is created and
		// composited through the clip active at that point. Clipping afterwards only trims the rim, and the
		// blurred backdrop leaks into the square corners.
		canvas.Save();
		canvas.ClipRoundRect(shape, SKClipOperation.Intersect, true);

		using var layerPaint = new SKPaint { Color = SKColors.White.WithAlpha((byte)(_layerOpacity * 255)) };
		canvas.SaveLayer(new SKCanvasSaveLayerRec { Bounds = sampleBounds, Backdrop = _chain, Paint = layerPaint });

		if (Preset.InnerShadow > 0 || Preset.BandLight > 0)
		{
			// The body starts where the inset copy of the surroundings starts: one displacement (half the
			// DisplacementMap scale) plus the rim in from the edge. The band between rim and body catches
			// light; the body reads darker, like looking down through a thick slab.
			var bodyInset = (Preset.Refraction / 2) + Preset.RimWidth;
			using var body = new SKRoundRect(bounds, radius);
			body.Deflate(bodyInset, bodyInset);

			if (Preset.BandLight > 0)
			{
				using var band = new SKPaint { Color = SKColors.White.WithAlpha((byte)(Preset.BandLight * 255)), IsAntialias = true };
				canvas.Save();
				canvas.ClipRoundRect(body, SKClipOperation.Difference, true);
				canvas.DrawRoundRect(shape, band);
				canvas.Restore();
			}

			if (Preset.InnerShadow > 0)
			{
				using var innerFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, InnerShadowSigma);
				using var inner = new SKPaint
				{
					Color = SKColors.Black.WithAlpha((byte)(Preset.InnerShadow * 255)),
					IsAntialias = true,
					MaskFilter = innerFilter,
				};
				canvas.DrawRoundRect(body, inner);
			}
		}

		if (Preset.Rim > 0)
		{
			// Inset by half the stroke so the clip does not cut the rim in two. Lit from above: full alpha at
			// the top edge, fading towards the bottom.
			using var rimShape = new SKRoundRect(bounds, radius);
			rimShape.Deflate(Preset.RimWidth / 2, Preset.RimWidth / 2);
			using var rimShader = SKShader.CreateLinearGradient(
				new SKPoint(bounds.MidX, bounds.Top),
				new SKPoint(bounds.MidX, bounds.Bottom),
				new[] { SKColors.White.WithAlpha((byte)(Preset.Rim * 255)), SKColors.White.WithAlpha((byte)(Preset.Rim * 0.45f * 255)) },
				SKShaderTileMode.Clamp);
			using var rim = new SKPaint
			{
				Shader = rimShader,
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = Preset.RimWidth,
			};
			canvas.DrawRoundRect(rimShape, rim);
		}

		canvas.Restore();
		canvas.Restore();
	}

	// Refraction, then blur, then saturation. Kept in one method: when SkiaSharp exposes runtime-effect
	// image filters the whole chain collapses into one shader, and the swap stays local.
	private static SKImageFilter CreateChain(SKRect bounds, SKRect sampleBounds, float radius, GlassPreset preset)
	{
		SKImageFilter? chain = null;

		if (preset.Refraction > 0 && radius > 0 && _normalMap.Value is { } normalMap)
		{
			var uniforms = new SKRuntimeEffectUniforms(normalMap)
			{
				["origin"] = new[] { bounds.Left, bounds.Top },
				["size"] = new[] { bounds.Width, bounds.Height },
				["radius"] = radius,
				["band"] = Math.Min(radius, MaxRefractionBand),
			};
			using var shader = normalMap.ToShader(uniforms);
			using var map = SKImageFilter.CreateShader(shader, false, sampleBounds);
			chain = SKImageFilter.CreateDisplacementMapEffect(SKColorChannel.R, SKColorChannel.G, preset.Refraction, map, null, sampleBounds);
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
				chain = SKImageFilter.CreateBlur(preset.Sigma, preset.Sigma, SKShaderTileMode.Clamp, chain, sampleBounds);
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
