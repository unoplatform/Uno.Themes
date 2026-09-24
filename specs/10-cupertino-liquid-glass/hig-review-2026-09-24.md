# Cupertino theme review against the Apple HIG and iOS — 2026-09-24

Requested by the maintainer: review the whole Cupertino theme against Apple's Human Interface Guidelines and real
iOS, fix what needs fixing, and report the comparison. This is the report; the changes shipped with it are listed
in [Fixes](#fixes), the gaps that remain in [Open](#open-items). Earlier passes: [hig-polish.md](hig-polish.md)
(2026-09-22, iOS/iPadOS confirmed as the reference) and the knob-lens rounds in [progress.md](progress.md).

## Method

- **Apple sources.** The current HIG text for 25 pages (toggles, sliders, buttons, text-fields, steppers, pickers,
  progress-indicators, alerts, menus, popovers, lists-and-tables, toolbars, sidebars, segmented-controls,
  page-controls, search-fields, tab-bars, materials, color, typography, layout, …) read from Apple's data
  endpoint, and all 643 `@2x` artwork files those pages reference, downloaded from
  `developer.apple.com/tutorials/images/com.apple.HIG/…` and **measured pixel by pixel** (row heights, separator
  colour, track thickness, menu hairlines). The digest in [apple-hig-reference.md](apple-hig-reference.md) was
  the checklist for colours, type and metrics; UIKit defaults filled what the HIG leaves unpublished.
- **Ours.** The Cupertino gallery page and every popup (menu, alert, calendar, wheel picker, toolbar overflow,
  navigation pane) rendered on Windows software Skia at 2× in Light and Dark, cropped and compared with the
  artwork. Four independent read-only audits of the XAML (selection controls; fields and pickers; buttons,
  text and containers; navigation, bars and foundations) produced per-control tables tagged [HIG] / [API] /
  [legacy UIKit] / [3P]. Everything below that says "renders" was seen in a capture; everything else is XAML.
- **Not done here.** GPU (Mac) rendering of the glass, iOS device screenshots side by side, browser runtime.
  The pressed-knob lens was tuned with the maintainer on the Mac in the previous rounds and is not re-judged.

## Comparison: iOS versus Cupertino

Legend: ✅ matches the reference · 🛠 fixed in this pass · ⚠ differs, left as is (see Open items) · — no iOS equivalent.

### Foundations

| Aspect | iOS reference | Cupertino | Verdict |
| --- | --- | --- | --- |
| System colours (12 hues, Light/Dark) | 2025 unified table [HIG] | Identical hex values | ✅ |
| Grays 1–6 | [HIG] | Identical | ✅ |
| Semantic labels / fills / separator alphas | 0.60/0.30/0.18 labels, 0.20/0.16/0.12/0.08 fills (0.36/0.32/0.24/0.18 dark), separator 0.29 / 0.60 [legacy UIKit] | Were one byte low on eight keys (`#4C` for 0.30, `#28` for 0.16 …) | 🛠 rounded to the nearest byte |
| Type ramp (Large Title 34/41 … Caption 2 11/13) | [HIG] | All 11 Apple-named styles match size, weight and leading | ✅ |
| Font | SF Pro (not redistributable) | Inter, the licensed metric substitute | ✅ (settled) |
| Button label weight | Body 17 Regular for plain / gray / tinted; Semibold on prominent [API] | 17 Medium everywhere | ⚠ |
| Disabled / medium / low opacity tokens | ≈ 0.30 / 0.60 / 0.30 [legacy UIKit] | `DisabledOpacity` 0.12, `MediumOpacity` 0.64, `LowOpacity` 0.32 come from the shared Material palette and cannot be overridden from the Cupertino palette (StaticResource at merge time) | ⚠ |
| Hover / pressed idiom | iPadOS pointer: light fill behind the item; press ≈ 0.75 highlight [API] | Whole-element opacity dim 0.85 / 0.6 | ⚠ |
| Motion | Interactive spring 0.15 s, default ≈ 0.35–0.55 s [API] | Knob lift 0.15 s ✅; switch toggle was instantaneous | 🛠 0.2 s glide |

### Selection and progress

| Control | iOS reference | Cupertino | Verdict |
| --- | --- | --- | --- |
| Toggle switch geometry | Capsule track with capsule knob (HIG artwork, 64 × 28 / 40 × 24 at 2×) | Same | ✅ (settled) |
| Toggle motion | Knob slides, track cross-fades (UISwitch) | `Duration="0"` jump | 🛠 |
| Slider at rest | Thin track, tertiary fill, soft thumb shadow | Same | ✅ |
| Slider details | Ticks in the accent; capsule track on every platform | Tick `Fill` bound to a `Color`; `CupertinoSecondaryBrush` undefined | 🛠 |
| Progress bar | 4 pt capsule track in a visible gray (artwork: `#DEDEDE` on `#F7F7F7`) | Stock Fluent 1 px hairline track in `#E5E5EA`, invisible on the page | 🛠 4 pt system-fill track, 2 pt radii, Apple yellow/red for paused/error |
| Activity indicator | 8 spokes, 20 / 37 pt | Same | ✅ |
| Page control | 7 pt dots, ≈ 30 % inactive | Same | ✅ |
| Checkbox (no iOS control) | macOS: gray outline, accent fill when checked | Accent outline when unchecked; `CheckedPressed` empty off-Windows so a pressed checked box flashed unchecked; three undefined brushes | 🛠 gray outline, pressed state, brushes |
| Radio (no iOS control) | Outline until selected | Dark mode drew a solid `#8E8E93` disc when unchecked | 🛠 |
| Rating | macOS only; App Store uses outline stars | Filled gray stars | ⚠ (compat control) |

### Fields and pickers

| Control | iOS reference | Cupertino | Verdict |
| --- | --- | --- | --- |
| Text field / password | Rounded field, 17 pt, placeholder 30 % label; clear button in a 44 hit region | Same; clear button was 48 and grew the field | 🛠 44 |
| Stepper field | Header above a 44 pt field; stepper 94 × 32 in a row | Header **inside** a 1 px outlined box, 61 pt tall | 🛠 header above, frame 44 |
| Pop-up button (ComboBox) | Gray capsule, label + tinted chevron [HIG pop-up buttons] | 1 px outlined rectangle, radius 10; press/focus overlays painted white on white | 🛠 gray capsule, accent chevron, system-fill press |
| Compact date pickers | Gray capsule, blue value | Same; CalendarDatePicker animated a border it does not have, took Fluent's focus fill and flipped the value to black once chosen | 🛠 |
| Wheel picker | ≈ 23 pt rows in a tertiary-fill band | 14 pt rows | 🛠 20 pt |
| Inline calendar | 13 pt semibold weekday letters, secondary label; today in the accent | Weekday letters 12 pt regular label; **today was not drawn at all** | 🛠 weekday letters; today kept visible by not highlighting it (see Open items) |
| Header labels | 13 pt, secondary label | 13 pt, label colour | ⚠ |

### Buttons, text and containers

| Control | iOS reference | Cupertino | Verdict |
| --- | --- | --- | --- |
| Buttons | Capsule; plain / gray / tinted / filled; ≥ 44 hit region | Same hierarchy | ✅ |
| Link | Plain tinted text | Dead `HyperlinkUnderlineVisible` key and a duplicated glyph key | 🛠 removed |
| Menu | 250 wide, 44 rows, hairlines between rows, 8 pt band between groups, soft shadow (artwork) | Rows ✅; group separator was a hairline in a gap; hover/pressed fills `#1C1C1F` on `#1C1C1E` in Dark | 🛠 8 pt band, translucent fills; ⚠ no per-row hairlines, no shadow |
| Alert | 270 wide, title 17 semibold, message 13, filled capsule actions (artwork) | Message 15; title clamped to two lines | 🛠 13 pt, no clamp |
| Inset-grouped list | 52 pt rows (artwork), `#C6C6C8` separators (artwork), gray selection, no line under the last row | 44 pt rows; separators `#E5E5EA` on white, i.e. **invisible**; accent-tinted selection; a line under the last row | 🛠 separator colour, neutral selection, last row clean; ⚠ rows stay 44 |
| Popover / tooltip | Regular glass, soft shadow | Thick glass, hairline rim, no shadow | ⚠ |

### Navigation and bars

| Control | iOS reference | Cupertino | Verdict |
| --- | --- | --- | --- |
| Toolbar group | Items on shared glass capsule, 44 pt, monochrome glyphs (artwork) | Same | ✅ |
| Sidebar pane | Glass pane over content, gray selection pill with accent glyph | Same; hover/pressed fills were opaque and invisible in Dark | 🛠 translucent fills |
| Detail column | No stroke, no rounded corner | Fluent card stroke + 8 px top-left corner | 🛠 removed |
| Page header | Large title 34 pt bold in a 44 pt bar | Fluent 28 pt semibold, 36 pt | 🛠 |
| Pane toggle / back / More | 44 pt glass circles with SF glyphs | Stock Fluent 40 × 36 buttons and glyphs | ⚠ |
| Prominent (Done) action | One accent-filled trailing item | No style | ⚠ |

## Fixes

All in `src/library/Uno.Cupertino`, each guarded by a test in `Given_CupertinoHigReview` that failed before the
change (27 of the 28 first tests were red on the previous commit; the DatePicker one passed because Uno converts
the `Color` it was given to a brush, and is kept as a guard). Public keys were kept; five existing tests that
pinned the old look were updated, none removed.

| Area | Change |
| --- | --- |
| Separators | `ListViewItemSeparatorBrush`, `NavigationViewItemSeparatorForeground` and the gallery's dividers → `CupertinoSeparatorBrush` (label @ 29 %). The list's `ItemsPresenter` is pulled up one pixel so the last row's hairline falls outside the group. |
| Fills | List, menu and sidebar hover / pressed → `CupertinoQuaternarySystemFillBrush` / `CupertinoSystemFillBrush`; list selection → `CupertinoSystemFillBrush` (neutral, translucent, reads in Dark). |
| Menu | Group separator is an 8 px band of `CupertinoSecondarySystemFillBrush` (`CupertinoMenuFlyoutSeparatorHeight`); `CupertinoMenuFlyoutSeparatorMargin` is `0`. |
| Progress bar | `ProgressBarTrackHeight` / `ProgressBarMinHeight` 4, `ProgressBarTrackCornerRadius` / `ProgressBarCornerRadius` 2, track `CupertinoSystemFillBrush`, paused / error → `CupertinoYellowColor` / `CupertinoRedColor`. |
| Toggle switch | `CupertinoToggleDuration` 0.2 s: knob glides and the track cross-fades in both directions (eased out). |
| Checkbox | Unchecked outline `CupertinoTertiaryGrayBrush`, transparent unchecked fill; `CheckedPressed` / `IndeterminatePressed` keep the check on every platform; `IndeterminateDisabled` 0.5; secondary style on `SecondaryBrush` / `OnSecondaryBrush`. |
| Radio | Dark `RadioButtonBackgroundColor` transparent; dead `BorderBrush` setter and `MinWidth` 120 removed. |
| Slider | Ticks `CupertinoSliderActiveTrackBrush`; secondary style `SecondaryBrush`. |
| ComboBox | `CupertinoQuinaryGrayBrush` capsule (`CupertinoComboBoxCornerRadius` → 22), no border, accent 13 px chevron, `CupertinoSystemFillBrush` press; dead white-on-white overlays removed. |
| NumberBox | Header above a 44 pt frame (`CupertinoNumberBoxPadding` 7,0,7,0, `MinHeight` 44); header keeps the caption tracking bridge. |
| DatePicker | Disabled setters assign brushes; header collapses when null and uses `CupertinoFootnote`; wheel rows 20 pt. |
| CalendarDatePicker | Press dims the capsule, disabled dims the root; Fluent focus fill and selected-text flip removed. |
| CalendarView | Weekday letters 13 pt semibold `CupertinoSecondaryLabelBrush`; `IsTodayHighlighted` off (also on `CalendarDatePicker`) so today's number is drawn. |
| TextBox | Clear button 44, so the field no longer grows to 48 with text. |
| Alert | Message on `CaptionLarge` (13); title `MaxLines` removed. |
| NavigationView | Content grid stroke / corner keys 0 (also Minimal and Top); `CupertinoNavigationViewHeaderStyle` = `DisplayMedium` 34 bold in a 44 bar. |
| HyperlinkButton | Dead `HyperlinkUnderlineVisible` and duplicate `MinusGlyphPathStyle` removed. |
| Palette | Alphas `#4D` (0.30), `#2E` (0.18), `#29` (0.16 / 0.16 dark quaternary label), `#1F` (0.12), `#4A` (0.29), `#5C` (0.36), `#52` (0.32). |

Docs: `doc/cupertino-controls-styles.md` (list, menu, progress, switch, number box, combo box, calendar,
navigation, compatibility controls).

## Open items

Ordered by visual impact. None blocks the branch; each is a maintainer decision or an upstream fix.

1. **Uno does not paint the "today" day item when `DayItemCornerRadius` is non-zero** (Skia). Measured: the whole
   item, background included, disappears; radius 0 shows it; `TodayBackground`, `TodayForeground`,
   `TodayFontWeight`, `CalendarItemBorderThickness`, `CalendarItemBorderBrush = null` and
   `CalendarItemCornerRadius` change nothing; `IsTodayHighlighted = false` shows it. Repro: a `CalendarView` with
   `DayItemCornerRadius="20"` on Uno 7.0-dev. The theme turns the highlight off so the date stays readable; iOS
   tints today's number. File upstream, then restore `IsTodayHighlighted`.
2. **List rows are 44 pt; iOS 26 rows are 52 pt** (measured from the HIG artwork: two rows in a 208 px card at
   2×). `CupertinoRowMinHeight` consumes `ControlHeightMediumLarge` (44) and the token scale has no 52 step;
   a Cupertino-only override of that token would move every 44 pt control. Needs a token decision.
3. **Menus and popovers cast no shadow.** iOS menus float on a soft, wide shadow. The Thick preset's backplate
   shadow is clipped by the presenter's rounded `Border`, and a `ThemeShadow` on a container holding the
   backdrop-sampling canvas draws nothing (measured 2026-09-23). Needs a shadow drawn by the presenter itself
   or an Uno fix; also affects the ComboBox popup and the toolbar overflow.
4. **Menu rows have no hairline between items.** iOS draws 0.33 pt hairlines inside a group; the last row of a
   group has none, which a per-item template cannot know. Cheap only with an items-host behaviour.
5. **State opacities.** `OnSurfaceDisabledBrush` (0.12), `OnSurfaceMediumBrush` (0.64) and `OnSurfaceLowBrush`
   (0.32) are Material's; iOS is ≈ 0.30 / 0.60 / 0.30. They resolve `{StaticResource}` inside the shared
   palette, so the Cupertino palette cannot override them; the brushes would have to be redeclared.
6. **Hover and press idiom.** Whole-element opacity dims (0.85 / 0.6) stand in for iPadOS's pointer highlight
   fill and ≈ 0.75 press highlight; glass items should brighten and scale rather than dim.
7. **Button label weight** is 17 Medium everywhere (`LabelLargeFontWeight`); iOS uses Regular for plain / gray /
   tinted and Semibold for prominent. Changing the slot moves every button.
8. **NavigationView chrome still Fluent**: pane toggle (`E700` 40 × 36), back button (`E72B`), top "More"
   (text + chevron), 16 px icon box, `NavigationViewTopPaneHeight` 48. iOS: 44 pt glass circles with
   `sidebar.leading`, `chevron.backward`, `ellipsis.circle`, 20 pt symbols, 44 pt bar. No prominent (Done)
   `AppBarButton` style exists.
9. **Field headers** are 13 pt in the label colour; iOS section / field labels are the secondary label. The
   `ToggleSwitch` header is 15 pt where a row label is 17.
10. **Glass**: no adaptive light / dark flip from backdrop luminance, no edge refraction on Regular (K = 0),
    Regular / Thick presets are unmeasured guesses; `GlassMaterial` XML docs still describe Clear as a media
    overlay and Thin as "strong refraction". The knob lens was tuned on the Mac; everything else awaits a GPU.
11. **Smaller**: `PasswordBox` ignores `PlaceholderForeground`; `TextBox` disabled dims the frame but not the
    header; `Slider` `CornerRadius` is `not_win` only (square track ends on WinAppSDK); `CupertinoSliderStyle`
    has no header presenter; `RatingControl` unselected stars are filled; `PipsPager` pips dim on hover;
    unused keys `ComboBoxPopupMaxHeight`, `CupertinoCalendarDatePickerBackground`,
    `MaterialDatePickerHostPadding`, `ContentDialogSeparatorBrush`; the `HighContrast` dictionaries duplicate
    Light rather than Apple's increased-contrast table.

## Verification

- Cupertino desktop Debug: full suite green after the fixes (see progress.md for the count), including the 30
  new `Given_CupertinoHigReview` cases, 27 of which were red on `a6337afc`.
- Updated expectations: `Given_CupertinoContainers` (selected row fill), `Given_CupertinoControls` (progress
  track brush), `Given_CupertinoHigFields` (combo box radius), `Given_CupertinoHigSelection` (waits for the
  glide), `Given_CupertinoSemanticTypography` (alert body slot is `CaptionLarge`).
- XAML Styler (pinned) passes on all tracked XAML; C# whitespace passes; the published doc passes markdownlint.
- Captures inspected: gallery Light / Dark at 2×, menu, alert, calendar popup, wheel picker, toolbar overflow,
  navigation pane, before and after. Software renders; the GPU look of glass is unchanged by this pass.
