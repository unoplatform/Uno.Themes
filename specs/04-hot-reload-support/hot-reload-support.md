# Hot Reload Support for `BaseTheme`

**Branch:** `dev/sb/themes-hr`
**Status:** Implemented (pending review)
**Related:** `uno-private/specs/043-hotreload-resource-dictionary-refresh/spec.md` — framework-side counterpart (lives in the private `uno-private` repository, not this one)

## Overview

Edits to a colour / font / control-style XAML file referenced (directly or transitively) from a `BaseTheme`-derived dictionary — e.g. `ThemeColors.xaml` brought in via `Colors.OverrideSource`, or any merged-pages entry baked into `Uno.Material` / `Uno.Cupertino` / `Uno.Simple.WinUI` — now propagate to the running app on .NET hot reload without an app restart.

The pre-fix theme model rebuilt its merged-dictionary tree from scratch each time a DP input changed (`PrimarySeed`, `DefaultDensity`, `DefaultCornerRadius`, override DPs, …). On hot reload there was no rebuild trigger at all, because `BaseTheme` had no opt-in to .NET's `MetadataUpdateHandler` pipeline. The framework's generic `UpdateResourceDictionaries` walk also couldn't help: it refreshes the *content* of a source-backed `ResourceDictionary` in-place, but it cannot regenerate a code-built dictionary (spacing scale, shape scale, density defaults, generated seed palette) or re-apply a precedence stack of merged children that lives in private fields on a typed subclass.

Two coordinated changes — one in this repo, one in `uno-private` — close the gap.

## Goals

1. Hot-reload edits to source-backed XAML referenced by a `BaseTheme` (directly or nested) propagate immediately.
2. A theme rebuild does not clear or replace the URI-backed base layer that the framework's own resource-refresh walk just updated. The two pipelines must compose, not fight.
3. Programmatically-generated layers (spacing / shape / density / generated seed palette / consumer overrides) regenerate on each rebuild from the current state of the theme's DP inputs.
4. Theme instances that consumers no longer reference are collectible. Hot-reload tracking must not pin them.
5. No new public API surface. Existing override DPs and `Colors.OverrideSource` continue to behave identically; the visible difference is purely "the edit takes effect on save".

---

## Approach

### 1. `BaseTheme` opts in to `[MetadataUpdateHandler]`

`src/library/Uno.Themes/BaseTheme.HotReload.cs` is a new partial that:

- Declares `[assembly: MetadataUpdateHandler(typeof(Uno.Themes.BaseTheme))]` so the .NET runtime calls back into the theme assembly when an HR delta lands.
- Maintains `List<WeakReference<BaseTheme>>` of live instances. Each `BaseTheme` constructor calls `RegisterInstance(this)` to opt in. Dead entries are swept on every register and every update pass, so a theme dropped by the consumer becomes collectible normally.
- Exposes a static `UpdateApplication(Type[]? updatedTypes)` invoked by the runtime. It early-outs unless at least one updated type is assignable to `ResourceDictionary`; on a match it iterates the live instances and re-runs `UpdateSource()` on each.
- Captures the construction-time `DispatcherQueue` so updates marshal back to the UI thread when the HR callback fires off-thread.

This is the *only* place anything in the theme libraries hooks `MetadataUpdateHandler`.

### 2. `BaseTheme.UpdateSource` is now non-destructive

Pre-fix:

```csharp
ThemeDictionaries.Clear();
MergedDictionaries.Clear();      // <- destroys the URI-backed CopyFrom layer
this.Clear();
// …rebuild everything from scratch (converters / colors / typography / mergedpages / overrides)
```

Combined with a brush-tracking pass (`CollectBrushes` / `UpdateOldBrushes`) to keep already-bound `{ThemeResource}` brushes pointing at the freshly-rebuilt colour values across the wipe. On the seed-color-change path the brush tracker compensated for the wipe; on hot reload, however, **there was nothing wiring the rebuild to the runtime's HR pass**, so brushes never got the new colours.

Post-fix `UpdateSource` keeps the URI-backed layer intact and only manages the *dynamic* layers it owns:

