using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

// ─── Sample heads ─────────────────────────────────────────────────────────────
// Each sample target is an explicit-start executable resource: the dashboard shows a Stopped
// tile until the developer clicks Start, so AppHost boot is instant and nothing builds until
// asked. AddExecutable (not AddProject) is deliberate — the Projects.* source generator
// mishandles multi-TFM Uno heads; an explicit `-f` + `--launch-profile` is the reliable path.
//
// workingDirectory is "." (the AppHost project directory); the sample project paths are relative
// to it. Resources are named <sample>-<target> so the dashboard groups a design system together.

// Desktop heads (Skia). No HTTP listener, so no endpoint is exposed.
AddDesktopSample("material-desktop", "MaterialSampleApp", "MaterialSampleApp (Desktop)");
AddDesktopSample("cupertino-desktop", "CupertinoSampleApp", "CupertinoSampleApp (Desktop)");
AddDesktopSample("simple-desktop", "SimpleSampleApp", "SimpleSamplesApp (Desktop)");

// WASM heads. Ports come from each sample's `(WebAssembly)` launch profile; Cupertino was
// re-pinned 5000 → 5001 so Material + Cupertino can run side by side under the orchestrator.
AddWasmSample("material-wasm", "MaterialSampleApp", "MaterialSampleApp (WebAssembly)", 5000);
AddWasmSample("cupertino-wasm", "CupertinoSampleApp", "CupertinoSampleApp (WebAssembly)", 5001);
AddWasmSample("simple-wasm", "SimpleSampleApp", "SimpleSamplesApp (WebAssembly)", 5002);

// ─── Runtime tests ──────────────────────────────────────────────────────────────
// Builds + runs the SimpleSampleApp runtime-test suite (the CI-validated host) as a foreground
// process: click Start, watch output stream, tile turns red on failure. Filter/config are set via
// env (shell before AppHost launch, or the resource's dashboard override). Bash-only → non-Windows
// (run the AppHost from WSL on Windows hosts).
if (!OperatingSystem.IsWindows())
{
	var runtimeTests = builder.AddExecutable(
			"simple-runtime-tests",
			"bash",
			workingDirectory: ".",
			"scripts/run-runtime-tests.sh")
		.WithExplicitStart();

	ForwardFromHostEnv(runtimeTests, "CONFIG", "UNO_RUNTIME_TESTS_RUN_TESTS", "UNO_RUNTIME_TESTS_OUTPUT_PATH");

	// ─── Android emulator + Simple head ──────────────────────────────────────────
	// Two resources so the emulator lifecycle ties to the AppHost process (dies on
	// Ctrl+C via `adb emu kill`) while staying out of the way until needed. Both
	// ExplicitStart. Clicking Start on simple-android transparently boots the
	// emulator (WaitFor + the start-kick below). Set ANDROID_AUTO_START_EMULATOR=0
	// in the launching shell to skip the emulator entirely and use your own device.
	// Only simple-android ships in v1; the launcher stays parameterized so Material /
	// Cupertino heads are a later registration-only addition (override ANDROID_PROJECT
	// / ANDROID_PACKAGE).
	var registerEmulator = Environment.GetEnvironmentVariable("ANDROID_AUTO_START_EMULATOR") != "0";

	IResourceBuilder<ExecutableResource>? emulator = null;
	if (registerEmulator)
	{
		emulator = builder.AddExecutable("android-emulator", "bash", workingDirectory: ".", "scripts/start-emulator.sh")
			.WithExplicitStart();
		ForwardFromHostEnv(emulator,
			"ANDROID_AVD_NAME", "ANDROID_SDK_API_LEVEL", "ANDROID_SDK_IMAGE",
			"ANDROID_EMULATOR_WINDOW", "ANDROID_EMULATOR_GPU", "DISPLAY");
	}

	var android = builder.AddExecutable("simple-android", "bash", workingDirectory: ".", "scripts/launch-android.sh")
		.WithEnvironment("ANDROID_PROJECT", "../../samples/SimpleSampleApp/SimpleSampleApp.csproj")
		.WithEnvironment("ANDROID_PACKAGE", "uno.platform.themes.simple")
		.WithEnvironment("ANDROID_CONFIG", "Debug")
		.WithExplicitStart();
	ForwardFromHostEnv(android, "ANDROID_SERIAL", "DEVICE_WAIT_TIMEOUT_S");

	// WaitFor only gates startup — it does NOT start an ExplicitStart dependency on
	// its own. Subscribe to BeforeResourceStartedEvent on simple-android and issue
	// the emulator's Start command from there so clicking Start on simple-android
	// pulls android-emulator up transparently. Idempotent: skipped when the emulator
	// is already Running (the dev clicked Start on it directly).
	if (emulator is not null)
	{
		var emulatorResource = emulator.Resource;
		android.WaitFor(emulator);
		builder.Eventing.Subscribe<BeforeResourceStartedEvent>(
			android.Resource,
			async (@event, ct) =>
			{
				var rns = @event.Services.GetRequiredService<ResourceNotificationService>();
				var current = await rns.WaitForResourceAsync(emulatorResource.Name, _ => true, ct).ConfigureAwait(false);
				if (current.Snapshot.State?.Text is { Length: > 0 } state
					&& state.Equals("Running", StringComparison.OrdinalIgnoreCase))
				{
					return;
				}

				var commands = @event.Services.GetRequiredService<ResourceCommandService>();
				await commands.ExecuteCommandAsync(emulatorResource, KnownResourceCommands.StartCommand, ct).ConfigureAwait(false);
			});
	}
}

