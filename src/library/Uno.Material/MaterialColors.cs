using Windows.Foundation.Metadata;

namespace Uno.Material;

/// <summary>
/// Material resources for colors.
/// </summary>
/// <remarks>
/// This class is like an alias for the latest version of MaterialColors,
/// which is currently pointing to <see cref="MaterialColorsV2"/>.
/// </remarks>
[Deprecated("Resource initialization for the Uno.Material theme should now be done using the MaterialTheme class instead.", DeprecationType.Deprecate, 3)]
public sealed partial class MaterialColors : MaterialColorsV2
{
}
