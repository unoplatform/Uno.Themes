#nullable enable

using System;
using Uno.Themes;

#if WinUI
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.UI;
using Windows.UI.Xaml;
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
	// The spec 05 §5.2 Ⓜ gap keys (ProgressRingStyle, NavigationViewStyle, …)
	// ship as declarative empty styles in _Resources.xaml — an empty style keeps
	// the control's built-in default template and appearance, so the key always
	// resolves and renders the Fluent default (D8: nearest-match over GAP).

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

	// Field initializers run before the base constructor, so these dictionaries
	// are usable from AddThemeSpecificResources during the base ctor's first
	// UpdateSource pass; _palette (assigned in the ctor body) is not — see below.
	private readonly ResourceDictionary _bundleAliasStyles = new();

	// Declarative per-branch defaults for the semantic lightweight-styling keys
	// (spec 05 §10 step 1), loaded once per theme instance and re-attached on
	// every rebuild pass. Only the accent-derived defaults and the
	// override-driven re-pointing remain code-built (FluentLightweightBridge).
	private readonly ResourceDictionary _lightweightDefaults = LoadLightweightDefaults();

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

	private static ResourceDictionary LoadLightweightDefaults()
	{
		try
		{
			return new ResourceDictionary { Source = new Uri(FluentConstants.ResourcePaths.LightweightDefaults) };
		}
		catch (Exception e)
		{
			// The packaged dictionary failed to load — the semantic lightweight
			// keys then carry no neutral defaults (overrides still re-point);
			// never throw from theme initialization.
			FluentDiagnostics.LogWarning(
				$"FluentTheme could not load its lightweight-styling defaults (LightweightDefaults.xaml). {e.Message}");
			return new ResourceDictionary();
		}
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

		EnsureBundleStyleAliases();
		AddThemeDictionary(_bundleAliasStyles);

		var effectiveSeed = Colors?.PrimarySeed ?? DefaultPrimarySeed;

		// The consumer color override, whatever channel supplied it —
		// Colors.OverrideDictionary / Colors.OverrideSource and the obsolete
		// BaseTheme ColorOverride* properties all funnel into
		// Colors.OverrideDictionary. URI-backed overrides are re-resolved from
		// their Source on each rebuild (mirroring BaseTheme.UpdateSource) so
		// hot-reload edits propagate to the accent/bridge layers too.
		var consumerOverride = Colors?.OverrideDictionary is { } overrideDictionary
			? overrideDictionary.Source is { } overrideSource
				? new ResourceDictionary { Source = overrideSource }
				: overrideDictionary
			: null;

		// An explicit PrimaryColor override is the highest-precedence statement
		// of what "Primary" is — per branch, it drives the accent verbatim,
		// above the seed (parity with Material/Simple, where a PrimaryColor
		// override visibly recolors the theme's controls).
		var (lightAccentBasis, darkAccentBasis) = FluentAccentPalette.ResolveAccentBasis(consumerOverride);

		// Reverse accent mapping (spec 05 §9, goal G5): when an effective
		// primary is active — seed or PrimaryColor override — the built-in
		// Fluent controls follow it too. Added on every rebuild pass so it
		// tracks changes and is dropped when the drivers clear (restoring the
		// platform accent). No driver → no entry at all.
		if (effectiveSeed is { } || lightAccentBasis is { } || darkAccentBasis is { })
		{
			// Only the pure-seed result is cached: override contents can mutate
			// without a reference change, so override-driven passes rebuild.
			if (consumerOverride is null && effectiveSeed is { } seed)
			{
				if (_accentOverride is not { } cached || cached.Seed != seed)
				{
					_accentOverride = (seed, FluentAccentPalette.Build(seed, lightBasis: null, darkBasis: null, consumerOverride: null));
				}

				AddThemeDictionary(_accentOverride.Value.Dictionary);
			}
			else
			{
				AddThemeDictionary(FluentAccentPalette.Build(effectiveSeed, lightAccentBasis, darkAccentBasis, consumerOverride));
			}
		}

		// Lightweight-styling bridge (spec 05 §10, goal G6). The static neutral
		// key defaults ship declaratively (LightweightDefaults.xaml, loaded once
		// per instance); the code layer above them carries only the
		// accent-derived defaults and the override-driven re-pointing of the
		// built-in per-control resources. The code layer is rebuilt each pass so
		// it tracks seed and override changes (the override's contents can
		// mutate without a reference change, so no cache here — it is a handful
		// of brushes and passes only run on theme-property changes).
		AddThemeDictionary(_lightweightDefaults);
		AddThemeDictionary(FluentLightweightBridge.Build(effectiveSeed, lightAccentBasis, darkAccentBasis, consumerOverride));

		// Base typography ships in the Source bundle (BaseDictionaries.xaml); only a
		// consumer-supplied font override is layered dynamically on top to shadow it.
		if (FontOverrideDictionary is { } fontOverride)
		{
			AddThemeDictionary(fontOverride);
		}
	}

	private void EnsureBundleStyleAliases()
	{
		foreach (var (semanticKey, bundleKey) in _bundleStyleAliases)
		{
			if (_bundleAliasStyles.TryGetValue(semanticKey, out _))
			{
				continue;
			}

			// The Source bundle (mergedpages.xaml) is loaded before the first
			// UpdateSource pass, so the bundle's own styles are resolvable here.
			// A miss is left unaliased rather than aliased to a wrong style.
			if (TryGetValue(bundleKey, out var bundleValue) && bundleValue is Style bundleStyle)
			{
				_bundleAliasStyles[semanticKey] = bundleStyle;
			}
		}
	}
}
