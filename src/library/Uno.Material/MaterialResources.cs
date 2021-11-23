using System;
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
			UpdateResources();
		}

		private void UpdateResources()
		{
			// Add all ResourceDictionaries for Variables here in alphabetical order
			Add("ms-appx:///Uno.Material/Styles/Application/AnimationConstants.xaml");
			Add("ms-appx:///Uno.Material/Styles/Application/Converters.xaml");
			Add("ms-appx:///Uno.Material/Styles/Application/TextBoxVariables.xaml");

			// Add all ResourceDictionaries for Controls here in alphabetical order
			Add("ms-appx:///Uno.Material/Styles/Controls/BottomNavigationBar.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/BottomNavigationBarItem.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/Button.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/Card.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/CalendarView.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/CalendarDatePicker.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/CheckBox.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/ComboBox.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/CommandBar.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/DatePicker.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/ExpandingBottomSheet.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/FloatingActionButton.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/Flyout.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/HyperlinkButton.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/InfoBar.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/ListView.xaml");
#if !WinUI
			Add("ms-appx:///Uno.Material/Styles/Controls/NavigationView/WUX/NavigationView.xaml"); // NavigationView merges it's related dictionaries already
#endif
			Add("ms-appx:///Uno.Material/Styles/Controls/NavigationView/NavigationView_MUX.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/PasswordBox.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/ProgressBar.xaml");
#if !WinUI_Desktop
			Add("ms-appx:///Uno.Material/Styles/Controls/ProgressRing.xaml");
#endif
			Add("ms-appx:///Uno.Material/Styles/Controls/RadioButton.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/RatingControl.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/Ripple.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/SnackBar.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/Slider.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/StandardBottomSheet.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/TextBox.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/TextBlock.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/TimePicker.xaml");
			Add("ms-appx:///Uno.Material/Styles/Controls/ToggleButton.xaml");
#if __ANDROID__ || __IOS__
			Add("ms-appx:///Uno.Material/Styles/Controls/ToggleSwitch.Mobile.xaml");
#else
			Add("ms-appx:///Uno.Material/Styles/Controls/ToggleSwitch.xaml");
#endif

			void Add(string source) => MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(source) });
		}
	}
}
