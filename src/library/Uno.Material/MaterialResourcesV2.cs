using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Material.Helpers;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Material resources including colors, layout values and styles
	/// </summary>
	public class MaterialResourcesV2 : ResourceDictionary
	{
		public string FontOverrideSource
		{
			get => (string)GetValue(FontOverrideSourceProperty);
			set => SetValue(FontOverrideSourceProperty, value);
		}

		public static DependencyProperty FontOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(FontOverrideSource),
				typeof(string),
				typeof(MaterialResourcesV2),
				new PropertyMetadata(null, OnDependencyPropertyChanged));

		public string ColorOverrideSource
		{
			get => (string)GetValue(ColorOverrideSourceProperty);
			set => SetValue(ColorOverrideSourceProperty, value);
		}

		public static DependencyProperty ColorOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(ColorOverrideSource),
				typeof(string),
				typeof(MaterialResourcesV2),
				new PropertyMetadata(null, OnDependencyPropertyChanged));

		private static void OnDependencyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (dependencyObject is not MaterialResourcesV2 resources)
			{
				return;
			}

			resources.OnPropertyChanged(args);
		}

		public MaterialResourcesV2()
		{
#if !__NETSTD_REFERENCE__

			Uno.Material.GlobalStaticResources.Initialize();
			Uno.Material.GlobalStaticResources.RegisterDefaultStyles();
			Uno.Material.GlobalStaticResources.RegisterResourceDictionariesBySource();
#endif
			UpdateSource();
		}

		public MaterialResourcesV2(ResourceDictionary fontOverride = null, ResourceDictionary colorOverride = null)
		{
			//_colorOverride = colorOverride;
			//_fontOverride = fontOverride;

			UpdateSource();
		}

		internal void OnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (args.Property == ColorOverrideSourceProperty)
			{
				MaterialColorsV2.ColorOverrideSource = args.NewValue as string;
			}
			else if (args.Property == FontOverrideSourceProperty)
			{
				MaterialFonts.FontOverrideSource = args.NewValue as string;
			}

			UpdateSource();
		}

		private void UpdateSource()
		{
			ThemeDictionaries?.Clear();
			Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml");
		}
	}
}
