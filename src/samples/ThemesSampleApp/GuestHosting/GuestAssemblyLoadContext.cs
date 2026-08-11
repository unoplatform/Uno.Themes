using System.Reflection;
using System.Runtime.Loader;

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Collectible <see cref="AssemblyLoadContext"/> for one hosted guest app.
/// </summary>
/// <remarks>
/// Share-vs-isolate policy (mirrors studio.live's proven <c>AppAssemblyLoadContext</c>):
/// framework assemblies whose types cross the host/guest boundary (Uno.UI, Skia, BCL, ...)
/// must resolve from the default ALC for type identity; everything else — the theme libraries
/// (<c>Uno.Themes.WinUI</c>, <c>Uno.Material.WinUI</c>, ...), ShowMeTheXAML, MSTest and the
/// guest head itself — loads per-ALC from the guest directory so each guest gets isolated
/// statics and resource registrations. Never blanket-share <c>Uno.*</c>: that would wrongly
/// try to share the theme libraries the wrapper deliberately does not carry.
/// </remarks>
internal sealed class GuestAssemblyLoadContext : AssemblyLoadContext, IDisposable
{
	// KEEP IN SYNC with the _GuestWasmExcluded filter in ThemesSampleApp.csproj: every name
	// excluded from the wasm guest payload MUST be resolvable through these share tiers
	// (tier 2 against the wrapper's own closure or the shared framework), or wasm guests
	// fail to bind it at runtime.

	// Repo-built theme libraries must always load per-ALC from the guest directory, even when
	// the host already has a same-named assembly loaded: the Uno SDK's Debug-only Hot Design
	// tooling (Uno.UI.HotDesign) depends on the *published* Uno.Themes.WinUI package, and the
	// dev-server client eagerly loads it into the default ALC at startup. Sharing that copy
	// would bind a repo-built guest against a mismatched theme base library (TypeLoadException
	// at guest boot). StartsWith so the *.Markup satellites stay isolated too.
	private static readonly string[] _isolatedStartsWith =
	[
		"Uno.Themes.WinUI",
		"Uno.Material.WinUI",
		"Uno.Cupertino.WinUI",
		"Uno.Simple.WinUI",
	];

	// Assemblies shared with the default ALC when the simple name matches exactly.
	private static readonly string[] _sharedEquals =
	[
		"Uno.UI",
		"Uno",
		"Uno.UI.Composition",
		"Uno.Foundation",
		"Uno.Foundation.Logging",
		"Uno.UI.Dispatching",
		"Uno.WinUI.Graphics2DSK",
		"Uno.UI.Lottie",
		"Microsoft.CSharp",
	];

	// Assemblies shared with the default ALC when the simple name starts with the prefix.
	// "Uno.UI.Runtime." keeps its trailing dot so "Uno.UI.RuntimeTests*" stays per-ALC.
	// "Microsoft.Win32"/"Microsoft.VisualBasic" are shared-framework facades resolvable from
	// the default ALC even when the wrapper never loaded them; "Uno.UI.Adapter." ships in the
	// wrapper's own closure.
	private static readonly string[] _sharedStartsWith =
	[
		"Uno.UI.Runtime.",
		"Uno.UI.FluentTheme",
		"Uno.UI.Adapter.",
		"SkiaSharp",
		"HarfBuzzSharp",
		"System",
		"Microsoft.Extensions.",
		"Microsoft.Win32",
		"Microsoft.VisualBasic",
		"netstandard",
		"mscorlib",
	];

	private readonly string _guestDirectory;
	private Dictionary<string, Assembly>? _defaultAssembliesByName;
	private volatile bool _disposed;

	/// <summary>
	/// Initializes a collectible load context for one guest app.
	/// </summary>
	/// <param name="guestAppName">Display/assembly name of the guest, used in the context name.</param>
	/// <param name="guestDirectory">Directory holding the guest's assemblies.</param>
	public GuestAssemblyLoadContext(string guestAppName, string guestDirectory)
		: base(name: $"GuestALC-{guestAppName}-{Guid.NewGuid():N}", isCollectible: true)
	{
		_guestDirectory = guestDirectory;
		AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoaded;
	}

