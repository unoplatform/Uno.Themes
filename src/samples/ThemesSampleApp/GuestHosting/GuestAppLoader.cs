using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Uno.UI.Hosting;
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
/// AssemblyLoadContext through a second <see cref="UnoPlatformHostBuilder"/>, and tears it
/// down in the order proven by studio.live (clear content → restore binding provider →
/// stop run loop → Exit → clear late content → unload → sweep + GC).
/// </summary>
internal sealed class GuestAppLoader
{
	// Guests boot a whole Uno app (XAML parse, theme merge, first layout); 30 s matches the
	// upstream runtime-test allowance and is generous enough for cold WASM interpretation.
	private static readonly TimeSpan _contentReadyTimeout = TimeSpan.FromSeconds(30);

	// How long the guest's run loop gets to finish on its own after content is cleared,
	// before the dedicated thread is interrupted (desktop only).
	private static readonly TimeSpan _executionStopTimeout = TimeSpan.FromSeconds(5);

#if !__WASM__
	// First and second Thread.Join windows around Thread.Interrupt during teardown.
	private static readonly TimeSpan _threadJoinInitialTimeout = TimeSpan.FromSeconds(2);
	private static readonly TimeSpan _threadJoinExtendedTimeout = TimeSpan.FromSeconds(3);
#endif

	// Budget for UI-thread dispatches during teardown (content clear, Application.Exit).
	private static readonly TimeSpan _uiDispatchTimeout = TimeSpan.FromSeconds(10);

	private static readonly ILogger _logger =
		global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory.CreateLogger<GuestAppLoader>();

	private readonly AlcContentHost _contentHost;
	private readonly DispatcherQueue _dispatcherQueue;
	private readonly SemaphoreSlim _gate = new(1, 1);
	private Session? _session;

	// Latched when a guest's run loop refused to stop: its code may still execute against the
	// shared content host, where it could satisfy a later load's content wait or re-project
	// resources. Hosting anything else in this process would be built on sand.
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
			DeepCollect();
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

		var builder = UnoPlatformHostBuilder.Create().App(capturingFactory);
#if __WASM__
		builder = builder.UseWebAssembly();
#else
		builder = builder
			.UseX11()
			.UseLinuxFrameBuffer()
			.UseMacOS()
			.UseWin32();
#endif
		var host = builder.Build();

#if __WASM__
		// The browser is single-threaded: RunAsync integrates with the JS event loop and only
		// completes when the guest app exits.
		session.ExecutionTask = Task.Run(host.RunAsync);
#else
		// Desktop: host.Run() blocks for the guest's lifetime, so it owns a dedicated
		// background thread that teardown can Thread.Interrupt if the loop won't stop.
		var runCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		var thread = new Thread(() =>
		{
			try
			{
				host.Run();
				runCompletion.TrySetResult();
			}
			catch (ThreadInterruptedException)
			{
				// Expected when teardown interrupts a run loop that outlived its app.
				runCompletion.TrySetResult();
			}
			catch (Exception ex)
			{
				runCompletion.TrySetException(ex);
			}
		})
		{
			IsBackground = true,
			Name = $"GuestApp-{info.AssemblyName}",
		};

		session.ExecutionThread = thread;
		session.ExecutionTask = runCompletion.Task;
		thread.Start();
#endif
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
		progress?.Report($"Stopping {session.Info.DisplayName}…");

		// 1. Release the guest's visual tree and projected resources first, so nothing in the
		//    host references guest types while the ALC is being brought down.
		if (!await RunOnUIThreadAsync(() => _contentHost.Content = null).ConfigureAwait(false))
		{
			_logger.LogWarning("Could not clear the guest content region within the teardown budget.");
		}

		// 2. Put the host's binding-metadata provider back (the guest overwrote the process-wide
		//    provider and its teardown nulls it; same hygiene as the Uno runtime tests). Done
		//    before any early-out so a stuck guest can't leave the host on a dying provider.
		global::Uno.UI.DataBinding.BindableMetadata.Provider = _hostBindableMetadataProvider;

