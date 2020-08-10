using Windows.Foundation;
using Uno.Material.Entities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Uno.Material.Controls
{
	public partial class BottomNavigationBarItem : ToggleButton
	{
		public string Label
		{
			get => (string)GetValue(LabelProperty);
			set => SetValue(LabelProperty, value);
		}

		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register(
				nameof(Label),
				typeof(string),
				typeof(BottomNavigationBarItem),
				new PropertyMetadata(string.Empty));

		public IconElement Icon
		{
			get => (IconElement)GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}

		public static readonly DependencyProperty IconProperty =
			DependencyProperty.Register(
				nameof(Icon),
				typeof(IconElement),
				typeof(BottomNavigationBarItem),
				new PropertyMetadata(null));

		public BottomNavigationBarBadge Badge
		{
			get => (BottomNavigationBarBadge)GetValue(BadgeProperty);
			set => SetValue(BadgeProperty, value);
		}

		public static readonly DependencyProperty BadgeProperty =
			DependencyProperty.Register(
				nameof(Badge),
				typeof(BottomNavigationBarBadge),
				typeof(BottomNavigationBarItem),
				new PropertyMetadata(null));

		public BottomNavigationBarItem()
		{
			DefaultStyleKey = typeof(BottomNavigationBarItem);
		}
	}
}
