# Lessons

Domain lessons and postmortems for the Uno.Themes repo. Append new entries at the top.

---

## Launching a sample head via `dotnet run` requires `-p:TargetFrameworkOverride=<platform>`, not just `-f`

**Context:** Spec 05 (Aspire AppHost, `dev/sb/aspire`). The AppHost launches sample heads as `dotnet run --project <sample>.csproj -f net10.0-desktop`. That failed with `NETSDK1147: the following workloads must be installed: wasm-tools-net9 / android` — even for a *desktop* run.

**Root cause:** The sample csprojs and the library projects they reference (`Uno.Simple.WinUI`, `Uno.Themes.WinUI`, …) are multi-targeted. `-f net10.0-desktop` selects the TFM for the sample's own *run*, but it does **not** cascade to restrict the referenced projects' TFM set — `dotnet run` implicitly builds the whole graph, so MSBuild still evaluates `net9.0-ios` / `net9.0-android` / `net10.0-android` on the dependencies and demands their workloads. The repo's single-platform switch is `TargetFrameworkOverride`: each sample csproj (and `src/library/tfm-common-winui.props`) only collapses to one platform's TFM when `TargetFrameworkOverride` is set (see the `Condition="'$(TargetFrameworkOverride)'!=''"` block in every sample csproj). AGENTS.md §4 documents `dotnet build … -p:TargetFrameworkOverride=desktop` for exactly this reason.

**How to apply:**
- Any tooling that shells out to `dotnet build`/`run` on a sample (or on `Uno.Themes.sln`) for a single platform **must** pass `-p:TargetFrameworkOverride=<platform>` (`desktop`, `browserwasm`, `android`, `ios`, …). `-f` alone is insufficient and will demand every platform's workload.
- Passing it as a **command-line** `-p:` property is also a robustness win: a command-line global property wins over the in-project reassignment done by a developer's `crosstargeting_override.props`, so the tool builds the platform it intends regardless of the dev's local override file. The AppHost resources rely on this.

---

## Typography slot→weight font mappings must be duplicated in Fonts.xaml (not only Typography.xaml)

**Context:** PR #1680 (`dev/sb/themes-revert`) — reworking `BaseTheme` resource management. CI runtime tests failed with 5 `Given_Fonts` cases: Bold display slots (`DisplayLargeFontFamily`, `DisplayMediumFontFamily`) resolved to `Inter-Regular` instead of `Inter-Bold`, and SemiBold slots (`HeadlineMediumFontFamily`, `TitleMediumFontFamily`, `LabelLargeFontFamily`) resolved to `Inter-Regular`/`Inter-Medium` instead of `Inter-SemiBold`.

**Root cause:** Simple's `Typography.xaml` maps the semantic font-family slots via `<StaticResource ResourceKey="SimpleBoldFontFamily" />` etc. `SharedTypography.xaml` (Uno.Themes core) *also* defines the same `*FontFamily` keys, aliased to `TypefacePlain`/`TypefaceBrand` (the Segoe-derived defaults). `<StaticResource>` aliases inside `ResourceDictionary.ThemeDictionaries` are resolved **eagerly at parse time** against whatever is visible in scope then — across separate merged dictionaries this resolution is unreliable, so the shared (wrong-weight) defaults can win. Master's fix was to **also** declare the slot→weight `<StaticResource>` mappings in `Fonts.xaml` (which is merged *after* `SharedTypography.xaml` inside `BaseDictionaries.xaml`), making the correct weights win deterministically. A reshape of `Fonts.xaml` deleted those duplicated mappings, reintroducing the bug.

**How to apply:**
- When a per-design-system typography file maps font-family slots to weight-specific keys, keep the matching mappings in the **font dictionary that is merged after `SharedTypography.xaml`** (e.g. Simple's `Fonts.xaml`). Do not assume the aliases in `Typography.xaml` alone are sufficient — they are not, because of eager cross-dictionary `<StaticResource>` resolution in theme dictionaries.
- Treat the slot→weight mappings in `Fonts.xaml` as load-bearing, not redundant. The comment in that file explains why; preserve it on any refactor.

**Verification trap (the more important lesson):** these font tests **passed in the minimal dedicated `Uno.Themes.RuntimeTests` host but failed in `SimpleSampleApp`** (and therefore in CI). The dedicated host merges `<SimpleTheme/>` app-wide, which "warms" the ambient resolution scope so the fragile `<StaticResource>` aliases happen to resolve to the right weight — a **false positive**. The real consumer-like host (`SimpleSampleApp`, also what CI runs) exposed the bug.
- **Always verify font/typography/resource-precedence changes in `SimpleSampleApp` (the CI host), not only in a minimal host.** A minimal single-theme host can mask cross-dictionary resolution and merge-order bugs. If two hosts disagree, trust the one that matches CI.
