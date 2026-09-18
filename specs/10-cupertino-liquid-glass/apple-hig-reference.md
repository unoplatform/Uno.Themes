# Reference — Apple Human Interface Guidelines, iOS 26/27 and macOS 26/27 (Liquid Glass era)

Retained digest of Apple's published design guidance, gathered 2026-09-18. Every HIG page is JS-rendered; the
text was read from Apple's data endpoint (`developer.apple.com/tutorials/data/design/human-interface-guidelines/<page>.json`)
and the developer-documentation JSON. Tags: **[HIG]** Apple-published on the HIG, **[API]** from Apple developer
docs / WWDC transcripts, **[3P]** third-party measurement or reverse engineering — verify from screenshots or the
Figma 27 kits before relying on it.

Sources: https://developer.apple.com/design/human-interface-guidelines (materials, color, typography, layout,
motion, buttons, toggles, sliders, segmented-controls, text-fields, steppers, pickers, progress-indicators,
tab-bars, toolbars, sidebars, lists-and-tables, alerts, action-sheets, sheets, menus, context-menus, popovers,
search-fields, labels, sf-symbols) · https://developer.apple.com/documentation/technologyoverviews/liquid-glass ·
https://developer.apple.com/documentation/technologyoverviews/adopting-liquid-glass ·
https://developer.apple.com/documentation/swiftui/applying-liquid-glass-to-custom-views · WWDC25 sessions 219
"Meet Liquid Glass", 356 "Get to know the new design system", 323 "Build a SwiftUI app with the new design", 284,
310 · https://developer.apple.com/design/resources/ · https://developer.apple.com/fonts/

## 1. Liquid Glass

### 1.1 Two material families [HIG]

- **Liquid Glass** — "a distinct functional layer for controls and navigation… floats above the content
  layer." Standard components adopt it automatically.
- **Standard materials** (ultraThin / thin / regular (default) / thick; macOS `NSVisualEffectView.Material`) —
  "for visual differentiation within the content layer."

### 1.2 How the material behaves [API, WWDC 219]

| Aspect | Fact |
|---|---|
| Lensing | "the primary way Liquid Glass visually defines itself" — it "bends, shapes, and concentrates light in real time"; refraction is concentrated at the edges |
| Highlights | Specular highlights "respond to geometry"; on iPhone lighting "responds to device motion" |
| Shadow | Adaptive: shadow opacity increases over text, decreases over a solid light background |
| Tint / dynamic range | "The amount of tint and the dynamic range shift to always ensure buttons remain legible" |
| Light / dark flip | Small elements (bars, buttons) flip light↔dark from backdrop luminance, glyphs flip with them (monochrome). Large elements (menus, sidebars) adapt but do not flip; larger elements are more opaque |
| Size-dependent thickness | Larger glass "casts deeper, richer shadows, has more pronounced lensing and refraction effects, and a softer scattering of light" |
| Light spill | Colourful content nearby "can subtly spill onto its surface… reflects, scatters, and bleeds into the shadow" |
| Interaction | "illuminates from within… the glow spreads throughout the element and onto any Liquid Glass elements nearby"; `.interactive()` adds "scaling, bouncing, and shimmering". Touch = greater emphasis, trackpad = more subdued |
| Morphing | Controls morph between states; "a menu… the bubble simply pops open"; sheets can zoom out of their source button |
| Composition | "composed of a number of layers… each layer continuously adapts based on what's behind it" |

### 1.3 Variants [HIG, WWDC 219]

| | Regular | Clear |
|---|---|---|
| Behaviour | Blurs and adjusts backdrop luminosity; all adaptive effects; "works in any size, over any content" | "does not have adaptive behaviors… permanently more transparent" |
| Use when | Legibility risk, text-heavy (alerts, sidebars, popovers) — most system components | Over media (photos, video), when the content layer tolerates a dimming layer, and when content above is "bold and bright" |
| Dimming | n/a | Bright backdrop → dark dimming layer at **35 % opacity** |
| Rule | "They should never be mixed" | |
| SwiftUI | `Glass.regular` (default) | `Glass.clear`; `Glass.identity` = none |

