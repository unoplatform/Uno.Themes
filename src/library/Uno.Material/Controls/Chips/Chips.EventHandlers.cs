using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.Material.Controls
{
	#region Chip event handlers
	public delegate void ChipRemovingEventHandler(object sender, ChipRemovingEventArgs e);
	public sealed class ChipRemovingEventArgs : EventArgs
	{
		public bool Cancel { get; set; }
	}

	public delegate void ChipRemovedEventHandler(object sender, EventArgs e);
	#endregion


	#region ChipGroup event handlers
	public delegate void ChipItemEventHandler(object sender, ChipItemEventArgs e);
	public class ChipItemEventArgs : EventArgs
	{
		internal ChipItemEventArgs(object item) => Item = item;

		public object Item { get; }
	}

	public delegate void ChipItemRemovingEventHandler(object sender, ChipItemEventArgs e);
	public class ChipItemRemovingEventArgs : ChipItemEventArgs
	{
		public ChipItemRemovingEventArgs(object item) : base(item) { }

		public bool Cancel { get; set; }
	}

	#endregion
}
