# Typography Tokens: Material vs Fluent

This document maps Material Design 3 typography tokens to their WinUI/Fluent equivalents. Material uses a systematic type scale with 15 predefined styles, while Fluent uses named TextBlock styles.

## Material Typography Overview

Material Design 3 defines a **type scale** with 15 styles organized by role:

- **Display** (3 sizes): Large headlines, hero text
- **Headline** (3 sizes): Section headers
- **Title** (3 sizes): Medium emphasis headers
- **Body** (3 sizes + Caption variants): Body text, long-form content
- **Label** (4 sizes): Button text, labels, UI elements

Each style defines:

- `FontFamily` - Font family resource
- `FontWeight` - Normal, Medium, Bold, etc.
- `FontSize` - Size in pixels
- `CharacterSpacing` - Letter spacing (tracking) in 1/1000 of an "em"

**Source**: [Typography.xaml](../../src/library/Uno.Material/Styles/Application/v2/Typography.xaml)

## Character Spacing Calculation

CharacterSpacing is measured in 1/1000 of an "em" (current font size):

```text
CharacterSpacing = (tracking in px / FontSize) × 1000
```

**Example**: For 12px font with 0.5px tracking:

```text
CharacterSpacing = (0.5 / 12) × 1000 = 41.666 ≈ 41
```

## Display Scale

Large, prominent text for hero sections and important messaging.

| Material Token | Font Family | Weight | Size (px) | Tracking | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| `DisplayLargeFontFamily`, `DisplayLargeFontWeight`, `DisplayLargeFontSize`, `DisplayLargeCharacterSpacing` | Roboto Regular | Normal | 57 | -17 (-0.25px) | Custom or `HeaderTextBlockStyle` (larger) | Hero text |
| `DisplayMediumFontFamily`, `DisplayMediumFontWeight`, `DisplayMediumFontSize` | Roboto Regular | Normal | 45 | 0 | Custom or `HeaderTextBlockStyle` | Large display |
| `DisplaySmallFontFamily`, `DisplaySmallFontWeight`, `DisplaySmallFontSize` | Roboto Regular | Normal | 36 | 0 | `HeaderTextBlockStyle` | Small display |

**Display Usage**: Marketing headers, splash screens, empty states

## Headline Scale

Section headers and important titles.

| Material Token | Font Family | Weight | Size (px) | Tracking | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| `HeadlineLargeFontFamily`, `HeadlineLargeFontWeight`, `HeadlineLargeFontSize` | Roboto Regular | Normal | 32 | 0 | `TitleTextBlockStyle` | Large sections |
| `HeadlineMediumFontFamily`, `HeadlineMediumFontWeight`, `HeadlineMediumFontSize` | Roboto Regular | Normal | 28 | 0 | `SubtitleTextBlockStyle` | Medium sections |
| `HeadlineSmallFontFamily`, `HeadlineSmallFontWeight`, `HeadlineSmallFontSize` | Roboto Regular | Normal | 24 | 0 | `SubtitleTextBlockStyle` (smaller) | Small sections |

**Headline Usage**: Page titles, dialog titles, section headers

## Title Scale

Medium-emphasis headers for subsections and list items.

| Material Token | Font Family | Weight | Size (px) | Tracking | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| `TitleLargeFontFamily`, `TitleLargeFontWeight`, `TitleLargeFontSize` | Roboto Regular | Normal | 22 | 0 | `SubtitleTextBlockStyle` | List headers |
| `TitleMediumFontFamily`, `TitleMediumFontWeight`, `TitleMediumFontSize` | Roboto **Medium** | **Medium** | 16 | 0 | `BaseTextBlockStyle` | Card titles |
| `TitleSmallFontFamily`, `TitleSmallFontWeight`, `TitleSmallFontSize` | Roboto **Medium** | **Medium** | 14 | 0 | `BodyTextBlockStyle` (medium weight) | Small titles |

**Title Usage**: App bar titles, card headers, list item titles, tabs

## Body Scale