		// 3. Stop the guest's run loop.
		var stopped = await StopExecutionAsync(session).ConfigureAwait(false);
		if (!stopped)
		{
			// Never unload an ALC whose code may still be running: leak it and surface it —
			// the caller latches the loader so nothing else is hosted in this process.
			_logger.LogError("Guest {App} run loop did not stop; skipping ALC unload this cycle.", session.Info.AssemblyName);
			progress?.Report($"{session.Info.DisplayName} did not stop cleanly; its resources stay resident until the app restarts.");
			return false;
		}

		// 4. Application.Exit() routes to ExitAlcApplication for ALC-hosted apps: closes ALC
		//    windows, resets the pinned theme, and sweeps the per-ALC static caches.
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

#if __WASM__
		// 5. (WASM) The single-threaded run loop can only complete once Exit() has run, so it
		//    is observed here — a pre-Exit wait would burn its full timeout on every unload.
		if (session.ExecutionTask is { } executionAfterExit)
		{
			await ObserveAsync(executionAfterExit).ConfigureAwait(false);
		}
#endif

		// 6. A guest that presented content after the load timed out — or a dispatch queued
		//    behind Exit() — can have re-filled the host region; anything still referencing
		//    guest types at unload would pin the dying ALC.
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
		GC.WaitForPendingFinalizers();
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

	// The three sweeps below compensate verified Uno 6.7-dev per-ALC cleanup gaps, tracked in
	// specs/05-alc-wrapper-app/progress.md → Review → "Upstream issues to file"; replace this
	// pointer with the issue URLs once filed, and delete each sweep when its fix ships.
	// Internal API by necessity; every step degrades to a logged warning (memory stays
	// resident until the next guest exits), never an exception.

	// Same sweep ExitAlcApplication runs, needed a second time after guest finalizers finish.
	private static readonly MethodInfo? _cleanupNonDefaultAlcCaches =
		SafeGetMethod(typeof(Application), "CleanupNonDefaultAlcCaches", BindingFlags.Static | BindingFlags.NonPublic);

