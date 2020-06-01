using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Uno.Material.Entities
{
	public partial class BottomNavigationBarBadge : DependencyObject
	{
		public bool IsVisible
		{
			get => (bool)GetValue(IsVisibleProperty);
			set => SetValue(IsVisibleProperty, value);
		}

		public static readonly DependencyProperty IsVisibleProperty =
			DependencyProperty.Register(
				nameof(IsVisible),
				typeof(bool),
				typeof(BottomNavigationBarBadge),
				new PropertyMetadata(false));

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(
				nameof(Text),
				typeof(string),
				typeof(BottomNavigationBarBadge),
				new PropertyMetadata(string.Empty));

	}
}
