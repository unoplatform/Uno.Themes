namespace Uno.Cupertino;

/// <summary>
/// The drawing parameters of one <see cref="GlassMaterial"/>.
/// </summary>
/// <param name="Sigma">Blur sigma of the backdrop, in pixels.</param>
/// <param name="Saturation">Saturation applied to the blurred backdrop; 1 leaves it unchanged.</param>
/// <param name="Refraction">Displacement of the backdrop along the edge, in pixels.</param>
/// <param name="Rim">Alpha of the white specular rim, 0–1.</param>
/// <param name="Veil">Opacity of the panel's <c>Background</c> over the glass, 0–1. It keeps content on top legible.</param>
internal readonly record struct GlassPreset(float Sigma, float Saturation, float Refraction, float Rim, float Veil)
{
	// Starting values from spec 10 (token-and-style-mapping.md §7): tuned from Apple's qualitative rules
	// and Cupertino.Avalonia's presets, still to be measured against the iOS 26 design kit.
	public static GlassPreset For(GlassMaterial material) => material switch
	{
		GlassMaterial.Prominent => new(Sigma: 12, Saturation: 1.0f, Refraction: 7, Rim: 0.18f, Veil: 0.10f),
		GlassMaterial.Thin => new(Sigma: 4, Saturation: 1.1f, Refraction: 9, Rim: 0.12f, Veil: 0.12f),
		GlassMaterial.Thick => new(Sigma: 30, Saturation: 1.8f, Refraction: 3, Rim: 0.5f, Veil: 0.55f),
		GlassMaterial.Clear => new(Sigma: 8, Saturation: 1.0f, Refraction: 7, Rim: 0.30f, Veil: 0.05f),
		_ => new(Sigma: 12, Saturation: 1.3f, Refraction: 7, Rim: 0.30f, Veil: 0.22f),
	};
}
