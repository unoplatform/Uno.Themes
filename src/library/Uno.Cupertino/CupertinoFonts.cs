using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif


namespace Uno.Cupertino
{
	public sealed partial class CupertinoFonts : ResourceDictionary
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
				typeof(CupertinoFonts),
				new PropertyMetadata(null, OnFontOverrideSourcePropertyChanged));

		private static void OnFontOverrideSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			FontOverrideSource = args.NewValue as string;
		}

		public CupertinoFonts()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/Fonts.xaml") });

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
