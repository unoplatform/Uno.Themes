using System;
using System.Collections.Generic;
using System.Text;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Themes;

internal static class ControlHelper
{
	public static TControl GetTemplateChild<TControl>(this Control control, Func<string, DependencyObject> getTemplateChildImpl, string childName)
#if HAS_UNO
		where TControl : class, DependencyObject
#else
		where TControl : DependencyObject
#endif
	{
		var child = getTemplateChildImpl(childName) ?? throw new Exception($"Unable to find template child ({childName}) in the control template of '{control.GetType().Name}'.");
		return child as TControl ?? throw new InvalidCastException($"Unable to cast template child ({childName}) from type of '{child.GetType()}' to '{typeof(TControl)}'.");
	}
}
