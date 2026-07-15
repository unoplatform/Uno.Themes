namespace Uno.Fluent;

internal static class FluentConstants
{
	// WinUI-lineage only (spec 05, N5) — no UWP package name variant.
	public static readonly string PackageName = "Uno.Fluent.WinUI";

	public static class ResourcePaths
	{
		public static readonly string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";
	}
}
