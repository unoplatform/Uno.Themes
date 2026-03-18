---
name: simple-autosuggestbox
description: Uses Simple Theme AutoSuggestBox styles in Uno Platform applications. Use when styling search or auto-complete input fields. Covers both AutoSuggestBox styles and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — AutoSuggestBox Styles

## Overview

The Simple theme provides a single AutoSuggestBox style. When the Simple theme is active, the **implicit default style** applied to every `<AutoSuggestBox>` is `SimpleAutoSuggestBoxStyle`.

## Available Styles

| Style Key | Alias | IsDefaultStyle |
|-----------|-------|----------------|
| `SimpleAutoSuggestBoxStyle` | `AutoSuggestBoxStyle` | **Yes** |

## Usage Examples

```xml
<!-- Implicit — no Style needed -->
<AutoSuggestBox PlaceholderText="Search..."
                QueryIcon="Find" />
```

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleAutoSuggestBoxForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxForegroundPointerOver` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxForegroundFocused` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleAutoSuggestBoxPlaceholderForeground` | `SolidColorBrush` | `SimpleTextDefaultTertiaryBrush` |
| `SimpleAutoSuggestBoxPlaceholderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |
| `SimpleAutoSuggestBoxBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxBackgroundPointerOver` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxBackgroundFocused` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxBackgroundDisabled` | `SolidColorBrush` | `SimpleBackgroundDisabledDefaultBrush` |
| `SimpleAutoSuggestBoxBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxBorderBrushPointerOver` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxBorderBrushFocused` | `SolidColorBrush` | `SimpleBorderBrandSecondaryBrush` |
| `SimpleAutoSuggestBoxBorderBrushDisabled` | `SolidColorBrush` | `SimpleBorderDisabledDefaultBrush` |
| `SimpleAutoSuggestBoxIconForeground` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxIconForegroundPointerOver` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxIconForegroundPressed` | `SolidColorBrush` | `SimpleIconDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxIconForegroundDisabled` | `SolidColorBrush` | `SimpleIconDisabledDefaultBrush` |
| `SimpleAutoSuggestBoxHeaderForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxHeaderForegroundDisabled` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

### Suggestions Popup Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleAutoSuggestBoxSuggestionsBackground` | `SolidColorBrush` | `SimpleBackgroundDefaultDefaultBrush` |
| `SimpleAutoSuggestBoxSuggestionsBorderBrush` | `SolidColorBrush` | `SimpleBorderDefaultDefaultBrush` |

### Layout Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleAutoSuggestBoxCornerRadius` | `CornerRadius` | `20` (pill shape) |
| `SimpleAutoSuggestBoxBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimpleAutoSuggestBoxFocusedBorderThickness` | `Thickness` | `SimpleStrokeFocusRingThickness` |
| `SimpleAutoSuggestBoxPadding` | `Thickness` | `16,10,40,10` |
| `SimpleAutoSuggestBoxMinHeight` | `Double` | `SimpleIconLarge` |
| `SimpleAutoSuggestBoxIconFontSize` | `Double` | `SimpleTypographyScale03` |
| `SimpleAutoSuggestBoxSuggestionsCornerRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimpleAutoSuggestBoxSuggestionsBorderThickness` | `Thickness` | `SimpleStrokeBorderThickness` |
| `SimpleAutoSuggestBoxSuggestionsPadding` | `Thickness` | `SimpleSpace100Thickness` |

## Related Skills

- [simple-textbox](../simple-textbox/SKILL.md)
- [simple-combobox](../simple-combobox/SKILL.md)
