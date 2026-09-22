# Handoff update — 2026-09-22 iOS/iPadOS HIG polish

The user confirmed iOS/iPadOS throughout and requested polishing the current styles against Apple's
current HIG. This update supersedes the solid navigation/bar recipe and optional visual-polish deferrals
in the historical handoff below. Full audit, references and results: [hig-polish.md](hig-polish.md).

## Current result

- Current capsule switches/sliders, interaction glass, and 44px touch targets.
- Persistent field labels, readable selections, compact date controls, rounded glass calendar/wheel
  popups, and subdued adjacent-month dates. Consumer calendar styles and borders remain honored.
- Glass navigation panes and floating command groups; Cupertino overflow rows, shortcuts and text-only
  actions. Dynamic overflow and consumer font overrides are covered.
- Leading alert title/message, adaptive capsule actions with long-label wrapping and RTL/default-action
  coverage; menu symbols trail labels and check marks have a shared gutter.
- All existing semantic keys remain available. The new live `CupertinoBarTintBrush` is documented.
- Permanent sample: **Styles → Cupertino Gallery** in CupertinoSampleApp. Launch with
  `dotnet run --project src/samples/CupertinoSampleApp/CupertinoSampleApp.csproj -f net10.0-desktop`.

## Verification and limits

- Cupertino desktop: **188/188 Debug and 188/188 Release**; Debug WebAssembly build passes.
- Material desktop Debug: **63/63**; Simple desktop Debug: **257 passed, 1 existing skip, 0 failed**.
- All 210 source XAML files pass formatting, solution C# whitespace passes, published docs and the new
  HIG audit pass spelling/Markdown. Historical spec/lesson sections retain existing lint findings.
- Light/Dark gallery, selection states, alerts, menus, navigation, toolbars and picker captures reviewed.
  Independent visual and quality/contract reviews approve. Scratch capture code was removed from the repo.
- Artifacts: `%TEMP%/cupertino-hig`, including `*-final.xml`, build logs, PNGs and scratch capture source.
- Existing build warnings remain (Cupertino desktop 105; WASM 107); no dependencies or suppressions added.
- No GPU/device or browser-runtime visual verification was performed. Inter is a cross-platform substitute
  for SF. Forced Liquid captures verify this renderer, not Apple's adaptive native optics. Generic Uno
  controls do not synthesize native background extension, adaptive sheets or continuous page scrubbing.
- The user explicitly authorized pushing everything during this pass; inspect branch history for publishing state.

## Historical Phase 4 continuation

The Phase 4 implementation below has now been completed locally. The earlier handoff is retained under
"Previous handoff" as historical implementation context; its pending-key and test-count statements are
superseded by this update. See `progress.md`, "Continuation review and results", for evidence and review.

## Current state

- All **72 semantic style keys resolve**; `PendingSemanticKeys` is empty.
- Added NavigationView/NavigationViewItem, CommandBar/AppBarButton, PipsPager and RatingControl styles,
  implicit styles, shared sample templates and Light/Dark rendered runtime coverage.
- Navigation and command bars use the handoff's **solid** recipe. Glass slabs/panes remain deferred.
- AppBarButton handles compact/right/overflow labels, shortcuts, submenu indicators and dimming. Its
  Width=Auto overrides the stock 68px width that clipped overflow labels (red/fix/green covered).
- Field/calendar brushes now repaint in place across override/clear and seed changes. Public legacy
  brush names remain aliases. Calendar hover/pressed opacity uses literal 0.85/0.4 defaults because
  resolving state tokens when the brush dictionary first loads gave opacity 1 at cold startup.
- RatingControl retains Uno's stock 32px layout with 16px glyphs; Uno assigns Height locally, so a
  proposed 20px style setter would not work.
- Cupertino mapping column, control reference and lightweight-styling cross-links are updated.

## Verified

