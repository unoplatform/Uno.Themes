#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

internal static class ResourceDictionaryExtensions
{
	/// <summary>
	/// Used to safely nest a res-dict under another, regardless if it was nested before.
	/// </summary>
	/// <param name="rd"></param>
	/// <param name="nested"></param>
	/// <remarks>Used for <see cref="MaterialColors"> and <see cref="MaterialFonts"> "delay-one" initialization only.</remarks>
	public static void SafeMerge(this ResourceDictionary rd, ResourceDictionary nested)
	{
		/* Here is an example of a typical App.xaml setup:
			<MaterialColors OverrideSource="ms-appx:///custom-colors.xaml" /> <!-- @1 -->
			<MaterialFonts OverrideSource="ms-appx:///custom-fonts.xaml" />
			<MaterialResources>
				<!-- simplified for brevity, but the essence is the same if we expand it -->
				<ResourceDictionary.MergedDictionaries>
					<MaterialColors />  <!-- @2 -->
					<MaterialFonts />
				<ResourceDictionary.MergedDictionaries>

				<!-- Resources/Styles... -->
			</MaterialResources>

			<MaterialColors OverrideSource=... /> <!-- @3 -->
			<MaterialToolkitResources /> <!-- same structure as MaterialResources that nested MaterialColors&Fonts -->
		*/

		// In order for the override to work, it has to be included within MaterialResource.MergedDictionary (see: @2).
		// Otherwise the brush/font will be resolved with the default font/colors. Due to the way MaterialResource is setup,
		// we don't have direct access to the nested resources, early enough (is this necessary?).
		// We workaround this by using a "delay-one" initialization hack. TL;DR: OverrideSource is merged into the next new instance ctor'd.
		// MaterialFonts/Colors contains a static field to hold the override dictionary.
		// We can indirectly assign this field through OverrideSource dependency property. And in the .ctor, we merge this dictionary if available.
		// And, the initialization sequence will look like the following:
		// 1. MaterialColors@1 is ctor'd, but the static field is still null.
		// 2. MaterialColors@1 OverrideSource is set, and we store that to the static field.
		// 3. MaterialColors@2 inside the MaterialResources is ctor'd, the static field is non-null, so we merge that.

		// That works fine, except on uwp/winui-desktop where it is illegal to nest the same instance of dictionary under multiple parent dictionary.
		// That happens when we new up any instance of MaterialFonts/Colors after the "delay-one" hack has proc'd once directly or indirectly:
		// 4. MaterialColors@3 is ctor'd, the static field IS NON-NULL, so we merge that AGAIN.

		// While we can set the static field to null after merging it, it would break if we define MaterialToolkitResources after MaterialResources.
		// The toolkit res would not receive the proper override, unless we define another MaterialColors with override.
		// But that is not intuitive for the user and would require additional explanation.
		// So to keep it simple, we will just always merge a clone instead of original instance (we have no way of determining if the dict is already a child of another).

		rd.MergedDictionaries
#if !HAS_UNO
			.Add(nested.Duplicate());
#else
			.Add(nested);
#endif
	}

	public static ResourceDictionary Duplicate(this ResourceDictionary source)
	{
		var clone = new ResourceDictionary();

		foreach (var kvp in source)
		{
			clone.Add(kvp);
		}
		foreach (var merged in source.MergedDictionaries)
		{
			clone.MergedDictionaries.Add(merged.Duplicate());
		}
		foreach (var theme in source.ThemeDictionaries)
		{
			clone.ThemeDictionaries.Add(theme.Key, ((ResourceDictionary)theme.Value).Duplicate());
		}

		return clone;
	}
}
