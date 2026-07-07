---
uid: Uno.Themes.Simple.Styles.TextBlock
---

# TextBlock Control

## Styles

All styles are based on `SimpleBaseTextBlockStyle` and reference shared semantic typography tokens from `Typography.xaml`.

| Style Key               | Font Specs         | SDS Mapping         |
|-------------------------|--------------------|---------------------|
| `SimpleBaseTextBlockStyle` | 16px Normal     | (base)              |
| `SimpleDisplayLarge`    | 72px Bold          | title-hero          |
| `SimpleDisplayMedium`   | 48px Bold          | title-page          |
| `SimpleDisplaySmall`    | 40px Normal        |                     |
| `SimpleHeadlineLarge`   | 32px Normal        | subtitle            |
| `SimpleHeadlineMedium`  | 24px SemiBold      | heading             |
| `SimpleHeadlineSmall`   | 20px Normal        | subheading          |
| `SimpleTitleLarge`      | 20px SemiBold      |                     |
| `SimpleTitleMedium`     | 16px SemiBold      | body-strong         |
| `SimpleTitleSmall`      | 14px SemiBold      | body-small-strong   |
| `SimpleBodyLarge`       | 16px Normal        | body-base           |
| `SimpleBodyMedium`      | 14px Normal        | body-small          |
| `SimpleBodySmall`       | 12px Normal        | caption             |
| `SimpleLabelLarge`      | 16px SemiBold      | body-strong         |
| `SimpleLabelMedium`     | 14px SemiBold      | body-small-strong   |
| `SimpleLabelSmall`      | 12px SemiBold      |                     |
| `SimpleLabelExtraSmall` | 12px Normal        |                     |
| `SimpleCaptionLarge`    | 14px Normal        | body-small          |
| `SimpleCaptionMedium`   | 12px Normal        | caption             |
| `SimpleCaptionSmall`    | 12px Normal        |                     |

## Lightweight Styling

> [!NOTE]
> The TextBlock control does not expose overridable themed brush resource keys. The `Foreground` is set to `OnSurfaceBrush` directly in `SimpleBaseTextBlockStyle`. Font properties are driven by shared semantic typography tokens (e.g. `DisplayLargeFontSize`, `BodyLargeFontWeight`).
