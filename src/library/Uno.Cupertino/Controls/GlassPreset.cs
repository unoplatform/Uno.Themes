namespace Uno.Cupertino;

/// <summary>
/// The drawing parameters of one <see cref="GlassMaterial"/>.
/// </summary>
/// <param name="Sigma">Blur sigma of the backdrop, in pixels.</param>
/// <param name="Saturation">Saturation applied to the blurred backdrop; 1 leaves it unchanged.</param>
/// <param name="Refraction">Lens strength: twice the outward shift of the sample point at the rim along the longer axis, in pixels. The glass shows the backdrop minified by 1 + Refraction / longer side.</param>
/// <param name="Rim">Alpha of the white specular rim, 0–1.</param>
/// <param name="RimWidth">Stroke width of the specular rim, in pixels.</param>
/// <param name="Veil">Opacity of the panel's <c>Background</c> over the glass, 0–1. It keeps content on top legible.</param>
/// <param name="Shadow">Alpha of the drop shadow the surface casts on what is beneath it, 0–1. 0 casts none.</param>
/// <param name="Outline">Alpha of the thin dark line at the very edge of the glass, 0–1, which separates a clear lens from what it sits on.</param>
/// <param name="Contrast">Contrast of what shows through, anchored at white: 1 leaves it alone, above 1 keeps light surfaces light and deepens mid-tones, as a thick clear lens does.</param>
/// <param name="Sheen">Strength of the body shading, 0–1: a light band above the middle and a dark band below it, the light caught in a thick lens.</param>
internal readonly record struct GlassPreset(float Sigma, float Saturation, float Refraction, float Rim, float Veil, float Shadow = 0, float RimWidth = 1, float Outline = 0, float Contrast = 1, float Sheen = 0)
{
	// Starting values from spec 10 (token-and-style-mapping.md §7): tuned from Apple's qualitative rules
	// and Cupertino.Avalonia's presets, still to be measured against the iOS 26 design kit.
	public static GlassPreset For(GlassMaterial material) => material switch
	{
		GlassMaterial.Prominent => new(Sigma: 12, Saturation: 1.0f, Refraction: 7, Rim: 0.18f, Veil: 0.10f),
		GlassMaterial.Thin => new(Sigma: 4, Saturation: 1.1f, Refraction: 9, Rim: 0.12f, Veil: 0.12f),
		GlassMaterial.Thick => new(Sigma: 30, Saturation: 1.8f, Refraction: 3, Rim: 0.5f, Veil: 0.55f),
		// Clear is the lens of a lifted switch/slider knob (iOS 26 switch): no veil, no blur, and a strong lens
		// that shows the track and page beneath it minified to about 60 %, bounded by a hairline outline and a
		// faint top-lit highlight, on a soft shadow. Starting values matched to an iOS 27 simulator capture.
		GlassMaterial.Clear => new(Sigma: 0, Saturation: 1.05f, Refraction: 32, Rim: 0.7f, Veil: 0f, Shadow: 0.18f, RimWidth: 1, Outline: 0.22f, Contrast: 1.6f, Sheen: 0.3f),
		_ => new(Sigma: 12, Saturation: 1.3f, Refraction: 7, Rim: 0.30f, Veil: 0.22f),
	};
}
