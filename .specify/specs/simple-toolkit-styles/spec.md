# Feature Specification: Simple Design System Styles for Uno.Toolkit Controls

**Feature Branch**: `feature/simple-toolkit-styles`  
**Created**: 2026-02-16  
**Status**: Draft  
**Input**: User description: "Add SDS-themed styles for ALL Toolkit controls
that Material also covers, creating parity between `Uno.Toolkit.Material` and
a new `Uno.Toolkit.Simple` library."

## Context

Material's Toolkit companion library (`Uno.Toolkit.Material`) provides **~48
consumer-facing style keys** across 6 control families: Card,
CardContentControl, Chip, ChipGroup, Divider, NavigationBar, and
TabBar/TabBarItem.

The Simple Design System currently has **zero** Toolkit styles. The goal is
full parity. The SDS Figma defines equivalent components (Card, Tag/TagToggle →
Chip, Menu Separator → Divider, Navigation Pill → TabBar, Header → NavBar)
that map to these Toolkit controls.

A previous attempt placed Card styles in `Uno.Themes` as `Border` styles. This
was rejected — Toolkit control styles belong in a **Toolkit companion library**
(Constitution Principle V).

### What's Already Done (Uno.Themes — WinUI Built-in Controls)

These live in `Uno.Simple.WinUI` and are ✅ complete:

| Control | Style Count | Status |
|---------|-------------|--------|
| Button | 18 | ✅ |
| TextBox | 3 | ✅ |
| ComboBox | 3 | ✅ |
| CheckBox | 1 | ✅ |
| RadioButton | 1 | ✅ |
| ToggleSwitch | 1 | ✅ |
| PersonPicture | 6 | ✅ |
| **Total** | **33** | |

### What's Missing (Toolkit Controls — This Spec)

| Toolkit Control | Material Style Count | SDS Figma Equivalent | Priority |
|-----------------|---------------------|---------------------|----------|
| CardContentControl | 3 (+1 base) | Card (Default/Stroke) | P1 |
| Card | 9 (+3 bases) | Card (Default/Stroke/Elevated × layout) | P1 |
| Chip | 8 (+2 internal) | Tag / Tag Toggle | P1 |
| ChipGroup | 8 (+1 base) | *(container for Tags)* | P2 |
| Divider | 1 (+1 alias) | Menu Separator | P2 |
| NavigationBar | 6 (+7 internal) | Header / Navigation buttons | P2 |
| TabBar | 4 (+2 bases) | Navigation Pill List / Tab | P1 |
| TabBarItem | 7 (+3 bases) | Navigation Pill / Tab item | P1 |

**Controls that DON'T need styles** (layout/behavior only, no visual chrome):
AutoLayout, DrawerControl, DrawerFlyoutPresenter, ExtendedSplashScreen,
LoadingView, ResponsiveView, SafeArea, ZoomContentControl.

---

## User Scenarios & Testing

### User Story 1 — Project Setup & Card Styles (Priority: P1)

As an app developer using SDS, I want `CardContentControl` and `Card` styled
with SDS tokens (filled, outlined, elevated) so my cards match the design system.

**Why this priority**: Cards are the most commonly used Toolkit control. Project
setup is a prerequisite for everything else.

**Independent Test**: Add `<SimpleToolkitTheme />` to App.xaml. Create a page
with 3 `CardContentControl` + 3 `Card` instances. Build and render on desktop.

**Acceptance Scenarios**:

1. **Given** a new Uno app references `Uno.Toolkit.WinUI.Simple`,
   **When** `<SimpleToolkitTheme />` is added to App.xaml,
   **Then** the project builds successfully on all target frameworks.
2. **Given** `Style="{StaticResource SimpleFilledCardContentControlStyle}"`,
   **When** rendered, **Then** white surface, 8px radius, no border, 24px padding.
3. **Given** `Style="{StaticResource SimpleOutlinedCardContentControlStyle}"`,
   **When** rendered, **Then** 1px `OutlineBrush` border added.
4. **Given** `Style="{StaticResource SimpleElevatedCardContentControlStyle}"`,
   **When** rendered, **Then** ThemeShadow at Z=8, no border.
