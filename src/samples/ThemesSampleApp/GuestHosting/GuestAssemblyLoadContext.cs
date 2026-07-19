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
	private static readonly string[] _sharedStartsWith =
	[
		"Uno.UI.Runtime.",
		"Uno.UI.FluentTheme",
		"SkiaSharp",
		"HarfBuzzSharp",
		"System",
		"Microsoft.Extensions.",
		"netstandard",
		"mscorlib",
	];

	private readonly string _guestDirectory;
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
			// Tier 1: anything already loaded in the default ALC is shared by simple name.
			// Covers the BCL, Microsoft.Extensions.*, and Uno framework assemblies once warm.
			foreach (var loaded in Default.Assemblies)
			{
				if (string.Equals(loaded.GetName().Name, name, StringComparison.Ordinal))
				{
					return loaded;
				}
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
		Unload();
	}
}
