# 11 — Generate the per-control style docs (`doc/styles/**`)

## Problem

`doc/styles/*.md` (Material v2) and `doc/styles/simple/*.md` are hand-maintained. Nothing
under `build/` produces them, so key removals and renames never reach them. The 2026-09-30
audit found stale rows from #1655 / #1666 (`SimpleButtonMedium*`, `SimpleIconButtonSmall*`),
wrong defaults (`styles/TextBox.md` → `MaterialMediumFontFamily`, Simple Button/ToggleButton
→ "Inter", `AppBarButtonHeight` → `SimpleSpace1600`), and ~17 undocumented keys.

## Approach

A C# file-based app, `build/scripts/GenerateStyleDocs.cs` (.NET 10 SDK is already the repo
baseline), that parses the library XAML statically and rewrites the generated regions of each
page. `--check` mode diffs and fails, mirroring `xstyler --passive`.

```bash
dotnet run build/scripts/GenerateStyleDocs.cs            # rewrite
dotnet run build/scripts/GenerateStyleDocs.cs -- --check # CI gate
```

### Sources of truth

| Column / table | Source |
|---|---|
| Style keys | `Styles/Controls/<Control>.xaml` (`<Style x:Key>`; `*Base*` templates excluded) |
| IsDefaultStyle | `Styles/Controls/_Resources.xaml` implicit `<Style BasedOn=…>` (both Material v2 and Simple) |
| Semantic alias | `Styles/Controls/SemanticStyles.xaml` alias block (`FilledButtonStyle` → `SimpleFilledButtonStyle`) |
| Lightweight keys | keys defined in `SemanticStyles.xaml` or `<Control>.xaml` **and referenced** (`{ThemeResource}` / `{StaticResource}`) by `<Control>.xaml`; `<!-- X.xaml lightweight resources -->` groups used as a cross-check that fails on disagreement |
| Type | element type; `StaticResource` aliases resolved by walking the alias chain across the library, `Uno.Themes` shared dictionaries, and a prefix→type table for tokens generated in `BaseTheme.ScaleGeneration.cs` (`Space*`, `Space*Thickness`, `Radius*CornerRadius`, `ControlHeight*`, `IconSize*`, …). Unresolvable key = hard failure |
| Value | Light-theme value (alias name or literal). A Dark column appears only on pages where some key differs (5 keys in Simple, 1 in Material today); HighContrast values footnoted |

### Page ownership

- Hand-owned: front-matter (`uid:`), title, prose, `> [!NOTE]` blocks — everything outside
  `<!-- BEGIN GENERATED --> … <!-- END GENERATED -->`.
- Generated: the Styles table and Lightweight Styling tables.
- Output: CRLF, backticked keys (cspell already skips inline code), padded tables matching markdownlint.

### Scope

- Material **v2** pages (`doc/styles/*.md`) and Simple pages (`doc/styles/simple/*.md`).
- `TextBlock.md` (type-scale tables) — generate from `SharedTypography.xaml` + per-theme `Typography.xaml` if the shape fits; otherwise leave hand-owned (decide during implementation).
- Cupertino has no `doc/styles` pages — out of scope.

### Rejected

- Runtime dump from the sample app: real types, but loses the alias names in the Value
  column, which is what consumers need to override.
- Preserving the current hand grouping (variants / states / "Themed vs Theme-agnostic") via
  per-page config: inconsistent today and nobody would maintain the config.

## Decisions

- [x] **Table grouping** — uniform generated layout chosen (2026-09-30): Styles table
      (`Style Key | Semantic Alias | IsDefaultStyle*`), then `### Theme-agnostic`
      (`Key | Type | Value`) and `### Themed` (`Key | Type | Light | Dark*`, Dark column only
      when a value differs), sorted by key. Hand-made variant/state subheadings are dropped.

## Plan

- [x] Generator: parse, resolve types, render, `--check`
- [x] Insert markers + regenerate all Material v2 and Simple pages; review diff for lost information
- [x] CI: `style_docs` job in `build/stage-docs-validations.yml` running `--check` (also a step in `build/stage-code-style.yml`, see Review)
- [x] Doc: note in `AGENTS.md` §13 that `doc/styles/**` tables are generated (and how to run it); reviewer lane (`quality`) flags hand edits inside generated regions
- [x] markdownlint + cspell pass on regenerated pages

