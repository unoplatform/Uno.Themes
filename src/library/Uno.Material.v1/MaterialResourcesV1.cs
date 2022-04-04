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
	public sealed class MaterialResourcesV1 : ResourceDictionary
	{
		public MaterialResourcesV1()
		{
			Source = new Uri("ms-appx:///Uno.Material.v1/Generated/mergedpages.xaml");
		}

		public bool WithImplicitStyles { set => ExportImplicitStyles(value); }

		private string[] GetImplicitStyles()
		{
			return new[] {
				// "ms-appx:///Uno.Material/Styles/Controls/Button.xaml",
				"MaterialContainedButtonStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/CalendarView.xaml",
				"MaterialCalendarViewStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/CalendarDatePicker.xaml",
				"MaterialCalendarDatePickerStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/CheckBox.xaml",
				"MaterialCheckBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/ComboBox.xaml",
				"MaterialComboBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/CommandBar.xaml",
				"MaterialCommandBarStyle", "MaterialAppBarButton",
				// "ms-appx:///Uno.Material/Styles/Controls/DatePicker.xaml",
				"MaterialDatePickerStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/FloatingActionButton.xaml", // no implicit style, as the target-type is Button
				// "ms-appx:///Uno.Material/Styles/Controls/Flyout.xaml",
				"MaterialFlyoutPresenterStyle", "MaterialMenuFlyoutPresenterStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/HyperlinkButton.xaml",
				"MaterialHyperlinkButtonStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/InfoBar.xaml",
				"MaterialInfoBarStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/ListView.xaml", // TODO: add implicit styles once tested: MaterialListViewStyle, MaterialListViewItemStyle
				// "ms-appx:///Uno.Material/Styles/Controls/NavigationView/NavigationView_MUX.xaml",
				"MaterialNavigationViewStyle", "MaterialNavigationViewItemStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/PasswordBox.xaml",
				"MaterialFilledPasswordBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/ProgressBar.xaml",
				"MaterialProgressBarStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/RadioButton.xaml",
				"MaterialRadioButtonStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/RatingControl.xaml",
				"MaterialRatingControlStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/Ripple.xaml", // default implicit: (not-keyed)
				// "ms-appx:///Uno.Material/Styles/Controls/Slider.xaml",
				"MaterialSliderStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/TextBox.xaml",
				"MaterialFilledTextBoxStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/TextBlock.xaml",
				"MaterialBody2",
				// "ms-appx:///Uno.Material/Styles/Controls/TimePicker.xaml",
				"MaterialTimePickerStyle",
				// "ms-appx:///Uno.Material/Styles/Controls/ToggleButton.xaml",
				"MaterialTextToggleButtonStyle",
				// $"ms-appx:///Uno.Material/Styles/Controls/ToggleSwitch{MobileSuffix}.xaml",
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
