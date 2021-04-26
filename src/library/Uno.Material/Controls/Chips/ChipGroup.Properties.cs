using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material.Controls
{
	public partial class ChipGroup
	{
#region DependencyProperty: SelectedItem

		public static DependencyProperty SelectedItemProperty { get; } = DependencyProperty.Register(
			nameof(SelectedItem),
			typeof(object),
			typeof(ChipGroup),
			new PropertyMetadata(default, (s, e) => (s as ChipGroup)?.OnSelectedItemChanged(e)));

		public object SelectedItem
		{
			get => (object)GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

#endregion

#region DependencyProperty: SelectedItems

		public static DependencyProperty SelectedItemsProperty { get; } = DependencyProperty.Register(
			nameof(SelectedItems),
			typeof(IList),
			typeof(ChipGroup),
			new PropertyMetadata(default, (s, e) => (s as ChipGroup)?.OnSelectedItemsChanged(e)));

		public IList SelectedItems
		{
			get => (IList)GetValue(SelectedItemsProperty);
			set => SetValue(SelectedItemsProperty, value);
		}

#endregion

#region DependencyProperty: SelectionMemberPath

		public static DependencyProperty SelectionMemberPathProperty { get; } = DependencyProperty.Register(
			nameof(SelectionMemberPath),
			typeof(string),
			typeof(ChipGroup),
			new PropertyMetadata(default, (s, e) => (s as ChipGroup)?.OnSelectionMemberPathChanged(e)));

		/// <summary>
		/// Gets or sets the name or path of the property that indicates selection state for each data item.
		/// </summary>
		public string SelectionMemberPath
		{
			get => (string)GetValue(SelectionMemberPathProperty);
			set => SetValue(SelectionMemberPathProperty, value);
		}

#endregion

#region DependencyProperty: SelectionMode = ChipSelectionMode.Single

		public static DependencyProperty SelectionModeProperty { get; } = DependencyProperty.Register(
			nameof(SelectionMode),
			typeof(ChipSelectionMode),
			typeof(ChipGroup),
			new PropertyMetadata(ChipSelectionMode.Single, (s, e) => (s as ChipGroup)?.OnSelectionModeChanged(e)));

		public ChipSelectionMode SelectionMode
		{
			get => (ChipSelectionMode)GetValue(SelectionModeProperty);
			set => SetValue(SelectionModeProperty, value);
		}

#endregion

#region DependencyProperty: ThumbnailTemplate

		public DataTemplate ThumbnailTemplate
		{
			get { return (DataTemplate)GetValue(ThumbnailTemplateProperty); }
			set { SetValue(ThumbnailTemplateProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailTemplateProperty =
			DependencyProperty.Register("ThumbnailTemplate", typeof(DataTemplate), typeof(ChipGroup), new PropertyMetadata(null, (s, e) => (s as ChipGroup)?.ApplyThumbnailTemplate()));

#endregion

#region DependencyProperty: CanRemove = false

		// TODO : Implement Remove Command/Event

		public bool CanRemove
		{
			get { return (bool)GetValue(CanRemoveProperty); }
			set { SetValue(CanRemoveProperty, value); }
		}

		public static readonly DependencyProperty CanRemoveProperty =
			DependencyProperty.Register("CanRemove", typeof(bool), typeof(ChipGroup), new PropertyMetadata(false, (s, e) => (s as ChipGroup)?.ApplyCanRemoveProperty()));

#endregion
	}
}
