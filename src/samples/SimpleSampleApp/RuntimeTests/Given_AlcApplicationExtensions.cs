using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies <c>application.GetTheme()</c> against a theme built in a secondary
/// <see cref="AssemblyLoadContext"/>, which is the arrangement multi-app hosts run
/// (Hot Design's guest apps, the <c>ThemesSampleApp</c> wrapper in <c>specs/05-alc-wrapper-app</c>)
/// — combined with the nested-dictionary layout of issue #1704.
/// </summary>
/// <remarks>
/// Whether the extension can see a guest's theme at all is decided by the host's assembly-sharing
/// policy, not by depth, so both policies are covered here. The share-vs-isolate markers mirrored
/// below are the wrapper's own, from <c>ThemesSampleApp/GuestHosting/GuestSharedAssemblies.txt</c>.
/// </remarks>
[TestClass]
public class Given_AlcApplicationExtensions
{
	/// <summary>
	/// A guest load context that isolates the assemblies named in <c>isolated</c> and shares
	/// everything else with the default context — above all <c>Uno.UI</c>, so
	/// <see cref="ResourceDictionary"/> keeps one identity across the boundary.
	/// </summary>
	private sealed class GuestAlc(string directory, params string[] isolated)
		: AssemblyLoadContext("GuestAlc-" + string.Join("+", isolated), isCollectible: true)
	{
		protected override Assembly Load(AssemblyName assemblyName)
		{
			if (assemblyName.Name is { } name && isolated.Contains(name, StringComparer.Ordinal))
			{
				var path = Path.Combine(directory, name + ".dll");
				if (File.Exists(path))
				{
					return LoadFromAssemblyPath(path);
				}
			}

			// Everything else resolves from the default context.
			return null;
		}
	}

	private static string OutputDirectory
		=> Path.GetDirectoryName(typeof(Given_AlcApplicationExtensions).Assembly.Location)!;

	/// <summary>
	/// Builds a <c>SimpleTheme</c> inside <paramref name="alc"/>, as a hosted guest application's
	/// own resources would.
	/// </summary>
	/// <param name="alc">The guest load context.</param>
	/// <returns>The guest theme, as the shared <see cref="ResourceDictionary"/> base type.</returns>
	private static ResourceDictionary CreateGuestTheme(GuestAlc alc)
	{
		var guestSimple = alc.LoadFromAssemblyPath(Path.Combine(OutputDirectory, "Uno.Simple.WinUI.dll"));
		var guestThemeType = guestSimple.GetType("Uno.Simple.SimpleTheme")
			?? throw new InvalidOperationException("Uno.Simple.SimpleTheme not found in the guest context.");

		return (ResourceDictionary)Activator.CreateInstance(guestThemeType)!;
	}

	/// <summary>
	/// Nests <paramref name="guestTheme"/> one level below the application's resources, with the
	/// application's own top-level theme taken away, and returns what the extension resolves.
	/// The application's resources are restored before returning.
	/// </summary>
	/// <param name="guestTheme">The theme to merge, one level down.</param>
	/// <returns>The theme the extension found, or <c>null</c>.</returns>
	private static BaseTheme ResolveWithGuestThemeNested(ResourceDictionary guestTheme)
	{
		var appResources = Application.Current.Resources;
		var original = appResources.MergedDictionaries.ToArray();
		var hostThemes = original.OfType<BaseTheme>().ToArray();
		var wrapper = new ResourceDictionary();

		try
		{
			foreach (var hostTheme in hostThemes)
			{
				appResources.MergedDictionaries.Remove(hostTheme);
			}

			wrapper.MergedDictionaries.Add(guestTheme);
			appResources.MergedDictionaries.Add(wrapper);

			return Application.Current.GetTheme();
		}
		finally
		{
			appResources.MergedDictionaries.Remove(wrapper);
			wrapper.MergedDictionaries.Clear();

			appResources.MergedDictionaries.Clear();
			foreach (var dictionary in original)
			{
				appResources.MergedDictionaries.Add(dictionary);
			}
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Themes assembly SHARED with the host (Hot Design's hosting shape): BaseTheme has one
	// identity across the boundary, so reaching a guest's theme is purely a question of depth —
	// which is the gap #1704 closes.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_GuestSharesTheThemesAssembly_Then_NestedGuestThemeIsFound()
	{
		var alc = new GuestAlc(OutputDirectory, "Uno.Simple.WinUI");

		try
		{
			var guestTheme = CreateGuestTheme(alc);

			Assert.IsInstanceOfType(guestTheme, typeof(BaseTheme),
				"With Uno.Themes.WinUI shared, a guest theme is a BaseTheme of the host's own type.");
			Assert.AreNotSame(typeof(Uno.Simple.SimpleTheme), guestTheme.GetType(),
				"The guest's SimpleTheme must come from the guest context, or this proves nothing.");

			Assert.IsNotNull(ResolveWithGuestThemeNested(guestTheme),
				"A guest application's theme, merged one level down as the recommended layout does, " +
				"must be reachable through the instance-based extension.");
		}
		finally
		{
			alc.Unload();
		}
	}

	// ─────────────────────────────────────────────────────────────────────
	// Themes assembly ISOLATED per guest (the wrapper sample's policy, `!Uno.Themes.WinUI`):
	// BaseTheme identity is NOT shared, so no amount of depth makes the guest's theme matchable
	// from the host. Pinned so a recursive walk is never mistaken for cross-ALC support.
	// ─────────────────────────────────────────────────────────────────────

	[TestMethod]
	[RunsOnUIThread]
	public void When_GuestIsolatesTheThemesAssembly_Then_HostSideLookupFindsNothing()
	{
		var alc = new GuestAlc(OutputDirectory, "Uno.Simple.WinUI", "Uno.Themes.WinUI");

		try
		{
			var guestTheme = CreateGuestTheme(alc);

			Assert.IsNotInstanceOfType(guestTheme, typeof(BaseTheme),
				"With Uno.Themes.WinUI isolated, the guest's BaseTheme is a different type entirely.");
			Assert.IsInstanceOfType(guestTheme, typeof(ResourceDictionary),
				"Uno.UI stays shared, so the guest theme still merges into the host's resource graph.");

			Assert.IsNull(ResolveWithGuestThemeNested(guestTheme),
				"A guest theme built from an isolated Uno.Themes.WinUI cannot be matched by type from " +
				"the host, at any depth. Reaching it requires the guest to resolve its own Application " +
				"from inside its own context, or a name-based match.");
		}
		finally
		{
			alc.Unload();
		}
	}
}
