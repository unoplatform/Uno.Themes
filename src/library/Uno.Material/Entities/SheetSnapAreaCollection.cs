using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.Material.Entities
{
	/// <summary>
	/// This is a list of <see cref="SheetSnapArea"/>.
	/// This class exists to simplify the XAML writing of <see cref="DrawerView.SnapAreas"/>.
	/// </summary>
	public class SheetSnapAreaCollection : List<SheetSnapArea>
	{
		public static readonly SheetSnapAreaCollection Empty = new SheetSnapAreaCollection();
	}
}
