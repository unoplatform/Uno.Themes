namespace Uno.Themes.WrapperApp.GuestHosting;

/// <summary>
/// Describes a theme sample app that this wrapper can host in a secondary AssemblyLoadContext.
/// </summary>
/// <param name="DisplayName">Name shown on the app-picker button.</param>
/// <param name="ProjectFolderName">Folder name under <c>src/samples/</c> (also the guest payload folder name).</param>
/// <param name="AssemblyName">Simple name of the guest's entry assembly (without extension).</param>
public sealed record GuestAppInfo(string DisplayName, string ProjectFolderName, string AssemblyName);

/// <summary>
/// The set of theme sample apps hostable by this wrapper.
/// </summary>
public static class GuestAppCatalog
{
	/// <summary>
	/// Gets the hostable theme sample apps, in picker order.
	/// </summary>
	public static IReadOnlyList<GuestAppInfo> Apps { get; } =
	[
		new GuestAppInfo("Material", "MaterialSampleApp", "MaterialSampleApp"),
		new GuestAppInfo("Cupertino", "CupertinoSampleApp", "CupertinoSampleApp"),
		new GuestAppInfo("Simple", "SimpleSampleApp", "SimpleSampleApp"),
	];
}
