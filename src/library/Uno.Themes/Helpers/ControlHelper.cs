using System;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Themes;

public static class ControlHelper
{
	public static TControl GetTemplateChild<TControl>(this Control control, Func<string, DependencyObject> getTemplateChildImpl, string childName)
		where TControl : DependencyObject
	{
		var child = getTemplateChildImpl(childName) ?? throw new Exception($"Unable to find template child ({childName}) in the control template of '{control.GetType().Name}'.");
		// A type pattern needs no class constraint, so this compiles whether DependencyObject is a class or an interface.
		return child is TControl typedChild
			? typedChild
			: throw new InvalidCastException($"Unable to cast template child ({childName}) from type of '{child.GetType()}' to '{typeof(TControl)}'.");
	}
}
