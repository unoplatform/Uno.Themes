# Tasks: Simple Toolkit Styles

**Input**: Design documents from `specs/simple-toolkit-styles/`
**Prerequisites**: plan.md ✅, spec.md ✅

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to

## Path Conventions

- **Toolkit repo**: `[local toolkit repo path]`
- **New library**: `src/library/Uno.Toolkit.Simple/`
- **Reference impl**: `src/library/Uno.Toolkit.Material/`
- **Style files**: `src/library/Uno.Toolkit.Simple/Styles/Controls/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the `Uno.Toolkit.WinUI.Simple` project with build pipeline

- [ ] T001 [SETUP] Create `src/library/Uno.Toolkit.Simple/Uno.Toolkit.WinUI.Simple.csproj` — copy from `Uno.Toolkit.WinUI.Material.csproj`, replace `Material`→`Simple`, change PackageRef from `Uno.Material.WinUI` to `Uno.Simple.WinUI`, remove v1/v2 version logic, keep `MSBuild.Sdk.Extras/3.0.44` SDK, keep ProjectRef to `Uno.Toolkit.WinUI.csproj`
- [ ] T002 [SETUP] Create `src/library/Uno.Toolkit.Simple/SimpleToolkitTheme.cs` — copy from `MaterialToolkitTheme.cs`, replace class name, namespace `Uno.Toolkit.UI.Simple`, URI paths to use `Uno.Toolkit.WinUI.Simple` assembly name, load `SimpleTheme` instead of `MaterialTheme`, use `v1` version suffix
- [ ] T003 [P] [SETUP] Create `src/library/Uno.Toolkit.Simple/AssemblyInfo.cs` — copy from Material, update namespace
- [ ] T004 [P] [SETUP] Create `src/library/Uno.Toolkit.Simple/xamlmerge-simple.props` — set `XamlMergeOutputFile` to `Generated\mergedpages.WinUI.v1.xaml`, set `XamlMergeInput` to `Styles\Controls\**\*.xaml`, import `..\..\..\..\build\XamlMerge.props`
- [ ] T005 [P] [SETUP] Create `src/library/Uno.Toolkit.Simple/build/Package.targets` — copy from Material, update assembly name to `Uno.Toolkit.WinUI.Simple`
- [ ] T006 [SETUP] Create minimal `src/library/Uno.Toolkit.Simple/Styles/Controls/_Common.xaml` — start with just the `xmlns` declarations and empty body. Will be populated as style files are added.
- [ ] T007 [SETUP] Add `Uno.Toolkit.WinUI.Simple.csproj` to the solution file (`uno.toolkit.ui.sln` or relevant `.slnf`)
- [ ] T008 [SETUP] Verify `dotnet build` succeeds for the new project on `net9.0-desktop` target

**Checkpoint**: Empty library builds. Entry point class loads. XamlMerge runs.

---

## Phase 2: User Story 1 — Card & CardContentControl (Priority: P1) 🎯 MVP

**Goal**: Filled, Outlined, Elevated card variants with SDS tokens  
**Independent Test**: Add `<SimpleToolkitTheme />` to a test page, render 6 card instances

### Implementation for User Story 1

- [ ] T010 [US1] Create `Styles/Controls/CardContentControl.xaml` — Read `Uno.Toolkit.Material/.../v2/CardContentControl.xaml` as reference. Create 4 styles: `SimpleBaseCardContentControlStyle` (shared setters: CornerRadius=8, Padding=24, HorizontalContentAlignment=Stretch), `SimpleFilledCardContentControlStyle` (BasedOn base, Template with Grid+ContentPresenter+HoverOverlay+PressedOverlay, Background=SurfaceBrush), `SimpleOutlinedCardContentControlStyle` (BasedOn filled, adds BorderBrush=OutlineBrush, BorderThickness=1), `SimpleElevatedCardContentControlStyle` (own Template with ThemeShadow at Z=8). Include ThemeDictionaries with `Default` and `Light` keys. Use opacity overlays instead of Ripple for PointerOver/Pressed states.
- [ ] T011 [US1] Create `Styles/Controls/Card.xaml` — Read `Uno.Toolkit.Material/.../v2/Card.xaml` as reference. Create 12 styles (3 variants × 3 layouts + 3 bases): base, filled, outlined, elevated for each of default, avatar, smallmedia layouts. Template includes HeaderContent, SubHeaderContent, AvatarContent, MediaContent, SupportingContent, IconsContent regions with SDS spacing (24/16/8px). Use ThemeShadow for elevated, opacity overlays for interaction states.
- [ ] T012 [US1] Update `_Common.xaml` — Add theme-agnostic aliases for all Card/CardContentControl styles: `FilledCardStyle`, `OutlinedCardStyle`, `ElevatedCardStyle`, `FilledCardContentControlStyle`, `OutlinedCardContentControlStyle`, `ElevatedCardContentControlStyle`, plus avatar and smallmedia variants.
- [ ] T013 [US1] Build and verify `dotnet build` succeeds on `net9.0-desktop`

**Checkpoint**: Cards render with SDS styling. Filled/Outlined/Elevated visually distinct.

---

## Phase 3: User Story 2 — Chip & ChipGroup (Priority: P1)

**Goal**: Tag-style chips with pill shape, filter/input/suggestion variants  
**Independent Test**: Render chips of each variant, toggle filter chips, remove input chips

### Implementation for User Story 2

- [ ] T020 [P] [US2] Create `Styles/Controls/Chip.xaml` — Read `Uno.Toolkit.Material/.../v2/Chip.xaml` as reference. Create 10 styles: `SimpleChipStyle` (base with full Template: pill CornerRadius=999, Height=32, Padding=12,0), `SimpleAssistChipStyle`, `SimpleInputChipStyle` (with remove icon), `SimpleFilterChipStyle` (with checkmark), `SimpleSuggestionChipStyle`, plus 3 elevated variants, `SimpleDefaultChipStyle` (implicit). Template uses SurfaceVariantBrush background, OnSurfaceBrush foreground. NO `um:Ripple` — use opacity overlays. Checked state uses PrimaryBrush background.
- [ ] T021 [P] [US2] Create `Styles/Controls/ChipGroup.xaml` — Read `Uno.Toolkit.Material/.../v2/ChipGroup.xaml` as reference. Create 8 styles: 7 consumer + 1 base. Each sets `ItemContainerStyle` to matching Chip variant style. `SimpleFilterChipGroupStyle` → `SimpleFilterChipStyle`, etc.
- [ ] T022 [US2] Update `_Common.xaml` — Add implicit `<Style TargetType="utu:Chip" BasedOn="{StaticResource SimpleDefaultChipStyle}" />` and theme-agnostic aliases for all chip/chipgroup styles.
- [ ] T023 [US2] Build and verify on `net9.0-desktop`

**Checkpoint**: Chips render as SDS Tags. Toggle, remove, selection all work.

---

## Phase 4: User Story 3 — TabBar & TabBarItem (Priority: P1)

**Goal**: Navigation tabs/pills for top, bottom, and vertical orientations  
**Independent Test**: Render TabBar with 4 items in each orientation, verify selection

### Implementation for User Story 3

- [ ] T030 [US3] Create `Styles/Controls/TabBar.xaml` — Read `Uno.Toolkit.Material/.../v2/TabBar.xaml` as reference. Create ALL TabBar AND TabBarItem styles in this single file (Material puts them together). TabBar styles (6): `SimpleBottomTabBarStyle`, `SimpleTopTabBarStyle`, `SimpleColoredTopTabBarStyle`, `SimpleVerticalTabBarStyle` + 2 bases. TabBarItem styles (10): `SimpleBottomTabBarItemStyle`, `SimpleTopTabBarItemStyle`, `SimpleColoredTopTabBarItemStyle`, `SimpleVerticalTabBarItemStyle`, `SimpleNavigationTabBarItemStyle`, `SimpleFabTabBarItemStyle`, `SimpleBottomFabTabBarItemStyle` + 3 bases. Selection indicator uses PrimaryBrush. Active item: PrimaryBrush foreground. Inactive: OnSurfaceMediumBrush.
- [ ] T031 [US3] Update `_Common.xaml` — Add theme-agnostic aliases for all TabBar/TabBarItem styles.
- [ ] T032 [US3] Build and verify on `net9.0-desktop`

**Checkpoint**: Tab navigation works in all 4 orientations.

---

## Phase 5: User Story 4 — Divider (Priority: P2)

**Goal**: Simple line separator matching SDS  
**Independent Test**: Render horizontal and vertical dividers

### Implementation for User Story 4

- [ ] T040 [P] [US4] Create `Styles/Controls/Divider.xaml` — Read `Uno.Toolkit.Material/.../v2/Divider.xaml` as reference. Create 2 styles: `SimpleDividerStyle` (1px Rectangle, OutlineBrush fill), `SimpleDefaultDividerStyle` (BasedOn SimpleDividerStyle, used for implicit). ThemeDictionaries with `DividerBackground` key → `OutlineBrush`. SubHeader foreground → `OnSurfaceMediumBrush`.
- [ ] T041 [US4] Update `_Common.xaml` — Add implicit `<Style TargetType="utu:Divider" BasedOn="{StaticResource SimpleDefaultDividerStyle}" />` and `DividerStyle` alias.
- [ ] T042 [US4] Build and verify on `net9.0-desktop`

**Checkpoint**: Dividers render as 1px lines.

---

## Phase 6: User Story 5 — NavigationBar (Priority: P2)

**Goal**: Top app bar with back button, title, and action commands  
**Independent Test**: Render NavigationBar with title, back button, and primary action

### Implementation for User Story 5

- [ ] T050 [US5] Create `Styles/Controls/NavigationBar.xaml` — Read `Uno.Toolkit.Material/.../v2/NavigationBar.xaml` as reference. Create 13 styles: 6 consumer (`SimpleNavigationBarStyle`, `SimpleModalNavigationBarStyle`, `SimplePrimaryNavigationBarStyle`, `SimplePrimaryModalNavigationBarStyle` + 2 defaults) + 7 internal (AppBarButton: `SimpleMainCommandStyle`, `SimpleModalMainCommandStyle`, `SimplePrimaryMainCommandStyle`, `SimplePrimaryModalMainCommandStyle`, `SimplePrimaryAppBarButtonStyle`, `SimpleDefaultMainCommandStyle`, `SimpleDefaultNavigationBarStyle`). Include NavigationBarPresenter and CommandBar internal styles. Background: SurfaceBrush. Primary variant: PrimaryBrush background.
- [ ] T051 [US5] Update `_Common.xaml` — Add implicit styles for `utu:NavigationBar` and `AppBarButton`, plus all theme-agnostic aliases.
- [ ] T052 [US5] Build and verify on `net9.0-desktop`

**Checkpoint**: NavigationBar renders with back button, title, and actions.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Finalize _Common.xaml, validate all builds, add samples

- [ ] T060 [POLISH] Final `_Common.xaml` audit — Verify ALL consumer-facing styles have theme-agnostic aliases. Compare alias count with Material's `_Common.xaml` to ensure parity.
- [ ] T061 [P] [POLISH] Lightweight styling validation — Create a test page that overrides at least one key per control family (`FilledCardContentBackground`, `ChipCornerRadius`, `DividerBackground`, etc.) and verify the override takes effect.
- [ ] T062 [P] [POLISH] Full build validation — Run `dotnet build` on ALL target frameworks: `net9.0-desktop`, `net9.0-browserwasm`, `net9.0-android`, `net9.0-ios`
- [ ] T063 [POLISH] Create sample pages — One page per control family showing all variants with `<smtx:XamlDisplay>` wrappers (following SimpleSamplesApp pattern from Uno.Themes).
- [ ] T064 [POLISH] Update NuGet package metadata — Description, tags, icon for `Uno.Toolkit.WinUI.Simple` package.
- [ ] T065 [POLISH] Final style count audit — Verify ~45 consumer-facing + ~20 internal styles. Document final count in a table.

**Checkpoint**: Library is production-ready. All builds pass. Samples render.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: No dependencies — start immediately
- **Phase 2 (US1 Cards)**: Depends on Phase 1 ⬜ — First control validates pipeline
- **Phase 3 (US2 Chips)**: Depends on Phase 1 ✅ only — parallel with US1
- **Phase 4 (US3 TabBar)**: Depends on Phase 1 ✅ only — parallel with US1/US2
- **Phase 5 (US4 Divider)**: Depends on Phase 1 ✅ only — parallel
- **Phase 6 (US5 NavBar)**: Depends on Phase 1 ✅ only — parallel
- **Phase 7 (Polish)**: Depends on ALL user stories

### Critical Path

```
T001 → T002 → T004 → T006 → T008 (Setup validated)
  → T010 → T011 → T012 → T013 (Cards validated = MVP)
    → T020+T021 → T022 → T023 (Chips)
    → T030 → T031 → T032 (TabBar)
    → T040 → T041 → T042 (Divider)
    → T050 → T051 → T052 (NavBar)
      → T060 → T062 (Polish)
