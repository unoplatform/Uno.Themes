# Reference — Cupertino.Avalonia (jsuarezruiz)

Snapshot: `jsuarezruiz/Cupertino.Avalonia` @ `a87eee43` (main, 2026-09-18). Package `Cupertino.Avalonia`
0.1.0-preview (2026-09-10), `net8.0;net10.0`, pinned to Avalonia `[12.1.1]` + `Avalonia.Skia`. MIT.

Purpose of this file: retain the *reference values and structure* an Avalonia theme author arrived at for the
same target (iOS 26 / Liquid Glass) so we can cross-check our own numbers. We do **not** copy code; the
shader lineage (KaranocaVe/LiquidGlassAvaloniaUI refraction, whynotmake-it/flutter_liquid_glass lighting)
is MIT and is documented in `liquid-glass-rendering.md` as prior art.

## 1. Scope the repo claims

- "An iOS 26 design system for Avalonia: control themes, typography, colors, motion and icons, plus the
  controls iOS has and Avalonia does not." Explicitly targets Liquid Glass ("glass that blurs and refracts
  the content behind it").
- Not a UIKit/SwiftUI wrapper; no SF Symbols (ships its own vector icon set). Animation parity with native
  "has not been verified". Preview; API may change.
- Themed Avalonia controls: Button, RepeatButton, HyperlinkButton, ToggleButton, SplitButton,
  DropDownButton, TextBox, MaskedTextBox, NumericUpDown, ComboBox, AutoCompleteBox, CheckBox, RadioButton,
  ToggleSwitch, Slider, ListBox, TreeView, TabControl, TabStrip, Carousel, Calendar, CalendarDatePicker,
  DatePicker, TimePicker, ProgressBar, RefreshContainer, ToolTip, Expander, Menu, MenuFlyout, ContextMenu,
  Flyout, ScrollBar, GridSplitter, SplitView, ColorPicker, NotificationCard.
- Cupertino-only controls: `GlassSurface`, `CupertinoNavigationPage`/`NavigationBar`, `CupertinoSheet`,
  `Dialog`, `CupertinoSwipeView`, `Cupertino{Date,Time,DateTime}Picker`, `CupertinoCalendarView`,
  `CupertinoToolbar`, `CupertinoBadge`, `CupertinoListCell`/`FormRow`/`Section`, `CupertinoSearchView`,
  `CupertinoPageControl`, `CupertinoIcon`, `CupertinoActivityIndicator`, `CupertinoWheel`.
- Accessibility contract: app supplies `ReduceMotion`, `ReduceTransparency`, `TextScaleFactor` via a static
  `CupertinoAccessibility`; system settings are **not** read automatically. Reduce Transparency → opaque
  fill. 44 pt minimum touch target recommended.
- Platforms: desktop, iOS, Android, Browser. Glass needs Skia; "flat fill when backdrop sampling is
  unavailable".

## 2. Repo layout (what to mirror)

```
src/Cupertino.Avalonia/
  Animation/     CriticallyDampedEasing, UnderdampedSpringEasing, SwitchTravelEasing, MotionCurve,
                 MotionDurationExtension (zeroes every duration under ReduceMotion), CupertinoCrossFade
  Controls/      41 files: custom controls + attached "Interaction" behaviours
  Rendering/     LiquidGlassDrawOperation, LiquidGlassShader (SkSL), FormattedTextCache
  Themes/        CupertinoTheme.axaml(.cs) — palette + merged dictionaries; Typography.axaml; Icons.axaml
  Themes/Controls/  44 dictionaries, one control family each (incl. GlassSurface.axaml = material presets,
                    Knob.axaml = shared switch/slider knob)
  Typography/    AppleCoreText / AppleSystemTextShaper (Apple-only SF shaping)
tests/           ThemeIntegrityTests (Light/Dark key + type parity), RenderTests (glass at 1x and 2x)
```

