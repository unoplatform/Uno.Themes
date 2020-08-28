using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Uno.Material.Entities;
using Uno.UI.Toolkit;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Uno.Material.Controls
{
	public partial class StandardBottomSheet : Control
	{
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Height > 0 && e.NewSize.Width > 0 && !_isInitialSnapAreaSet)
			{
				SetPositionFromInitialSnapArea();
#if __IOS__
				_header.SizeChanged -= OnSizeChanged;
#else
				SizeChanged -= OnSizeChanged;
#endif
			}

			void SetPositionFromInitialSnapArea()
			{
				var hasSnapAreas = SnapAreas.Any();

				if (hasSnapAreas)
				{
					var area = SnapAreas.Single(a => a.Name == InitialSnapAreaName);
					var size = GetAreaSize(area);
					var position = area.SnapType == SnapType.Bottom ? (size.bottom - _header.ActualHeight) : size.top;

					// Set the position and CurrentSnapArea
					SetSheetPosition(position);
					CurrentSnapArea = area;
				}
				else
				{
					// Sets initial the position as the header at the bottom of the page
					SetSheetPosition(_sheet.ActualHeight - _header.ActualHeight);
				}

				// Make sure this method isn't called again.
				_isInitialSnapAreaSet = true;
			}
		}

		private bool TryComputeAreaSizeComponents(SheetSnapAreaCollection areas, out double totalStarSize, out double starDenominator)
		{
			if (areas.Empty())
			{
				totalStarSize = starDenominator = 0;
				return false;
			}
			else
			{
				var totalSize = ActualHeight;

				// This is the space occupied by all absolute-sized areas.
				var totalAbsoluteSize = areas
					.Where(a => a.Height.IsAbsolute)
					.Sum(a => a.Height.Value);

				// This is the space occupied by all star-sized areas.
				totalStarSize = totalSize - totalAbsoluteSize;

				// this is used to calculate the height of star-sized areas.
				starDenominator = areas
					.Where(a => a.Height.IsStar)
					.Sum(a => a.Height.Value);

				return true;
			}
		}

		/// <summary>
		/// Gets the area (if any) in which the <paramref name="releasePositionY"/> is found.
		/// This is used for snapping.
		/// </summary>
		/// <param name="releasePositionY">The position at which we want to find the surrounding area.</param>
		/// <param name="snapTop">Outputs the top of the area.</param>
		/// <param name="snapBottom">Outputs the bottom of the area</param>
		private SheetSnapArea GetSnapArea(double releasePositionY, out double snapTop, out double snapBottom)
		{
			snapTop = snapBottom = 0d;
			var areas = SnapAreas;
			if (TryComputeAreaSizeComponents(areas, out var totalStarSize, out var starDenominator))
			{
				var distances = GetDistances();
				var minDistance = distances.Min(tuple => tuple.distance);
				var result = distances.Single(tuple => tuple.distance == minDistance);
				// We take the nearest snap area, then snap to it.

				snapTop = result.top;
				snapBottom = result.bottom;
				return result.area;
			}
			else
			{
				// If there are no areas defined, return null.
				return null;
			}

			List<(SheetSnapArea area, double top, double bottom, double distance)> GetDistances()
			{
				var bottom = 0d;
				var top = 0d;
				var distances = new List<(SheetSnapArea area, double top, double bottom, double distance)>();

				foreach (var area in areas)
				{
					// Calculate the height of the area based on its type (absolute or star).
					var height = area.Height.IsAbsolute
						? area.Height.Value
						: area.Height.Value / starDenominator * totalStarSize;

					bottom = top + height;
					var distance = double.PositiveInfinity;

					switch (area.SnapType)
					{
						case SnapType.Top:
							distance = Math.Abs(releasePositionY - top);
							break;
						case SnapType.Bottom:
							distance = Math.Abs(releasePositionY - bottom + _header.ActualHeight);
							break;
						case SnapType.None:
							distance = Math.Min(
								Math.Abs(releasePositionY - top),
								Math.Abs(releasePositionY - bottom + _header.ActualHeight)
							);
							break;
						default:
							throw new NotSupportedException($"The snap type {area.SnapType} is not supported.");
					}

					distances.Add((area, top, bottom, distance));

					top = bottom;
				}

				return distances;
			}
		}

		public (double top, double bottom) GetAreaSize(SheetSnapArea targetArea)
		{
			var areas = SnapAreas;
			TryComputeAreaSizeComponents(areas, out var totalStarSize, out var starDenominator);

			var top = 0d;
			var bottom = 0d;
			foreach (var area in areas)
			{
				// Calculate the height of the area based on its type (absolute or star).
				var height = area.Height.IsAbsolute
					? area.Height.Value
					: area.Height.Value / starDenominator * totalStarSize;

				bottom = top + height;
				if (targetArea == area)
				{
					return (top, bottom);
				}
				else
				{
					top = bottom;
				}
			}

			throw new ArithmeticException($"Failed to get area size for area {targetArea.Name}");
		}
	}
}
