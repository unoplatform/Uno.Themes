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
	// A capsule lens. The sample point moves outward from the centre in proportion to its distance from it,
	// so the glass shows the backdrop minified, strongest at the centre and easing to nothing at the tips, so
	// what shows at the rim is what lies beneath it: a track running through the knob shows as an hourglass,
	// narrowest in the middle and widening until it meets the real track at both caps; a track ending under
	// the knob shows its end pushed in (iOS 26). The long-axis shift is half the short-axis one so that,
	// with the 1 − u² ease, the mapping never runs backwards along the length (a fold would read as a hard
	// step); a steeper ease aliases at the caps on the GPU.
	// Encoded into R / G for SKImageFilter.CreateDisplacementMapEffect, where 0.5 is "no displacement" and
	// the full shift along the longer axis is half the effect's scale. Evaluated in canvas coordinates,
	// hence the origin.
	private const string LensMapSksl = """
		uniform float2 origin;
		uniform float2 size;
		half4 main(float2 p) {
			float2 c = size * 0.5;
			float2 v = p - origin - c;
			bool horizontal = c.x >= c.y;
			float u = horizontal ? abs(v.x) / c.x : abs(v.y) / c.y;
			float w = 1.0 - u * u;
			float2 disp = v / max(c.x, c.y) * w;
			if (horizontal) { disp.x *= 0.5; } else { disp.y *= 0.5; }
			return half4(0.5 + 0.5 * disp.x, 0.5 + 0.5 * disp.y, 0.0, 1.0);
		}
		""";

	// Blurs at or above this sigma are computed on a downsampled layer (k = sigma / 8), as Uno's acrylic does.
	private const float DownsampleSigma = 16f;


	// Drop shadow of a preset with Shadow > 0: offset down, blurred. The element is inflated by ShadowMargin
	// on every side (a negative Margin set by GlassPanel) so the shadow has room outside the glass shape.
	private const float ShadowDy = 5f;
	private const float ShadowSigma = 7f;
	internal const float ShadowMargin = 20f;

	private static readonly Lazy<SKRuntimeEffect?> _lensMap = new(CreateLensMap);

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

	private static SKRuntimeEffect? CreateLensMap()
	{
		var effect = SKRuntimeEffect.CreateShader(LensMapSksl, out var errors);
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

		// The backdrop the filters read is wider than the shape when the element is inflated, so that the
		// refraction can sample what lies just outside the glass instead of reading transparent.
		var sampleBounds = bounds;
		sampleBounds.Inflate(inset, inset);

		var key = (width, height, radius, Preset);
		if (_chain is null || key != _chainKey)
		{
			_chain?.Dispose();
			_chain = CreateChain(bounds, sampleBounds, Preset);
			_chainKey = key;
		}

		// The clip has to come first: the layer is filled with the filtered backdrop when it is created and
		// composited through the clip active at that point. Clipping afterwards only trims the rim, and the
		// blurred backdrop leaks into the square corners.
		canvas.Save();
		canvas.ClipRoundRect(shape, SKClipOperation.Intersect, true);

		using var layerPaint = new SKPaint { Color = SKColors.White.WithAlpha((byte)(_layerOpacity * 255)) };
		canvas.SaveLayer(new SKCanvasSaveLayerRec { Bounds = sampleBounds, Backdrop = _chain, Paint = layerPaint });

		if (Preset.Sheen > 0)
		{
			// Body shading across the short axis: light caught above the middle, shadow below it. Faint and
			// soft — at full strength the knob reads as chrome, not glass.
			var vertical = bounds.Width >= bounds.Height;
			using var sheenShader = SKShader.CreateLinearGradient(
				vertical ? new SKPoint(bounds.MidX, bounds.Top) : new SKPoint(bounds.Left, bounds.MidY),
				vertical ? new SKPoint(bounds.MidX, bounds.Bottom) : new SKPoint(bounds.Right, bounds.MidY),
				new[]
				{
					SKColors.White.WithAlpha((byte)(Preset.Sheen * 0.30f * 255)),
					SKColors.White.WithAlpha((byte)(Preset.Sheen * 0.30f * 255)),
					SKColors.Transparent,
					SKColors.Black.WithAlpha((byte)(Preset.Sheen * 0.14f * 255)),
					SKColors.Black.WithAlpha((byte)(Preset.Sheen * 0.14f * 255)),
				},
				new[] { 0f, 0.2f, 0.5f, 0.8f, 1f },
				SKShaderTileMode.Clamp);
			using var sheen = new SKPaint { Shader = sheenShader, IsAntialias = true };
			canvas.DrawRoundRect(shape, sheen);
		}

		if (Preset.Outline > 0)
		{
			// A hairline at the very edge, inside the clip: the boundary of a clear lens against its surface.
			using var outlineShape = new SKRoundRect(bounds, radius);
			outlineShape.Deflate(0.5f, 0.5f);
			using var outline = new SKPaint
			{
				Color = SKColors.Black.WithAlpha((byte)(Preset.Outline * 255)),
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				StrokeWidth = 1,
			};
			canvas.DrawRoundRect(outlineShape, outline);
		}

		if (Preset.Rim > 0)
		{
			// Just inside the outline, inset by half the stroke so the clip does not cut the rim in two. Lit
			// from above: full alpha at the top edge, fading towards the bottom.
			using var rimShape = new SKRoundRect(bounds, radius);
			var rimInset = (Preset.RimWidth / 2) + (Preset.Outline > 0 ? 1 : 0);
			rimShape.Deflate(rimInset, rimInset);
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

		if (Preset.Shadow > 0)
		{
			// After the backdrop was read, so the lens does not show its own shadow (iOS does not), and only
			// outside the shape, so the glass stays clear.
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
	}

	// Refraction, then blur, then saturation. Kept in one method: when SkiaSharp exposes runtime-effect
	// image filters the whole chain collapses into one shader, and the swap stays local.
	private static SKImageFilter CreateChain(SKRect bounds, SKRect sampleBounds, GlassPreset preset)
	{
		SKImageFilter? chain = null;

		if (preset.Refraction > 0 && _lensMap.Value is { } lensMap)
		{
			var uniforms = new SKRuntimeEffectUniforms(lensMap)
			{
				["origin"] = new[] { bounds.Left, bounds.Top },
				["size"] = new[] { bounds.Width, bounds.Height },
			};
			using var shader = lensMap.ToShader(uniforms);
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
		// Contrast is anchored at white (out = c·in + 1 − c): a light page stays light through the lens while
		// a grey track deepens, as through the iOS knob. The offset column is in 0–1 units.
		var s = preset.Saturation;
		var contrast = preset.Contrast;
		var offset = 1 - contrast;
		float r = 0.2126f * (1 - s), g = 0.7152f * (1 - s), b = 0.0722f * (1 - s);
		using var saturation = SKColorFilter.CreateColorMatrix(new[]
		{
			(r + s) * contrast, g * contrast, b * contrast, 0, offset,
			r * contrast, (g + s) * contrast, b * contrast, 0, offset,
			r * contrast, g * contrast, (b + s) * contrast, 0, offset,
			0, 0, 0, 1, 0,
		});
		return SKImageFilter.CreateColorFilter(saturation, chain);
	}
}
#endif
