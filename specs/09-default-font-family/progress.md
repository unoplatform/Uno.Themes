# `DefaultFontFamily` on `BaseTheme` + font-override re-resolution (issue #1705)

> Numbered 09 because PR #1702 (live spacing & density sample) already claims `specs/08-…`.

Implements https://github.com/unoplatform/Uno.Themes/issues/1705 and the Uno.Themes half of
Hot Design's design-token editing (`unoplatform/uno.hotdesign#7871`, spec PR `#7895`): the
designer edits `BaseTheme` properties through its property grid, so the two root typefaces need
to be properties, the way the seed colours and the scale measures already are.

## Why the existing channels were not enough

- **No property seam.** The only way to move the root typeface token was
  `FontOverrideDictionary` — the *app author's* channel. Hot Design's proof of concept therefore
  cloned that dictionary, inserted the token and reassigned it, which conflates designer intent
  with the application's own declaration and drops the dictionary's `Source`.
- **An alias-only cascade is fragile.** The base layer's alias chain (every slot →
  `DefaultFontFamily`) now works on every theme — Simple's slot re-declarations, which used to break
  it, are gone. The generator still writes the slot keys directly rather than leaning on the
  aliases, so the seam does not depend on where a design system declares its slots or in which
  order its dictionaries merge (see the eager-alias-resolution trap in `specs/lessons.md`).
- **#1705.** `FontOverrideSource` was resolved to a dictionary **once**, in
  `OnFontOverrideSourceChanged`, and each concrete theme re-merged that same instance from
  `AddThemeSpecificResources()` on every rebuild. `Source` assignment *copies* the file's entries
  into the instance (`ResourceDictionary.CopyFrom`), so the instance is a load-time snapshot: a
  hot-reload edit to the override file refreshed the registered dictionary for that source but never
  the theme's copy. The colour override has re-resolved from its `Source` on every rebuild since the
  seed work; fonts were the one silent exception.

## Design

One `FontFamily` dependency property on `BaseTheme`, beside `DefaultCornerRadius` /
`DefaultSpacing` / `DefaultDensity` and named to match them:

- `DefaultFontFamily`, default `null` (= unset), change callback → `UpdateSource()`.
- Typed `FontFamily`, not `string`: Uno's XAML generator converts a literal by property *type*
  (`XamlFileGenerator`, `XamlConstants.Types.FontFamily`), so
  `DefaultFontFamily="ms-appx:///…#Font"` works as an attribute; and Hot Design's property grid
  selects its font editor — the suggest box sourced from the application's packaged fonts — off the
  property type alone, so the designer needs no bespoke editor for that row.

