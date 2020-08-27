using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Material.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uno.Material.Samples.Content.Controls
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ChipSamplePage : Page
	{
		public ChipSamplePage()
		{
			this.InitializeComponent();
			this.DataContext = Enumerable.Range(0, 10)
				.Select(x => new SelectableData
				{
					Text = $"Item {x}",
					IsSelected = BindingChipBag.SelectionMode == ChipSelectionMode.Multiple && x % 2 == 0
				})
				.ToArray();
		}

		private void ChangeItemsSource(object sender, RoutedEventArgs e)
		{
			this.DataContext = Enumerable.Range(0, new Random().Next(8, 12 + 2))
				.Select(x => new SelectableData
				{
					Text = $"Item {x}",
					IsSelected = BindingChipBag.SelectionMode == ChipSelectionMode.Multiple && x % 2 == 0
				})
				.ToArray();
		}

		private void SetBindingChipBagSelectionMode(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton radio)
			{
				switch (radio.Content)
				{
					case "None": BindingChipBag.SelectionMode = ChipSelectionMode.None; break;
					case "Single": BindingChipBag.SelectionMode = ChipSelectionMode.Single; break;
					case "Multiple": BindingChipBag.SelectionMode = ChipSelectionMode.Multiple; break;

					default: throw new ArgumentOutOfRangeException($"Invalid option: {radio.Content}");
				}
			}
		}

		public class SelectableData : InpcObject
		{
			public string Text { get => GetProperty<string>(); set => SetProperty(value); }

			public bool IsSelected { get => GetProperty<bool>(); set => SetProperty(value); }

			public override string ToString() => $"[{(IsSelected ? 'X' : ' ')}] {Text}";
		}

		public class InpcObject : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			private readonly Dictionary<string, object> _propertyValueStore = new Dictionary<string, object>();

			protected T GetProperty<T>([CallerMemberName] string propertyName = null)
			{
				return _propertyValueStore.TryGetValue(propertyName, out var value) ? (T)value : default;
			}

			protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
			{
				if (!(_propertyValueStore.TryGetValue(propertyName, out var oldValue) && oldValue?.Equals(value) == true))
				{
					_propertyValueStore[propertyName] = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}
	}
}
