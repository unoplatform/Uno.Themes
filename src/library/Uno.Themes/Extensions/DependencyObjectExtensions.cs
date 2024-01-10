using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Disposables;
using Uno.Extensions;
using System.Reflection;
using Microsoft.Extensions.Logging;
#if WinUI
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
#endif

namespace Uno.Themes;

internal static class DependencyObjectExtensions
{
	public static IDisposable RegisterDisposablePropertyChangedCallback(this DependencyObject instance, DependencyProperty property, DependencyPropertyChangedCallback callback)
	{

		var token = instance.RegisterPropertyChangedCallback(property, callback);
		return Disposable.Create(() => instance.UnregisterPropertyChangedCallback(property, token));
	}
}