Body text for paragraphs, lists, and readable content.

| Material Token | Font Family | Weight | Size (px) | Tracking | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| `BodyLargeFontFamily`, `BodyLargeFontWeight`, `BodyLargeFontSize`, `BodyLargeCharacterSpacing` | Roboto **Medium** | **Medium** | 16 | 9 (0.15px) | `BodyTextBlockStyle` | Primary body text |
| `BodyMediumFontFamily`, `BodyMediumFontWeight`, `BodyMediumFontSize`, `BodyMediumCharacterSpacing` | Roboto **Medium** | **Medium** | 14 | 17 (0.25px) | `BodyTextBlockStyle` (14px) | Secondary body |
| `BodySmallFontFamily`, `BodySmallFontWeight`, `BodySmallFontSize`, `BodySmallCharacterSpacing` | Roboto **Medium** | **Medium** | 12 | 33 (0.4px) | `CaptionTextBlockStyle` | Small descriptions |

**Body Usage**: Paragraphs, descriptions, list items, form fields

## Label Scale

Labels for buttons, form fields, and UI elements.

| Material Token | Font Family | Weight | Size (px) | Tracking | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| `LabelLargeFontFamily`, `LabelLargeFontWeight`, `LabelLargeFontSize`, `LabelLargeCharacterSpacing` | Roboto **Medium** | **Medium** | 14 | 7 (0.1px) | Button default | Buttons, large labels |
| `LabelMediumFontFamily`, `LabelMediumFontWeight`, `LabelMediumFontSize`, `LabelMediumCharacterSpacing` | Roboto **Medium** | **Medium** | 12 | 41 (0.5px) | `CaptionTextBlockStyle` (medium) | Medium labels |
| `LabelSmallFontFamily`, `LabelSmallFontWeight`, `LabelSmallFontSize`, `LabelSmallCharacterSpacing` | Roboto **Medium** | **Medium** | 11 | 45 (0.5px) | `CaptionTextBlockStyle` (11px) | Small labels |
| `LabelExtraSmallFontFamily`, `LabelExtraSmallFontWeight`, `LabelExtraSmallFontSize`, `LabelExtraSmallCharacterSpacing` | Roboto **Medium** | Normal | 11 | 7 (0.1px) | Custom (Badges) | Badges, chips |

**Label Usage**: Button text, form labels, tabs, chips, badges

**Note**: `LabelExtraSmall` is an Uno-specific addition for badges, not part of official Material M3.

## Caption Scale (Gap Fillers)

Additional styles bridging gaps between Material and other design systems. **Not part of official Material M3.**

| Material Token | Font Family | Weight | Size (px) | Tracking | WinUI/Fluent Equivalent | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| `CaptionLargeFontFamily`, `CaptionLargeFontWeight`, `CaptionLargeFontSize`, `CaptionLargeCharacterSpacing` | Roboto **Medium** | **Medium** | 13 | -3 (-0.05px) | `CaptionTextBlockStyle` (13px) | Gap filler |
| `CaptionMediumFontFamily`, `CaptionMediumFontWeight`, `CaptionMediumFontSize`, `CaptionMediumCharacterSpacing` | Roboto **Medium** | **Medium** | 12 | 0 | `CaptionTextBlockStyle` | Gap filler |

**Caption Usage**: Custom scenarios requiring intermediate sizes

## Font Families

Material uses two font weights of Roboto, defined in [Fonts.xaml](../../src/library/Uno.Material/Styles/Application/Common/Fonts.xaml):

| Material Resource | Font Weight | File | WinUI/Fluent Equivalent |
| --- | --- | --- | --- |
| `MaterialRegularFontFamily` | Normal (400) | Roboto-Regular.ttf | `SegoeUI` |
| `MaterialMediumFontFamily` | Medium (500) | Roboto-Medium.ttf | `SegoeUI Semibold` |

Fluent uses Segoe UI (Windows) or SF Pro (iOS) as system fonts.

## Font Weight Values

