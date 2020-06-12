using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Uno.Material.Controls
{
	[TemplateVisualState(GroupName = OpenStateGroup, Name = OpenedState)]
	[TemplateVisualState(GroupName = OpenStateGroup, Name = ClosedState)]
	public partial class Card : Control
	{
		private const string OpenStateGroup = "OpenStates";
		private const string OpenedState = "Opened";
		private const string ClosedState = "Closed";

		public Card()
		{
			DefaultStyleKey = typeof(Card);

			Loaded += (s, e) => { UpdateOpenVisualStates(false); };
			Unloaded += TryCloseOnUnloaded;
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
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			var size = base.MeasureOverride(availableSize);

			UpdatePresenterHeight((float)availableSize.Width);
			return size;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			return base.ArrangeOverride(finalSize);
		}

		protected void UpdatePresenterHeight(double availableWidth)
		{
			//No need to recalculate the Height of child
			if (PresenterHeight == 0)
			{
				var element = this.ExpanderContent as FrameworkElement;

				if (element == null)
				{
					var presenter = new ContentPresenter() { Content = this.ExpanderContent, ContentTemplate = this.ExpanderContentTemplate };

					presenter.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
					PresenterHeight = presenter.DesiredSize.Height;
				}
				else
				{
					element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
					PresenterHeight = element.DesiredSize.Height;
				}
			}
		}

		private void UpdateOpenVisualStates(bool useTransitions)
		{
			if (IsOpened)
			{
				VisualStateManager.GoToState(this, OpenedState, useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, ClosedState, useTransitions);
			}
		}

		private void TryCloseOnUnloaded(object sender, RoutedEventArgs e)
		{
			if (CloseWhenUnloaded)
			{
				IsOpened = false;
			}
		}

		protected override void OnPointerEntered(PointerRoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "PointerOver", true);

			base.OnPointerEntered(e);
		}

		protected override void OnPointerExited(PointerRoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Normal", true);

			base.OnPointerExited(e);
		}

		protected override void OnPointerPressed(PointerRoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Pressed", true);

			base.OnPointerPressed(e);
		}

		protected override void OnPointerReleased(PointerRoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Normal", true);

			base.OnPointerReleased(e);
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Focused", true);
			VisualStateManager.GoToState(this, "PointerFocused", true);

			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Unfocused", true);

			base.OnLostFocus(e);
		}
	}
}
