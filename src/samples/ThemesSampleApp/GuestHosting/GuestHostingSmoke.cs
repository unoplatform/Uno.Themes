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
			// TEMP DIAG: SMOKE_REVERSE flips the guest order.
			var apps = Environment.GetEnvironmentVariable("SMOKE_REVERSE") is { Length: > 0 }
				? GuestAppCatalog.Apps.Reverse().ToArray()
				: GuestAppCatalog.Apps.ToArray();
			_logger.LogInformation("[HOSTING-SMOKE] Order: {Order}", string.Join(", ", apps.Select(a => a.DisplayName)));
			foreach (var app in apps)
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
				passed &= CheckXamlSnippets(loader, app);
				passed &= await CheckReclamationAsync(loader);
			}

			if (loader.CurrentApp is not null)
			{
				_logger.LogInformation("[HOSTING-SMOKE] Unloading the last guest…");
				await loader.UnloadAsync();
				passed &= await CheckReclamationAsync(loader);
			}

			if (_diag)
			{
				await DiagCensusAsync(loader);
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

	/// <summary>
	/// Verifies the guest registered its generated ShowMeTheXAML dictionary.
	/// </summary>
	/// <remarks>
	/// <c>XamlDisplay.Init()</c> resolves the generated dictionary from
	/// <c>Assembly.GetEntryAssembly()</c> unless the guest names its own assembly, and under
	/// hosting the entry assembly is this wrapper. Nothing throws when that probe misses — the
	/// resolver just answers every key with an empty string — so only a count proves it worked.
	/// </remarks>
	private static bool CheckXamlSnippets(GuestAppLoader loader, GuestAppInfo app)
	{
		var count = loader.GetHostedXamlSnippetCount();
		if (count is null)
		{
			// Distinct from "registered nothing": the probe itself could not read the resolver —
			// most likely Uno.ShowMeTheXAML stopped resolving per-ALC (a GuestSharedAssemblies.txt
			// edit) rather than anything to do with XamlDisplay.Init().
			_logger.LogError(
				"[HOSTING-SMOKE] Could not read {App}'s ShowMeTheXAML registrations; is Uno.ShowMeTheXAML still loaded per-ALC?",
				app.DisplayName);
			return false;
		}

		if (count == 0)
		{
			_logger.LogError(
				"[HOSTING-SMOKE] {App} registered no ShowMeTheXAML snippets; its XAML panes render blank.",
				app.DisplayName);
			return false;
		}

		_logger.LogInformation("[HOSTING-SMOKE] {App} registered {Count} ShowMeTheXAML snippets.", app.DisplayName, count);
		return true;
	}

	// TEMP DIAG: SMOKE_DIAG polls reclamation for up to 60 s and runs an end-of-run census.
	private static readonly bool _diag = Environment.GetEnvironmentVariable("SMOKE_DIAG") is { Length: > 0 };

	private static async Task DiagCensusAsync(GuestAppLoader loader)
	{
		var watch = System.Diagnostics.Stopwatch.StartNew();
		string[] alive = [];
		while (watch.Elapsed < TimeSpan.FromSeconds(60))
		{
			await GuestAppLoader.DiagDeepCollectAsync();
			alive = loader.DiagUnloadedAlcs.Where(x => x.Ref.TryGetTarget(out _)).Select(x => x.Name).ToArray();
			if (alive.Length == 0)
			{
				break;
			}

			await Task.Delay(500);
		}

		_logger.LogWarning("[HOSTING-SMOKE][DIAG] Census after {Elapsed:N1}s: {Total} unloaded, still alive: [{Alive}]",
			watch.Elapsed.TotalSeconds, loader.DiagUnloadedAlcs.Count, string.Join(", ", alive));

		if (alive.Length > 0 && Environment.GetEnvironmentVariable("SMOKE_HOLD") is { Length: > 0 } hold)
		{
			_logger.LogWarning("[HOSTING-SMOKE][DIAG] Holding for a dump (pid {Pid}).", Environment.ProcessId);
			System.IO.File.WriteAllText(hold, Environment.ProcessId.ToString());
			await Task.Delay(TimeSpan.FromMinutes(10));
		}
	}

	private static async Task<bool> CheckReclamationAsync(GuestAppLoader loader)
	{
		bool? collected = null;
		var watch = System.Diagnostics.Stopwatch.StartNew();
		for (var attempt = 0; _diag ? watch.Elapsed < TimeSpan.FromSeconds(60) : attempt < 5; attempt++)
		{
			collected = await loader.VerifyPreviousAlcCollectedAsync();
			if (collected != false)
			{
				break;
			}

			// Finalizer-driven unpinning can lag a collection pass, especially in the browser.
			await Task.Delay(_diag ? 500 : 200);
		}

		if (_diag)
		{
			_logger.LogWarning("[HOSTING-SMOKE][DIAG] Reclamation check: collected={Collected} after {Elapsed:N1}s", collected, watch.Elapsed.TotalSeconds);
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
