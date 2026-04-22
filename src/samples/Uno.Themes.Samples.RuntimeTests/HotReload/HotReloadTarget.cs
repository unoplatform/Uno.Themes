namespace Uno.Themes.Samples.RuntimeTests.HotReload;

// Mutation target for Given_HotReload. The literal inside GetValue() is rewritten
// at test time by HotReloadHelper.UpdateSourceFile to verify the harness delivers
// a metadata update to the running app.
internal static class HotReloadTarget
{
	internal static string GetValue()
	{
		return "original";
	}
}
