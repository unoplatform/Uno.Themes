using System;
using System.Collections.Generic;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino;

/// <summary>
/// The frozen pre-<see cref="CupertinoTheme"/> Cupertino resources (colors, layout values and styles): a drop-in
/// replacement for <see cref="CupertinoResources"/> in an app pinned to the previous look.
/// </summary>
[Obsolete("Frozen pre-CupertinoTheme resources kept as a migration escape hatch. Use CupertinoTheme. This type will be removed in the next major version.")]
public sealed class CupertinoResourcesV1 : ResourceDictionary
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CupertinoResourcesV1"/> class.
	/// </summary>
	public CupertinoResourcesV1()
	{
		MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(Themes.ThemesConstants.ConverterResourcePath) });
		ImportResourceDictionaries();
	}

	/// <summary>
	/// Sets whether the Cupertino styles are also exported as the implicit styles of their target types.
	/// Setting it back to <see langword="false"/> has no effect.
	/// </summary>
	public bool WithImplicitStyles { set => ExportImplicitStyles(value); }

	private IEnumerable<(string Source, string[] ImplicitStyles)> GetResourceInfos()
	{
		var resources = new List<(string Path, string[] ImplicitStyles)>();

		var implicitStyles = new[] {
			// Add all ResourceDictionaries for Controls here in alphabetical order
			// "ms-appx:///Uno.Cupertino/Styles/Controls/Button.xaml",
			"CupertinoButtonStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/CalendarDatePicker.xaml",
			"CupertinoCalendarDatePickerStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/CalendarView.xaml",
			"CupertinoCalendarViewStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/CheckBox.xaml",
			"CupertinoCheckBoxStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/ComboBox.xaml",
			"CupertinoComboBoxStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/DatePicker.xaml",
			"CupertinoDatePickerStyle", "CupertinoDatePickerFlyoutPresenterStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/HyperlinkButton.xaml",
			"CupertinoHyperlinkButtonStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/NumberBox.xaml",
			"CupertinoNumberBoxStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/PasswordBox.xaml",
			"CupertinoPasswordBoxStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/ProgressBar.xaml",
			"CupertinoProgressBarStyle",
#if !WinUI_Desktop
			// "ms-appx:///Uno.Cupertino/Styles/Controls/ProgressRing.xaml",
			"CupertinoProgressRingStyle",
#endif
			// "ms-appx:///Uno.Cupertino/Styles/Controls/RadioButton.xaml",
			"CupertinoRadioButtonStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/Slider.xaml",
			"CupertinoSliderStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/TextBlock.xaml",
			"CupertinoBody",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/TextBox.xaml",
			"CupertinoTextBoxStyle",
			// "ms-appx:///Uno.Cupertino/Styles/Controls/ToggleSwitch.xaml",
			"CupertinoToggleSwitchStyle",
		};

		Add(CupertinoConstants.V1.MergedPages, implicitStyles);

		return resources;

		void Add(string source, params string[] styles) => resources.Add((source, styles));
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

		var implicitResources = new ResourceDictionary();
		foreach (var info in GetResourceInfos())
		{
			foreach (var key in info.ImplicitStyles ?? Array.Empty<string>())
			{
				if (!this.TryGetValue(key, out var resource) || !(resource is Style style))
				{
					// uwp: If the {key} style is clearly defined in {info.Source}, but we can't find it here.
					// And, that it only happens on uwp, and not other uno platforms.
					// It means that the style references resources that are not directly included.
					// This can usually be fixed by including `<CupertinoColorsV1 xmlns="using:Uno.Cupertino" />` in the MergedDictionaries of {info.Source}.
					// note: Resources used on Style.Setters need to be directly defined/included, those used in Style.Template dont have to be.
					throw new ArgumentException($"Missing resource: key={key} from={info.Source}");
				}
				if (style.TargetType == null)
				{
					throw new InvalidOperationException($"Missing TargetType on style: key={key}");
				}

				implicitResources.Add(style.TargetType, style);
			}
		}

		// UWP don't allow for res-dict with Source set to contain resource directly:
		// > Local values are not allowed in resource dictionary with Source set
		// but, we can add them through merged-dict instead.
		this.MergedDictionaries.Add(implicitResources);
	}
}
