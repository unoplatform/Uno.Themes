#if HAS_UNO

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;

namespace Uno.Themes.AlcHost;

/// <summary>
/// Custom AssemblyLoadContext for loading secondary Uno Platform sample applications.
/// Shares Uno framework and system assemblies with the host to preserve type identity,
/// while loading app-specific assemblies from the secondary app directory.
/// </summary>
internal sealed class SampleAppAssemblyLoadContext : AssemblyLoadContext, IDisposable
{
    /// <summary>
    /// Assemblies that must be shared exactly (by name) with the host ALC.
    /// </summary>
    private static readonly HashSet<string> SharedEquals = new(StringComparer.OrdinalIgnoreCase)
    {
        "Uno.UI",
        "Uno",
        "Uno.UI.Composition",
        "Uno.Foundation",
        "Uno.Foundation.Logging",
        "Uno.UI.Dispatching",
        "Uno.UI.Lottie",
        "Uno.WinUI.Graphics2DSK",
        "Uno.Core.Extensions.Disposables",
    };

    /// <summary>
    /// Assembly name prefixes that should be shared with the host ALC.
    /// </summary>
    private static readonly string[] SharedStartsWith =
    {
        "Uno.UI.Runtime.",
        "Uno.UI.FluentTheme.",
        "SkiaSharp",
        "HarfBuzzSharp",
        "System",
        "Microsoft.Extensions.",
        "Microsoft.UI.",
        "Windows.",
    };

    private readonly string _basePath;
    private readonly ILogger? _logger;
    private bool _disposed;

    public SampleAppAssemblyLoadContext(string basePath, ILogger? logger = null)
        : base(name: $"SampleAppALC-{Guid.NewGuid():N}", isCollectible: true)
    {
        _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        _logger = logger;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var name = assemblyName.Name;
        if (name is null)
        {
            return null;
        }

        // Tier 0/1: Check if already loaded in default ALC
        var alreadyLoaded = TryGetLoadedDefaultAssembly(name);
        if (alreadyLoaded is not null)
        {
            _logger?.LogDebug("Assembly already loaded in default ALC: {AssemblyName}", assemblyName);
            return alreadyLoaded;
        }

        // Check if this assembly should be shared with the host
        if (ShouldShareWithHost(name))
        {
            var defaultAssembly = TryLoadFromDefaultAlc(assemblyName);
            if (defaultAssembly is not null)
            {
                _logger?.LogDebug("Assembly loaded from default ALC: {AssemblyName}", assemblyName);
                return defaultAssembly;
            }

            _logger?.LogDebug("Default ALC missing assembly, falling back to secondary ALC: {AssemblyName}", assemblyName);
        }

        // Tier 3: App-specific — load from the secondary app's output directory
        var appAssemblyPath = Path.Combine(_basePath, name + ".dll");
        if (File.Exists(appAssemblyPath))
        {
            _logger?.LogDebug("Loading assembly from stream: {AssemblyPath}", appAssemblyPath);

            // Use stream-based loading to avoid file locking
            using var stream = File.OpenRead(appAssemblyPath);
            return LoadFromStream(stream);
        }

        _logger?.LogDebug("Assembly not found: {AssemblyName}", assemblyName);

        return null;
    }

    private Assembly? TryLoadFromDefaultAlc(AssemblyName assemblyName)
    {
        try
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (FileLoadException)
        {
            return null;
        }
        catch (BadImageFormatException)
        {
            return null;
        }
    }

    private static Assembly? TryGetLoadedDefaultAssembly(string assemblyName)
    {
        foreach (var assembly in AssemblyLoadContext.Default.Assemblies)
        {
            var loadedName = assembly.GetName().Name;
            if (loadedName is not null && loadedName.Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
            {
                return assembly;
            }
        }

        return null;
    }

    internal static bool ShouldShareWithHost(string? assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName))
        {
            return false;
        }

        if (SharedEquals.Contains(assemblyName))
        {
            return true;
        }

        for (var i = 0; i < SharedStartsWith.Length; i++)
        {
            if (assemblyName.StartsWith(SharedStartsWith[i], StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        try
        {
            Unload();
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to unload SampleAppAssemblyLoadContext {Name}", Name);
        }

        GC.SuppressFinalize(this);
    }
}
#endif