| Material FontWeight | Numeric Value | WinUI Equivalent | Notes |
| --- | --- | --- | --- |
| `Normal` | 400 | `Normal` | Regular weight |
| `Medium` | 500 | `Medium` or `SemiBold` | Emphasis |

## Typography Usage by Component

| Component | Recommended Material Scale | Fluent Equivalent |
| --- | --- | --- |
| **Button** | `LabelLarge` (14px Medium) | Default button font |
| **AppBar Title** | `TitleLarge` (22px) | `SubtitleTextBlockStyle` |
| **Dialog Title** | `HeadlineMedium` (28px) | `TitleTextBlockStyle` |
| **Body Text** | `BodyLarge` or `BodyMedium` | `BodyTextBlockStyle` |
| **List Item** | `BodyLarge` (title), `BodyMedium` (subtitle) | `BodyTextBlockStyle` |
| **TextBox Label** | `BodyMedium` (14px) | Default TextBox font |
| **Chip/Badge** | `LabelExtraSmall` (11px) | `CaptionTextBlockStyle` |
| **FAB** | `LabelLarge` (14px Medium) | Custom |

## Using Typography Tokens

### XAML

```xml
<TextBlock Text="Display Large"
           FontFamily="{StaticResource DisplayLargeFontFamily}"
           FontWeight="{StaticResource DisplayLargeFontWeight}"
           FontSize="{StaticResource DisplayLargeFontSize}"
           CharacterSpacing="{StaticResource DisplayLargeCharacterSpacing}" />

<TextBlock Text="Body Large"
           FontFamily="{StaticResource BodyLargeFontFamily}"
           FontWeight="{StaticResource BodyLargeFontWeight}"
           FontSize="{StaticResource BodyLargeFontSize}"
           CharacterSpacing="{StaticResource BodyLargeCharacterSpacing}" />
```

### C# Markup

```csharp
using Uno.Themes.Markup;

new TextBlock()
    .FontFamily(Theme.Typography.DisplayLarge.FontFamily)
    .FontWeight(Theme.Typography.DisplayLarge.FontWeight)
    .FontSize(Theme.Typography.DisplayLarge.FontSize)
    .CharacterSpacing(Theme.Typography.DisplayLarge.CharacterSpacing);
```

### Lightweight Styling

```xml
<TextBlock Text="Title"
           Style="{StaticResource MaterialTitleLargeTextBlockStyle}" />
```

See [TextBlock Lightweight Styling](../styles/TextBlock.md) for predefined styles.

## Fluent Typography Comparison

| Fluent Style | Size (px) | Weight | Closest Material Scale |
| --- | --- | --- | --- |
| `HeaderTextBlockStyle` | 46 | Light | `DisplayMedium` (45px) |
| `SubheaderTextBlockStyle` | 34 | Light | `DisplaySmall` (36px) |
| `TitleTextBlockStyle` | 28 | SemiLight | `HeadlineMedium` (28px) |
| `SubtitleTextBlockStyle` | 20 | Normal | `TitleLarge` (22px) |
| `BaseTextBlockStyle` | 15 | Normal | `TitleMedium` (16px) |
| `BodyTextBlockStyle` | 15 | Normal | `BodyLarge` (16px) |
| `CaptionTextBlockStyle` | 12 | Normal | `LabelMedium` (12px) |

## Customization

### Override Type Scale

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key="Default">
                <!-- Use larger body text -->
                <x:Double x:Key="BodyLargeFontSize">18</x:Double>
            </ResourceDictionary>
        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### Custom Font Family

```xml
<Application.Resources>
    <FontFamily x:Key="MaterialRegularFontFamily">Inter</FontFamily>
    <FontFamily x:Key="MaterialMediumFontFamily">Inter Medium</FontFamily>
</Application.Resources>
```

## Related Documentation

- [TextBlock Styles](../styles/TextBlock.md) - Lightweight styling for typography
- [Material Getting Started](../material-getting-started.md) - Font setup
- [Material Design Typography](https://m3.material.io/styles/typography) - Official M3 spec
