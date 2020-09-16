using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Uno.Material.Controls
{
	public delegate void ChipItemEventHandler(object sender, ItemClickEventArgs e);

	public sealed class ItemClickEventArgs : EventArgs
	{
		public ItemClickEventArgs(object item) => Item = item;

		public object Item { get; }
	}
}
