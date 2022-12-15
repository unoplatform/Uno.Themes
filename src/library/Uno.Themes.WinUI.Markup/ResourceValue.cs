using System;
using Microsoft.UI.Xaml.MarkupHelpers;

namespace Uno.Themes.Markup
{
	public struct ResourceValue<T>
	{
		public ResourceValue(string key)
		{
			Key = key;
		}

		public string Key { get; }

		public static implicit operator Action<IDependencyPropertyBuilder<T>>(ResourceValue<T> resource) =>
			x => x.ThemeResource(resource.Key);
	}
}
