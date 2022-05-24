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

			foreach (var (resKey, sharedKey, isDefaultStyle) in GetStyleInfos())
			{
				var style = GetStyle(resKey);

				if (isDefaultStyle)
				{
					implicitResources.Add(style.TargetType, style);
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

		private IEnumerable<(string ResourceKey, string SharedKey, bool IsDefaultStyle)> GetStyleInfos()
		{
			var result = new List<(string ResourceKey, string SharedKey, bool IsDefaultStyle)>();

			Add("MaterialAppBarButtonStyle", isImplicit: true);
			Add("MaterialBodyLarge");
			Add("MaterialBodyMedium", isImplicit: true);
			Add("MaterialBodySmall");
			Add("MaterialCalendarDatePickerStyle", isImplicit: true);
			Add("MaterialCalendarViewStyle", isImplicit: true);
			Add("MaterialCheckBoxStyle", isImplicit: true);
			Add("MaterialComboBoxStyle", isImplicit: true);
			Add("MaterialCommandBarStyle", isImplicit: true);
			Add("MaterialDatePickerStyle", isImplicit: true);
			Add("MaterialDisplayLarge");
			Add("MaterialDisplayMedium");
			Add("MaterialDisplaySmall");
			Add("MaterialElevatedButtonStyle");
			Add("MaterialExtendedFabStyle");
			Add("MaterialFabStyle");
			Add("MaterialFilledButtonStyle", isImplicit: true);
			Add("MaterialFilledPasswordBoxStyle", isImplicit: true);
			Add("MaterialFilledTextBoxStyle", isImplicit: true);
			Add("MaterialFilledTonalButtonStyle");
			Add("MaterialFlyoutPresenterStyle", isImplicit: true);
			Add("MaterialHeadlineLarge");
			Add("MaterialHeadlineMedium");
			Add("MaterialHeadlineSmall");
			Add("MaterialHyperlinkButtonStyle", isImplicit: true);
			Add("MaterialIconButtonStyle");
			Add("MaterialIconToggleButtonStyle");
			Add("MaterialLabelLarge");
			Add("MaterialLabelMedium");
			Add("MaterialLabelSmall");
			Add("MaterialLargeFabStyle");
			Add("MaterialListViewItemStyle", isImplicit: true);
			Add("MaterialListViewStyle", isImplicit: true);
			Add("MaterialMenuFlyoutPresenterStyle", isImplicit: true);
			Add("MaterialNavigationViewItemStyle", isImplicit: true);
			Add("MaterialNavigationViewStyle", isImplicit: true);
			Add("MaterialOutlinedButtonStyle");
			Add("MaterialOutlinedPasswordBoxStyle");
			Add("MaterialOutlinedTextBoxStyle");
			Add("MaterialProgressBarStyle", isImplicit: true);
			Add("MaterialProgressRingStyle", isImplicit: true);
			Add("MaterialRadioButtonStyle", isImplicit: true);
			Add("MaterialSecondaryCheckBoxStyle");
			Add("MaterialSecondaryExtendedFabStyle");
			Add("MaterialSecondaryFabStyle");
			Add("MaterialSecondaryHyperlinkButtonStyle");
			Add("MaterialSecondaryLargeFabStyle");
			Add("MaterialSecondaryRadioButtonStyle");
			Add("MaterialSecondarySmallFabStyle");
			Add("MaterialSliderStyle", isImplicit: true);
			Add("MaterialSmallFabStyle");
			Add("MaterialSurfaceExtendedFabStyle");
			Add("MaterialSurfaceFabStyle");
			Add("MaterialSurfaceLargeFabStyle");
			Add("MaterialSurfaceSmallFabStyle");
			Add("MaterialTertiaryExtendedFabStyle");
			Add("MaterialTertiaryFabStyle");
			Add("MaterialTertiaryLargeFabStyle");
			Add("MaterialTertiarySmallFabStyle");
			Add("MaterialTextButtonStyle");
			Add("MaterialTextToggleButtonStyle", isImplicit: true);
			Add("MaterialTitleLarge");
			Add("MaterialTitleMedium");
			Add("MaterialTitleSmall");
			Add("MaterialToggleSwitchStyle", isImplicit: true);

			return result;

			void Add(string key, string alias = null, bool isImplicit = false) =>
				result.Add((key, alias ?? key.Substring(StylePrefix.Length), isImplicit));
		}
	}
}
