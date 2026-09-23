using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Uno.UI.Xaml;
using Uno.UI.Xaml.Controls;

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Raised when a guest app cannot be located, booted, or stopped. The message is user-presentable.
/// </summary>
internal sealed class GuestAppLoadException : Exception
{
	public GuestAppLoadException(string message)
		: base(message)
	{
	}

	public GuestAppLoadException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

/// <summary>
/// Owns at most one hosted guest app: locates its binaries, boots it in a collectible
/// AssemblyLoadContext through a second <see cref="global::Uno.UI.Hosting.UnoPlatformHostBuilder"/>,
/// and tears it down in the order proven by studio.live (clear content → restore binding
/// provider → stop run loop → Exit → clear late content → unload → sweep + GC).
/// </summary>
/// <remarks>
/// Split across partials: session lifecycle here; run-loop and binary location per platform in
/// <c>GuestAppLoader.Desktop.cs</c> / <c>GuestAppLoader.Wasm.cs</c>; the reflection-based
/// compensations for Uno 6.7-dev per-ALC sweep gaps in <c>GuestAppLoader.Sweeps.cs</c>.
/// </remarks>
internal sealed partial class GuestAppLoader
{
	// Guests boot a whole Uno app (XAML parse, theme merge, first layout); 30 s matches the
	// upstream runtime-test allowance and is generous enough for cold WASM interpretation.
	private static readonly TimeSpan _contentReadyTimeout = TimeSpan.FromSeconds(30);

	// How long the guest's run loop gets to finish on its own after content is cleared,
	// before the dedicated thread is interrupted (desktop only).
	private static readonly TimeSpan _executionStopTimeout = TimeSpan.FromSeconds(5);

	// Budget for UI-thread dispatches during teardown (content clear, Application.Exit).
	private static readonly TimeSpan _uiDispatchTimeout = TimeSpan.FromSeconds(10);

	private static readonly ILogger _logger =
		global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory.CreateLogger<GuestAppLoader>();

	private readonly AlcContentHost _contentHost;
	private readonly DispatcherQueue _dispatcherQueue;
	private readonly SemaphoreSlim _gate = new(1, 1);
	private Session? _session;

	// Latched when a guest could not be proven stopped (a run loop that refused to stop, or a
	// teardown that threw): its code may still execute against the shared content host, where
	// it could satisfy a later load's content wait or re-project resources. Hosting anything
	// else in this process would be built on sand.
	private bool _faulted;

	// The host's original binding-metadata provider: a hosted guest overwrites the process-wide
	// provider with its own, and ALC teardown then nulls it (it belongs to a dying ALC). Nothing
	// upstream restores the host's provider, so we must (same hygiene as the Uno runtime tests).
	private readonly global::Uno.UI.DataBinding.IBindableMetadataProvider? _hostBindableMetadataProvider =
		global::Uno.UI.DataBinding.BindableMetadata.Provider;

	// Diagnostics: tracks the previously unloaded guest ALC so the next load can report whether
	// it was actually collected (the wrapper's reason to exist is exercising this machinery).
	private WeakReference<GuestAssemblyLoadContext>? _lastUnloadedAlc;

	private sealed class Session
	{
		public required GuestAppInfo Info { get; init; }

		public required GuestAssemblyLoadContext Alc { get; init; }

		public Application? GuestApp;

		public Task? ExecutionTask;

		public Thread? ExecutionThread;
	}

	/// <summary>
	/// Initializes the loader around the persistent content host. Must be called on the UI thread.
	/// </summary>
	public GuestAppLoader(AlcContentHost contentHost)
	{
		ArgumentNullException.ThrowIfNull(contentHost);

		_contentHost = contentHost;
		_dispatcherQueue = contentHost.DispatcherQueue
			?? throw new InvalidOperationException("GuestAppLoader must be created on the UI thread.");
	}

	/// <summary>
	/// Gets the app currently hosted, if any.
	/// </summary>
	public GuestAppInfo? CurrentApp => _session?.Info;

	/// <summary>
	/// Gets whether the most recently unloaded guest ALC was observed collected: <see langword="true"/>
	/// once the weak reference died, <see langword="false"/> while it is still alive after a
	/// collection pass, <see langword="null"/> when nothing has been unloaded yet.
	/// </summary>
	internal bool? LastUnloadedAlcCollected { get; private set; }

