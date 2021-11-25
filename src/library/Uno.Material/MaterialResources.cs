using System;
using System.Collections.Generic;
using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Material resources including colors, layout values and styles
	/// </summary>
	public sealed class MaterialResources : ResourceDictionary
	{
		public MaterialResources()
		{
			ImportResourceDictionaries();
		}

		public bool WithImplicitStyles { set => ExportImplicitStyles(value); }

		private IEnumerable<(string Source, string[] ImplicitStyles)> GetResourceInfos()
		{
			var resources = new List<(string Path, string[] ImplicitStyles)>();

			// Add all ResourceDictionaries for Variables here in alphabetical order
			Add("ms-appx:///Uno.Material/Styles/Application/AnimationConstants.xaml");
			Add("ms-appx:///Uno.Material/Styles/Application/Converters.xaml");
			Add("ms-appx:///Uno.Material/Styles/Application/TextBoxVariables.xaml");

			// Add all ResourceDictionaries for Controls here in alphabetical order
			Add("ms-appx:///Uno.Material/Styles/Controls/Button.xaml", "MaterialContainedButtonStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/CalendarView.xaml", "MaterialCalendarViewStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/CalendarDatePicker.xaml", "MaterialCalendarDatePickerStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/CheckBox.xaml", "MaterialCheckBoxStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/ComboBox.xaml", "MaterialComboBoxStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/CommandBar.xaml", "MaterialCommandBarStyle", "MaterialAppBarButton");
			Add("ms-appx:///Uno.Material/Styles/Controls/DatePicker.xaml", "MaterialDatePickerStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/FloatingActionButton.xaml"); // no implicit style, as the target-type is Button
			Add("ms-appx:///Uno.Material/Styles/Controls/Flyout.xaml", "MaterialFlyoutPresenterStyle", "MaterialMenuFlyoutPresenterStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/HyperlinkButton.xaml", "MaterialHyperlinkButtonStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/InfoBar.xaml", "MaterialInfoBarStyle");
#if !WinUI
			Add("ms-appx:///Uno.Material/Styles/Controls/NavigationView/WUX/NavigationView.xaml", "MaterialWUXNavigationViewStyle"); // NavigationView merges it's related dictionaries already
#endif
			Add("ms-appx:///Uno.Material/Styles/Controls/NavigationView/NavigationView_MUX.xaml", "MaterialNavigationViewStyle", "MaterialNavigationViewItemStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/PasswordBox.xaml", "MaterialFilledPasswordBoxStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/ProgressBar.xaml", "MaterialProgressBarStyle");
#if !WinUI_Desktop
			Add("ms-appx:///Uno.Material/Styles/Controls/ProgressRing.xaml", "MaterialProgressRingStyle");
#endif
			Add("ms-appx:///Uno.Material/Styles/Controls/RadioButton.xaml", "MaterialRadioButtonStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/RatingControl.xaml", "MaterialRatingControlStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/Ripple.xaml"); // default implicit: (not-keyed)
			Add("ms-appx:///Uno.Material/Styles/Controls/Slider.xaml", "MaterialSliderStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/TextBox.xaml", "MaterialFilledTextBoxStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/TextBlock.xaml", "MaterialBody2");
			Add("ms-appx:///Uno.Material/Styles/Controls/TimePicker.xaml", "MaterialTimePickerStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/ToggleButton.xaml", "MaterialTextToggleButtonStyle");
			Add("ms-appx:///Uno.Material/Styles/Controls/ToggleSwitch.xaml", "MaterialToggleSwitchStyle");

			return resources;

			void Add(string source, params string[] implicitStyles) => resources.Add((source, implicitStyles));
		}

		private void ImportResourceDictionaries()
		{
			foreach (var info in GetResourceInfos())
			{
				MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(info.Source) });
			}
		}

		private void ExportImplicitStyles(bool value)
		{
			if (!value) return; // we don't support teardown

			foreach (var info in GetResourceInfos())
			{
				foreach (var key in info.ImplicitStyles ?? Array.Empty<string>())
				{
					if (!this.TryGetValue(key, out var resource) || !(resource is Style style))
					{
						throw new ArgumentException($"Missing resource: key={key} from={info.Source}");
					}
					if (style.TargetType == null)
					{
						throw new InvalidOperationException($"Missing TargetType on style: key={key}");
					}
					
					this.Add(style.TargetType, style);
				}
			}
		}
	}
}
