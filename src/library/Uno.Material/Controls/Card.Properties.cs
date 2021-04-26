using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Material.Controls
{
	public partial class Card : Control
	{
#if __ANDROID__
		private static readonly Windows.UI.Color _defaultShadowColor = Colors.Black;
#else
		private static readonly Windows.UI.Color _defaultShadowColor = Windows.UI.Color.FromArgb(64, 0, 0, 0);
#endif

#region HeaderContent and HeaderContentTemplate
		public object HeaderContent
		{
			get { return (object)GetValue(HeaderContentProperty); }
			set { SetValue(HeaderContentProperty, value); }
		}

		public static readonly DependencyProperty HeaderContentProperty =
			DependencyProperty.Register("HeaderContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate HeaderContentTemplate
		{
			get { return (DataTemplate)GetValue(HeaderContentTemplateProperty); }
			set { SetValue(HeaderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty HeaderContentTemplateProperty =
			DependencyProperty.Register("HeaderContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
#endregion

#region SubHeaderContent and SubHeaderContentTemplate
		public object SubHeaderContent
		{
			get { return (object)GetValue(SubHeaderContentProperty); }
			set { SetValue(SubHeaderContentProperty, value); }
		}

		public static readonly DependencyProperty SubHeaderContentProperty =
			DependencyProperty.Register("SubHeaderContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate SubHeaderContentTemplate
		{
			get { return (DataTemplate)GetValue(SubHeaderContentTemplateProperty); }
			set { SetValue(SubHeaderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty SubHeaderContentTemplateProperty =
			DependencyProperty.Register("SubHeaderContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
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

#region SupportingContent and SupportingContentTemplate
		public object SupportingContent
		{
			get { return (object)GetValue(SupportingContentProperty); }
			set { SetValue(SupportingContentProperty, value); }
		}

		public static readonly DependencyProperty SupportingContentProperty =
			DependencyProperty.Register("SupportingContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate SupportingContentTemplate
		{
			get { return (DataTemplate)GetValue(SupportingContentTemplateProperty); }
			set { SetValue(SupportingContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty SupportingContentTemplateProperty =
			DependencyProperty.Register("SupportingContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
#endregion

#region IconsContent and IconsContentTemplate
		public object IconsContent
		{
			get { return (object)GetValue(IconsContentProperty); }
			set { SetValue(IconsContentProperty, value); }
		}

		public static readonly DependencyProperty IconsContentProperty =
			DependencyProperty.Register("IconsContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate IconsContentTemplate
		{
			get { return (DataTemplate)GetValue(IconsContentTemplateProperty); }
			set { SetValue(IconsContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty IconsContentTemplateProperty =
			DependencyProperty.Register("IconsContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
#endregion

#region Elevation
		public double Elevation
		{
			get { return (double)GetValue(ElevationProperty); }
			set { SetValue(ElevationProperty, value); }
		}

		public static readonly DependencyProperty ElevationProperty =
			DependencyProperty.Register("Elevation", typeof(double), typeof(Card), new PropertyMetadata(0));
#endregion

#region ShadowColor
		public Windows.UI.Color ShadowColor
		{
			get { return (Windows.UI.Color)GetValue(ShadowColorProperty); }
			set { SetValue(ShadowColorProperty, value); }
		}

		public static readonly DependencyProperty ShadowColorProperty =
			DependencyProperty.Register("ShadowColor", typeof(Windows.UI.Color), typeof(Card), new PropertyMetadata(_defaultShadowColor));
#endregion
	}
}
