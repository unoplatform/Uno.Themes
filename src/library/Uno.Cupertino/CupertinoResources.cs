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
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/AnimationConstants.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/Colors.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/Converters.xaml") });
			//this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/TextBoxVariables.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/StateConstants.xaml") });

			// Add all ResourceDictionaries for Controls here in alphabetical order
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/Button.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/CheckBox.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/ComboBox.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/DatePicker.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/HyperlinkButton.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/NumberBox.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/PasswordBox.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/ProgressBar.xaml") });
#if !WinUI_Desktop
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/ProgressRing.xaml") });
#endif
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/RadioButton.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/Slider.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/TextBlock.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/TextBox.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/ToggleSwitch.xaml") });

		}
	}
}
