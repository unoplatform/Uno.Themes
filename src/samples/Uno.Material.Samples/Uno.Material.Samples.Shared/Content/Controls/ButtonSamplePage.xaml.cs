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
using Uno.Material.Samples.Entities;


namespace Uno.Material.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "Button", Description = "A button is used to interpret a user click or tap interaction. They're often bound to commands.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/buttons")]
	public sealed partial class ButtonSamplePage : Page
	{
		public ButtonSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