5. **Given** `IsClickable="True"`, **When** hover, **Then** subtle opacity overlay.
6. **Given** `Card` with `HeaderContent` + `SupportingContent` + `MediaContent`,
   **When** rendered, **Then** all regions display with SDS spacing (24/16/8px).

---

### User Story 2 — Chip & ChipGroup (Priority: P1)

As an app developer using SDS, I want Tag-style chips so I can display labels,
filters, and toggleable tags matching the SDS design.

**Why this priority**: Chips are the second most commonly used Toolkit control
and the SDS Figma has a well-defined Tag component.

**Independent Test**: Render chips with `SimpleChipStyle`, `SimpleFilterChipStyle`,
`SimpleInputChipStyle`. Toggle and remove chips to verify behavior.

**Acceptance Scenarios**:

1. **Given** `Style="{StaticResource SimpleChipStyle}"`, **When** rendered,
   **Then** shows a pill-shaped (999px radius) label with SDS neutral colors.
2. **Given** `Style="{StaticResource SimpleFilterChipStyle}"` with `IsCheckable`,
   **When** toggled, **Then** checked state shows filled `PrimaryBrush` background.
3. **Given** `Style="{StaticResource SimpleInputChipStyle}"` with `CanRemove`,
   **When** remove icon clicked, **Then** chip is removed.
4. **Given** a `ChipGroup` with `SimpleFilterChipGroupStyle`, **When** multiple
   chips are selected, **Then** selection state is tracked correctly.

---

### User Story 3 — TabBar & TabBarItem (Priority: P1)

As an app developer using SDS, I want navigation tabs/pills styled with SDS
tokens for top, bottom, and vertical navigation patterns.

**Why this priority**: Navigation is a core UX concern and SDS defines a
Navigation Pill component specifically for this.

**Independent Test**: Render a `TabBar` with 4 items in bottom, top, and vertical
orientations. Verify selection indicator, active/inactive colors.

**Acceptance Scenarios**:

1. **Given** `Style="{StaticResource SimpleBottomTabBarStyle}"`, **When** rendered,
   **Then** shows a horizontal bar with icon+label items.
2. **Given** `Style="{StaticResource SimpleTopTabBarStyle}"`, **When** rendered,
   **Then** shows a horizontal tab strip at the top.
3. **Given** a `TabBarItem` is selected, **When** rendered, **Then** uses
   `PrimaryBrush` foreground and shows selection indicator.
4. **Given** `Style="{StaticResource SimpleVerticalTabBarStyle}"`, **When** rendered,
   **Then** shows a vertical navigation rail.

---

### User Story 4 — Divider (Priority: P2)

As an app developer, I want a styled `Divider` control matching SDS separators.

**Why this priority**: Divider is the simplest Toolkit control and a
low-effort, high-value addition.

**Independent Test**: Render horizontal and vertical dividers. Verify 1px height.

**Acceptance Scenarios**:

1. **Given** `Style="{StaticResource SimpleDividerStyle}"`, **When** rendered,
   **Then** shows a 1px line using `OutlineBrush`.
2. **Given** a Divider with `SubHeader`, **When** rendered, **Then** shows a
   label centered on the divider line.

---

### User Story 5 — NavigationBar (Priority: P2)

As an app developer, I want a top app bar / navigation bar styled with SDS
tokens, with back button, title, and action commands.

**Why this priority**: NavigationBar is the most complex Toolkit control with
many internal styles. Lower priority due to scope.

**Independent Test**: Render a `NavigationBar` with title, back button, and
primary action. Verify layout and colors.

**Acceptance Scenarios**:

1. **Given** `Style="{StaticResource SimpleNavigationBarStyle}"`, **When** rendered,
   **Then** shows a top bar with `SurfaceBrush` background.
2. **Given** `MainCommand` is set, **When** rendered, **Then** a back arrow icon
   appears on the leading edge.
3. **Given** `Style="{StaticResource SimpleModalNavigationBarStyle}"`, **When**
   rendered, **Then** shows a close (X) icon instead of back arrow.

---

### User Story 6 — Lightweight Styling Override (Priority: P3)

As an app developer, I want to override individual properties via resource keys.

**Why this priority**: Quality-of-life for advanced consumers. Not blocking.

**Acceptance Scenarios**:

1. **Given** `FilledCardContentBackground` is overridden in app resources,
   **When** a filled card renders, **Then** it uses the overridden color.
