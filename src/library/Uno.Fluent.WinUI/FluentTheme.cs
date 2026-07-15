#nullable enable

using System;
using Uno.Themes;

#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace Uno.Fluent;

/// <summary>
/// Fluent (WinUI-default) theme resources: maps the Uno Themes semantic styles,
/// colors, and typography onto the built-in Fluent styles and design tokens
/// provided by <c>XamlControlsResources</c>.
/// </summary>
/// <remarks>
/// This library is an adapter, not a style library — it ships no control templates
/// and no implicit styles; Fluent is already the implicit default of every WinUI /
/// Uno Platform app. <c>XamlControlsResources</c> must be merged <b>before</b> this
/// dictionary; when its tokens are unreachable the semantic color roles keep the
/// shared defaults and a warning is logged (theme initialization never throws).
/// </remarks>
public class FluentTheme : BaseTheme
{
	// Semantic keys whose Fluent style has no public XCR key on every platform
	// (spec 05 §5.2 Ⓜ set): resolved late-bound on each rebuild pass because a
	// XAML <StaticResource> alias to a missing key fails the whole dictionary at
	// parse time. Resolution order per key: the named XCR key where the platform
	// publishes one (Windows), the type-keyed implicit style, and finally an
	// EMPTY style — an explicit empty style keeps the control's built-in default
	// template and appearance (default-style resolution via DefaultStyleKey is
	// independent of element.Style), so the key always resolves and renders the
	// Fluent default (D8: nearest-match over GAP). Uno publishes neither the
	// named keys nor type-keyed app resources for these controls (its default
	// styles live in an internal registry), so the empty style is the Uno path.
	private static readonly (string SemanticKey, string XcrKey, Type ControlType)[] _lateBoundStyleAliases =
	{
		("ProgressRingStyle", "DefaultProgressRingStyle", typeof(ProgressRing)),
		("ListViewStyle", "DefaultListViewStyle", typeof(ListView)),
		("CommandBarStyle", "DefaultCommandBarStyle", typeof(CommandBar)),
		("CalendarDatePickerStyle", "DefaultCalendarDatePickerStyle", typeof(CalendarDatePicker)),
		("PipsPagerStyle", "DefaultPipsPagerStyle", typeof(PipsPager)),
		("RatingControlStyle", "DefaultRatingControlStyle", typeof(RatingControl)),
		("MenuFlyoutSeparatorStyle", "DefaultMenuFlyoutSeparatorStyle", typeof(MenuFlyoutSeparator)),
		("NavigationViewStyle", "DefaultNavigationViewStyle", typeof(NavigationView)),
		("NavigationViewItemStyle", "DefaultNavigationViewItemStyle", typeof(NavigationViewItem)),
	};

	// Semantic keys whose target style ships IN THIS BUNDLE (bridge and typography
	// styles): also resolved late-bound, from the theme's own Source bundle. A XAML
	// <StaticResource> alias resolves at parse time against the app-level scope
	// only — when the theme is instantiated below app scope it cannot see this
	// bundle's own keys, and silently binds to a foreign app-level theme's key of
	// the same name if one exists (see specs/lessons.md).
	private static readonly (string SemanticKey, string BundleKey)[] _bundleStyleAliases = BuildBundleStyleAliases();

	private static (string SemanticKey, string BundleKey)[] BuildBundleStyleAliases()
	{
		string[] typographySlots =
		{
			"DisplayLarge", "DisplayMedium", "DisplaySmall",
			"HeadlineLarge", "HeadlineMedium", "HeadlineSmall",
			"TitleLarge", "TitleMedium", "TitleSmall",
			"BodyLarge", "BodyMedium", "BodySmall",
			"LabelLarge", "LabelMedium", "LabelSmall", "LabelExtraSmall",
			"CaptionLarge", "CaptionMedium", "CaptionSmall",
		};

		var aliases = new (string, string)[typographySlots.Length + 2];
		aliases[0] = ("TextButtonStyle", "FluentTextButtonStyle");
		aliases[1] = ("IconButtonStyle", "FluentIconButtonStyle");
		for (var i = 0; i < typographySlots.Length; i++)
		{
			aliases[i + 2] = (typographySlots[i], "Fluent" + typographySlots[i]);
		}

		return aliases;
	}

	// Field initializers run before the base constructor, so this dictionary is
	// usable from AddThemeSpecificResources during the base ctor's first
	// UpdateSource pass; _palette (assigned in the ctor body) is not — see below.
	private readonly ResourceDictionary _lateBoundStyles = new();

	// The palette container passed to BaseTheme as its base color override,
	// populated in code from the live XamlControlsResources token values
	// ("mechanism C", spec 05 D6) — per-theme-branch XAML <StaticResource>
	// color aliases resolve against the ambient theme on Uno and cannot express
	// a branch-correct palette.
	private readonly ResourceDictionary _palette;

	// Single-entry cache for the reverse accent mapping (spec 05 §9): seed
	// changes are rare, but UpdateSource runs on every theme-property change —
	// don't re-solve the tonal palette when the seed hasn't moved.
	private (Color Seed, ResourceDictionary Dictionary)? _accentOverride;

