---
name: simple-personpicture
description: Uses Simple Theme PersonPicture styles in Uno Platform applications. Use when styling avatar/profile picture controls with circle or square shapes in Small, Medium, or Large sizes. Covers all PersonPicture style variants and lightweight resource keys.
metadata:
  author: unoplatform
  version: "1.0"
  framework: uno-platform
  category: simple-theme-controls
---

# Simple Theme — PersonPicture Styles

## Overview

The Simple theme provides six PersonPicture styles across two shapes (circle, square) and three sizes (small, medium, large). **There is no implicit default style** — PersonPicture must be explicitly styled.

## Available Styles

### Circle Styles

| Style Key | Size | Use Case |
|-----------|------|----------|
| `SimplePersonPictureStyle` | Medium (default) | Standard user avatar |
| `SimplePersonPictureSmallStyle` | Small | Compact lists, chat bubbles, inline mentions |
| `SimplePersonPictureLargeStyle` | Large | Profile pages, hero sections |

### Square Styles

| Style Key | Size | Use Case |
|-----------|------|----------|
| `SimplePersonPictureSquareStyle` | Medium (default) | Organization logos, non-person entities |
| `SimplePersonPictureSquareSmallStyle` | Small | Compact organization lists |
| `SimplePersonPictureSquareLargeStyle` | Large | Organization profile pages |

## Style Hierarchy (BasedOn)

```
SimplePersonPictureStyle
├── SimplePersonPictureSmallStyle
└── SimplePersonPictureLargeStyle

SimplePersonPictureSquareStyle
├── SimplePersonPictureSquareSmallStyle
└── SimplePersonPictureSquareLargeStyle
```

## Usage Examples

```xml
<!-- Circle avatar (medium) -->
<PersonPicture DisplayName="John Doe"
               Style="{StaticResource SimplePersonPictureStyle}" />

<!-- Small circle in a list -->
<PersonPicture DisplayName="Jane"
               Style="{StaticResource SimplePersonPictureSmallStyle}" />

<!-- Large profile picture -->
<PersonPicture DisplayName="Admin"
               Style="{StaticResource SimplePersonPictureLargeStyle}" />

<!-- Square for an organization -->
<PersonPicture DisplayName="Acme Corp"
               Style="{StaticResource SimplePersonPictureSquareStyle}" />
```

## When to Use Each Shape

| Shape | When to Use |
|-------|-------------|
| **Circle** | People, user accounts, contacts — follows convention for human identity |
| **Square** | Organizations, teams, bots, apps, non-person entities |

## Lightweight Styling

### Color Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimplePersonPictureBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimplePersonPictureForeground` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |
| `SimplePersonPictureBadgeBackground` | `SolidColorBrush` | `SimpleBackgroundBrandDefaultBrush` |
| `SimplePersonPictureBadgeForeground` | `SolidColorBrush` | `SimpleTextBrandOnBrandBrush` |

### Sizing Resources

| Key | Type | Default Token |
|-----|------|---------------|
| `SimplePersonPictureSmallSize` | `Double` | `SimpleIconSmall` |
| `SimplePersonPictureMediumSize` | `Double` | `SimpleIconMedium` |
| `SimplePersonPictureLargeSize` | `Double` | `SimpleIconLarge` |
| `SimplePersonPictureSquareSmallRadius` | `CornerRadius` | `SimpleRadius100CornerRadius` |
| `SimplePersonPictureSquareMediumRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimplePersonPictureSquareLargeRadius` | `CornerRadius` | `SimpleRadius200CornerRadius` |
| `SimplePersonPictureInitialsSmallFontSize` | `Double` | `10` |
| `SimplePersonPictureInitialsMediumFontSize` | `Double` | `SimpleTypographyScale02` |
| `SimplePersonPictureInitialsLargeFontSize` | `Double` | `SimpleTypographyScale03` |

## Related Skills

- [simple-listview](../simple-listview/SKILL.md)