## Fallback

If the generator does not land in this change, the known-wrong rows (TextBox.md:73,77;
simple Button.md/ToggleButton.md stale rows and "Inter"; AppBarButton.md:28-29;
CalendarView/Expander font weights) are fixed by hand.

## Review (2026-09-30)

Implemented as `build/scripts/GenerateStyleDocs.cs` (System.Xml.Linq only, no packages).
`build/scripts/Directory.Build.props`/`.targets` are empty stubs that stop the repo-root ones
(library packaging, `DotNet.ReproducibleBuilds` without a version → NU1015) from leaking into
the file-based app; `#:property` is evaluated too late to switch them off.

### Final rules

- **Pages:** every existing `doc/styles/<Control>.md` (Material v2) and `doc/styles/simple/<Control>.md`
  (Simple) except `TextBlock.md`, which stays hand-owned in both (Material's keys live in
  `SharedTypography.xaml`/`Typography.xaml`, Simple's table is a custom Font Specs/SDS Mapping
  shape). Sources: `<Control>.xaml` plus `<Control>.Base.xaml` when present (PipsPager). No pages
  are created; controls without a page are logged.
- **Styles table:** top-level `<Style x:Key>` in the sources whose `TargetType` (prefix stripped)
  contains the control name (FloatingActionButton → `Button`), excluding keys containing `Base` and
  `MUX_*`. Source order. Template-part styles are therefore excluded (logged as `info`).
- **IsDefaultStyle:** from each implicit `<Style BasedOn>` in `_Resources.xaml`, walk the BasedOn
  chain and mark every style visited until a `*Base*` key. Both the `*Default*Style` shim and the
  real style it is based on are marked.
- **Semantic Alias:** theme-agnostic `StaticResource` entries in `SemanticStyles.xaml` whose
  `ResourceKey` is a style; several aliases are comma-joined, ordinal-sorted.
- **Lightweight keys** = keys under the `<!-- <Control>.xaml lightweight resources -->` (and
  `<Control>.Base.xaml`) comment groups of `SemanticStyles.xaml`, **plus** every non-Style keyed
  resource defined in the control's own XAML (top level or `ThemeDictionaries`), minus aliases of
  styles. Group keys are included whether or not the control XAML references them: unreferenced ones
  (e.g. `PaneToggleButtonWidth`, `OutlinedButton*`) are consumed by WinUI templates or by aliases, and
  consumers override them all the same.
- **Cross-check (hard error):** a `SemanticStyles.xaml` key that the control XAML references
  (`{ThemeResource}`/`{StaticResource}`/`ResourceKey=`) must sit in that control's comment group. It
  held on every page at implementation time, so it is a reliable gate. Group keys the control does not
  reference are not reported (by design, see above).
- **Theme-agnostic vs Themed:** top-level definition → `### Theme-agnostic` (`Key | Type | Value`);
  otherwise `### Themed` (`Key | Type | Light [| Dark]`). `Default` and `Dark` dictionaries are both
  Dark. Dark column only when some row differs; otherwise the line "The Dark theme uses the same values
  as Light." HighContrast values that differ from Light are listed under the table. The control's own
  definition wins over `SemanticStyles.xaml`. Rows sorted ordinally by key; empty tables omitted.
- **Type:** element local name (`x:Double` → `Double`, …). `StaticResource` chains are walked across
  the theme's dictionaries (Material: `Controls/v2`, `Application/Common`, `Application/v2`; Simple:
  `Styles/**`; both: `Uno.Themes/Styles/Applications/Common`), then the tokens generated by
  `BaseTheme.ScaleGeneration.cs` (`Space*`, `Space*{,Horizontal,Vertical,Top,Bottom,Left,Right}Thickness`,
  `Radius*`, `Radius*CornerRadius`, `ControlHeight*`, `IconSize*`, `TouchTargetMinSize`, the
  `TypefaceScaleKeys` font families), styles (`Style`), and an allowlist of WinUI system resources.
  Unresolvable = error, exit 2.
- **Value:** alias key in backticks; `SolidColorBrush Color="{…}"` → the color key plus `(Opacity n)`;
  literals unformatted with whitespace collapsed, private-use glyphs as `\uXXXX`, pipes escaped.
- **Output:** LF (the existing pages are all `i/lf w/lf` and there is no `.gitattributes`; emitting
  CRLF would have rewritten every line), padded tables, trailing newline. Line endings of an existing
  page are preserved if it uses CRLF.