builder.Build().Run();

// Local helpers keep the six registrations DRY. Both return the resource builder so future phases
// can chain WaitFor / event wiring onto a specific head.
IResourceBuilder<ExecutableResource> AddDesktopSample(string name, string project, string launchProfile) =>
	builder.AddExecutable(
			name,
			"dotnet",
			workingDirectory: ".",
			"run", "--project", $"../../samples/{project}/{project}.csproj",
			"-f", "net10.0-desktop",
			// Collapse the sample AND its multi-targeted library deps to just the desktop TFM.
			// Without this, `dotnet run` evaluates every platform TFM in the graph (android/ios/wasm)
			// and fails on missing workloads. As a command-line global property it also wins over any
			// crosstargeting_override.props the dev has, so this resource always builds desktop.
			"-p:TargetFrameworkOverride=desktop",
			"--launch-profile", launchProfile)
		// Hot reload needs modifiable assemblies; pin it here so an edit to the launch profile
		// can't silently disable hot reload for the AppHost-launched instance.
		.WithEnvironment("DOTNET_MODIFIABLE_ASSEMBLIES", "debug")
		.WithExplicitStart();

IResourceBuilder<ExecutableResource> AddWasmSample(string name, string project, string launchProfile, int port) =>
	builder.AddExecutable(
			name,
			"dotnet",
			workingDirectory: ".",
			"run", "--project", $"../../samples/{project}/{project}.csproj",
			"-f", "net10.0-browserwasm",
			// See desktop note: collapse to the single WASM TFM so unrelated platform workloads
			// aren't required, and override any crosstargeting_override.props the dev has set.
			"-p:TargetFrameworkOverride=browserwasm",
			"--launch-profile", launchProfile)
		// The Uno WASM bootstrap dev server binds its own port (from the sample's launch profile).
		// isProxied:false makes the dashboard surface that real URL instead of an Aspire forwarder.
		.WithHttpEndpoint(port: port, name: "http", isProxied: false)
		.WithExternalHttpEndpoints()
		.WithExplicitStart();

// Aspire's ExecutableResource does NOT inherit arbitrary parent-process env vars — only the keys
// forwarded here reach the child. Empty/unset parent values are skipped so the scripts'
// ${VAR:-default} fallbacks aren't clobbered by an empty string. Shared by the runtime-tests and
// (Phase 3) Android resources.
void ForwardFromHostEnv(IResourceBuilder<ExecutableResource> resource, params string[] names) =>
	resource.WithEnvironment(ctx =>
	{
		foreach (var name in names)
		{
			var value = Environment.GetEnvironmentVariable(name);
			if (!string.IsNullOrEmpty(value))
			{
				ctx.EnvironmentVariables[name] = value;
			}
		}
	});
