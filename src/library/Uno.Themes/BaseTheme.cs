using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.Themes.ColorGeneration;
using Uno.Themes.Helpers;


#if WinUI
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif


namespace Uno.Themes;

/// <summary>
/// Controls the spacing density applied to all controls.
/// A density is a <em>mode</em>: it scales the base spacing unit
/// (<see cref="BaseTheme.DefaultSpacing"/>) without defining it.
/// </summary>
public enum Density
{
	/// <summary>Compact — spacing scaled to 0.75× the base unit; tighter padding for data-dense UIs.</summary>
	Compact,

	/// <summary>Regular (default) — spacing at 1× the base unit; balanced spacing.</summary>
	Regular,

	/// <summary>Comfortable — spacing scaled to 1.25× the base unit; more generous padding.</summary>
	Comfy,
}

public abstract partial class BaseTheme : ResourceDictionary
{
	private bool _isUpdatingColorOverrides;
	private bool _isFontOverrideMuted;
	private ResourceDictionary _baseColorOverride;

	// SharedColors.xaml, created on the first UpdateSource and kept for the theme's lifetime so
	// consumers keep resolving to the same SolidColorBrush instances across rebuilds; only their
	// Color is rewritten. _previousColorsLayer is the parent it is currently nested under.
	private ResourceDictionary _semanticBrushes;
	private ResourceDictionary _previousColorsLayer;

	// Tracks the dictionaries this theme appends to its own MergedDictionaries during
	// UpdateSource() so a subsequent rebuild (theme-property change or hot reload) removes
	// only those, leaving any other entries in place rather than wiping MergedDictionaries.
	private readonly List<ResourceDictionary> _dynamicDictionaries = new();
	#region FontOverrideSource (DP)
	/// <summary>
	/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
	/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
	/// </summary>
	public string FontOverrideSource
	{
		get => (string)GetValue(FontOverrideSourceProperty);
		set => SetValue(FontOverrideSourceProperty, value);
	}

