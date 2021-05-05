using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uno.Disposables;
using Uno.Material.Entities;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material.Controls
{
	[TemplateVisualState(GroupName = __sheetStateGroup, Name = __closedState)]
	[TemplateVisualState(GroupName = __sheetStateGroup, Name = __openedState)]
	public partial class ModalStandardBottomSheet : StandardBottomSheet
	{
		private const string __sheetStateGroup = "SheetStates";
		private const string __closedState = "Closed";
		private const string __openedState = "Opened";
		private int _almostClosed = 5;

		public ModalStandardBottomSheet()
		{
			DefaultStyleKey = typeof(ModalStandardBottomSheet);

			SnapAreas = new SheetSnapAreaCollection()
			{
				_defaultTopSnapPoint,
				_defaultCenterSnapPoint,
				_defaultBottomSnapPoint
			};

			InitialSnapAreaName = _defaultBottomSnapPoint.Name;

			(this as DependencyObject).RegisterPropertyChangedCallback(ModalStandardBottomSheet.CurrentSnapAreaProperty, (s, e) => OnCurrentSnapAreaChanged(s, e));
			(this as DependencyObject).RegisterPropertyChangedCallback(ModalStandardBottomSheet.SheetPositionProperty, (s, e) => OnPositionChanged(s, e));
#if __IOS__
			// Workaround for #405 - iOS ActualHeight of inner elemens not known on creation
			SizeChanged += OnSizeChanged;
#endif
		}

		private static void OnCurrentSnapAreaChanged(object d, DependencyProperty e)
		{
			var sheet = d as ModalStandardBottomSheet;

			if (sheet.CurrentSnapArea.Name.Equals(sheet._defaultBottomSnapPoint.Name))
			{
				sheet.IsOpened = false;
			}
			else
			{
				sheet.IsOpened = true;
			}
		}

		private static void OnPositionChanged(object d, DependencyProperty e)
		{
			var sheet = d as ModalStandardBottomSheet;

			if (sheet.SheetPosition > sheet.ActualHeight - sheet._almostClosed)
			{
				//sheet.CurrentSnapArea = sheet._defaultBottomSnapPoint;
				sheet.IsOpened = false;
			}
		}

		protected override void OnApplyTemplate()
		{
			if (IsEnabled)
			{
				VisualStateManager.GoToState(this, "Normal", true);
			}
			else
			{
				VisualStateManager.GoToState(this, "Disabled", true);
			}

			UpdateVisualStates(true);

			base.OnApplyTemplate();
			_backdrop.PointerPressed += ToggleSheet;
		}

		private void UpdateVisualStates(bool useTransitions)
		{
			if (IsOpened)
			{
				VisualStateManager.GoToState(this, __openedState, useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, __closedState, useTransitions);
			}
		}

		private void ToggleSheet(object d, RoutedEventArgs e)
		{
			CloseBottomSheet();
		}

		protected async override void CloseBottomSheet()
		{
			await AnimateTo(_sheet.ActualHeight, _animationTime);
			IsOpened = false;
		}

#region IsOpened
		public bool IsOpened
		{
			get { return (bool)GetValue(IsOpenedProperty); }
			set { SetValue(IsOpenedProperty, value); }
		}

		public static readonly DependencyProperty IsOpenedProperty =
			DependencyProperty.Register("IsOpened", typeof(bool), typeof(ModalStandardBottomSheet), new PropertyMetadata(false, OnIsOpenedChanged));

		private static async void OnIsOpenedChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var sheet = d as ModalStandardBottomSheet;
			sheet?.UpdateVisualStates(true);

			if ((!(e.OldValue as bool?) ?? false) && ((e.NewValue as bool?) ?? false))
			{
				var area = sheet.SnapAreas.First(c => c.Name.Equals(sheet._defaultCenterSnapPoint.Name));
				var size = sheet.GetAreaSize(area);
				var position = area.SnapType == SnapType.Bottom ? size.bottom: size.top;

				if (position == 0d)
				{
					sheet.InitialSnapAreaName = sheet._defaultCenterSnapPoint.Name;
				}
				else
				{
					await sheet.AnimateTo(position, sheet._animationTime);
				}
			}
		}
#endregion
	}
}
