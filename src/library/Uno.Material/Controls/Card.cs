using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Uno.Material.Controls
{
	public partial class Card : Control
	{
		public Card()
		{
			DefaultStyleKey = typeof(Card);
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
