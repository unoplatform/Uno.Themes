// TEMPORARY - stand-in for Uno's input validation until unoplatform/uno#24838 is merged and published.
// Delete this folder when bumping to that version: the framework ships the same types in the same namespace.
#if HAS_UNO
namespace Uno.Extras.Input;

/// <summary>
/// Whether a control validates its bound value.
/// </summary>
public enum InputValidationMode : int
{
	/// <summary>Errors reported by the binding source (<see cref="System.ComponentModel.INotifyDataErrorInfo"/>) are presented.</summary>
	Auto = 0,

	/// <summary>The default: no validation.</summary>
	Default = 1,

	/// <summary>No validation.</summary>
	Disabled = 2,
}
#endif