	// Any newly loaded assembly (any context) invalidates the tier-1 snapshot below.
	private void OnAssemblyLoaded(object? sender, AssemblyLoadEventArgs args) =>
		Volatile.Write(ref _defaultAssembliesByName, null);

	/// <summary>
	/// Gets the directory the guest's isolated assemblies are loaded from.
	/// </summary>
	public string GuestDirectory => _guestDirectory;

	/// <inheritdoc />
	protected override Assembly? Load(AssemblyName assemblyName)
	{
		if (_disposed || assemblyName.Name is not { } name)
		{
			return null;
		}

		try
		{
			// The theme libraries under test never resolve through the share tiers (see
			// _isolatedStartsWith): the guest directory below is their only valid source.
			if (!IsIsolatedFromHost(name))
			{
				// Tier 1: anything already loaded in the default ALC is shared by simple name.
				// Covers the BCL, Microsoft.Extensions.*, and Uno framework assemblies once warm.
				// Snapshot rebuilt lazily after any assembly load — a per-probe scan would be
				// O(loaded × binds) with an AssemblyName allocation per step.
				var defaultAssemblies = Volatile.Read(ref _defaultAssembliesByName) ?? BuildDefaultAssemblyMap();
				if (defaultAssemblies.TryGetValue(name, out var loaded))
				{
					return loaded;
				}

				// Tier 2: explicit share list — resolve through the default ALC so host and guest
				// agree on type identity. Fall through on failure (e.g. the wrapper doesn't carry it).
				if (ShouldShareWithHost(name))
				{
					try
					{
						return Default.LoadFromAssemblyName(assemblyName);
					}
					catch (Exception ex) when (ex is FileNotFoundException or FileLoadException or BadImageFormatException)
					{
						// Not available host-side; the guest directory may still satisfy it below.
					}
				}
			}

			// Tier 3: guest directory, isolated per-ALC. LoadFromStream on desktop so the head's
			// output files stay unlocked (rebuildable while the wrapper runs); MEMFS on WASM has
			// no such locking and path-based loading lets the runtime reuse the mapped image.
			var candidate = Path.Combine(_guestDirectory, name + ".dll");
			if (File.Exists(candidate))
			{
				if (OperatingSystem.IsBrowser())
				{
					return LoadFromAssemblyPath(candidate);
				}

				using var stream = File.OpenRead(candidate);
				return LoadFromStream(stream);
			}
		}
		catch (InvalidOperationException) when (_disposed)
		{
			// Unloaded while a resolve was in flight; treat as unresolvable.
		}

		return null;
	}

	private Dictionary<string, Assembly> BuildDefaultAssemblyMap()
	{
		var map = new Dictionary<string, Assembly>(StringComparer.Ordinal);
		foreach (var assembly in Default.Assemblies)
		{
			if (assembly.GetName().Name is { } simpleName)
			{
				map[simpleName] = assembly;
			}
		}

		Volatile.Write(ref _defaultAssembliesByName, map);
		return map;
	}

	private static bool IsIsolatedFromHost(string simpleName)
	{
		foreach (var prefix in _isolatedStartsWith)
		{
			if (simpleName.StartsWith(prefix, StringComparison.Ordinal))
			{
				return true;
			}
		}

		return false;
	}

	private static bool ShouldShareWithHost(string simpleName)
	{
		foreach (var shared in _sharedEquals)
		{
			if (string.Equals(simpleName, shared, StringComparison.Ordinal))
			{
				return true;
			}
		}

		foreach (var prefix in _sharedStartsWith)
		{
			if (simpleName.StartsWith(prefix, StringComparison.Ordinal))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Marks the context as disposed and initiates unload. Safe to call once only.
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoaded;
		Unload();
	}
}
