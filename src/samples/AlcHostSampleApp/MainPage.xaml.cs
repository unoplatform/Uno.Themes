#if HAS_UNO
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uno.UI.Xaml.Controls;

namespace Uno.Themes.AlcHost;

/// <summary>
/// Main page for the ALC Host application.
/// Provides UI to select and load either MaterialSampleApp or CupertinoSampleApp
/// into a secondary AssemblyLoadContext.
/// </summary>
public sealed partial class MainPage : Page
{
    private readonly ILogger? _logger;
    private readonly SampleAppLoader _appLoader;
    private AlcContentHost? _contentHost;
    private CancellationTokenSource? _loadCts;

    public MainPage()
    {
        this.InitializeComponent();

        var factory = Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory;
        _logger = factory?.CreateLogger<MainPage>();
        _appLoader = new SampleAppLoader(factory?.CreateLogger<SampleAppLoader>());
    }

    private async void OnLoadMaterialClick(object sender, RoutedEventArgs e)
    {
        try
        {
            await LoadSampleAppAsync("MaterialSampleApp");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load MaterialSampleApp");
            StatusText.Text = $"Error: {ex.Message}";
        }
    }

    private async void OnLoadCupertinoClick(object sender, RoutedEventArgs e)
    {
        try
        {
            await LoadSampleAppAsync("CupertinoSampleApp");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load CupertinoSampleApp");
            StatusText.Text = $"Error: {ex.Message}";
        }
    }

    private async void OnUnloadClick(object sender, RoutedEventArgs e)
    {
        try
        {
            await UnloadCurrentAppAsync();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to unload app");
        }
    }

    private async Task LoadSampleAppAsync(string sampleAppName)
    {
        // Cancel any in-progress load
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();

        SetLoadingState(sampleAppName);

        // Unload current app first
        await UnloadCurrentAppAsync();

        var assemblyPath = ResolveSampleAppAssemblyPath(sampleAppName);
        if (assemblyPath is null)
        {
            StatusText.Text = $"Could not find {sampleAppName} assembly. Build it first.";
            SetIdleState();
            return;
        }

        _logger?.LogInformation("Resolved assembly path: {AssemblyPath}", assemblyPath);

        // Create and configure the AlcContentHost
        _contentHost = new AlcContentHost();
        HostContainer.Content = _contentHost;
        PlaceholderPanel.Visibility = Visibility.Collapsed;

        var success = await _appLoader.LoadApplicationAsync(assemblyPath, _contentHost, _loadCts.Token);

        if (success)
        {
            StatusText.Text = $"{sampleAppName} loaded";
            UnloadButton.IsEnabled = true;
            _logger?.LogInformation("{SampleAppName} loaded successfully", sampleAppName);
        }
        else
        {
            StatusText.Text = $"Failed to load {sampleAppName}";
            await UnloadCurrentAppAsync();
        }

        LoadMaterialButton.IsEnabled = true;
        LoadCupertinoButton.IsEnabled = true;
    }

    private async Task UnloadCurrentAppAsync()
    {
        await _appLoader.StopActiveApplicationAsync();

        if (_contentHost is not null)
        {
            HostContainer.Content = null;
            _contentHost = null;
        }

        PlaceholderPanel.Visibility = Visibility.Visible;
        UnloadButton.IsEnabled = false;
        StatusText.Text = "No app loaded";
    }

    private void SetLoadingState(string sampleAppName)
    {
        StatusText.Text = $"Loading {sampleAppName}...";
        LoadMaterialButton.IsEnabled = false;
        LoadCupertinoButton.IsEnabled = false;
    }

    private void SetIdleState()
    {
        LoadMaterialButton.IsEnabled = true;
        LoadCupertinoButton.IsEnabled = true;
    }

    /// <summary>
    /// Resolves the path to the compiled sample app DLL.
    /// Searches the expected build output locations relative to this project.
    /// </summary>
    private string? ResolveSampleAppAssemblyPath(string sampleAppName)
    {
        // The sample apps should be pre-built. We look for their output in:
        // 1. A configurable path from environment variable
        // 2. Adjacent build output directories
        // 3. Standard bin/Debug output of sibling projects

        // Check environment variable first (useful for CI)
        var envPath = Environment.GetEnvironmentVariable($"ALC_SAMPLE_{sampleAppName.ToUpperInvariant()}_PATH");
        if (!string.IsNullOrEmpty(envPath) && File.Exists(envPath))
        {
            return envPath;
        }

        // Try to find the assembly relative to the current app's base directory
        var baseDir = AppContext.BaseDirectory;
        _logger?.LogInformation("App base directory: {BaseDir}", baseDir);

        // Look in common locations relative to the base directory
        var searchPaths = new[]
        {
            // Same output directory (if sample apps are copied here)
            Path.Combine(baseDir, $"{sampleAppName}.dll"),

            // Sibling project output (Debug, same TFM)
            FindSiblingProjectOutput(baseDir, sampleAppName, "Debug"),

            // Sibling project output (Release, same TFM)
            FindSiblingProjectOutput(baseDir, sampleAppName, "Release"),
        };

        foreach (var path in searchPaths)
        {
            if (path is not null && File.Exists(path))
            {
                _logger?.LogInformation("Found sample app assembly at: {Path}", path);
                return path;
            }
        }

        _logger?.LogWarning("Could not find {SampleAppName} assembly in any expected location", sampleAppName);
        return null;
    }

    /// <summary>
    /// Attempts to find the build output of a sibling sample app project.
    /// </summary>
    private static string? FindSiblingProjectOutput(string baseDir, string sampleAppName, string configuration)
    {
        // Navigate from: AlcHostSampleApp/bin/Debug/net10.0-desktop/
        // To:            {SampleAppName}/bin/Debug/net10.0-desktop/
        try
        {
            var dir = new DirectoryInfo(baseDir);

            // Walk up to find the project root (looking for bin directory)
            while (dir is not null && dir.Name != "bin")
            {
                dir = dir.Parent;
            }

            if (dir?.Parent?.Parent is null)
            {
                return null;
            }

            // dir is now "bin", parent is "AlcHostSampleApp", parent.parent is "samples"
            var samplesDir = dir.Parent.Parent;
            var tfmDir = new DirectoryInfo(baseDir).Name; // e.g., "net10.0-desktop"

            var candidatePath = Path.Combine(
                samplesDir.FullName,
                sampleAppName,
                "bin",
                configuration,
                tfmDir,
                $"{sampleAppName}.dll");

            return candidatePath;
        }
        catch
        {
            return null;
        }
    }
}

#else

namespace Uno.Themes.AlcHost;

public sealed partial class MainPage : Microsoft.UI.Xaml.Controls.Page
{
    public MainPage()
    {
        this.InitializeComponent();
    }
}

#endif
