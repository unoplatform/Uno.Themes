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
	/// Adding a head means touching every declaration site: this catalog, the
	/// <c>_GuestWasmApp</c> list and ordering <c>ProjectReference</c>s in
	/// <c>ThemesSampleApp.csproj</c>, the wrapper's <c>ProjectDependencies</c> in
	/// <c>Uno.Themes.sln</c>, and the guest-head build step in
	/// <c>build/stage-build-wasm.yml</c>. Misses degrade softly (build warning or a friendly
	/// runtime error), but only this list drives the picker.
	/// </remarks>
	public static IReadOnlyList<GuestAppInfo> Apps { get; } =
	[
		new GuestAppInfo("Material", "MaterialSampleApp", "MaterialSampleApp"),
		new GuestAppInfo("Cupertino", "CupertinoSampleApp", "CupertinoSampleApp"),
		new GuestAppInfo("Simple", "SimpleSampleApp", "SimpleSampleApp"),
		new GuestAppInfo("Omarchy", "OmarchySampleApp", "OmarchySampleApp"),
	];
}
