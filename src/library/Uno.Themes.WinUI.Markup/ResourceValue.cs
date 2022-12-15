using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.MarkupHelpers;


namespace Uno.Themes.Markup
{
	public struct ResourceValue<T>
	{
		public ResourceValue(string key, bool isThemeResource = true)
		{
			Key = key;
			IsThemeResource = isThemeResource;
		}

		public string Key { get; }

		public bool IsThemeResource { get; }

		public static implicit operator Action<IDependencyPropertyBuilder<T>>(ResourceValue<T> resource) =>
			x => GetResourceBasedOnIsThemeResourceProperty(resource);

		private static Action<IDependencyPropertyBuilder<T>> GetResourceBasedOnIsThemeResourceProperty(ResourceValue<T> resource)
		{
			return resource.IsThemeResource ?
				ThemeResource.Get<T>(resource.Key)
				: StaticResource.Get<T>(resource.Key);
		}
	}
}
