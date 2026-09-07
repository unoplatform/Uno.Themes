#nullable enable

using Uno.Themes.Helpers;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Fluent;

/// <summary>
/// Resolves Fluent consumer resources using the shared appearance rules.
/// </summary>
internal static class FluentResourceResolver
{
	internal static object? Resolve(ResourceDictionary? dictionary, string appearance, string key)
		=> ThemeResourceResolver.Resolve(dictionary, appearance, key);
}
