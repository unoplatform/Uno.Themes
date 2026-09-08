#nullable enable

using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;

#if WinUI
using Microsoft.UI.Dispatching;
#else
using Windows.System;
#endif

namespace Uno.Fluent;

public partial class FluentTheme
{
	// Keep the UISettings publisher alive with the theme. The event delegate holds only a weak
	// reference back, since Windows can retain a subscribed publisher past the dictionary's life.
	private UISettings? _uiSettings;

	private void ObserveSystemAccent()
	{
		_uiSettings = new UISettings();
		var weakTheme = new WeakReference<FluentTheme>(this);
		var dispatcher = DispatcherQueue.GetForCurrentThread();
		TypedEventHandler<UISettings, object>? handler = null;
		handler = (sender, args) =>
		{
			if (!weakTheme.TryGetTarget(out var theme))
			{
				sender.ColorValuesChanged -= handler;
				return;
			}

			if (dispatcher is { HasThreadAccess: false })
			{
				dispatcher.TryEnqueue(theme.RefreshSystemAccent);
			}
			else
			{
				theme.RefreshSystemAccent();
			}
		};
		_uiSettings.ColorValuesChanged += handler;
	}

	private void RefreshSystemAccent()
	{
		try
		{
			UpdateSource();
		}
		catch (InvalidOperationException e)
		{
			FluentDiagnostics.LogWarning($"FluentTheme could not refresh the system accent. {e.Message}");
		}
		catch (Exception e)
		{
			FluentDiagnostics.LogWarning($"FluentTheme could not refresh the system accent. {e.Message}");
		}
	}
}
