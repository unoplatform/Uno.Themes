using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Themes.Common;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public sealed class MaterialResourcesV2 : ResourceDictionary
	{
		private static string _colorPaletteOverrideSource;

		public string ColorPaletteOverrideSource
		{
			get => (string)GetValue(ColorPaletteOverrideSourceProperty);
			set => SetValue(ColorPaletteOverrideSourceProperty, value);
		}

		public static DependencyProperty ColorPaletteOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(ColorPaletteOverrideSource),
				typeof(string),
				typeof(MaterialResourcesV2),
				new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			_colorPaletteOverrideSource = args.NewValue as string;
		}

		private static string _fontOverrideSource;

		public string FontOverrideSource
		{
			get => (string)GetValue(FontOverrideSourceProperty);
			set => SetValue(FontOverrideSourceProperty, value);
		}

		public static DependencyProperty FontOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(FontOverrideSource),
				typeof(string),
				typeof(MaterialResourcesV2),
				new PropertyMetadata(null, OnFontOverrideSourcePropertyChanged));

		private static void OnFontOverrideSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			_fontOverrideSource = args.NewValue as string;
		}


		public MaterialResourcesV2()
		{
			Source = new Uri("ms-appx:///Uno.Material.v2/Generated/mergedpages.xaml");

			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Themes.Common/ColorPalette.xaml") });

			if (!string.IsNullOrWhiteSpace(_colorPaletteOverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(_colorPaletteOverrideSource) });
			}

			MergedDictionaries.Add(new Colors());

			//MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/Fonts.xaml") });

			//if (!string.IsNullOrWhiteSpace(_fontOverrideSource))
			//{
			//	MergedDictionaries.Add(new ResourceDictionary
			//	{
			//		Source = new Uri(_fontOverrideSource)
			//	});
			//}
		}
	}
}
