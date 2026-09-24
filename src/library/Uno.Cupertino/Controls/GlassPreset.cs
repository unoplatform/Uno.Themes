namespace Uno.Cupertino;

/// <summary>
/// The drawing parameters of one <see cref="GlassMaterial"/>.
/// </summary>
/// <param name="Sigma">Blur sigma of the backdrop, in pixels.</param>
/// <param name="Saturation">Saturation applied to the blurred backdrop; 1 leaves it unchanged.</param>
/// <param name="Refraction">Lens strength K of the cubic profile s = n (1 + K n²), n being the position across the glass in −1…1: a thin line at the centre is left alone, content towards the rim is drawn in. 0 is flat glass.</param>
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
		// Refraction is off for the blurred materials until it has been measured against Apple's surfaces.
		GlassMaterial.Prominent => new(Sigma: 12, Saturation: 1.0f, Refraction: 0, Rim: 0.18f, Veil: 0.10f),
		GlassMaterial.Thin => new(Sigma: 4, Saturation: 1.1f, Refraction: 0, Rim: 0.12f, Veil: 0.12f),
		GlassMaterial.Thick => new(Sigma: 30, Saturation: 1.8f, Refraction: 0, Rim: 0.5f, Veil: 0.55f),
		// Clear is the lens of a lifted switch/slider knob (iOS 26 switch): no veil, no blur, and a strong lens
		// that shows the track and page beneath it minified to about 60 %, bounded by a hairline outline and a
		// faint top-lit highlight, on a soft shadow. Starting values matched to an iOS 27 simulator capture.
		GlassMaterial.Clear => new(Sigma: 0, Saturation: 1f, Refraction: 2.2f, Rim: 0.7f, Veil: 0f, Shadow: 0.18f, RimWidth: 1, Outline: 0.22f, Sheen: 0.2f),
		_ => new(Sigma: 12, Saturation: 1.3f, Refraction: 0, Rim: 0.30f, Veil: 0.22f),
	};
}