2. **Given** `ChipCornerRadius` is overridden, **When** a chip renders, **Then**
   it uses the new corner radius.

---

### Edge Cases

- `CardContentControl` with `null` content → empty container at minimum size
- `IsEnabled="False"` on any control → `Opacity=0.4`
- Using SDS theme without Toolkit installed → build error (expected)
- `ChipGroup` with zero items → renders empty, no crash
- `TabBar` with single item → renders correctly, item selected by default

---

## Requirements

### Functional Requirements — Project Setup

- **FR-001**: A new library `Uno.Toolkit.WinUI.Simple` MUST be created under
  `uno.toolkit.ui/src/library/Uno.Toolkit.Simple/`, following `Uno.Toolkit.Material`
  project structure exactly.
- **FR-002**: The `.csproj` MUST use `MSBuild.Sdk.Extras/3.0.44` SDK and reference:
  - `Uno.Toolkit.WinUI` (ProjectReference — control types)
  - `Uno.Simple.WinUI` (PackageReference — SDS brushes/colors)
  - `Uno.WinUI` (PackageReference — Uno platform)
- **FR-003**: A `SimpleToolkitTheme.cs` entry point MUST extend `ResourceDictionary`
  and load styles via `MergedDictionaries`, following `MaterialToolkitTheme.cs`.
- **FR-004**: A `_Common.xaml` MUST define implicit styles + theme-agnostic aliases.
- **FR-005**: A `xamlmerge-simple.props` MUST configure XamlMerge to generate
  `mergedpages.WinUI.v1.xaml` from `Styles/Controls/**/*.xaml`.
- **FR-006**: Theme-agnostic aliases (`FilledCardStyle` → `SimpleFilledCardStyle`,
  etc.) MUST be registered in `_Common.xaml` using `<StaticResource>` alias pattern.

### Functional Requirements — CardContentControl

- **FR-010**: Three consumer styles + one base style:
  - `SimpleBaseCardContentControlStyle` — shared setters
  - `SimpleFilledCardContentControlStyle` — `BasedOn` base, white surface
  - `SimpleOutlinedCardContentControlStyle` — `BasedOn` filled, adds border
  - `SimpleElevatedCardContentControlStyle` — `BasedOn` base, adds ThemeShadow
- **FR-011**: Each variant MUST expose lightweight styling keys in ThemeDictionaries
  (both `Default` and `Light` keys with identical content):
  `{Variant}CardContentBackground`, `{Variant}CardContentBorderBrush`
- **FR-012**: VisualStates: `CommonStates` (Normal, PointerOver, Pressed,
  Disabled) + `FocusStates` (Focused, PointerFocused, Unfocused).
  Overlays use opacity transitions, NOT Material ripple.
- **FR-013**: Elevated variant uses `<ThemeShadow>` on a wrapper element with
  `Translation="0,0,8"`, NOT `toolkit:ElevatedView`.

### Functional Requirements — Card

- **FR-020**: Nine consumer styles (3 variants × 3 layouts) + 3 base styles:
  - `SimpleFilledCardStyle`, `SimpleOutlinedCardStyle`, `SimpleElevatedCardStyle`
  - `SimpleAvatarFilledCardStyle`, `SimpleAvatarOutlinedCardStyle`,
    `SimpleAvatarElevatedCardStyle`
  - `SimpleSmallMediaFilledCardStyle`, `SimpleSmallMediaOutlinedCardStyle`,
    `SimpleSmallMediaElevatedCardStyle`
- **FR-021**: Same lightweight styling key pattern as CardContentControl.

### Functional Requirements — Chip

- **FR-030**: Eight consumer styles + one base + one default:
  - `SimpleChipStyle` — base with full template
  - `SimpleAssistChipStyle`, `SimpleInputChipStyle`, `SimpleFilterChipStyle`,
    `SimpleSuggestionChipStyle` — flat variants with border
  - `SimpleElevatedAssistChipStyle`, `SimpleElevatedFilterChipStyle`,
    `SimpleElevatedSuggestionChipStyle` — elevated variants
  - `SimpleDefaultChipStyle` — empty BasedOn for implicit style target
- **FR-031**: Chip corner radius: `999` (pill shape per SDS Tag component).
- **FR-032**: Template MUST NOT use `um:Ripple`. Use opacity-based state overlays.
- **FR-033**: SDS color scheme resources (Brand, Neutral, Positive, Danger,
  Warning) available as lightweight styling overrides on chip background/foreground.

