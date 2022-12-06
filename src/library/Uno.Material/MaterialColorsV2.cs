﻿using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public partial class MaterialColorsV2 : ResourceDictionary
	{
		private static ResourceDictionary ColorPaletteOverride;

		public string OverrideSource
		{
			get => (string)GetValue(OverrideSourceProperty);
			set => SetValue(OverrideSourceProperty, value);
		}

		public static DependencyProperty OverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(OverrideSource),
				typeof(string),
				typeof(MaterialColorsV2),
				new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

		public ResourceDictionary OverrideDictionary
		{
			get => (ResourceDictionary)GetValue(OverrideDictionaryProperty);
			set => SetValue(OverrideDictionaryProperty, value);
		}

		public static DependencyProperty OverrideDictionaryProperty { get; } =
			DependencyProperty.Register(
				nameof(OverrideDictionary),
				typeof(ResourceDictionary),
				typeof(MaterialColorsV2),
				new PropertyMetadata(null, OnOverrideChanged));

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (dependencyObject is MaterialColorsV2 colors && args.NewValue is string sourceUri)
			{
				colors.OverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
			}
		}

		private static void OnOverrideChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			ColorPaletteOverride = args.NewValue as ResourceDictionary;
		}

		public MaterialColorsV2()
		{
			Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColors.xaml");

			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColorPalette.xaml") });
			if (ColorPaletteOverride is { } colorOverride)
			{
				MergedDictionaries.Add(colorOverride);
			}
		}
	}
}
