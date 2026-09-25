namespace Uno.Themes.Samples.Helpers;

/// <summary>
/// Resolves the <see cref="BaseTheme"/> merged into <em>this</em> sample head's application
/// resources, for sample pages that configure the theme at runtime.
/// </summary>
/// <remarks>
/// <para>
/// Sample pages must not use <see cref="SemanticThemeHelper"/> (or anything else hanging off
/// <see cref="Application.Current"/>) for this. <c>Application.Current</c> is a plain process-wide
/// static in the shared <c>Uno.UI</c>, and Uno only assigns it for the application whose assembly
/// lives in the <em>default</em> <c>AssemblyLoadContext</c>: a head hosted in a secondary ALC by
/// <c>ThemesSampleApp</c> is registered per-ALC instead, leaving <c>Application.Current</c> pointing
/// at the wrapper — which is deliberately theme-free. The lookup then finds no theme and the
/// throwing members of <see cref="SemanticThemeHelper"/> fail the page (the
/// <c>SeedColorSamplePage</c> load failure that motivated this helper).
/// </para>
/// <para>
/// Each head therefore registers itself in its <c>App</c> constructor. This file compiles into the
/// head assembly, so <see cref="CurrentApplication"/> is per-head and — when hosted — per-ALC, the
/// same mechanism <c>NavigationHelper.MainWindow</c> and <c>SamplePageLayout.ActiveDesign</c>
/// already rely on.
/// </para>
/// </remarks>
internal static class SampleThemeHelper
{
	/// <summary>
	/// Gets or sets the application owning the sample UI. Set by each head's <c>App</c> constructor;
	/// falls back to <see cref="Application.Current"/> so an unregistered head still works standalone.
	/// </summary>
	public static Application? CurrentApplication { get; set; }

	/// <summary>
	/// Gets the <see cref="BaseTheme"/> merged into this head's application resources, or
	/// <c>null</c> when it merges none.
	/// </summary>
	public static BaseTheme? GetTheme() => (CurrentApplication ?? Application.Current).GetTheme();

	/// <summary>
	/// Gets the <see cref="ThemeColors"/> of this head's theme, creating it if the theme carries none.
	/// </summary>
	/// <exception cref="InvalidOperationException">This head merges no <see cref="BaseTheme"/>.</exception>
	public static ThemeColors GetColorsOrThrow()
	{
		var theme = GetTheme() ?? throw new InvalidOperationException(
			$"No BaseTheme found in the resources of {(CurrentApplication ?? Application.Current)?.GetType().FullName ?? "<no application>"}. " +
			$"The head's App constructor must set {nameof(SampleThemeHelper)}.{nameof(CurrentApplication)}, " +
			"and its App.xaml must merge a theme.");

		return theme.Colors ??= new ThemeColors();
	}
}