Conventions worth borrowing (`Themes/README.md`): one control family per dictionary; implicit default +
named variants; `DynamicResource` for anything appearance-dependent, `StaticResource` for invariant
geometry; **Light and Dark dictionaries must have identical key sets and types, enforced by a test**;
localizable strings are plain resources.

## 3. Colour palette (`Themes/CupertinoTheme.axaml`, `#AARRGGBB`)

### System colours

| Key | Light | Dark |
|---|---|---|
| Blue (accent default) | `#FF0088FF` | `#FF0091FF` |
| Red | `#FFFF3B30` | `#FFFF453A` |
| Orange | `#FFFF9500` | `#FFFF9F0A` |
| Yellow | `#FFFFCC00` | `#FFFFD60A` |
| Green | `#FF34C759` | `#FF30D158` |
| Mint | `#FF00C7BE` | `#FF63E6E2` |
| Teal | `#FF30B0C7` | `#FF40C8E0` |
| Cyan | `#FF32ADE6` | `#FF64D2FF` |
| Indigo | `#FF5856D6` | `#FF5E5CE6` |
| Purple | `#FFAF52DE` | `#FFBF5AF2` |
| Pink | `#FFFF2D55` | `#FFFF375F` |
| Brown | `#FFA2845E` | `#FFAC8E68` |
| Gray | `#8E8E93` | `#8E8E93` |

Note: blue is the iOS 26 revised `#0088FF` / `#0091FF`, not the classic `#007AFF` / `#0A84FF`. Only one
gray; no `systemGray2-6` scale.

### Labels, separators, placeholders

| Key | Light | Dark |
|---|---|---|
| Label | `#FF000000` | `#FFFFFFFF` |
| SecondaryLabel | `#993C3C43` (60 %) | `#99EBEBF5` |
| TertiaryLabel | `#4D3C3C43` (30 %) | `#4DEBEBF5` |
| LabelOnGlass | `#FFFFFFFF` | `#FFFFFFFF` |
| Separator | `#5C3C3C43` (36 %) | `#5C545458` |
| Placeholder | `#FFC5C5C7` | `#FF5C5C60` |
| BarForeground | `#FF222222` | `#FFF4F4F5` |

### Backgrounds and fills

| Key | Light | Dark |
|---|---|---|
| GroupedBackground (page) | `#F2F2F7` | `#FF000000` |
| Card (grouped rows) | `#FFFFFFFF` | `#FF1C1C1E` |
| ReadOnlyField | `#FFF2F2F7` | `#FF242426` |
| FieldBorder / PointerOver | `#FFCCCCCC` / `#FFA9A9AE` | `#FF48484A` / `#FF6C6C70` |
| SegmentTrack (= tertiaryFill) | `#1F787880` (12 %) | `#3D747482` (24 %) |
| StepperTrack | `#1F787880` | `#2C787880` |
| PickerField | `#1E767680` | `#2E767680` |
| SearchField | `#04767680` | `#3D767680` |
| BorderedFill (`.bordered` button) | `#8CA2A2A2` | `#6FAFAFAF` |
| SegmentSelected (pill) | `#FFFFFFFF` | `#FF69696F` |
| RowPointerOver / RowPressed | `#14000000` / `#FFD1D1D6` | `#14FFFFFF` / `#FF2C2C2E` |
| MenuRowPointerOver / Pressed | `#0D000000` / `#1F000000` | `#14FFFFFF` / `#26FFFFFF` |
| MenuPanel (tooltip, notification) | `#F5F7F7FA` | `#F52C2C2E` |
| ProgressTrack | `#FFE4E4E5` | `#FF3A3A3C` |
| ScrollIndicator / Pressed | `#59000000` / `#8C000000` | `#59FFFFFF` / `#8CFFFFFF` |
| DialogAction / Over / Pressed | `#26787880` / `#3D787880` / `#59787880` | `#3DFFFFFF` / `#52FFFFFF` / `#70FFFFFF` |
| SheetGrabber | `#4D3C3C43` | `#5BEBEBF5` |
| ContextMenuScrim | `#30000015` | `#1A302858` |

