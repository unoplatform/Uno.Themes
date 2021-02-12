using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Uno.Themes.Samples.Entities;


namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "ComboBox", Description = "This control used for selection is a drop-down list that shows its selection in a box.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.combobox")]
	public sealed partial class ComboBoxSamplePage : Page
	{
		public ComboBoxSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
