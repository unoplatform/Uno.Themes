// TEMPORARY - stand-in for Uno's input validation until unoplatform/uno#24838 is merged and published.
// Delete this folder when bumping to that version: the framework ships the same types in the same namespace.
#if HAS_UNO
#nullable enable

namespace Uno.Extras.Input;

/// <summary>
/// A validation error presented by a control.
/// </summary>
public class InputValidationError
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InputValidationError"/> class.
	/// </summary>
	/// <param name="errorMessage">The message to present.</param>
	public InputValidationError(string errorMessage) => ErrorMessage = errorMessage;

	/// <summary>
	/// Gets the message to present.
	/// </summary>
	public string ErrorMessage { get; }

	/// <inheritdoc />
	public override string ToString() => ErrorMessage;
}
#endif
