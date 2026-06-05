# Color overrides no longer reach StaticResource brushes — #1679 regression

**Status:** Root cause proven. A first fix is implemented in `BaseTheme.UpdateSource` but is **insufficient** (resource-precedence bug). Work in progress; instrumentation present and must be stripped before PR.

**Repo:** Uno.Themes. **Branch at handoff:** changes are uncommitted on `master` (move to a feature branch).

---

## Symptom

A consumer applies a **color-only** override (via `BaseTheme.ColorOverrideSource` / `ThemeColors.OverrideSource`, i.e. a `ThemeColors.xaml` whose theme dictionaries define `<Color>` entries such as `PrimaryColor`, `SurfaceColor`, …). The overridden **colors** take effect, but the **rendered controls do not change** — backgrounds/text/buttons keep the base palette. Only UI that reads a `Color` directly changes.

Reproduced with the **Simple** theme (`SimpleTheme` / a toolkit `SimpleToolkitTheme` subclass) hosted by a downstream consumer. The defect is **not** hosting-specific — it is a layering regression in `BaseTheme`.

## Why (mechanism)

Semantic brushes are defined (correctly, per WinUI guidance) inside `ThemeDictionaries` using `{StaticResource}`:

```xml
<!-- Styles/Applications/Common/SharedColors.xaml -->
<SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}" />
```

Controls bind `{ThemeResource PrimaryBrush}` → `PrimaryBrush` → `{StaticResource PrimaryColor}`. For an override to change the rendered brush, the brush's `{StaticResource PrimaryColor}` must resolve in a scope where the **overridden** `PrimaryColor` is visible / wins.

### Before #1679 (worked)

`BaseTheme.UpdateSource()` built a single `colors` dictionary that **owned the brushes** and had palette + overrides merged into it:

```csharp
var colors = new ResourceDictionary { Source = SharedColors.xaml };          // brushes
colors.MergedDictionaries.Add(new RD { Source = SharedColorPalette.xaml });  // base colors
if (_baseColorOverride) colors.SafeMerge(_baseColorOverride);                 // e.g. Simple grayscale
if (seed)               colors.SafeMerge(seedPalette);
if (userOverride)       colors.SafeMerge(userOverride);                       // the override
// colors was then the SOLE color/brush source on the theme (no Source-baked base)
```

The brushes' `{StaticResource PrimaryColor}` resolved to the override. `SimpleTheme` supplied its grayscale palette as `_baseColorOverride`. (#1679 also removed the `_originalBrushes`/`CollectBrushes`/`UpdateOldBrushes` runtime brush-propagation fallback.)

### After #1679 (broken) — commit `4a715e41`

The colors+brushes were moved into an **immutable base layer** loaded once via `Source` (`DefaultStylesSource` → `mergedpages` → `BaseDictionaries.xaml` → `SharedColorPalette` + `SharedColors` + theme grayscale), CopyFrom'd into the theme's **own** content. Overrides are now added as **sibling merged dictionaries**:

```csharp
Source = new Uri(DefaultStylesSource);                              // base brushes+colors, immutable
AddThemeDictionary(seedPalette);                                   // sibling
AddThemeDictionary(new ResourceDictionary { Source = override });  // sibling — NOT in the brushes' scope
```

The brushes resolve `{StaticResource PrimaryColor}` against the base at load → base color. The sibling override is never searched by that `StaticResource` → **brushes keep base colors.**

### Proof

Instrumented probe in `UpdateSource` resolving Color vs Brush after a color-only override:

```
theme resolves 'PrimaryColor' = #FFFF6EC7              (override applied)
theme resolves 'PrimaryBrush' = SolidColorBrush #FFF5F5F5   (BASE — not override)
```

Control: when the override `ThemeColors.xaml` *also* contained the brushes (`<SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}"/>` in the same theme dictionary), the brushes resolved to the override — confirming the brush mechanism is fine; only the layering is wrong.

---

## Fix attempt 1 (IMPLEMENTED, INSUFFICIENT)

