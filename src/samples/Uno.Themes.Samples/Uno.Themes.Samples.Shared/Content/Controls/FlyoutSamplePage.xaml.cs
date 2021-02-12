using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Themes.Samples.Entities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "Flyout", Description = "A flyout is a UI container that can be light dismissed. It can contain other flyouts or context menus to create a nested experience.", DocumentationLink = "https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/dialogs-and-flyouts/flyouts")]
	public sealed partial class FlyoutSamplePage : Page
	{
		public FlyoutSamplePage()
		{
			this.InitializeComponent();
		}
	}
}