	// DependencyProperty._getPropertyCache memoizes (targetType, "ns:Owner.Property") -> DP
	// lookups from style/VSM target paths. A guest style targeting an attached property on a
	// framework element caches a DEFAULT-ALC key (e.g. Button) with a GUEST-ALC value, which
	// Uno's per-key ALC sweep can never remove — pinning the whole guest ALC (verified via
	// heap dump). It is a pure cache over DependencyPropertyRegistry, so clearing it wholesale
	// is safe; it repopulates on demand.
	private static readonly FieldInfo? _getPropertyCacheField =
		SafeGetField(typeof(DependencyProperty), "_getPropertyCache", BindingFlags.Static | BindingFlags.NonPublic);
	private static readonly MethodInfo? _getPropertyCacheClear =
		_getPropertyCacheField is { } cacheField
			? SafeGetMethod(cacheField.FieldType, "Clear", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			: null;

	// Lookup failures surface at the sweep call sites (which log); throwing here would turn a
	// future Uno rename into a TypeInitializationException at app launch.
	private static MethodInfo? SafeGetMethod(Type type, string name, BindingFlags flags)
	{
		try
		{
			return type.GetMethod(name, flags);
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static FieldInfo? SafeGetField(Type type, string name, BindingFlags flags)
	{
		try
		{
			return type.GetField(name, flags);
		}
		catch (Exception)
		{
			return null;
		}
	}

	private void SweepNonDefaultAlcCaches()
	{
		// Each mitigation is independent: one failing must not skip the others.
		try
		{
			if (_cleanupNonDefaultAlcCaches is { } cleanup)
			{
				cleanup.Invoke(null, null);
			}
			else if (_logger.IsEnabled(LogLevel.Warning))
			{
				_logger.LogWarning("Application.CleanupNonDefaultAlcCaches was not found; guest ALC memory may stay resident until the next guest exits.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Application.CleanupNonDefaultAlcCaches failed; guest ALC memory may stay resident.");
		}

		try
		{
			if (_getPropertyCacheField?.GetValue(null) is { } propertyCache && _getPropertyCacheClear is { } clear)
			{
				clear.Invoke(propertyCache, null);
			}
			else if (_logger.IsEnabled(LogLevel.Warning))
			{
				_logger.LogWarning("DependencyProperty._getPropertyCache was not reachable; cross-ALC cache entries may pin the guest ALC.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Clearing DependencyProperty._getPropertyCache failed; cross-ALC cache entries may pin the guest ALC.");
		}

		PruneGuestNavigationHandlers();
	}

	// The samples' Shell subscribes to the process-wide SystemNavigationManager.BackRequested
	// and nothing unsubscribes it when a hosted guest is torn down (Uno's per-ALC sweep does
	// not cover this singleton's event fields), so the whole guest visual tree stays rooted —
	// verified via heap dump. Remove any handler whose target lives in a collectible ALC.
	private static readonly string[] _navigationManagerEventFields = ["_backRequested", "InternalBackRequested"];

	private void PruneGuestNavigationHandlers()
	{
		try
		{
			var manager = global::Windows.UI.Core.SystemNavigationManager.GetForCurrentView();
			var anyFieldFound = false;
			foreach (var fieldName in _navigationManagerEventFields)
			{
				var field = typeof(global::Windows.UI.Core.SystemNavigationManager)
					.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
				if (field is null)
				{
					continue;
				}

				anyFieldFound = true;
				if (field.GetValue(manager) is not MulticastDelegate handlers)
				{
					continue;
				}

				var pruned = (Delegate?)handlers;
				foreach (var handler in handlers.GetInvocationList())
				{
					var targetAlc = handler.Target is { } target
						? System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(target.GetType().Assembly)
						: null;
					if (targetAlc is not null && targetAlc != System.Runtime.Loader.AssemblyLoadContext.Default)
					{
						pruned = Delegate.Remove(pruned, handler);
					}
				}

				if (!ReferenceEquals(pruned, handlers))
				{
					field.SetValue(manager, pruned);
				}
			}

			// Mirror the sibling sweeps: a silent no-op after an Uno rename would let the guest
			// visual tree stay rooted with nothing in the logs to say why.
			if (!anyFieldFound && _logger.IsEnabled(LogLevel.Warning))
			{
				_logger.LogWarning("SystemNavigationManager event fields were not found; guest navigation handlers may keep the guest visual tree rooted.");
			}
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Pruning guest navigation handlers failed; the guest visual tree may stay rooted.");
		}
	}

	private void ReportPreviousAlcCollectionState()
	{
		if (_lastUnloadedAlc is not { } weakAlc)
		{
			return;
		}

		if (weakAlc.TryGetTarget(out var previous))
		{
			_logger.LogWarning("Previous guest ALC {Name} is still alive after unload + GC; its memory is not yet reclaimed.", previous.Name);
		}
		else
		{
			_logger.LogInformation("Previous guest ALC was fully collected.");
			_lastUnloadedAlc = null;
		}
	}

#if __WASM__
	// Single-threaded browser: the run loop only completes after Application.Exit(), so there
	// is no pre-Exit wait (it would burn its full timeout on every unload) and no thread to
	// interrupt. TeardownCoreAsync observes the loop right after Exit() instead.
	private Task<bool> StopExecutionAsync(Session session) => Task.FromResult(true);
#else
	private async Task<bool> StopExecutionAsync(Session session)
	{
		if (session.ExecutionTask is not { } execution)
		{
			return true;
		}

		var finished = await Task.WhenAny(execution, Task.Delay(_executionStopTimeout)).ConfigureAwait(false) == execution;

		var thread = session.ExecutionThread;
		if (!finished && thread is { IsAlive: true })
		{
			// The run loop idles in managed waits; Interrupt breaks it out. Two attempts,
			// matching the reference implementation's initial + extended join windows.
			thread.Interrupt();
			if (!thread.Join(_threadJoinInitialTimeout))
			{
				thread.Interrupt();
				thread.Join(_threadJoinExtendedTimeout);
			}
		}

		if (thread is { IsAlive: true })
		{
			return false;
		}

		await ObserveAsync(execution).ConfigureAwait(false);
		return true;
	}
#endif

	private async Task ObserveAsync(Task execution)
	{
		try
		{
			// Give an already-finishing loop a brief window, then stop waiting; faults are
			// logged rather than propagated because teardown must run to completion.
			await Task.WhenAny(execution, Task.Delay(_executionStopTimeout)).ConfigureAwait(false);
			if (execution is { IsCompleted: true, IsFaulted: true })
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

	private static void DeepCollect()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}

#if __WASM__
	// Payloads fetched once per session are kept in MEMFS and reused on reload. Downloads land
	// in a .partial sibling that is renamed into place only when complete, so a mid-fetch
	// failure can never leave a directory that passes the cache probe.
	private const string _guestPayloadRoot = "/GuestApps";

	private static async Task<string> LocateGuestDirectoryAsync(GuestAppInfo info, CancellationToken cancellationToken)
	{
		var targetDirectory = $"{_guestPayloadRoot}/{info.ProjectFolderName}";
		if (File.Exists(Path.Combine(targetDirectory, info.AssemblyName + ".dll")))
		{
			return targetDirectory;
		}

		var partialDirectory = targetDirectory + ".partial";
		try
		{
			var packageBase = $"ms-appx:///GuestApps/{info.ProjectFolderName}";
			var manifestFile = await global::Windows.Storage.StorageFile
				.GetFileFromApplicationUriAsync(new Uri($"{packageBase}/manifest.txt"))
				.AsTask(cancellationToken)
				.ConfigureAwait(false);
			var names = await global::Windows.Storage.FileIO.ReadLinesAsync(manifestFile)
				.AsTask(cancellationToken)
				.ConfigureAwait(false);

			if (Directory.Exists(partialDirectory))
			{
				Directory.Delete(partialDirectory, recursive: true);
			}

			Directory.CreateDirectory(partialDirectory);
			foreach (var name in names)
			{
				if (string.IsNullOrWhiteSpace(name))
				{
					continue;
				}

				// Manifest lines are build-generated file names; a separator would escape the
				// payload directory.
				if (name.IndexOfAny(['/', '\\']) >= 0 || name.Contains(".."))
				{
					throw new GuestAppLoadException($"The {info.DisplayName} guest manifest contains an invalid entry.");
				}

				cancellationToken.ThrowIfCancellationRequested();
				var payload = await global::Windows.Storage.StorageFile
					.GetFileFromApplicationUriAsync(new Uri($"{packageBase}/{name}.bin"))
					.AsTask(cancellationToken)
					.ConfigureAwait(false);
				var buffer = await global::Windows.Storage.FileIO.ReadBufferAsync(payload)
					.AsTask(cancellationToken)
					.ConfigureAwait(false);

				// Stream the IBuffer out instead of materializing a second byte[] copy: peak
				// managed footprint permanently raises HEAPU8's high-water mark in the browser.
				using var source = global::System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeBufferExtensions.AsStream(buffer);
				using var destination = File.Create(Path.Combine(partialDirectory, name));
				await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
			}

			Directory.Move(partialDirectory, targetDirectory);
		}
		catch (OperationCanceledException)
		{
			TryDeleteDirectory(partialDirectory);
			throw;
		}
		catch (GuestAppLoadException)
		{
			TryDeleteDirectory(partialDirectory);
			throw;
		}
		catch (Exception ex)
		{
			TryDeleteDirectory(partialDirectory);
			throw new GuestAppLoadException(
				$"The {info.DisplayName} guest payload is missing from this build. Build {info.ProjectFolderName} for net10.0-browserwasm before building the wrapper, then rebuild.", ex);
		}

		return targetDirectory;
	}

	private static void TryDeleteDirectory(string directory)
	{
		try
		{
			if (Directory.Exists(directory))
			{
				Directory.Delete(directory, recursive: true);
			}
		}
		catch (IOException)
		{
			// Best effort: the .partial suffix alone keeps leftovers out of the cache probe.
		}
	}
#else
	private static Task<string> LocateGuestDirectoryAsync(GuestAppInfo info, CancellationToken cancellationToken) =>
		Task.Run(() => LocateGuestDirectory(info), cancellationToken);

	// Guests are hosted from their own head's build output; the wrapper stays desktop-TFM-aligned.
	private const string _guestTargetFramework = "net10.0-desktop";

	private static string LocateGuestDirectory(GuestAppInfo info)
	{
		var baseDirectory = AppContext.BaseDirectory;

		// 1. Packaged layout: GuestApps/<Head>/ beside the wrapper binaries (future self-contained drop).
		var packaged = Path.Combine(baseDirectory, "GuestApps", info.ProjectFolderName);
		if (File.Exists(Path.Combine(packaged, info.AssemblyName + ".dll")))
		{
			return packaged;
		}

		// 2. Developer layout: sibling head outputs under src/samples/<Head>/bin/<Config>/<tfm>/.
		//    Anchored on the shared sample project: this probe executes whatever it finds, so a
		//    directory merely named "samples" (a relocated wrapper under some other tree) must
		//    not be trusted.
		if (FindAncestorDirectory(baseDirectory, "samples") is { } samplesDirectory
			&& Directory.Exists(Path.Combine(samplesDirectory, "SamplesApp.Shared")))
		{
			var configurations = new List<string>(3);
			if (GetOwnConfiguration(baseDirectory) is { } ownConfiguration)
			{
				configurations.Add(ownConfiguration);
			}

			if (!configurations.Contains("Debug"))
			{
				configurations.Add("Debug");
			}

			if (!configurations.Contains("Release"))
			{
				configurations.Add("Release");
			}

			string? newestDirectory = null;
			var newestStamp = DateTime.MinValue;
			foreach (var configuration in configurations)
			{
				var candidate = Path.Combine(samplesDirectory, info.ProjectFolderName, "bin", configuration, _guestTargetFramework);
				var assemblyPath = Path.Combine(candidate, info.AssemblyName + ".dll");
				if (!File.Exists(assemblyPath))
				{
					continue;
				}

				var stamp = File.GetLastWriteTimeUtc(assemblyPath);
				if (stamp > newestStamp)
				{
					newestStamp = stamp;
					newestDirectory = candidate;
				}
			}

			if (newestDirectory is not null)
			{
				return newestDirectory;
			}
		}

		throw new GuestAppLoadException(
			$"Could not find {info.AssemblyName}.dll. Build {info.ProjectFolderName} for {_guestTargetFramework} first: " +
			$"dotnet build src/samples/{info.ProjectFolderName}/{info.ProjectFolderName}.csproj -f {_guestTargetFramework}");
	}

	private static string? FindAncestorDirectory(string startDirectory, string directoryName)
	{
		var current = new DirectoryInfo(startDirectory);
		// Bounded walk: bin/<Config>/<tfm> is 3 levels below the project, which sits directly
		// under samples; 10 leaves room for RID or publish segments without scanning to root.
		for (var depth = 0; current is not null && depth < 10; depth++, current = current.Parent)
		{
			if (string.Equals(current.Name, directoryName, StringComparison.OrdinalIgnoreCase))
			{
				return current.FullName;
			}
		}

		return null;
	}

	private static string? GetOwnConfiguration(string baseDirectory)
	{
		var segments = baseDirectory.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		for (var i = 0; i < segments.Length - 1; i++)
		{
			if (string.Equals(segments[i], "bin", StringComparison.OrdinalIgnoreCase))
			{
				return segments[i + 1];
			}
		}

		return null;
	}
#endif
}
