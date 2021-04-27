using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Uno.Material.Entities;
using Uno.UI.Toolkit;
using Windows.ApplicationModel.Store;
using Windows.Foundation;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
#endif

namespace Uno.Material.Controls
{
	public partial class StandardBottomSheet : Control
	{
#region PointerSubscriptions
		private void SubscribeToPointerEvents()
		{
			if (_header != null)
			{
				_header.PointerPressed -= OnHeaderPressed;
				_header.PointerPressed += OnHeaderPressed;

				_content.PointerPressed -= OnHeaderPressed;
				_content.PointerPressed += OnHeaderPressed;

#if __IOS__ && __WASM__
				// Workaround for #404 - Gestures don't work properly so we need to subscribe to the _header's events too.
				_header.PointerMoved -= OnHeaderMoved;
				_header.PointerMoved += OnHeaderMoved;

				_header.PointerReleased -= OnHeaderReleased;
				_header.PointerReleased += OnHeaderReleased;
#endif
				PointerMoved -= OnHeaderMoved;
				PointerMoved += OnHeaderMoved;
				PointerReleased -= OnHeaderReleased;
				PointerReleased += OnHeaderReleased;
				// If the pointer exits, treat it as release.
				PointerExited -= OnHeaderReleased;
				PointerExited += OnHeaderReleased;
			}
		}

		private void UnsubscribeFromPointerEvents()
		{
			if (_header != null)
			{
				_header.PointerPressed -= OnHeaderPressed;
			}

			PointerMoved -= OnHeaderMoved;
			PointerReleased -= OnHeaderReleased;
			PointerExited -= OnHeaderReleased;
		}

		private void OnHeaderPressed(object sender, PointerRoutedEventArgs e)
		{
			_state = SheetState.Grabbed;
			_sheetYWhenGrabbed = _transform.Y;
			_lastY = _sheetYWhenGrabbed;
			_pointerYWhenGrabbed = e.GetCurrentPoint(this).Position.Y;
			_grabbedTimer.Start();

#if !__IOS__
			// Workaround for #404 - We don't do this on iOS because it breaks other pointer events afterwards.
			e.Handled = true;
#endif
		}

		private void OnHeaderMoved(object sender, PointerRoutedEventArgs e)
		{
			if (_state == SheetState.Grabbed)
			{
				var currentPointer = e.GetCurrentPoint(this);
				var absoluteDeltaY = currentPointer.Position.Y - _pointerYWhenGrabbed;
				var deltaY = currentPointer.Position.Y - _lastY;
				_lastY = currentPointer.Position.Y;
				SetSheetPosition(ClampValue(_sheetYWhenGrabbed + absoluteDeltaY, min: 0, max: MaxY));

				// Record the last movements to calculate the speed when releasing.
				_lastMoves.AddLast(new MoveUpdate()
				{
					DeltaY = deltaY,
					DeltaT = _grabbedTimer.Elapsed.TotalSeconds
				});
				_grabbedTimer.Restart();
				if (_lastMoves.Count > MaximumRetainedMovementCount)
				{
					// Only keep the last N movements.
					_lastMoves.RemoveFirst();
				}
#if __IOS__
				if (sender == this)
				{
					// Workaround for #404 - Since we subscribe on both this and _handle on iOS, we set Handled only the parent.
					e.Handled = true;
				}
#else
				e.Handled = true;
#endif
			}
		}

		/// <summary>
		/// This method exists only to workaround an iOS issue.
		/// </summary>
		public void ForceRelease()
		{
			OnHeaderReleased(sender: null, e: null);
		}

		private async void OnHeaderReleased(object sender, PointerRoutedEventArgs e)
		{
			try
			{
				if (_state == SheetState.Grabbed)
				{
#if __IOS__
					if (sender == this)
					{
						// Workaround for #404 - Since we subscribe on both this and _handle on iOS, we set Handled only the parent.
						e.Handled = true;
					}
#else
					if (e != null)
					{
						e.Handled = true;
					}
#endif

					_state = SheetState.Snapping;
					if (_lastMoves.Count == MaximumRetainedMovementCount)
					{
						const double durationThreshold = 0.3; // in seconds

						// When released at high speed, the sheet slides to either the top or bottom edge.
						// We calculate the speed and distance to decide whether to snap to an edge.
						var speed = _lastMoves.Average(m => m.DeltaY / m.DeltaT);
						var distanceToEdge = speed < 0 ? _transform.Y : (MaxY - _transform.Y);
						var durationToEdge = distanceToEdge / Math.Abs(speed);
						if (durationToEdge <= durationThreshold)
						{
							// If the Sheet would reach the edge within the durationThresholdwith its current speed, we let it.
							await SnapToEdge(speed, durationToEdge);
						}
						else
						{
							await SnapToPotentialSnapArea();
						}
					}
					else
					{
						await SnapToPotentialSnapArea();
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				// Remove the background to allow pointer events to pass through the DrawerView.
				_root.Background = null;
				_state = SheetState.Normal;
				_grabbedTimer.Stop();
			}

			async Task SnapToEdge(double speed, double duration)
			{
				const double durationThreshold = 0.05; // in seconds

				var finalPosition = speed < 0 ? 0 : MaxY;
				if (duration > durationThreshold)
				{
					// If the animation would not be too fast, we animate the value.
					await AnimateTo(finalPosition, duration);
				}
				else
				{
					// If the animation would be too fast, we just set the value instead.
					SetSheetPosition(finalPosition);
				}

				// Make sure we set this.CurrentSnapArea if possible. 
				var snapArea = GetSnapArea(finalPosition, out _, out _);
				if (snapArea != null)
				{
					CurrentSnapArea = snapArea;
				}
			}
		}
#endregion

#region SizeSubscriptions
		private void SubscribeToSizeChangedEvents()
		{
			if (_header != null)
			{
				_header.SizeChanged -= OnComponentSizeChanged;
				_header.SizeChanged += OnComponentSizeChanged;
			}
		}

		private void UnsubscribeFromSizeChangedEvents()
		{
			if (_header != null)
			{
				_header.SizeChanged -= OnComponentSizeChanged;
			}
		}

		private void OnComponentSizeChanged(object sender, SizeChangedEventArgs e)
		{
			ComponentSizeChanged?.Invoke();
		}

		/// <summary>
		/// This event is raised when the view inside the <see cref="Header"/> changed size.
		/// </summary>
		public event Action ComponentSizeChanged;
#endregion
	}
}
