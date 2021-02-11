using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Uno.Themes.Samples.Content.Controls
{
	public class DividerSamplePageViewModel
	{
		[Bindable]
		public class Item
		{
			public Item(int i, bool displayBorder = true)
			{
				SubItems = Enumerable.Range(1, 2).Select(x => $"group {i} item {x}");
			}

			public IEnumerable<string> SubItems { get; }
		}

		public IEnumerable<Item> Items { get; } = new Item[]
		{
			new Item(1),
			new Item(2),
			new Item(3)
		};
	}

	public sealed partial class DividerSamplePage : Page
	{
		public DividerSamplePage()
		{
			this.InitializeComponent();
			DataContext = new DividerSamplePageViewModel();
		}
	}
}
