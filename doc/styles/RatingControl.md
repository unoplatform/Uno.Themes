---
uid: Uno.Themes.Styles.RatingControl
---

# RatingControl Control

The tables below describe Material. A selected rating uses `RatingControlSelectedForeground` normally and `RatingControlSelectedForegroundPointerOver` while hovered. The secondary style uses the corresponding `SecondaryRatingControl*` keys. Override these state resources independently to customize the selected and hovered appearances.

## Styles

| Style Key                     | IsDefaultStyle\* |
|-------------------------------|------------------|
| `RatingControlStyle`          | True             |
| `SecondaryRatingControlStyle` |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                                      | Type                | Value                       |
|----------------------------------------------------------|---------------------|-----------------------------|
| `RatingControlHeight`                                    | `Double`            | 32                          |
| `RatingControlCaptionHeight`                             | `Double`            | 32                          |
| `SecondaryRatingControlCaptionHeight`                    | `Double`            | 32                          |
| `RatingControlForeground`                                | `SolidColorBrush`   | `PrimaryBrush`              |
| `RatingControlUnselectedForeground`                      | `SolidColorBrush`   | `OnSurfaceLowBrush`         |
| `RatingControlSelectedForeground`                        | `SolidColorBrush`   | `PrimaryBrush`              |
| `RatingControlPlaceholderForeground`                     | `SolidColorBrush`   | `OnSurfaceLowBrush`         |
| `RatingControlPlaceholderForegroundPointerOver`          | `SolidColorBrush`   | `PrimaryBrush`              |
| `RatingControlUnselectedForegroundPointerOver`           | `SolidColorBrush`   | `PrimaryBrush`              |
| `RatingControlSelectedForegroundPointerOver`             | `SolidColorBrush`   | `PrimaryBrush`              |
| `RatingControlSelectedForegroundDisabled`                | `SolidColorBrush`   | `OnSurfaceLowBrush`         |
| `SecondaryRatingControlForeground`                       | `SolidColorBrush`   | `SecondaryBrush`            |
| `SecondaryRatingControlUnselectedForeground`             | `SolidColorBrush`   | `OnSurfaceLowBrush`         |
| `SecondaryRatingControlSelectedForeground`               | `SolidColorBrush`   | `SecondaryBrush`            |
| `SecondaryRatingControlPlaceholderForeground`            | `SolidColorBrush`   | `OnSurfaceLowBrush`         |
| `SecondaryRatingControlPlaceholderForegroundPointerOver` | `SolidColorBrush`   | `SecondaryBrush`            |
| `SecondaryRatingControlUnselectedForegroundPointerOver`  | `SolidColorBrush`   | `SecondaryBrush`            |
| `SecondaryRatingControlSelectedForegroundPointerOver`    | `SolidColorBrush`   | `SecondaryBrush`            |
| `SecondaryRatingControlSelectedForegroundDisabled`       | `SolidColorBrush`   | `OnSurfaceLowBrush`         |
| `RatingControlCaptionForeground`                         | `SolidColorBrush`   | `OnSurfaceBrush`            |
| `RatingControlCaptionFontFamily`                         | `FontFamily`        | `DefaultFontFamily`         |
| `RatingControlCaptionStyle`                              | `StaticResourceRef` | `CaptionMedium`             |
| `RatingControlFontFamily`                                | `FontFamily`        | `SymbolThemeFontFamily`     |
| `SecondaryRatingControlCaptionForeground`                | `SolidColorBrush`   | `OnSurfaceBrush`            |
| `SecondaryRatingControlCaptionFontFamily`                | `FontFamily`        | `DefaultFontFamily`         |
| `SecondaryRatingControlCaptionStyle`                     | `StaticResourceRef` | `CaptionMedium`             |
