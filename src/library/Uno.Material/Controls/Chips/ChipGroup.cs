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
	public partial class ChipGroup : ItemsControl
	{
		public event ChipItemEventHandler ItemClick;
		public event ChipItemEventHandler ItemChecked;
		public event ChipItemEventHandler ItemUnchecked;
		public event ChipItemRemovingEventHandler ItemRemoving;
		public event ChipItemEventHandler ItemRemoved;

		private bool _isSynchronizingSelection = false;
		private bool _isUpdatingSelection = false;

		public ChipGroup()
		{
			RegisterPropertyChangedCallback(ItemsSourceProperty, (s, e) => (s as ChipGroup)?.OnItemsSourceChanged());

			this.Loaded += OnLoaded;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			EnforceSelectionMode();
			ApplyThumbnailTemplate();
		}

		private void OnSelectionMemberPathChanged(DependencyPropertyChangedEventArgs e)
		{
			var binding = SelectionMemberPath != null
				? new Binding { Path = new PropertyPath(SelectionMemberPath), Mode = BindingMode.TwoWay }
				: null;
			foreach (var container in GetItemContainers())
			{
				if (binding != null)
				{
					container.SetBinding(Chip.IsCheckedProperty, binding);
				}

				container.ClearValue(Chip.IsCheckedProperty);
			}
		}

		private void ApplyThumbnailTemplate()
		{
			if (ThumbnailTemplate != null)
			{
				foreach (var container in GetItemContainers())
				{
					container.Thumbnail = container.Content;
					container.ThumbnailTemplate = ThumbnailTemplate;
				}
			}
		}

		private void ApplyCanRemoveProperty()
		{
			foreach (var container in GetItemContainers())
			{
				container.CanRemove = CanRemove;
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

				if (ThumbnailTemplate != null)
				{
					container.Thumbnail = container.Content;
					container.ThumbnailTemplate = ThumbnailTemplate;
				}

				container.CanRemove = CanRemove;

				container.IsCheckedChanged += OnItemIsCheckedChanged;
				container.Click += OnItemClick;
				container.Checked += OnItemChecked;
				container.Unchecked += OnItemUnchecked;
				container.Removing += OnItemRemoving;
				container.Removed += OnItemRemoved;
			}
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);

			if (element is Chip container)
			{
				container.ClearValue(Chip.IsCheckedProperty);

				container.Thumbnail = null;
				container.ThumbnailTemplate = null;
				container.CanRemove = false;

				container.IsCheckedChanged -= OnItemIsCheckedChanged;
				container.Click -= OnItemClick;
				container.Checked -= OnItemChecked;
				container.Unchecked -= OnItemUnchecked;
				container.Removing -= OnItemRemoving;
				container.Removed -= OnItemRemoved;
			}
		}

		private void OnItemClick(object sender, RoutedEventArgs e) => RaiseItemEvent(ItemClick, sender);

		private void OnItemChecked(object sender, RoutedEventArgs e) => RaiseItemEvent(ItemChecked, sender);

		private void OnItemUnchecked(object sender, RoutedEventArgs e) => RaiseItemEvent(ItemUnchecked, sender);

		private void OnItemRemoving(object sender, ChipRemovingEventArgs e)
		{
			if (sender is Chip container)
			{
				var args = new ChipItemRemovingEventArgs(ItemFromContainer(container));
				args.Cancel = e.Cancel;

				ItemRemoving?.Invoke(this, new ChipItemRemovingEventArgs(ItemFromContainer(container)));
				e.Cancel = args.Cancel;
			}
		}

		private void OnItemRemoved(object sender, RoutedEventArgs e)
		{
			if (sender is Chip container)
			{
				// there isn't much that can be done here if the item is generated from an ItemsSource
				// in such case, the removal should be handled from the source provider (view-model or code-behind)

				// remove the item only if it was added via xaml or .Add(item)
				if (ItemsSource == null &&
					Items?.IndexOf(container) is int index && index != -1)
				{
					Items.RemoveAt(index);
				}

				RaiseItemEvent(ItemRemoved, sender);
			}
		}

		private void RaiseItemEvent(ChipItemEventHandler handler, object originalSender)
		{
			if (originalSender is Chip container)
			{
				handler?.Invoke(this, new ChipItemEventArgs(ItemFromContainer(container)));
			}
		}

		private void EnforceSelectionMode()
		{
			if (ItemsPanelRoot == null) return;

			if (SelectionMode == ChipSelectionMode.None)
			{
				UpdateItemsIsCheckable();
				UpdateSelection(default);
			}
			else if (SelectionMode == ChipSelectionMode.Single)
			{
				UpdateItemsIsCheckable();

				// either one is selected or none are selected
				var selectedContainers = GetItemContainers().Where(x => x.IsChecked ?? false).ToArray();
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
				UpdateItemsIsCheckable();
				UpdateSelection(default);
			}

			void UpdateItemsIsCheckable()
			{
				foreach (var container in GetItemContainers())
				{
					container.IsCheckable = SelectionMode != ChipSelectionMode.None;
				}
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
					if (!container.IsChecked ?? false)
					{
						continue;
					}

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
