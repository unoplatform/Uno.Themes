﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Themes.Samples.Entities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Uno.Themes.Samples.Content.Styles
{
	[SamplePage(SampleCategory.Styles, "Colors", IconPath = Icons.Styles.Colors)]
	public sealed partial class ColorsSamplePage : Page
	{
		public ColorsSamplePage()
		{
			this.InitializeComponent();
		}
	}
}
