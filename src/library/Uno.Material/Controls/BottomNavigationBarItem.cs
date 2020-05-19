using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

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

		public BottomNavigationBarItem()
		{
			DefaultStyleKey = typeof(BottomNavigationBarItem);
		}
	}
}