- Cupertino desktop: **135/135 Debug and 135/135 Release**.
- Material desktop Debug: **63/63**.
- Simple desktop Debug: **257 passed, 1 existing ignored test, 0 failed**.
- Cupertino Debug browserwasm build passes. No browser runtime smoke was performed.
- All tracked XAML and changed/new XAML formatting, solution C# whitespace, cspell and markdownlint pass.
- Four software-rendered Light/Dark captures inspected. Temporary capture test removed.
- Seven review lenses applied; findings fixed, skeptic re-review says ship.
- Builds retain package/generated/shared-sample warnings; no new warning suppressions or dependencies.

Artifacts: `%TEMP%/cupertino-phase4`, including `*-verified.xml`, `*-verified-build.log`, regression XML
for Material/Simple, `wasm-final-build.log`, and `phase4-{Light,Dark}-{controls,overflow}.png`. Scratch
`run-tests.ps1` there launches hidden Windows processes, sets DOTNET_MODIFIABLE_ASSEMBLIES=debug, captures
stdout/stderr and rejects missing/empty results. Do not assume scratch artifacts survive on another machine.

## Remaining work / maintainer decisions

1. GPU, browser-runtime, Android and Windows-native visual/device verification. Glass tint/presets still
   need hardware tuning. Hosting smoke was not rerun; the older Win32 failure remains recorded below.
2. Glass NavigationView pane / shared CommandBar slab, calendar restyling, drag glass and pop-open polish
   remain optional follow-ups. No Expander style is required by the semantic contract.
3. Semantic button aliases stay as previously agreed/implemented (ordinary buttons solid; FAB family
   glass). A change to the earlier spec's glass button aliases remains a maintainer decision.
4. Toolkit adoption, pointer idiom, Markup package and other Phase 5 follow-up issues remain to be
   created/published explicitly. No push or PR was performed.
5. Other known review items from earlier phases remain in progress.md; this continuation's review scope
   was its own Phase 4 diff, not a fresh audit of the entire branch.

## Previous handoff (historical)

# Handoff — Cupertino v2 (spec 10), written 2026-09-22

For the next agent picking up `dev/sb/cupertino-v2`. `progress.md` is the plan and the per-item record;
this file is the state of play, how to work the branch, and what is left with a concrete plan for each item.
Read `AGENTS.md` and `specs/lessons.md` first; the rules there (red / fix / green, no `Assert.Inconclusive`,
no push without approval, Conventional Commits, formatters) all apply.

## 1. Where things stand

- **Branch:** `dev/sb/cupertino-v2` on top of `master`. The theme is `CupertinoTheme : BaseTheme`, the only
  entry point (no V1, no compatibility classes — D-1 as amended). Everything up to and including Phase 3 is
  pushed. **Three Phase 4 commits are local and unpushed** — pushing needs the maintainer's OK:
  - `62b11489 feat(cupertino): put popovers and menus on Thick glass`
  - `3f4c1b41 feat(cupertino): restyle the list view as an inset group`
  - `b2f60186 feat(cupertino): restyle the content dialog as an alert on Thick glass`
- **Semantic contract ratchet** (`Given_CupertinoTheme.PendingSemanticKeys`): 66 of 72 keys resolve. Pending:
  `AppBarButtonStyle`, `CommandBarStyle`, `NavigationViewStyle`, `NavigationViewItemStyle`, `PipsPagerStyle`,
  `RatingControlStyle`. The test fails as soon as a pending key starts resolving, so shrink the list with the
  work.
- **Verified state at the last commit:** Cupertino head **105 / 105 in Debug** (`net10.0-desktop`); XAML
  Styler passive pass on every touched file; `dotnet format whitespace --verify-no-changes` clean; cspell +
  markdownlint clean on `doc/cupertino-controls-styles.md`.
- **Not re-verified this session:** the Release configuration (last full Release run was at the end of Phase
  3: 99 / 99), and the Material and Simple heads (this session edited shared sample pages: Flyout, MenuFlyout,
  ToolTip, ListView, ContentDialog — the heads compiled them last at the end of Phase 3 for other pages).
  Build both heads before the next push.

## 2. What Phase 4 shipped so far

