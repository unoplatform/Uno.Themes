using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public sealed partial class MaterialColors : ResourceDictionary
	{
		private static string OverrideSource;

		public string ColorPaletteOverrideSource
		{
			get => (string)GetValue(ColorPaletteOverrideSourceProperty);
			set => SetValue(ColorPaletteOverrideSourceProperty, value);
		}

		public static DependencyProperty ColorPaletteOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(ColorPaletteOverrideSource),
				typeof(string),
				typeof(MaterialColors),
				new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			OverrideSource = args.NewValue as string;
		}

		public MaterialColors()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/ColorPalette.xaml") });
			if (!string.IsNullOrWhiteSpace(OverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(OverrideSource) });
			}

			InitializeComponent();
		}

	}
}
