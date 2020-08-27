using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uno.Extensions.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Uno.Material.Controls
{
	public partial class ChipBag : ItemsControl
	{
		#region DependencyProperty: SelectedItem

		public static DependencyProperty SelectedItemProperty { get; } = DependencyProperty.Register(
			nameof(SelectedItem),
			typeof(object),
			typeof(ChipBag),
			new PropertyMetadata(default, (s, e) => (s as ChipBag)?.OnSelectedItemChanged(e)));

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
			typeof(ChipBag),
			new PropertyMetadata(default, (s, e) => (s as ChipBag)?.OnSelectedItemsChanged(e)));

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
			typeof(ChipBag),
			new PropertyMetadata(default, (s, e) => (s as ChipBag)?.OnSelectionMemberPathChanged(e)));

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
			typeof(ChipBag),
			new PropertyMetadata(ChipSelectionMode.Single, (s, e) => (s as ChipBag)?.OnSelectionModeChanged(e)));

		public ChipSelectionMode SelectionMode
		{
			get => (ChipSelectionMode)GetValue(SelectionModeProperty);
			set => SetValue(SelectionModeProperty, value);
		}

		#endregion

		private bool _isSynchronizingSelection = false;
		private bool _isUpdatingSelection = false;

		public ChipBag()
		{
#if WINDOWS_UWP
			RegisterPropertyChangedCallback(ItemsSourceProperty, (s, e) => (s as ChipBag)?.OnItemsSourceChanged());
#endif

			this.Loaded += OnLoaded;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			EnforceSelectionMode();
		}

		private void OnSelectionMemberPathChanged(DependencyPropertyChangedEventArgs e)
		{
			var binding = SelectionMemberPath != null
				? new Binding { Path = new PropertyPath(SelectionMemberPath), Mode = BindingMode.TwoWay }
				: null;
			foreach (var container in GetItemContainers())
			{
				if (binding != null) container.SetBinding(Chip.IsCheckedProperty, binding);
				container.ClearValue(Chip.IsCheckedProperty);
			}
		}

		private void OnSelectionModeChanged(DependencyPropertyChangedEventArgs e)
		{
			EnforceSelectionMode();
		}

		private void OnItemIsCheckedChanged(object sender, RoutedEventArgs e)
		{
			if (_isSynchronizingSelection) return;
			if (sender is Chip container)
			{
				UpdateSelection(new[] { container });
			}
		}

		protected override void OnItemsChanged(object e)
		{
			base.OnItemsChanged(e);

			EnforceSelectionMode();
		}

#if !WINDOWS_UWP
		protected override void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnItemsSourceChanged(e);

			OnItemsSourceChanged();
		}
