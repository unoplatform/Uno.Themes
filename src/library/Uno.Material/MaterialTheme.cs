using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Material.Helpers;

#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Material Theme resources including colors, fonts, layout values, and styles
	/// </summary>
	public class MaterialTheme : ResourceDictionary
	{
		private bool _isColorOverrideMuted;
		private bool _isFontOverrideMuted;
		private ResourceDictionary _implicitStyles;
		private ResourceDictionary _colors;
		private ResourceDictionary _fonts;

		#region FontOverrideSource (DP)
		/// <summary>
		/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
		/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
		/// </summary>
		public string FontOverrideSource
		{
			get => (string)GetValue(FontOverrideSourceProperty);
			set => SetValue(FontOverrideSourceProperty, value);
		}

		public static DependencyProperty FontOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(FontOverrideSource),
				typeof(string),
				typeof(MaterialTheme),
				new PropertyMetadata(null, OnFontOverrideSourceChanged));

		private static void OnFontOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialTheme theme && e.NewValue is string sourceUri)
			{
				theme.FontOverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
			}
		}
		#endregion

		#region ColorOverrideSource (DP)
		/// <summary>
		/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
		/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
		/// </summary>
		/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
		public string ColorOverrideSource
		{
			get => (string)GetValue(ColorOverrideSourceProperty);
			set => SetValue(ColorOverrideSourceProperty, value);
		}

		public static DependencyProperty ColorOverrideSourceProperty { get; } =
			DependencyProperty.Register(
				nameof(ColorOverrideSource),
				typeof(string),
				typeof(MaterialTheme),
				new PropertyMetadata(null, OnColorOverrideSourceChanged));

		private static void OnColorOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialTheme theme && e.NewValue is string sourceUri)
			{
				theme.ColorOverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
			}
		}
		#endregion

		#region FontOverrideDictionary (DP)
		/// <summary>
		/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
		/// </summary>
		public ResourceDictionary FontOverrideDictionary
		{
			get => (ResourceDictionary)GetValue(FontOverrideDictionaryProperty);
			set => SetValue(FontOverrideDictionaryProperty, value);
		}

		public static DependencyProperty FontOverrideDictionaryProperty { get; } =
			DependencyProperty.Register(
				nameof(FontOverrideDictionary),
				typeof(ResourceDictionary),
				typeof(MaterialTheme),
				new PropertyMetadata(null, OnFontOverrideChanged));

		private static void OnFontOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialTheme theme)
			{
				theme._fonts.SafeMerge(e.NewValue as ResourceDictionary);
			}
		}
		#endregion

		#region ColorOverrideDictionary (DP)
		/// <summary>
		/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
		/// </summary>
		/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
		public ResourceDictionary ColorOverrideDictionary
		{
			get => (ResourceDictionary)GetValue(ColorOverrideDictionaryProperty);
			set => SetValue(ColorOverrideDictionaryProperty, value);
		}

		public static DependencyProperty ColorOverrideDictionaryProperty { get; } =
			DependencyProperty.Register(
				nameof(ColorOverrideDictionary),
				typeof(ResourceDictionary),
				typeof(MaterialTheme),
				new PropertyMetadata(null, OnColorOverrideChanged));

		private static void OnColorOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialTheme theme)
			{
				theme._colors.SafeMerge(e.NewValue as ResourceDictionary);
			}
		}
		#endregion

		#region UseImplicitStyles (DP)
		/// <summary>
		/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
		/// </summary>
		/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
		public bool UseImplicitStyles
		{
			get => (bool)GetValue(UseImplicitStylesProperty);
			set => SetValue(UseImplicitStylesProperty, value);
		}

		public static DependencyProperty UseImplicitStylesProperty { get; } =
			DependencyProperty.Register(
				nameof(UseImplicitStyles),
				typeof(bool),
				typeof(MaterialTheme),
				new PropertyMetadata(true, UseImplicitStylesChanged));

		private static void UseImplicitStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialTheme theme && e.NewValue is bool useImplicit && !useImplicit)
			{
				theme._implicitStyles?.Clear();
			}
		}
		#endregion

		public MaterialTheme()
		{
			var mergedPages = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml") };

			_colors = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColors.xaml") };
			_colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColorPalette.xaml") });

			_fonts = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/Common/Fonts.xaml") };

			mergedPages.MergedDictionaries.Add(_colors);
			mergedPages.MergedDictionaries.Add(_fonts);

			MergedDictionaries.Add(mergedPages);

			_implicitStyles = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Controls/v2/_ImplicitStyles.xaml") };
			MergedDictionaries.Add(_implicitStyles);
		}

		public MaterialTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
		{
			if (colorOverride is { })
			{
				ColorOverrideDictionary = colorOverride;
			}

			if (fontOverride is { })
			{
				FontOverrideDictionary = fontOverride;
			}
		}
	}
}
