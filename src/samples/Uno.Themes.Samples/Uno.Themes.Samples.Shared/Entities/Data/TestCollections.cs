using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Uno.Themes.Samples.Entities.Data
{
	public class TestCollections
	{
		public ObservableCollection<SelectableData> MutableTestCollection { get; } = new ObservableCollection<SelectableData>(CreateItems());
		public ObservableCollection<SelectableData> TestCollection { get; } = new ObservableCollection<SelectableData>(CreateItems());
		public static IEnumerable<SelectableData> TestArray { get; } = CreateItems();
		public static IEnumerable<SelectableData> TestSelectedItems { get; } = TestArray.Take(3).ToArray();
		public static SelectableData TestItem { get; } = TestArray.First();
		public static IEnumerable<DayOfWeek> TestEnumArray { get; } = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
		public static DayOfWeek TestEnumItem { get; } = DayOfWeek.Monday;

		private static IEnumerable<SelectableData> CreateItems()
		{
			return Enumerable.Range(0, 10)
				.Select(x => new SelectableData
				{
					Index = x,
					Image = new Uri("ms-appx:///Assets/Avatar.png"),
				})
				.ToArray();
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