| Slice | Files | Notes |
|---|---|---|
| Popovers, menus, tooltip, ComboBox popup | `Styles/Controls/Flyout.xaml`, `MenuFlyout.xaml`, `ToolTip.xaml`, `ComboBox.xaml` (popup), `CupertinoBrushes.xaml` + `CupertinoConstants.BrushColorKeys` (`CupertinoPopoverTintBrush`, `SurfaceColor` at 0.76) | `FlyoutPresenter` / `MenuFlyoutPresenter` retemplated: `Grid > [uc:GlassPanel Material=Thick, Border CornerRadius (clips) > ScrollViewer > presenter]`. Menu rows 44 px, Padding 16,0, icon 22 leading + 10; checked toggle **and radio** rows show a checkmark (Apple has no radio glyph); sub-item chevron trailing; separator = 0.5 px hairline in an 8 px band. Rows highlight with `SecondaryContainerBrush`, dim when disabled. |
| Lists | `Styles/Controls/ListView.xaml` | The `ListView` is the group (`SurfaceBrush`, `CupertinoGroupCornerRadius` 26, clipped); rows `CupertinoRowMinHeight` 44, hairline inset 16, selected = `PrimarySelectedBrush`. Every row draws its hairline, so the last one sits on the group's edge (invisible in captures). |
| Alert | `Styles/Controls/ContentDialog.xaml`, `CupertinoDialogTintBrush` (`SurfaceColor` at 0.8) | `BackgroundElement` **must stay a `Border`** (Uno casts it). Rows stacked primary → secondary → close with hairlines, default action SemiBold. Dimming: Uno paints it through the popup's light-dismiss overlay from `ContentDialogLightDismissOverlayBackground` (aliased to `CupertinoDimmingBrush` #59000000); the template's `LayoutRoot.Background` is only for WinAppSDK. `CupertinoContentDialog{,Default,Destructive}ButtonStyle` are public. |

Tests: `src/samples/CupertinoSampleApp/RuntimeTests/Given_CupertinoContainers.cs` (flyout, menu, list, dialog,
combo popup). Samples: Cupertino templates added to the Flyout, MenuFlyout, ToolTip, ListView and
ContentDialog sample pages (`SupportedDesigns` now include `Design.Cupertino`). Docs:
`doc/cupertino-controls-styles.md` table rows + "Lists", "Alerts", "Popovers and menus" sections.

## 3. How to work the branch

### Build and run the runtime tests (headless, Windows dev box)

There is no `dotnet test`. The Cupertino head hosts its own tests (`Uno.UI.RuntimeTests.Engine`).

```powershell
dotnet build src/samples/CupertinoSampleApp/CupertinoSampleApp.csproj -c Debug -f net10.0-desktop -nologo -v q "/clp:ErrorsOnly;Summary"
```

Runner script (recreate it in a scratch folder; it was never checked in):

```powershell
param([string]$App = 'CupertinoSampleApp', [string]$Config = 'Release', [string]$Filter = '{}', [int]$TimeoutSec = 600)
$scratch = $PSScriptRoot
$out = Join-Path $scratch "$App-tests.xml"
if (Test-Path $out) { Remove-Item $out }
$env:UNO_RUNTIME_TESTS_RUN_TESTS = $Filter
$env:UNO_RUNTIME_TESTS_OUTPUT_PATH = $out
$env:DOTNET_MODIFIABLE_ASSEMBLIES = 'debug'
$exe = "X:\src\uno.themes\src\samples\$App\bin\$Config\net10.0-desktop\$App.exe"
$p = Start-Process $exe -ArgumentList "--runtime-tests=$out" -PassThru `
    -RedirectStandardOutput (Join-Path $scratch "$App-stdout.log") -RedirectStandardError (Join-Path $scratch "$App-stderr.log")