#endif

		protected void OnItemsSourceChanged()
		{
			EnforceSelectionMode();
		}

		private void OnSelectedItemChanged(DependencyPropertyChangedEventArgs e)
		{
			if (_isSynchronizingSelection || _isUpdatingSelection) return;

			if (SelectionMode == ChipSelectionMode.Single && FindContainer(SelectedItem) is Chip container)
			{
				container.SetIsCheckedSilently(true);
				UpdateSelection(new[] { container });
			}
			else
			{
				UpdateSelection(null);
			}
		}

		private void OnSelectedItemsChanged(DependencyPropertyChangedEventArgs e)
		{
			if (_isSynchronizingSelection || _isUpdatingSelection) return;

			if (SelectionMode == ChipSelectionMode.Multiple)
			{
				var selectedContainers = SelectedItems != null
					? GetItemContainers().Where(x => SelectedItems.Contains(x)).ToArray()
					: null;
				foreach (var container in selectedContainers ?? Enumerable.Empty<Chip>())
				{
					container.SetIsCheckedSilently(true);
				}

				UpdateSelection(selectedContainers, forceClearOthersSelection: true);
			}
			else
			{
				UpdateSelection(null, forceClearOthersSelection: true);
			}

		}

		protected override bool IsItemItsOwnContainerOverride(object item) => item is Chip;

		protected override DependencyObject GetContainerForItemOverride() => new Chip();

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);

			if (element is Chip container)
			{
				if (SelectionMemberPath != null)
				{
					container.SetBinding(Chip.IsCheckedProperty, new Binding { Path = new PropertyPath(SelectionMemberPath), Mode = BindingMode.TwoWay });
				}

				container.IsCheckedChanged += OnItemIsCheckedChanged;
			}
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);

			if (element is Chip container)
			{
				container.ClearValue(Chip.IsCheckedProperty);

				container.IsCheckedChanged -= OnItemIsCheckedChanged;
			}
		}

		private void EnforceSelectionMode()
		{
			if (ItemsPanelRoot == null) return;

			if (SelectionMode == ChipSelectionMode.None)
			{
				UpdateSelection(default);
			}
			else if (SelectionMode == ChipSelectionMode.Single)
			{
				var containers = GetItemContainers().ToArray();
				var selectedContainers = GetItemContainers().Where(x => x.IsChecked == true).ToArray();
				// either one is selected or none are selected
				if (selectedContainers.Length > 1)
				{
					foreach (var container in selectedContainers)
					{
						container.SetIsCheckedSilently(false);
					}
					UpdateSelection(default);
				}
				else
				{
					UpdateSelection(selectedContainers);
				}
			}
			else if (SelectionMode == ChipSelectionMode.Multiple)
			{
				UpdateSelection(default);
			}
		}

		private void UpdateSelection(Chip[] newlySelectedContainers, bool forceClearOthersSelection = false)
		{
			if (ItemsPanelRoot == null) return;
			if (_isSynchronizingSelection) return;

			try
			{
				_isSynchronizingSelection = true;

				var selectedItems = new List<object>();
				foreach (var container in GetItemContainers())
				{
					if (container.IsChecked == true)
					{
						if (ShouldClearSelection())
						{
							container.SetIsCheckedSilently(false);
						}
						else
						{
							selectedItems.Add(ItemFromContainer(container));
						}

						bool ShouldClearSelection()
						{
							switch (SelectionMode)
							{
								case ChipSelectionMode.None:
									// uncheck all
									return true;
								case ChipSelectionMode.Single:
									// uncheck every other items
									return newlySelectedContainers?.Contains(container) != true;
								case ChipSelectionMode.Multiple:
									// uncheck other items if SelectedItem or SelectedItems got updated
									return forceClearOthersSelection && newlySelectedContainers?.Contains(container) != true;

								default: throw new ArgumentOutOfRangeException(nameof(SelectionMode));
							}
						}
					}
				}

				// update selection properties
				SelectedItem = SelectionMode == ChipSelectionMode.Single && selectedItems.Count == 1
					? selectedItems[0]
					: null;
				SelectedItems = SelectionMode == ChipSelectionMode.Multiple
					? selectedItems
					: null;
			}
			finally
			{
				_isSynchronizingSelection = false;
			}
		}

		/// <summary>
		/// Get the items.
		/// </summary>
		/// <remarks>The item itself maybe its own container, as in the case of <see cref="Chip"> added as child to <see cref="ItemsControl.Items"/>.</remarks>
		private IEnumerable GetItems() =>
			ItemsSource as IEnumerable ??
			(ItemsSource as CollectionViewSource)?.View ??
			Items ??
			Enumerable.Empty<object>();

		private Chip FindContainer(object item)
		{
			if (item == null) return null;

			return
				item as Chip ??
				ContainerFromItem(item) as Chip;
		}

		/// <summary>
		/// Get the item containers.
		/// </summary>
		/// <remarks>An empty enumerable will returned if the <see cref="ItemsControl.ItemsPanelRoot"/> and the containers have not been materialized.</remarks>
		private IEnumerable<Chip> GetItemContainers() =>
			ItemsPanelRoot?.Children.OfType<Chip>() ??
			Enumerable.Empty<Chip>();
	}
}
