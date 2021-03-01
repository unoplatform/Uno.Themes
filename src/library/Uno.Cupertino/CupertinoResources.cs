using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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
			//this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/AnimationConstants.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/Colors.xaml") });
			//this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/Converters.xaml") });
			//this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Application/TextBoxVariables.xaml") });

			// Add all ResourceDictionaries for Controls here in alphabetical order
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/Button.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/TextBlock.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/TextBox.xaml") });
			this.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("ms-appx:///Uno.Cupertino/Styles/Controls/ToggleSwitch.xaml") });

		}
	}
}
