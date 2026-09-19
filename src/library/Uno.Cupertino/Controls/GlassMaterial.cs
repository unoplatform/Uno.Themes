namespace Uno.Cupertino;

/// <summary>
/// The Liquid Glass variants a <see cref="GlassPanel"/> can render. Each one is a preset of blur,
/// saturation, refraction and rim strength.
/// </summary>
public enum GlassMaterial : int
{
	/// <summary>The default glass: bars, glass buttons, sidebars.</summary>
	Regular = 0,

	/// <summary>Glass meant to carry a <see cref="GlassPanel.TintColor"/>: prominent buttons, the primary toolbar item.</summary>
	Prominent = 1,

	/// <summary>A light blur with strong refraction: small moving parts such as a slider knob.</summary>
	Thin = 2,

	/// <summary>A heavy, saturated blur: popovers, menus, alerts and sheets.</summary>
	Thick = 3,

	/// <summary>The most transparent glass, for media overlays. The consumer supplies its own dimming layer.</summary>
	Clear = 4,
}
