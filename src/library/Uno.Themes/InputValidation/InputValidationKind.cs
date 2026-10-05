// TEMPORARY - stand-in for Uno's input validation until unoplatform/uno#24838 is merged and published.
// Delete this folder when bumping to that version: the framework ships the same types in the same namespace.
#if HAS_UNO
namespace Uno.Extras.Input;

/// <summary>
/// How a control presents its validation errors.
/// </summary>
public enum InputValidationKind : int
{
	/// <summary>Resolves to <see cref="Compact"/>.</summary>
	Auto = 0,

	/// <summary>An error icon beside the field, listing the errors in its tooltip.</summary>
	Compact = 1,

	/// <summary>The errors listed under the field.</summary>
	Inline = 2,
}
#endif
