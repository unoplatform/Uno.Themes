# Cupertino HIG visual polish

Requested 2026-09-22 after the user found the implemented toggles did not match Apple's HIG.

## Acceptance and scope

Audit every current Cupertino control and shared visual foundation against the current Apple HIG.
The user confirmed **iOS/iPadOS throughout** on 2026-09-22. Distinguish macOS-only controls and
guidance rather than mixing platform appearances. This covers the current theme, not new implementations
of every platform feature in Apple's website (for example spatial input, CarPlay or app architecture).
Old "polish deferred" entries do not establish visual acceptance and are reopened by this request.

Use official guidance and actual reference artwork; label measured implementation choices separately
from published requirements. Preserve public keys, functional template parts, accessibility and live
color overrides. Do not treat semantic resolution or passing behavior tests as visual proof.

## Plan

- [x] Refresh current HIG evidence and inventory every existing control; record gaps by family.
- [x] Selection controls: switches, sliders, toggle buttons, checkbox/radio, progress and ratings.
- [x] Fields and pickers: text/password, number stepper, combo and calendar/date controls.
- [x] Navigation and containers: navigation, command bars, lists, alerts, menus/popovers and page dots.
- [x] Foundations and buttons: typography, palette, shape, dimensions, materials and semantic variants.
- [x] Add red reproductions before bug fixes; preserve all existing test coverage.
- [x] Inspect rendered Light/Dark state comparisons and document what the renderer cannot reproduce.
- [x] Build/test Cupertino Debug/Release desktop, build WASM, verify Material/Simple regressions.
- [x] Run formatters/doc checks, review the final diff, and update handoff with exact remaining gaps.

## Verification

Builds run in the main lane to avoid collisions in shared generated output. Tests use the existing
Cupertino runtime host and the repository runtime-test skill. Visual captures are required for shape
and layout changes. No new dependencies or target-framework changes are planned.

## Findings and review

The current HIG text is available from Apple's official `/tutorials/data/design/human-interface-guidelines/<slug>.json` endpoint. Its relative artwork URLs require the **`/tutorials` prefix**: `https://developer.apple.com/tutorials/images/com.apple.HIG/...`. The earlier attempt to load `/images/...` directly produced 404s and missed the visual evidence.

