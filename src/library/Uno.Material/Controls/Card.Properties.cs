using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Material.Controls
{
	public partial class Card : Control
	{
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

		#region ExpanderHeaderContent and ExpanderHeaderContentTemplate
		public object ExpanderHeaderContent
		{
			get { return (object)GetValue(ExpanderHeaderContentProperty); }
			set { SetValue(ExpanderHeaderContentProperty, value); }
		}

		public static readonly DependencyProperty ExpanderHeaderContentProperty =
			DependencyProperty.Register("ExpanderHeaderContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate ExpanderHeaderContentTemplate
		{
			get { return (DataTemplate)GetValue(ExpanderHeaderContentTemplateProperty); }
			set { SetValue(ExpanderHeaderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ExpanderHeaderContentTemplateProperty =
			DependencyProperty.Register("ExpanderHeaderContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
		#endregion

		#region ExpanderContent and ExpanderContentTemplate
		public object ExpanderContent
		{
			get { return (object)GetValue(ExpanderContentProperty); }
			set { SetValue(ExpanderContentProperty, value); }
		}

		public static readonly DependencyProperty ExpanderContentProperty =
			DependencyProperty.Register("ExpanderContent", typeof(object), typeof(Card), new PropertyMetadata(null));

		public DataTemplate ExpanderContentTemplate
		{
			get { return (DataTemplate)GetValue(ExpanderContentTemplateProperty); }
			set { SetValue(ExpanderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ExpanderContentTemplateProperty =
			DependencyProperty.Register("ExpanderContentTemplate", typeof(DataTemplate), typeof(Card), new PropertyMetadata(null));
		#endregion

		#region IsOpened
		public bool IsOpened
		{
			get { return (bool)GetValue(IsOpenedProperty); }
			set { SetValue(IsOpenedProperty, value); }
		}

		public static readonly DependencyProperty IsOpenedProperty =
			DependencyProperty.Register("IsOpened", typeof(bool), typeof(Card), new PropertyMetadata(false, OnIsOpenedChanged));

		private static void OnIsOpenedChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var card = d as Card;
			card?.UpdateOpenVisualStates(true);
		}
		#endregion

		#region CloseWhenUnloaded
		public bool CloseWhenUnloaded
		{
			get { return (bool)GetValue(CloseWhenUnloadedProperty); }
			set { SetValue(CloseWhenUnloadedProperty, value); }
		}

		public static readonly DependencyProperty CloseWhenUnloadedProperty =
			DependencyProperty.Register("CloseWhenUnloaded", typeof(bool), typeof(Card), new PropertyMetadata(false));
		#endregion

		#region PresenterHeight
		public double PresenterHeight
		{
			get { return (double)GetValue(PresenterHeightProperty); }
			set { SetValue(PresenterHeightProperty, value); }
		}

		public static readonly DependencyProperty PresenterHeightProperty =
			DependencyProperty.Register("PresenterHeight", typeof(double), typeof(Card), new PropertyMetadata(0d, OnPresenterHeightChanged));

		private static void OnPresenterHeightChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var card = d as Card;
			card?.UpdateOpenVisualStates(true);
			card?.UpdatePresenterHeight(0);
		}
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
	}
}