```csharp
// Constructor (once per instance)
Source = new Uri(DefaultStylesSource);   // CopyFrom populates the URI-backed base layer
UpdateSource();                          // appends dynamic layers on top

// UpdateSource (called on every input change AND on hot reload)
foreach (var d in _dynamicDictionaries) MergedDictionaries.Remove(d);
_dynamicDictionaries.Clear();
AddDynamic(GenerateSpacingScale(...));
AddDynamic(GenerateShapeScale(...));
AddDynamic(GenerateDensityDefaults());
if (_baseColorOverride is { } baseColorOverride) AddDynamic(baseColorOverride);
if (effectivePrimary is { } seed)
    AddDynamic(SeedColorPaletteGenerator.Default.Generate(seed, …, UseHighFidelityColors));
if (Colors?.OverrideDictionary is { } userOverride)
    AddDynamic(userOverride.Source is { } src
        ? new ResourceDictionary { Source = src }   // freshly resolved → picks up edits
        : userOverride);
if (FontOverrideDictionary is { } fontOverride) AddDynamic(fontOverride);
```

Key invariants:

- **`Source` is set exactly once** (in the constructor). The framework's `CopyFrom` brings the merged-pages contents into `MergedDictionaries`. The framework's own hot-reload pass refreshes the *content* of those entries in place — they stay at the same indices, and the dynamic layers above them keep their precedence.
- **`_dynamicDictionaries` is the only thing `UpdateSource` mutates.** Each call removes exactly the entries it appended on the previous call and re-appends fresh ones from the current state of `DefaultDensity`, `DefaultCornerRadius`, `Colors.PrimarySeed`, `Colors.OverrideDictionary`, `FontOverrideDictionary` and the constructor-supplied base overlay.
- **`Colors.OverrideDictionary` URIs are re-resolved each pass.** When the override carries a `Source`, we synthesise a fresh `ResourceDictionary { Source = src }` instead of re-adding the original instance. The original was populated at init time and would otherwise serve stale key/value pairs even after the framework refreshed its underlying file — the framework refreshes *the file the resolver hands out*, but the original consumer-held instance is a separate snapshot.

Consequence: the brush-tracking pass (`CollectBrushes`, `CollectBrushEntries`, `UpdateOldBrushes`, `IsInResourceTree`, `IsReachableFrom` in `BaseTheme.SeedColors.cs`) is no longer needed. The framework's `{ThemeResource}` re-binding (`UpdateResourceBindingsForHotReload`) handles propagation now that the underlying dictionaries are refreshed in place rather than replaced. These helpers were removed.

### 3. `SimpleTheme` loses the C# resource-stitching path

`SimpleTheme` used to build its dictionary tree in C#:

- `GetSimpleColorOverride` merged the consumer `colorOverride` on top of `Uno.Simple.WinUI/Styles/Application/ColorPalette.xaml`.
- `GenerateSpecificResources` returned a fresh `ResourceDictionary` whose merged graph stitched together `MergedPages`, `Thickness`, and `Fonts` (plus the font override).

Both methods exist *only* because the base class used to require subclasses to materialise their full static layer per rebuild. With the new "set `Source` once" contract the static layer comes from the merged-pages XAML directly, and a new `src/library/Uno.Simple.WinUI/Styles/Application/BaseDictionaries.xaml` file gathers the shared converters / typography / palette / shared-colours plus Simple's own fonts, thickness and grayscale palette in the correct precedence order:

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="ms-appx:///Uno.Themes.WinUI/Styles/Applications/Common/Converters.xaml" />
    <ResourceDictionary Source="ms-appx:///Uno.Themes.WinUI/Styles/Applications/Common/SharedTypography.xaml" />
    <ResourceDictionary Source="ms-appx:///Uno.Themes.WinUI/Styles/Applications/Common/SharedColorPalette.xaml" />
    <ResourceDictionary Source="ms-appx:///Uno.Themes.WinUI/Styles/Applications/Common/SharedColors.xaml" />
    <ResourceDictionary Source="ms-appx:///Uno.Simple.WinUI/Styles/Application/Common/Fonts.xaml" />
    <ResourceDictionary Source="ms-appx:///Uno.Simple.WinUI/Styles/Application/Common/Thickness.xaml" />
    <!-- Simple's grayscale palette: listed last so it shadows SharedColorPalette. -->
    <ResourceDictionary Source="ms-appx:///Uno.Simple.WinUI/Styles/Application/ColorPalette.xaml" />
