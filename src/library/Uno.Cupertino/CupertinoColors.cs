using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino
{
	public sealed partial class CupertinoColors : ResourceDictionary
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
				typeof(CupertinoColors),
				new PropertyMetadata(null, OnColorPaletteOverrideSourceChanged));

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			OverrideSource = args.NewValue as string;
		}

		public CupertinoColors()
		{
			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/ColorPalette.xaml") });
			if (!string.IsNullOrWhiteSpace(OverrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(OverrideSource) });
			}

			InitializeComponent();
		}
	}
}