	/// <summary>
	/// Loads <paramref name="info"/> into a fresh collectible ALC, tearing down any previous guest first.
	/// </summary>
	public async Task LoadAsync(GuestAppInfo info, IProgress<string>? progress = null, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(info);

		await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			ThrowIfFaulted();

			// Locate before tearing anything down: a click on a guest whose binaries are
			// missing must not destroy the session that is currently running.
			progress?.Report($"Locating {info.DisplayName} binaries…");
			var guestDirectory = await LocateGuestDirectoryAsync(info, cancellationToken).ConfigureAwait(false);

			await UnloadCoreAsync(progress).ConfigureAwait(false);

			cancellationToken.ThrowIfCancellationRequested();

			// The override can drift if anything else touched it; the guest's Window ctor
			// reads it, so it must point at our host before the guest boots.
			await RunOnUIThreadAsync(() => WindowHelper.ContentHostOverride = _contentHost).ConfigureAwait(false);

			if (_logger.IsEnabled(LogLevel.Information))
			{
				_logger.LogInformation("Loading guest {App} from {Directory}", info.AssemblyName, guestDirectory);
			}

			// Release-before-allocate: collect what the previous guest pinned before the next
			// ALC maps its assemblies (WASM memory growth is irreversible).
			await DeepCollectAsync().ConfigureAwait(false);
			ReportPreviousAlcCollectionState();

			var session = new Session
			{
				Info = info,
				Alc = new GuestAssemblyLoadContext(info.AssemblyName, guestDirectory),
			};

			progress?.Report($"Starting {info.DisplayName}…");

			var contentReady = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
			EventHandler<EventArgs> onContentChanged = (_, _) =>
			{
				// ContentChanged also fires on clear-to-null; only non-null content means "ready".
				if (_contentHost.Content is not null)
				{
					contentReady.TrySetResult();
				}
			};

			_contentHost.ContentChanged += onContentChanged;
			try
			{
				await Task.Run(() => BootGuest(session), cancellationToken).ConfigureAwait(false);
				await WaitForFirstContentAsync(session, contentReady.Task, cancellationToken).ConfigureAwait(false);
			}
			catch
			{
				if (!await TeardownCoreAsync(session, progress).ConfigureAwait(false))
				{
					_faulted = true;
				}

				throw;
			}
			finally
			{
				_contentHost.ContentChanged -= onContentChanged;
			}

			_session = session;

			// Headless/CI diagnosability: the InfoBar success message never reaches a log.
			if (_logger.IsEnabled(LogLevel.Information))
			{
				_logger.LogInformation("Guest {App} presented content and is running.", info.AssemblyName);
			}

			progress?.Report($"{info.DisplayName} is running.");
		}
		finally
		{
			_gate.Release();
		}
	}

