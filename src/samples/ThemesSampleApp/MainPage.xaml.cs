using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uno.Themes.WrapperApp.GuestHosting;
using Uno.UI.Xaml;
using Uno.UI.Xaml.Controls;

namespace Uno.Themes.WrapperApp;

/// <summary>
/// Hosts the guest-app picker and the region the guest apps render into.
/// </summary>
public sealed partial class MainPage : Page
{
	private static readonly ILogger _logger =
		global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory.CreateLogger<MainPage>();

	private readonly AlcContentHost _contentHost;
	private readonly GuestAppLoader _loader;
	private GuestAppInfo? _lastRequestedApp;
	private bool _operationInProgress;

	public MainPage()
	{
		this.InitializeComponent();

		_contentHost = new AlcContentHost
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch,
			HorizontalContentAlignment = HorizontalAlignment.Stretch,
			VerticalContentAlignment = VerticalAlignment.Stretch,
		};
		GuestRegion.Child = _contentHost;

		// Redirect hosted guests' windows into our region. Created once and kept for the app's
		// lifetime; the loader reasserts the override before each load.
		WindowHelper.ContentHostOverride = _contentHost;

		_loader = new GuestAppLoader(_contentHost);

		foreach (var app in GuestAppCatalog.Apps)
		{
			var button = new Button
			{
				Content = app.DisplayName,
				Tag = app,
			};
			button.Click += OnGuestAppClick;
			AppButtonsPanel.Children.Add(button);
		}
	}

	private async void OnGuestAppClick(object sender, RoutedEventArgs e)
	{
		try
		{
			if ((sender as FrameworkElement)?.Tag is GuestAppInfo info)
			{
				_lastRequestedApp = info;
				await RunLoaderOperationAsync(
					$"Loading {info.DisplayName}…",
					(progress, ct) => _loader.LoadAsync(info, progress, ct));
			}
		}
		catch (Exception ex)
		{
			// async void handler: nothing may escape.
			ReportUnexpected(ex);
		}
	}

	private async void OnUnloadClick(object sender, RoutedEventArgs e)
	{
		try
		{
			await RunLoaderOperationAsync(
				"Unloading…",
				(progress, ct) => _loader.UnloadAsync(progress, ct));
		}
		catch (Exception ex)
		{
			ReportUnexpected(ex);
		}
	}

	private async void OnReloadClick(object sender, RoutedEventArgs e)
	{
		try
		{
			if ((_loader.CurrentApp ?? _lastRequestedApp) is not { } info)
			{
				ShowStatus(InfoBarSeverity.Informational, "Load a sample app first.");
				return;
			}

			await RunLoaderOperationAsync(
				$"Reloading {info.DisplayName}…",
				(progress, ct) => _loader.LoadAsync(info, progress, ct));
		}
		catch (Exception ex)
		{
			ReportUnexpected(ex);
		}
	}

	private async Task RunLoaderOperationAsync(string initialStatus, Func<IProgress<string>, CancellationToken, Task> operation)
	{
		if (_operationInProgress)
		{
			return;
		}

		_operationInProgress = true;
		SetPickerEnabled(false);
		ShowStatus(InfoBarSeverity.Informational, initialStatus);
		try
		{
			var progress = new Progress<string>(message => ShowStatus(InfoBarSeverity.Informational, message));
			await operation(progress, CancellationToken.None);

			ShowStatus(
				InfoBarSeverity.Success,
				_loader.CurrentApp is { } current ? $"{current.DisplayName} is running." : "No guest app is loaded.");
		}
		catch (GuestAppLoadException ex)
		{
			ShowStatus(InfoBarSeverity.Error, ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Guest hosting operation failed unexpectedly.");
			ShowStatus(InfoBarSeverity.Error, $"Unexpected failure: {ex.Message}");
		}
		finally
		{
			_operationInProgress = false;
			SetPickerEnabled(true);
		}
	}

	private void SetPickerEnabled(bool isEnabled)
	{
		foreach (var child in AppButtonsPanel.Children)
		{
			if (child is Control control)
			{
				control.IsEnabled = isEnabled;
			}
		}

		UnloadButton.IsEnabled = isEnabled;
		ReloadButton.IsEnabled = isEnabled;
	}

	private void ShowStatus(InfoBarSeverity severity, string message)
	{
		StatusBar.Severity = severity;
		StatusBar.Message = message;
		StatusBar.IsOpen = true;
	}

	private void ReportUnexpected(Exception ex)
	{
		_logger.LogError(ex, "Unhandled failure in a picker interaction.");
		ShowStatus(InfoBarSeverity.Error, $"Unexpected failure: {ex.Message}");
	}
}