```

### Parallel Opportunities

Once Setup (Phase 1) completes, ALL user stories (Phases 2-6) can proceed in
parallel since each creates independent XAML files. The only shared file is
`_Common.xaml`, which each story appends to (non-conflicting additions).

---

## Implementation Strategy

### For a Single Agent

1. Complete Phase 1 (Setup) — validate build
2. Complete Phase 2 (Cards) — validate build (**MVP**)
3. Complete Phase 5 (Divider) — simplest control, quick win
4. Complete Phase 3 (Chips) — second most common control
5. Complete Phase 4 (TabBar) — navigation controls
6. Complete Phase 6 (NavBar) — most complex control
7. Complete Phase 7 (Polish)

### Key Implementation Rule

**ALWAYS read the corresponding Material XAML file FIRST** before creating the
Simple version. The Material file is the authoritative template. Then:

1. Copy the Material file
2. Replace `Material` → `Simple` in all style names
3. Replace Material design tokens with SDS tokens (see spec.md token table)
4. Remove `um:Ripple` elements → replace with opacity overlay `<Border>`
5. Replace `toolkit:ElevatedView` → `ThemeShadow` on Grid wrapper
6. Adjust CornerRadius, Padding, etc. per SDS token table
7. Remove `v2/` from folder path (no versioning)
8. Update ThemeDictionaries to use SDS brush resource keys

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story adds ONE XAML file (except US1 which adds 2)
- All stories share `_Common.xaml` but with non-conflicting additions
- Commit after each checkpoint
- Reference: `D:\source\uno.toolkit.ui\src\library\Uno.Toolkit.Material\`
