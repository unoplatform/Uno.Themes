---
uid: Uno.Themes.Omarchy.Styles
---

# Omarchy Controls Styles

Styles, palette resources and tokens shipped by `Uno.Omarchy.WinUI`. See [Uno Omarchy](omarchy-getting-started.md) to install the theme and pick a palette.

## Control Styles

`{Ansi}` stands for one of the eight ANSI accent variants: `Black`, `White`, `Red`, `Green`, `Yellow`, `Blue`, `Magenta`, `Cyan` (for example `OmarchyFilledButtonRedStyle`). An accent variant only swaps the colors of the base style — the normal ANSI color for the tint and border, the bright one for the text — mirroring the `accent` parameter of the flutter_omarchy widgets.

| Control                | Style Key                                   | IsDefaultStyle\* |
|------------------------|---------------------------------------------|------------------|
| `Button`               | `OmarchyOutlinedButtonStyle`                | True             |
| `Button`               | `OmarchyOutlinedButton{Ansi}Style`          |                  |
| `Button`               | `OmarchyFilledButtonStyle`                  |                  |
| `Button`               | `OmarchyFilledButton{Ansi}Style`            |                  |
| `Button`               | `OmarchyTextButtonStyle`                    |                  |
| `Button`               | `OmarchyTextButton{Ansi}Style`              |                  |
| `Button`               | `OmarchyIconButtonStyle`                    |                  |
| `Button`               | `OmarchyIconButton{Ansi}Style`              |                  |
| `CheckBox`             | `OmarchyCheckBoxStyle`                      | True             |
| `CheckBox`             | `OmarchyAccentCheckBoxStyle`                |                  |
| `CheckBox`             | `OmarchyCheckBox{Ansi}Style`                |                  |
| `ComboBox`             | `OmarchyComboBoxStyle`                      | True             |
| `ComboBoxItem`         | `OmarchyComboBoxItemStyle`                  | True             |
| `ContentDialog`        | `OmarchyContentDialogStyle`                 | True             |
| `FlyoutPresenter`      | `OmarchyFlyoutPresenterStyle`               | True             |
| `HyperlinkButton`      | `OmarchyHyperlinkButtonStyle`               | True             |
| `HyperlinkButton`      | `OmarchySecondaryHyperlinkButtonStyle`      |                  |
| `ListView`             | `OmarchyListViewStyle`                      | True             |
| `ListViewItem`         | `OmarchyListViewItemStyle`                  | True             |
| `MenuFlyoutPresenter`  | `OmarchyMenuFlyoutPresenterStyle`           | True             |
| `MenuFlyoutItem`       | `OmarchyMenuFlyoutItemStyle`                | True             |
| `MenuFlyoutSeparator`  | `OmarchyMenuFlyoutSeparatorStyle`           | True             |
| `MenuFlyoutSubItem`    | `OmarchyMenuFlyoutSubItemStyle`             | True             |
| `ToggleMenuFlyoutItem` | `OmarchyToggleMenuFlyoutItemStyle`          | True             |
| `RadioMenuFlyoutItem`  | `OmarchyRadioMenuFlyoutItemStyle`           | True             |
| `NavigationView`       | `OmarchyNavigationViewStyle`                | True             |
| `NavigationViewItem`   | `OmarchyNavigationViewItemStyle`            | True             |
| `PasswordBox`          | `OmarchyOutlinedPasswordBoxStyle`           | True             |
| `PasswordBox`          | `OmarchyFilledPasswordBoxStyle`             |                  |
| `ProgressBar`          | `OmarchyProgressBarStyle`                   | True             |
| `ProgressBar`          | `OmarchyProgressBar{Ansi}Style`             |                  |
| `ProgressRing`         | `OmarchyProgressRingStyle`                  | True             |
| `ProgressRing`         | `OmarchyProgressRing{Ansi}Style`            |                  |
| `RadioButton`          | `OmarchyRadioButtonStyle`                   | True             |
| `RadioButton`          | `OmarchyAccentRadioButtonStyle`             |                  |
| `RadioButton`          | `OmarchyRadioButton{Ansi}Style`             |                  |
| `Slider`               | `OmarchySliderStyle`                        | True             |
| `Slider`               | `OmarchySlider{Ansi}Style`                  |                  |
| `TextBlock`            | `OmarchyBaseTextBlockStyle`                 | True             |
| `TextBox`              | `OmarchyOutlinedTextBoxStyle`               | True             |
| `TextBox`              | `OmarchyFilledTextBoxStyle`                 |                  |
| `ToggleButton`         | `OmarchyTextToggleButtonStyle`              | True             |
| `ToggleButton`         | `OmarchyIconToggleButtonStyle`              |                  |
| `ToggleSwitch`         | `OmarchyToggleSwitchStyle`                  | True             |
| `ToggleSwitch`         | `OmarchyToggleSwitch{Ansi}Style`            |                  |
| `ToolTip`              | `OmarchyToolTipStyle`                       | True             |

