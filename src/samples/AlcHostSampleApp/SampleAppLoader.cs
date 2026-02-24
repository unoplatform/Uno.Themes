#if HAS_UNO

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Uno.UI.Xaml;
using Uno.UI.Xaml.Controls;

namespace Uno.Themes.AlcHost;

/// <summary>
/// Loads and manages a secondary Uno Platform sample application via AssemblyLoadContext.
/// </summary>
internal sealed class SampleAppLoader : IDisposable
{
    private readonly ILogger? _logger;
    private SampleAppAssemblyLoadContext? _activeContext;
    private Task? _activeExecutionTask;
    private CancellationTokenSource? _activeCancellation;
    private bool _disposed;

    public SampleAppLoader(ILogger? logger = null)
    {
        _logger = logger;
    }

    /// <summary>
    /// Loads the sample application from the specified assembly path into the content host.
    /// </summary>
    /// <param name="assemblyPath">Full path to the compiled sample app assembly (DLL).</param>
    /// <param name="contentHost">The AlcContentHost that will display the secondary app content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the app was loaded successfully.</returns>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ALC loading requires dynamic assembly loading")]
    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Dynamic type access required for ALC scenarios")]
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Dynamic type access required for ALC scenarios")]
    public async Task<bool> LoadApplicationAsync(
        string assemblyPath,
        AlcContentHost contentHost,
        CancellationToken cancellationToken = default)
    {
        if (contentHost is null)
        {
            throw new ArgumentNullException(nameof(contentHost));
        }

        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(SampleAppLoader));
        }

        if (!File.Exists(assemblyPath))
        {
            _logger?.LogError("Assembly file does not exist: {AssemblyPath}", assemblyPath);
            return false;
        }

        // Stop any previously running application
        await StopActiveApplicationAsync().ConfigureAwait(false);

        var assemblyDirectory = Path.GetDirectoryName(assemblyPath)!;

        _logger?.LogInformation("Loading application from: {AssemblyPath}", assemblyPath);

        SampleAppAssemblyLoadContext? pendingContext = null;

        try
        {
            pendingContext = new SampleAppAssemblyLoadContext(assemblyDirectory, _logger);

            // Load the main assembly via stream to avoid file locking
            Assembly assembly;
            using (var stream = File.OpenRead(assemblyPath))
            {
                assembly = pendingContext.LoadFromStream(stream);
            }

            _logger?.LogInformation("Loaded assembly: {AssemblyName}", assembly.GetName());

            // Find Program type with public static Main method
            var appTypes = assembly.GetTypes();
            var programType = appTypes
                .FirstOrDefault(t =>
                    t.Name == "Program" &&
                    t.GetMethod("Main", BindingFlags.Public | BindingFlags.Static) is not null);

            if (programType is null)
            {
                _logger?.LogError("Could not find Program type with public static Main method in {AssemblyPath}", assemblyPath);
                return false;
            }

            var mainMethod = programType.GetMethod("Main", BindingFlags.Public | BindingFlags.Static)!;

            // Verify there's an Application-derived type
            var appType = appTypes
                .FirstOrDefault(t => typeof(Application).IsAssignableFrom(t));

            if (appType is null)
            {
                _logger?.LogError("Could not find Application-derived type in {AssemblyPath}", assemblyPath);
                return false;
            }

            _logger?.LogInformation("Found App type: {AppType}, Program type: {ProgramType}", appType.FullName, programType.FullName);

            // Set up the content host override so the secondary app's Window.Content
            // redirects into our host control.
            // When a secondary ALC app registers itself, Application.HasSecondaryApps
            // is automatically set to true internally.
            WindowHelper.ContentHostOverride = contentHost;

            var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var startSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            // Launch the secondary app on a background task
            var executionTask = Task.Run(() =>
            {
                try
                {
                    _logger?.LogInformation("Invoking Main method for secondary app");
                    startSignal.TrySetResult(true);
                    mainMethod.Invoke(null, [Array.Empty<string>()]);
                }
                catch (OperationCanceledException oce)
                {
                    startSignal.TrySetCanceled(oce.CancellationToken);
                    _logger?.LogInformation("Secondary app execution cancelled");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is OperationCanceledException)
                {
                    startSignal.TrySetCanceled(cancellationToken);
                    _logger?.LogInformation("Secondary app execution cancelled");
                }
                catch (Exception ex)
                {
                    startSignal.TrySetException(ex);
                    _logger?.LogError(ex, "Secondary app execution failed");
                }
            });

            // Track active state
            _activeExecutionTask = executionTask;
            _activeContext = pendingContext;
            _activeCancellation = linkedCancellation;
            pendingContext = null; // Ownership transferred

            // Register cancellation callback
            using var registration = cancellationToken.Register(() =>
            {
                startSignal.TrySetCanceled(cancellationToken);
                linkedCancellation.Cancel();
            });

            var launched = await startSignal.Task.ConfigureAwait(false);

            _logger?.LogInformation("Secondary app launched: {Result}", launched);
            return launched;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load application from {AssemblyPath}", assemblyPath);
            await StopActiveApplicationAsync().ConfigureAwait(false);
            return false;
        }
        finally
        {
            // Dispose pending context if ownership was not transferred
            pendingContext?.Dispose();
        }
    }

    /// <summary>
    /// Stops the currently running secondary application and cleans up.
    /// </summary>
    public async Task StopActiveApplicationAsync()
    {
        var executionTask = _activeExecutionTask;
        var cancellation = _activeCancellation;
        var context = _activeContext;

        _activeExecutionTask = null;
        _activeCancellation = null;
        _activeContext = null;

        if (cancellation is not null)
        {
            try
            {
                await cancellation.CancelAsync().ConfigureAwait(false);
            }
            catch
            {
                // Ignore cancellation errors
            }
        }

        if (executionTask is not null)
        {
            try
            {
                await executionTask.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            }
            catch (TimeoutException)
            {
                _logger?.LogWarning("Secondary app did not stop within 5 seconds");
            }
            catch (OperationCanceledException)
            {
                // Expected
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error while stopping secondary app");
            }
        }

        context?.Dispose();
        cancellation?.Dispose();

        // Clear the content host override
        WindowHelper.ContentHostOverride = null;

        // Force GC to help unload the ALC
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        // Fire-and-forget cleanup with try/catch inside
        _ = StopActiveApplicationAsync();
    }
}
#endif