### Functional Requirements — ChipGroup

- **FR-040**: Seven consumer styles:
  `SimpleAssistChipGroupStyle`, `SimpleElevatedAssistChipGroupStyle`,
  `SimpleFilterChipGroupStyle`, `SimpleElevatedFilterChipGroupStyle`,
  `SimpleSuggestionChipGroupStyle`, `SimpleElevatedSuggestionChipGroupStyle`,
  `SimpleInputChipGroupStyle`
- **FR-041**: ChipGroup styles set `ItemContainerStyle` to the matching Chip style.

### Functional Requirements — Divider

- **FR-050**: `SimpleDividerStyle` — 1px Rectangle using `OutlineBrush`.
- **FR-051**: `SimpleDefaultDividerStyle` — empty BasedOn for implicit style.
- **FR-052**: SubHeader foreground uses `OnSurfaceMediumBrush`, typography uses
  SDS `BodySmall` scale.

### Functional Requirements — NavigationBar

- **FR-060**: Consumer styles:
  `SimpleNavigationBarStyle`, `SimpleModalNavigationBarStyle`,
  `SimplePrimaryNavigationBarStyle`, `SimplePrimaryModalNavigationBarStyle`
- **FR-061**: AppBarButton internal styles:
  `SimpleMainCommandStyle`, `SimpleModalMainCommandStyle`,
  `SimplePrimaryMainCommandStyle`, `SimplePrimaryModalMainCommandStyle`,
  `SimplePrimaryAppBarButtonStyle`
- **FR-062**: `SimpleDefaultNavigationBarStyle` — implicit style target.
- **FR-063**: `SimpleDefaultMainCommandStyle` — implicit AppBarButton style.
- **FR-064**: Internal styles for `NavigationBarPresenter` and `CommandBar`.

### Functional Requirements — TabBar / TabBarItem

- **FR-070**: TabBar consumer styles:
  `SimpleBottomTabBarStyle`, `SimpleTopTabBarStyle`,
  `SimpleColoredTopTabBarStyle`, `SimpleVerticalTabBarStyle`
- **FR-071**: TabBarItem consumer styles:
  `SimpleBottomTabBarItemStyle`, `SimpleTopTabBarItemStyle`,
  `SimpleColoredTopTabBarItemStyle`, `SimpleVerticalTabBarItemStyle`,
  `SimpleNavigationTabBarItemStyle`,
  `SimpleFabTabBarItemStyle`, `SimpleBottomFabTabBarItemStyle`
- **FR-072**: Selection indicator using `PrimaryBrush` for active state.
- **FR-073**: SDS Navigation Pill maps to `TabBarItem` — `6px` corner radius,
  `PrimaryBrush` active background, `OnSurfaceMediumBrush` inactive foreground.

### Key Entities

- **`CardContentControl`**: `ContentControl` wrapping content in a card surface.
  Properties: `Elevation`, `ShadowColor`, `IsClickable`.
- **`Card`**: Extends `CardContentControl` with structured regions:
  `HeaderContent`, `SubHeaderContent`, `AvatarContent`, `MediaContent`,
  `SupportingContent`, `IconsContent`.
- **`Chip`**: `ToggleButton`-based tag/label. Properties: `CanRemove`,
  `IsCheckable`, `Elevation`, `Icon`, `IconTemplate`.
- **`ChipGroup`**: `ItemsControl`-based container. Property: `CanRemove`,
  `CanReorder`, `SelectionMode`.
- **`Divider`**: Simple line separator. Properties: `SubHeader`, `SubHeaderForeground`.
- **`NavigationBar`**: Top app bar with `MainCommand`, `PrimaryCommands`, `Content`.
- **`TabBar`**: Navigation container. Properties: `SelectionIndicatorContent`,
  `SelectionIndicatorTransitionMode`.
- **`TabBarItem`**: Navigation item. Properties: `Icon`, `BadgeValue`, `IsSelected`.

---

## Reference Implementation: How `Uno.Toolkit.Material` Is Built

