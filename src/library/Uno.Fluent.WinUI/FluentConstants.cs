namespace Uno.Fluent;

internal static class FluentConstants
{
	// WinUI-lineage only (spec 05, N5) — no UWP package name variant.
	public static readonly string PackageName = "Uno.Fluent.WinUI";

	public static class ResourcePaths
	{
		public static readonly string MergedPages = $"ms-appx:///{PackageName}/Generated/mergedpages.xaml";

		// Theme-branch dictionaries loaded standalone via their ms-appx Source —
		// excluded from the merged pages because theme-branch resources inside the
		// merged bundle do not resolve reliably in Release builds (see
		// fluent-common.props and specs/lessons.md).
		public static readonly string ColorPalette = $"ms-appx:///{PackageName}/Styles/Application/ColorPalette.xaml";
		public static readonly string LightweightDefaults = $"ms-appx:///{PackageName}/Styles/Application/LightweightDefaults.xaml";
	}
}
