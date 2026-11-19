namespace Uno.Themes;

/// <summary>
/// A theme-specific radius generated as <c>min(DefaultCornerRadius × Multiplier, Cap)</c>,
/// see <see cref="BaseTheme.CappedRadii"/>.
/// </summary>
/// <param name="Key">The resource key of the radius; the <c>CornerRadius</c> companions append to it.</param>
/// <param name="Multiplier">The shape scale step the radius follows (2 for Radius200).</param>
/// <param name="Cap">The largest radius the control keeps its shape at, in pixels.</param>
internal readonly record struct CappedRadius(string Key, double Multiplier, double Cap);
