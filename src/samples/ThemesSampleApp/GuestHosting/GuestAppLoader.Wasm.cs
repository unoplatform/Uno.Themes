#if __WASM__
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Uno.UI.Hosting;

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Browser-specific loader pieces: the single-threaded run loop, event-loop-driven GC drains,
/// and the packaged <c>GuestApps/</c> payload fetch into MEMFS.
/// </summary>
internal sealed partial class GuestAppLoader
{
	private static UnoPlatformHost BuildGuestHost(Func<Application> factory) =>
		UnoPlatformHostBuilder.Create()
			.App(factory)
			.UseWebAssembly()
			.Build();

	private static void StartGuestExecution(Session session, UnoPlatformHost host)
	{
		// The browser is single-threaded: RunAsync integrates with the JS event loop and only
		// completes when the guest app exits.
		session.ExecutionTask = Task.Run(host.RunAsync);
	}

	// Single-threaded browser: the run loop can only complete once Exit() has run, so there is
	// no pre-Exit wait (it would burn its full timeout on every unload) and no thread to
	// interrupt. ObserveExecutionAfterExitAsync below watches the loop right after Exit()
	// instead; a loop that still doesn't complete is logged there, never silently dropped.
	private Task<bool> StopExecutionAsync(Session session) => Task.FromResult(true);

	private async Task ObserveExecutionAfterExitAsync(Session session)
	{
		if (session.ExecutionTask is { } execution)
		{
			await ObserveAsync(execution).ConfigureAwait(false);
		}
	}

	// Single-threaded browser: finalizers run as event-loop work items — there is no dedicated
	// finalizer thread for WaitForPendingFinalizers to join on. Yield to the event loop between
	// collections so the guest's DependencyObject finalizers actually execute before the
	// post-unload sweep (they can re-populate swept caches; see the teardown comment).
	private static async Task DeepCollectAsync()
	{
		GC.Collect();
		await DrainFinalizersAsync().ConfigureAwait(false);
		GC.Collect();
	}

	private static async Task DrainFinalizersAsync()
	{
		for (var i = 0; i < 5; i++)
		{
			await Task.Delay(50).ConfigureAwait(false);
			GC.Collect();
		}
	}

	// Payloads fetched once per session are kept in MEMFS and reused on reload. Downloads land
	// in a .partial sibling that is renamed into place only when complete, so a mid-fetch
	// failure can never leave a directory that passes the cache probe.
	private const string _guestPayloadRoot = "/GuestApps";

	// Manifest lines are build-generated assembly file names; anything else (separators,
	// traversal sequences, escapes) must be rejected before it reaches a path or URI.
	[GeneratedRegex("^[A-Za-z0-9._-]+$")]
	private static partial Regex ManifestEntryRegex();

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

				if (!ManifestEntryRegex().IsMatch(name))
				{
					throw new GuestAppLoadException($"The {info.DisplayName} guest manifest contains an invalid entry.");
				}

				cancellationToken.ThrowIfCancellationRequested();
				var payload = await global::Windows.Storage.StorageFile
					.GetFileFromApplicationUriAsync(new Uri($"{packageBase}/{name}.bin"))
					.AsTask(cancellationToken)
					.ConfigureAwait(false);

				using var destination = File.Create(Path.Combine(partialDirectory, name));
				using var source = await OpenPayloadStreamAsync(payload, cancellationToken).ConfigureAwait(false);
				await source.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
			}

			Directory.Move(partialDirectory, targetDirectory);
		}
		catch (Exception ex)
		{
			TryDeleteDirectory(partialDirectory);
			if (ex is OperationCanceledException or GuestAppLoadException)
			{
				throw;
			}

			throw new GuestAppLoadException(
				$"The {info.DisplayName} guest payload is missing from this build or could not be downloaded. " +
				$"Build {info.ProjectFolderName} for net10.0-browserwasm before building the wrapper, then rebuild.", ex);
		}

		return targetDirectory;
	}

	private static async Task<Stream> OpenPayloadStreamAsync(global::Windows.Storage.StorageFile payload, CancellationToken cancellationToken)
	{
		try
		{
			// Preferred: stream the payload directly — materializing each file as a single
			// IBuffer raises the peak managed footprint by the largest dll's size, and HEAPU8's
			// high-water mark never comes back down in the browser.
			var randomAccess = await payload.OpenReadAsync().AsTask(cancellationToken).ConfigureAwait(false);
			return global::System.IO.WindowsRuntimeStreamExtensions.AsStreamForRead(randomAccess);
		}
		catch (Exception ex) when (ex is NotImplementedException or NotSupportedException)
		{
			// Graceful fallback when this Uno build cannot stream ms-appx reads: buffer the
			// file, but still stream the IBuffer out instead of a second byte[] copy.
			var buffer = await global::Windows.Storage.FileIO.ReadBufferAsync(payload)
				.AsTask(cancellationToken)
				.ConfigureAwait(false);
			return global::System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeBufferExtensions.AsStream(buffer);
		}
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
		catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
		{
			// Best effort: the .partial suffix alone keeps leftovers out of the cache probe.
		}
	}
}
#endif
