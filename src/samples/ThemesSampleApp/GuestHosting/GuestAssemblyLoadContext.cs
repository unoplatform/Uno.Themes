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
/// try to share the theme libraries the wrapper deliberately does not carry. The concrete
/// rules are data, not code: <c>GuestSharedAssemblies.txt</c> (embedded here, also consumed
/// by the wasm payload filter in the csproj) is the single source of truth.
/// </remarks>
internal sealed class GuestAssemblyLoadContext : AssemblyLoadContext, IDisposable
{
	// The share-vs-isolate rules live in ONE place — GuestSharedAssemblies.txt, embedded in
	// this assembly and read at build time by the _IncludeGuestWasmAssemblies payload filter
	// in ThemesSampleApp.csproj. See the file's header for format and invariants.
	private const string _rulesResourceName = "GuestSharedAssemblies.txt";

	// Repo-built theme libraries must always load per-ALC from the guest directory ('!' rules),
	// even when the host already has a same-named assembly loaded — see the rules file.
	private static readonly string[] _isolatedStartsWith;

	// Assemblies shared with the default ALC on exact simple-name match ('=' and '~' rules).
	private static readonly string[] _sharedEquals;

	// Assemblies shared with the default ALC on prefix match ('^' rules).
	private static readonly string[] _sharedStartsWith;

	static GuestAssemblyLoadContext()
	{
		var isolated = new List<string>();
		var exact = new List<string>();
		var prefixes = new List<string>();

		using var stream = typeof(GuestAssemblyLoadContext).Assembly.GetManifestResourceStream(_rulesResourceName)
			?? throw new InvalidOperationException(
				$"Embedded resource '{_rulesResourceName}' is missing; guest assembly resolution rules are unavailable.");
		using var reader = new StreamReader(stream);
		while (reader.ReadLine() is { } line)
		{
			line = line.Trim();
			if (line.Length < 2 || line[0] == '#')
			{
				continue;
			}

			var name = line[1..].Trim();
			switch (line[0])
			{
				case '!':
					isolated.Add(name);
					break;
				case '=':
				case '~':
					exact.Add(name);
					break;
				case '^':
					prefixes.Add(name);
					break;
			}
		}

		_isolatedStartsWith = [.. isolated];
		_sharedEquals = [.. exact];
		_sharedStartsWith = [.. prefixes];
	}

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

	// Only a load into the DEFAULT context changes tier-1 resolution. Guest loads (this very
	// ALC) fire this event once per assembly during boot — invalidating on those would rebuild
	// the snapshot O(loaded × binds), the exact cost the snapshot exists to avoid.
	private void OnAssemblyLoaded(object? sender, AssemblyLoadEventArgs args)
	{
		if (GetLoadContext(args.LoadedAssembly) == Default)
		{
			Volatile.Write(ref _defaultAssembliesByName, null);
		}
	}

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
	/// Detaches the process-wide <see cref="AppDomain.AssemblyLoad"/> diagnostics handler
	/// without initiating unload. Called when the context must be deliberately leaked (its
	/// code may still be running): the handler would otherwise keep this instance rooted and
	/// fire on every future assembly load for the process lifetime.
	/// </summary>
	/// <remarks>
	/// The tier-1 snapshot can go stale from here on — acceptable for a condemned context
	/// whose loader has latched itself faulted.
	/// </remarks>
	public void DetachDiagnostics() =>
		AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoaded;

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
		DetachDiagnostics();
		Unload();
	}
}