	public static DependencyProperty FontOverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(FontOverrideSource),
			typeof(string),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnFontOverrideSourceChanged));

	private static void OnFontOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not BaseTheme theme)
		{
			return;
		}

		// Guarded for the same reason as ThemeColors.OnOverrideSourceChanged: a property-changed
		// callback must not throw on a malformed or unresolvable consumer-supplied URI.
		if (e.NewValue is string sourceUri
			&& !string.IsNullOrWhiteSpace(sourceUri)
			&& Uri.TryCreate(sourceUri, UriKind.Absolute, out var source))
		{
			try
			{
				theme.FontOverrideDictionary = new ResourceDictionary() { Source = source };
			}
			catch (Exception ex)
			{
				if (theme.Log().IsEnabled(LogLevel.Warning))
				{
					theme.Log().LogWarning(ex, "FontOverrideSource '{Source}' could not be loaded; the font override is cleared.", sourceUri);
				}
				theme.FontOverrideDictionary = null;
			}
		}
		else
		{
			theme.FontOverrideDictionary = null;
		}
	}
	#endregion

	#region ColorOverrideSource (DP)
	/// <summary>
	/// (Optional) Gets or sets a Uniform Resource Identifier (<see cref="Uri"/>) that provides the source location
	/// of a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
	[Obsolete("Use Colors.OverrideSource on ThemeColors instead. This property will be removed in a future version.")]
	public string ColorOverrideSource
	{
		get => (string)GetValue(ColorOverrideSourceProperty);
		set => SetValue(ColorOverrideSourceProperty, value);
	}

	public static DependencyProperty ColorOverrideSourceProperty { get; } =
		DependencyProperty.Register(
			nameof(ColorOverrideSource),
			typeof(string),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorOverrideSourceChanged));

	private static void OnColorOverrideSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			try
			{
				theme._isUpdatingColorOverrides = true;
				if (e.NewValue is string sourceUri)
				{
					var tc = theme.EnsureColors();
					tc.OverrideSource = sourceUri;
				}
				else if (theme.Colors is { } tc)
				{
					tc.OverrideDictionary = null;
				}

				theme.UpdateSource();
			}
			finally
			{
				theme._isUpdatingColorOverrides = false;
			}
		}
	}
	#endregion

	#region FontOverrideDictionary (DP)
	/// <summary>
	/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="FontFamily"/> resources
	/// </summary>
	public ResourceDictionary FontOverrideDictionary
	{
		get => (ResourceDictionary)GetValue(FontOverrideDictionaryProperty);
		set => SetValue(FontOverrideDictionaryProperty, value);
	}

	public static DependencyProperty FontOverrideDictionaryProperty { get; } =
		DependencyProperty.Register(
			nameof(FontOverrideDictionary),
			typeof(ResourceDictionary),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnFontOverrideChanged));

	private static void OnFontOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme { _isFontOverrideMuted: false } theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region ColorOverrideDictionary (DP)
	/// <summary>
	/// (Optional) Gets or sets a <see cref="ResourceDictionary"/> containing overrides for the default Uno.Material <see cref="Color"/> resources
	/// </summary>
	/// <remarks>The overrides set here should be re-defining the <see cref="Color"/> resources used by Uno.Material, not the <see cref="SolidColorBrush"/> resources</remarks>
	[Obsolete("Use Colors.OverrideDictionary on ThemeColors instead. This property will be removed in a future version.")]
	public ResourceDictionary ColorOverrideDictionary
	{
		get => (ResourceDictionary)GetValue(ColorOverrideDictionaryProperty);
		set => SetValue(ColorOverrideDictionaryProperty, value);
	}

	public static DependencyProperty ColorOverrideDictionaryProperty { get; } =
		DependencyProperty.Register(
			nameof(ColorOverrideDictionary),
			typeof(ResourceDictionary),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorOverrideChanged));

	private static void OnColorOverrideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme { _isUpdatingColorOverrides: false } theme)
		{
			try
			{
				theme._isUpdatingColorOverrides = true;
				if (e.NewValue is ResourceDictionary dict)
				{
					var tc = theme.EnsureColors();
					tc.OverrideDictionary = dict;
				}
				else
				{
					if (theme.Colors is { } tc)
					{
						tc.OverrideDictionary = null;
					}
				}

				theme.UpdateSource();
			}
			finally
			{
				theme._isUpdatingColorOverrides = false;
			}
		}
	}
	#endregion

	#region Colors (DP)
	/// <summary>
	/// Gets or sets a <see cref="ThemeColors"/> object that groups all color-related configuration
	/// including seed colors, overrides, and the palette generation algorithm.
	/// This is the recommended way to configure theme colors.
	/// </summary>
	public ThemeColors Colors
	{
		get => (ThemeColors)GetValue(ColorsProperty);
		set => SetValue(ColorsProperty, value);
	}

	public static DependencyProperty ColorsProperty { get; } =
		DependencyProperty.Register(
			nameof(Colors),
			typeof(ThemeColors),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnColorsChanged));

	private static void OnColorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			if (e.OldValue is ThemeColors old)
			{
				old.SetChangedCallback(null);
			}

			if (e.NewValue is ThemeColors tc)
			{
				tc.SetChangedCallback((_) =>
				{
					if (theme._isUpdatingColorOverrides)
					{
						return;
					}

					theme.UpdateSource();
				});
			}

			if (!theme._isUpdatingColorOverrides)
			{
				theme.UpdateSource();
			}
		}
	}
	#endregion

	#region DefaultCornerRadius (DP)
	/// <summary>
	/// Gets or sets the base corner radius unit (in pixels). Default is 4.
	/// All shape scale tokens (Radius100, Radius200, …) are computed
	/// as multiples of this value — e.g. <c>DefaultCornerRadius="6"</c> makes
	/// Radius100=6, Radius200=12, Radius400=24, etc.
	/// <c>RadiusFull</c> always remains 9999 (pill shape).
	/// Individual tokens can still be overridden via lightweight styling.
	/// </summary>
	/// <remarks>
	/// This is a <b>construction-time</b> setting: assign it where the theme is declared
	/// (normally <c>App.xaml</c>). Assigning it later regenerates the <c>Radius*</c> token
	/// resources but does not restyle controls — not the ones already rendered, and not ones
	/// created afterwards. The per-control keys that consume these tokens (<c>ButtonCornerRadius</c>
	/// and friends) are resolved once when the theme's control-style dictionaries are parsed, and
	/// a <see cref="CornerRadius"/> is a value with no live instance to update. To offer shape as
	/// a user setting, change the property and then recreate the root content.
	/// </remarks>
	public double DefaultCornerRadius
	{
		get => (double)GetValue(DefaultCornerRadiusProperty);
		set => SetValue(DefaultCornerRadiusProperty, value);
	}

	public static DependencyProperty DefaultCornerRadiusProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultCornerRadius),
			typeof(double),
			typeof(BaseTheme),
			new PropertyMetadata(4.0, OnDefaultCornerRadiusChanged));

	private static void OnDefaultCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region DefaultSpacing (DP)
	/// <summary>
	/// The default base spacing unit (px) when <see cref="DefaultSpacing"/> is not set
	/// or holds an invalid (non-finite or negative) value.
	/// </summary>
	private const double DefaultBaseSpacing = 4.0;

	/// <summary>
	/// Gets or sets the base spacing unit (in pixels). Default is 4.
	/// All spacing scale tokens (Space0, Space050, Space100, …) and their
	/// <see cref="Thickness"/> companions are computed as multiples of this value,
	/// scaled by the <see cref="DefaultDensity"/> mode —
	/// e.g. <c>DefaultSpacing="6"</c> at <see cref="Density.Regular"/> makes
	/// Space100=6, Space200=12, Space400=24; at <see cref="Density.Compact"/> Space100=4.5.
	/// Individual tokens can still be overridden via lightweight styling.
	/// </summary>
	/// <remarks>
	/// This is a <b>construction-time</b> setting, for the same reason as
	/// <see cref="DefaultCornerRadius"/>: assigning it later regenerates the <c>Space*</c> token
	/// resources but does not restyle controls, because the per-control padding and margin keys
	/// hold resolved <see cref="Thickness"/> values. To offer spacing as a user setting, change the
	/// property and then recreate the root content.
	/// Non-finite or negative values are treated as unset and fall back to the default of 4.
	/// </remarks>
	public double DefaultSpacing
	{
		get => (double)GetValue(DefaultSpacingProperty);
		set => SetValue(DefaultSpacingProperty, value);
	}

	public static DependencyProperty DefaultSpacingProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultSpacing),
			typeof(double),
			typeof(BaseTheme),
			new PropertyMetadata(DefaultBaseSpacing, OnDefaultSpacingChanged));

	private static void OnDefaultSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region DefaultDensity (DP)
	/// <summary>
	/// Gets or sets the density mode for the theme. Default is <see cref="Density.Regular"/>.
	/// The mode scales the <see cref="DefaultSpacing"/> base unit used by all Space* tokens
	/// (Compact = ×0.75, Regular = ×1, Comfy = ×1.25), so density and spacing compose:
	/// the effective base unit is <c>DefaultSpacing × density factor</c>.
	/// Control heights and icon sizes remain constant across densities.
	/// </summary>
	/// <remarks>
	/// This is a <b>construction-time</b> setting, for the same reason as
	/// <see cref="DefaultCornerRadius"/>: assigning it later regenerates the <c>Space*</c> token
	/// resources but does not restyle controls, because the per-control padding and margin keys
	/// hold resolved <see cref="Thickness"/> values. To offer density as a user setting, change the
	/// property and then recreate the root content.
	/// </remarks>
	public Density DefaultDensity
	{
		get => (Density)GetValue(DefaultDensityProperty);
		set => SetValue(DefaultDensityProperty, value);
	}

	public static DependencyProperty DefaultDensityProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultDensity),
			typeof(Density),
			typeof(BaseTheme),
			new PropertyMetadata(Density.Regular, OnDefaultDensityChanged));

	private static void OnDefaultDensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	#region DefaultFontFamily (DP)
	/// <summary>
	/// (Optional) Gets or sets the <see cref="FontFamily"/> the theme's whole type scale is generated
	/// from. Left unset (the default), the concrete theme's own typeface stands (Material: Roboto,
	/// Simple: Inter, Cupertino: SF Pro, otherwise Segoe UI).
	/// </summary>
	/// <remarks>
	/// <para>
	/// Assigning this regenerates the <c>DefaultFontFamily</c> root token and every
	/// <c>*FontFamily</c> key of the type scale derived from it, so one value swaps the font
	/// everywhere the design system styles text. Per-scale variation is expressed through the
	/// <c>*FontWeight</c> tokens, so the assigned family should resolve multiple weights (a variable
	/// font, or a font with a font manifest). Changing only some scales is not expressed here:
	/// declare the individual <c>*FontFamily</c> keys in a <see cref="FontOverrideDictionary"/> for
	/// that.
	/// </para>
	/// <para>
	/// Unlike <see cref="DefaultCornerRadius"/> and <see cref="DefaultSpacing"/>, this is a
	/// <b>runtime</b> setting: text laid out after the change picks the new family up. Text
	/// <b>already rendered</b> keeps the family it resolved when it loaded — a
	/// <see cref="FontFamily"/> is an immutable value, so unlike a seed color, whose brushes are live
	/// instances this theme rewrites in place, there is nothing to mutate. Moving realized text needs
	/// the application to re-resolve its resource bindings (what a hot reload does at the end of a
	/// reload) or the content recreated.
	/// </para>
	/// <para>
	/// This covers text the design system styles. Text with no style resolves the framework's own
	/// default instead (<c>FeatureConfiguration.Font.DefaultTextFontFamily</c>), which this property
	/// does not touch.
	/// </para>
	/// <para>
	/// A consumer <see cref="FontOverrideDictionary"/> declaring any of these keys still wins: it is
	/// merged above the generated tokens, matching how
	/// <see cref="ThemeColors.OverrideDictionary"/> beats the generated seed palette.
	/// </para>
	/// </remarks>
	public FontFamily DefaultFontFamily
	{
		get => (FontFamily)GetValue(DefaultFontFamilyProperty);
		set => SetValue(DefaultFontFamilyProperty, value);
	}

	/// <summary>Identifies the <see cref="DefaultFontFamily"/> dependency property.</summary>
	public static DependencyProperty DefaultFontFamilyProperty { get; } =
		DependencyProperty.Register(
			nameof(DefaultFontFamily),
			typeof(FontFamily),
			typeof(BaseTheme),
			new PropertyMetadata(null, OnDefaultFontFamilyChanged));

	private static void OnDefaultFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BaseTheme theme)
		{
			theme.UpdateSource();
		}
	}
	#endregion

	/// <summary>
	/// Gets the default primary seed color for this theme.
	/// When not <c>null</c>, seed color generation is always active — even
	/// without an explicit <see cref="ThemeColors.PrimarySeed"/> — so the
	/// theme uses algorithmically-derived colors by default.
	/// Override in subclasses to provide a theme-specific default.
	/// </summary>
	protected virtual Color? DefaultPrimarySeed => null;

	/// <summary>
	/// When <c>true</c>, seed color generation preserves the source color's
	/// actual chroma (<see cref="SeedColorMode.Fidelity"/>). When <c>false</c>,
	/// the standard M3 minimum chroma of 48 is enforced
	/// (<see cref="SeedColorMode.TonalSpot"/>), which
	/// guarantees vibrant colors but distorts low-chroma seeds like gray.
	/// </summary>
	[Obsolete("Use ThemeColors.SeedColorMode instead. This property will be removed in a future version.")]
	protected virtual bool UseHighFidelityColors => true;

	public BaseTheme() : this(colorOverride: null, fontOverride: null)
	{

	}

	public BaseTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
	{
		// Opt this instance into hot-reload tracking so BaseTheme.UpdateApplication can
		// rebuild it when a XAML resource edit is applied (see BaseTheme.HotReload.cs).
		RegisterInstance(this);

		if (colorOverride is { })
		{
			_baseColorOverride = colorOverride;
		}

		if (fontOverride is { })
		{
			SetFontOverrideSilently(fontOverride);
		}

		// Set once: populates this dictionary with the concrete theme's static base layer
		// (control styles) from the URI it provides. UpdateSource() appends the dynamic
		// layers (colors/typography/spacing/…) on top and rebuilds them on every pass.
		Source = new Uri(DefaultStylesSource);
		UpdateSource();
	}

	private void SetFontOverrideSilently(ResourceDictionary fontOverride)
	{
		try
		{
			_isFontOverrideMuted = true;
			FontOverrideDictionary = fontOverride;
		}
		finally
		{
			_isFontOverrideMuted = false;
		}
	}

	private ThemeColors EnsureColors()
	{
		var colors = Colors;
		if (colors is null)
		{
			colors = new ThemeColors();
			Colors = colors;
		}
		return colors;
	}

	protected void UpdateSource()
	{
		// Build every dynamic layer BEFORE touching MergedDictionaries. Parsing a consumer-supplied
		// override, generating the seed palette and sweeping the brushes can all throw, and this runs
		// from property-changed callbacks and the hot-reload handler. Committing only once the new
		// layers exist means a failure leaves the theme on its last good palette, instead of stripping
		// every colour, spacing and shape key out of the consuming app permanently.
		var colors = BuildColorLayer(out var resolvedOverride);

		// Spacing and density are orthogonal: DefaultSpacing supplies the base unit, the density
		// mode scales it. Non-finite or negative consumer values degrade to the default base (4)
		// instead of poisoning every Space* token — this runs from property-changed callbacks and
		// must not misbehave on untrusted input.
		var requestedSpacing = DefaultSpacing;
		var baseSpacing = double.IsFinite(requestedSpacing) && requestedSpacing >= 0
			? requestedSpacing
			: DefaultBaseSpacing;
		// Factors chosen so the default base of 4 yields the historical presets: 3 / 4 / 5.
		var densityFactor = DefaultDensity switch
		{
			Density.Compact => 0.75,
			Density.Comfy => 1.25,
			_ => 1.0, // Regular, and graceful fallback for undefined enum values
		};
		var spacing = GenerateSpacingScale(baseSpacing * densityFactor);
		var shape = GenerateShapeScale(DefaultCornerRadius);
		var density = GenerateDensityDefaults();

		// Null when no font family is set, which is the common case: the concrete theme's own
		// Fonts.xaml then stands with no layer over it.
		var typefaces = GenerateFontFamilyScale(DefaultFontFamily);

		// Re-resolved from its Source when a hot reload invalidates it: the in-memory entries of the
		// instance built when the Source was first assigned are a load-time snapshot, so a hot-reload
		// edit to the override file would otherwise never reach the theme (unoplatform/Uno.Themes#1705).
		// Unrelated rebuilds (a seed-color change, a spacing tweak) reuse the resolved copy.
		var fontOverride = ResolveFontOverride();

		// ── Commit. Nothing below parses XAML or runs consumer-supplied code. ──

		// Remove only the dictionaries this theme appended on a previous pass. The URI-backed
		// base layer populated when Source was set (the concrete theme's control styles) stays
		// in place so it is not rebuilt on every theme-property change or hot reload.
		foreach (var dictionary in _dynamicDictionaries)
		{
			MergedDictionaries.Remove(dictionary);
		}
		_dynamicDictionaries.Clear();

		// Detach from the previous pass's layer before re-parenting: a ResourceDictionary may
		// not be nested under two parents at once.
		_previousColorsLayer?.MergedDictionaries.Remove(_semanticBrushes);
		colors.MergedDictionaries.Add(_semanticBrushes);
		_previousColorsLayer = colors;

		// Merged last so a consumer override wins for both *Color and *Brush keys.
		if (resolvedOverride is { })
		{
			colors.SafeMerge(resolvedOverride);
		}

		AddThemeDictionary(spacing);
		AddThemeDictionary(shape);
		AddThemeDictionary(density);
		AddThemeDictionary(colors);

		if (typefaces is { })
		{
			AddThemeDictionary(typefaces);
		}

		// Let the concrete theme append its own dynamic dictionaries on top of the generated
		// layers. The static converter/typography/font/thickness defaults live in the Source
		// bundle (BaseDictionaries.xaml), below the theme's own overrides; only resources that
		// must shadow that base belong here.
		AddThemeSpecificResources();

		// Merged last so a consumer font override wins over both the generated typeface tokens and
		// anything the concrete theme added, matching how the colour override is merged over the
		// generated seed palette.
		if (fontOverride is { })
		{
			AddThemeDictionary(fontOverride);
		}
	}

	// The last successfully resolved copy of a URI-backed font override, the consumer instance it
	// was resolved from, and whether the next rebuild must re-read it from its Source. Re-reading
	// on every rebuild would re-parse the override XAML on unrelated theme changes (a seed-color
	// drag, a spacing tweak) — the re-read only exists for hot reload, so the hot-reload handler
	// and the override property setters are the only invalidators.
	private ResourceDictionary _resolvedFontOverride;
	private ResourceDictionary _resolvedFontOverrideOrigin;
	private bool _fontOverrideNeedsReresolve = true;

	/// <summary>
	/// The consumer font override to merge last, or <c>null</c> when there is none. A URI-backed
	/// override is re-read from its <see cref="ResourceDictionary.Source"/> when a hot-reload pass
	/// invalidates it (or when the override itself is reassigned), so an edit to that file
	/// propagates — see unoplatform/Uno.Themes#1705 — while unrelated rebuilds reuse the resolved
	/// copy. An explicitly assigned dictionary with no <c>Source</c> is merged as assigned.
	/// </summary>
	/// <returns>The dictionary to merge, or <c>null</c>.</returns>
	private ResourceDictionary ResolveFontOverride()
	{
		if (FontOverrideDictionary is not { } fontOverride)
		{
			_resolvedFontOverride = null;
			_resolvedFontOverrideOrigin = null;
			return null;
		}

		if (fontOverride.Source is not { } overrideSource)
		{
			// No file to re-read. On non-Uno targets a dictionary instance may not be nested under
			// two parents at once, so merge a clone there (mirrors SafeMerge); on Uno the instance
			// itself is merged so consumer-side mutations stay visible across rebuilds.
#if !HAS_UNO
			return fontOverride.Duplicate();
#else
			return fontOverride;
#endif
		}

		if (!_fontOverrideNeedsReresolve
			&& ReferenceEquals(_resolvedFontOverrideOrigin, fontOverride)
			&& _resolvedFontOverride is { })
		{
			return _resolvedFontOverride;
		}

		_fontOverrideNeedsReresolve = false;
		_resolvedFontOverrideOrigin = fontOverride;

		try
		{
			_resolvedFontOverride = new ResourceDictionary { Source = overrideSource };
		}
		catch (Exception ex)
		{
			// The source loaded once (this instance exists) but no longer does, e.g. a hot-reload edit
			// broke the XAML. A stale override beats losing the typefaces altogether, and this runs
			// from a property-changed callback where throwing takes the consuming app down.
			if (this.Log().IsEnabled(LogLevel.Warning))
			{
				this.Log().LogWarning(ex, "The font override '{Source}' could not be re-read; keeping the previously resolved copy.", overrideSource);
			}
			_resolvedFontOverride ??=
#if !HAS_UNO
				fontOverride.Duplicate();
#else
				fontOverride;
#endif
		}

		return _resolvedFontOverride;
	}

	/// <summary>
	/// Assembles the colour layer and rewrites the semantic brushes from it, without mutating this
	/// dictionary's <see cref="ResourceDictionary.MergedDictionaries"/>. The caller commits the result.
	/// </summary>
	/// <param name="resolvedOverride">
	/// The consumer override to merge last, or <c>null</c>. Returned separately because it has to be
	/// merged after the brush dictionary so a consumer-defined <c>*Brush</c> key still wins.
	/// </param>
	private ResourceDictionary BuildColorLayer(out ResourceDictionary resolvedOverride)
	{
		// The value dictionaries — shared palette, the theme's own base palette (e.g. SimpleTheme's
		// grayscale, supplied via _baseColorOverride), the seed palette and finally the consumer
		// override — are merged in increasing precedence, and tracked in that same order so the
		// brushes can be resolved against them below. This layer is intentionally NOT baked into
		// the Source bundle (BaseDictionaries.xaml) so colour edits never require reloading the
		// static base.
		var colors = new ResourceDictionary();
		var colorLayers = new List<ResourceDictionary>(4);

		var sharedPalette = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorPaletteResourcePath) };
		colors.MergedDictionaries.Add(sharedPalette);
		colorLayers.Add(sharedPalette);

		// Theme-specific base colors (e.g. SimpleTheme's grayscale palette) are merged
		// before the seed so that seed-generated colors take precedence.
		if (_baseColorOverride is { } baseColorOverride)
		{
			colors.SafeMerge(baseColorOverride);
			colorLayers.Add(baseColorOverride);
		}

		// Resolve seed colors from Colors property, falling back to theme default
		var effectivePrimary = Colors?.PrimarySeed ?? DefaultPrimarySeed;
		var effectiveSecondary = Colors?.SecondarySeed;
		var effectiveTertiary = Colors?.TertiarySeed;

		if (effectivePrimary is { } seed)
		{
			// An explicit Colors.SeedColorMode wins; otherwise fall back to the obsolete
			// virtual so a subclass that overrode it before 8.0 keeps its behavior.
#pragma warning disable CS0618 // Type or member is obsolete
			var seedColorMode = Colors is { HasExplicitSeedColorMode: true } explicitColors
				? explicitColors.SeedColorMode
				: UseHighFidelityColors ? SeedColorMode.Fidelity : SeedColorMode.TonalSpot;
#pragma warning restore CS0618

			var seedPalette = SeedColorPaletteGenerator.Default.Generate(seed, effectiveSecondary, effectiveTertiary, seedColorMode);
			colors.SafeMerge(seedPalette);
			colorLayers.Add(seedPalette);
		}

		// Explicit user overrides from Colors.OverrideDictionary take highest precedence.
		// URI-backed overrides are re-resolved from their Source on each rebuild so that
		// hot-reload edits to the underlying XAML propagate: the in-memory key/value pairs of
		// the original instance were loaded at init time and would otherwise be stale.
		resolvedOverride = null;
		if (Colors?.OverrideDictionary is { } userOverride)
		{
			if (userOverride.Source is { } overrideSource)
			{
				try
				{
					resolvedOverride = new ResourceDictionary { Source = overrideSource };
				}
				catch (Exception ex)
				{
					// The source loaded once (the override instance exists) but no longer does —
					// e.g. a hot-reload edit broke the XAML. A stale override beats losing it, and
					// throwing here would propagate out of a property-changed callback.
					if (this.Log().IsEnabled(LogLevel.Warning))
					{
						this.Log().LogWarning(ex, "The color override '{Source}' could not be re-read; keeping the assigned copy.", overrideSource);
					}
					resolvedOverride = userOverride;
				}
			}
			else
			{
				resolvedOverride = userOverride;
			}

			colorLayers.Add(resolvedOverride);
		}

		// The brush dictionary is created once and reused for the lifetime of the theme; the
		// colours resolved above are written into its existing SolidColorBrush instances.
		// Rebuilding it would neither pick up the seed (its {StaticResource *Color} bindings
		// resolve eagerly at parse time, against the ambient scope) nor reach anything already
		// rendered (consumers bind {ThemeResource *Brush}, which re-evaluates only on a theme
		// change). See SemanticBrushUpdater.
		_semanticBrushes ??= new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorsResourcePath) };
		SemanticBrushUpdater.Apply(_semanticBrushes, colorLayers);

		return colors;
	}

	/// <summary>
	/// Adds <paramref name="dictionary"/> to this theme's <see cref="ResourceDictionary.MergedDictionaries"/>
	/// and tracks it so the next <see cref="UpdateSource"/> removes it. Use this instead of adding to
	/// <c>MergedDictionaries</c> directly so the entry participates in the rebuild lifecycle.
	/// </summary>
	protected void AddThemeDictionary(ResourceDictionary dictionary)
	{
		MergedDictionaries.Add(dictionary);
		_dynamicDictionaries.Add(dictionary);
	}

	/// <summary>
	/// URI of the concrete theme's static base layer (its merged control-style pages).
	/// Set as this dictionary's <see cref="ResourceDictionary.Source"/> once during construction;
	/// the dynamic layers generated by <see cref="UpdateSource"/> are appended on top.
	/// </summary>
	protected abstract string DefaultStylesSource { get; }

	/// <summary>
	/// Appends design-system-specific resource dictionaries on top of the layers generated by
	/// <see cref="UpdateSource"/>. Called on every rebuild pass, after the generated layers and before
	/// the consumer font override. Override in a concrete theme and call
	/// <see cref="AddThemeDictionary(ResourceDictionary)"/> for each dictionary so it participates in
	/// the dynamic rebuild lifecycle. A <see cref="FontOverrideDictionary"/> does not belong here:
	/// <see cref="UpdateSource"/> resolves and merges it, so it is re-read from its
	/// <see cref="ResourceDictionary.Source"/> when a hot reload invalidates it.
	/// Overrides <b>must not throw</b>: this is called from dependency-property change callbacks
	/// (and the hot-reload handler), where an exception propagates into the consuming app and
	/// leaves the merged dictionaries partially rebuilt. Degrade gracefully instead.
	/// </summary>
	protected virtual void AddThemeSpecificResources()
	{
	}
}
