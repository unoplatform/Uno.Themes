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
		private const string StylePrefix = "Material";
		public MaterialResourcesV2()
		{
			Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml");

			MapStyleInfo();
		}

		private void MapStyleInfo()
		{
			var aliasedResources = new ResourceDictionary();
			var implicitResources = new ResourceDictionary();

			foreach (var (resKey, sharedKey, implicitTargetType) in GetStyleInfos())
			{
				var style = DictionaryHelper.CreateLazyResource(() =>
				{
					var s = GetStyle(resKey);
#if DEBUG
					if(implicitTargetType != null && s.TargetType != implicitTargetType)
					{
						throw new InvalidOperationException($"The target type for {resKey} does not match (Expected {implicitTargetType}, got {s.TargetType})");
					}
#endif
					return s;
				});

				if (implicitTargetType != null)
				{
					implicitResources.Add(implicitTargetType, style);
				}

				aliasedResources.Add(sharedKey, style);
			}

			// UWP don't allow for res-dict with Source set to contain resource directly:
			// > Local values are not allowed in resource dictionary with Source set
			// but, we can add them through merged-dict instead.
			this.MergedDictionaries.Add(aliasedResources);
			this.MergedDictionaries.Add(implicitResources);
		}

		private Style GetStyle(string key)
		{
			if (!this.TryGetValue(key, out var resource) || !(resource is Style style))
			{
				// uwp: If the {key} style is clearly defined, but we can't find it here.
				// And, that it only happens on uwp, and not other uno platforms.
				// It means that the style references resources that are not directly included.
				// note: Resources used on Style.Setters need to be directly defined/included, those used in Style.Template dont have to be.
				throw new ArgumentException($"Missing resource: key={key}");
			}
			if (style.TargetType == null)
			{
				throw new InvalidOperationException($"Missing TargetType on style: key={key}");
			}

			return style;
		}

		private IEnumerable<(string ResourceKey, string SharedKey, Type ImplicitTargetType)> GetStyleInfos()
		{
			var result = new List<(string ResourceKey, string SharedKey, Type ImplicitTargetType)>();

			Add("MaterialAppBarButtonStyle", implicitTargetType: typeof(AppBarButton));
			Add("MaterialBodyLarge");
			Add("MaterialBodyMedium", implicitTargetType: typeof(TextBlock));
			Add("MaterialBodySmall");
			Add("MaterialCalendarDatePickerStyle", implicitTargetType: typeof(CalendarDatePicker));
			Add("MaterialCalendarViewStyle", implicitTargetType: typeof(CalendarView));
			Add("MaterialCaptionLarge");
			Add("MaterialCaptionMedium");
			Add("MaterialCaptionSmall");
			Add("MaterialCheckBoxStyle", implicitTargetType: typeof(CheckBox));
			Add("MaterialContentDialogStyle", implicitTargetType: typeof(ContentDialog));
			Add("MaterialComboBoxStyle", implicitTargetType: typeof(ComboBox));
			Add("MaterialCommandBarStyle", implicitTargetType: typeof(CommandBar));
			Add("MaterialDatePickerStyle", implicitTargetType: typeof(DatePicker));
			Add("MaterialDisplayLarge");
			Add("MaterialDisplayMedium");
			Add("MaterialDisplaySmall");
			Add("MaterialElevatedButtonStyle");
			Add("MaterialFabStyle");
			Add("MaterialFilledButtonStyle", implicitTargetType: typeof(Button));
			Add("MaterialFilledPasswordBoxStyle", implicitTargetType: typeof(PasswordBox));
			Add("MaterialFilledTextBoxStyle", implicitTargetType: typeof(TextBox));
			Add("MaterialFilledTonalButtonStyle");
			Add("MaterialFlyoutPresenterStyle", implicitTargetType: typeof(FlyoutPresenter));
			Add("MaterialHeadlineLarge");
			Add("MaterialHeadlineMedium");
			Add("MaterialHeadlineSmall");
			Add("MaterialHyperlinkButtonStyle", implicitTargetType: typeof(HyperlinkButton));
			Add("MaterialIconButtonStyle");
			Add("MaterialIconToggleButtonStyle");
			Add("MaterialLabelExtraSmall");
			Add("MaterialLabelLarge");
			Add("MaterialLabelMedium");
			Add("MaterialLabelSmall");
			Add("MaterialLargeFabStyle");
			Add("MaterialListViewItemStyle", implicitTargetType: typeof(ListViewItem));
			Add("MaterialListViewStyle", implicitTargetType: typeof(ListView));
			Add("MaterialMenuFlyoutPresenterStyle", implicitTargetType: typeof(MenuFlyoutPresenter));
			Add("MaterialNavigationViewStyle", implicitTargetType: typeof(Microsoft.UI.Xaml.Controls.NavigationView));
			Add("MaterialNavigationViewItemStyle", implicitTargetType: typeof(Microsoft.UI.Xaml.Controls.NavigationViewItem));
			Add("MaterialOutlinedButtonStyle");
			Add("MaterialOutlinedPasswordBoxStyle");
			Add("MaterialOutlinedTextBoxStyle");
			Add("MaterialProgressBarStyle", implicitTargetType: typeof(Microsoft.UI.Xaml.Controls.ProgressBar));
			Add("MaterialProgressRingStyle", implicitTargetType: typeof(Microsoft.UI.Xaml.Controls.ProgressRing));
			Add("MaterialRadioButtonStyle", implicitTargetType: typeof(RadioButton));
			Add("MaterialSecondaryCheckBoxStyle");
			Add("MaterialSecondaryFabStyle");
			Add("MaterialSecondaryHyperlinkButtonStyle");
			Add("MaterialSecondaryLargeFabStyle");
			Add("MaterialSecondaryRadioButtonStyle");
			Add("MaterialSecondarySmallFabStyle");
			Add("MaterialSliderStyle", implicitTargetType: typeof(Slider));
			Add("MaterialSmallFabStyle");
			Add("MaterialSurfaceFabStyle");
			Add("MaterialSurfaceLargeFabStyle");
			Add("MaterialSurfaceSmallFabStyle");
			Add("MaterialTertiaryFabStyle");
			Add("MaterialTertiaryLargeFabStyle");
			Add("MaterialTertiarySmallFabStyle");
			Add("MaterialTextButtonStyle");
			Add("MaterialTextToggleButtonStyle", implicitTargetType: typeof(ToggleButton));
			Add("MaterialTitleLarge");
			Add("MaterialTitleMedium");
			Add("MaterialTitleSmall");
			Add("MaterialToggleSwitchStyle", implicitTargetType: typeof(ToggleSwitch));

			return result;

			void Add(string key, string alias = null, Type implicitTargetType = null) =>
				result.Add((key, alias ?? key.Substring(StylePrefix.Length), implicitTargetType));
		}
	}
}