	/// <summary>
	/// Initializes the Fluent theme resources with the default palette and typography.
	/// </summary>
	public FluentTheme()
		: this(colorOverride: null, fontOverride: null)
	{
	}

	/// <summary>
	/// Initializes the Fluent theme resources.
	/// </summary>
	/// <param name="colorOverride">(Optional) Overrides for the semantic <see cref="Color"/> resources, layered above the Fluent palette.</param>
	/// <param name="fontOverride">(Optional) Overrides for the typography resources, layered above the Fluent type ramp.</param>
	public FluentTheme(ResourceDictionary? colorOverride = null, ResourceDictionary? fontOverride = null)
		: base(CreateFluentColorOverride(colorOverride, out var palette), fontOverride)
	{
		_palette = palette;
	}

	/// <summary>
	/// Fluent's default colors come from the platform (accent + neutrals), not from
	/// a generated seed palette — seed color generation stays opt-in.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	/// <summary>
	/// When a consumer opts into a seed, high-fidelity mode preserves the source
	/// chroma: Windows accent colors are often corporate colors that must not be
	/// re-saturated by the M3 minimum-chroma floor.
	/// </summary>
	protected override bool UseHighFidelityColors => true;

	/// <inheritdoc />
	protected override string DefaultStylesSource => FluentConstants.ResourcePaths.MergedPages;

	private static ResourceDictionary CreateFluentColorOverride(ResourceDictionary? colorOverride, out ResourceDictionary palette)
	{
		palette = new ResourceDictionary();
		FluentColorPalette.TryPopulate(palette);

		if (colorOverride is { })
		{
			palette.SafeMerge(colorOverride);
		}

		return palette;
	}

	/// <inheritdoc />
	protected override void AddThemeSpecificResources()
	{
		base.AddThemeSpecificResources();

		// _palette is null only during the base ctor's first pass, where
		// CreateFluentColorOverride has just attempted the same build.
		if (_palette is { ThemeDictionaries.Count: 0 } palette)
		{
			// The Fluent tokens were unreachable when this theme was constructed
			// (e.g. XamlControlsResources merged after FluentTheme, against the
			// documented ordering) — retry on every rebuild so the palette heals
			// once the tokens become available.
			FluentColorPalette.TryPopulate(palette);
		}

		EnsureLateBoundStyleAliases();
		AddThemeDictionary(_lateBoundStyles);

		// Reverse accent mapping (spec 05 §9, goal G5): when a seed is active,
		// the built-in Fluent controls follow it too. Added on every rebuild
		// pass so it tracks seed changes and is dropped when the seed clears
		// (restoring the platform accent). No seed → no entry at all.
		var effectiveSeed = Colors?.PrimarySeed ?? DefaultPrimarySeed;
		if (effectiveSeed is { } seed)
		{
			if (_accentOverride is not { } cached || cached.Seed != seed)
			{
				_accentOverride = (seed, FluentAccentPalette.Build(seed));
			}

			AddThemeDictionary(_accentOverride.Value.Dictionary);
		}

		// Lightweight-styling bridge (spec 05 §10, goal G6): semantic
		// lightweight keys with Fluent defaults + override-driven re-pointing
		// of the built-in per-control resources. Rebuilt each pass so it
		// tracks seed and override changes (the override's contents can
		// mutate without a reference change, so no cache here — the bridge is
		// a few dozen brushes and passes only run on theme-property changes).
		AddThemeDictionary(FluentLightweightBridge.Build(effectiveSeed, Colors?.OverrideDictionary));

		// Base typography ships in the Source bundle (BaseDictionaries.xaml); only a
		// consumer-supplied font override is layered dynamically on top to shadow it.
		if (FontOverrideDictionary is { } fontOverride)
		{
			AddThemeDictionary(fontOverride);
		}
	}

	private void EnsureLateBoundStyleAliases()
	{
		foreach (var (semanticKey, xcrKey, controlType) in _lateBoundStyleAliases)
		{
			if (_lateBoundStyles.TryGetValue(semanticKey, out _))
			{
				continue;
			}

			_lateBoundStyles[semanticKey] = ResolveBuiltInStyle(xcrKey, controlType);
		}

		foreach (var (semanticKey, bundleKey) in _bundleStyleAliases)
		{
			if (_lateBoundStyles.TryGetValue(semanticKey, out _))
			{
				continue;
			}

			// The Source bundle (mergedpages.xaml) is loaded before the first
			// UpdateSource pass, so the bundle's own styles are resolvable here.
			// A miss is left unaliased rather than aliased to a wrong style.
			if (TryGetValue(bundleKey, out var bundleValue) && bundleValue is Style bundleStyle)
			{
				_lateBoundStyles[semanticKey] = bundleStyle;
			}
		}
	}

	private static Style ResolveBuiltInStyle(string xcrKey, Type controlType)
	{
		if (Application.Current?.Resources is { } resources)
		{
			if (resources.TryGetValue(xcrKey, out var named) && named is Style namedStyle)
			{
				return namedStyle;
			}

			if (resources.TryGetValue(controlType, out var implicitEntry) && implicitEntry is Style implicitStyle)
			{
				return implicitStyle;
			}
		}

		return new Style(controlType);
	}
}
