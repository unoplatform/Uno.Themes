using Windows.Foundation.Metadata;

namespace Uno.Material;

/// <summary>
/// Material resources including colors, layout values and styles
/// </summary>
/// <remarks>
/// This class is like an alias for the latest version of MaterialResources,
/// which is currently pointing to <see cref="MaterialResourcesV2"/>.
/// </remarks>
[Deprecated("Resource initialization for the Uno.Material theme should now be done using the MaterialTheme class instead.", DeprecationType.Deprecate, 3)]
public sealed class MaterialResources : MaterialResourcesV2
{
}