</ResourceDictionary.MergedDictionaries>
```

`BaseDictionaries.xaml` is picked up automatically by Simple's `XamlMergeInput Include="Styles\Application\**\*.xaml"` glob in `simple-common.props`, so it ends up inside `mergedpages.xaml` and arrives via the single `Source = new Uri(SimpleConstants.ResourcePaths.MergedPages)` set in `BaseTheme`'s constructor. The class itself collapses to:

```csharp
public class SimpleTheme(ResourceDictionary colorOverride = null, ResourceDictionary fontOverride = null)
    : BaseTheme(colorOverride, fontOverride)
{
    protected override Color? DefaultPrimarySeed => null;
    protected override bool UseHighFidelityColors => true;
    protected override string DefaultStylesSource => SimpleConstants.ResourcePaths.MergedPages;
}
```

Note ordering: Simple's `ColorPalette.xaml` is listed *last* inside `BaseDictionaries.xaml` so its grayscale values shadow the shared palette (`MergedDictionaries` are searched in reverse).

### 4. `MaterialTheme` — partial migration

`MaterialTheme.GenerateSpecificResources` is replaced by `DefaultStylesSource => MaterialConstants.ResourcePaths.Version2.MergedPages`. Material's `material-common.props` still excludes `Styles\Application\Common\Fonts.xaml` from the merged-pages output (`<XamlMergeInput Remove="Styles\Application\Common\Fonts.xaml" />`) so that the consumer font override can be `SafeMerge`d on top of the default font dictionary before joining the tree.

> **Known gap (follow-up commit required).** The current `MaterialTheme.cs` declares `protected override void AddThemeSpecificDictionaries(IList<ResourceDictionary>)` to perform that font composition, but `BaseTheme` does not (yet) expose that virtual hook. Until one of (a) `BaseTheme` re-introduces a `protected virtual void AddThemeSpecificDictionaries(IList<ResourceDictionary>)` extension point invoked from `UpdateSource()`, or (b) `MaterialTheme` is reworked to compose the font override through the existing `FontOverrideDictionary` path (the Simple model — list `Common/Fonts.xaml` inside `mergedpages.xaml` via `XamlMergeInput` and rely on `BaseTheme.UpdateSource` to overlay the consumer override), Material will not compile. The fix lands in a follow-up commit; this spec describes the model the follow-up converges on.

### 5. Framework-side dependency

This work composes with `uno-private/specs/043-hotreload-resource-dictionary-refresh`, which makes the framework's `UpdateResourceDictionaries` walk:

- recurse into typed `ResourceDictionary` subclasses (so it reaches dictionaries `BaseTheme.UpdateSource` appended);
- only call `RefreshMergedDictionary` on entries whose runtime type is exactly `ResourceDictionary` (so `BaseTheme` is never *replaced* with a freshly-loaded plain `ResourceDictionary` — which would lose every dynamic layer this spec adds).

Either change alone is insufficient. Together: the framework refreshes the URI-backed base layer in place, then the runtime fires our `MetadataUpdateHandler`, which rebuilds the dynamic layers on top.

---

## Code Path on Hot Reload

1. Developer saves an edit to `ThemeColors.xaml` (consumer file referenced via `Colors.OverrideSource`).
2. DevServer compiles and ships a metadata-update delta to the running app.
3. The runtime calls `ClientHotReloadProcessor.UpdateGlobalResources` (framework). With the spec-043 changes that pass recurses through the merged-dictionary graph and refreshes every plain `ResourceDictionary` whose source matches an updated entry. The `BaseTheme`-derived dictionary itself is traversed but not replaced.
4. The runtime invokes every registered `MetadataUpdateHandler`, including `BaseTheme.UpdateApplication`.
5. `BaseTheme.UpdateApplication` checks the updated-types batch for any `ResourceDictionary` subclass; if found, it iterates live `BaseTheme` instances and dispatches `UpdateSource()` to each via the captured `DispatcherQueue`.
6. `UpdateSource` removes the previous dynamic-layer batch and re-appends fresh spacing/shape/density/seed/override layers on top of the now-refreshed base.
7. The framework's `UpdateResourceBindingsForHotReload` re-evaluates `{ThemeResource}` bindings.

---

## API Surface

No additions. The following members of `BaseTheme` changed:

| Member | Before | After |
| --- | --- | --- |
| `GenerateSpecificResources()` | `protected abstract ResourceDictionary` | **Removed.** |
| `DefaultStylesSource` | — | `protected abstract string` (URI of the theme's merged-pages dictionary). |
| `UpdateSource(bool fromHotReload)` overload | existed transiently | **Removed.** A single parameterless `UpdateSource()` covers both DP-driven and HR-driven rebuilds. |
| `_dynamicDictionaries` | — | New private field tracking the entries owned by `UpdateSource`. |
| `_originalBrushes`, `_isInResourceTree`, `CollectBrushes`, `CollectBrushEntries`, `UpdateOldBrushes`, `IsInResourceTree`, `IsReachableFrom` | existed | **Removed.** The non-destructive rebuild model makes brush capture unnecessary. |

`GenerateSpecificResources` was protected-abstract; consumer themes that derived from `BaseTheme` directly will need to migrate to overriding `DefaultStylesSource` (and optionally `AddThemeSpecificDictionaries`). The first commit on this branch (`b04b7c6c`) is tagged `fix!:` to flag the break.

---

## Tests

`src/samples/SimpleSampleApp/RuntimeTests/Given_HotReload.cs` (new):

| Test | What it asserts |
| --- | --- |
| `When_AssemblyLoaded_Then_MetadataUpdateHandlerTargetsBaseTheme` | `[assembly: MetadataUpdateHandler(typeof(BaseTheme))]` is present and `UpdateApplication(Type[]?)` exists as a static method. Guards against an accidental rename / removal that would silently disable hot-reload propagation. |
| `When_HotReloadHandlerFires_Then_LiveBaseThemeInstancesAreRebuilt` | Invoking `UpdateApplication` against a live `SimpleTheme` regenerates the dynamic-layer entries inside `MergedDictionaries` (count preserved, at least one entry replaced by a fresh instance). |
| `When_BaseThemeIsCollected_Then_HotReloadHandlerDoesNotResurrectIt` | Dropping the last strong reference to a `SimpleTheme` makes the instance eligible for GC, and the dead entry is swept by the next `UpdateApplication` pass without throwing. Guards against the weak-ref tracking accidentally becoming strong. |

These run under the existing `Uno.UI.RuntimeTests.Engine` host inside `SimpleSampleApp`.

---

## File-Level Change List

### `src/library/Uno.Themes/`

- `BaseTheme.HotReload.cs` (new) — `[MetadataUpdateHandler]` registration, live-instance tracking, `UpdateApplication` dispatcher.
- `BaseTheme.cs` — Source-once + `_dynamicDictionaries` model. Removed `_originalBrushes` / `_isInResourceTree`. Replaced abstract `GenerateSpecificResources` with abstract `DefaultStylesSource`.
- `BaseTheme.SeedColors.cs` — removed `CollectBrushes`, `CollectBrushEntries`, `UpdateOldBrushes`, `IsInResourceTree`, `IsReachableFrom` (no longer needed under the non-destructive rebuild).
- `ColorGeneration/Hct/HctColor.cs` — `[SuppressMessage(CA1815)]` justification for not implementing `Equals` on the value type (transient computation result; equality not part of the consumer contract).
- `Converters/FromBoolToValueConverter.cs`, `Converters/StringFormatConverter.cs` — `CultureInfo.InvariantCulture` passed explicitly to `Convert.ToBoolean` / `string.Format` to silence the culture analyser without behaviour change.

### `src/library/Uno.Simple.WinUI/`

- `SimpleTheme.cs` — collapsed to `DefaultPrimarySeed` / `UseHighFidelityColors` / `DefaultStylesSource`. Removed `GetSimpleColorOverride` and `GenerateSpecificResources`.
- `Styles/Application/BaseDictionaries.xaml` (new) — static base layer (shared converters/typography/palette/colours + Simple fonts/thickness/grayscale palette), picked up by the existing `XamlMergeInput` glob in `simple-common.props`.

### `src/library/Uno.Material/`

- `MaterialTheme.cs` — replaced `GenerateSpecificResources` with `DefaultStylesSource`. Currently also declares an `AddThemeSpecificDictionaries` override that has no matching base member (see *Known gap* above); resolution deferred to a follow-up.

### `src/samples/SimpleSampleApp/`

- `RuntimeTests/Given_HotReload.cs` (new) — three guard tests described above.

### Tooling

- `.vscode/settings.json` — pinned `dotnet.defaultSolution` to `Uno.Themes.sln`.

### Outside this repo

- `uno-private/src/Uno.UI.RemoteControl/HotReload/ClientHotReloadProcessor.MetadataUpdate.cs` — recursive walk with typed-subclass guard, sibling-isolated `try/catch` with `Trace`-level logging. Documented in `uno-private/specs/043-hotreload-resource-dictionary-refresh/spec.md`.

---

## Verification

### Code review

- The `MetadataUpdateHandler` path holds weak references; live-instance scan sweeps dead entries on every pass.
- `UpdateSource` only ever touches entries it itself appended; the framework's in-place URI-backed refresh is undisturbed.
- The `Colors.OverrideDictionary.Source` re-resolution closes the "consumer-held instance is stale even after the framework refreshes the underlying file" hole.

### Compile

- `Uno.Themes.WinUI` and `Uno.Simple.WinUI` build clean for the Skia target via the standard `crosstargeting_override.props` workflow.
- `Uno.Material.WinUI` does **not** currently build because of the dangling `AddThemeSpecificDictionaries` override (see *Known gap* under §4 above). A follow-up commit closes it.

### Runtime

End-to-end verification was done against the `artifacts/colors` desktop sample wired to `SimpleTheme` + `Colors.OverrideSource="ms-appx:///ThemeColors.xaml"`:

1. Launch via `dotnet watch run -f net10.0-desktop`.
2. Confirm baseline red background driven by `<Color x:Key="PrimaryColor">Red</Color>` in `ThemeColors.xaml`.
3. Edit the file, save (`Red` → `Green` in both Light and Default dictionaries).
4. `dotnet watch` logs `🔥 C# and Razor changes applied`.
5. Window updates to green without a restart and without the white-screen regression that an earlier naive-recursion attempt produced.

