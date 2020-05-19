using System;
using System.Collections.Generic;
using Uno.Extensions.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Material.Controls
{
	public partial class BottomNavigationBar : Control
	{
		public List<BottomNavigationBarItem> Items
		{
			get => (List<BottomNavigationBarItem>)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		public readonly DependencyProperty ItemsProperty =
			DependencyProperty.Register(
				"Items",
				typeof(List<BottomNavigationBarItem>),
				typeof(BottomNavigationBar),
				new PropertyMetadata(new List<BottomNavigationBarItem>(), OnItemsChanged));

		public BottomNavigationBarItem SelectedItem
		{
			get => (BottomNavigationBarItem)GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register(
				"SelectedItem",
				typeof(BottomNavigationBarItem),
				typeof(BottomNavigationBar),
				new PropertyMetadata(null, OnSelectedItemChanged));

		public int SelectedIndex
		{
			get => (int)GetValue(SelectedIndexProperty);
			set => SetValue(SelectedIndexProperty, value);
		}

		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.Register(
				"SelectedIndex",
				typeof(int),
				typeof(BottomNavigationBar),
				new PropertyMetadata(0));

		public BottomNavigationBar()
		{
			DefaultStyleKey = typeof(BottomNavigationBar);

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

				if (i == 0)
				{
					SelectedItem = item;
				}
			}
		}

		private void BottomNavigationBarItem_Checked(object sender, RoutedEventArgs e)
		{
			var navItem = sender as BottomNavigationBarItem;

			foreach (BottomNavigationBarItem item in Items.Where(i => !i.Equals(navItem)))
			{
				item.IsChecked = false;
			}

			SelectedIndex = Items.IndexOf(navItem);
		}
	}
}
