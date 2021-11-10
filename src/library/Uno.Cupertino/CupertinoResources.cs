using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Cupertino
{
	/// <summary>
	/// Cupertino resources including colors, layout values and styles
	/// </summary>
	public sealed class CupertinoResources : ResourceDictionary
	{
		public CupertinoResources()
		{
			ImportResourceDictionaries();
		}

		public bool WithImplicitStyles { set => ExportImplicitStyles(value); }

		private IEnumerable<(string Source, string[] ImplicitStyles)> GetResourceInfos()
		{
			var resources = new List<(string Path, string[] ImplicitStyles)>();
			
			// Add all ResourceDictionaries for Variables here in alphabetical order
			Add("ms-appx:///Uno.Cupertino/Styles/Application/AnimationConstants.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Application/Colors.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Application/Converters.xaml");
			//Add("ms-appx:///Uno.Cupertino/Styles/Application/TextBoxVariables.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Application/StateConstants.xaml");

			// Add all ResourceDictionaries for Controls here in alphabetical order
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/Button.xaml", "CupertinoButtonStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/CalendarDatePicker.xaml", "CupertinoCalendarDatePickerStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/CalendarView.xaml", "CupertinoCalendarViewStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/CheckBox.xaml", "CupertinoCheckBoxStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ComboBox.xaml", "CupertinoComboBoxStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/DatePicker.xaml", "CupertinoDatePickerStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/HyperlinkButton.xaml", "CupertinoHyperlinkButtonStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/NumberBox.xaml", "CupertinoNumberBoxStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/PasswordBox.xaml", "CupertinoPasswordBoxStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ProgressBar.xaml", "CupertinoProgressBarStyle");
#if !WinUI_Desktop
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ProgressRing.xaml", "CupertinoProgressRingStyle");
#endif
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/RadioButton.xaml", "CupertinoRadioButtonStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/Slider.xaml", "CupertinoSliderStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/TextBlock.xaml", "CupertinoBody");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/TextBox.xaml", "CupertinoTextBoxStyle");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ToggleSwitch.xaml", "CupertinoToggleSwitchStyle");

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
						throw new ArgumentException($"Missing resource: key={key}");
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