**One property, one token.** The property maps 1:1 onto the base layer's single
`DefaultFontFamily` token: a single family is the shape the common ask has ("use this font in my
app"), it matches the `Default*` measures next to it, and per-scale weight nuance stays in the
`*FontWeight` tokens. Changing only some scales stays expressible — and is tested — through the
font override dictionary, the same channel that already owns per-key precedence.

`GenerateFontFamilyScale(family)` (in `BaseTheme.ScaleGeneration.cs`, alongside the spacing and shape
generators) emits one dictionary with `Default`, `Light` and `HighContrast` theme dictionaries, each carrying the same
family under all 20 keys of `TypefaceScaleKeys`: the `DefaultFontFamily` root token and the 19
`*FontFamily` slot keys `SharedTypography.xaml` derives from it.

Generating the slot keys rather than relying on those aliases is what makes the seam uniform: it
does not depend on where a design system happens to declare its slot aliases or in which order its
dictionaries merge. Per-slot weight nuance (Simple's Display → Bold, Title → SemiBold) lives in the
`*FontWeight` tokens and is untouched by a family swap — the assigned family should resolve
multiple weights (a variable font, or a font with a font manifest; see the base layer's spec).

An unset family **generates no key at all** (not a null entry), so the design system's own
declarations stand untouched.

Merge order in `UpdateSource()` — later wins:

```text
spacing / shape / density
colours (+ semantic brushes, + colour override)
font family               <- generated layer, new
AddThemeSpecificResources()
font override             <- FontOverrideDictionary, re-resolved from Source
```

The font override is merged **last**, so a consumer file declaring `DefaultFontFamily` beats the
property — "override beats preset", the same precedence as `Colors.OverrideDictionary` over the
generated seed palette, and the rule `specs/lessons.md` already states for two sources of the same
value. Hot Design can detect the shadowed case with a lookup after its write and refuse the edit
visibly rather than appearing to work.

`UpdateSource()` now owns that merge (via `ResolveFontOverride()`), instead of each concrete theme
re-merging the held instance from `AddThemeSpecificResources()`. That is what fixes #1705, and it
removes the identical block from `MaterialTheme` and `SimpleTheme`. An override with no `Source` is
merged as assigned; a `Source` that no longer loads keeps the stale copy rather than dropping the
typefaces, matching `BuildColorLayer`'s contract — a property-changed callback must not throw into
the consuming app.

## What the seam deliberately does not do

Nothing here moves text **already rendered**. A seed colour reaches rendered UI because the theme
mutates live `SolidColorBrush` instances; a `FontFamily` is an immutable value, `ResourceDictionary`
raises nothing when an entry is set, and `{ThemeResource}` re-evaluates only on a resources-changed
sweep. The sweep that exists — `Application.UpdateResourceBindingsForHotReload()` — is `internal` to
Uno.UI, so a library cannot call it. Hot Design reaches it by reflection today; the upstream ask to
make it public stands (recorded on `uno.hotdesign#7871`). Newly created content picks the new
typeface up either way, and a reload always does.

## Checklist

- [x] `DefaultFontFamily` DP on `BaseTheme` with XML docs (runtime semantics, the already-rendered
      caveat, the override precedence, the framework default it does not touch)
- [x] `TypefaceScaleKeys` + `GenerateFontFamilyScale` in `BaseTheme.ScaleGeneration.cs`
- [x] `ThemesConstants.DefaultFontFamilyKey`
- [x] `UpdateSource()` merges the generated layer, then resolves and merges the font override last
- [x] `ResolveFontOverride()` re-reads a URI-backed override when a hot reload invalidates it (#1705)
- [x] Duplicated font-override merge removed from `MaterialTheme` and `SimpleTheme`
- [x] `SemanticThemeHelper.DefaultFontFamily` for parity with the seed accessors
- [x] Runtime tests: `SimpleSampleApp/RuntimeTests/Given_DefaultFontFamily.cs`
  - [x] Family set → all 20 keys follow (`[DataRow]` per key)
  - [x] Unset → Simple's own declarations stand (unset generates no key)
  - [x] Assigned after construction → the scales follow; replaced → the new family wins
  - [x] Cleared → the design system's defaults return (no null left shadowing them)
  - [x] Font override declaring a token wins; silent on it, the generated family stands
  - [x] Override setting **one** root only → the other keeps the generated family (the documented
        route to setting the roots apart)
  - [x] #1705 red/green: a URI-backed override is re-read from `Source` across a rebuild
  - [x] An override with no `Source` is merged as assigned
- [x] `doc/design-tokens.md`: token table, properties reference, rewritten *Typography Font Swap*
      (single property, runtime semantics, override precedence, the framework-default note)
- [x] Build clean (desktop TFM, Simple + Material libraries, Simple sample head)
- [x] Full runtime suite green headlessly

## Review-panel fixes (2026-09-01)

Seven-lane panel over `master..HEAD` (stack #1710 + #1707). Fixes landing in this layer:

- **Per-control alias keys regenerate.** `SimpleButtonFontFamily`,
  `SimpleToggleButtonFontFamily`, Material v2's twelve `*FontFamily` lightweight keys and the shared
  `DatePickerFlyoutPresenterFontFamily` are `StaticResource` aliases that snapshot at parse time,
  so a runtime `DefaultFontFamily` change never reached the control templates. `BaseTheme` now
  exposes an internal `FontFamilyAliasKeys` hook that each concrete theme overrides with the keys
  its Styles tree declares; the generated layer writes those too. Cupertino has no `BaseTheme`
  subclass, so `CupertinoFontFamily` has no runtime seam to follow — it stays a static alias.
- **…and the setters that read them must be `{ThemeResource}`.** Regenerating an alias key reaches
  nothing if the style reads it with `{StaticResource}`: Simple's `Button` and `ToggleButton` did,
  so the key followed the property while the rendered control stayed on Inter. Both now use
  `{ThemeResource}`, matching every other `*FontFamily` setter in Material v2 and Simple
  (`DatePickerFlyoutPresenterFontFamily` was already correct). Guarded red/green by
  `Given_DefaultFontFamily.When_DefaultFontFamilyChanges_Then_StyledButtonFollows`, which asserts
  the *rendered* `FontFamily` rather than the token.
- **HighContrast.** The generated layer now emits `Default`/`Light`/`HighContrast` (Material and
  Cupertino declare their root in a HighContrast font dictionary that would otherwise win).
- **Override re-read is cached.** `ResolveFontOverride()` re-parses the consumer file only when the
  hot-reload handler (or an override reassignment) invalidates it; a seed-color drag or spacing
  tweak reuses the resolved copy. On non-Uno targets a `Source`-less override is merged as a
  clone, since one instance may not sit under two parents there.
- **Fallbacks log.** The three catch-all degradations (`FontOverrideSource` load, font-override
  re-read, colour-override re-read) emit a guarded `LogWarning` with the URI, instead of nothing.
- **`AddThemeSpecificResources()`** documents its no-throw contract.
- **`TypefaceScaleKeys`** moved to `ThemesConstants`; a runtime test enumerates
  `SharedTypography.xaml`'s `*FontFamily` keys and asserts each follows the property, so the C#
  list can no longer drift from the XAML silently.
- **Tests added:** alias keys follow and clear; SharedTypography drift guard; generated layer
  declares the root under each of the three appearances; rendered text keeps the family across a
  Dark/Light flip; the #1705 test drives the real `MetadataUpdateHandler` entry point.
- **Deferred:** Material's legacy `MaterialLight/Medium/RegularFontFamily` keys still reference the
  static Roboto files next to the variable entry point, so a WASM payload carries both. Trimming is
  a `Uno.Fonts.Roboto` packaging concern (buildTransitive, alongside uno.fonts#75), and re-pointing
  the legacy keys at the entry point would change the weight consumers of those keys render.

## Open items

- **The sample UI is a separate commit, to be dropped if #1702's live-tuning page absorbs it.**
  `FontFamilyTunerControl`, in the Typography section of the Design Tokens page, sets the family on
  the running theme and then flips the root's `RequestedTheme` away from its actual theme and back so
  text already on screen re-resolves. `Given_FontFamilyTuner` covers it end to end, including a
  Display and a Body TextBlock both restyling from the one property.
- **That flip is a public route to the sweep this seam cannot perform itself**, and it is worth
  carrying back to `uno.hotdesign#7871`: it moves realized text without the reflected
  `Application.UpdateResourceBindingsForHotReload()`. It is heavier (a full theme-change pass, twice)
  and it re-resolves every `{ThemeResource}` in the tree rather than only the typography, so it is a
  fallback rather than a replacement — but it needs no private API.
- **The Fluent theme (PR #1695)** carries its own copy of the `AddThemeSpecificResources()`
  font-override block. It becomes redundant once this lands — a double merge of the stale instance
  over the re-resolved one — and should be removed when that branch rebases.
- **Per-appearance families.** The generated layer writes one value into `Default`, `Light` and
  `HighContrast` because a font family is appearance-independent. The theme-dictionary shape leaves room for a
  per-appearance root should one be wanted; the font override covers it today.