### Accent family (regenerated from one `Accent` colour, preserving each role's alpha)

| Key | Light | Dark | Rule |
|---|---|---|---|
| Accent | `#FF0088FF` | `#FF0091FF` | alpha FF |
| FocusRing | `#990088FF` | `#990091FF` | alpha 99 |
| Selection | `#4D0088FF` | `#4D0091FF` | alpha 4D |
| AccentSubtle | `#1F0088FF` | `#2E0091FF` | alpha 1F / 2E |
| ProminentTint (glass) | `#FF0088FF` | `#FF0091FF` | alpha FF |
| ProminentPressedTint | `#FF4BB7FF` | `#FF3A9BFF` | fixed |
| ProminentRim | `#7744FFFF` | `#7700FFFF` | custom accent → `#77FFFFFF` |
| TabAccent | light: accent × 0.85 RGB | accent | |

### Switch / knob / slider

| Key | Light | Dark |
|---|---|---|
| SwitchTrackOff | `#FFC3C3C5` | `#52787880` |
| SwitchTrackOn | `#FF34C759` | `#FF30D158` |
| Knob / KnobPressed | `#FFFFFFFF` / `#C9FFFFFF` | same |
| SliderRemainder | `#24505050` | `#1CFAFAFA` |
| SliderThumbDragBase | `#FFF7F7F7` | `#FF2C2C2E` |

### Material tints (alpha = tint strength on glass)

| Key | Light | Dark |
|---|---|---|
| GlassButtonTint | `#8CF2F2F5` | `#8CFAFAFC` |
| BarButtonTint | `#C2FFFFFF` | `#C21A1A1C` |
| NavigationBarTint | `#FFF2F2F7` | `#FF000000` |
| TabBarTint | `#B8FFFFFF` | `#8C242428` |
| TabPill / TabPillColor / TabPillBorder | `#8AFFFFFF` / `#0AFFFFFF` / `#40FFFFFF` | `#59FFFFFF` / `#1FFFFFFF` / `#33FFFFFF` |
| TabLensTint / SegmentLensTint | `#12000000` / `#0D000000` | `#17FFFFFF` / `#17000000` |
| SheetTint | `#E6F6F6F8` | `#E61C1C1E` |
| ThickMaterialTint (dialog) | `#C9FAFAFC` | `#D11C1C1E` |
| PopoverTint (menus) | `#C2FAFAFC` | `#C22C2C2E` |
| ContextMenuTint | `#D1F9F9FF` | `#C21C1C1E` |

## 4. Typography

- **No font shipped.** `ResolveSystemFont()` probes `.AppleSystemUIFont`, `SF Pro Text`, `SF Pro Display`,
  `SF Pro`, `Helvetica Neue`, `Segoe UI`, `Roboto`, `Inter`, else default — with a probe for a
  nonexistent family so fallback faces are rejected. Gallery bundles Inter for Browser.
- Size tokens `CupertinoFontSize{10,11,12,13,14,15,16,17,18,20,22,24,28,34,35}` × `TextScaleFactor`
  (0.8-2.35).

| Class | Size | Line height | Weight |
|---|---|---|---|
| largetitle | 34 | 41 | Regular (Bold in nav bar) |
| title1 | 28 | 34 | Regular |
| title2 | 22 | 28 | Regular |
| title3 | 20 | 25 | Regular |
| headline | 17 | 22 | SemiBold |
| body | 17 | 22 | Regular |
| callout | 16 | 21 | Regular |
| subheadline | 15 | 20 | Regular |
| footnote | 13 | 18 | Regular |
| caption1 | 12 | 16 | Regular |
| caption2 | 11 | 13 | Regular |

Control weights: Button **Medium** (`.prominent`/`.plain` → Normal), segmented selected Medium, bottom tab
label 10 pt Medium → SemiBold selected, inline nav title 17 SemiBold, large title 34 Bold, dialog title
18 Bold, notification title 15 SemiBold.