### 1.4 Placement rules [HIG]

- Never in the content layer (table views, cards, backgrounds). Exception: slider and toggle **knobs become
  glass only during interaction**.
- Never glass on glass. Elements placed on glass use "fills, transparency, and vibrancy" as a thin overlay.
  Glass cannot sample other glass, so group siblings in one container.
- Avoid content / glass intersection in the steady state.
- Use sparingly; one or two prominent (tinted) buttons per view. Tint the **background**, not glyphs;
  "Refrain from adding color to the background of multiple controls."
- Remove custom backgrounds behind bars, sheets, popovers and toolbars.

### 1.5 Scroll edge effect [HIG, API]

Separates a bar from scrolling content — "gently dissolves the content into the background". `soft` (iOS
default) = blurred boundary; `hard` (macOS default, pinned headers, dense UIs) = linear, nearly opaque
boundary. One effect per view, never stacked; replaces solid bar backgrounds.

### 1.6 Accessibility fallbacks [WWDC 219]

| Setting | Effect |
|---|---|
| Reduce Transparency | "frostier and obscures more of the content behind it" |
| Increase Contrast | "predominantly black or white… with a contrasting border" |
| Reduce Motion | "decreases the intensity of some effects and disables any elastic properties" |
| Preferred look (iOS 26.1+) | System-wide Clear vs Tinted (more opaque) choice |

### 1.7 API surface (to map onto WinUI styles) [API]

| SwiftUI | UIKit | AppKit |
|---|---|---|
| `.glassEffect(.regular/.clear, in: shape = Capsule)` | `UIGlassEffect(style:)`, `.tintColor`, `.isInteractive` on `UIVisualEffectView` + `.cornerConfiguration` | `NSGlassEffectView` (`cornerRadius`, `tintColor`, `style`) |
| `Glass.tint(Color?)`, `.interactive(Bool)` | | `NSButton.BezelStyle.glass` + `bezelColor` |
| `GlassEffectContainer(spacing:)` — larger spacing merges shapes sooner (16 / 20 / 40 pt with 80 pt items) | `UIGlassContainerEffect().spacing` | `NSGlassEffectContainerView.spacing` |
| `.glassEffectID`, `.glassEffectUnion`, `.glassEffectTransition(.matchedGeometry / .materialize)` | | |
| `.buttonStyle(.glass)`, `.glassProminent` | `UIButton.Configuration.glass() / .prominentGlass() / .clearGlass() / .prominentClearGlass()` | `.glass` bezel + `tintProminence` (.none / .secondary / .primary) |
| `.scrollEdgeEffectStyle(.hard/.soft)` | `UIScrollEdgeEffect.Style` | `NSScrollEdgeEffectStyle` |
| `.backgroundExtensionEffect()` (mirror + blur under sidebar) | `UIBackgroundExtensionView` | `NSBackgroundExtensionView` |
| `.tabBarMinimizeBehavior(.onScrollDown/.onScrollUp/.never)`, `.tabViewBottomAccessory`, `Tab(role: .search)` | `tabBarMinimizeBehavior`, `UITabAccessory`, `UISearchTab` | |
| `ToolbarSpacer(.fixed/.flexible)`, `.sharedBackgroundVisibility(.hidden)` | `UIBarButtonItem.fixedSpace(0)` splits groups; `.style = .prominent` replaces `.done` | `NSToolbarItem.style = .prominent` |
| `.rect(corner: .containerConcentric)`, `ConcentricRectangle` | `UICornerConfiguration` (`.capsule(maximumRadius:)`, `.containerConcentric(minimum:)`, `.fixed`) | `NSButton.borderShape = .capsule / .roundedRectangle` |
| `.controlSize(.extraLarge)` | `UIButton.Configuration.Size.extraLarge` | `prefersCompactControlSizeMetrics` reverts to macOS 15 sizes |
| Opt-out | `UIDesignRequiresCompatibility` Info.plist key | |

### 1.8 Standard materials and vibrancy [HIG]

