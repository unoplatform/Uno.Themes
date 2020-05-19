using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Uno.Material.Controls
{
	public partial class BottomNavigationBarItem : ToggleButton
	{
		public string Data
		{
			get => (string)GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register(
				"Data",
				typeof(string),
				typeof(BottomNavigationBarItem),
				new PropertyMetadata(string.Empty));

		public string Badge
		{
			get => (string)GetValue(BadgeProperty);
			set => SetValue(BadgeProperty, value);
		}

		public static readonly DependencyProperty BadgeProperty =
			DependencyProperty.Register(
				"Badge",
				typeof(string),
				typeof(BottomNavigationBarItem),
				new PropertyMetadata(string.Empty));

		public BottomNavigationBarItem()
		{
			DefaultStyleKey = typeof(BottomNavigationBarItem);
		}
	}
}
