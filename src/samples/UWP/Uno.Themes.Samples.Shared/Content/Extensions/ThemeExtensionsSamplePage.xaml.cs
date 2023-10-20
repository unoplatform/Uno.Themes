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

namespace Uno.Themes.Samples.Shared.Content.Extensions
{
	[SamplePage(SampleCategory.Helpers, "ThemeExtensions", IconPath = Icons.Helpers.ThemeExtensions)]
	public sealed partial class ThemeExtensionsSamplePage : Page
	{
		public ThemeExtensionsSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
