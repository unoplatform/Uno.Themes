using System;
using System.Collections.Generic;
using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes.Common
{
	public sealed partial class Colors : ResourceDictionary
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
				typeof(Colors),
				new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			ColorPaletteOverrideSource = args.NewValue as string;
		}

		public Colors()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Themes.Common/ColorPalette.xaml") });
			if (!string.IsNullOrWhiteSpace(OverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(OverrideSource) });
			}

			InitializeComponent();
		}
	}
}