> **CRITICAL FOR AGENTS**: This section documents the exact patterns that MUST
> be replicated. Read `D:\source\uno.toolkit.ui\src\library\Uno.Toolkit.Material\`
> as the authoritative reference.

### Repository & File Layout

```
uno.toolkit.ui/src/library/Uno.Toolkit.Material/
├── Uno.Toolkit.WinUI.Material.csproj    ← MSBuild.Sdk.Extras/3.0.44
├── MaterialToolkitTheme.cs              ← Entry point (ResourceDictionary subclass)
├── MaterialToolkitResourcesV2.cs        ← V2 loader (deprecated)
├── MaterialToolkitResourcesV1.cs        ← V1 loader (deprecated)
├── MaterialToolkitResources.cs          ← Alias → V2
├── AssemblyInfo.cs
├── xamlmerge-material.props             ← XamlMerge config
├── build/Package.targets                ← Implicit usings for consumers
├── Styles/Controls/
│   └── v2/
│       ├── _Common.xaml                 ← Implicit styles + aliases
│       ├── Card.xaml
│       ├── CardContentControl.xaml
│       ├── Chip.xaml
│       ├── ChipGroup.xaml
│       ├── Divider.xaml
│       ├── NavigationBar.xaml
│       └── TabBar.xaml
└── Generated/
    └── mergedpages.WinUI.v2.xaml        ← Auto-generated merged output
```

### csproj Pattern

```xml
<Project Sdk="MSBuild.Sdk.Extras/3.0.44">
  <PropertyGroup>
    <TargetFrameworks>$(NetCurrentAll)</TargetFrameworks>
    <GenerateLibraryLayout>true</GenerateLibraryLayout>
    <DefineConstants>$(DefineConstants);IS_WINUI</DefineConstants>
    <AssemblyName>Uno.Toolkit.WinUI.Simple</AssemblyName>
    <RootNamespace>Uno.Toolkit.UI.Simple</RootNamespace>
    <PackageId>Uno.Toolkit.WinUI.Simple</PackageId>
    <FrameworkLineage>WinUI</FrameworkLineage>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Uno.Simple.WinUI" />
    <PackageReference Include="Uno.WinUI" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\Uno.Toolkit.UI\Uno.Toolkit.WinUI.csproj" />
  </ItemGroup>
  <Import Project="xamlmerge-simple.props" />
</Project>
```

### Entry Point Class Pattern (`SimpleToolkitTheme.cs`)

The class MUST extend `ResourceDictionary` and load THREE dictionaries in order:
1. `SimpleTheme` — the base theme (colors + fonts from `Uno.Simple.WinUI`)
2. Base toolkit styles from `Uno.Toolkit.WinUI` generated merged pages
3. Simple companion styles from this project's generated merged pages

```csharp
// Pattern from MaterialToolkitTheme.cs — adapt for Simple
namespace Uno.Toolkit.UI.Simple;

public partial class SimpleToolkitTheme : ResourceDictionary
{
    private const string PackageNameSuffix = "WinUI";
    private const string ToolkitPackageName = "Uno.Toolkit." + PackageNameSuffix;
    private const string ToolkitSimplePackageName =
        "Uno.Toolkit." + PackageNameSuffix + ".Simple";

    public SimpleToolkitTheme()
    {
        MergedDictionaries.Add(new Themes.SimpleTheme());
        MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(
                $"ms-appx:///{ToolkitPackageName}/Generated/mergedpages.{PackageNameSuffix}.xaml")
        });
        MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(
                $"ms-appx:///{ToolkitSimplePackageName}/Generated/mergedpages.{PackageNameSuffix}.v1.xaml")
        });
    }
}
```

### XamlMerge Props Pattern (`xamlmerge-simple.props`)

```xml
<Project>
  <PropertyGroup>
    <XamlMergeOutputFile>Generated\mergedpages.$(FrameworkLineage).v1.xaml</XamlMergeOutputFile>
  </PropertyGroup>
  <ItemGroup>
    <XamlMergeInput Include="Styles\Controls\**\*.xaml" />
  </ItemGroup>
  <Import Project="..\..\..\..\build\XamlMerge.props"
          Condition="Exists('..\..\..\..\build\XamlMerge.props')" />
