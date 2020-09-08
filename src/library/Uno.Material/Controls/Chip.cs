using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Uno.Material.Controls
{
	public partial class Chip : ToggleButton
	{
		public object Thumbnail
		{
			get { return (object)GetValue(ThumbnailProperty); }
			set { SetValue(ThumbnailProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailProperty =
			DependencyProperty.Register("Thumbnail", typeof(object), typeof(Chip), new PropertyMetadata(null));

		public DataTemplate ThumbnailTemplate
		{
			get { return (DataTemplate)GetValue(ThumbnailTemplateProperty); }
			set { SetValue(ThumbnailTemplateProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailTemplateProperty =
			DependencyProperty.Register("ThumbnailTemplate", typeof(DataTemplate), typeof(Chip), new PropertyMetadata(null));

		public bool CanRemove
		{
			get { return (bool)GetValue(CanRemoveProperty); }
			set { SetValue(CanRemoveProperty, value); }
		}

		public static readonly DependencyProperty CanRemoveProperty =
			DependencyProperty.Register("CanRemove", typeof(bool), typeof(Chip), new PropertyMetadata(false));
	}
}