if (-not $p.WaitForExit($TimeoutSec * 1000)) { $p.Kill(); 'TIMED OUT' }
"exit=$($p.ExitCode)"
if (-not (Test-Path $out)) { 'no results file'; Get-Content (Join-Path $scratch "$App-stdout.log") -Tail 30; return }
[xml]$x = Get-Content $out
$cases = $x.SelectNodes('//test-case')
$failed = @($cases | Where-Object { $_.result -eq 'Failed' })
$other = @($cases | Where-Object { $_.result -ne 'Failed' -and $_.result -ne 'Passed' })
"{0} cases, {1} passed, {2} failed, {3} other" -f $cases.Count, ($cases.Count - $failed.Count - $other.Count), $failed.Count, $other.Count
$failed + $other | ForEach-Object { "  $($_.result): $($_.fullname)"; if ($_.failure.message) { "    " + ($_.failure.message.InnerText ?? $_.failure.message).ToString().Split("`n")[0] } }
```

Usage: `pwsh -NoProfile -File run-tests.ps1 -Config Debug` (whole suite, ~1 min) or
`-Filter '{"Filter":{"Value":"Given_CupertinoContainers | EverySemanticKeyResolvesOrIsTrackedAsPending"},"Attempts":1}'`.
The full failure text is in the XML (`failure/message`); the script prints only the first line.

### Look at what you built

The dev box has no GPU: `AreEffectsFast()` is false, so `GlassRenderingMode.Auto` picks the **Solid** tier,
and a popup's `GlassPanel` reads the *application* theme's mode (it is not under your test container), so
popups are always Solid here. Shape, colour and layout are still worth a look, and shape bugs have only ever
shown in captures (`specs/lessons.md`: the ellipse, the vanished hairline). Recipe, as a throwaway test class
in `RuntimeTests/` (**delete before committing**; `dotnet format` will also fail on it because the Write tool
emits LF):

- Host: `new Grid { Width = 600, Height = 500, RequestedTheme = theme }` with
  `Resources.MergedDictionaries.Add(new CupertinoTheme { GlassRenderingMode = GlassRenderingMode.Liquid })`
  and a `StackPanel` of coloured 42 px `Border`s as a backdrop; set `UnitTestsUIContentHelper.Content`.
- Open the thing (`flyout.ShowAt(anchor)`, `menu.ShowAt(anchor)`, `comboBox.IsDropDownOpen = true`,
  `dialog.ShowAsync()` with `XamlRoot = host.XamlRoot`), `await WaitForIdle()`, `await Task.Delay(1500)`.
- Walk `VisualTreeHelper.GetParent` from the host to the root visual and `RenderTargetBitmap.RenderAsync` it:
  popups are included when the root is captured. Copy the BGRA pixels into an `SKBitmap`
  (`Bgra8888`, `Premul`), encode PNG, write to a folder; read the PNG with the image tool.
- A `ContentDialog` shown with `XamlRoot` follows the *app* theme, not the host's `RequestedTheme`.

### Gates before a commit

```powershell
# XAML Styler: two passes, then passive, on the files you touched (comma-separated list)
dotnet xstyler -c Settings.XamlStyler -f $files; dotnet xstyler -c Settings.XamlStyler -f $files
dotnet xstyler -c Settings.XamlStyler --passive -f $files
# C# whitespace
$env:TargetFrameworkOverride='desktop'; dotnet format whitespace Uno.Themes.sln --verify-no-changes --exclude src/samples/SamplesApp.Shared
# Docs (cspell / markdownlint are not installed globally; npx works)
npx --yes cspell --config build/cspell.json --no-progress doc/cupertino-controls-styles.md
npx --yes markdownlint-cli -c build/.markdownlint.json doc/cupertino-controls-styles.md
```

New `.xaml` / `.cs` files written with the Write tool come out LF; normalise to CRLF before the styler
(a one-line Python `replace(b'\r\n', b'\n').replace(b'\n', b'\r\n')`). `progress.md` is LF in the index —
leave it.

Commit messages: Conventional Commits, `feat(cupertino): …` / `fix(cupertino): …` / `docs(cupertino): …`,
bullet body, trailers `Co-Authored-By: Claude Fable 5.1 <noreply@anthropic.com>` and
`Claude-Session: https://claude.ai/code/session_017aEgrH6nk9JAwJn6xXCpcG`. In PowerShell write the message
to a file and `git commit -F` it (piping a here-string into `git commit -F -` does not work).

