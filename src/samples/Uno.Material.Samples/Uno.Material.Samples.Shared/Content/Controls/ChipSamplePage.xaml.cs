using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Uno.Material.Samples.Content.Controls
{
	public sealed partial class ChipSamplePage : Page
	{
		public ObservableCollection<SelectableData> MutableTestCollection { get; } = new ObservableCollection<SelectableData>(CreateItems());
		public ObservableCollection<SelectableData> TestCollection { get; } = new ObservableCollection<SelectableData>(CreateItems());

		public ChipSamplePage()
		{
			this.InitializeComponent();
		}

		private static IEnumerable<SelectableData> CreateItems()
		{
			return Enumerable.Range(0, 10)
				.Select(x => new SelectableData
				{
					Index = x,
					Image = new Uri("ms-appx:///Assets/Cards/Avatar.png"),
				})
				.ToArray();
		}

		private void RemoveChipItem(object sender, ChipItemEventArgs e)
		{
			MutableTestCollection.Remove(e.Item as SelectableData);
		}

		private void ResetChipItems(object sender, RoutedEventArgs e)
		{
			MutableTestCollection.Clear();
			foreach (var item in CreateItems())
			{
				MutableTestCollection.Add(item);
			}
		}

		public class SelectableData : InpcObject
		{
			public int Index { get => GetProperty<int>(); set => SetProperty(value); }

			public Uri Image { get => GetProperty<Uri>(); set => SetProperty(value); }
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