	/// <summary>
	/// Tears down the current guest, if any.
	/// </summary>
	public async Task UnloadAsync(IProgress<string>? progress = null, CancellationToken cancellationToken = default)
	{
		await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			if (_session is null)
			{
				progress?.Report("No guest app is loaded.");
				return;
			}

			await UnloadCoreAsync(progress).ConfigureAwait(false);
			progress?.Report("Guest app unloaded.");
		}
		finally
		{
			_gate.Release();
		}
	}

	private async Task UnloadCoreAsync(IProgress<string>? progress)
	{
		if (_session is not { } session)
		{
			return;
		}

		_session = null;
		if (!await TeardownCoreAsync(session, progress).ConfigureAwait(false))
		{
			_faulted = true;
			throw new GuestAppLoadException(
				$"{session.Info.DisplayName} did not stop cleanly; its resources stay resident and guest hosting is disabled until the app restarts.");
		}
	}

	private void ThrowIfFaulted()
	{
		if (_faulted)
		{
			throw new GuestAppLoadException("A previous guest did not stop cleanly; restart the app to host guests again.");
		}
	}

	private void BootGuest(Session session)
	{
		var info = session.Info;

		Assembly mainAssembly;
		try
		{
			mainAssembly = session.Alc.LoadFromAssemblyName(new AssemblyName(info.AssemblyName));
		}
		catch (Exception ex) when (ex is FileNotFoundException or FileLoadException or BadImageFormatException)
		{
			throw new GuestAppLoadException($"Could not load {info.AssemblyName}.dll from {session.Alc.GuestDirectory}.", ex);
		}

		Type applicationType;
		try
		{
			applicationType = FindApplicationType(mainAssembly)
				?? throw new GuestAppLoadException($"{info.AssemblyName} does not contain an Application-derived type.");
		}
		catch (ReflectionTypeLoadException ex)
		{
			// Typically a stale or mixed-version guest output (e.g. a theme library compiled
			// against a different Uno.Themes.WinUI than the one beside it).
			var detail = ex.LoaderExceptions is { Length: > 0 } inner && inner[0] is { } first
				? first.Message
				: ex.Message;
			throw new GuestAppLoadException(
				$"{info.AssemblyName} could not be inspected from {session.Alc.GuestDirectory} — the build output may be stale or mixed-version. " +
				$"Rebuild {info.ProjectFolderName} and retry. Detail: {detail}", ex);
		}

		var constructor = applicationType.GetConstructor(Type.EmptyTypes)
			?? throw new GuestAppLoadException($"{applicationType.FullName} has no public parameterless constructor.");

		// Non-generic Func<Application> is load-bearing: App<TApp>(() => new TApp()) would
		// instantiate Func<TApp>, whose shared-generic dictionary entry pins the collectible
		// ALC's LoaderAllocator and blocks unload. preferInterpretation keeps this AOT-safe.
		var factory = Expression.Lambda<Func<Application>>(Expression.New(constructor)).Compile(preferInterpretation: true);
		Func<Application> capturingFactory = () =>
		{
			var app = factory();
			session.GuestApp = app;
			return app;
		};

		var host = BuildGuestHost(capturingFactory);
		StartGuestExecution(session, host);
	}

	private async Task WaitForFirstContentAsync(Session session, Task contentReady, CancellationToken cancellationToken)
	{
		var execution = session.ExecutionTask ?? Task.CompletedTask;
		var timeout = Task.Delay(_contentReadyTimeout, cancellationToken);
		var completed = await Task.WhenAny(contentReady, execution, timeout).ConfigureAwait(false);

		if (completed == execution && !execution.IsFaulted)
		{
			// A run loop that returns is not a lifetime signal on every backend. Win32 keeps one
			// process-wide message loop (Win32Host._isRunning is static), so a hosted guest's
			// RunLoop only *schedules* its Application.Start on the host's loop and returns
			// immediately — X11 instead blocks in a keep-alive loop for the guest's lifetime.
			// Only a faulted run loop is a boot failure; keep waiting for content otherwise.
			completed = await Task.WhenAny(contentReady, timeout).ConfigureAwait(false);
		}

		if (completed == contentReady)
		{
			return;
		}

		if (completed == execution)
		{
			// Faulted by construction here: propagate the run loop's own failure.
			await execution.ConfigureAwait(false);
		}

		cancellationToken.ThrowIfCancellationRequested();
		throw new GuestAppLoadException(
			$"{session.Info.DisplayName} did not present content within {_contentReadyTimeout.TotalSeconds:N0}s.");
	}

	private async Task<bool> TeardownCoreAsync(Session session, IProgress<string>? progress)
	{
		try
		{
			return await TeardownUnguardedAsync(session, progress).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			// A guest exception escaping teardown (e.g. an Unloaded handler throwing while the
			// content region clears, or a failing ALC dispose) must be indistinguishable from
			// "did not stop": callers latch _faulted on false, and nothing may be hosted over a
			// half-dead guest whose content could satisfy a later load's content wait.
			_logger.LogError(ex, "Guest {App} teardown failed; treating the guest as not stopped.", session.Info.AssemblyName);
			session.Alc.DetachDiagnostics();
			progress?.Report($"{session.Info.DisplayName} did not stop cleanly; its resources stay resident until the app restarts.");
			return false;
		}
	}

	private async Task<bool> TeardownUnguardedAsync(Session session, IProgress<string>? progress)
	{
		progress?.Report($"Stopping {session.Info.DisplayName}…");

		// 1. Release the guest's visual tree and projected resources first, so nothing in the
		//    host references guest types while the ALC is being brought down. A guest Unloaded
		//    handler can throw here — degrade and continue; step 6 retries the clear.
		try
		{
			if (!await RunOnUIThreadAsync(() => _contentHost.Content = null).ConfigureAwait(false))
			{
				_logger.LogWarning("Could not clear the guest content region within the teardown budget.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Clearing the guest content region threw; continuing teardown.");
		}

		// 2. Put the host's binding-metadata provider back (the guest overwrote the process-wide
		//    provider and its teardown nulls it; same hygiene as the Uno runtime tests). Done
		//    before any early-out so a stuck guest can't leave the host on a dying provider.
		global::Uno.UI.DataBinding.BindableMetadata.Provider = _hostBindableMetadataProvider;

		// 3. Stop the guest's run loop. Reach differs per backend: X11 blocks for the guest's
		//    lifetime, so a wedged guest is detected here; on Win32 the loop returned at boot
		//    (shared pump) and on WASM there is no separate loop to stop — a wedged guest on
		//    those backends is only caught by a throwing teardown (the guard above).
		var stopped = await StopExecutionAsync(session).ConfigureAwait(false);
		if (!stopped)
		{
			// Never unload an ALC whose code may still be running: leak it and surface it —
			// the caller latches the loader so nothing else is hosted in this process. The
			// AppDomain diagnostics handler is detached so the condemned ALC is not kept
			// rooted by it (and does not fire for the rest of the process lifetime).
			session.Alc.DetachDiagnostics();
			_logger.LogError("Guest {App} run loop did not stop; skipping ALC unload this cycle.", session.Info.AssemblyName);
			progress?.Report($"{session.Info.DisplayName} did not stop cleanly; its resources stay resident until the app restarts.");
			return false;
		}

		// 4. Application.Exit() routes to ExitAlcApplication for ALC-hosted apps: closes ALC
		//    windows, resets the pinned theme, and sweeps the per-ALC static caches. On Win32
		//    the run loop only *schedules* Application.Start on the host's shared pump, so a
		//    fast teardown can get here before the guest Application has constructed — drain
		//    the UI queue once so any pending Start executes before the instance is read.
		if (session.GuestApp is null)
		{
			if (!await RunOnUIThreadAsync(static () => { }).ConfigureAwait(false))
			{
				_logger.LogWarning("Could not drain the UI queue before reading the guest Application instance.");
			}
		}

		if (session.GuestApp is { } guestApp)
		{
			try
			{
				if (await RunOnUIThreadAsync(guestApp.Exit).ConfigureAwait(false))
				{
					_logger.LogInformation("Guest {App} Exit() completed (per-ALC caches swept).", session.Info.AssemblyName);
				}
				else
				{
					_logger.LogWarning("Guest {App} Exit() did not complete within the teardown budget.", session.Info.AssemblyName);
				}
			}
			catch (Exception ex)
			{
				// Teardown continues: a failed sweep must not strand the unload.
				_logger.LogError(ex, "Guest {App} Exit() failed.", session.Info.AssemblyName);
			}
		}
		else
		{
			// Without an Application instance the per-ALC cache sweep never runs and the ALC
			// cannot be reclaimed — this must be loud, not silent.
			_logger.LogWarning("Guest {App} produced no Application instance; Exit()/cache sweep skipped.", session.Info.AssemblyName);
		}

		// 5. (WASM only) The single-threaded run loop can only complete once Exit() has run, so
		//    it is observed here — a pre-Exit wait would burn its full timeout on every unload.
		await ObserveExecutionAfterExitAsync(session).ConfigureAwait(false);

		// 6. A guest that presented content after the load timed out — or a dispatch queued
		//    behind Exit() — can have re-filled the host region; anything still referencing
		//    guest types at unload would pin the dying ALC. If this clear itself throws, the
		//    region cannot be proven clean — fail the teardown rather than report success.
		try
		{
			if (!await RunOnUIThreadAsync(() =>
				{
					if (_contentHost.Content is not null)
					{
						_logger.LogWarning("Guest {App} content re-appeared during teardown; clearing it before unload.", session.Info.AssemblyName);
						_contentHost.Content = null;
					}
				}).ConfigureAwait(false))
			{
				_logger.LogWarning("Could not verify the guest content region was clear before unload.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Re-clearing the guest content region threw; the region cannot be proven clean before unload.");
			session.Alc.DetachDiagnostics();
			progress?.Report($"{session.Info.DisplayName} did not stop cleanly; its resources stay resident until the app restarts.");
			return false;
		}

		// 7. Drop every session reference before unloading so the collectible ALC can go.
		var alc = session.Alc;
		session.GuestApp = null;
		session.ExecutionTask = null;
		session.ExecutionThread = null;

		await Task.Run(alc.Dispose).ConfigureAwait(false);
		_lastUnloadedAlc = new WeakReference<GuestAssemblyLoadContext>(alc);

		// Guest finalizers (DependencyObject teardown) run during unload and can re-populate
		// the shared property-system caches AFTER ExitAlcApplication's sweep — observed via
		// heap dump as a guest ControlExtensions attached-property entry re-rooting the dying
		// ALC. Let the finalizers finish, sweep once more, then collect the unrooted ALC.
		// (Release-before-allocate: WASM memory growth is irreversible.)
		GC.Collect();
		await DrainFinalizersAsync().ConfigureAwait(false);
		if (!await RunOnUIThreadAsync(SweepNonDefaultAlcCaches).ConfigureAwait(false))
		{
			_logger.LogWarning("Post-unload ALC cache sweep could not run on the UI thread; guest ALC memory may stay resident until the next guest exits.");
		}

		GC.Collect();

		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("Guest {App} torn down; ALC unload initiated.", session.Info.AssemblyName);
		}

		return true;
	}

	private void ReportPreviousAlcCollectionState()
	{
		if (_lastUnloadedAlc is not { } weakAlc)
		{
			LastUnloadedAlcCollected = null;
			return;
		}

		if (weakAlc.TryGetTarget(out var previous))
		{
			LastUnloadedAlcCollected = false;
			_logger.LogWarning("Previous guest ALC {Name} is still alive after unload + GC; its memory is not yet reclaimed.", previous.Name);
		}
		else
		{
			LastUnloadedAlcCollected = true;
			_logger.LogInformation("Previous guest ALC was fully collected.");
			_lastUnloadedAlc = null;
		}
	}

	/// <summary>
	/// Forces a collection pass and re-checks whether the most recently unloaded guest ALC has
	/// been reclaimed. Regular loads report this automatically; the hosting smoke calls it
	/// after the final unload (and to retry while finalizers drain).
	/// </summary>
	internal async Task<bool?> VerifyPreviousAlcCollectedAsync()
	{
		if (_lastUnloadedAlc is null)
		{
			return LastUnloadedAlcCollected;
		}

		await DeepCollectAsync().ConfigureAwait(false);
		ReportPreviousAlcCollectionState();
		return LastUnloadedAlcCollected;
	}

	private async Task ObserveAsync(Task execution)
	{
		try
		{
			// Give an already-finishing loop a brief window, then stop waiting; faults are
			// logged rather than propagated because teardown must run to completion.
			await Task.WhenAny(execution, Task.Delay(_executionStopTimeout)).ConfigureAwait(false);
			if (!execution.IsCompleted)
			{
				// Not a stop signal on backends whose loop is scheduled on a shared pump, but
				// it must not be silent either: the guest's code may still be executing.
				_logger.LogWarning("Guest run loop has not completed within {Timeout:N0}s; its code may still be executing.", _executionStopTimeout.TotalSeconds);
			}
			else if (execution.IsFaulted)
			{
				await execution.ConfigureAwait(false);
			}
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Guest run loop completed with an error.");
		}
	}

	private async Task<bool> RunOnUIThreadAsync(Action action)
	{
		var abandoned = false;
		var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		var enqueued = _dispatcherQueue.TryEnqueue(() =>
		{
			// A dispatch that outlived its timeout must not run late against state teardown
			// has already moved past (e.g. an Exit() landing on an unloading ALC).
			if (Volatile.Read(ref abandoned))
			{
				completion.TrySetResult();
				return;
			}

			try
			{
				action();
				completion.TrySetResult();
			}
			catch (Exception ex)
			{
				completion.TrySetException(ex);
			}
		});

		if (!enqueued)
		{
			return false;
		}

		var completed = await Task.WhenAny(completion.Task, Task.Delay(_uiDispatchTimeout)).ConfigureAwait(false);
		if (completed != completion.Task)
		{
			Volatile.Write(ref abandoned, true);
			// A fault that raced the flag must not surface as an unobserved task exception.
			_ = completion.Task.ContinueWith(static t => _ = t.Exception, TaskContinuationOptions.OnlyOnFaulted);
			return false;
		}

		await completion.Task.ConfigureAwait(false);
		return true;
	}

	private static Type? FindApplicationType(Assembly assembly)
	{
		foreach (var type in assembly.GetTypes())
		{
			if (!type.IsAbstract && typeof(Application).IsAssignableFrom(type))
			{
				return type;
			}
		}

		return null;
	}
}