### Docs table helper

`doc/cupertino-controls-styles.md` starts with a column-aligned control table. A script that takes
`"<Control>=Key1,Key2"` arguments, rewrites the matching row (or inserts it, sorted case-insensitively) and
re-aligns the columns saves a lot of hand editing; it is ~35 lines of Python (parse the `| Control` table,
`dict` of control → cell, `'<br/>'.join` the keys, pad to the widest cell). Re-pass the **full** key list for
a row you extend (the `Button` row has 22 keys).

## 4. Facts learned that are not written in the code

- `{ThemeResource}` inside a visual-state setter, and any brush read inside a popup, resolves against the
  **application** theme, another instance of the same palette. Compare colours, never brush instances, in
  those cases (`Given_CupertinoContainers.ColorOf`). Under the test container itself, `Assert.AreSame` on the
  theme's brush works.
- A top-level `<SolidColorBrush Color="{ThemeResource XColor}" />` is a one-time snapshot no seed or override
  reaches. Opacity-carrying brushes go into `CupertinoBrushes.xaml` (three theme blocks, no `Color`) with a
  `CupertinoConstants.BrushColorKeys` entry; the theme paints them from the resolved colours. Plain aliases
  go into the control file's `ThemeDictionaries` as `StaticResource` and are read with `{ThemeResource}`.
- `StaticResource` across the merged control dictionaries works at runtime regardless of file order
  (`ComboBox.xaml` reads keys declared in `MenuFlyout.xaml` and `Flyout.xaml`).
- A `Border` with `CornerRadius` and no background clips its child on Uno Skia — used to clip menu rows,
  list rows and the dialog's content to the glass shape.
- A 0.5 px `Rectangle` is layout-rounded to 0 unless `UseLayoutRounding="False"` (lesson recorded).
- `RadiusFull` (9999) on a non-square makes an ellipse (lesson recorded); use real radii.
- `VisualTreeHelper.GetOpenPopupsForXamlRoot(xamlRoot)` finds an open flyout's presenter (`popup.Child`), a
  `ComboBox`'s popup child (the `PopupBorder` grid) and a shown `ContentDialog` (`popup.Child` is the dialog).
  `Popup.LightDismissOverlayBackground` is internal — assert the resource instead.
- Uno's `ContentDialog` template parts are cast: `BackgroundElement`, `Container` → `Border`; `LayoutRoot`,
  `CommandSpace`, `DialogSpace` → `Grid`; `Title` → `ContentControl`; `Content` → `ContentPresenter`;
  `ContentScrollViewer` → `ScrollViewer`; the three buttons → `ButtonBase`. Uno goes to
  `DialogShowingWithoutSmokeLayer` (it dims through the popup), so the template's smoke is WinAppSDK-only.
- The `MenuFlyoutPresenter` needs `MenuFlyoutPresenterScrollViewer`; the item templates want `TextBlock`,
  `IconRoot`, `IconContent`, `CheckGlyph` / `SubItemChevron`, `KeyboardAcceleratorTextBlock` (null-checked).
- `SymbolThemeFontFamily` glyphs (`E73E` checkmark, `E76C` chevron) render on Skia.
- Font weights: no `{ThemeResource …FontWeight}` precedent in Cupertino XAML — use literals.
- The sandboxed PowerShell tool refuses a command containing `Remove-Item` together with a `//` elsewhere in
  the same command (it reads `//` as a system path). Delete files in a separate call.

## 5. What is left in Phase 4

