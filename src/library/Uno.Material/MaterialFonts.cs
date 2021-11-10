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
		private static string FontOverrideSource;

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

		private static void OnFontOverrideSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			FontOverrideSource = args.NewValue as string;
		}

		public MaterialFonts()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/Fonts.xaml") });

			if (!string.IsNullOrWhiteSpace(FontOverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary
				{
					Source = new Uri(FontOverrideSource)
				});
			}
		}
	}
}
