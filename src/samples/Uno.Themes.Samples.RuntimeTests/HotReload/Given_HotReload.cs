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
		// The dev-server and the secondary app are spawned in parallel by the
		// runtime-tests engine. On slower CI agents the secondary may attempt its
		// WebSocket connection before the dev-server has finished binding the port;
		// that initial failure is not auto-retried by RemoteControlClient, and the
		// next call times out waiting for the workspace. Lowering the per-call
		// timeout and retrying inside the test (see below) gives several chances
		// to win the race within the same overall budget.
		HotReloadHelper.DefaultWorkspaceTimeout = TimeSpan.FromSeconds(90);
		HotReloadHelper.DefaultMetadataUpdateTimeout = TimeSpan.FromSeconds(60);
	}

	[TestMethod]
	public async Task When_UpdateMethodBody_Then_MetadataUpdateReceived(CancellationToken ct)
	{
		Assert.AreEqual("original", HotReloadTarget.GetValue());

		const int maxAttempts = 3;
		for (var attempt = 1; attempt <= maxAttempts; attempt++)
		{
			try
			{
				await using var _ = await HotReloadHelper.UpdateSourceFile(
					"HotReload/HotReloadTarget.cs",
					"""return "original";""",
					"""return "updated";""",
					ct);

				Assert.AreEqual("updated", HotReloadTarget.GetValue());
				return;
			}
			catch (TimeoutException) when (attempt < maxAttempts)
			{
				// Brief pause then retry — covers the dev-server startup race described
				// above without leaking into the assertion failure path.
				await Task.Delay(TimeSpan.FromSeconds(5), ct);
			}
		}
	}
}
#endif
