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
	private CancellationTokenSource? _operationCts;

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

		Loaded += OnFirstLoaded;
	}

	private async void OnFirstLoaded(object sender, RoutedEventArgs e)
	{
		try
		{
			Loaded -= OnFirstLoaded;

			// The hosting smoke (--smoke / ?smoke) drives the full load/switch/unload cycle
			// unattended and reports a machine-readable verdict — see GuestHostingSmoke.
			if (GuestHostingSmoke.IsRequested)
			{
				SetPickerEnabled(false);
				CancelButton.IsEnabled = false;
				ShowStatus(InfoBarSeverity.Informational, "Hosting smoke test running…");

				var passed = await GuestHostingSmoke.RunAsync(_loader);

				ShowStatus(
					passed ? InfoBarSeverity.Success : InfoBarSeverity.Error,
					passed ? "Hosting smoke test passed." : "Hosting smoke test FAILED — see logs.");
				SetPickerEnabled(true);
				CancelButton.IsEnabled = false;
				GuestHostingSmoke.Exit(passed);
				return;
			}

			// A launch selector (?app=material / --app=material) loads that guest unattended;
			// without one the picker just waits for a click.
			if (GuestAppDeepLink.Resolve() is { } info)
			{
				await RunLoaderOperationAsync(
					$"Loading {info.DisplayName}…",
					(progress, ct) => _loader.LoadAsync(info, progress, ct),
					requestedApp: info);
			}
		}
		catch (Exception ex)
		{
			// async void handler: nothing may escape.
			ReportUnexpected(ex);
		}
	}

	private void OnCancelClick(object sender, RoutedEventArgs e)
	{
		// Cancels the in-flight load; teardown deliberately ignores cancellation (a half-torn
		// guest is worse than a slow one), so Unload runs to completion regardless.
		_operationCts?.Cancel();
	}

	private async void OnGuestAppClick(object sender, RoutedEventArgs e)
	{
		try
		{
			if ((sender as FrameworkElement)?.Tag is GuestAppInfo info)
			{
				await RunLoaderOperationAsync(
					$"Loading {info.DisplayName}…",
					(progress, ct) => _loader.LoadAsync(info, progress, ct),
					requestedApp: info);
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
				(progress, ct) => _loader.LoadAsync(info, progress, ct),
				requestedApp: info);
		}
		catch (Exception ex)
		{
			ReportUnexpected(ex);
		}
	}

	private async Task RunLoaderOperationAsync(
		string initialStatus,
		Func<IProgress<string>, CancellationToken, Task> operation,
		GuestAppInfo? requestedApp = null)
	{
		if (_operationInProgress)
		{
			return;
		}

		// Only an accepted request becomes the Reload target; a click rejected by the guard
		// above must not redirect a later Reload.
		if (requestedApp is not null)
		{
			_lastRequestedApp = requestedApp;
		}

		_operationInProgress = true;
		using var cts = new CancellationTokenSource();
		_operationCts = cts;
		SetPickerEnabled(false);
		ShowStatus(InfoBarSeverity.Informational, initialStatus);
		try
		{
			var progress = new Progress<string>(message => ShowStatus(InfoBarSeverity.Informational, message));
			await operation(progress, cts.Token);

			ShowStatus(
				InfoBarSeverity.Success,
				_loader.CurrentApp is { } current ? $"{current.DisplayName} is running." : "No guest app is loaded.");
		}
		catch (OperationCanceledException)
		{
			_logger.LogInformation("Guest hosting operation canceled by the user.");
			ShowStatus(InfoBarSeverity.Informational, "Operation canceled.");
		}
		catch (GuestAppLoadException ex)
		{
			// The InfoBar alone is not enough: headless and CI runs only have the log.
			_logger.LogError(ex, "Guest hosting operation failed.");
			ShowStatus(InfoBarSeverity.Error, ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Guest hosting operation failed unexpectedly.");
			ShowStatus(InfoBarSeverity.Error, $"Unexpected failure: {ex.Message}");
		}
		finally
		{
			_operationCts = null;
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

		// Cancel is the inverse: only meaningful while an operation is in flight.
		CancelButton.IsEnabled = !isEnabled;
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
