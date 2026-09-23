using Microsoft.Extensions.Logging;

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Self-driving hosting verification (launch with <c>--smoke</c> on desktop or <c>?smoke</c>
/// in the browser): loads every catalog guest in sequence, unloads the last one, and checks
/// that each unloaded guest ALC is actually reclaimed. On desktop the process exits with
/// <c>0</c> (pass) / <c>1</c> (fail) so CI can gate on it; in the browser the verdict is
/// logged as <c>[HOSTING-SMOKE] RESULT: …</c> for a driving harness to scrape.
/// </summary>
internal static class GuestHostingSmoke
{
	private const string SmokeFlagName = "smoke";

	// ALC reclamation is only deterministic on Release desktop builds: Debug JIT root
	// retention and residual WASM roots are documented, accepted limitations (see
	// specs/05-alc-wrapper-app/progress.md), so those configurations report reclamation
	// without failing on it.
#if DEBUG || __WASM__
	private static readonly bool _reclamationIsAuthoritative = false;
#else
	private static readonly bool _reclamationIsAuthoritative = true;
#endif

	private static readonly ILogger _logger =
		global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory.CreateLogger("Uno.Themes.WrapperApp.GuestHosting.GuestHostingSmoke");

	/// <summary>
	/// Gets whether the smoke was requested by a launch selector.
	/// </summary>
	public static bool IsRequested => GuestAppDeepLink.GetLaunchFlag(SmokeFlagName);

	/// <summary>
	/// Runs the full load/switch/unload cycle and returns the verdict. Never throws.
	/// </summary>
	public static async Task<bool> RunAsync(GuestAppLoader loader)
	{
		var passed = true;
		try
		{
			foreach (var app in GuestAppCatalog.Apps)
			{
				_logger.LogInformation("[HOSTING-SMOKE] Loading {App}…", app.DisplayName);
				await loader.LoadAsync(app);
				if (!ReferenceEquals(loader.CurrentApp, app))
				{
					_logger.LogError("[HOSTING-SMOKE] {App} did not become the hosted app.", app.DisplayName);
					passed = false;
					break;
				}

				_logger.LogInformation("[HOSTING-SMOKE] {App} is hosted.", app.DisplayName);
				passed &= await CheckReclamationAsync(loader);
			}

			if (loader.CurrentApp is not null)
			{
				_logger.LogInformation("[HOSTING-SMOKE] Unloading the last guest…");
				await loader.UnloadAsync();
				passed &= await CheckReclamationAsync(loader);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "[HOSTING-SMOKE] Failed with an exception.");
			passed = false;
		}

		_logger.LogInformation("[HOSTING-SMOKE] RESULT: {Result}", passed ? "PASS" : "FAIL");
		return passed;
	}

	/// <summary>
	/// Ends the smoke run: exits the process with the verdict on desktop; no-ops in the
	/// browser (the logged RESULT line is the verdict there).
	/// </summary>
	public static void Exit(bool passed)
	{
#if !__WASM__
		Environment.Exit(passed ? 0 : 1);
#endif
	}

	private static async Task<bool> CheckReclamationAsync(GuestAppLoader loader)
	{
		bool? collected = null;
		for (var attempt = 0; attempt < 5; attempt++)
		{
			collected = await loader.VerifyPreviousAlcCollectedAsync();
			if (collected != false)
			{
				break;
			}

			// Finalizer-driven unpinning can lag a collection pass, especially in the browser.
			await Task.Delay(200);
		}

		if (collected is null)
		{
			// Nothing has been unloaded yet (first load of the run).
			return true;
		}

		if (collected == true)
		{
			_logger.LogInformation("[HOSTING-SMOKE] Previous guest ALC reclaimed.");
			return true;
		}

		if (_reclamationIsAuthoritative)
		{
			_logger.LogError("[HOSTING-SMOKE] Previous guest ALC was NOT reclaimed.");
			return false;
		}

		_logger.LogWarning("[HOSTING-SMOKE] Previous guest ALC not reclaimed — expected on Debug/WASM (documented root retention), not failing the smoke.");
		return true;
	}
}
