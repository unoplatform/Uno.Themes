using System;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	public partial class MaterialColorsV2 : ResourceDictionary
	{
		internal static string ColorOverrideSource;

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

		private static void OnColorPaletteOverrideSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (args.NewValue is string sourceUri)
			{
				ColorOverrideSource = sourceUri;
			}
		}

		public MaterialColorsV2()
		{
			Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColors.xaml");

			MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColorPalette.xaml") });

			var overrideSource = ColorOverrideSource;
			if (!string.IsNullOrWhiteSpace(overrideSource))
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(overrideSource) });
			}
		}
	}
}
