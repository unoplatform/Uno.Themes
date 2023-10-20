using System;
using System.Diagnostics.CodeAnalysis;

#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

#endif
namespace Uno.Material
{
	public class ThemeExtensions
	{
		#region DependencyProperty: ColorPalette
		public static DependencyProperty ColorPaletteProperty { [DynamicDependency(nameof(GetColorPalette))] get; } = DependencyProperty.RegisterAttached(
			"ColorPalette",
			typeof(ResourceDictionary),
			typeof(ThemeExtensions),
			new PropertyMetadata(default, OnColorPaletteChanged));

		[DynamicDependency(nameof(SetColorPalette))]
		public static ResourceDictionary GetColorPalette(Control obj) => (ResourceDictionary)obj.GetValue(ColorPaletteProperty);

		[DynamicDependency(nameof(GetColorPalette))]
		public static void SetColorPalette(Control obj, ResourceDictionary value) => obj.SetValue(ColorPaletteProperty, value);

		private static void OnColorPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is Control control && e.NewValue is ResourceDictionary rd)
			{
				MaterialBrushes materialBrushes = new()
				{
					ColorOverrideDictionary = rd
				};

				control.Resources = materialBrushes;
			}
		}
			
		#endregion

		#region DependencyProperty: ColorPaletteSource
		public static DependencyProperty ColorPaletteSourceProperty { [DynamicDependency(nameof(GetColorPaletteSource))] get; } = DependencyProperty.RegisterAttached(
			"ColorPaletteSource",
			typeof(string),
			typeof(ThemeExtensions),
			new PropertyMetadata(default, OnColorPaletteSourceChanged));

		[DynamicDependency(nameof(SetColorPaletteSource))]
		public static string GetColorPaletteSource(Control obj) => (string)obj.GetValue(ColorPaletteSourceProperty);

		[DynamicDependency(nameof(GetColorPaletteSource))]
		public static void SetColorPaletteSource(Control obj, string value) => obj.SetValue(ColorPaletteSourceProperty, value);

		private static void OnColorPaletteSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is Control control && e.NewValue is string sourceUri)
			{
				MaterialBrushes materialBrushes = new()
				{
					ColorOverrideSource = sourceUri
				};

				control.Resources = materialBrushes;
			}
		}
		#endregion
	}
}
