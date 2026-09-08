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
				passed &= CheckGuestThemeIsReachable(loader, app);
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

	/// <summary>
	/// Verifies the hosted guest can reach its own theme from its own shared sample code.
	/// </summary>
	/// <remarks>
	/// Guests must not resolve their theme through <c>Application.Current</c>: it is a process-wide
	/// static in the shared Uno.UI and is never assigned for a secondary ALC, so it returns this
	/// deliberately theme-free wrapper. That is what broke the Seed Color page, which threw out of
	/// its own constructor and could not be opened in any guest. The guest heads publish themselves
	/// on their per-ALC <c>NavigationHelper.CurrentApplication</c> instead; this asserts that
	/// hand-off is in place and actually yields a <c>BaseTheme</c>.
	/// <para>
	/// Read via reflection because the guest's <c>SamplesApp.Shared</c> and theme libraries are
	/// isolated per-ALC (<c>!Uno.Themes.WinUI</c> in GuestSharedAssemblies.txt), so the wrapper
	/// shares no type identity with them — the same reason it cannot call <c>GetTheme()</c> on a
	/// guest app itself. Existing reflection precedent: GuestAppLoader.Sweeps.cs.
	/// </para>
	/// <para>
	/// Deliberately asserts the invariant rather than constructing the Seed Color page: building a
	/// guest visual tree here would add ALC roots and could destabilise the reclamation check below,
	/// failing this CI gate for reasons unrelated to theming. Systematic per-page coverage is a
	/// separate follow-up.
	/// </para>
	/// </remarks>
	private static bool CheckGuestThemeIsReachable(GuestAppLoader loader, GuestAppInfo app)
	{
		var guestApp = loader.CurrentGuestApp;
		if (guestApp is null)
		{
			_logger.LogError("[HOSTING-SMOKE] {App} produced no Application instance to check.", app.DisplayName);
			return false;
		}

		var navigationHelper = guestApp.GetType().Assembly.GetType("Uno.Themes.Samples.NavigationHelper");
		if (navigationHelper?.GetProperty("CurrentApplication")?.GetValue(null) is not Application registered)
		{
			_logger.LogError(
				"[HOSTING-SMOKE] {App} did not publish NavigationHelper.CurrentApplication; its shared code cannot reach its own theme.",
				app.DisplayName);
			return false;
		}

		if (!ReferenceEquals(registered, guestApp))
		{
			_logger.LogError(
				"[HOSTING-SMOKE] {App} published a different Application than the one hosted.",
				app.DisplayName);
			return false;
		}

		// Mirrors Uno.Themes' own first-level MergedDictionaries scan, matched by name against the
		// guest's isolated BaseTheme type — hence this rather than calling GetTheme() directly.
		var theme = registered.Resources?.MergedDictionaries
			.FirstOrDefault(d => IsBaseTheme(d.GetType()));

		if (theme is null)
		{
			// Not a failure: only Material and Simple ship a BaseTheme-derived dictionary. Cupertino
			// merges CupertinoColors/Fonts/Resources and has no CupertinoTheme type at all, which is
			// why the Seed Color page declares SupportedDesigns = { Material, Simple }. The invariant
			// this gate enforces is the hand-off above, which every guest owes.
			_logger.LogInformation(
				"[HOSTING-SMOKE] {App} publishes its application; it merges no BaseTheme (expected for this design).",
				app.DisplayName);
			return true;
		}

		_logger.LogInformation(
			"[HOSTING-SMOKE] {App} resolves its own theme ({Theme}) from its own application.",
			app.DisplayName,
			theme.GetType().Name);
		return true;
	}

	/// <summary>
	/// Walks the base chain by name: <c>BaseTheme</c> lives in the guest's isolated
	/// <c>Uno.Themes.WinUI</c>, so the wrapper has no shared type to compare against.
	/// </summary>
	private static bool IsBaseTheme(Type? type)
	{
		for (var current = type; current is not null; current = current.BaseType)
		{
			if (current.FullName == "Uno.Themes.BaseTheme")
			{
				return true;
			}
		}

		return false;
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
