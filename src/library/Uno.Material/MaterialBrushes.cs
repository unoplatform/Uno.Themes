using System;

#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#endif

namespace Uno.Material
{
	public class MaterialBrushes : ResourceDictionary
	{
		private bool _isColorOverrideMuted;

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
				typeof(MaterialBrushes),
				new PropertyMetadata(null, OnColorOverrideSourceChanged));

		private static void OnColorOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialBrushes theme && e.NewValue is string sourceUri)
			{
				theme.ColorOverrideDictionary = new ResourceDictionary() { Source = new Uri(sourceUri) };
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
				typeof(MaterialBrushes),
				new PropertyMetadata(null, OnColorOverrideChanged));

		private static void OnColorOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialBrushes { _isColorOverrideMuted: false } theme)
			{
				theme.UpdateSource();
			}
		}

		#endregion

		#region Primary (DP)
		/// <summary>
		/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
		/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
		/// </summary>
		/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
		public Color Primary
		{
			get => (Color)GetValue(PrimaryProperty);
			set => SetValue(PrimaryProperty, value);
		}

		public static DependencyProperty PrimaryProperty { get; } =
			DependencyProperty.Register(
				nameof(Primary),
				typeof(Color),
				typeof(MaterialBrushes),
				new PropertyMetadata(null, OnPrimaryChanged));

		private static void OnPrimaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is MaterialBrushes { _isColorOverrideMuted: false } theme)
			{
				theme.UpdatePrimary();
			}
		}
		#endregion

		public MaterialBrushes() : this(colorOverride: null)
		{
			
		}

		public MaterialBrushes(ResourceDictionary colorOverride = null)
		{
			if (colorOverride is { })
			{
				SetColorOverrideSilently(colorOverride);
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

		public void UpdateSource()
		{
#if !HAS_UNO
			Source = null;
#endif
			ThemeDictionaries.Clear();
			MergedDictionaries.Clear();
			this.Clear();

			var colors = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColors.xaml") };

			colors.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Styles/Application/v2/SharedColorPalette.xaml") });

			if (ColorOverrideDictionary is { } colorOverride)
			{
				colors.SafeMerge(colorOverride);
			}

			var mergedPages = new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml") };

			mergedPages.MergedDictionaries.Add(colors);

			MergedDictionaries.Add(mergedPages);
		}

		private void UpdatePrimary()
		{
			var resourceDictionary = new ResourceDictionary();

			var lightThemeDictionary = new ResourceDictionary
			{
				["PrimaryColor"] = new SolidColorBrush(Primary)
			};

			var darkThemeDictionary = new ResourceDictionary
			{
				["PrimaryColor"] = new SolidColorBrush(Primary)
			};

			resourceDictionary.ThemeDictionaries["Light"] = lightThemeDictionary;
			resourceDictionary.ThemeDictionaries["Dark"] = darkThemeDictionary;

			ColorOverrideDictionary = resourceDictionary;
		}
	}
}