`BaseTheme.UpdateSource()` now rebuilds a dynamic color+brush layer (restoring the pre-#1679 `colors` shape) when there is any color customization, and adds a `ColorPaletteSource` hook so themes contribute their base palette (Simple → grayscale):

```csharp
var effectivePrimary = Colors?.PrimarySeed ?? DefaultPrimarySeed;
var seedPalette = effectivePrimary is { } seed ? SeedColorPaletteGenerator.Default.Generate(...) : null;
var userOverride = Colors?.OverrideDictionary;
if (_baseColorOverride is not null || seedPalette is not null || userOverride is not null)
{
    var colors = new ResourceDictionary { Source = new Uri(ThemesConstants.SharedColorsResourcePath) }; // brushes
    colors.MergedDictionaries.Add(new RD { Source = new Uri(ThemesConstants.SharedColorPaletteResourcePath) });
    if (ColorPaletteSource is { } cps) colors.MergedDictionaries.Add(new RD { Source = new Uri(cps) });
    if (_baseColorOverride is { } b) colors.SafeMerge(b);
    if (seedPalette is not null)     colors.SafeMerge(seedPalette);
    if (userOverride is not null)    colors.SafeMerge(userOverride.Source is { } s ? new RD { Source = s } : userOverride);
    AddThemeDictionary(colors);
}
```

- `BaseTheme`: added `protected virtual string ColorPaletteSource => null;`
- `SimpleTheme`: `protected override string ColorPaletteSource => SimpleConstants.ResourcePaths.ColorPalette;`
- `SafeMerge` is `MergedDictionaries.Add` on Uno (`HAS_UNO`); `.Add(nested.Duplicate())` only on WinAppSDK.

### Why it is still insufficient — precedence

A `ResourceDictionary`'s **own content out-ranks its `MergedDictionaries`**. After `Source = DefaultStylesSource` CopyFrom, the base **brushes** are in the theme's **own** `ThemeDictionaries`, while the base **colors** are in *merged* dictionaries. So:

- override **color** wins (merged-vs-merged, dynamic layer added later), but
- base **brush** wins (own-content beats the dynamic merged layer).

Validated: with the fix loaded (`built dynamic colour layer … themePalette=True`), `PrimaryColor=#FFFF6EC7` but `PrimaryBrush=#FFF5F5F5` still. The dynamic `colors` layer cannot out-rank the baked base brushes (pre-#1679 there was no baked base, so it could).

---

## Remaining options (pick one)

1. **De-bake the color/brush layer from the immutable base** so the dynamic layer is again the sole source (true revert of #1679's color path). Remove `SharedColorPalette`/`SharedColors`/grayscale from whatever the active theme's `DefaultStylesSource`/`mergedpages` bakes in (note: a toolkit subclass may override `DefaultStylesSource` to a toolkit `mergedpages` that bakes them — that file must be handled too).
2. **Inject the override brushes into the theme's OWN `ThemeDictionaries`** (not `MergedDictionaries`) so they out-rank the baked base brushes; track for removal each rebuild like `_dynamicDictionaries`. Self-contained in `BaseTheme`.

Either way, keep `{StaticResource}` (never `{ThemeResource}`) inside theme dictionaries.

---

## Files changed (uncommitted)

- `src/library/Uno.Themes/BaseTheme.cs` — `UpdateSource` dynamic color layer + `ColorPaletteSource` virtual + `[STEVE-HR]` probe (`HrProbe`, `HrDumpOverride`).
- `src/library/Uno.Themes/BaseTheme.HotReload.cs` — `[STEVE-HR]` logging in `UpdateApplication`/`RegisterInstance`.
- `src/library/Uno.Simple.WinUI/SimpleTheme.cs` — `ColorPaletteSource` override.
- `Uno.CrossTargeting.props` — local override config churn (review before commit).

## Before PR

- Strip all `[STEVE-HR]` `Console.WriteLine` instrumentation.
- Move to a feature branch (currently uncommitted on `master`).
- Add a runtime regression test under `src/samples/SimpleSampleApp/RuntimeTests/` (extend `Given_ColorOverridePrecedence`) asserting a color-only override changes the **rendered brush** (`PrimaryBrush`/etc.), under Light and Dark.
- Validate via a real consumer that controls visibly retheme (not just the resolved-value probe).

## Validation harness used

The end-to-end repro/validation was driven from a downstream consumer's runtime-test harness (color-only override on a freshly-loaded app, probing `theme resolves 'PrimaryBrush'` == override). When validating here, the equivalent is a `Given_*` runtime test that applies `Colors.OverrideSource` and asserts the rendered brush color.

## References

- Regression: PR #1679 (commit `4a715e41`, "Streamline theme resource management and add hot-reload support").
- WinUI guidance (StaticResource inside ThemeDictionaries): <https://learn.microsoft.com/en-us/windows/apps/develop/platform/xaml/xaml-theme-resources#guidelines-for-custom-theme-resources>