## 5. Control metrics

Common: focus ring = 2 px template `Border` in `FocusRing` colour with negative margin, shown on
keyboard focus only; capsules use `CornerRadius="999"`; hairlines 0.5 (separators) or 0.333 / 0.667
(borders / rims); every duration routed through a reduce-motion-aware extension.

Pressed / disabled opacity ladder: pressed 0.4 (hyperlink, icon buttons), 0.5 (ComboBox), 0.55 (picker
capsule), 0.6 (bordered button, segment), 0.7 (CheckBox / RadioButton), 0.75 (plain button); disabled 0.5
(Button, Switch, Slider, Check, Radio, Progress), 0.4 (most others), 0.35 (toolbar / stepper).

### Button

| | Default (glass) | `.prominent` | `.bordered` | `.plain` | `.small` | `.large` |
|---|---|---|---|---|---|---|
| MinHeight | 36 | 36 | 36 | 36 | 28 | 50 |
| Padding | 14,7 | 14,7 | 14,7 | 14,7 | 11,4 | 20,12 |
| Font | 17 Medium | 17 Normal, white | 17 Medium, Accent | 17 Normal, Accent | 15 | 17 |
| Background | glass button surface | glass + ProminentTint + 0.667 rim | flat BorderedFill | none | | |
| Pressed | scale 1.04, light ×1.4 | scale 1.16/1.22, tint → pressed | opacity 0.6 | opacity 0.75 | | |

Transitions: RenderTransform 130 ms SineEaseOut, Opacity 100 ms, tint 130 ms. ToggleButton checked →
prominent tint, SemiBold. HyperlinkButton hover 0.7, pressed 0.4.

### ToggleSwitch

| Element | Value |
|---|---|
| Track | 63×28, CR 14; colour lerps Off→On from knob position; hover mixes 6 % toward white |
| Knob (rest) | 37×24 pill, CR 12, white; travel 22 |
| Travel | 334 ms critically damped (omega 21 /s) |
| Pressed / dragging knob | scale 1.3 / 1.45; 110 ms SineEaseOut in, 150 ms Linear out |
| Held knob glass | Blur 1, Thickness 7, Refraction 8, Chroma 0.35, Depth 0.30, Light 0.2, Fresnel 0.55 |

The chromatic rim on the held knob is two clipped `Border` halves with 0.667 gradient strokes, not shader
work.

### Slider

Rail 6 thick, CR 2.85, Accent fill / remainder brush, MinHeight 28. Thumb 37×24 pill CR 12, rest shadow
`0 3 12 #24000000`. Hover scale 1.04 (110 ms). Active scale 1.5 / 1.55 with drag shadow
`0 16.5 10 #24002F50` and a glass knob. Disabled 0.5.

### CheckBox / RadioButton

22×22 circle; ring 1.6 TertiaryLabel (hover SecondaryLabel); checked fill Accent (120 ms), 16×16 check
stroke 2 or 8×8 dot in LabelOnGlass; label margin 10; MinHeight 28; font 17; pressed 0.7; disabled 0.5.

### TextBox

| | Default | `.plain` | `.search` | `.glass` |
|---|---|---|---|---|
| MinHeight | 34 | 51 | 47 | 47 |
| CornerRadius | 6 | — | 23.5 | 23.5 |
| Border | 0.333 FieldBorder; hover PointerOver; focus Accent 1; error Danger 1; 150 ms | 0 | 0 | 0 |
| Padding | 10,0 | 0 | 14,0,16,0 | 14,0 |
| Background | Card (ReadOnlyField when read-only) | transparent | SearchField + shadow | glass Blur 14 / Thickness 7 / Refraction 8 |

Font 17; caret Accent; clear button 17 disc in a 30×47 hit target.

### ComboBox

