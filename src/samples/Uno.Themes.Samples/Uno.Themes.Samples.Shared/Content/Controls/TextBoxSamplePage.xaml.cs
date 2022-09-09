using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Uno.Themes.Samples.Entities;
using Uno.Themes.Samples.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.FormattableString;
using static Uno.Themes.Samples.ViewModels.ViewModelBase;

namespace Uno.Themes.Samples.Content.Controls
{
	[SamplePage(SampleCategory.Controls, "TextBox", Description = "This control allows users to input a textual value.", DocumentationLink = "https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.textbox", DataType = typeof(TextBoxSamplePageVM))]
	public sealed partial class TextBoxSamplePage : Page
	{
		public TextBoxSamplePage()
		{
			this.InitializeComponent();
		}

		public class TextBoxSamplePageVM : ViewModelBase
		{
			public string InputDebugText { get => GetProperty<string>(); set => SetProperty(value); }

			public ICommand DebugInputCommand => new Command(DebugInput);

			private void DebugInput(object parameter) => InputDebugText = Invariant($"{DateTime.Now:HH:mm:ss}: parameter={parameter}");
		}
	}
}
