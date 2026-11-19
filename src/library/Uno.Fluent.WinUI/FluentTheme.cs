#nullable enable

using System;
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

		// The built-in controls follow the effective primary: a PrimaryColor override, else the seed.
		var seed = Colors?.PrimarySeed ?? DefaultPrimarySeed;
		var mode = Colors?.SeedColorMode ?? SeedColorMode.Fidelity;
		_accent.Update(isLight: true, FluentAccentPalette.Build(isLight: true, seed, mode, ResolvedColorOverride));
		_accent.Update(isLight: false, FluentAccentPalette.Build(isLight: false, seed, mode, ResolvedColorOverride));
		AddThemeDictionary(_accent.Resources);

		if (BuildPlatformTokenOverrides() is { } platformTokens)
		{
			AddThemeDictionary(platformTokens);
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

			if (dispatcher is { HasThreadAccess: false })
			{
				dispatcher.TryEnqueue(theme.RefreshSystemAccent);
			}
			else
			{
				theme.RefreshSystemAccent();
			}
		};
		_uiSettings.ColorValuesChanged += handler;
	}

	private void RefreshSystemAccent()
	{
		try
		{
			FluentColorPalette.TryPopulate(_palette);
			UpdateSource();
		}
		catch (Exception e)
		{
			// Raised from a platform event: an escaping exception would crash the consuming app.
			FluentDiagnostics.LogWarning($"FluentTheme could not refresh the system accent. {e.Message}");
		}
	}
}
