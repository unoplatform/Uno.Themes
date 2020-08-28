using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Uno.Material.Controls
{
	[TemplateVisualState(GroupName = __sheetStateGroup, Name = __collapsedState)]
	[TemplateVisualState(GroupName = __sheetStateGroup, Name = __expandedState)]
	public partial class ExpandingBottomSheet : Control
	{
		private const string __sheetStateGroup = "SheetStates";
		private const string __collapsedState = "Collapsed";
		private const string __expandedState = "Expanded";

		public ExpandingBottomSheet()
		{
			DefaultStyleKey = typeof(ExpandingBottomSheet);
		}

		protected override void OnApplyTemplate()
		{
			if (IsEnabled)
			{
				VisualStateManager.GoToState(this, "Normal", true);
			}
			else
			{
				VisualStateManager.GoToState(this, "Disabled", true);
			}

			base.OnApplyTemplate();
		}

		private void UpdateVisualStates(bool useTransitions)
		{
			if (IsExpanded)
			{
				VisualStateManager.GoToState(this, __expandedState, useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, __collapsedState, useTransitions);
			}
		}

		private void ToggleSheet(object d, RoutedEventArgs e)
		{
			IsExpanded = !IsExpanded;
		}

		#region IsExpanded
		public bool IsExpanded
		{
			get { return (bool)GetValue(IsExpandedProperty); }
			set { SetValue(IsExpandedProperty, value); }
		}

		public static readonly DependencyProperty IsExpandedProperty =
			DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ExpandingBottomSheet), new PropertyMetadata(false, OnIsExpandedChanged));

		private static void OnIsExpandedChanged(object d, DependencyPropertyChangedEventArgs e)
		{
			var sheet = d as ExpandingBottomSheet;
			sheet?.UpdateVisualStates(true);
		}
		#endregion

		#region CollapsedContent, CollapsedContentTemplate and MinimalCollapsedContentTemplate
		public object CollapsedContent
		{
			get { return (object)GetValue(CollapsedContentProperty); }
			set { SetValue(CollapsedContentProperty, value); }
		}

		public static readonly DependencyProperty CollapsedContentProperty =
			DependencyProperty.Register("CollapsedContent", typeof(object), typeof(ExpandingBottomSheet), new PropertyMetadata(null));

		public DataTemplate CollapsedContentTemplate
		{
			get { return (DataTemplate)GetValue(CollapsedTemplateContentProperty); }
			set { SetValue(CollapsedTemplateContentProperty, value); }
		}

		public static readonly DependencyProperty CollapsedTemplateContentProperty =
			DependencyProperty.Register("CollapsedContentTemplate", typeof(DataTemplate), typeof(ExpandingBottomSheet), new PropertyMetadata(null));

		public DataTemplate MinimalCollapsedContentTemplate
		{
			get { return (DataTemplate)GetValue(MinimalCollapsedContentTemplateProperty); }
			set { SetValue(MinimalCollapsedContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty MinimalCollapsedContentTemplateProperty =
			DependencyProperty.Register("MinimalCollapsedContentTemplate", typeof(DataTemplate), typeof(ExpandingBottomSheet), new PropertyMetadata(null));
		#endregion

		#region ExpandedContent, ExpandedContentTemplate and ExpandedHeaderContentTemplate
		public object ExpandedContent
		{
			get { return (object)GetValue(ExpandedContentProperty); }
			set { SetValue(ExpandedContentProperty, value); }
		}

		public static readonly DependencyProperty ExpandedContentProperty =
			DependencyProperty.Register("ExpandedContent", typeof(object), typeof(ExpandingBottomSheet), new PropertyMetadata(null));

		public DataTemplate ExpandedContentTemplate
		{
			get { return (DataTemplate)GetValue(ExpandedContentTemplateProperty); }
			set { SetValue(ExpandedContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ExpandedContentTemplateProperty =
			DependencyProperty.Register("ExpandedContentTemplate", typeof(DataTemplate), typeof(ExpandingBottomSheet), new PropertyMetadata(null));

		public DataTemplate ExpandedHeaderContentTemplate
		{
			get { return (DataTemplate)GetValue(ExpandedHeaderContentTemplateProperty); }
			set { SetValue(ExpandedHeaderContentTemplateProperty, value); }
		}

		public static readonly DependencyProperty ExpandedHeaderContentTemplateProperty =
			DependencyProperty.Register("ExpandedHeaderContentTemplate", typeof(DataTemplate), typeof(ExpandingBottomSheet), new PropertyMetadata(null));
		#endregion

		#region IsMinimalSheet
		public bool IsMinimalSheet
		{
			get { return (bool)GetValue(IsMinimalSheetProperty); }
			set { SetValue(IsMinimalSheetProperty, value); }
		}

		public static readonly DependencyProperty IsMinimalSheetProperty =
			DependencyProperty.Register("IsMinimalSheet", typeof(bool), typeof(ExpandingBottomSheet), new PropertyMetadata(false));
		#endregion
	}
}