Value style: MinHeight 34, font 17 Accent, right-aligned, up/down chevron 16×18 stroke 1.9; pressed 0.5;
disabled 0.4. Popup panel CR 14 popover glass, MinWidth 220, shadow 0.20/30/10. Item MinHeight 44,
Padding 16,0, tick 16 stroke 2 Accent; hover MenuRowPointerOver, pressed MenuRowPressed (180 ms).

### ListBox

Item MinHeight 44 (52 with icons), Padding 16,0; separator 0.5 inset 16 (59 with icon), hidden on last;
first / last CR 10; `.inset`: Card fill, CR 10, margin 16,0,16,24.

### Tabs

`.segmented`: track Height 32 CR 16 SegmentTrack; pill margin 2 CR 14 SegmentSelected, 150 ms; item font
13 → Medium selected, Padding 12,0; travel 180 ms with a glass "travel lens" that swells with hop count.

`.bottom` (iOS 26 floating tab bar): capsule Height 62 CR 31, MaxWidth 560, margin 21,0,21,26; item Width
86, font 10 Medium → SemiBold + TabAccent; resting lens CR 26.5; travel lens Magnification 1.18. A 32×32
backdrop luminance sample flags dark when Rec.709 luma < 0.34.

### ProgressBar / activity indicator

Bar 4 high, CR 2, MinWidth 80, ProgressTrack / Accent, 250 ms SineEaseOut. Indeterminate → 20 px spinner:
8 spokes, 100 ms step (0.8 s cycle), inner radius 0.31, stroke 0.133 × size round caps, spoke opacity
`max(0.35, 1 - i × 0.185)`, SecondaryLabel colour.

### Navigation bar / toolbar

Bar 113 large-title, 61 inline, 54 embedded. Backdrop glass 110 high, Blur 7, Saturation 1.15, no
refraction, opacity mask 1→0 from 78 % to 100 %. Toolbar row 44, margin 16,17,16,0; inline title 17
SemiBold; large title 34 Bold. Bar button 44×44, Padding 12,0, CR 22 glass capsule, pressed scale 0.96.
Toolbar MinHeight 48, buttons 48×48, pressed 0.4, disabled 0.35.

### Dialog / Sheet / Menu / ToolTip / Notification

| Control | Values |
|---|---|
| Dialog | Width 320, CR 30; frozen backdrop, Blur 30, Saturation 4.2, Thickness 3, Refraction 3; content margin 22,20,22,18; title 18 Bold, message 15; action Height 46 CR 23 font 17 Medium; preferred = Accent fill white SemiBold; stacked spacing 8 |
| Sheet | CR 36, shadow `0 6 32 #14000000`; Blur 30 Saturation 1.3 no refraction; grabber 36×5 CR 2.5 top 5 |
| Menu | item MinHeight 44 Padding 16,0 font 17; icon slot 26 + 10; popup CR 26 Blur 36 PopoverTint MinWidth 220; separator 0.5; open 180 ms QuadraticEaseOut opacity 0→1 + scale 0.94→1 from top-centre |
| ToolTip | MenuPanel fill, font 13, Padding 10,6, CR 8, MaxWidth 320, shadow `0 4 14 #26000000`, fade 120 ms |
| Notification | CR 18, MaxWidth 350, shadows `0 10 30 #33000000` + `0 2 6 #1F000000`, title 15 SemiBold, body 13; enter 250 ms slide -16, exit 120 ms |
| ScrollBar | track 7, thumb 3 (min 36) CR 2.5, hover scaleX 1.6 150 ms, auto-hide 250 ms |
| Expander | header MinHeight 52, chevron rotate 90° 250 ms, content slide -8→0 250 ms |
| Stepper | capsule 94×32 CR 16, buttons 47×32, divider 1×24, Delay 500 / Interval 80 |
| Picker capsule | MinHeight 34 Padding 13,0 CR 17 PickerField, pressed 0.55 |
| List cell | MinHeight 44, Padding 16,0, bottom 0.5; title 17 / subtitle 15 / detail 17 secondary; disclosure 8×14 stroke 2 |
| Section | header / footer 13 secondary margin 32,0,32,7, card CR 10 margin 16,0, bottom 24 |
| Badge | 18 high MinWidth 18 CR 9 Padding 5,0 font 13 Red; dot 10 |

