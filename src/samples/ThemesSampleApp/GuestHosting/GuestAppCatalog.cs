namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Describes a theme sample app that this wrapper can host in a secondary AssemblyLoadContext.
/// </summary>
/// <param name="DisplayName">Name shown on the app-picker button.</param>
/// <param name="ProjectFolderName">Folder name under <c>src/samples/</c> (also the guest payload folder name).</param>
/// <param name="AssemblyName">Simple name of the guest's entry assembly (without extension).</param>
internal sealed record GuestAppInfo(string DisplayName, string ProjectFolderName, string AssemblyName);

/// <summary>
/// The set of theme sample apps hostable by this wrapper.
/// </summary>
internal static class GuestAppCatalog
{
	/// <summary>
	/// Gets the hostable theme sample apps, in picker order.
	/// </summary>
	/// <remarks>
	/// Adding a head means touching every declaration site. Misses degrade softly (build
	/// warning or a friendly runtime error), but only this list drives the picker:
	/// <list type="number">
	/// <item>this catalog;</item>
	/// <item>the <c>_GuestWasmApp</c> payload list and the desktop ordering
	/// <c>ProjectReference</c>s in <c>ThemesSampleApp.csproj</c>;</item>
	/// <item>the wrapper's <c>ProjectDependencies</c> in <c>Uno.Themes.sln</c> (IDE/solution
	/// build order — the csproj P2P only covers override-driven CLI builds);</item>
	/// <item>the head loop in <c>build/scripts/build-wasm-guest-heads.sh</c> (the script CI
	/// and the static-web-app workflow both call — the yml legs carry no head list);</item>
	/// <item><c>GuestSharedAssemblies.txt</c>, if the head ships a repo-built theme library
	/// not already marked <c>!</c> isolated;</item>
	/// <item><c>build-Themes-wasm</c>'s <c>dependsOn</c> in <c>.vscode/tasks.json</c>.</item>
	/// </list>
	/// The head itself needs only <c>UnoEnableAlcAppSupport</c> and <c>new Window()</c> in
	/// place of <c>Window.Current</c>; see AGENTS.md's <c>src/samples/ThemesSampleApp/</c> entry.
	/// </remarks>
	public static IReadOnlyList<GuestAppInfo> Apps { get; } =
	[
		new GuestAppInfo("Material", "MaterialSampleApp", "MaterialSampleApp"),
		new GuestAppInfo("Cupertino", "CupertinoSampleApp", "CupertinoSampleApp"),
		new GuestAppInfo("Simple", "SimpleSampleApp", "SimpleSampleApp"),
		new GuestAppInfo("Fluent", "FluentSampleApp", "FluentSampleApp"),
	];
}
