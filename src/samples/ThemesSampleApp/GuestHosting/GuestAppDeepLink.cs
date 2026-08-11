#nullable enable

using System;
using System.Linq;
#if __WASM__
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
#endif

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Resolves launch selectors — from the page URL in the browser (<c>?app=material</c>) or
/// from the command line on desktop (<c>--app=material</c>).
/// </summary>
/// <remarks>
/// Only the <c>app</c> and <c>smoke</c> selectors belong to the host. A guest hosted in the
/// browser shares the host's document, so it can read any further selectors (for example
/// <c>sample</c>) straight off the same URL — no host-to-guest plumbing across the ALC
/// boundary is needed.
/// </remarks>
internal static class GuestAppDeepLink
{
	private const string AppParameterName = "app";

	/// <summary>
	/// Gets the catalog entry named by the launch selector, or <see langword="null"/> when no
	/// selector was supplied or it matches no known guest.
	/// </summary>
	public static GuestAppInfo? Resolve()
	{
		if (GetLaunchParameter(AppParameterName) is not { Length: > 0 } requested)
		{
			return null;
		}

		return GuestAppCatalog.Apps.FirstOrDefault(app =>
			string.Equals(app.DisplayName, requested, StringComparison.OrdinalIgnoreCase) ||
			string.Equals(app.ProjectFolderName, requested, StringComparison.OrdinalIgnoreCase));
	}

	/// <summary>
	/// Gets a launch selector by name — <c>?name=value</c> in the browser, <c>--name=value</c>
	/// on the command line — or <see langword="null"/> when absent. A bare flag
	/// (<c>?name</c> / <c>--name</c>) yields an empty string.
	/// </summary>
	public static string? GetLaunchParameter(string name)
	{
#if __WASM__
		return GetQueryParameterFromLocation(name);
#else
		var prefix = "--" + name;
		foreach (var arg in Environment.GetCommandLineArgs())
		{
			if (string.Equals(arg, prefix, StringComparison.OrdinalIgnoreCase))
			{
				return string.Empty;
			}

			if (arg.Length > prefix.Length
				&& arg[prefix.Length] == '='
				&& arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
			{
				return arg[(prefix.Length + 1)..];
			}
		}

		return null;
#endif
	}

	/// <summary>
	/// Gets whether a boolean launch flag is set: present (bare or with a value) and not
	/// explicitly <c>0</c>/<c>false</c>.
	/// </summary>
	public static bool GetLaunchFlag(string name) =>
		GetLaunchParameter(name) is { } value
			&& !string.Equals(value, "0", StringComparison.OrdinalIgnoreCase)
			&& !string.Equals(value, "false", StringComparison.OrdinalIgnoreCase);

#if __WASM__
	[SupportedOSPlatform("browser")]
	private static string? GetQueryParameterFromLocation(string name)
	{
		// Reading the location object directly avoids shipping any JS glue for a one-off lookup.
		using var location = JSHost.GlobalThis.GetPropertyAsJSObject("location");

		return location?.GetPropertyAsString("search") is { Length: > 0 } search
			? GetQueryParameter(search, name)
			: null;
	}
#endif

	private static string? GetQueryParameter(string query, string name)
	{
		foreach (var pair in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
		{
			var separator = pair.IndexOf('=');
			var key = separator >= 0 ? pair.AsSpan(0, separator) : pair.AsSpan();
			if (key.Equals(name.AsSpan(), StringComparison.OrdinalIgnoreCase))
			{
				return separator >= 0
					? Uri.UnescapeDataString(pair[(separator + 1)..])
					: string.Empty;
			}
		}

		return null;
	}
}
