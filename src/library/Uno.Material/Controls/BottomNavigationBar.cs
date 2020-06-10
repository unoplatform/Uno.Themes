using System;
using System.Collections.Generic;
using Uno.Extensions.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Material.Controls
{
	[TemplatePart(Name = "PART_Grid", Type = typeof(Grid))]
	public partial class BottomNavigationBar : Control
	{
		public List<BottomNavigationBarItem> Items
		{
			get => (List<BottomNavigationBarItem>)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		public static readonly DependencyProperty ItemsProperty =
			DependencyProperty.Register(
				nameof(Items),
				typeof(List<BottomNavigationBarItem>),
				typeof(BottomNavigationBar),
				new PropertyMetadata(null, OnItemsChanged));

		public BottomNavigationBarItem SelectedItem
		{
			get => (BottomNavigationBarItem)GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register(
				nameof(SelectedItem),
				typeof(BottomNavigationBarItem),
				typeof(BottomNavigationBar),
				new PropertyMetadata(null, OnSelectedItemChanged));

		private bool _initialized = false;

		public BottomNavigationBar()
		{
			DefaultStyleKey = typeof(BottomNavigationBar);
			Items = new List<BottomNavigationBarItem>();

			Unloaded += BottomNavigationBar_Unloaded;
		}

		private void BottomNavigationBar_Unloaded(object sender, RoutedEventArgs e)
		{
			Unloaded -= BottomNavigationBar_Unloaded;

			foreach (var item in Items)
			{
				item.Checked -= BottomNavigationBarItem_Checked;
			}
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_initialized = true;
			GenerateTabItems();
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
		}

		internal void GenerateTabItems()
		{
			if (!_initialized) return;

			// Unhook events of any previous items
			for (var i = 0; i < Items.Count; i++)
			{
				Items[i].Checked -= BottomNavigationBarItem_Checked;
				Items[i].Unchecked -= BottomNavigationBarItem_Unchecked;
			}

			var rootGrid = GetTemplateChild("PART_Grid") as Grid;

			rootGrid.ColumnDefinitions.Clear();
			rootGrid.Children.Clear();

			for (var i = 0; i < Items.Count; i++)
			{
				var item = Items[i];

				rootGrid.ColumnDefinitions.Add(new ColumnDefinition());
				rootGrid.Children.Add(item);
				Grid.SetColumn(item, i);

				item.Checked += BottomNavigationBarItem_Checked;
				item.Unchecked += BottomNavigationBarItem_Unchecked;

				if (i == 0)
				{
					SelectedItem = item;
				}
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
	}
}
