#if HAS_UNO

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Uno.Themes.AlcHost;

/// <summary>
/// Custom AssemblyLoadContext for loading secondary Uno Platform sample applications.
/// Loads assemblies from the secondary app directory except for a curated allowlist and
/// prefix-based shared set (with explicit exclusions) that must be loaded from the default ALC
/// to preserve type identity and interop.
/// </summary>
internal sealed class SampleAppAssemblyLoadContext : AssemblyLoadContext, IAsyncDisposable
{
	private static readonly string[] SharedEquals =
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

	private static readonly HashSet<string> SharedEqualsSet = new(SharedEquals, StringComparer.OrdinalIgnoreCase);

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

	private static readonly string[] SharedExclusions =
	{
		// Non-BCL "System.*" NuGet assembly that may not be present in the host's default ALC; load from the app output instead of sharing.
		"System.Linq.Async",
		// Hot Design assemblies should always load from the secondary app to allow version independence.
		"Uno.UI.HotDesign.Chat",
		"Uno.UI.HotDesign.Client",
		"Uno.UI.HotDesign.Client.Core",
		"Uno.UI.HotDesign.Hierarchy",
		"Uno.UI.HotDesign.PropertyGrid",
		"Uno.UI.HotDesign.Stories",
		"Uno.UI.HotDesign.Toolbox",
	};

	private static readonly HashSet<string> SharedExclusionsSet = new(SharedExclusions, StringComparer.OrdinalIgnoreCase);

	private readonly string _basePath;
	private readonly AlcContentPool? _hostPool;
	private readonly ILogger? _logger;
	private bool _disposed;

	public SampleAppAssemblyLoadContext(string basePath, ILogger? logger = null, AlcContentPool? hostPool = null)
		: base(name: $"SampleAppALC-{Guid.NewGuid()}", isCollectible: true)
	{
		_basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
		_logger = logger;
		_hostPool = hostPool;
	}

	[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "ALC loading requires dynamic assembly loading which is incompatible with trimming")]
	protected override Assembly? Load(AssemblyName assemblyName)
	{
		_logger?.LogDebug("Searching assembly: {AssemblyName}", assemblyName);

		var name = assemblyName.Name;
		if (name is null)
		{
			return null;
		}

		var isSharedExclusion = IsSharedExclusion(name);
		if (isSharedExclusion)
		{
			_logger?.LogDebug(
				"SharedExclusion assembly: {AssemblyName}, HasHostPool={HasHostPool} — skipping Tiers 0/1",
				assemblyName, _hostPool is not null);

			// DIAGNOSTIC: For SharedExclusion assemblies, log what the pool contains
			if (_hostPool is not null && _logger?.IsEnabled(LogLevel.Debug) == true)
			{
				var allFiles = _hostPool.ListFiles();
				var matchingFiles = allFiles.Where(f => f.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
				if (matchingFiles.Count > 0)
				{
					_logger.LogDebug("Pool contains {Count} matching entries for '{Name}': {Entries}",
						matchingFiles.Count, name, string.Join(", ", matchingFiles));
				}
				else
				{
					_logger.LogWarning("Pool does NOT contain any entries matching '{Name}' (total pool entries: {TotalCount})",
						name, allFiles.Count);
				}
			}
		}

		// Tier 0/1: Check if already loaded in default ALC (skip for shared exclusions)
		if (!isSharedExclusion)
		{
			var alreadyLoaded = TryGetLoadedDefaultAssembly(name);
			if (alreadyLoaded is not null)
			{
				_logger?.LogDebug("Assembly already loaded in default ALC: {AssemblyName}", assemblyName);
				return alreadyLoaded;
			}
		}

		// Let shared assemblies be loaded from the default ALC first.
		// If the default ALC cannot resolve it (e.g. trimmed forwarding assemblies), fall back to
		// the host pool (Tier 2) or the secondary app output (Tier 3).
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

		// Tier 2: Aligned deps -> LoadFromAssemblyPath via host pool (MonoVM image reuse).
		// Produces a separate Assembly instance (ALC isolation) but shares the native image
		// when multiple ALCs load from the same physical path.
		var hostPath = _hostPool?.ResolveByFileName(name + ".dll");
		if (hostPath is not null)
		{
			_logger?.LogDebug("Loading assembly from host pool: {AssemblyPath}", hostPath);
			return LoadFromAssemblyPath(hostPath);
		}

		// Tier 3: App-specific -> LoadFromStream (hot reload / deps not in host).
		var appAssemblyPath = Path.Combine(_basePath, name + ".dll");
		if (File.Exists(appAssemblyPath))
		{
			_logger?.LogDebug("Loading assembly from stream: {AssemblyPath}", appAssemblyPath);

			// Use stream-based loading to avoid file locking.
			// LoadFromAssemblyPath keeps the file locked, preventing reload on Windows.
			using var stream = File.OpenRead(appAssemblyPath);
			return LoadFromStream(stream);
		}

		_logger?.LogDebug("Assembly not found: {AssemblyName}", assemblyName);

		// Fall back to default resolution
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

	public ValueTask DisposeAsync()
	{
		DisposeInternal();
		return ValueTask.CompletedTask;
	}

	public void Dispose()
	{
		DisposeInternal();
		GC.SuppressFinalize(this);
	}

	private void DisposeInternal()
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
	}

	internal static bool ShouldShareWithHost(string? assemblyName)
	{
		if (string.IsNullOrWhiteSpace(assemblyName))
		{
			return false;
		}

		if (IsSharedExclusion(assemblyName))
		{
			return false;
		}

		if (SharedEqualsSet.Contains(assemblyName))
		{
			return true;
		}

		for (var i = 0; i < SharedStartsWith.Length; i++)
		{
			var prefix = SharedStartsWith[i];
			if (assemblyName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}

	internal static bool IsSharedExclusion(string? assemblyName)
	{
		if (string.IsNullOrWhiteSpace(assemblyName))
		{
			return false;
		}

		for (var i = 0; i < SharedExclusions.Length; i++)
		{
			if (assemblyName.Contains(SharedExclusions[i], StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}
}
#endif
