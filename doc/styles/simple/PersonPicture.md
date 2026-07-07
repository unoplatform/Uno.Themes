---
uid: Uno.Themes.Simple.Styles.PersonPicture
---

# PersonPicture Control

## Styles

| Style Key                              | IsDefaultStyle\* |
|----------------------------------------|------------------|
| `SimplePersonPictureStyle`             |                  |
| `SimplePersonPictureSmallStyle`        |                  |
| `SimplePersonPictureLargeStyle`        |                  |
| `SimplePersonPictureSquareStyle`       |                  |
| `SimplePersonPictureSquareSmallStyle`  |                  |
| `SimplePersonPictureSquareLargeStyle`  |                  |

IsDefaultStyle\*: Styles in this column will be set as the default implicit style for the matching control

## Lightweight Styling

| Key                                              | Type              | Value                                      |
|--------------------------------------------------|-------------------|--------------------------------------------|
| `SimplePersonPictureSmallSize`                   | `Double`          | `SimpleIconSmall` (24)                     |
| `SimplePersonPictureMediumSize`                  | `Double`          | `SimpleIconMedium` (32)                    |
| `SimplePersonPictureLargeSize`                   | `Double`          | `SimpleIconLarge` (40)                     |
| `SimplePersonPictureInitialsSmallFontSize`       | `Double`          | 10                                         |
| `SimplePersonPictureInitialsMediumFontSize`      | `Double`          | `BodyMediumFontSize` (14)                  |
| `SimplePersonPictureInitialsLargeFontSize`       | `Double`          | `BodyLargeFontSize` (16)                   |

> [!NOTE]
> Corner radii for square variants are applied directly via `SimpleRadius100CornerRadius` and `SimpleRadius200CornerRadius` in the style templates, not through overridable resource keys. Brushes (`PrimaryBrush`, `OnPrimaryBrush`) are also applied directly in style setters and templates.
