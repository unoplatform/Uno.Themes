using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Uno.Material.Controls
{
	[TemplateVisualState(GroupName = __openStateGroup, Name = __openedState)]
	[TemplateVisualState(GroupName = __openStateGroup, Name = __closedState)]
	public partial class Card : Control
	{
		private const string __openStateGroup = "OpenStates";
		private const string __openedState = "Opened";
		private const string __closedState = "Closed";

		#region Headear and SubHeader
		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(Card), new PropertyMetadata(string.Empty));

		public string SubHeader
		{
			get { return (string)GetValue(SubHeaderProperty); }
			set { SetValue(SubHeaderProperty, value); }
		}

		public static readonly DependencyProperty SubHeaderProperty =
			DependencyProperty.Register("SubHeader", typeof(string), typeof(Card), new PropertyMetadata(string.Empty));
		#endregion

		#region AvatarContent and AvatarContentTemplate
		public object AvatarContent
		{
			get { return (object)GetValue(AvatarContentProperty); }
			set { SetValue(AvatarContentProperty, value); }
		}

		public static readonly DependencyProperty AvatarContentProperty =
			DependencyProperty.Register("AvatarContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate AvatarContentTemplate
		{
			get { return (DataTemplate)GetValue(AvatarContentTemplateProperty); }
			set { SetValue(AvatarContentTemplateProperty, value); }
		}
		public static readonly DependencyProperty AvatarContentTemplateProperty =
			DependencyProperty.Register("AvatarContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
		#endregion

		#region MediaContent and MediaContentTemplate
		public object MediaContent
		{
			get { return (object)GetValue(MediaContentProperty); }
			set { SetValue(MediaContentProperty, value); }
		}

		public static readonly DependencyProperty MediaContentProperty =
			DependencyProperty.Register("MediaContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate MediaContentTemplate
		{
			get { return (DataTemplate)GetValue(MediaContentTemplateProperty); }
			set { SetValue(MediaContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty MediaContentTemplateProperty =
			DependencyProperty.Register("MediaContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
		#endregion

		#region SupportingText
		public string SupportingText
		{
			get { return (string)GetValue(SupportingTextProperty); }
			set { SetValue(SupportingTextProperty, value); }
		}

		public static readonly DependencyProperty SupportingTextProperty =
			DependencyProperty.Register("SupportingText", typeof(string), typeof(Card), new PropertyMetadata(string.Empty));
		#endregion

		#region ButtonsItemsSource and IconsItemsSource
		public object ButtonsItemsSource
		{
			get { return (object)GetValue(ButtonsItemsSourceProperty); }
			set { SetValue(ButtonsItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty ButtonsItemsSourceProperty =
			DependencyProperty.Register("ButtonsItemsSource", typeof(object), typeof(Card), new PropertyMetadata(null));

		public object IconsItemsSource
		{
			get { return (object)GetValue(IconsItemsSourceProperty); }
			set { SetValue(IconsItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty IconsItemsSourceProperty =
			DependencyProperty.Register("IconsItemsSource", typeof(object), typeof(Card), new PropertyMetadata(null));
		#endregion

		#region ExpanderHeaderContent, ExpanderContent and ExpanderContentTemplate
		public object ExpanderHeaderContent
		{
			get { return (object)GetValue(ExpanderHeaderContentProperty); }
			set { SetValue(ExpanderHeaderContentProperty, value); }
		}

		public static readonly DependencyProperty ExpanderHeaderContentProperty =
			DependencyProperty.Register("ExpanderHeaderContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public object ExpanderContent
		{
			get { return (object)GetValue(ExpanderContentProperty); }
			set { SetValue(ExpanderContentProperty, value); }
		}

		public static readonly DependencyProperty ExpanderContentProperty =
			DependencyProperty.Register("ExpanderContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate ExpanderContentTemplate
		{
			get { return (DataTemplate)GetValue(ExpanderContentTemplateProperty); }
			set { SetValue(ExpanderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ExpanderContentTemplateProperty =
			DependencyProperty.Register("ExpanderContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
		#endregion

		#region IsOpened
		public bool IsOpened
		{
			get { return (bool)GetValue(IsOpenedProperty); }
			set { SetValue(IsOpenedProperty, value); }
		}

		public static readonly DependencyProperty IsOpenedProperty =
			DependencyProperty.Register("IsOpened", typeof(bool), typeof(Card), new PropertyMetadata(false, OnIsOpenedChanged));

		private static void OnIsOpenedChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var card = d as Card;
			card?.UpdateOpenVisualStates(true);
		}
		#endregion

		#region CloseWhenUnloaded
		public bool CloseWhenUnloaded
		{
			get { return (bool)GetValue(CloseWhenUnloadedProperty); }
			set { SetValue(CloseWhenUnloadedProperty, value); }
		}

		public static readonly DependencyProperty CloseWhenUnloadedProperty =
			DependencyProperty.Register("CloseWhenUnloaded", typeof(bool), typeof(Card), new PropertyMetadata(false));
		#endregion

		#region PresenterHeight
		public double PresenterHeight
		{
			get { return (double)GetValue(PresenterHeightProperty); }
			set { SetValue(PresenterHeightProperty, value); }
		}

		public static readonly DependencyProperty PresenterHeightProperty =
			DependencyProperty.Register("PresenterHeight", typeof(double), typeof(Card), new PropertyMetadata(0d, OnPresenterHeightChanged));

		private static void OnPresenterHeightChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var card = d as Card;
			card?.UpdateOpenVisualStates(true);
			card?.UpdatePresenterHeight(0);
		}
		#endregion

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
				VisualStateManager.GoToState(this, __openedState, useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, __closedState, useTransitions);
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
