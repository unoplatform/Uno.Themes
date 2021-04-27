using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Material.Controls
{
	public partial class Divider : Control
	{
		public string SubHeader
		{
			get { return (string)GetValue(SubHeaderProperty); }
			set { SetValue(SubHeaderProperty, value); }
		}

		public static readonly DependencyProperty SubHeaderProperty =
			DependencyProperty.Register("SubHeader", typeof(string), typeof(Divider), new PropertyMetadata(string.Empty));

		public Brush SubHeaderForeground
		{
			get { return (Brush)GetValue(SubHeaderForegroundProperty); }
			set { SetValue(SubHeaderForegroundProperty, value); }
		}

		public static readonly DependencyProperty SubHeaderForegroundProperty =
			DependencyProperty.Register("SubHeaderForeground", typeof(Brush), typeof(Divider), new PropertyMetadata(null));
	}
}