### Results

- 40 pages regenerated (19 Material, 21 Simple); `--check` exits 0; idempotent; a hand edit inside the
  markers makes `--check` exit 1.
- markdownlint (`build/.markdownlint.json`) and cspell (`build/cspell.json`, unchanged) pass on
  `doc/styles/**`. The hand-owned `simple/TextBlock.md` table was realigned for MD060. The HEAD
  versions of these pages had MD060 failures under current markdownlint; other docs
  (`doc/semantic-styles.md`, `doc/simple-controls-styles.md`) still do — out of scope.
- Keys that disappeared from pages: only `SimpleButton{Medium,Small}{FontSize,MinHeight,Padding}`,
  `SimpleIconButton{Medium,Small}{MinSize,Padding}`, `SimpleToggleButton{Medium,Small}{FontSize,MinHeight,Padding}`
  (gone from source), the Material semantic names (moved to the Semantic Alias column), and the
  template-part styles `SimpleAutoSuggestBoxTextBoxStyle`, `SimpleDatePickerFlyoutButtonStyle`,
  `SimpleSliderThumbStyle` (still in source, excluded by the TargetType rule).
- Prose moved below the END marker: NOTE blocks on simple AutoSuggestBox, CalendarView,
  ContentDialog (+ a paragraph), Expander, MenuFlyout, PersonPicture, ToolTip; the Outlined-alias
  sentence on simple Button (reworded to name `OutlinedButton*`). The MenuFlyout NOTE was corrected
  (`SimpleMenuFlyoutItemHeight` is now defined; `SimpleMenuFlyoutSeparatorHeight` is referenced but
  defined nowhere).
- Source gaps surfaced: Simple `CalendarDatePickerHeight` has no Dark (`Default`) definition
  (rendered "(not defined)", logged as a warning); `SimpleMenuFlyoutSeparatorHeight` is undefined.
- CI: Docs_Validations only runs when docs change, so the gate is also a step in Code Style (runs for
  every non-docs-only change), otherwise a XAML-only PR would skip it.
- Not covered by any page (no page exists): Material CalendarView, CommandBar, ContentDialog, Flyout,
  ListView, MediaPlayerElement, Ripple; Simple Flyout, NavigationView, PipsPager, ProgressBar,
  ProgressRing, RatingControl. Their lightweight keys (e.g. Simple `FlyoutPresenter{Background,BorderBrush}`,
  `NavigationView{Content,ExpandedPane}Background`, `ProgressBar{Background,Foreground}Disabled`) are
  therefore not documented yet.

### Follow-up (same change)

- Added stub pages (front-matter + H1 only) for every control with a Styles XAML but no page, then
  let the generator fill them: Simple `Flyout`, `NavigationView`, `PipsPager`, `ProgressBar`,
  `ProgressRing`, `RatingControl`; Material `CalendarView`, `CommandBar`, `ContentDialog`, `Flyout`,
  `ListView`, `MediaPlayerElement`, `Ripple`. All added to `doc/toc.yml` (both lists now sorted).
  This documents the `FlyoutPresenter*`, `NavigationView{Content,ExpandedPane}Background` and
  `ProgressBar*Disabled` keys the audit found missing.
- To add a page for a new control: create `doc/styles[/simple]/<Control>.md` with the `uid`
  front-matter and `# <Control> Control`, run the generator, add the TOC entry.
- Left as-is: `SimpleRegularFontWeight` / `SimpleSemiBoldFontWeight` (defined in
  `Styles/Application/Typography.xaml`, outside the inclusion rule).
- Source issues surfaced, not fixed here: Simple `CalendarDatePickerHeight` has no Dark definition;
  `SimpleMenuFlyoutSeparatorHeight` is referenced but never defined.
- **Pre-#1730 compatibility:** `SemanticStyles.xaml` was introduced by #1730 (backported to
  `servicing/8.0` in #1741). The generator treats it as optional and reads semantic style aliases
  from both it and `_Resources.xaml` (their earlier home), so the same script works on a branch
  either side of that change. Verified on a cherry-pick onto
  `origin/servicing/8.0`: `--check` passes, markdownlint passes, and the only keys dropped are the
  same removed `Simple*Medium*/Small*` keys and template-part styles as on master.
