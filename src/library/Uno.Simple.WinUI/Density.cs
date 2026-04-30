namespace Uno.Simple;

/// <summary>
/// Controls the overall spacing density of Simple theme controls.
/// Each level maps to a base spacing unit that scales all Space* tokens.
/// </summary>
public enum Density
{
	/// <summary>Tighter spacing (base unit = 3px).</summary>
	Compact,

	/// <summary>Default spacing (base unit = 4px).</summary>
	Regular,

	/// <summary>Looser spacing (base unit = 5px).</summary>
	Comfy,
}
