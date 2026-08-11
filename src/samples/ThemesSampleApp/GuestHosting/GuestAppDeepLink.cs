#nullable enable

using System;
using System.Linq;
#if __WASM__
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
#endif

namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Resolves the guest app requested at launch — from the page URL in the browser
/// (<c>?app=material</c>) or from the command line on desktop (<c>--app=material</c>).
/// </summary>
/// <remarks>
/// Only the <c>app</c> selector belongs to the host. A guest hosted in the browser shares the
/// host's document, so it can read any further selectors (for example <c>sample</c>) straight
/// off the same URL — no host-to-guest plumbing across the ALC boundary is needed.
/// </remarks>
internal static class GuestAppDeepLink
{
	private const string AppParameterName = "app";
	private const string CommandLinePrefix = "--" + AppParameterName + "=";

	/// <summary>
	/// Gets the catalog entry named by the launch selector, or <see langword="null"/> when no
	/// selector was supplied or it matches no known guest.
	/// </summary>
	public static GuestAppInfo? Resolve()
	{
		if (GetRequestedAppName() is not { Length: > 0 } requested)
		{
			return null;
		}

		return GuestAppCatalog.Apps.FirstOrDefault(app =>
			string.Equals(app.DisplayName, requested, StringComparison.OrdinalIgnoreCase) ||
			string.Equals(app.ProjectFolderName, requested, StringComparison.OrdinalIgnoreCase));
	}

	private static string? GetRequestedAppName()
	{
#if __WASM__
		return GetQueryParameterFromLocation(AppParameterName);
#else
		var value = Environment.GetCommandLineArgs()
			.FirstOrDefault(arg => arg.StartsWith(CommandLinePrefix, StringComparison.OrdinalIgnoreCase));

		return value?[CommandLinePrefix.Length..];
#endif
	}

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
			if (separator > 0 &&
				pair.AsSpan(0, separator).Equals(name.AsSpan(), StringComparison.OrdinalIgnoreCase))
			{
				return Uri.UnescapeDataString(pair[(separator + 1)..]);
			}
		}

		return null;
	}
}
