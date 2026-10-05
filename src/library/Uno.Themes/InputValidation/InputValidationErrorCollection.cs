// TEMPORARY - stand-in for Uno's input validation until unoplatform/uno#24838 is merged and published.
// Delete this folder when bumping to that version: the framework ships the same types in the same namespace.
#if HAS_UNO
#nullable enable

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.Foundation.Collections;

namespace Uno.Extras.Input;

/// <summary>
/// The <see cref="Validation.GetErrors"/> collection: an observable vector like the framework's, which
/// also raises <see cref="INotifyCollectionChanged"/> for bindings.
/// </summary>
internal sealed class InputValidationErrorCollection : ObservableCollection<InputValidationError>, IObservableVector<InputValidationError>
{
	public event VectorChangedEventHandler<InputValidationError>? VectorChanged;

	protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	{
		base.OnCollectionChanged(e);

		if (VectorChanged is not { } handler)
		{
			return;
		}

		var args = e.Action switch
		{
			NotifyCollectionChangedAction.Add => new VectorChangedEventArgs(CollectionChange.ItemInserted, (uint)e.NewStartingIndex),
			NotifyCollectionChangedAction.Remove => new VectorChangedEventArgs(CollectionChange.ItemRemoved, (uint)e.OldStartingIndex),
			NotifyCollectionChangedAction.Replace => new VectorChangedEventArgs(CollectionChange.ItemChanged, (uint)e.NewStartingIndex),
			_ => new VectorChangedEventArgs(CollectionChange.Reset, 0),
		};
		handler(this, args);
	}

	private sealed record VectorChangedEventArgs(CollectionChange CollectionChange, uint Index) : IVectorChangedEventArgs;
}
#endif
