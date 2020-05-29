using Uno.Material.Entities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Uno.Material.Controls
{
	public partial class BottomNavigationBarItem : ToggleButton
	{
		public string Header
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register(
				nameof(Header),
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
				"Icon",
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
				"Badge",
				typeof(BottomNavigationBarBadge),
				typeof(BottomNavigationBarItem),
				new PropertyMetadata(null));

		public BottomNavigationBarItem()
		{
			DefaultStyleKey = typeof(BottomNavigationBarItem);
		}
	}
}