\* Styles marked as `IsDefaultStyle` are applied implicitly to every control of that type.

The typography styles are `OmarchyDisplayLarge` … `OmarchyCaptionSmall`, one per slot of the shared type scale.

### Semantic style keys

The theme-agnostic [semantic style keys](semantic-styles.md) (`FilledButtonStyle`, `OutlinedTextBoxStyle`, `DisplayLarge`, …) resolve to the Omarchy styles above; the full mapping is in the Omarchy column of the semantic styles tables. Controls flutter_omarchy has no widget for — `AppBarButton`, `CommandBar`, `CalendarView`, `CalendarDatePicker`, `DatePicker`, `MediaTransportControls`, `PipsPager`, `RatingControl` and the elevated button — have no Omarchy style, and their semantic keys are not defined.

The semantic aliases and the lightweight styling keys below are declared in `Styles/Controls/SemanticStyles.xaml`, a `SemanticResources` dictionary, so tooling can tell them from the `Omarchy`-prefixed keys.

## Palette Resources

The active palette (`OmarchyTheme.Palette`) is exposed as colors and brushes. Brushes are stable instances whose color is rewritten when the palette changes, so content already on screen repaints.

| Color Key                         | Brush Key                         | Palette Value     |
|-----------------------------------|-----------------------------------|-------------------|
| `OmarchyBackgroundColor`          | `OmarchyBackgroundBrush`          | `Background`      |
| `OmarchyForegroundColor`          | `OmarchyForegroundBrush`          | `Foreground`      |
| `OmarchyAccentColor`              | `OmarchyAccentBrush`              | `Accent`          |
| `OmarchySelectionColor`           | `OmarchySelectionBrush`           | `Selection`       |
| `OmarchyMutedColor`               | `OmarchyMutedBrush`               | `Muted`           |
| `OmarchyNormal{Ansi}Color`        | `OmarchyNormal{Ansi}Brush`        | `Normal.{Ansi}`   |
| `OmarchyBright{Ansi}Color`        | `OmarchyBright{Ansi}Brush`        | `Bright.{Ansi}`   |

A palette is either light or dark and has no light and dark variants: the same values are applied under both the `Light` and `Dark` XAML themes.

### Semantic Color Roles

The shared semantic colors (`PrimaryColor`, `SurfaceColor`, …, and their `*Brush` counterparts) are mapped from the palette, so theme-agnostic XAML keeps working.

| Role                                                          | Palette Value                              |
|---------------------------------------------------------------|--------------------------------------------|
| `Primary`, `PrimaryInverse`, `PrimaryVariantDark`, `PrimaryVariantLight`, `SurfaceTint` | `Accent`                 |
| `OnPrimary`, `OnSecondary`, `OnTertiary`, `OnError`           | `Background`                               |
| `PrimaryContainer`                                            | `Accent` at 15 % over `Background`         |
| `OnPrimaryContainer`                                          | `Accent`                                   |
| `Secondary`, `SecondaryVariantDark`                           | `Normal.Magenta`                           |
| `SecondaryContainer`                                          | `Normal.Magenta` at 15 % over `Background` |
| `OnSecondaryContainer`, `SecondaryVariantLight`               | `Bright.Magenta`                           |
| `Tertiary`                                                    | `Normal.Cyan`                              |
| `TertiaryContainer`                                           | `Normal.Cyan` at 15 % over `Background`    |
| `OnTertiaryContainer`                                         | `Bright.Cyan`                              |
| `Error`                                                       | `Normal.Red`                               |
| `ErrorContainer`                                              | `Normal.Red` at 15 % over `Background`     |
| `OnErrorContainer`                                            | `Bright.Red`                               |
| `Background`, `Surface`, `OnSurfaceInverse`                   | `Background`                               |
| `OnBackground`, `OnSurface`, `SurfaceInverse`                 | `Foreground`                               |
| `SurfaceVariant`, `OutlineVariant`                            | `Normal.Black`                             |
| `OnSurfaceVariant`                                            | `Muted`                                    |
| `Outline`                                                     | `Normal.White`                             |

"At 15 % over `Background`" is the opaque result of compositing the Omarchy filled tint over the background, so container roles look like Omarchy's filled widgets without translucent brushes.