| Current styles / foundation | Apple reference | Audit / action |
| --- | --- | --- |
| ToggleSwitch | [Toggles](https://developer.apple.com/design/human-interface-guidelines/toggles), `toggles-ios-default-color@2x.png` | Current artwork measures 64×28 track, 40×24 capsule thumb at 2x; replace stale 51×31/circular thumb |
| Slider | [Sliders](https://developer.apple.com/design/human-interface-guidelines/sliders), HIG hero | Elongated capsule thumb; 40×24 is our consistent sizing choice, not an Apple numeric requirement |
| Active switch/slider thumb | [Materials](https://developer.apple.com/design/human-interface-guidelines/materials) | Transient glass during interaction; remove rendering work at rest |
| TextBox / PasswordBox | [Text fields](https://developer.apple.com/design/human-interface-guidelines/text-fields) | Persistent headers, readable body text, usable clear/reveal targets |
| NumberBox | [Steppers](https://developer.apple.com/design/human-interface-guidelines/steppers) | Legible value/header and paired touch targets; retain iOS plus/minus |
| ComboBox / date / calendar | [Pickers](https://developer.apple.com/design/human-interface-guidelines/pickers), [Pop-up buttons](https://developer.apple.com/design/human-interface-guidelines/pop-up-buttons) | Neutral compact capsules, larger date text, wheel selection band and popover glass |
| NavigationView | [Sidebars](https://developer.apple.com/design/human-interface-guidelines/sidebars) | Translucent Regular surface over content; retain stock navigation behavior |
| CommandBar / AppBarButton | [Toolbars](https://developer.apple.com/design/human-interface-guidelines/toolbars) | Floating rounded action group, glass overflow; preserve shortcut and submenu behavior |
| ContentDialog | [Alerts](https://developer.apple.com/design/human-interface-guidelines/alerts), `alert-ios@2x.png` | Left title/message, separated filled capsule actions; adaptive horizontal/stacked actions |
| MenuFlyout family | [Menus](https://developer.apple.com/design/human-interface-guidelines/menus), `small-medium-large-menu-layouts@2x.png` | iOS action symbols trail labels; selection check marks remain leading |
| Flyout | [Popovers](https://developer.apple.com/design/human-interface-guidelines/popovers) | Existing rounded glass surface retained; inspect against surrounding controls |
| ListView / ListViewItem | [Lists and tables](https://developer.apple.com/design/human-interface-guidelines/lists-and-tables) | Keep content solid, grouped corners and separators; inspect current rows in context |
| Button family | [Buttons](https://developer.apple.com/design/human-interface-guidelines/buttons) | Existing plain/tinted/prominent/glass hierarchy retained; fix short-label hit regions below 44 points |
| ToggleButton | Toggles | Current alternate background is a documented toggle idiom; preserve text/icon variants |
| HyperlinkButton / TextBlock / typography | [Typography](https://developer.apple.com/design/human-interface-guidelines/typography) | Retain semantic scale and inherited text; Inter remains licensed cross-platform substitute, not SF |
| Palette / spacing / shapes | [Color](https://developer.apple.com/design/human-interface-guidelines/color), [Layout](https://developer.apple.com/design/human-interface-guidelines/layout) | Retain current Apple palette/live roles; measure artwork and use real capsule radii |
| ProgressBar / ProgressRing | [Progress indicators](https://developer.apple.com/design/human-interface-guidelines/progress-indicators) | Content indicators remain solid; retain determinate track/spinner |
| PipsPager | [Page controls](https://developer.apple.com/design/human-interface-guidelines/page-controls) | Minimal dot style is supported; 7/16 sizing is a theme choice; stock control is not native continuous scrubbing |
| CheckBox / RadioButton / RatingControl / ToolTip | Toggles / platform considerations | Compatibility controls without direct native iOS equivalence; retain recognizable behavior, do not invent parity |
| GlassPanel / motion | Materials, [Motion](https://developer.apple.com/design/human-interface-guidelines/motion) | Keep fallback and lifecycle contract; software captures cannot certify Apple's adaptive optical rendering |

Twenty-one initial cases failed before their corresponding fixes (selection/button 8, fields/container 13); an additional menu-placement case failed separately. Capture and full-suite results follow once integrated.

The first integrated build passed 33 targeted cases and 159 permanent Cupertino cases. Light/Dark
captures exposed missing ComboBox headers/disclosure, a disabled-looking selected value, repeated
DatePicker labels, crowded menu check marks, and indistinct dark alert secondary actions. Independent
review found long alert label overflow and lost DatePicker presenter border bindings. Ten additional
cases reproduced these issues plus undersized toggle/checkbox/radio touch targets before their fixes.
Close-default, hidden-default, RTL and live Light/Dark bar-tint cases already passed as non-regression
guards. Final verification below supersedes these intermediate counts.

The navigation/command templates retain the framework's named parts and state transitions. Their
larger XAML diff is necessary to place glass behind the actual pane/action group while keeping dynamic
overflow, hierarchy and keyboard behavior. Unsupported stock GeneratedDuration attributes were
removed; explicit timed animations remain. No warning suppression or package dependency was added.

Further capture checks reproduced the misplaced disclosure, inherited date minimum width, square
calendar popup, undifferentiated adjacent-month dates, small slider touch area, malformed ellipsis,
stock overflow typography, clipped shortcuts and native navigation selection line. Each repro was
retained. The toolbar overflow's generic item-container style was replacing implicit Cupertino styles;
removing it restored the theme while preserving explicit consumer styles. A separate calendar-style
override repro moved popup defaults out of local values so consumer styles continue to take precedence.

Independent quality/contract review covered long alert text, right-to-left layout, default and hidden
actions, font overrides, borders, dynamic overflow, implicit styles and UTF-8 glyph integrity. No public
style key was removed or renamed. The new bar tint is registered in the live brush map and tested in
both appearances. The internal alert panel has no subscriptions or per-measure collection allocation.

## Final verification — 2026-09-22

- Cupertino desktop: **188/188 Debug and 188/188 Release**, with the temporary capture class removed.
- Cupertino Debug WebAssembly: build passed. Browser runtime was not exercised.
- Material desktop Debug: **63/63**.
- Simple desktop Debug: **257 passed, 1 existing skipped test, 0 failed**. The existing skip is
  `When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt`.
- XAML: all **210** tracked/new source files pass the pinned formatter. Solution C# whitespace passes;
  the formatter reports existing workspace-load warnings. Published docs and this audit pass spelling
  and Markdown checks; historical handoff/progress/lesson sections retain their existing lint findings.
- Final Cupertino builds report 105 warnings for each desktop configuration and 107 for WASM, with
  zero errors. Existing package/generated/shared-sample warnings remain; no new suppressions were added.
- Independent quality/contract and final visual reviews approve the result. Light/Dark captures cover
  the gallery, rest/pressed selection controls, menus, alerts, navigation over a colored backdrop,
  closed/open toolbars, calendar popups and wheel pickers.

Artifacts are under `%TEMP%/cupertino-hig`: `*-final.xml`, final build/test logs, and the named PNGs.
The scratch capture source is retained there, outside the repository. `popup-green` includes two
intermediate assertion failures caused by counting the accelerator label's leading margin as glyph
width. Numeric diagnostics and screenshots confirmed the visible 16px inset; the final regression
measures natural text width like the existing overflow-label test and passes in both appearances.

The **Styles → Cupertino Gallery** sample provides a permanent place to inspect the controls together.
Inter remains the cross-platform font substitute. Forced Liquid software captures demonstrate this
renderer’s effects, not parity with Apple's adaptive native optics; GPU/device and browser-runtime
visual verification were not performed. Native background extension, adaptive sheets and continuous
page-control scrubbing require platform/control behavior beyond this style pass and are not claimed.

The user authorized pushing everything after verification during this pass.
