using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Uno.Material.Entities;
using Uno.Material.Extensions;
using Uno.UI.Toolkit;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Uno.Material.Controls
{
	[TemplatePart(Name = RootPartName, Type = typeof(Grid))]
	[TemplatePart(Name = BackdropPartName, Type = typeof(Grid))]
	[TemplatePart(Name = SheetPartName, Type = typeof(ElevatedView))]
	[TemplatePart(Name = HeaderPartName, Type = typeof(Grid))]
	[TemplatePart(Name = FullScreenHeaderPartName, Type = typeof(ElevatedView))]
	[TemplatePart(Name = HeaderPresenterPartName, Type = typeof(ContentPresenter))]
	[TemplatePart(Name = ContentPresenterPartName, Type = typeof(ContentPresenter))]
	[TemplatePart(Name = CloseFullScreenButtonPartName, Type = typeof(Button))]
	public partial class StandardBottomSheet : Control
	{
		public const string RootPartName = "Part_Root";
		public const string BackdropPartName = "Part_Backdrop";
		public const string SheetPartName = "Part_Sheet";
		public const string HeaderPartName = "Part_Header"; // The header is the part that can be "grabbed" to move the sheet.
		public const string FullScreenHeaderPartName = "Part_FullScreenHeader";
		public const string HeaderPresenterPartName = "Part_HeaderPresenter";
		public const string ContentPresenterPartName = "Part_ContentPresenter"; // The content is the part that display the content.
		public const string CloseFullScreenButtonPartName = "Part_CloseFullScreenButton";
		public const int MaximumRetainedMovementCount = 3;

		protected Grid _root;
		protected Grid _backdrop;
		protected ElevatedView _sheet;
		protected Grid _header;
		protected ElevatedView _fullScreenHeader;
		protected ContentPresenter _headerPresenter;
		protected ContentPresenter _content;
		protected Button _closeFullScreenButton;
		protected TranslateTransform _transform;

		protected SheetSnapArea _defaultTopSnapPoint = new SheetSnapArea()
		{
			Name = "DefaultTopSnapPoint",
			SnapType = SnapType.Top,
			Height = new GridLength(0.4, GridUnitType.Star)
		};
		protected SheetSnapArea _defaultCenterSnapPoint = new SheetSnapArea()
		{
			Name = "DefaultCenterSnapPoint",
			SnapType = SnapType.Top,
			Height = new GridLength(0.3, GridUnitType.Star)
		};
		protected SheetSnapArea _defaultBottomSnapPoint = new SheetSnapArea()
		{
			Name = "DefaultBottomSnapPoint",
			SnapType = SnapType.Bottom,
			Height = new GridLength(0.3, GridUnitType.Star)
		};

		private readonly LinkedList<MoveUpdate> _lastMoves = new LinkedList<MoveUpdate>(); // Used to calculate the sheet's speed when releasing.
		private readonly Stopwatch _grabbedTimer = new Stopwatch(); // Used to calculate the sheet's speed when releasing.
		protected readonly double _animationTime = 0.25;

		private SheetState _state;
		private double _pointerYWhenGrabbed; // Used to calculate the delta position when moving.
		private double _sheetYWhenGrabbed; // Used to calculate the drawer's position with the pointer's delta position (because we don't set the drawer's position to the pointer's position directly).
		private double _lastY; // Used to update the _lastMoves.
		private bool _isInitialSnapAreaSet;

		private struct MoveUpdate
		{
			public double DeltaY { get; set; }
			public double DeltaT { get; set; }
		}

		public enum SheetState
		{
			Normal,
			Grabbed,
			Snapping
		}

		public StandardBottomSheet()
		{
			DefaultStyleKey = typeof(StandardBottomSheet);

			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
#if !__IOS__
			// Workaround for #405 - iOS ActualHeight of inner elemens not known on creation
			SizeChanged += OnSizeChanged;
#endif
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			SubscribeToPointerEvents();
			SubscribeToSizeChangedEvents();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			UnsubscribeFromPointerEvents();
			UnsubscribeFromSizeChangedEvents();
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

			base.OnApplyTemplate();

			_root = (Grid)GetTemplateChild(RootPartName);
			_backdrop = (Grid)GetTemplateChild(BackdropPartName);
			_sheet = (ElevatedView)GetTemplateChild(SheetPartName);
			_header = (Grid)GetTemplateChild(HeaderPartName);
			_fullScreenHeader = (ElevatedView)GetTemplateChild(FullScreenHeaderPartName);
			_headerPresenter = (ContentPresenter)GetTemplateChild(ContentPresenterPartName);
			_content = (ContentPresenter)GetTemplateChild(ContentPresenterPartName);
			_closeFullScreenButton = (Button)GetTemplateChild(CloseFullScreenButtonPartName);

#if __IOS__
			// Workaround for #405 - iOS ActualHeight of inner elemens not known on creation
			if (_header != null)
			{
				_header.SizeChanged += OnSizeChanged;
			}
#endif

			if (_closeFullScreenButton != null)
			{
				_closeFullScreenButton.Click += CloseButton_Clicked;
			}

				_transform = _sheet.RenderTransform as TranslateTransform;
			if (_transform == null)
			{
				// Make sure we have a TranslateTransform since we absolutely need one.
				_sheet.RenderTransform = _transform = new TranslateTransform()
				{
					Y = 0
				};
			}

			SubscribeToPointerEvents();
			SubscribeToSizeChangedEvents();
		}

		private void CloseButton_Clicked(object sender, RoutedEventArgs e)
		{
			CloseBottomSheet();
		}

		/// <summary>
		/// This event is raised when the sheet is moved from moved events (when grabbed).
		/// The event parameter is the new sheet position.
		/// </summary>
		public event Action<double> SheetMoved;

		/// <summary>
		/// This event is raised when the sheet is being animated to a new position (when snapping).
		/// The event parameter is the new sheet position.
		/// </summary>
		public event Action<double> SheetAnimating;

		public async Task SnapToPotentialSnapArea()
		{
			var snapArea = GetSnapArea(_transform.Y, out var snapTop, out var snapBottom);
			if (snapArea != null)
			{
				await SnapTo(snapArea, snapTop, snapBottom);
			}
		}

		private async Task SnapTo(SheetSnapArea snapArea, double snapTop, double snapBottom)
		{
			CurrentSnapArea = snapArea;

			switch (snapArea.SnapType)
			{
				case SnapType.Top:
					await SnapTo(snapTop);
					break;
				case SnapType.Bottom:
					await SnapTo(snapBottom - _header.ActualHeight);
					break;
				case SnapType.None:
					break;
				default:
					throw new NotSupportedException($"The snap type {snapArea.SnapType} is not supported.");
			}
		}

		public async Task SnapTo(double y)
		{
			if (_transform == null)
			{
				// Don't do anything if this public method was called while the control is initiating.
				return;
			}

			const double shortDistanceThreshold = 20; // int pixels
			const double nearDistanceThreshold = 60; // in pixels
			const double fastDuration = 0.1; // in seconds
			const double normalDuration = 0.2; // in seconds

			var distance = Math.Abs(_transform.Y - y);
			if (distance < shortDistanceThreshold)
			{
				// If the distance is too short, we don't animate.
				SetSheetPosition(y);
			}
			else if (distance < nearDistanceThreshold)
			{
				// If the distance is relatively short we animate quickly.
				await AnimateTo(y, fastDuration);
			}
			else
			{
				await AnimateTo(y, normalDuration);
			}
		}

		protected async Task AnimateTo(double finalPosition, double duration)
		{
			SheetAnimating?.Invoke(finalPosition);

			var storyboard = new Storyboard();
			var animation = new DoubleAnimation()
			{
				To = finalPosition,
				Duration = new Duration(TimeSpan.FromSeconds(duration)),
			};
			Storyboard.SetTarget(animation, _transform);
			Storyboard.SetTargetProperty(animation, "Y");
			storyboard.Children.Add(animation);

			await storyboard.Run();
			SheetPosition = finalPosition;
		}

		private void SetSheetPosition(double y)
		{
			_transform.Y = y;
#if __IOS__ || __ANDROID__
			_transform.SetValue(TranslateTransform.YProperty, y, DependencyPropertyValuePrecedences.Animations);
#endif
			SheetPosition = y;
			SheetMoved?.Invoke(y);
		}

		protected async virtual void CloseBottomSheet()
		{
			await AnimateTo(_sheet.ActualHeight - _header.ActualHeight, _animationTime);
		}

		/// <summary>
		/// Restricts a value to defined mininum and maximum.
		/// </summary>
		public double ClampValue(double value, double min, double max)
		{
			if (value < min)
			{
				return min;
			}
			else if (value > max)
			{
				return max;
			}
			else
			{
				return value;
			}
		}
	}
}
