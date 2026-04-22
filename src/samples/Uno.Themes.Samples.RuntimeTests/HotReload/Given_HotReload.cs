#if DEBUG // Hot-reload tests only run in Debug — Uno.WinUI.DevServer is excluded from Release builds.
namespace Uno.Themes.Samples.RuntimeTests.HotReload;

/// <summary>
/// Smoke test that proves the hot-reload runtime-test harness is wired end-to-end:
/// DevServer loads the Roslyn workspace, a source edit is delta-compiled, and the
/// metadata update reaches the running sample app. Runs in every sample app that
/// ProjectReferences this shared library.
/// </summary>
[TestClass]
[RunsInSecondaryApp(ignoreIfNotSupported: true)]
public class Given_HotReload
{
	[TestInitialize]
	public void Setup()
	{
		// The themes solution pulls in several design-system libraries; allow a generous
		// workspace load budget on CI hosts where Roslyn first-load can be slow.
		HotReloadHelper.DefaultWorkspaceTimeout = TimeSpan.FromSeconds(180);
		HotReloadHelper.DefaultMetadataUpdateTimeout = TimeSpan.FromSeconds(60);
	}

	[TestMethod]
	public async Task When_UpdateMethodBody_Then_MetadataUpdateReceived(CancellationToken ct)
	{
		Assert.AreEqual("original", HotReloadTarget.GetValue());

		await using var _ = await HotReloadHelper.UpdateSourceFile(
			"src/samples/Uno.Themes.Samples.RuntimeTests/HotReload/HotReloadTarget.cs",
			"""return "original";""",
			"""return "updated";""",
			ct);

		Assert.AreEqual("updated", HotReloadTarget.GetValue());
	}
}
#endif
