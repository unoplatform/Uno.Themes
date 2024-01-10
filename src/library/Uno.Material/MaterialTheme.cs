using System;
using Uno.Themes;


#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI;
using Windows.UI.Xaml;
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
			if (d is MaterialTheme { _isFontOverrideMuted: false } theme)
			{
				theme.UpdateSource();
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
			if (d is MaterialTheme { _isColorOverrideMuted: false } theme)
			{
				theme.UpdateSource();
			}
		}
		#endregion

		public MaterialTheme() : this(colorOverride: null, fontOverride: null)
		{

		}

		public MaterialTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
		{
			if (colorOverride is { })
			{
				SetColorOverrideSilently(colorOverride);
			}

			if (fontOverride is { })
			{
				SetFontOverrideSilently(fontOverride);
			}

			UpdateSource();
		}

		private void SetColorOverrideSilently(ResourceDictionary colorOverride)
		{
			try
			{
				_isColorOverrideMuted = true;
				ColorOverrideDictionary = colorOverride;
			}
			finally
			{
				_isColorOverrideMuted = false;
			}
		}

		private void SetFontOverrideSilently(ResourceDictionary fontOverride)
		{
			try
			{
				_isFontOverrideMuted = true;
				FontOverrideDictionary = fontOverride;
			}
			finally
			{
				_isFontOverrideMuted = false;
			}
		}

		private void UpdateSource()
		{
#if !HAS_UNO
			Source = null;
#endif
			ThemeDictionaries.Clear();
			MergedDictionaries.Clear();
			this.Clear();

			var colors = new ResourceDictionary { Source = new Uri(Constants.SharedColorsResourcePath) };

			colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(Constants.SharedColorPaletteResourcePath) });

			if (ColorOverrideDictionary is { } colorOverride)
			{
				colors.SafeMerge(colorOverride);
			}

			var mergedPages = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml") };

			var fonts = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/Common/Fonts.xaml") };

			if (FontOverrideDictionary is { } fontOverride)
			{
				fonts.SafeMerge(fontOverride);
			}

			mergedPages.MergedDictionaries.Add(colors);
			mergedPages.MergedDictionaries.Add(fonts);

			MergedDictionaries.Add(mergedPages);
		}
	}
}
