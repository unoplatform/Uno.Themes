---
name: simple-textblock
description: Uses Simple Theme TextBlock typography styles in Uno Platform applications. Use when applying text styles for titles, headings, body, captions, and other typographic scales. Covers all TextBlock style keys, the SDS type scale, Material-compatible aliases, and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — TextBlock Typography Styles

## Overview

The Simple theme provides a rich typography scale for TextBlock. When the Simple theme is active, the **implicit default style** applied to every `<TextBlock>` is `SimpleBaseTextBlockStyle`.

The type scale has two naming systems:
1. **SDS (Simple Design System)** names — the canonical set (e.g., `SimpleTitleHeroTextBlockStyle`)
2. **Material-compatible aliases** — for cross-theme portability (e.g., `SimpleDisplayLarge`)

## SDS Typography Styles

| Style Key | FontSize | FontWeight | FontStyle | Use Case |
|-----------|----------|------------|-----------|----------|
| `SimpleBaseTextBlockStyle` | Body | Normal | — | **Default implicit style**. Body text, paragraphs |
| `SimpleTitleHeroTextBlockStyle` | TitleHero | Bold | — | Hero banners, splash screens |
| `SimpleTitlePageTextBlockStyle` | TitlePage | Bold | — | Page-level titles |
| `SimpleSubtitleTextBlockStyle` | Subtitle | Normal | — | Section subtitles, large descriptive text |
| `SimpleHeadingTextBlockStyle` | Heading | SemiBold | — | Section headings |
| `SimpleSubheadingTextBlockStyle` | Subheading | Normal | — | Subsection headings, large body |
| `SimpleBodyBaseTextBlockStyle` | Body | Normal | — | Standard body text (same as base) |
| `SimpleBodyStrongTextBlockStyle` | Body | SemiBold | — | Emphasized body text, labels |
| `SimpleBodyEmphasisTextBlockStyle` | Body | Normal | Italic | Quotes, emphasis, secondary info |
| `SimpleBodySmallTextBlockStyle` | BodySmall | Normal | — | Smaller body text, metadata |
| `SimpleBodySmallStrongTextBlockStyle` | BodySmall | SemiBold | — | Small emphasized text |
| `SimpleBodyCodeTextBlockStyle` | Body | Normal | — | Code/monospace text (uses `SimpleFontFamilyMono`) |
| `SimpleCaptionTextBlockStyle` | Caption | Normal | — | Captions, footnotes, timestamps |

## Material-Compatible Alias Styles

These map to SDS styles for cross-theme portability. Use these if your app might switch between Simple and Material themes.

### Display Scale

| Alias | Resolves To |
|-------|-------------|
| `SimpleDisplayLarge` / `DisplayLarge` | `SimpleTitleHeroTextBlockStyle` |
| `SimpleDisplayMedium` / `DisplayMedium` | `SimpleTitlePageTextBlockStyle` |
| `SimpleDisplaySmall` / `DisplaySmall` | `SimpleSubtitleTextBlockStyle` |

### Headline Scale

| Alias | Resolves To |
|-------|-------------|
| `SimpleHeadlineLarge` / `HeadlineLarge` | `SimpleHeadingTextBlockStyle` |
| `SimpleHeadlineMedium` / `HeadlineMedium` | `SimpleSubheadingTextBlockStyle` |
| `SimpleHeadlineSmall` / `HeadlineSmall` | `SimpleBodyStrongTextBlockStyle` |

### Title Scale

| Alias | Resolves To |
|-------|-------------|
| `SimpleTitleLarge` / `TitleLarge` | `SimpleTitlePageTextBlockStyle` |
| `SimpleTitleMedium` / `TitleMedium` | `SimpleHeadingTextBlockStyle` |
| `SimpleTitleSmall` / `TitleSmall` | `SimpleSubheadingTextBlockStyle` |

### Body Scale

| Alias | Resolves To |
|-------|-------------|
| `SimpleBodyLarge` / `BodyLarge` | `SimpleSubheadingTextBlockStyle` |
| `SimpleBodyMedium` / `BodyMedium` | `SimpleBodyBaseTextBlockStyle` |
| `SimpleBodySmall` / `BodySmall` | `SimpleBodySmallTextBlockStyle` |