Every item below has a resource-override recipe (Simple's approach, no retemplate) unless said otherwise —
that is the lazy version that meets the contract; glass on these bars is a maintainer call after a GPU look.

### 5.1 NavigationView / NavigationViewItem (2 keys)

- New `Styles/Controls/NavigationView.xaml` with `ThemeDictionaries` overriding the stock keys (copy the key
  set from `src/library/Uno.Simple.WinUI/Styles/Controls/NavigationView.xaml`): pane / content backgrounds →
  `SurfaceBrush`; item foreground `OnSurfaceBrush`, pointer-over / pressed `SecondaryContainerBrush`,
  selected `PrimarySelectedBrush` with `PrimaryBrush` foreground; selection indicator `PrimaryBrush`;
  separator `OutlineVariantBrush`; top-nav item foregrounds likewise.
- `CupertinoNavigationViewStyle` (`Background`) and `CupertinoNavigationViewItemStyle` (`Foreground`,
  `CornerRadius` 10, `MinHeight` 44). Check which sizing keys the stock template reads with `{ThemeResource}`
  (grep Uno's NavigationView XAML under `X:\src\uno\src\Uno.UI\Microsoft\UI\Xaml\Controls\NavigationView\` —
  the folder was not located this session; `NavigationViewItemOnLeftMinHeight` is the WinUI name).
- Test: resolve both keys, render a `NavigationView` with two items (one selected) and assert the selected
  container's presenter colours / the item height; `NavigationViewSamplePage_MUX` gets a Cupertino template
  and `Design.Cupertino`. Docs rows `muxc:NavigationView`, `muxc:NavigationViewItem`.
- **Cut, documented:** the glass sidebar (`GlassPanel Material=Regular` behind the pane) needs the stock
  template retemplated; list it for the maintainer.

### 5.2 CommandBar / AppBarButton (2 keys)

- `AppBarButton`: retemplate along Simple's `AppBarButton.xaml` shape — `Grid Root > StackPanel(icon
  Viewbox 22 px, label)`; 44 × 44 minimum, glyph `OnSurfaceBrush`, interaction dims `Root.Opacity` like every
  Cupertino button (hover `CupertinoHoverOpacity`, pressed `CupertinoButtonPressedOpacity`, disabled
  `CupertinoDisableStateOpacity`). Bind the label to `Label`, not `Content`. Visual states the control drives:
  `CommonStates`, `ApplicationViewStates` (`FullSize`, `Compact`, `LabelOnRight`, `LabelCollapsed`,
  `Overflow`, `OverflowWithToggle`, …) — collapse the label in `Compact` / `LabelCollapsed`, stack
  horizontally in `LabelOnRight`; missing states are ignored. Uno's stock template is
  `X:\src\uno\src\Uno.UI\UI\Xaml\Controls\AppBar\AppBar.xaml` (`XamlDefaultAppBarButton`, line ~1060).
- `CommandBar`: `CupertinoCommandBarStyle` on the stock template with `ThemeDictionaries` overriding
  `CommandBarBackground` (transparent or `SurfaceBrush`), `CommandBarForeground`,
  `CommandBarOverflowPresenterBackground` / `BorderBrush` (surface + hairline — or make the overflow the
  glass menu by retemplating only the overflow presenter), `CommandBarEllipsisIconForegroundDisabled`; bar
  height 44 via `AppBarThemeCompactHeight` if the stock template reads it with `{ThemeResource}` (check
  `X:\src\uno\src\Uno.UI\UI\Xaml\Controls\CommandBar\CommandBar.xaml`, `XamlDefaultCommandBar` line ~218).
- Samples: `CommandBarSamplePage` and `AppBarButtonSamplePage` need Cupertino templates + `Design.Cupertino`.
- **Cut, documented:** "one shared glass slab per group, prominent primary command" — a full `CommandBar`
  retemplate (parts `PrimaryItemsControl`, `SecondaryItemsControl`, `MoreButton`, `OverflowPopup`,
  `OverflowContentRoot`, `ContentControl`, `LayoutRoot`, `ContentRoot`). Maintainer call after a GPU look.

### 5.3 PipsPager (1 key)

- The stock pip styles read their sizes with `{StaticResource}` from the system dictionary, so an app-level
  key cannot change them. Set the pips through the control's own properties instead: `CupertinoPipsPagerStyle`
  with `NormalPipStyle` / `SelectedPipStyle` setters pointing at two small `Button` styles whose template is a
  7 px `Ellipse` (`Fill="{TemplateBinding Foreground}"`) in a 16 px box (7 pt dot + 9 pt gap), normal
  `OnSurfaceLowBrush`, selected `OnSurfaceBrush`. Override the `PipsPagerNavigationButton*` brushes in
  `ThemeDictionaries` for the arrows. Uno's stock resources:
  `X:\src\uno\src\Uno.UI\UI\Xaml\Controls\PipsPager\PipsPager_themeresources.xaml`.
- Test: resolve the key, render a 5-page pager, assert the selected pip's colour and size; sample page gets
  a Cupertino template.

### 5.4 RatingControl (1 key)

- Simple's recipe: `ThemeDictionaries` overriding `RatingControlSelectedForeground` → `PrimaryBrush`,
  `RatingControlUnselectedForeground` → `SecondaryBrush`, `RatingControlPointerOverSelectedForeground` →
  `PrimaryBrush`, `RatingControlPointerOverUnselectedForeground` / `RatingControlPointerOverPlaceholderForeground`
  → `OnSurfaceVariantBrush`, `RatingControlPlaceholderForeground` → `OnSurfaceBrush`,
  `RatingControlDisabledSelectedForeground` → `OnSurfaceDisabledBrush`, `RatingControlCaptionForeground` →
  `OnSurfaceVariantBrush` (Uno's key names, from
  `X:\src\uno\src\Uno.UI\UI\Xaml\Controls\RatingControl\RatingControl_themeresources_v1.xaml` — note
  `PointerOver` is in the middle of the name, unlike Simple's file). `CupertinoRatingControlStyle`: `Height`
  20, `Foreground`, `FontFamily SymbolThemeFontFamily`. The glyph size is internal (16 px), fine.

### 5.5 The snapshot-brush bug in the files Phase 4 was going to rewrite (red / fix / green)

Same defect as the Slider / ToggleSwitch fix (`b94789d0`): brushes declared next to a style as
`<SolidColorBrush Color="{ThemeResource XColor}" Opacity=… />` are one-time snapshots. Remaining:

| Key (file) | Recipe | Fix |
|---|---|---|
| `CupertinoTextBoxBorderBrush` (TextBox, NumberBox), `CupertinoPasswordBoxBorderBrush`, `CupertinoComboBoxBorderBrush`, `CupertinoDatePickerBorderBrush`, `CupertinoDeleteButtonTextBoxBrush` (TextBox) | `LabelColor` at 0.2 | One painted brush `CupertinoFieldBorderBrush` (`CupertinoBrushes.xaml`, Opacity 0.2, `BrushColorKeys` → `LabelColor`); the five keys become per-theme `StaticResource` aliases of it, read with `{ThemeResource}` |
| `CupertinoHeaderForegroundBrush` (NumberBox) | `LabelColor` at 0.7 | painted brush |
| `CupertinoDetailsLightBrush` (NumberBox) | `CupertinoPrimaryGrayColor` at 0.3 | painted brush |
| `CupertinoCalendarViewSelectedBackground` (CalendarView) | `CupertinoBlueColor` at 0.27 | painted brush with `new[] { "CupertinoBlueColor", PrimaryColor }` so it follows a seed like `CupertinoBlueBrush` |
| `CupertinoCalendarDatePickerBorderBrushPointerOver` / `Pressed` | `CupertinoPrimaryGrayColor` at hover / pressed opacity | painted brushes |
| `CupertinoDatePickerFlyoutPresenterHighlightFill` | `CupertinoQuinaryGrayColor`, no opacity | per-theme alias of `CupertinoQuinaryGrayBrush` |

Test first (fails today): `new CupertinoTheme(colorOverride: { LabelColor = #123456 })` then assert
`Resource<SolidColorBrush>(container, "CupertinoTextBoxBorderBrush").Color == #123456`; a `PrimarySeed` for
the calendar brush. The Phase 3 fix's script pattern: remove the top-level brush, insert the per-theme alias
block, replace `{StaticResource Key}` reads with `{ThemeResource Key}`.

### 5.6 Optional / cut candidates (decide with the maintainer)

- CalendarView / CalendarDatePicker / DatePicker restyle (day cells 44, accent circle) and
  `DatePickerFlyoutPresenter` on a glass backplate — the semantic keys already resolve; only the look changes.
- Glass interactivity polish (switch / slider knobs become `Thin` glass while dragging) — repaints every frame
  while dragging; needs GPU eyes. Recommend cutting.
- Expander — no Cupertino style, no semantic key; skip.
- The popover pop-open animation (scale 0.94 → 1, `CupertinoPopoverDuration` 0.18 s) — cut; Uno's popup
  transition is what shows.

### 5.7 Phase 4 gate

- Ratchet empty (`PendingSemanticKeys` = `{}`); `Given_CupertinoContainers` green.
- `doc/semantic-styles.md` gains its Cupertino column (every key → `Cupertino*` style, gaps named).
- Build the Cupertino head for WASM (`dotnet build … -f net10.0-browserwasm`), and the Material and Simple
  heads for desktop; run the Cupertino suite in **Release** too; XAML Styler passive over `git ls-files
  '*.xaml'` (two format passes first), `dotnet format` verify.
- The `ThemesSampleApp --smoke` hosting check fails on `master` on Win32 (`CleanupNonDefaultAlcCaches`
  reflection drift) — separate issue, do not chase it here; CI runs it on X11.

## 6. Phase 5 (wrap-up)

- `doc/lightweight-styling.md` cross-links; `doc/cupertino-controls-styles.md` read end to end.
- `/review-panel` on the final diff; findings triaged in `progress.md` under "Review".
- `specs/lessons.md` for any correction received.
- Follow-up issues: uno.toolkit.ui `CupertinoToolkitTheme` → derive from `CupertinoTheme`, `GlassPanel` for
  TabBar / NavigationBar; macOS pointer idiom (D-6); `Uno.Cupertino.WinUI.Markup`; SkSL single-pass glass;
  composition tier for a non-Skia backend. `AGENTS.md` still says libraries target `net9.0`.

## 7. Open questions for the maintainer (unanswered as of this handoff)

1. **`NoWarn Uno0001`** — he picked "add it" earlier, but the premise (V1 duplicating the warnings) went away
   with V1; not added. Confirm or drop.
2. **No semantic button key resolves to glass except the FAB family** (spec said `FilledButtonStyle` → glass
   prominent, `ElevatedButtonStyle` / `IconButtonStyle` → glass). One alias line each to flip.
3. **Glass presets and veils are untuned** and the popover / dialog tints (0.76 / 0.8 of the surface) were set
   from the spec's hex values, never seen on a GPU. WASM / Android / GPU checks need his hardware.
4. **Bars on glass** (CommandBar slab, NavigationView pane) — retemplate or keep the solid recipe (§5.1, 5.2).
5. **List selection = `PrimarySelectedBrush`** (accent tint) per the spec; iOS uses a gray. Cheap to flip.
6. Alert width 270–320 (Apple's 270; the spec said 320 "[measure]"), radius 30 "[measure]".

## 8. File map (Cupertino library)

`src/library/Uno.Cupertino/`: `CupertinoTheme.cs` (theme, `GlassRenderingMode` DP), `CupertinoConstants.cs`
(`BrushColorKeys` — every painted brush must be listed), `Controls/GlassPanel.cs` + `SkiaGlassBackplate.cs` +
`GlassPreset.cs` + enums, `Styles/Application/{ColorPalette,CupertinoBrushes,Fonts,StateConstants,AnimationConstants}.xaml`,
`Styles/Controls/*.xaml` (one file per control, merged by `Uno.XamlMerge.Task`; `_Resources.xaml` holds the
implicit styles and the semantic aliases). Tests: `src/samples/CupertinoSampleApp/RuntimeTests/Given_*.cs`.
Samples: `src/samples/SamplesApp.Shared/Content/Controls/*SamplePage.xaml(.cs)` (`SamplePageLayout.CupertinoTemplate`
+ `Design.Cupertino` in the attribute). Docs: `doc/cupertino-controls-styles.md`, `doc/cupertino-getting-started.md`.
