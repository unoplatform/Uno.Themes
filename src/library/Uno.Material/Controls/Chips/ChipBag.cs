using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Uno.Material.Controls
{
	public partial class ChipBag : ItemsControl
	{
		#region Property: ItemIsCheckedBinding
		private Binding _itemIsCheckedBinding;
		public Binding ItemIsCheckedBinding
		{
			get => _itemIsCheckedBinding;
			set
			{
				if (_itemIsCheckedBinding == value) return;

				_itemIsCheckedBinding = value;
			}
		}
		#endregion
		#region Property: ItemContentBinding
		private Binding _itemContentBinding;
		public Binding ItemContentBinding
		{
			get => _itemContentBinding;
			set
			{
				if (_itemContentBinding == value) return;

				_itemContentBinding = value;
			}
		}
		#endregion

		protected override bool IsItemItsOwnContainerOverride(object item) => item is Chip;

		protected override DependencyObject GetContainerForItemOverride() => new Chip();

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);

			if (element is Chip container)
			{
				ForwardBinding(Chip.IsCheckedProperty, ItemIsCheckedBinding);
				ForwardBinding(Chip.ContentProperty, ItemContentBinding);
			}

			void ForwardBinding(DependencyProperty property, Binding binding)
			{
				if (binding != null)
				{
					container.SetBinding(property, binding);
				}
			}
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);

			if (element is Chip container)
			{
				container.ClearValue(Chip.IsCheckedProperty);
				container.ClearValue(Chip.ContentProperty);
			}
		}
	}
}