Individual roles can still be overridden through `OmarchyTheme.Colors`; overrides take precedence over the palette.

## Tokens

| Key                                  | Value  | Usage                                                        |
|--------------------------------------|--------|--------------------------------------------------------------|
| `OmarchyTintOpacity`                 | 0.15   | Filled tint at rest; outline and bar hover                   |
| `OmarchyTintOpacityPointerOver`      | 0.25   | Filled hover; outline and bar pressed                        |
| `OmarchyTintOpacityPressed`          | 0.35   | Filled pressed                                               |
| `OmarchyTintOpacityDisabled`         | 0.02   | Filled disabled                                              |
| `OmarchyForegroundOpacityDisabled`   | 0.3    | Disabled foreground                                          |
| `OmarchyBorderOpacityDisabled`       | 0.5    | Disabled outline border                                      |
| `OmarchyMutedForegroundOpacity`      | 0.6    | Muted foreground                                             |
| `OmarchyFocusBorderOpacity`          | 0.5    | Focus stroke                                                 |
| `OmarchyCheckFillOpacity`            | 0.4    | Check box and radio button accent fill                       |
| `OmarchyCheckFillOpacityHighlighted` | 0.24   | Accent fill while hovered                                    |
| `OmarchyHighlightOpacity`            | 0.6    | Check box and radio button hover                             |
| `OmarchyHighlightSelectedOpacity`    | 0.9    | Hover border while checked                                   |
| `OmarchySliderTrackOpacity`          | 0.6    | Slider filled track                                          |
| `OmarchySliderThumbOpacity`          | 0.9    | Slider thumb at rest                                         |
| `OmarchyTileHoverOpacity`            | 0.5    | List and navigation item hover                               |
| `OmarchyBorderWidth` / `OmarchyBorderThickness`   | 2 | Buttons, inputs, dialogs, pop-overs                  |
| `OmarchyDividerWidth` / `OmarchyDividerThickness` | 1 | Dividers                                             |
| `OmarchyCornerRadius`                | 0      | Every control: sharp corners are the identity of the theme   |
| `OmarchyTileHeight`                  | 56     | List and navigation item height                              |
| `OmarchyAnimationDuration` / `OmarchyAnimationDurationTimeSpan` | 0.12 s | Transitions                       |

The `Space*` spacing scale and the density tokens are generated by the theme as for the other design systems (see [Design Tokens](design-tokens.md)). The `Radius*` shape scale is generated too, but no Omarchy style consumes it.

## Lightweight Styling

Omarchy templates derive hover, pressed and disabled visuals from one color per role and the opacity tokens above, instead of a brush per state. The button styles therefore expose only their base keys (`*Background`, `*Foreground`, `*BorderBrush`); the `*PointerOver`, `*Pressed` and `*Disabled` button keys of the other themes do not exist here. Other controls expose the state keys listed below. See [Lightweight Styling](lightweight-styling.md) for how to override them.