### Label Scale

| Alias | Resolves To |
|-------|-------------|
| `SimpleLabelLarge` / `LabelLarge` | `SimpleBodyStrongTextBlockStyle` |
| `SimpleLabelMedium` / `LabelMedium` | `SimpleBodySmallStrongTextBlockStyle` |
| `SimpleLabelSmall` / `LabelSmall` | `SimpleCaptionTextBlockStyle` |
| `SimpleLabelExtraSmall` / `LabelExtraSmall` | `SimpleCaptionTextBlockStyle` |

### Caption Scale

| Alias | Resolves To |
|-------|-------------|
| `SimpleCaptionLarge` / `CaptionLarge` | `SimpleBodySmallTextBlockStyle` |
| `SimpleCaptionMedium` / `CaptionMedium` | `SimpleCaptionTextBlockStyle` |
| `SimpleCaptionSmall` / `CaptionSmall` | `SimpleCaptionTextBlockStyle` |

## Style Hierarchy (BasedOn)

```
SimpleBaseTextBlockStyle
├── SimpleTitleHeroTextBlockStyle → SimpleDisplayLarge
├── SimpleTitlePageTextBlockStyle → SimpleDisplayMedium, SimpleTitleLarge
├── SimpleSubtitleTextBlockStyle → SimpleDisplaySmall
├── SimpleHeadingTextBlockStyle → SimpleHeadlineLarge, SimpleTitleMedium
├── SimpleSubheadingTextBlockStyle → SimpleHeadlineMedium, SimpleTitleSmall, SimpleBodyLarge
├── SimpleBodyBaseTextBlockStyle → SimpleBodyMedium
├── SimpleBodyStrongTextBlockStyle → SimpleHeadlineSmall, SimpleLabelLarge
├── SimpleBodyEmphasisTextBlockStyle
├── SimpleBodySmallTextBlockStyle → SimpleBodySmall, SimpleCaptionLarge
│   └── SimpleBodySmallStrongTextBlockStyle → SimpleLabelMedium
├── SimpleBodyCodeTextBlockStyle
└── SimpleCaptionTextBlockStyle → SimpleLabelSmall, SimpleLabelExtraSmall, SimpleCaptionMedium, SimpleCaptionSmall
```

## Usage Examples

```xml
<!-- Implicit body text -->
<TextBlock Text="This uses the default body style." />

<!-- Page title -->
<TextBlock Text="Settings"
           Style="{StaticResource SimpleTitlePageTextBlockStyle}" />

<!-- Section heading -->
<TextBlock Text="Account"
           Style="{StaticResource SimpleHeadingTextBlockStyle}" />

<!-- Caption -->
<TextBlock Text="Last updated: March 2026"
           Style="{StaticResource SimpleCaptionTextBlockStyle}" />

<!-- Using Material alias for cross-theme compat -->
<TextBlock Text="Hello World"
           Style="{StaticResource DisplayLarge}" />
```

## Lightweight Styling

### Foreground Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimpleTextBlockForeground` | `SolidColorBrush` | `SimpleTextDefaultDefaultBrush` |
| `SimpleTextBlockSecondaryForeground` | `SolidColorBrush` | `SimpleTextDefaultSecondaryBrush` |
| `SimpleTextBlockTertiaryForeground` | `SolidColorBrush` | `SimpleTextDefaultTertiaryBrush` |
| `SimpleTextBlockBrandForeground` | `SolidColorBrush` | `SimpleTextBrandDefaultBrush` |
| `SimpleTextBlockDangerForeground` | `SolidColorBrush` | `SimpleTextDangerDefaultBrush` |
| `SimpleTextBlockDisabledForeground` | `SolidColorBrush` | `SimpleTextDisabledDefaultBrush` |

These foreground resources can be applied to any TextBlock by setting them as the `Foreground` property value, or overridden via lightweight styling at the App, Page, or Control level.

## Related Skills

- [simple-skills-index](../simple-skills-index/SKILL.md)
