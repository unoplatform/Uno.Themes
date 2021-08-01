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
			// Add all ResourceDictionaries for Variables here in alphabetical order
			Add("ms-appx:///Uno.Cupertino/Styles/Application/AnimationConstants.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Application/Colors.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Application/Converters.xaml");
			//Add("ms-appx:///Uno.Cupertino/Styles/Application/TextBoxVariables.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Application/StateConstants.xaml");

			// Add all ResourceDictionaries for Controls here in alphabetical order
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/Button.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/CalendarDatePicker.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/CalendarView.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/CheckBox.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ComboBox.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/DatePicker.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/HyperlinkButton.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/NumberBox.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/PasswordBox.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ProgressBar.xaml");
#if !WinUI_Desktop
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ProgressRing.xaml");
#endif
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/RadioButton.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/SlidingSegmentedControl.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/Slider.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/TextBlock.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/TextBox.xaml");
			Add("ms-appx:///Uno.Cupertino/Styles/Controls/ToggleSwitch.xaml");

			void Add(string source) => MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(source) });
		}
	}
}
