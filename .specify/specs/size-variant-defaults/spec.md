# Feature Specification: Configurable Default Size Variants

**Library**: `Uno.Simple.WinUI`
**Source Path**: `src/library/Uno.Simple.WinUI/`
**Reference**: [XAML Styles – Aliasing control resources to new names](https://learn.microsoft.com/en-us/windows/apps/develop/platform/xaml/xaml-styles#aliasing-control-resources-to-new-names)

## Context

The Simple Design System defines two size variants for interactive controls:
**Small** (compact, 32 px height) and **Medium** (standard, 40 px height).
Today the "default" (non-prefixed) styles are hard-wired to Medium sizing.
Consumers who want the Small variant must apply an explicit `SimpleSmall*`
style on every control instance.

This specification describes a refactor that:

1. Makes the **default size configurable** via a property on `SimpleTheme`.
2. Sets the default to **Small** for the Simple Theme.
3. Uses `StaticResource` aliasing to implement the indirection.
4. Introduces explicit **Medium variant styles** that were previously missing.
5. Preserves all existing explicit Small variant styles and their resource keys.

---

## Consumer API

```xml
<!-- Default: Small (no extra configuration needed) -->
<SimpleTheme xmlns="using:Uno.Simple" />

<!-- Switch to Medium default -->
<SimpleTheme xmlns="using:Uno.Simple" DefaultSize="Medium" />
```

The `DefaultSize` property is a `DependencyProperty` of type `SimpleControlSize`
(enum with values `Small`, `Medium`). It follows the existing BaseTheme pattern
used by `ColorOverrideSource` and `FontOverrideSource` — the property change
callback triggers `UpdateSource()` to rebuild the resource tree.

---

## Design

### Resource Aliasing Pattern

Use the WinUI `<StaticResource x:Key="..." ResourceKey="..." />` pattern to
create an indirection layer between concrete size values and the keys consumed
by base styles.

```
Concrete values (defined in Button.xaml, always available):
  SimpleButtonSmallMinHeight  = 32      ← unchanged
  SimpleButtonMediumMinHeight = 40      ← renamed from SimpleButtonMinHeight

Generic alias (defined in size defaults file, loaded by SimpleTheme):
  SimpleButtonMinHeight ──→ SimpleButtonSmallMinHeight   (when DefaultSize=Small)
  SimpleButtonMinHeight ──→ SimpleButtonMediumMinHeight  (when DefaultSize=Medium)

Base style reads the generic key:
  SimpleBaseButtonStyle { MinHeight = {StaticResource SimpleButtonMinHeight} }
```

### Size Defaults Files

Two new XAML resource dictionaries, excluded from XamlMerge, loaded
conditionally by `SimpleTheme.cs`:

- `Styles/Application/Common/SizeDefaults.Small.xaml` — aliases generic keys → Small
- `Styles/Application/Common/SizeDefaults.Medium.xaml` — aliases generic keys → Medium

`SimpleTheme.GenerateSpecificResources()` loads the appropriate file based on
`DefaultSize` and merges it into the resource tree:

```
mergedPages (from mergedpages.xaml — concrete values, styles, templates)
  └── MergedDictionaries
      ├── sizeDefaults (aliases from chosen SizeDefaults file)
      └── fonts
```

### Resource Key Inventory

| Generic Key (alias)           | Small Target                      | Medium Target (renamed)            |
|-------------------------------|-----------------------------------|------------------------------------|
| `SimpleButtonMinHeight`       | `SimpleButtonSmallMinHeight` (32) | `SimpleButtonMediumMinHeight` (40) |
| `SimpleButtonPadding`         | `SimpleButtonSmallPadding` (8)    | `SimpleButtonMediumPadding` (12)   |
| `SimpleButtonFontSize`        | `SimpleButtonSmallFontSize` (14)  | `SimpleButtonMediumFontSize` (16)  |
| `SimpleIconButtonMinSize`     | `SimpleIconButtonSmallMinSize` (32) | `SimpleIconButtonMediumMinSize` (40) |
| `SimpleIconButtonPadding`     | `SimpleIconButtonSmallPadding` (4)  | `SimpleIconButtonMediumPadding` (8)  |
| `SimpleToggleButtonMinHeight` | `SimpleToggleButtonSmallMinHeight` (32) | `SimpleToggleButtonMediumMinHeight` (40) |
| `SimpleToggleButtonPadding`   | `SimpleToggleButtonSmallPadding` (8)    | `SimpleToggleButtonMediumPadding` (12)   |
| `SimpleToggleButtonFontSize`  | `SimpleToggleButtonSmallFontSize` (14)  | `SimpleToggleButtonMediumFontSize` (16)  |

### Style Inventory

After refactoring, each control family exposes three tiers of styles:

| Tier | Example Key | Sizing |
|------|-------------|--------|
| **Default** (follows alias) | `SimplePrimaryButtonStyle` | Default (Small unless overridden) |
| **Explicit Small** | `SimpleSmallPrimaryButtonStyle` | Always Small |
| **Explicit Medium** (new) | `SimpleMediumPrimaryButtonStyle` | Always Medium |

New **Medium** styles to add:

**Button** (5):
- `SimpleMediumPrimaryButtonStyle`
- `SimpleMediumNeutralButtonStyle`
- `SimpleMediumSubtleButtonStyle`
- `SimpleMediumDangerPrimaryButtonStyle`
- `SimpleMediumDangerSubtleButtonStyle`

**IconButton** (5):
- `SimpleMediumIconButtonPrimaryStyle`
- `SimpleMediumIconButtonNeutralStyle`
- `SimpleMediumIconButtonSubtleStyle`
- `SimpleMediumIconButtonDangerPrimaryStyle`
- `SimpleMediumIconButtonDangerSubtleStyle`

**ToggleButton** (1):
- `SimpleMediumToggleButtonStyle`

### Theme-Agnostic Aliases (`_Resources.xaml`)

New entries to add alongside existing `Small*` aliases:

```xml
<!-- Button: Medium size variants -->
<StaticResource x:Key="MediumPrimaryButtonStyle"       ResourceKey="SimpleMediumPrimaryButtonStyle" />
<StaticResource x:Key="MediumNeutralButtonStyle"        ResourceKey="SimpleMediumNeutralButtonStyle" />
<StaticResource x:Key="MediumSubtleButtonStyle"         ResourceKey="SimpleMediumSubtleButtonStyle" />
<StaticResource x:Key="MediumDangerPrimaryButtonStyle"  ResourceKey="SimpleMediumDangerPrimaryButtonStyle" />
<StaticResource x:Key="MediumDangerSubtleButtonStyle"   ResourceKey="SimpleMediumDangerSubtleButtonStyle" />

<!-- IconButton: Medium size variants -->
<StaticResource x:Key="MediumIconButtonPrimaryStyle"       ResourceKey="SimpleMediumIconButtonPrimaryStyle" />
<StaticResource x:Key="MediumIconButtonNeutralStyle"        ResourceKey="SimpleMediumIconButtonNeutralStyle" />
<StaticResource x:Key="MediumIconButtonSubtleStyle"         ResourceKey="SimpleMediumIconButtonSubtleStyle" />
<StaticResource x:Key="MediumIconButtonDangerPrimaryStyle"  ResourceKey="SimpleMediumIconButtonDangerPrimaryStyle" />
<StaticResource x:Key="MediumIconButtonDangerSubtleStyle"   ResourceKey="SimpleMediumIconButtonDangerSubtleStyle" />

<!-- ToggleButton: Medium size variant -->
<StaticResource x:Key="MediumToggleButtonStyle" ResourceKey="SimpleMediumToggleButtonStyle" />
```

### SimpleTheme.cs Changes

```csharp
public enum SimpleControlSize { Small, Medium }

public class SimpleTheme : BaseTheme
{
    public static readonly DependencyProperty DefaultSizeProperty =
        DependencyProperty.Register(
            nameof(DefaultSize), typeof(SimpleControlSize), typeof(SimpleTheme),
            new PropertyMetadata(SimpleControlSize.Small, OnDefaultSizeChanged));

    public SimpleControlSize DefaultSize
    {
        get => (SimpleControlSize)GetValue(DefaultSizeProperty);
        set => SetValue(DefaultSizeProperty, value);
    }

    private static void OnDefaultSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((SimpleTheme)d).UpdateSource();

    protected override ResourceDictionary GenerateSpecificResources()
    {
        var mergedPages = new ResourceDictionary { Source = ... };

        // Load size defaults based on DefaultSize property
        var sizeDefaultsPath = DefaultSize == SimpleControlSize.Medium
            ? SimpleConstants.ResourcePaths.Common.SizeMediumDefaults
            : SimpleConstants.ResourcePaths.Common.SizeSmallDefaults;
        var sizeDefaults = new ResourceDictionary { Source = new Uri(sizeDefaultsPath) };
        mergedPages.MergedDictionaries.Add(sizeDefaults);

        // ... fonts, etc.
        return mergedPages;
    }
}
```

---

## Files Changed

| File | Change |
|------|--------|
| `SimpleTheme.cs` | Add `DefaultSize` DependencyProperty, `SimpleControlSize` enum, load appropriate size defaults file |
| `SimpleConstants.cs` | Add resource paths for `SizeDefaults.Small.xaml` and `SizeDefaults.Medium.xaml` |
| `simple-common.props` | Exclude new SizeDefaults files from XamlMerge |
| `Styles/Application/Common/SizeDefaults.Small.xaml` | **New** — aliases generic keys → Small resources |
| `Styles/Application/Common/SizeDefaults.Medium.xaml` | **New** — aliases generic keys → Medium resources |
| `Styles/Controls/Button.xaml` | Rename Medium resources to explicit `Medium` suffix, remove generic keys (moved to SizeDefaults files), add 10 Medium variant styles |
| `Styles/Controls/ToggleButton.xaml` | Rename Medium resources, remove generic keys, add 1 Medium style |
| `Styles/Controls/_Resources.xaml` | Add `Medium*` theme-agnostic style aliases |
| `Styles/Application/Common/Thickness.xaml` | Rename for naming consistency (unused at runtime) |

---

## Behavioral Notes

- Existing consumers using `SimplePrimaryButtonStyle` will see a visual change
  from Medium → Small sizing. This is intentional — Small is the new default.
- Consumers using `SimpleSmallPrimaryButtonStyle` see no change (still Small).
- Consumers wanting Medium default set `<SimpleTheme DefaultSize="Medium" />`.
- The implicit `<Style TargetType="Button" BasedOn="SimplePrimaryButtonStyle"/>`
  in `_Resources.xaml` automatically picks up the configured default sizing.
- The `DefaultSize` DependencyProperty follows the same change-callback pattern
  as `ColorOverrideSource` and `FontOverrideSource` in BaseTheme — setting it
  triggers `UpdateSource()` to rebuild the resource tree.
