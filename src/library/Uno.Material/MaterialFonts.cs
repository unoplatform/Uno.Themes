using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public sealed partial class MaterialFonts : ResourceDictionary
	{
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
			if (dependencyObject is MaterialFonts fonts && args.NewValue is ResourceDictionary @override)
			{
				fonts.MergedDictionaries.Add(@override);
			}
		}

		public MaterialFonts()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/Common/Fonts.xaml") });
		}
	}
}
