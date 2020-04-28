using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Material.Controls
{
	public partial class Card : Control
	{
		public string HeaderText
		{
			get => (string)GetValue(HeaderTextProperty);
			set => SetValue(HeaderTextProperty, value);
		}

		public static readonly DependencyProperty HeaderTextProperty =
			DependencyProperty.Register(
				nameof(HeaderText),
				typeof(string),
				typeof(Card),
				new PropertyMetadata(string.Empty));

		public string SubHeaderText
		{
			get => (string)GetValue(SubHeaderTextProperty);
			set => SetValue(SubHeaderTextProperty, value);
		}

		public static readonly DependencyProperty SubHeaderTextProperty =
			DependencyProperty.Register(
				nameof(SubHeaderText),
				typeof(string),
				typeof(Card),
				new PropertyMetadata(string.Empty));

		public string SupportingText
		{
			get => (string)GetValue(SupportingTextProperty);
			set => SetValue(SupportingTextProperty, value);
		}

		public static readonly DependencyProperty SupportingTextProperty =
			DependencyProperty.Register(
				nameof(SupportingText),
				typeof(string),
				typeof(Card),
				new PropertyMetadata(string.Empty));

		public Card()
		{
			DefaultStyleKey = typeof(Card);
		}
	}
}