</Project>
```

### `_Common.xaml` Pattern (Implicit Styles + Aliases)

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:utu="using:Uno.Toolkit.UI">

    <!-- Implicit styles (default for TargetType — no x:Key) -->
    <Style BasedOn="{StaticResource SimpleDefaultDividerStyle}"
           TargetType="utu:Divider" />
    <Style BasedOn="{StaticResource SimpleDefaultNavigationBarStyle}"
           TargetType="utu:NavigationBar" />
    <Style BasedOn="{StaticResource SimpleDefaultMainCommandStyle}"
           TargetType="AppBarButton" />
    <Style BasedOn="{StaticResource SimpleDefaultChipStyle}"
           TargetType="utu:Chip" />

    <!-- Theme-agnostic aliases (short names) -->
    <StaticResource x:Key="DividerStyle"
                    ResourceKey="SimpleDividerStyle" />
    <StaticResource x:Key="ChipStyle"
                    ResourceKey="SimpleChipStyle" />
    <StaticResource x:Key="FilledCardStyle"
                    ResourceKey="SimpleFilledCardStyle" />
    <StaticResource x:Key="OutlinedCardStyle"
                    ResourceKey="SimpleOutlinedCardStyle" />
    <StaticResource x:Key="ElevatedCardStyle"
                    ResourceKey="SimpleElevatedCardStyle" />
    <StaticResource x:Key="FilledCardContentControlStyle"
                    ResourceKey="SimpleFilledCardContentControlStyle" />
    <StaticResource x:Key="OutlinedCardContentControlStyle"
                    ResourceKey="SimpleOutlinedCardContentControlStyle" />
    <StaticResource x:Key="ElevatedCardContentControlStyle"
                    ResourceKey="SimpleElevatedCardContentControlStyle" />
    <!-- ... repeat for ALL consumer-facing styles ... -->
</ResourceDictionary>
```

### XAML Namespace Declarations (Every Style File)

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:utu="using:Uno.Toolkit.UI"
                    xmlns:toolkit="using:Uno.UI.Toolkit">
```

`um:` (Material Ripple namespace) is NOT used. Simple uses opacity overlays.

### Lightweight Styling Key Pattern (ThemeDictionaries)

Every style XAML file MUST start with:

```xml
<ResourceDictionary.ThemeDictionaries>
    <ResourceDictionary x:Key="Default">
        <StaticResource x:Key="FilledCardContentBackground"
                        ResourceKey="SurfaceBrush" />
        <StaticResource x:Key="FilledCardContentBorderBrush"
                        ResourceKey="SurfaceBrush" />
        <StaticResource x:Key="OutlinedCardContentBorderBrush"
                        ResourceKey="OutlineBrush" />
    </ResourceDictionary>
    <ResourceDictionary x:Key="Light">
        <!-- EXACT DUPLICATE of Default dictionary -->
        <StaticResource x:Key="FilledCardContentBackground"
                        ResourceKey="SurfaceBrush" />
        <StaticResource x:Key="FilledCardContentBorderBrush"
                        ResourceKey="SurfaceBrush" />
        <StaticResource x:Key="OutlinedCardContentBorderBrush"
                        ResourceKey="OutlineBrush" />
    </ResourceDictionary>
</ResourceDictionary.ThemeDictionaries>
```

Key naming: `{Variant}{ControlType}{Property}` or
`{Variant}{ControlType}{Property}{State}`.

### Style Inheritance Chain Pattern

```
SimpleBase{Control}Style (shared: CornerRadius, Alignment, MinHeight)
  ├── SimpleFilled{Control}Style (Background, BorderBrush, Template)
  │     └── SimpleOutlined{Control}Style (overrides BorderBrush, BorderThickness)
  └── SimpleElevated{Control}Style (adds ThemeShadow, different Template)