| Control           | Keys                                                                                                                                                     |
|-------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Button`          | `FilledButton*`, `FilledTonalButton*`, `OutlinedButton*`, `IconButton*` (`Background`, `Foreground`, `BorderBrush`); `TextButtonBackground`, `TextButtonForeground` |
| `CheckBox`        | `CheckBoxForeground`, `CheckBoxBorderBrushUnchecked`, `CheckBoxBackgroundUnchecked`, `CheckBoxBorderBrushChecked`, `CheckBoxBackgroundChecked`, `CheckBoxGlyphForegroundChecked` |
| `ComboBox`        | `ComboBoxForeground`, `ComboBoxBackground`, `ComboBoxBorderBrush`, `ComboBoxPlaceholderForeground`, `ComboBoxHeaderForeground`, `ComboBoxDropDownBackground`, `ComboBoxDropDownBorderBrush`, `ComboBoxItemForeground`, `ComboBoxItemForegroundPointerOver`, `ComboBoxItemForegroundSelected`, `ComboBoxItemBackgroundPointerOver` |
| `ContentDialog`   | `ContentDialogBackground`, `ContentDialogForeground`, `ContentDialogBorderBrush`, `ContentDialogTitleForeground`, `ContentDialogDividerBrush`, `ContentDialogSmokeFill` |
| `FlyoutPresenter` | `FlyoutPresenterBackground`, `FlyoutPresenterBorderBrush`, `FlyoutPresenterBorderThickness`                                                              |
| `HyperlinkButton` | `HyperlinkButtonBackground`, `HyperlinkButtonForeground`, `SecondaryHyperlinkButtonBackground`, `SecondaryHyperlinkButtonForeground`, `HyperlinkUnderlineVisible` |
| `ListView`        | `ListViewBackground`, `ListViewItemBackground`, `ListViewItemForeground`, `ListViewItemBackgroundPointerOver`, `ListViewItemForegroundPointerOver`, `ListViewItemForegroundSelected`, `ListViewItemForegroundDisabled` |
| `MenuFlyout`      | `MenuFlyoutPresenterBackground`, `MenuFlyoutPresenterBorderBrush`, `MenuFlyoutItemBackground`, `MenuFlyoutItemForeground`, `MenuFlyoutItemBackgroundPointerOver`, `MenuFlyoutItemForegroundPointerOver`, `MenuFlyoutItemForegroundPressed`, `MenuFlyoutItemForegroundDisabled`, `MenuFlyoutItemKeyboardAcceleratorTextForeground`, `MenuFlyoutSubItemChevron`, `ToggleMenuFlyoutItemCheckGlyphForeground`, `MenuFlyoutSeparatorBackground` |
| `NavigationView`  | `NavigationViewDefaultPaneBackground`, `NavigationViewExpandedPaneBackground`, `NavigationViewTopPaneBackground`, `NavigationViewContentBackground`, `NavigationViewDividerBrush`, `NavigationViewItemSeparatorForeground`, `TopNavigationViewItemSeparatorForeground`, `NavigationViewPaneTitleForeground`, `NavigationViewHeaderForeground`, `NavigationViewItemHeaderForeground`, `NavigationViewButtonForeground`, `NavigationViewButtonBackground`, `NavigationViewFocusVisualBrush`, `NavigationViewItemForeground`, `NavigationViewItemForegroundPointerOver`, `NavigationViewItemForegroundPressed`, `NavigationViewItemForegroundSelected`, `NavigationViewItemIconForeground`, `NavigationViewItemBackground`, `NavigationViewSelectionIndicatorForeground`, `NavigationViewCompactPaneLength`, `PaneToggleButtonWidth`, `PaneToggleButtonHeight` |
| `PasswordBox`     | `OutlinedPasswordBox*` and `FilledPasswordBox*`: `Foreground`, `Background`, `BorderBrush`, `BorderBrushPointerOver`, `BorderBrushFocused`, `PlaceholderForeground`, `HeaderForeground`, `SelectionHighlightColor`, `BorderThickness`, `Padding` |
| `ProgressBar`     | `ProgressBarForeground`, `ProgressBarBackground`                                                                                                         |
| `ProgressRing`    | `ProgressRingForeground`                                                                                                                                 |
| `RadioButton`     | `RadioButtonForeground`, `RadioButtonBorderBrushUnchecked`, `RadioButtonBackgroundUnchecked`, `RadioButtonBorderBrushChecked`, `RadioButtonBackgroundChecked`, `RadioButtonCheckGlyphFill` |
| `Slider`          | `SliderForeground`, `SliderTrackFill`, `SliderThumbBackgroundPressed`, `SliderTrackValueFillDisabled`, `SliderThumbBackgroundDisabled`, `SliderHeaderForeground` |
| `TextBlock`       | `TextBlockForeground`                                                                                                                                    |
| `TextBox`         | `OutlinedTextBox*` and `FilledTextBox*`: `Foreground`, `Background`, `BorderBrush`, `BorderBrushPointerOver`, `BorderBrushFocused`, `PlaceholderForeground`, `HeaderForeground`, `SelectionHighlightColor`, `BorderThickness`, `Padding` |
| `ToggleButton`    | `TextToggleButton*` and `IconToggleButton*`: `Background` and `Foreground`, each with the `PointerOver`, `Pressed`, `Checked`, `CheckedPointerOver` and `CheckedPressed` states |
| `ToggleSwitch`    | `ToggleSwitchForeground`, `ToggleSwitchFillOff`, `ToggleSwitchFillOn`, `ToggleSwitchFillOnPointerOver`, `ToggleSwitchKnobFillOff`, `ToggleSwitchKnobFillOffPointerOver`, `ToggleSwitchKnobFillOn` |
| `ToolTip`         | `ToolTipBackground`, `ToolTipForeground`                                                                                                                 |

## Typography

Omarchy uses the CaskaydiaMono Nerd Font Mono face bundled with the package. `DefaultFontFamily` is the regular cut; titles (`Display*`, `Headline*`, `Title*`) use the bold cut through `OmarchyBoldFontFamily`, and captions (`Caption*`) the italic cut through `OmarchyItalicFontFamily`. Setting `DefaultFontFamily` on the theme replaces the face of every type scale; see [Typography Font Swap](design-tokens.md#typography-font-swap).
