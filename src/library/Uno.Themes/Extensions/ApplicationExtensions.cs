using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

/// <summary>
/// Provides extension methods on <see cref="Application"/> for theme access.
/// </summary>
public static class ApplicationExtensions
{
	/// <summary>
	/// Gets the <see cref="BaseTheme"/> instance from the given application's resources.
	/// Returns <c>null</c> if no <see cref="BaseTheme"/> is found, or if <paramref name="application"/> is <c>null</c>.
	/// </summary>
	public static BaseTheme GetTheme(this Application application) =>
		application?.Resources?.MergedDictionaries.OfType<BaseTheme>().FirstOrDefault();
}