### Motion

Durations used: 70, 90, 100, 110, 120, 130, 150, 180, 200, 250, 334 ms. Easings: SineEaseOut (press
scale), QuadraticEaseOut/In (popovers, expanders, notifications), critically damped `(1 - (1 + x)e^-x)`
(omega 7 default, 8.4 for navigation / dialog / tab), underdamped spring (zeta 0.8, omega 12, "UIKit
presentation springs"), switch travel (omega 21 × 0.334 s).

## 6. Liquid Glass implementation (prior art, MIT)

`GlassSurface : Decorator` emits a custom draw op that leases the Skia canvas, **snapshots the backdrop**,
and runs an SkSL `SKRuntimeEffect`:

1. `surface.Snapshot(crop)` of the padded device rect (padding = 3 sigma + 1.35 × refraction + shadow
   blur × 2.8). Hard cap 8192² / 16 M px.
2. Offscreen pass: two-layer shadow (contact + ambient, clipped outside the shape), then blur
   (`SKImageFilter.CreateBlur`, sigma = BlurRadius × scale × 0.5) + saturation colour matrix. Filters cached
   by quantised key.
3. Shader pass clipped to the rounded rect:

```glsl
// refraction core (rounded-rect SDF `sd`, thickness `h`)
float band = 1.0 - clamp(-min(sd, 0.0) / h, 0.0, 1.0);   // 0 interior -> 1 edge
float d = circleMap(band) * uRefraction;                   // 1 - sqrt(1 - x^2)
float2 lensDir = normalize(grad + uDepth * radial);
float2 magnified = halfSize + c / max(uMagnify, 0.001);
float2 refracted = magnified + d * lensDir;
// chroma: 3 taps at refracted +/- d*lensDir*uChroma*(0.35 + 0.9*band^2)
// lighting: total = ndl + 0.55*opp; brightness = total*sqrt(total)*intensity*edge; b/(1+b)
// rim: smoothstep(0.72, 1.0, band) * uFresnel * rimScale * 0.25
```

Adaptive mode reads backdrop luminance (1×1 luma texture per frame or 9 in-shader taps); luma drives a
white / black veil (`0.100 - 0.110·luma`, `0.090·(luma - 0.55)`) and rim / specular scaling.

**Fallback ladder:** no Skia lease → solid tint (alpha + 60/255); skew / perspective transform → flat;
shader compile failure → blurred image + tint; ReduceTransparency → tint over Card fill + drop shadow.

**Repaint model:** one pulse coordinator per window arms a 350 ms "pulse" on pointer press / release /
wheel / layout (not pointer move), then loops `RequestAnimationFrame`. `IsLive` = continuous;
`IsBackdropFrozen` = snapshot once (Dialog). Popups are forced into the overlay layer so they can sample
window pixels.

**Material presets**

| Preset | Blur | Thick | Refr | Chroma | Depth | Sat | Light | Fresnel | Shadow op/blur/off | Magnify | Adaptive |
|---|---|---|---|---|---|---|---|---|---|---|---|
| Defaults | 20 | 16 | 24 | 0.5 | 0.15 | 1.5 | 1.2 | 1.0 | 0.18/14/4 | 1 | yes |
| Glass button | 12 | 1 | 7 | 0.2 | 0.25 | 0.65 | 0.15 | 0.30 | 0.1/24/2 | 1 | yes |
| Prominent button | 12 | 1 | 7 | 0.05 | 0.12 | 1.0 | 0.15 | 0.18 | 0.1/24/1 | 1 | no |
| Popover / menu | 28-36 | 0.35 | 0.10 | 0 | 0.25 | 1.8 | 1.2 | 1.0 | 0.14/18/6 | 1 | yes |
| Bar capsule | 18 | 1 | 0 | 0 | 0 | 1 | 0.25 | 0 | 0.10/24/2 | 1 | yes |
| Bottom tab bar | 18 | 1 | 8 | 0.5 | 0.15 | 1.8 | 0.25 | 0.25 | 0.1/24/3 | 1 | yes |
| Tab travel lens | 0 | 9 | 30 | 1.1 | 0.18 | 1.25 | 0.35 | 0.5 | 0.07/14/3 | 1.18 | no |
| Navigation bar | 7 | 0.1 | 0 | 0 | 0 | 1.15 | 1.2 | 1.0 | 0 | 1 | yes |
| Dialog | 30 | 3 | 3 | 0 | 0.15 | 4.2 | 0 | 0.5 | 0.14/22/5 | 1 | no |
| Sheet | 30 | 0 | 0 | 0 | 0 | 1.3 | 0 | 0 | 0 | 1 | no |

## 7. C# surface (for comparison)

- `CupertinoTheme : Styles` with `Accent : Color?` that rewrites the accent roles preserving alpha.
- Static `CupertinoAccessibility` (`ReduceTransparency`, `ReduceMotion`, `TextScaleFactor`, `Changed`).
- Platform detection only for the Apple font shaper; everything else is capability-based (Skia lease,
  GPU context, shader compile result).
- Attached "Interaction" behaviours per control (switch colour lerp, slider active class, tab travel).
- `CupertinoHaptics.Handler : Action<HapticFeedback>` — in this repo that would have to be
  `EventHandler<T>` (AGENTS.md §10.bis).
- Tests: Light/Dark key parity, render tests for glass at 1× and 2×.

## 8. Related projects by render approach

| Repo | Platform | Approach |
|---|---|---|
| `KaranocaVe/LiquidGlassAvaloniaUI` | Avalonia + SkiaSharp | SkSL: vibrancy → blur → rounded-rect SDF lens refraction + dispersion → edge highlight. Source of Cupertino.Avalonia's refraction. |
| `whynotmake-it/flutter_liquid_glass` | Flutter | Fragment shader; source of the lighting model. |
| `ChaosJulien/LiquidGlassLab` | Avalonia + SkiaSharp | One shared backdrop texture per window via GrContext, triple-buffered; CPU RenderTargetBitmap fallback. |
| `luckyelysia/LiquidGlassWinUI` | WinUI 3 | HLSL in the DWM graph via an IAT hook (not portable); good parameter reference (thickness 20, refraction 1.4, dispersion 7, fresnel 30/20/20, glare 30/20/90 at -45°, superellipse roundness 5). |
| `pratikone/liquid-glass-WinUI` | WinUI 3 + Win2D | Blur → Turbulence → DisplacementMap → Tint → Blend with radial edge mask. |
| `danardelean/Maui.NativeStyles` | .NET MAUI | Passes native `UIGlassEffect` through on iOS 26; useful metric cross-check: 44 pt buttons, 52 pt rows with 26 pt continuous corners, 60 pt glass search capsule, 20 pt margin, inset grouped card 26 pt, `systemBlue = #0088FF`. |

## 9. Take-aways for Uno.Cupertino

- Palette and type tables map 1:1 onto our `SharedColorPalette.xaml` / `SharedTypography.xaml` pattern.
  Divergences from classic iOS values to adopt: iOS 26 blue (`#0088FF` / `#0091FF`), 0.36-alpha
  separators, 63×28 switch with a 37×24 pill knob, capsule buttons at 36 / 28 / 50 heights.
- The ReduceTransparency path (tint over Card fill + drop shadow) is the correct **first rung** for our
  port: every glass surface degrades to it and it needs no compositor access.
- A single reduce-motion-aware duration seam (resource-driven `Duration`s a theme switch can zero) is
  the equivalent of their markup extension.
