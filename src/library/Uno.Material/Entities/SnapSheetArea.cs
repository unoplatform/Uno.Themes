using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Uno.Material.Entities
{
	/// <summary>
	/// This represents a snap area of a <see cref="StandardBottomSheet"/>.
	/// </summary>
	public class SheetSnapArea
	{
		/// <summary>
		/// The name of the snap area
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The snap type of this area.
		/// </summary>
		public SnapType SnapType { get; set; }

		/// <summary>
		/// The size of the area.
		/// Note that only absolute and start sizes are supported. Auto is not supported.
		/// </summary>
		public GridLength Height { get; set; }
	}

	public enum SnapType
	{
		/// <summary>
		/// The sheet will not snap when released in an area of this type.
		/// </summary>
		None,

		/// <summary>
		/// The sheet will snap aligning the top of the handle with the top of the snap area when released in an area of this type.
		/// </summary>
		Top,

		/// <summary>
		/// The sheet will snap aligning the bottom of the handle with the bottom of the snap area when released in an area of this type.
		/// </summary>
		Bottom,
	}
}
