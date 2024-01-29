﻿using System;
using Uno.Themes;
using Windows.Foundation.Metadata;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	[Deprecated("Resource initialization for the Uno.Material theme should now be done using the MaterialTheme class instead.", DeprecationType.Deprecate, 3)]
	public sealed partial class MaterialFonts : ResourceDictionary
	{
		private static ResourceDictionary FontOverride;

		public string OverrideSource
		{
			get => (string)GetValue(OverrideSourceProperty);
			set => SetValue(OverrideSourceProperty, value);
		}

		public static DependencyProperty OverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(OverrideSource),
				typeof(string),
				typeof(MaterialFonts),
				new PropertyMetadata(null, OnFontOverrideSourcePropertyChanged));

		public ResourceDictionary OverrideDictionary
		{
			get => (ResourceDictionary)GetValue(OverrideDictionaryProperty);
			set => SetValue(OverrideDictionaryProperty, value);
		}

		public static DependencyProperty OverrideDictionaryProperty { get; } =
			DependencyProperty.Register(
				nameof(OverrideDictionary),
				typeof(ResourceDictionary),
				typeof(MaterialFonts),
				new PropertyMetadata(null, OnOverrideChanged));

		private static void OnFontOverrideSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (dependencyObject is MaterialFonts fonts && args.NewValue is string sourceUri)
			{
				fonts.OverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
			}
		}

		private static void OnOverrideChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			FontOverride = args.NewValue as ResourceDictionary;
		}

		public MaterialFonts()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/Common/Fonts.xaml") });

			if (FontOverride is { } fontOverride)
			{
				this.SafeMerge(fontOverride);
			}
		}
	}
}
