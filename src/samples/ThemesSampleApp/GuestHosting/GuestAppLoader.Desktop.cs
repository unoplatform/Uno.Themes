#if !__WASM__
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Uno.UI.Hosting;

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Desktop-specific loader pieces: the dedicated guest run-loop thread (with interrupt-based
/// stop), synchronous GC drains, and sibling-bin guest discovery.
/// </summary>
internal sealed partial class GuestAppLoader
{
	// First and second Thread.Join windows around Thread.Interrupt during teardown.
	private static readonly TimeSpan _threadJoinInitialTimeout = TimeSpan.FromSeconds(2);
	private static readonly TimeSpan _threadJoinExtendedTimeout = TimeSpan.FromSeconds(3);

	private static UnoPlatformHost BuildGuestHost(Func<Application> factory) =>
		UnoPlatformHostBuilder.Create()
			.App(factory)
			.UseX11()
			.UseLinuxFrameBuffer()
			.UseMacOS()
			.UseWin32()
			.Build();

	private static void StartGuestExecution(Session session, UnoPlatformHost host)
	{
		// Desktop: host.Run() owns a dedicated background thread that teardown can
		// Thread.Interrupt if the loop won't stop. X11 blocks here for the guest's lifetime;
		// Win32 gates its pump on a static _isRunning, so a hosted guest's Run() merely
		// schedules Application.Start on the host's shared loop and returns at once — the
		// thread is load-bearing only on X11-style backends.
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
			Name = $"GuestApp-{session.Info.AssemblyName}",
		};

		session.ExecutionThread = thread;
		session.ExecutionTask = runCompletion.Task;
		thread.Start();
	}

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
			// Only reachable on X11-style backends — on Win32 the loop returned at boot, so
			// `finished` is trivially true and a wedged guest is not detectable here (see
			// step 3 of the teardown).
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

	// Desktop: the run loop is stopped before Exit(), so there is nothing left to observe.
	private Task ObserveExecutionAfterExitAsync(Session session) => Task.CompletedTask;

	// Desktop has a real finalizer thread; the synchronous waits are reliable.
	private static Task DeepCollectAsync()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		return Task.CompletedTask;
	}

	private static Task DrainFinalizersAsync()
	{
		GC.WaitForPendingFinalizers();
		return Task.CompletedTask;
	}

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
			var ownConfiguration = GetOwnConfiguration(baseDirectory);

			var configurations = new List<string>(3);
			if (ownConfiguration is not null)
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
			string? newestConfiguration = null;
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
					newestConfiguration = configuration;
				}
			}

			if (newestDirectory is not null)
			{
				// Newest-wins keeps the dev loop convenient, but a silent configuration
				// mismatch (Debug wrapper hosting a newer Release guest, or vice versa) can
				// mask staleness — surface it.
				if (ownConfiguration is not null
					&& !string.Equals(newestConfiguration, ownConfiguration, StringComparison.OrdinalIgnoreCase)
					&& _logger.IsEnabled(LogLevel.Warning))
				{
					_logger.LogWarning(
						"Hosting {App} from its {GuestConfiguration} output while the wrapper runs {HostConfiguration}; rebuild the guest in the wrapper's configuration if this is unintended.",
						info.AssemblyName, newestConfiguration, ownConfiguration);
				}

				return newestDirectory;
			}
		}

		throw new GuestAppLoadException(
			$"Could not find {info.AssemblyName}.dll. Build {info.ProjectFolderName} for {_guestTargetFramework} first: " +
			$"dotnet build src/samples/{info.ProjectFolderName}/{info.ProjectFolderName}.csproj -f {_guestTargetFramework} -p:TargetFrameworkOverride=desktop");
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
}
#endif
