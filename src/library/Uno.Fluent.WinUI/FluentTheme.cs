#nullable enable

using System;
using System.Threading;
using Uno.Themes;
using Windows.Foundation;
using Windows.UI.ViewManagement;

#if WinUI
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Windows.UI;
#else
using Windows.System;
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
/// This library is an adapter, not a style library: it ships no control templates
/// and no implicit styles, since Fluent is already the implicit default of every WinUI /
/// Uno Platform app. <c>XamlControlsResources</c> must be merged <b>before</b> this
/// dictionary; when its tokens are unreachable the semantic color roles keep the
/// shared defaults and a warning is logged (theme initialization never throws).
/// </remarks>
public partial class FluentTheme : BaseTheme
{
	// Field initializers run before the base constructor, whose first rebuild pass already uses them.
	private readonly FluentAccentLayer _accent = new();
	private readonly ResourceDictionary _palette;

	// Kept alive with the theme; the handler holds the theme weakly (see ObserveSystemAccent).
	private UISettings? _uiSettings;
	// 1 while a refresh is queued; the event can fire on a background thread (WinUI).
	private int _refreshQueued;

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
	/// <param name="colorOverride">
	/// (Optional) Overrides for the semantic <see cref="Color"/> resources, layered above the Fluent palette.
	/// Stored as <see cref="BaseTheme.Colors"/>' <see cref="ThemeColors.OverrideDictionary"/> so it also drives
	/// the built-in accent; assigning a new <see cref="BaseTheme.Colors"/> replaces it.
	/// </param>
	/// <param name="fontOverride">(Optional) Overrides for the typography resources, layered above the Fluent type ramp.</param>
	public FluentTheme(ResourceDictionary? colorOverride = null, ResourceDictionary? fontOverride = null)
		: base(CreatePalette(out var palette), fontOverride)
	{
		_palette = palette;

		// As the consumer override (not merged into the base palette) so it also drives the accent.
		if (colorOverride is { })
		{
			Colors = new ThemeColors { OverrideDictionary = colorOverride };
		}

		ObserveSystemAccent();
	}

	/// <summary>
	/// Fluent's default colors are the platform's (accent and neutrals), so seed generation is opt-in.
	/// A seed recolors the built-in Fluent controls too, generated with <see cref="BaseTheme.Colors"/>'
	/// <see cref="SeedColorMode"/> like the semantic palette.
	/// </summary>
	protected override Color? DefaultPrimarySeed => null;

	/// <inheritdoc />
	protected override string DefaultStylesSource => FluentConstants.ResourcePaths.MergedPages;

	/// <inheritdoc />
	protected override string SemanticStylesSource => FluentConstants.ResourcePaths.SemanticStyles;

	private static ResourceDictionary CreatePalette(out ResourceDictionary palette)
	{
		palette = new ResourceDictionary();
		FluentColorPalette.TryPopulate(palette);
		return palette;
	}

	/// <inheritdoc />
	protected override void AddThemeSpecificResources()
	{
		base.AddThemeSpecificResources();

		// The layer is merged even if updating it fails, so it keeps its last good values.
		AddThemeDictionary(_accent.Resources);
		try
		{
			// The built-in controls follow the effective primary: a PrimaryColor override, else the seed
			// (in the same mode as the semantic palette).
			var seedShades = EffectivePrimarySeed is { } seed ? FluentAccentPalette.AccentShades.FromSeed(seed.Seed, seed.Mode) : (FluentAccentPalette.AccentShades?)null;
			_accent.Update(isLight: true, FluentAccentPalette.Build(isLight: true, seedShades, ResolvedColorOverride));
			_accent.Update(isLight: false, FluentAccentPalette.Build(isLight: false, seedShades, ResolvedColorOverride));

			if (BuildPlatformTokenOverrides() is { } platformTokens)
			{
				AddThemeDictionary(platformTokens);
			}
		}
		catch (Exception e)
		{
			// Runs from property-changed callbacks and the constructor; it reads consumer dictionaries,
			// which WinUI can fail to enumerate. Degrade rather than throw into the app.
			FluentDiagnostics.LogWarning("FluentTheme could not update the Fluent accent resources.", e);
		}
	}

	/// <summary>
	/// The platform tokens the built-in templates read (<c>ControlCornerRadius</c>,
	/// <c>OverlayCornerRadius</c>, <c>ContentControlThemeFontFamily</c>), re-pointed at
	/// <see cref="BaseTheme.DefaultCornerRadius"/> / <see cref="BaseTheme.DefaultFontFamily"/> when the
	/// consumer set them; <c>null</c> otherwise, leaving the platform values.
	/// </summary>
	private ResourceDictionary? BuildPlatformTokenOverrides()
	{
		ResourceDictionary? tokens = null;

		// Only a local value is a consumer choice; the DP default means "the platform's". Overlays use
		// twice the unit (Radius200), matching the platform's 4 / 8.
		if (ReadLocalValue(DefaultCornerRadiusProperty) is double radius && double.IsFinite(radius) && radius >= 0)
		{
			tokens ??= new ResourceDictionary();
			tokens["ControlCornerRadius"] = new CornerRadius(radius);
			tokens["OverlayCornerRadius"] = new CornerRadius(radius * 2);
		}

		if (DefaultFontFamily is { } family)
		{
			tokens ??= new ResourceDictionary();
			tokens["ContentControlThemeFontFamily"] = family;
		}

		return tokens;
	}

	private void ObserveSystemAccent()
	{
		// Windows can keep a subscribed UISettings alive past the theme, so the handler only holds the
		// theme weakly and unsubscribes once it is gone.
		_uiSettings = new UISettings();
		var weakTheme = new WeakReference<FluentTheme>(this);
		var dispatcher = DispatcherQueue.GetForCurrentThread();
		TypedEventHandler<UISettings, object>? handler = null;
		handler = (sender, args) =>
		{
			if (!weakTheme.TryGetTarget(out var theme))
			{
				sender.ColorValuesChanged -= handler;
				return;
			}

			if (dispatcher is null)
			{
				theme.RefreshSystemAccent();
			}
			// The event fires several times per system change: coalesce into one rebuild. The flag is
			// claimed before enqueuing so the queued refresh can never clear it before it is set.
			else if (Interlocked.Exchange(ref theme._refreshQueued, 1) == 0
				&& !dispatcher.TryEnqueue(theme.RefreshSystemAccent))
			{
				Volatile.Write(ref theme._refreshQueued, 0);
			}
		};
		_uiSettings.ColorValuesChanged += handler;
	}

	private void RefreshSystemAccent()
	{
		Volatile.Write(ref _refreshQueued, 0);
		try
		{
			FluentColorPalette.TryPopulate(_palette);
			UpdateSource();
		}
		catch (Exception e)
		{
			// Raised from a platform event: an escaping exception would crash the consuming app.
			FluentDiagnostics.LogWarning("FluentTheme could not refresh the system accent.", e);
		}
	}
}
