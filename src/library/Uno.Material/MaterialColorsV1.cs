using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public partial class MaterialColorsV1 : ResourceDictionary
	{
		private static string ColorPaletteOverrideSource;

		public string OverrideSource
		{
			get => (string)GetValue(OverrideSourceProperty);
			set => SetValue(OverrideSourceProperty, value);
		}

		public static DependencyProperty OverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(OverrideSource),
				typeof(string),
				typeof(MaterialColorsV1),
				new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			ColorPaletteOverrideSource = args.NewValue as string;
		}

		public MaterialColorsV1()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v1/ColorPalette.xaml") });
			if (!string.IsNullOrWhiteSpace(ColorPaletteOverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(ColorPaletteOverrideSource) });
			}

			InitializeComponent();
		}
	}
}