The runtime tests in `Given_HotReload` guard the wiring contract independently of the visual verification path.

### Console-output cleanup

The earlier debugging instrumentation (three `Console.WriteLine($"[STEVE] …")` statements added to `ClientHotReloadProcessor.MetadataUpdate.cs` while diagnosing the typed-subclass replacement bug) was removed in the same change set that adopted the typed-subclass guard. Failure to refresh a single source-backed merged dictionary now logs at `Trace` level only, gated by `IsEnabled(LogLevel.Trace)` so disabled paths pay no allocation or formatting cost.

---

## Out of Scope

- **Compile-time-embedded `ms-resource:///` URIs.** Framework-merged pages and library-embedded XAML are baked into assemblies; refreshing them requires assembly hot-reload, which is not what metadata-update deltas provide.
- **DP-driven theme inputs reacting to HR-only edits.** If a hot-reload edit changes a value bound to `DefaultCornerRadius` (or another input DP) via `{ThemeResource}`, the binding will re-evaluate but the DP change-callback doesn't refire on a same-value re-bind. In practice consumers don't edit token-binding indirection at runtime; the documented pattern is to edit the override dictionary directly.

---

## Related Specs

- `specs/01-design-tokens/` — token model the dynamic spacing/shape/density layers regenerate from.
- `specs/02-semantic-brushes/` — brush layer that consumes the regenerated colour palette.
- `specs/03-seed-color-palette/` — seed-driven palette generation; one of the dynamic layers `UpdateSource` rebuilds.
- `uno-private/specs/043-hotreload-resource-dictionary-refresh/` — framework-side counterpart that makes the source-backed entries inside a `BaseTheme` reachable for the in-place refresh.
