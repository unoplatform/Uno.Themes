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
			Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v1.xaml");
		}

		public bool WithImplicitStyles { set => ExportImplicitStyles(value); }

		private string[] GetImplicitStyles()
		{
			return new[] {
				// "ms-appx:///Uno.Material/Styles/Controls/v1/Button.xaml",
				"MaterialContainedButtonStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/CalendarView.xaml",
				"MaterialCalendarViewStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/CalendarDatePicker.xaml",
				"MaterialCalendarDatePickerStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/CheckBox.xaml",
				"MaterialCheckBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/ComboBox.xaml",
				"MaterialComboBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/CommandBar.xaml",
				"MaterialCommandBarStyle", "MaterialAppBarButton",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/DatePicker.xaml",
				"MaterialDatePickerStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/FloatingActionButton.xaml", // no implicit style, as the target-type is Button
				// "ms-appx:///Uno.Material/Styles/Controls/v1/Flyout.xaml",
				"MaterialFlyoutPresenterStyle", "MaterialMenuFlyoutPresenterStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/HyperlinkButton.xaml",
				"MaterialHyperlinkButtonStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/InfoBar.xaml",
				"MaterialInfoBarStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/ListView.xaml", // TODO: add implicit styles once tested: MaterialListViewStyle, MaterialListViewItemStyle
				// "ms-appx:///Uno.Material/Styles/Controls/v1/NavigationView/NavigationView_MUX.xaml",
				"MaterialNavigationViewStyle", "MaterialNavigationViewItemStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/PasswordBox.xaml",
				"MaterialFilledPasswordBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/ProgressBar.xaml",
				"MaterialProgressBarStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/RadioButton.xaml",
				"MaterialRadioButtonStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/RatingControl.xaml",
				"MaterialRatingControlStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/Ripple.xaml", // default implicit: (not-keyed)
				// "ms-appx:///Uno.Material/Styles/Controls/v1/Slider.xaml",
				"MaterialSliderStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/TextBox.xaml",
				"MaterialFilledTextBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/TextBlock.xaml",
				"MaterialBody2",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/TimePicker.xaml",
				"MaterialTimePickerStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/v1/ToggleButton.xaml",
				"MaterialTextToggleButtonStyle",
				// $"ms-appx:///Uno.Material/Styles/Controls/v1/ToggleSwitch{MobileSuffix}.xaml",
				"MaterialToggleSwitchStyle",
			};
		}


		private void ExportImplicitStyles(bool value)
		{
			if (!value) return; // we don't support teardown

			var implicitResources = new ResourceDictionary();
			foreach (var key in GetImplicitStyles())
			{
				if (!this.TryGetValue(key, out var resource) || !(resource is Style style))
				{
					// uwp: If the {key} style is clearly defined in {info.Source}, but we can't find it here.
					// And, that it only happens on uwp, and not other uno platforms.
					// It means that the style references resources that are not directly included.
					// This can usually be fixed by including `<MaterialColors xmlns="using:Uno.Material" />` in the MergedDictionaries of {info.Source}.
					// note: Resources used on Style.Setters need to be directly defined/included, those used in Style.Template dont have to be.
					throw new ArgumentException($"Missing resource: key={key}");
				}
				if (style.TargetType == null)
				{
					throw new InvalidOperationException($"Missing TargetType on style: key={key}");
				}

				implicitResources.Add(style.TargetType, style);
			}

			// UWP don't allow for res-dict with Source set to contain resource directly:
			// > Local values are not allowed in resource dictionary with Source set
			// but, we can add them through merged-dict instead.
			this.MergedDictionaries.Add(implicitResources);
		}
	}
}
