---
uid: Uno.Themes.Fluent.GetStarted
---

# Fluent-styled controls

<p align="center">
  <img src="assets/fluent-design-system.png" alt="Fluent design system" />
</p>

Uno Platform 3.0 and above supports control styles conforming to the [Fluent design system](https://www.microsoft.com/design/fluent).
The details below explain how to use them in your app.

## Upgrading existing Uno apps to use Fluent styles

Overall, the Uno Platform uses the same mechanism as WinUI to enable Fluent styles. After installing the `Uno.UI` NuGet version 3.0 or above, Fluent styles are enabled by specifying the `XamlControlsResources` within the application's resources (inside `App.xaml`).

For the UWP head, an additional WinUI 2 NuGet package reference must be added. This is following the same process as UWP because, for the UWP head, the Uno Platform is not used.

The step-by-step process to enable Fluent design styles within an existing Uno Platform solution is as follows:

1. In all platform head projects except UWP update the `Uno.UI` NuGet packages to 3.0 or above.

1. In only the `UWP` head project of your solution, if you have one, install the [WinUI 2 NuGet package](https://www.nuget.org/packages/Microsoft.UI.Xaml). This step is the same as required for WinUI 2 UWP apps.

1. Add the `XamlControlsResources` resource dictionary to your application resources inside `App.xaml`. This step is the same as required for WinUI 2 UWP apps.

    ```xml
    <Application>
        <Application.Resources>
            <!-- Load WinUI resources -->
            <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
        </Application.Resources>
    </Application>
    ```

    Or, if you have other existing application-scope resources, add `XamlControlsResources` at the top (before other resources) as a merged dictionary:

    ```xml
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Load WinUI resources -->
                <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
                <!-- Other merged dictionaries here -->
            </ResourceDictionary.MergedDictionaries>
            <!-- Other app resources here -->
        </ResourceDictionary>
    </Application.Resources>
    ```

## FluentTheme: the Uno Themes semantic layer with a Fluent look

`XamlControlsResources` alone gives you the Fluent control styles under their WinUI names (`AccentButtonStyle`, `BodyTextBlockStyle`, …). The `Uno.Fluent.WinUI` package adds the Uno Themes **semantic abstraction layer**: [semantic style keys](semantic-styles.md) like `FilledButtonStyle`, the semantic color palette (`PrimaryBrush`, `OnSurfaceBrush`, …), the semantic typography slots (`DisplayLarge` … `CaptionSmall`), and the [design tokens](design-tokens.md). These keys let shared XAML select a Fluent appearance. Resource overrides have the compatibility limits described below; resolving the same key does not mean every built-in control consumes it.

`FluentTheme` is an *adapter*, not a style library: it ships **no control templates and no implicit styles**. Fluent is already the implicit default of every WinUI / Uno Platform app; the theme only adds:

- **Semantic style aliases** onto the built-in Fluent styles (`FilledButtonStyle` &rarr; `AccentButtonStyle`, `OutlinedTextBoxStyle` &rarr; `DefaultTextBoxStyle`, …) — see the Fluent column in [Semantic Styles](semantic-styles.md).
- **Fluent color values** for the semantic color roles: the accent roles read the system accent (`SystemAccentColor` and its shades — on Windows, the user's real accent color), and the neutral/surface/error roles carry the corresponding Fluent design-token values, per Light/Dark theme. The palette refreshes when the platform reports an accent change and on theme rebuilds.
- **Fluent typography values** for the 19 semantic type slots, using the [Fluent type ramp](https://learn.microsoft.com/en-us/windows/apps/design/style/typography) sizes/weights and the platform-default font (`ContentControlThemeFontFamily`) — no font package is shipped.
- The shared `BaseTheme` machinery: `Space*`/`Radius*` design tokens, `DefaultSpacing`, `DefaultDensity`, `DefaultCornerRadius`, `DefaultFontFamily`, [seed colors](seed-colors.md) (opt-in), color overrides, and hot reload.

### Installation

Add a reference to the `Uno.Fluent.WinUI` NuGet package to your application project.

A runnable showcase lives in the repository at `src/samples/FluentSampleApp` — the shared Uno Themes sample gallery rendered through `FluentTheme`: Fluent sample tabs for every control the semantic layer covers, plus the Overview, Semantic Styling, Colors, Seed Color and Design Tokens pages.

### App.xaml setup

Merge `FluentTheme` **after** `XamlControlsResources`:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <!-- Load WinUI resources — MUST come first -->
            <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />

            <!-- Load the Uno Themes semantic layer mapped onto Fluent -->
            <FluentTheme xmlns="using:Uno.Fluent" />

            <!-- Other merged dictionaries here -->
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

> [!IMPORTANT]
> The ordering requirement is load-bearing: `FluentTheme`'s semantic aliases and color mappings resolve against the resources `XamlControlsResources` provides. When the Fluent tokens are unreachable (for example, the dictionaries are merged in the wrong order), the semantic colors keep the Uno Themes shared defaults and a warning is logged — theme initialization never throws.

### Behavior notes

- **Interaction states**: the semantic layer encodes interaction states as opacity variants of the rest color (`PrimaryHoverBrush` = `PrimaryColor` at hover opacity), while Fluent uses discrete per-state fill colors inside its templates. Under `FluentTheme`, built-in controls keep Fluent's own state behavior (correct by definition); app XAML that uses semantic *state* brushes gets opacity-derived variants of the Fluent rest colors — visually consistent, but not token-identical to Fluent's hover/pressed colors.
- **Materials**: Mica and Acrylic are not applied by the theme. The semantic surface roles map to Fluent's solid-color fallback tokens (`SolidBackgroundFillColor*`); apps can layer materials themselves where supported.
- **Seed colors**: setting `Colors.PrimarySeed` generates a semantic palette from your brand color ([seed colors](seed-colors.md)). The generation mode is the shared `SeedColorMode` default (`Fidelity`: the seed's chroma is preserved and the light `PrimaryColor` is the seed verbatim); the seed also supplies accent resources for the built-in Fluent controls ([seed → accent cascade](seed-colors.md#fluenttheme-seed--accent-cascade)). Without a seed, the palette uses the platform accent and Fluent neutrals. The theme observes `UISettings.ColorValuesChanged` where the platform supplies that notification; explicit seeds and overrides retain precedence when the platform accent changes.
- **Lightweight styling**: the semantic [lightweight-styling](lightweight-styling.md) keys are bridged to the built-in Fluent control resources for Button, TextBox, CheckBox, RadioButton, ToggleSwitch, and Slider. For CheckBox/RadioButton/Slider most key names are WinUI's own, so overrides work natively at any scope; for divergent names, app-wide overrides go through `Colors.OverrideDictionary` and page-scoped overrides target the Fluent per-control keys directly. See [Lightweight Styling — Fluent theme](lightweight-styling.md#fluent-theme).
- **Design tokens reach the built-in controls**: the Fluent templates read `ControlCornerRadius` / `OverlayCornerRadius` and `ContentControlThemeFontFamily`, so a `DefaultCornerRadius` or `DefaultFontFamily` set on `FluentTheme` re-points those platform tokens and stock Fluent controls follow (buttons, text boxes, flyouts, …), as they do under Material and Simple. `OverlayCornerRadius` is twice the base unit (the `Radius200` token). Left unset, the platform values stand. A handful of templates read the corner radius with `StaticResource` and keep the platform value (CalendarView, RadioButton, Slider, ToggleSwitch, PagerControl, ColorPicker); `DefaultSpacing` / `DefaultDensity` have no effect on Fluent templates (their metrics are baked in).
- **Semantic Primary vs. accent fill**: with the platform accent or a seed, Fluent paints accent controls with a *shade* of the accent (`Dark1` in the light theme, `Light2` in the dark theme), while the semantic `PrimaryBrush` is the accent itself in the light theme. A `FilledButtonStyle` button therefore renders darker than a surface painted with `PrimaryBrush` in the light theme; in the dark theme the two coincide. An explicit `PrimaryColor` supplied through `Colors.OverrideDictionary` instead becomes the accent fill verbatim for its appearance.

### Override compatibility

Use the shared override channels for semantic resources, with these Fluent mappings and scope rules:

- **Color roles and brushes**: `PrimaryColor` or `PrimaryBrush` drives the native accent fill; `OnPrimaryColor` or `OnPrimaryBrush` customizes its foreground. An explicit brush takes precedence over its source color and preserves its opacity. Explicit native accent resources and supported control keys, such as `FilledButtonBackground` and `FilledButtonForeground`, can refine the generated result. Other semantic roles remain available to app content; they are not a general translation of every native Fluent resource.
- **Dictionary structure and channels**: `Colors.OverrideDictionary`, `Colors.OverrideSource`, the constructor's `colorOverride` argument, and the obsolete forwarding properties feed the same bridge. Nested `MergedDictionaries` are supported. Resolution checks a dictionary's own entries, merged children in reverse order, then its selected appearance dictionary. `Light` and `Dark` select their respective appearances; `Default` is used only when that appearance dictionary is absent. A Dark-only accent override leaves Light on the platform accent when no seed is set. A failed reload of a previously valid override source retains the assigned values.
- **Scope and variants**: divergent semantic control keys go through the theme's override channel; page or subtree overrides generally use the corresponding native Fluent keys. The supplied text-button style also consumes `TextButtonForeground*`, `TextButtonBackground*`, and `TextButtonBorderBrush*` directly at element scope, including pointer-over, pressed, and disabled states. `IconButtonForeground` works directly for the icon-button rest state. Text-button state overrides do not change standard buttons, and explicit native state overrides retain precedence. Filled tonal and elevated buttons share the standard Fluent button, so their semantic resource families cannot customize those variants independently. See [Lightweight Styling](lightweight-styling.md#fluent-theme).
- **Typography**: set the theme's `DefaultFontFamily` property to change every semantic font slot and the native control font. With that property unset, a `DefaultFontFamily` resource in `FontOverrideDictionary` or `FontOverrideSource` also cascades to all slots and `ContentControlThemeFontFamily`; explicit slot keys such as `BodyMediumFontFamily` take precedence. See [font override precedence](design-tokens.md#typography-font-swap).
- **Existing controls**: the theme retains live solid brushes for its native accent and lightweight resources. Changing or clearing a seed or replacing a supported override updates controls that already use those brushes, including color and opacity. A non-solid brush replacement and value resources such as font families still follow framework resource-refresh behavior; see [seed colors](seed-colors.md#runtime-seed-color-changes) and [design tokens](design-tokens.md#via-scalar-properties).
