namespace Uno.Themes;

/// <summary>
/// Controls the spacing density applied to all controls.
/// Drives the base spacing unit used by all Space* tokens.
/// </summary>
public enum Density
{
	/// <summary>Tighter spacing (base unit = 3px).</summary>
	Compact = 3,

	/// <summary>Default spacing (base unit = 4px).</summary>
	Regular = 4,

	/// <summary>Looser spacing (base unit = 5px).</summary>
	Comfy = 5,
}
