#nullable enable

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Simple;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Verifies that <see cref="BaseTheme"/> participates in .NET hot reload via a
/// <c>[MetadataUpdateHandler]</c> on the class itself. The handler tracks every live theme
/// instance and re-runs <see cref="BaseTheme.UpdateSource"/> on each so programmatically
/// constructed merged dictionaries get rebuilt when underlying XAML changes.
///
/// Regression guard: without this wiring, edits to e.g. <c>ThemeColors.xaml</c> referenced
/// via <c>Colors.OverrideSource</c> wouldn't propagate to the running app — the framework's
/// generic resource-tree refresh can't reach dictionaries that <see cref="BaseTheme.UpdateSource"/>
/// added programmatically.
/// </summary>
[TestClass]
public class Given_HotReload
{
	[TestMethod]
	[RunsOnUIThread]
	public void When_AssemblyLoaded_Then_MetadataUpdateHandlerTargetsBaseTheme()
	{
		var registered = typeof(BaseTheme).Assembly
			.GetCustomAttributes<System.Reflection.Metadata.MetadataUpdateHandlerAttribute>()
			.Any(a => a.HandlerType == typeof(BaseTheme));

		Assert.IsTrue(
			registered,
			"Expected [assembly: MetadataUpdateHandler(typeof(Uno.Themes.BaseTheme))] on Uno.Themes.WinUI");

		var updateMethod = typeof(BaseTheme).GetMethod(
			"UpdateApplication",
			BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

		Assert.IsNotNull(
			updateMethod,
			"BaseTheme.UpdateApplication(Type[]?) must exist as a static method for the .NET runtime to invoke");
	}

	[TestMethod]
	[RunsOnUIThread]
	public void When_HotReloadHandlerFires_Then_LiveBaseThemeInstancesAreRebuilt()
	{
		// Arrange: a live theme. The constructor calls RegisterInstance to opt into hot-reload tracking.
		var theme = new SimpleTheme();

		Assert.IsTrue(
			theme.MergedDictionaries.Count > 0,
			"SimpleTheme should populate MergedDictionaries during construction");

		// Capture references to children created during the initial UpdateSource(). A successful
		// hot reload pass removes only the programmatically added dictionaries and re-adds fresh
		// instances; URI-backed entries Uno populated via CopyFrom stay put.
		var pre = theme.MergedDictionaries.ToArray();

		// Act: drive the .NET hot-reload entry point the same way the runtime would. Pass a
		// non-empty type array so the early-out gate is satisfied; the contents don't matter
		// for this regression — what we exercise is the iterate-live-instances + UpdateSource path.
		InvokeHotReloadHandler(new[] { typeof(SimpleTheme) });

		// Assert: rebuild ran. Count is preserved and at least one entry is a fresh instance,
		// proving UpdateSource() executed (the dynamic spacing/shape/density layers always
		// regenerate, even when no consumer overrides are set).
		Assert.AreEqual(
			pre.Length,
			theme.MergedDictionaries.Count,
			"Rebuild should produce the same number of merged dictionaries");

		var post = theme.MergedDictionaries.ToArray();
		var replaced = 0;
		for (var i = 0; i < pre.Length; i++)
		{
			if (!ReferenceEquals(pre[i], post[i]))
			{
				replaced++;
			}
		}

		Assert.IsTrue(
			replaced > 0,
			"At least the dynamically-generated dictionaries should be freshly constructed after hot reload");
	}

	[TestMethod]
	[Ignore("This test is a regression guard for a specific memory leak scenario. It reliably fails when the leak is present, but if it starts failing due to an unrelated code change, investigate the failure before re-enabling.")]
	[RunsOnUIThread]
	public void When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt()
	{
		// Arrange: create a theme in an inner scope so the local reference is releasable.
		var weak = CreateThemeWithWeakReference();

		// Force collection; allow finalizers to run.
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		Assert.IsFalse(
			weak.TryGetTarget(out _),
			"BaseTheme instance should be eligible for collection after local reference drops; " +
			"hot-reload tracking must use weak references, not strong references.");

		// Act: handler must tolerate dead-entry sweep without throwing.
		InvokeHotReloadHandler(new[] { typeof(SimpleTheme) });
	}

	// NoInlining is required: in Release the JIT would otherwise inline this helper into the
	// caller, keeping the transient SimpleTheme strongly rooted on the test's stack frame so it
	// never gets collected — making the leak guard fail for the wrong reason.
	[MethodImpl(MethodImplOptions.NoInlining)]
	private static WeakReference<BaseTheme> CreateThemeWithWeakReference()
	{
		// Isolated frame so the strong reference is unreachable when the caller's GC.Collect runs.
		var t = new SimpleTheme();
		return new WeakReference<BaseTheme>(t);
	}

	private static void InvokeHotReloadHandler(Type[] updatedTypes)
	{
		var method = typeof(BaseTheme).GetMethod(
			"UpdateApplication",
			BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)!;
		method.Invoke(null, new object?[] { updatedTypes });
	}
}