```

### VisualState Pattern (No Ripple)

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="CommonStates">
        <VisualState x:Name="Normal" />
        <VisualState x:Name="PointerOver">
            <Storyboard>
                <DoubleAnimation Storyboard.TargetName="HoverOverlay"
                                 Storyboard.TargetProperty="Opacity"
                                 Duration="0:0:0.25" To="1" />
            </Storyboard>
        </VisualState>
        <VisualState x:Name="Pressed">
            <Storyboard>
                <DoubleAnimation Storyboard.TargetName="PressedOverlay"
                                 Storyboard.TargetProperty="Opacity"
                                 Duration="0:0:0.1" To="1" />
            </Storyboard>
        </VisualState>
        <VisualState x:Name="Disabled">
            <VisualState.Setters>
                <Setter Target="GridRoot.Opacity" Value="0.4" />
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

### Key Differences From Material

| Aspect | Material | Simple |
|--------|----------|--------|
| Ripple | `um:Ripple` in Card/Chip | **NOT used** — opacity overlays |
| ElevatedView | `toolkit:ElevatedView` | **ThemeShadow** on Grid directly |
| Corner Radius | Cards=12, Chips=8 | Cards=8, Chips=999 (pill) |
| Padding | Cards=16 | Cards=24 |
| Elevation | Cards=6 | Cards=8 |
| Version folders | `v1/` + `v2/` | Single flat folder (no versioning) |
| Typography refs | `LabelLargeFontFamily` | SDS type scale keys |

---

## SDS Design Token Reference

### Shared Constants

| Resource Key | Value | Material Equivalent |
|-------------|-------|---------------------|
| `CardCornerRadius` | `8` | `12` |
| `CardPadding` | `24` | `16` |
| `CardBorderThickness` | `1` | `1` |
| `CardContentSpacing` | `16` | — |
| `CardTextAreaSpacing` | `8` | — |
| `CardElevation` | `8` | `6` |
| `CardMinWidth` | `240` | — |
| `CardMaxWidth` | `440` | `344` |
| `ChipCornerRadius` | `999` (pill) | `8` |
| `ChipHeight` | `32` | `32` |
| `ChipPadding` | `12,0` | `16,0` |
| `ChipBorderThickness` | `1` | `1` |
| `DividerHeight` | `1` | `1` |
| `NavigationBarHeight` | `56` | `64` |
| `TabBarHeight` | `56` | `80` (bottom) |
| `TabBarItemIconSize` | `24` | `24` |

### Color Mapping

All styles use existing SDS brush resources from `Uno.Simple.WinUI`:

| SDS Brush | Hex | Usage |
|-----------|-----|-------|
| `PrimaryBrush` | `#2c2c2c` | Active tab, selected chip, primary nav |
| `OnPrimaryBrush` | `#f5f5f5` | Text on primary surfaces |
| `SurfaceBrush` | `#ffffff` | Card/nav backgrounds |
| `SurfaceVariantBrush` | `#f5f5f5` | Chip backgrounds, inactive states |
| `OnSurfaceBrush` | `#1e1e1e` | Primary text, icons |
| `OnSurfaceMediumBrush` | `#757575` | Secondary text, inactive tabs |
| `OutlineBrush` | `#d9d9d9` | Borders, dividers, outlined cards |
| `OutlineVariantBrush` | `#e8e8e8` | Subtle borders |
| `ErrorBrush` | `#dc2626` | Danger chips, error states |

---

## Full Style Key Inventory

| Control | Style Count | Internal/Base | Consumer-Facing |
|---------|-------------|---------------|-----------------|
| CardContentControl | 4 | 1 base | 3 |
| Card | 12 | 3 bases | 9 |
| Chip | 10 | 2 internal | 8 |
| ChipGroup | 8 | 1 base | 7 |
| Divider | 2 | 1 default | 1 |
| NavigationBar | 13 | 7 internal | 6 |
| TabBar | 6 | 2 bases | 4 |
| TabBarItem | 10 | 3 bases | 7 |
| **Total** | **~65** | **~20** | **~45** |

---

## Success Criteria

- **SC-001**: All ~45 consumer-facing styles render correctly on desktop, WASM,
  Android, and iOS
- **SC-002**: Lightweight styling keys work — overriding any key changes the
  rendered appearance without re-templating
- **SC-003**: Hover, press, and focus states work correctly when `IsClickable`
  is true (cards, chips)
- **SC-004**: Selection states work correctly (chips, tab bar items)
- **SC-005**: Theme-agnostic aliases resolve correctly (e.g., `FilledCardStyle`
  → `SimpleFilledCardStyle`)
- **SC-006**: Sample pages demonstrate all styles with `XamlDisplay` code preview
- **SC-007**: `dotnet build` succeeds with zero errors on all target frameworks
- **SC-008**: Style count achieves parity with `Uno.Toolkit.Material` (~48 keys)

**Version**: 3.0.0 | **Last Updated**: 2026-02-16
