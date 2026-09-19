namespace Uno.Cupertino;

/// <summary>
/// How a <see cref="GlassPanel"/> is drawn.
/// </summary>
public enum GlassRenderingMode : int
{
	/// <summary>Liquid where the platform renders through Skia with fast effects, Solid everywhere else.</summary>
	Auto = 0,

	/// <summary>The blurred, refracting backdrop, wherever Skia can draw it, even when effects are slow.</summary>
	Liquid = 1,

	/// <summary>An opaque fill with a hairline. This is also the Reduce Transparency rendering.</summary>
	Solid = 2,
}
