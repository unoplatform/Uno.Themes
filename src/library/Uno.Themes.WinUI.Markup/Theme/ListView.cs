﻿using Microsoft.UI.Xaml;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;

namespace Uno.Themes.Markup;

public static partial class Theme
{
	public static partial class ListView
	{
		public static partial class Styles
		{
			[ResourceKeyDefinition(typeof(Style), "ListViewStyle", TargetType = typeof(ListView))]
			public static StaticResourceKey<Style> Default => new("ListViewStyle");
		}
	}
}
