---
uid: Uno.Themes.SemanticStyles
---

# Semantic Styles

Uno Themes provides a **semantic style abstraction layer** that lets you write theme-agnostic XAML. Instead of referencing theme-prefixed style keys (e.g. `MaterialFilledButtonStyle` or `SimpleFilledButtonStyle`), you use a single **semantic key** like `FilledButtonStyle` and the active theme resolves it to the correct design-system-specific style at runtime.

The Uno Semantic Design Language includes control styles, typography, color roles and brushes, and spacing, shape, and density tokens. The names describe intent; each design system supplies its own appearance. This page covers **Material v2 (Material 3), Simple, and Fluent**. Material v1 and Cupertino do not implement this entire contract.

Shared names do not yet guarantee identical override behavior. Check the mapping tables and [override compatibility](#override-compatibility) before switching themes; some styles are missing, some variants share a template, and some declared keys are not consumed by their control.

```xml
<!-- Works under Material, Simple, and Fluent themes -->
<Button Style="{StaticResource FilledButtonStyle}" Content="Save" />
```

## How It Works

Each theme's `_Resources.xaml` defines `<StaticResource>` aliases that map semantic keys to theme-specific styles:

- **Material**: `FilledButtonStyle` &rarr; `MaterialFilledButtonStyle`
- **Simple**: `FilledButtonStyle` &rarr; `SimpleFilledButtonStyle`
- **Fluent**: `FilledButtonStyle` &rarr; `AccentButtonStyle` (the built-in WinUI style)

The Fluent theme is an *adapter*: it ships no control templates of its own and instead maps every semantic key onto the built-in Fluent styles provided by `XamlControlsResources` (WinUI on Windows, Uno.UI everywhere else). See [Fluent getting started](fluent-getting-started.md) for setup and behavior notes.

## Control Style Mappings

The following tables show every semantic style key and how it resolves under each theme.

A few Fluent targets are marked *built-in&nbsp;¹*: the corresponding Fluent style has no public resource key on every platform, so the theme declares an empty style of the correct control type. It leaves the control's built-in template in effect; it does not expose the built-in style object or its setters through `BasedOn`.

### Button

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FilledButtonStyle` | `MaterialFilledButtonStyle` | `SimpleFilledButtonStyle` | `AccentButtonStyle` | Default implicit style for Material/Simple; Fluent accent button |
| `ElevatedButtonStyle` | `MaterialElevatedButtonStyle` | **GAP** | `DefaultButtonStyle` | Simple has no elevated/shadow variant; Fluent has no elevation, standard button is the nearest match |
| `FilledTonalButtonStyle` | `MaterialFilledTonalButtonStyle` | `SimpleFilledTonalButtonStyle` | `DefaultButtonStyle` | Simple "Neutral" is closest tonal match; Fluent standard button has a neutral fill |
| `OutlinedButtonStyle` | `MaterialOutlinedButtonStyle` | `SimpleFilledTonalButtonStyle` | `DefaultButtonStyle` | Fluent standard button carries a stroke |
| `TextButtonStyle` | `MaterialTextButtonStyle` | `SimpleTextButtonStyle` | `FluentTextButtonStyle` | Text-only appearance; Fluent "subtle button" (transparent at rest) |
| `IconButtonStyle` | `MaterialIconButtonStyle` | `SimpleIconButtonStyle` | `FluentIconButtonStyle` | Simple has multiple icon button colors; Primary is default |

### Floating Action Button (FAB)

FAB is a Material-specific concept. Under Simple theme, FAB keys resolve to existing icon button styles; under Fluent they resolve to the accent/standard buttons (Fluent buttons size to content, so there is no size differentiation).

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FabStyle` | `MaterialFabStyle` | `SimpleIconButtonStyle` | `AccentButtonStyle` | Primary icon button as FAB equivalent |
| `SmallFabStyle` | `MaterialSmallFabStyle` | `SimpleIconButtonStyle` | `AccentButtonStyle` | Same as FabStyle |
| `LargeFabStyle` | `MaterialLargeFabStyle` | `SimpleIconButtonStyle` | `AccentButtonStyle` | No large variant in Simple/Fluent |
| `SecondaryFabStyle` | `MaterialSecondaryFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SecondarySmallFabStyle` | `MaterialSecondarySmallFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SecondaryLargeFabStyle` | `MaterialSecondaryLargeFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `TertiaryFabStyle` | `MaterialTertiaryFabStyle` | `SimpleIconButtonSubtleStyle` | `DefaultButtonStyle` | |
| `TertiarySmallFabStyle` | `MaterialTertiarySmallFabStyle` | `SimpleIconButtonSubtleStyle` | `DefaultButtonStyle` | |
| `TertiaryLargeFabStyle` | `MaterialTertiaryLargeFabStyle` | `SimpleIconButtonSubtleStyle` | `DefaultButtonStyle` | |
| `SurfaceFabStyle` | `MaterialSurfaceFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SurfaceSmallFabStyle` | `MaterialSurfaceSmallFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |
| `SurfaceLargeFabStyle` | `MaterialSurfaceLargeFabStyle` | `SimpleIconButtonNeutralStyle` | `DefaultButtonStyle` | |

### ToggleButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `TextToggleButtonStyle` | `MaterialTextToggleButtonStyle` | `SimpleTextToggleButtonStyle` | `DefaultToggleButtonStyle` | Text content toggle |
| `IconToggleButtonStyle` | `MaterialIconToggleButtonStyle` | `SimpleIconToggleButtonStyle` | `DefaultToggleButtonStyle` | Compact icon-only toggle |

### TextBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FilledTextBoxStyle` | `MaterialFilledTextBoxStyle` | `SimpleFilledTextBoxStyle` | `DefaultTextBoxStyle` | Background fill, no border; the Fluent TextBox (fill + underline) serves both variants |
| `OutlinedTextBoxStyle` | `MaterialOutlinedTextBoxStyle` | `SimpleOutlinedTextBoxStyle` | `DefaultTextBoxStyle` | Default implicit style for Simple |

### PasswordBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FilledPasswordBoxStyle` | `MaterialFilledPasswordBoxStyle` | `SimpleFilledPasswordBoxStyle` | `DefaultPasswordBoxStyle` | Background fill with border |
| `OutlinedPasswordBoxStyle` | `MaterialOutlinedPasswordBoxStyle` | `SimpleOutlinedPasswordBoxStyle` | `DefaultPasswordBoxStyle` | Default implicit style for Simple |

### HyperlinkButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `HyperlinkButtonStyle` | `MaterialHyperlinkButtonStyle` | `SimpleHyperlinkButtonStyle` | `DefaultHyperlinkButtonStyle` | Primary underlined link |
| `SecondaryHyperlinkButtonStyle` | `MaterialSecondaryHyperlinkButtonStyle` | `SimpleSecondaryHyperlinkButtonStyle` | `DefaultHyperlinkButtonStyle` | Secondary underlined link |

### ComboBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ComboBoxStyle` | `MaterialComboBoxStyle` | `SimpleComboBoxStyle` | `DefaultComboBoxStyle` | Direct match |
| `ComboBoxItemStyle` | `MaterialComboBoxItemStyle` | `SimpleComboBoxItemStyle` | `DefaultComboBoxItemStyle` | Direct match |

### CheckBox

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CheckBoxStyle` | `MaterialCheckBoxStyle` | `SimpleCheckBoxStyle` | `DefaultCheckBoxStyle` | Direct match |

### RadioButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `RadioButtonStyle` | `MaterialRadioButtonStyle` | `SimpleRadioButtonStyle` | `DefaultRadioButtonStyle` | Direct match |

### ToggleSwitch

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ToggleSwitchStyle` | `MaterialToggleSwitchStyle` | `SimpleToggleSwitchStyle` | `DefaultToggleSwitchStyle` | Direct match |

### Slider

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `SliderStyle` | `MaterialSliderStyle` | `SimpleSliderStyle` | `DefaultSliderStyle` | Direct match |

### ProgressBar

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ProgressBarStyle` | `MaterialProgressBarStyle` | `SimpleProgressBarStyle` | `DefaultProgressBarStyle` | Horizontal indicator |

### ProgressRing

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ProgressRingStyle` | `MaterialProgressRingStyle` | `SimpleProgressRingStyle` | built-in&nbsp;¹ | Circular indicator |

### ListView

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ListViewStyle` | `MaterialListViewStyle` | `SimpleListViewStyle` | built-in&nbsp;¹ | Direct match |
| `ListViewItemStyle` | `MaterialListViewItemStyle` | `SimpleListViewItemStyle` | `DefaultListViewItemStyle` | Direct match |

### ContentDialog

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `ContentDialogStyle` | `MaterialContentDialogStyle` | `SimpleContentDialogStyle` | `DefaultContentDialogStyle` | Direct match |

### CommandBar

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CommandBarStyle` | `MaterialCommandBarStyle` | **GAP** | built-in&nbsp;¹ | Simple has no CommandBar style |

### AppBarButton

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `AppBarButtonStyle` | `MaterialAppBarButtonStyle` | `SimpleAppBarButtonStyle` | `DefaultAppBarButtonStyle` | Direct match |

### NavigationView

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `NavigationViewStyle` | `MaterialNavigationViewStyle` | `SimpleNavigationViewStyle` | built-in&nbsp;¹ | |
| `NavigationViewItemStyle` | `MaterialNavigationViewItemStyle` | `SimpleNavigationViewItemStyle` | built-in&nbsp;¹ | |

### CalendarView

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CalendarViewStyle` | `MaterialCalendarViewStyle` | `SimpleCalendarViewStyle` | `DefaultCalendarViewStyle` | Direct match |

### CalendarDatePicker

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `CalendarDatePickerStyle` | `MaterialCalendarDatePickerStyle` | `SimpleCalendarDatePickerStyle` | built-in&nbsp;¹ | Direct match |

### DatePicker

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `DatePickerStyle` | `MaterialDatePickerStyle` | `SimpleDatePickerStyle` | `DefaultDatePickerStyle` | Direct match |
| `DatePickerFlyoutPresenterStyle` | `MaterialDatePickerFlyoutPresenterStyle` | **GAP** | **GAP** | Simple has a concrete presenter style, but does not publish this semantic alias |

### MediaPlayerElement

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `MediaTransportControlsStyle` | `MaterialMediaTransportControlsStyle` | **GAP** | `DefaultMediaTransportControlsStyle` | Simple has no media style |

### PipsPager

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `PipsPagerStyle` | `MaterialPipsPagerStyle` | `SimplePipsPagerStyle` | built-in&nbsp;¹ | Pagination dots |

### RatingControl

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `RatingControlStyle` | `MaterialRatingControlStyle` | `SimpleRatingControlStyle` | built-in&nbsp;¹ | Star rating with brand colors |

### Flyout / MenuFlyout

| Semantic Key | Material | Simple | Fluent | Notes |
|---|---|---|---|---|
| `FlyoutPresenterStyle` | `MaterialFlyoutPresenterStyle` | `SimpleFlyoutPresenterStyle` | `DefaultFlyoutPresenterStyle` | |
| `MenuFlyoutPresenterStyle` | `MaterialMenuFlyoutPresenterStyle` | `SimpleMenuFlyoutPresenterStyle` | `DefaultMenuFlyoutPresenterStyle` | Direct match |
| `MenuFlyoutItemStyle` | `MaterialMenuFlyoutItemStyle` | `SimpleMenuFlyoutItemStyle` | `DefaultMenuFlyoutItemStyle` | Direct match |
| `MenuFlyoutSeparatorStyle` | `MaterialMenuFlyoutSeparatorStyle` | `SimpleMenuFlyoutSeparatorStyle` | built-in&nbsp;¹ | Direct match |
| `MenuFlyoutSubItemStyle` | `MaterialMenuFlyoutSubItemStyle` | `SimpleMenuFlyoutSubItemStyle` | `DefaultMenuFlyoutSubItemStyle` | Direct match |
| `ToggleMenuFlyoutItemStyle` | `MaterialToggleMenuFlyoutItemStyle` | `SimpleToggleMenuFlyoutItemStyle` | `DefaultToggleMenuFlyoutItemStyle` | Direct match |
| `RadioMenuFlyoutItemStyle` | `MaterialRadioMenuFlyoutItemStyle` | `SimpleRadioMenuFlyoutItemStyle` | `DefaultRadioMenuFlyoutItemStyle` | Radio bullet indicator |

¹ *built-in*: an empty style preserves the built-in template and stock Fluent appearance for a control without a portable public default-style key.

## Typography

Material v2, Simple, and Fluent provide the same 19 semantic typography style keys. Material uses the Material type scale, Simple uses its own design-system values, and Fluent uses the Fluent type ramp; sizes and weights intentionally differ.

Set the theme's `DefaultFontFamily` **property** to generate the root token and all 19 family keys consistently across these themes. A resource override of only the `DefaultFontFamily` **key** has narrower behavior: Material and Simple aliases resolve it at application scope, while Fluent aliases target `ContentControlThemeFontFamily` directly. See [Design Tokens - Typography](design-tokens.md#typography) for override precedence and scope.

| Semantic Style Key | Font Resource Keys |
|---|---|
| `DisplayLarge` | `DisplayLargeFontFamily`, `DisplayLargeFontSize`, `DisplayLargeFontWeight`, `DisplayLargeCharacterSpacing` |
| `DisplayMedium` | `DisplayMediumFontFamily`, `DisplayMediumFontSize`, `DisplayMediumFontWeight` |
| `DisplaySmall` | `DisplaySmallFontFamily`, `DisplaySmallFontSize`, `DisplaySmallFontWeight` |
| `HeadlineLarge` | `HeadlineLargeFontFamily`, `HeadlineLargeFontSize`, `HeadlineLargeFontWeight` |
| `HeadlineMedium` | `HeadlineMediumFontFamily`, `HeadlineMediumFontSize`, `HeadlineMediumFontWeight` |
| `HeadlineSmall` | `HeadlineSmallFontFamily`, `HeadlineSmallFontSize`, `HeadlineSmallFontWeight` |
| `TitleLarge` | `TitleLargeFontFamily`, `TitleLargeFontSize`, `TitleLargeFontWeight` |
| `TitleMedium` | `TitleMediumFontFamily`, `TitleMediumFontSize`, `TitleMediumFontWeight` |
| `TitleSmall` | `TitleSmallFontFamily`, `TitleSmallFontSize`, `TitleSmallFontWeight` |
| `BodyLarge` | `BodyLargeFontFamily`, `BodyLargeFontSize`, `BodyLargeFontWeight`, `BodyLargeCharacterSpacing` |
| `BodyMedium` | `BodyMediumFontFamily`, `BodyMediumFontSize`, `BodyMediumFontWeight`, `BodyMediumCharacterSpacing` |
| `BodySmall` | `BodySmallFontFamily`, `BodySmallFontSize`, `BodySmallFontWeight`, `BodySmallCharacterSpacing` |
| `LabelLarge` | `LabelLargeFontFamily`, `LabelLargeFontSize`, `LabelLargeFontWeight`, `LabelLargeCharacterSpacing` |
| `LabelMedium` | `LabelMediumFontFamily`, `LabelMediumFontSize`, `LabelMediumFontWeight`, `LabelMediumCharacterSpacing` |
| `LabelSmall` | `LabelSmallFontFamily`, `LabelSmallFontSize`, `LabelSmallFontWeight`, `LabelSmallCharacterSpacing` |
| `LabelExtraSmall` | `LabelExtraSmallFontFamily`, `LabelExtraSmallFontSize`, `LabelExtraSmallFontWeight`, `LabelExtraSmallCharacterSpacing` |
| `CaptionLarge` | `CaptionLargeFontFamily`, `CaptionLargeFontSize`, `CaptionLargeFontWeight`, `CaptionLargeCharacterSpacing` |
| `CaptionMedium` | `CaptionMediumFontFamily`, `CaptionMediumFontSize`, `CaptionMediumFontWeight`, `CaptionMediumCharacterSpacing` |
| `CaptionSmall` | `CaptionSmallFontFamily`, `CaptionSmallFontSize`, `CaptionSmallFontWeight`, `CaptionSmallCharacterSpacing` |

Each theme's `Typography.xaml` provides the concrete values for these keys. The font resource keys (e.g. `BodyLargeFontSize`) are the same across themes and can be used directly for lightweight styling overrides.

Material uses the Material Design 3 type scale. Simple maps the same names to its own scale (for example, its `DisplayLarge` is 72/Bold). Fluent maps the slots onto the Fluent type ramp (for example, `DisplayLarge` is 68/SemiBold and `BodyMedium` is 14/Regular). Under Fluent, every slot defaults to `ContentControlThemeFontFamily` and zero character spacing; no font package is shipped.

The table lists the character-spacing keys; slots without a listed key use their style's default spacing.

## Semantic Colors and Brushes

All three themes expose the same **33 color keys** from the shared palette. Their values vary by design system and appearance. The full key and brush inventory is in [Colors and Brushes](material-colors.md); its numeric defaults describe Material, while its shared key names also apply to Simple and Fluent.

Simple's seedless palette currently leaves `SecondaryVariantDarkColor` and `SecondaryVariantLightColor` at their shared purple defaults. Override those legacy roles explicitly if using them in an otherwise neutral Simple design.

| Role group | Color keys |
|---|---|
| Primary | `PrimaryColor`, `OnPrimaryColor`, `PrimaryContainerColor`, `OnPrimaryContainerColor`, `PrimaryInverseColor`, `PrimaryVariantDarkColor`, `PrimaryVariantLightColor` |
| Secondary | `SecondaryColor`, `OnSecondaryColor`, `SecondaryContainerColor`, `OnSecondaryContainerColor`, `SecondaryVariantDarkColor`, `SecondaryVariantLightColor` |
| Tertiary | `TertiaryColor`, `OnTertiaryColor`, `TertiaryContainerColor`, `OnTertiaryContainerColor` |
| Error | `ErrorColor`, `OnErrorColor`, `ErrorContainerColor`, `OnErrorContainerColor` |
| Surface | `SurfaceColor`, `OnSurfaceColor`, `SurfaceVariantColor`, `OnSurfaceVariantColor`, `SurfaceInverseColor`, `OnSurfaceInverseColor`, `SurfaceTintColor` |
| Background and boundaries | `BackgroundColor`, `OnBackgroundColor`, `OutlineColor`, `OutlineVariantColor`, `ShadowColor` |

The shared brush dictionary provides **280 brush keys per appearance**: nine variants for each of 31 roles, plus `SurfaceTintBrush`. `ShadowColor` has no shared `ShadowBrush`, and `SurfaceTintBrush` has no state variants. Form a brush key by removing `Color` and adding the state suffix and `Brush`: `PrimaryColor` becomes `PrimaryBrush` or `PrimaryHoverBrush`.

| State suffix | Opacity key | Default |
|---|---|---|
| None | None | 1 |
| `Hover` | `HoverOpacity` | 0.08 |
| `Focused` | `FocusedOpacity` | 0.12 |
| `Pressed` | `PressedOpacity` | 0.12 |
| `Dragged` | `DraggedOpacity` | 0.16 |
| `Selected` | `SelectedOpacity` | 0.08 |
| `Medium` | `MediumOpacity` | 0.64 |
| `Low` | `LowOpacity` | 0.32 |
| `Disabled` | `DisabledOpacity` | 0.12 |

Pair a fill with its corresponding foreground role: `PrimaryBrush` with `OnPrimaryBrush`, `PrimaryContainerBrush` with `OnPrimaryContainerBrush`, and likewise for Secondary, Tertiary, Error, Background, and inverse surfaces. On ordinary surfaces, use `OnSurfaceBrush` for primary content and `OnSurfaceVariantBrush` for secondary content. An explicit palette override does not automatically choose a contrasting foreground; supply both roles when changing a pair.

Use `{ThemeResource}` for brushes and value tokens that should follow appearance changes, and `{StaticResource}` for styles:

```xml
<Border Background="{ThemeResource PrimaryContainerBrush}">
    <TextBlock Style="{StaticResource BodyMedium}"
               Foreground="{ThemeResource OnPrimaryContainerBrush}"
               Text="Saved" />
</Border>
```

### Choosing an Override Layer

| Change | Override channel | Effect |
|---|---|---|
| Generate a palette | `ThemeColors.PrimarySeed`, optionally `SecondarySeed`, `TertiarySeed`, and `SeedColorMode` | Generates Light and Dark semantic colors; Error roles retain their base values |
| Change a color role or state opacity | `ThemeColors.OverrideDictionary` or `OverrideSource` | Rebuilds the theme's shared brushes from the supplied `*Color` and `*Opacity` values |
| Replace one brush | A `*Brush` resource override | Replaces that resource; other variants of the role are independent |
| Change one control variant | A per-control lightweight resource, such as `FilledButtonForeground` | Affects templates that actually consume that key; see compatibility below |
| Change typeface, spacing, or shape | `BaseTheme` properties or individual token overrides | See [Design Tokens](design-tokens.md) for propagation, scope, and runtime refresh |

For palette changes, use the theme's `Colors` override channel rather than declaring only a `*Color` in `Page.Resources`: the brush updater reads the theme's configured color layers, not arbitrary ancestor resources. A page-local color key alone does not recolor already-created shared brushes. Scoped brush or per-control resource overrides target the rendered resource directly.

For predictable palette and Fluent bridge behavior, place override entries directly in the override dictionary or its immediate `ThemeDictionaries`. Nested merged override dictionaries are not consistently traversed by the current color/bridge implementation. Supply both `Light` and `Default` values for a complete appearance pair; `Default` is a fallback for any appearance without its own branch, not an exclusively dark key. `Dark` can specify a dark-only override. Shared semantic brushes also have a `HighContrast` branch, but the palette generator does not generate a dedicated high-contrast scheme: without explicit high-contrast colors, their updater falls back to the dark/default palette.

Replacing an override dictionary triggers a rebuild; editing entries in an existing dictionary does not itself notify `ThemeColors`. Shared semantic brushes are mutated in place on a rebuild. Replacing a brush resource or regenerating value tokens does not provide that same live-instance guarantee; existing controls may require a theme-change pass or recreation. See [Seed Color Palette](seed-colors.md) for the configuration API and [Fluent behavior](fluent-getting-started.md) for the native-control boundary.

## Lightweight Styling Portability

Semantic style keys also enable portable [lightweight styling](lightweight-styling.md). Material and Simple expose semantic resource keys for customizing control appearance:

```xml
<!-- This override works under both Material and Simple themes -->
<SolidColorBrush x:Key="FilledButtonForeground" Color="Red" />
```

Both **Material** and **Simple** templates reference the same unprefixed keys (e.g. `FilledButtonForeground`) directly.

### Override Compatibility

The following are current implementation gaps, not guarantees that consumers should rely on permanently:

| Surface | Current behavior and limitation |
|---|---|
| Missing style aliases | Simple lacks `ElevatedButtonStyle`, `CommandBarStyle`, `MediaTransportControlsStyle`, and `DatePickerFlyoutPresenterStyle`. Fluent also lacks `DatePickerFlyoutPresenterStyle`. A control's implicit fallback does not make an explicit missing semantic key resolve. |
| Simple outlined buttons | `OutlinedButtonStyle` uses the tonal template, which consumes `FilledTonalButton*`. The declared `OutlinedButton*` aliases do not customize it; use the tonal keys, affecting both variants. |
| Simple icon toggle buttons | The separate icon template consumes `TextToggleButton*` resources; Material's separate `IconToggleButton*` brush overrides are not portable to Simple. |
| Simple state overrides | Some CheckBox label, TextBox header/placeholder, and secondary HyperlinkButton interaction-state keys are declared but not read. See the [Simple control reference](simple-controls-styles.md) and its per-control notes. |
| Material state overrides | The selected RatingControl hover-foreground keys and the normal CalendarDatePicker glyph-foreground key are declared but not read by their matching states. |
| Button measurements | Material and Simple do not consistently consume the same border/typography keys. A key resolving successfully does not prove that changing it restyles the button. |
| Fluent palette and brush overrides | `PrimaryColor` is translated into native accent resources. Other semantic color/brush overrides still affect semantic resources, but generally do not change native controls that read different Fluent keys; for example, `OnPrimaryColor` does not set Fluent filled-button text. |
| Fluent lightweight overrides | Divergent semantic names require the theme's `Colors.OverrideDictionary` channel; page/control overrides generally need native Fluent names. Constructor overrides, nested merged dictionaries, text-button state keys, and isolated Dark-only accent overrides have additional gaps. See [Fluent lightweight styling](lightweight-styling.md#fluent-theme). |
| Typography root resource | Prefer the `DefaultFontFamily` property across themes. Overriding only the root resource does not cascade through Fluent's slot aliases; Material/Simple root aliases require application scope and can be superseded by generated concrete slot values. |

Use the style tables to check key availability and the per-control references to check the keys each template consumes. Identical names and resource-resolution tests alone are insufficient to establish override parity.

The color palette underneath these keys can itself be swapped wholesale — generated from a single seed color, and even changed at runtime — see [Seed Color Palette](seed-colors.md).

> [!NOTE]
> The **Fluent** theme bridges the semantic lightweight-styling keys to the built-in Fluent control resources for Button, TextBox, CheckBox, RadioButton, ToggleSwitch, and Slider. For CheckBox/RadioButton/Slider most key names are WinUI's own, so overrides work natively at any scope; for divergent names, app-wide overrides go through the theme's `Colors.OverrideDictionary` and page-scoped overrides target the Fluent per-control keys directly. See [Lightweight Styling — Fluent theme](lightweight-styling.md#fluent-theme).

### C# Markup Compatibility

`Uno.Themes.WinUI.Markup` exposes many semantic style and resource names through `Uno.Themes.Markup.Theme`. For example, `.Style(Theme.Button.Styles.Filled)` selects `FilledButtonStyle`. These helpers select resource keys; they do not add missing styles or translate overrides differently from XAML.

The current helper surface is incomplete. There are no style helpers for `ComboBoxItemStyle`, `DatePickerFlyoutPresenterStyle`, `MediaTransportControlsStyle`, `MenuFlyoutSeparatorStyle`, `MenuFlyoutSubItemStyle`, `RadioMenuFlyoutItemStyle`, or `ToggleMenuFlyoutItemStyle`. Typography helpers omit the font-family keys, and color helpers omit `ShadowColor`. Use the corresponding XAML keys directly where the active theme supports them.

Two helper type declarations also need correction: typography weight helpers use the `FontWeights` provider class, and the PipsPager previous/next button path-data helpers use `double` even though their resources contain path strings. For these resources, use XAML overrides with the declared resource type until the typed helpers are corrected. See [Lightweight Styling in C#](lightweight-styling.md#c-markup) for supported examples.

For more details on per-control lightweight styling resources, see the individual control style pages:

- [Button](styles/Button.md)
- [TextBox](styles/TextBox.md)
- [PasswordBox](styles/PasswordBox.md)
- [CheckBox](styles/CheckBox.md)
- [RadioButton](styles/RadioButton.md)
- [ToggleSwitch](styles/ToggleSwitch.md)
- [Slider](styles/Slider.md)
- [ToggleButton](styles/ToggleButton.md)
