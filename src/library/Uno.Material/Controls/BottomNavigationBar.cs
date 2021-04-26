using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Uno.Material.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Uno.Material.Controls
{
	[TemplatePart(Name = LayoutRootName, Type = typeof(Grid))]
	public partial class BottomNavigationBar : Control
	{
		private const string LayoutRootName = "PART_LayoutRoot";

		private bool _templateApplied = false;
		private Grid _layoutRoot;

		public BottomNavigationBar()
		{
			DefaultStyleKey = typeof(BottomNavigationBar);
			Items = new List<BottomNavigationBarItem>();

			Unloaded += BottomNavigationBar_Unloaded;
			Loaded += BottomNavigationBar_Loaded;
		}

		#region Property: Items

		public static DependencyProperty ItemsProperty { get; } = DependencyProperty.Register(
			nameof(Items),
			typeof(IList<BottomNavigationBarItem>),
			typeof(BottomNavigationBar),
			new PropertyMetadata(default, OnItemsChanged));
		public IList<BottomNavigationBarItem> Items
		{
			get => (IList<BottomNavigationBarItem>)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		#endregion
		#region Property: SelectedItem

		public static DependencyProperty SelectedItemProperty { get; } = DependencyProperty.Register(
			nameof(SelectedItem),
			typeof(BottomNavigationBarItem),
			typeof(BottomNavigationBar),
			new PropertyMetadata(default, OnSelectedItemChanged));
		public BottomNavigationBarItem SelectedItem
		{
			get => (BottomNavigationBarItem)GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		#endregion

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_layoutRoot = this.GetTemplateChild<Grid>(GetTemplateChild, LayoutRootName);
			_templateApplied = true;

			GenerateTabItems();
		}

		internal void GenerateTabItems()
		{
			if (!_templateApplied) return;
			if (_layoutRoot != null)
			{
				// todo: diff update
				_layoutRoot.ColumnDefinitions.Clear();
				_layoutRoot.Children.Clear();

				if (Items != null)
				{
					foreach (var item in Items)
					{
						_layoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
						_layoutRoot.Children.Add(item);
						Grid.SetColumn(item, _layoutRoot.Children.Count - 1);
					}

					RegisterBarItemsEvents();

					SelectedItem = Items.FirstOrDefault(x => x.IsChecked == true) ?? Items.FirstOrDefault();
				}
				else
				{
					SelectedItem = null;
				}
			}
		}

		private void BottomNavigationBar_Unloaded(object sender, RoutedEventArgs e)
		{
			UnregisterBarItemsEvents();
		}

		private void BottomNavigationBar_Loaded(object sender, RoutedEventArgs e)
		{
			RegisterBarItemsEvents();
		}

		private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as BottomNavigationBar).GenerateTabItems();
		}

		private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is BottomNavigationBarItem item)
			{
				item.IsChecked = true;
			}
			else if (e.NewValue == null && e.OldValue is BottomNavigationBarItem oldItem)
			{
				oldItem.IsChecked = false;
			}
		}

		private void BottomNavigationBarItem_Checked(object sender, RoutedEventArgs e)
		{
			var navItem = sender as BottomNavigationBarItem;

			SelectedItem = navItem;

			foreach (BottomNavigationBarItem item in Items.Where(i => !i.Equals(navItem)))
			{
				item.IsChecked = false;
			}
		}

		private void BottomNavigationBarItem_Unchecked(object sender, RoutedEventArgs e)
		{
			// We make sure to not unselect the currently selected item
			if (sender is BottomNavigationBarItem item && item == SelectedItem)
			{
				item.IsChecked = true;
			}
		}

		private void RegisterBarItemsEvents()
		{
			if (!_templateApplied) return;

			foreach (var item in Items.Safe())
			{
				item.Checked -= BottomNavigationBarItem_Checked;
				item.Unchecked -= BottomNavigationBarItem_Unchecked;

				item.Checked += BottomNavigationBarItem_Checked;
				item.Unchecked += BottomNavigationBarItem_Unchecked;
			}
		}

		private void UnregisterBarItemsEvents()
		{
			foreach (var item in Items.Safe())
			{
				item.Checked -= BottomNavigationBarItem_Checked;
				item.Unchecked -= BottomNavigationBarItem_Unchecked;
			}
		}
	}
}
