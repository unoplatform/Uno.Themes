using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
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
		/// <summary>
		/// Gets the maximum value of <see cref="StandardBottomSheet"/>.
		/// </summary>
		public double MaxY => ActualHeight - _header.ActualHeight;

		#region  HeaderContent, HeaderContentTemplate & FullScreenHeaderContentTemplate
		public object HeaderContent
		{
			get { return (object)GetValue(HeaderContentProperty); }
			set { SetValue(HeaderContentProperty, value); }
		}

		public static readonly DependencyProperty HeaderContentProperty =
			DependencyProperty.Register("HeaderContent", typeof(object), typeof(StandardBottomSheet), new PropertyMetadata(null));

		public DataTemplate HeaderContentTemplate
		{
			get { return (DataTemplate)GetValue(HeaderContentTemplateProperty); }
			set { SetValue(HeaderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty HeaderContentTemplateProperty =
			DependencyProperty.Register("HeaderContentTemplate", typeof(DataTemplate), typeof(StandardBottomSheet), new PropertyMetadata(null));

		public DataTemplate FullScreenHeaderContentTemplate
		{
			get { return (DataTemplate)GetValue(FullScreenHeaderContentTemplateProperty); }
			set { SetValue(FullScreenHeaderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty FullScreenHeaderContentTemplateProperty =
			DependencyProperty.Register("FullScreenHeaderContentTemplate", typeof(DataTemplate), typeof(StandardBottomSheet), new PropertyMetadata(null));
		#endregion

		#region Content & ContentTemplate
		public object Content
		{
			get { return (object)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(object), typeof(StandardBottomSheet), new PropertyMetadata(null));

		public DataTemplate ContentTemplate
		{
			get { return (DataTemplate)GetValue(ContentTemplateProperty); }
			set { SetValue(ContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ContentTemplateProperty =
			DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(StandardBottomSheet), new PropertyMetadata(null));
		#endregion

		public SheetSnapAreaCollection SnapAreas
		{
			get { return (SheetSnapAreaCollection)GetValue(SnapAreasProperty); }
			set { SetValue(SnapAreasProperty, value); }
		}

		public static readonly DependencyProperty SnapAreasProperty =
			DependencyProperty.Register("SnapAreas", typeof(SheetSnapAreaCollection), typeof(StandardBottomSheet), new PropertyMetadata(SheetSnapAreaCollection.Empty));

		public SheetSnapArea CurrentSnapArea
		{
			get { return (SheetSnapArea)GetValue(CurrentSnapAreaProperty); }
			set { SetValue(CurrentSnapAreaProperty, value); }
		}

		public static readonly DependencyProperty CurrentSnapAreaProperty =
			DependencyProperty.Register("CurrentSnapArea", typeof(SheetSnapArea), typeof(StandardBottomSheet), new PropertyMetadata(null));

		/// <summary>
		/// When setting this, the Drawer will be initialized at a specified snap area.
		/// Changing this value once the <see cref="StandardBottomSheet"/> is rendered doesn't do anything.
		/// </summary>
		public string InitialSnapAreaName
		{
			get { return (string)GetValue(InitialSnapAreaNameProperty); }
			set { SetValue(InitialSnapAreaNameProperty, value); }
		}

		public static readonly DependencyProperty InitialSnapAreaNameProperty =
			DependencyProperty.Register("InitialSnapAreaName", typeof(string), typeof(StandardBottomSheet), new PropertyMetadata(null));

		public double SheetPosition
		{
			get { return (double)GetValue(SheetPositionProperty); }
			set { SetValue(SheetPositionProperty, value); }
		}

		public static readonly DependencyProperty SheetPositionProperty =
			DependencyProperty.Register("SheetPosition", typeof(double), typeof(StandardBottomSheet), new PropertyMetadata(0d, OnSheetPositionChanged));

		private static void OnSheetPositionChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var sheet = d as StandardBottomSheet;

			if ((e.NewValue as double?) == 0 && sheet.FullScreenHeaderContentTemplate != null)
			{
				sheet._fullScreenHeader.Visibility = Visibility.Visible;
				sheet._headerPresenter.Visibility = Visibility.Collapsed;
			}
			else
			{
				sheet._headerPresenter.Visibility = Visibility.Visible;
				sheet._fullScreenHeader.Visibility = Visibility.Collapsed;
			}

			// Needed to maintain content in FullScreen mode
			sheet._content.Visibility = Visibility.Visible;

#if __IOS__
			// Workaround for ios content not clipping
			if ((e.NewValue as double?) == (sheet.ActualHeight - sheet._header.ActualHeight))
			{
				sheet._content.Visibility = Visibility.Collapsed;
			}
			else
			{
				sheet._content.Visibility = Visibility.Visible;
			}
#endif
		}
	}
}
