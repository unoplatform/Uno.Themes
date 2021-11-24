using System;
using System.ComponentModel;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public sealed partial class MaterialColors : ResourceDictionary
	{
		public static ResourceDictionary _overrideDictionary;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public ResourceDictionary OverrideDictionary
		{
			get => _overrideDictionary;
			set => _overrideDictionary = value;
		}

		private static string _colorPaletteOverrideSource;

		public string OverrideSource
		{
			get => (string)GetValue(OverrideSourceProperty);
			set => SetValue(OverrideSourceProperty, value);
		}

		public static DependencyProperty OverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(OverrideSource),
				typeof(string),
				typeof(MaterialColors),
				new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			_colorPaletteOverrideSource = args.NewValue as string;
		}

		public MaterialColors()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/ColorPalette.xaml") });
			if (!string.IsNullOrWhiteSpace(_colorPaletteOverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(_colorPaletteOverrideSource) });
			}

			if (_overrideDictionary is { } overrideDictionary)
			{
				var d = new ResourceDictionary();

				foreach (var key in _overrideDictionary.Keys)
				{
					d[key] = _overrideDictionary[key];
				}
				
				MergedDictionaries.Add(d);
			}

			InitializeComponent();
		}

	}
}