iOS: ultraThin / thin / regular / thick. Vibrant labels `label` → `secondaryLabel` → `tertiaryLabel` →
`quaternaryLabel` (avoid quaternary on thin materials); fills `fill`, `secondaryFill`, `tertiaryFill`; one
separator vibrancy. Thicker = better text contrast, thinner = more context. macOS uses purpose-named
materials (sidebar, menu, popover, titlebar, hudWindow, sheet, windowBackground, underWindowBackground,
contentBackground, fullScreenUI, toolTip, headerView, selection) blended `behindWindow` or `withinWindow`.
**Apple publishes no blur radii or tint values for any material.**

### 1.9 Third-party rendering recipes [3P]

- kube.io (https://kube.io/blog/liquid-glass-css-svg/): Snell's law with glass n ≈ 1.5; edge ("bezel")
  profile is a convex squircle `y = (1 - (1 - x)^4)^(1/4)`; the interior is flat, only the bezel refracts;
  normal from the numeric derivative; displacement encoded 8-bit `128 + v × 127`; specular = rim light with
  intensity ∝ normal · light direction; playground specular opacity 0.20–0.50, saturation 4–9; backdrop
  `blur + saturate`; chromatic aberration optional.
- 1ar.io (https://1ar.io/updates/how-liquid-glass-works): "first shrinks the interface, then stretches the
  edges outward"; parameters blur radius, opacity, refraction degree, mask colour.
- Others: https://blog.logrocket.com/how-create-liquid-glass-effects-css-and-svg/ ,
  https://github.com/deepika-builds/liquid-glass , https://liquid-glass.ybouane.com/ .

## 2. Colour

### 2.1 System colours — unified iOS / iPadOS / macOS table (HIG, updated 2025-06-09) [HIG]

| Name | Light | Dark | Increased contrast light | Increased contrast dark |
|---|---|---|---|---|
| Red | 255,56,60 `#FF383C` | 255,66,69 `#FF4245` | 233,21,45 | 255,97,101 |
| Orange | 255,141,40 `#FF8D28` | 255,146,48 `#FF9230` | 197,83,0 | 255,160,86 |
| Yellow | 255,204,0 `#FFCC00` | 255,214,0 `#FFD600` | 161,106,0 | 254,223,67 |
| Green | 52,199,89 `#34C759` | 48,209,88 `#30D158` | 0,137,50 | 74,217,104 |
| Mint | 0,200,179 `#00C8B3` | 0,218,195 `#00DAC3` | 0,133,117 | 84,223,203 |
| Teal | 0,195,208 `#00C3D0` | 0,210,224 `#00D2E0` | 0,129,152 | 59,221,236 |
| Cyan | 0,192,232 `#00C0E8` | 60,211,254 `#3CD3FE` | 0,126,174 | 109,217,255 |
| Blue | 0,136,255 `#0088FF` | 0,145,255 `#0091FF` | 30,110,244 | 92,184,255 |
| Indigo | 97,85,245 `#6155F5` | 109,124,255 `#6D7CFF` | 86,74,222 | 167,170,255 |
| Purple | 203,48,224 `#CB30E0` | 219,52,242 `#DB34F2` | 176,47,194 | 234,141,255 |
| Pink | 255,45,85 `#FF2D55` | 255,55,95 `#FF375F` | 231,18,77 | 255,138,196 |
| Brown | 172,127,94 `#AC7F5E` | 183,138,102 `#B78A66` | 149,109,81 | 219,166,121 |

Pre-2025 values differed (Blue `#007AFF` / `#0A84FF`, Red `#FF3B30` / `#FF453A`, Orange `#FF9500`). The
HIG no longer splits by platform; `NSColor.systemBlue` on macOS 26 is `#0088FF`. visionOS uses the dark values.

### 2.2 iOS / iPadOS grays [HIG]

| Name | Light | Dark | IC light | IC dark |
|---|---|---|---|---|
| systemGray | 142,142,147 `#8E8E93` | 142,142,147 `#8E8E93` | 108,108,112 | 174,174,178 |
| systemGray2 | 174,174,178 `#AEAEB2` | 99,99,102 `#636366` | 142,142,147 | 124,124,128 |
| systemGray3 | 199,199,204 `#C7C7CC` | 72,72,74 `#48484A` | 174,174,178 | 99,99,102 |
| systemGray4 | 209,209,214 `#D1D1D6` | 58,58,60 `#3A3A3C` | 184,184,188 | 81,81,84 |
| systemGray5 | 229,229,234 `#E5E5EA` | 44,44,46 `#2C2C2E` | 209,209,214 | 63,63,66 |
| systemGray6 | 242,242,247 `#F2F2F7` | 28,28,30 `#1C1C1E` | 229,229,234 | 36,36,38 |

### 2.3 iOS semantic colours [3P baseline iOS 13–18; HIG: "Avoid hard-coding… values may fluctuate"]

| Name | Light | Dark |
|---|---|---|
| label | `#000000` | `#FFFFFF` |
| secondaryLabel | 60,60,67 @ 0.60 `#993C3C43` | 235,235,245 @ 0.60 `#99EBEBF5` |
| tertiaryLabel | 60,60,67 @ 0.30 `#4D3C3C43` | 235,235,245 @ 0.30 `#4DEBEBF5` |
| quaternaryLabel | 60,60,67 @ 0.18 `#2E3C3C43` | 235,235,245 @ 0.16–0.18 `#2EEBEBF5` |
| placeholderText | 60,60,67 @ 0.30 | 235,235,245 @ 0.30 |
| separator | 60,60,67 @ 0.29 `#4A3C3C43` | 84,84,88 @ 0.60 `#99545458` |
| opaqueSeparator | `#C6C6C8` | `#38383A` |
| link | `#007AFF` (pre-26; expected to track Blue `#0088FF` / `#0091FF` — verify) | `#0984FF` |
| systemBackground | `#FFFFFF` | `#000000` |
| secondarySystemBackground | `#F2F2F7` | `#1C1C1E` |
| tertiarySystemBackground | `#FFFFFF` | `#2C2C2E` |
| systemGroupedBackground | `#F2F2F7` | `#000000` |
| secondarySystemGroupedBackground | `#FFFFFF` | `#1C1C1E` |
| tertiarySystemGroupedBackground | `#F2F2F7` | `#2C2C2E` |
| systemFill | 120,120,128 @ 0.20 `#33787880` | 120,120,128 @ 0.36 `#5C787880` |
| secondarySystemFill | 120,120,128 @ 0.16 `#29787880` | @ 0.32 `#52787880` |
| tertiarySystemFill | 118,118,128 @ 0.12 `#1F767680` | @ 0.24 `#3D767680` |
| quaternarySystemFill | 116,116,128 @ 0.08 `#14747480` | 118,118,128 @ 0.18 `#2E767680` |

Usage: the **system** set for plain views, the **grouped** set for grouped tables; primary = overall view,
secondary = grouping within it, tertiary = grouping within secondary.

### 2.4 macOS semantic colours [HIG names; alphas from a macOS 12 dump — recheck on 26]

labelColor black @ 0.847 / white @ 0.847 · secondaryLabel @ 0.498 / 0.549 · tertiaryLabel @ 0.259 / 0.247 ·
quaternaryLabel @ 0.098 · placeholderText @ 0.247 · separatorColor @ 0.098 · textBackground `#FFFFFF` /
`#1E1E1E` · windowBackground `#ECECEC` / `#323232` · controlBackground `#FFFFFF` / `#1E1E1E` ·
underPageBackground 150,150,150 @ 0.898 / `#282828` · selectedContentBackground `#0063E1` / `#0058D0` ·
unemphasizedSelectedContentBackground `#DCDCDC` / `#464646` · selectedTextBackground `#B3D7FF` / `#3F638B` ·
keyboardFocusIndicator 0,103,244 @ 0.498 / 26,169,255 @ 0.498 · linkColor `#0068DA` / `#419CFF` · gridColor
`#E6E6E6` / `#1A1A1A` · controlAccentColor = user accent (default blue).

### 2.5 Accent and tint guidance [HIG]

- macOS 11+: an app accent applies only when the user's system accent is multicolor.
- Liquid Glass "has no inherent color"; bars default to **monochrome** glyphs; the prominent (Done) button
  gets the accent as **background**. Ship light, dark and increased-contrast variants "even if your app
  ships in a single appearance mode, to support Liquid Glass adaptivity."
- The June 2025 palette was re-tuned "to work in harmony with Liquid Glass, improve hue differentiation".

## 3. Typography

### 3.1 Defaults and minimums [HIG]

| Platform | Default | Minimum |
|---|---|---|
| iOS, iPadOS | 17 pt | 11 pt |
| macOS | 13 pt | 10 pt |
| visionOS | 17 pt | 12 pt |

### 3.2 iOS / iPadOS text styles at Large (default) [HIG]

| Style | Weight | Size | Leading | Emphasized |
|---|---|---|---|---|
| Large Title | Regular | 34 | 41 | Bold |
| Title 1 | Regular | 28 | 34 | Bold |
| Title 2 | Regular | 22 | 28 | Bold |
| Title 3 | Regular | 20 | 25 | Semibold |
| Headline | Semibold | 17 | 22 | Semibold |
| Body | Regular | 17 | 22 | Semibold |
| Callout | Regular | 16 | 21 | Semibold |
| Subhead | Regular | 15 | 20 | Semibold |
| Footnote | Regular | 13 | 18 | Semibold |
| Caption 1 | Regular | 12 | 16 | Semibold |
| Caption 2 | Regular | 11 | 13 | Semibold |

Dynamic Type matrix (size/leading) [HIG]:

| Style | xS | S | M | L | xL | xxL | xxxL | AX1 | AX5 |
|---|---|---|---|---|---|---|---|---|---|
| Large Title | 31/38 | 32/39 | 33/40 | 34/41 | 36/43 | 38/46 | 40/48 | 44/52 | 60/70 |
| Title 1 | 25/31 | 26/32 | 27/33 | 28/34 | 30/37 | 32/39 | 34/41 | 38/46 | 58/68 |
| Title 2 | 19/24 | 20/25 | 21/26 | 22/28 | 24/30 | 26/32 | 28/34 | 34/41 | 56/66 |
| Title 3 | 17/22 | 18/23 | 19/24 | 20/25 | 22/28 | 24/30 | 26/32 | 31/38 | 55/65 |
| Headline / Body | 14/19 | 15/20 | 16/21 | 17/22 | 19/24 | 21/26 | 23/29 | 28/34 | 53/62 |
| Callout | 13/18 | 14/19 | 15/20 | 16/21 | 18/23 | 20/25 | 22/28 | 26/32 | 51/60 |
| Subhead | 12/16 | 13/18 | 14/19 | 15/20 | 17/22 | 19/24 | 21/28 | 25/31 | 49/58 |
| Footnote | 12/16 | 12/16 | 12/16 | 13/18 | 15/20 | 17/22 | 19/24 | 23/29 | 44/52 |
| Caption 1 | 11/13 | 11/13 | 11/13 | 12/16 | 14/19 | 16/21 | 18/23 | 22/28 | 43/51 |
| Caption 2 | 11/13 | 11/13 | 11/13 | 11/13 | 13/18 | 15/20 | 17/22 | 20/25 | 40/48 |

### 3.3 macOS text styles [HIG]

| Style | Weight | Size | Line height | Emphasized |
|---|---|---|---|---|
| Large Title | Regular | 26 | 32 | Bold |
| Title 1 | Regular | 22 | 26 | Bold |
| Title 2 | Regular | 17 | 22 | Bold |
| Title 3 | Regular | 15 | 20 | Semibold |
| Headline | Bold | 13 | 16 | Heavy |
| Body | Regular | 13 | 16 | Semibold |
| Callout | Regular | 12 | 15 | Semibold |
| Subheadline | Regular | 11 | 14 | Semibold |
| Footnote | Regular | 10 | 13 | Semibold |
| Caption 1 | Regular | 10 | 13 | Medium |
| Caption 2 | Medium | 10 | 13 | Semibold |

macOS has no Dynamic Type.

### 3.4 Fonts, tracking, licensing

- System font is **SF Pro** (variable, dynamic optical sizes; Text/Display switch historically at 20 pt).
  Avoid Ultralight / Thin / Light. "The system font dynamically adjusts tracking at every point size"; the
  tracking tables are no longer published on the HIG.
- iOS 26 (WWDC 356): typography "bolder", **left-aligned** (alerts, onboarding); list section headers are
  now **title case**.
- **SF license (EA1370/EA1371):** "solely for creating mock-ups of user interfaces to be used in software
  products running on Apple's iOS, OS X or tvOS". No embedding, no non-Apple OS, no redistribution.
  **Uno.Cupertino must not ship SF Pro.** Use the host SF on Apple targets and a bundled open fallback
  elsewhere — **Inter** is the closest widely used metric match and is already packaged in this repo
  (`Uno.Fonts.Inter`). SF Symbols are likewise system-provided images under the SDK license; do not
  redistribute glyph outlines.

## 4. Layout and shape [HIG, API, 3P]

| Item | Value |
|---|---|
| Minimum hit region | **44 × 44 pt** (iOS, iPadOS, macOS pointer); 60 × 60 pt visionOS [HIG] |
| Layout margins | The HIG Layout page (2026-09 revision) publishes no margin numbers; UIKit layout margins are 16 pt compact / 20 pt regular [3P, verify for 26] |
| Shape system | **Fixed** (constant radius), **Capsule** (radius = ½ height), **Concentric** (radius = parent radius − padding) [WWDC 356] |
| Concentric API | `ConcentricRectangle`, `.concentric(minimum:)`, `.fixed(r)`, container via `.containerShape(.rect(cornerRadius: 24))`; UIKit `.containerConcentric(minimum:)`, `.capsule(maximumRadius:)` [API] |
| Control shapes | iOS: **capsule default** for buttons; macOS: mini / small / medium → rounded rectangle, large / extraLarge → capsule; mirrored in sliders, switches, bars, grouped-table corners [WWDC 323/356/310] |
| Toolbar composition | Items grouped on shared glass; ≤ 3 groups; text-labelled actions separated by fixed space; one `.prominent` primary action, trailing [HIG] |
| macOS window radius | 16 pt title-bar-only, 26 pt toolbar windows (was 10) [3P] |
| iOS sheets | `preferredCornerRadius` default **40 pt** on iOS 26 (was 20) [3P] |
| List rows (iOS 26.5, on device) | min row height **52 pt** (was 44); cell margins 15 / 16 / 15 / 16 (was 11 / 20 / 11 / 20); "sections have an increased corner radius" (value unpublished) [3P + Apple "Adopting Liquid Glass"] |
| Spacing rhythm | Not published; Apple kits use 8 pt increments |

## 5. Components (iOS 26/27, macOS 26/27)

Legacy UIKit metrics **[legacy]** are the iOS 13–18 defaults still widely documented; iOS 26 changed several.

| Component | HIG guidance | Metrics | Liquid Glass behaviour |
|---|---|---|---|
| **Buttons** | Style, content, role (normal / primary / cancel / destructive). Primary = accent background; destructive = red. ≤ 1–2 prominent per view. Always a press state. macOS: push (default), square / gradient (icon), help (circular "?"), image | Sizes mini / small / medium / large / **extraLarge**; capsule on iOS. Hit ≥ 44 × 44. Legacy `UIButton.Configuration` heights ≈ mini 28 / small 28 / medium 34 / large 50 [legacy] | `.glass` (translucent, monochrome label), `.glassProminent` (accent-tinted); interactive scale / bounce / glow; buttons morph into menus |
| **Toggles / switch** | Switch only in list rows, default **green**; outside lists use toggle buttons. macOS: switch, checkbox (on / off / mixed), radio (2–5 items) | UISwitch **51 × 31 pt**, knob 27 pt [legacy, confirmed]; macOS checkbox / radio 14 pt [legacy] | Knob becomes glass while dragging |
| **Sliders** | Track fills leading → thumb; optional min / max icons; new `step`, `.ticks(at:)`, `neutralValue`, `.thumbless`, `tintProminence` | Legacy track 4 pt, thumb 28 pt white with shadow; iOS 26 track thicker (measure) | Thumb is glass inside a container; "refracts the track and merges with it near the ends" |
| **Segmented controls** | Equal widths; text **or** images; ≤ 5 segments on iPhone | Legacy height 32 pt, selected segment white with shadow, radius ≈ 8–9; iOS 26 "more rounded" | Selected indicator adopts glass knob |
| **Text fields** | Placeholder + separate label; clear button trailing; leading image = purpose | Legacy rounded-rect height 34 pt | Content layer: no glass |
| **Steppers** | Two-segment ±; pair with a text field | Legacy 94 × 32 pt | — |
| **Pickers / date pickers** | compact (button → popover), inline, wheels, automatic | Wheel row ≈ 32 pt [legacy] | Compact picker opens a glass popover |
| **Progress indicators** | Bar / circular determinate; spinner indeterminate; never switch between them | Activity indicator medium 20 pt, large 37 pt; progress bar 4 pt [legacy] | — |
| **Tab bars** | iOS: floats at bottom on glass; minimizes on scroll; **search tab** trailing as a separate button; iPadOS tab bar near top, convertible to sidebar; ≤ 5 tabs; badge = red oval | iOS 26 measured slab **62 pt** inside an 83 pt container [3P]; legacy 49 pt | Glass slab, light / dark flip, monochrome glyphs, selected pill glides |
| **Toolbars / navigation bars** | Merged page. Leading: back / sidebar + title; centre: customizable; trailing: persistent + inspector + search + More + primary (`.prominent`, one). ≤ 3 groups; title ≤ 15 chars; large title collapses on scroll | Legacy nav bar 44 pt, large-title bar 96 pt (44 + 52), toolbar 44 pt; large title 34 pt bold | Items grouped on shared glass; scroll edge effect replaces bar background |
| **Sidebars** | Leading; inset, floats on glass; ≤ 2 hierarchy levels; icons use accent | Row heights unpublished (macOS General setting) | More opaque than bars; does not flip light / dark |
| **Lists and tables** | plain / grouped / inset-grouped / sidebar; iOS 26: larger rows, increased section radius, title-case headers | Row min 52 pt, margins 15 / 16 / 15 / 16 (iOS 26.5, measured); legacy 44 pt; separator inset 16; legacy inset-grouped radius 10 → larger (measure) | Content layer: no glass |
| **Alerts** | Title + message + ≤ 3 buttons; default trailing / top, Cancel leading / bottom; text left-aligned on iOS 26 | Legacy width 270 pt, button row 44 pt, radius 13 → larger (measure) | Regular glass over a dimming layer |
| **Action sheets** | Cancel bottom, destructive top; iOS 26 **anchors to the source control** | — | Glass popover |
| **Sheets** | Detents large / medium / custom; grabber; Cancel leading / Done trailing | iOS 26 default corner radius 40 pt [3P] | Inset glass at partial height, opaque at full height; can zoom out of its source |
| **Menus** | Verb labels, title case, ellipsis for more input; icons leading, all-or-none per group; submenus one level | Legacy width 250 pt | Pops open from its button; thicker glass, deeper shadow |
| **Context menus** | ≤ 3 groups; destructive red at end; preview with matching clip | — | Glass |
| **Popovers** | Arrow at source; not in compact width (use a sheet) | — | Regular glass |
| **Search fields** | Icon + clear + placeholder; tab, toolbar or inline placements; field slides above keyboard | Legacy height 36 pt | Glass capsule; minimizes to a glass button |
| **Labels** | label / secondaryLabel / tertiaryLabel / quaternaryLabel | §2.3 | Vibrant automatically on glass |
| **Disclosure controls** | Triangle: collapsed points leading, expanded points down; ≤ 1 disclosure button per view | — | — |
| **Pull-down / pop-up buttons** | Pull-down = actions (chevron); pop-up = mutually exclusive options (up / down chevron) | macOS heights = push-button heights | Glass menu morphs from the button |
| **Rating indicators (macOS)** | Stars, no partials | — | — |
| **Windows (macOS)** | Title bar + toolbar + body; key window shows coloured traffic lights | Radius 16 / 26 pt, traffic lights 16 × 16 [3P] | Title bar / toolbar on glass; scroll edge effect under toolbar |

## 6. Motion [HIG, API]

The HIG publishes **no durations**. Rules: purposeful, optional (Reduce Motion), brief and precise, avoid
animating frequent interactions, cancellable. Liquid Glass: touch → greater emphasis, trackpad → more
subdued; Reduce Motion "disables any elastic properties."

| SwiftUI preset | Parameters |
|---|---|
| `.default` (iOS 17+) | spring response 0.55, dampingFraction 1.0 (pre-17: easeInOut 0.35 s) |
| `.smooth` | duration 0.5, bounce 0.0 |
| `.snappy` | duration 0.5, bounce 0.15 |
| `.bouncy` | duration 0.5, bounce 0.3 |
| `.spring()` | response 0.55, dampingFraction 0.825 [3P consensus] |
| `.interactiveSpring()` | response 0.15, dampingFraction 0.86, blendDuration 0.25 |
| Conversion | `Spring(duration: 0.5, bounce: 0.3)` → stiffness 157.9, damping 17.6; dampingRatio ≈ 1 − bounce |

Recommended WinUI mapping: default state transitions = critically damped ≈ 0.35–0.55 s (cubic ease-out
fallback); glass press / hover = interactive spring 0.15 s; morph / menus = `.smooth` / `.snappy`.

## 7. Design resources [HIG]

- UI kits: iOS & iPadOS **27** and macOS **27** (Figma, Sketch), released 2026-06-23 — "Updates to Liquid
  Glass, expanded component and state support, naming changes to better align with code, improved
  resizing, and the addition of Dark Mode for macOS." The Figma community files
  (https://www.figma.com/community/file/1651309003795292092/ios-and-ipados-27 ,
  https://www.figma.com/community/file/1651309434229735362/macos-27) return HTTP 403 to fetchers; open them
  in Figma to measure. Sketch: https://www.sketch.com/s/04c24d8b-38fb-4afb-8836-36617e022f02 (iOS),
  https://www.sketch.com/s/57153a31-3379-4737-8ac6-dbfd6525f052 (macOS).
- SF Symbols 7 / "27": 7000+ symbols, 9 weights, 3 scales, 4 rendering modes. Not redistributable.
- Fonts: SF Pro, SF Compact, SF Mono, New York — mock-up license only (§3.4).

## 8. Not published by Apple — measure from screenshots or the Figma 27 kits

1. Glass render parameters: blur radius, saturation boost, bezel width, refraction magnitude, specular rim
   width / angle / opacity, inner shadow, drop shadow, light / dark flip threshold, tint tone mapping,
   regular vs clear opacity, thickness scaling with size, press glow radius / duration / scale.
2. Standard material filter values (ultraThin / thin / regular / thick).
3. iOS 26 control heights for mini / small / medium / large / extraLarge buttons and their padding / font;
   post-26 switch / slider / segmented / stepper / text-field dimensions; disabled and pressed opacities.
4. macOS 26 control heights ("slightly taller"), rounded-rect radii, sidebar row heights, checkbox size.
5. Bars: exact tab-bar slab height and inset, item spacing, selected pill size; toolbar slab and capsule
   sizes; minimized tab bar; search-tab button size.
6. Radii: inset-grouped section radius (iOS 26), alert radius / width, popover, menu, macOS sheet.
7. Layout margins (16 / 20 pt) and readable width for 26.
8. Whether `link`, `separator`, fills and label alphas changed with the June 2025 palette; dark elevated
   backgrounds; macOS 26 semantic alphas.
9. SF Pro tracking tables at each point size.
10. Numeric weight deltas behind "bolder" typography; iOS 26 section-header spec.
11. Motion durations for glass morph, menu pop, tab-bar minimize, sheet zoom.
