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
		private static string OverrideSource;

		public string FontOverrideSource
		{
			get => (string)GetValue(FontOverrideSourceProperty);
			set => SetValue(FontOverrideSourceProperty, value);
		}

		public static DependencyProperty FontOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(FontOverrideSource),
				typeof(string),
				typeof(CupertinoFonts),
				new PropertyMetadata(null, OnFontOverrideSourcePropertyChanged));

		private static void OnFontOverrideSourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			OverrideSource = args.NewValue as string;
		}

		public CupertinoFonts()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/Fonts.xaml") });

			if (!string.IsNullOrWhiteSpace(OverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary
				{
					Source = new Uri(OverrideSource)
				});
			}
		}
	}
}
