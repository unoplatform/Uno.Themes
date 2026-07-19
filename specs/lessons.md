# Lessons

Domain lessons and postmortems for the Uno.Themes repo. Append new entries at the top.

---

## Collectible-ALC guests: sweep-proof roots pin the ALC; verify reclamation with weak-ref telemetry, not RSS eyeballing

**Context:** ThemesSampleApp ALC wrapper (spec 05, Uno 6.7-dev). Load/unload soak leaked every guest ALC (~50 MB/cycle RSS growth); Debug builds of the wrapper collected fine, Release never did, with identical teardown logs.

**Root causes (three distinct, found via `dotnet-dump` `gcroot`):**
1. `DependencyProperty._getPropertyCache` caches `(targetType, "ns:Owner.Property") → DP` from style/VSM target paths. A guest style targeting an attached property on a **framework** element stores a default-ALC key with a guest-ALC value; Uno's `RemoveNonDefaultAlcEntries` checks only the **key's** ALC, so the entry — and through the DP's owner type, the whole guest ALC — survives every sweep.
2. The samples' `Shell` subscribes to the process-wide `SystemNavigationManager.BackRequested` and never unsubscribes; Uno's ALC event-subscription pruning does not cover that singleton, so the entire guest visual tree stays rooted.
3. Guest `DependencyObject` finalizers run during ALC unload and can re-populate caches **after** `ExitAlcApplication`'s sweep ran.

**How to apply:**
- When hosting (or testing) collectible-ALC guests, treat "Exit ran + Unload called" as insufficient: add a `WeakReference<AssemblyLoadContext>` check after a post-unload `GC.Collect/WaitForPendingFinalizers/GC.Collect`, and log collected/alive every cycle. RSS alone conflates managed leaks, GC retention, and native leaks.
- Diagnose with `dotnet-dump`: `dumpheap -type <ALC>` → `gcroot <addr>`, then **census all root anchors** (`grep "static variable:" | sort | uniq -c`) instead of reading only the first chain — multi-root pinning is the norm, and each fix reveals the next root. Dependent handles `(10)` are usually circular (CWT/collectible-statics), not true roots; a strong handle directly on an ALC in `_state == 1` (Unloading) is the runtime's own until unload completes.
- Debug-vs-Release differences in ALC collection are usually **timing masks**, not fixes — never conclude "works in Debug" means reclaimed.
- Mixed-ALC key/value caches are a general Uno hazard: any process-wide cache keyed by framework type but holding guest values defeats per-ALC sweeps. Prefer upstream fixes that also check the **value's** ALC.
- Known upstream gap (Uno 6.7-dev): each ALC guest window create/close leaks its native X11 GL context (+ llvmpipe threads) even through `Window.CloseAlcWindows`; managed heap stays clean. Track via llvmpipe thread-group count.

---

## Typography slot→weight font mappings must be duplicated in Fonts.xaml (not only Typography.xaml)

**Context:** PR #1680 (`dev/sb/themes-revert`) — reworking `BaseTheme` resource management. CI runtime tests failed with 5 `Given_Fonts` cases: Bold display slots (`DisplayLargeFontFamily`, `DisplayMediumFontFamily`) resolved to `Inter-Regular` instead of `Inter-Bold`, and SemiBold slots (`HeadlineMediumFontFamily`, `TitleMediumFontFamily`, `LabelLargeFontFamily`) resolved to `Inter-Regular`/`Inter-Medium` instead of `Inter-SemiBold`.

**Root cause:** Simple's `Typography.xaml` maps the semantic font-family slots via `<StaticResource ResourceKey="SimpleBoldFontFamily" />` etc. `SharedTypography.xaml` (Uno.Themes core) *also* defines the same `*FontFamily` keys, aliased to `TypefacePlain`/`TypefaceBrand` (the Segoe-derived defaults). `<StaticResource>` aliases inside `ResourceDictionary.ThemeDictionaries` are resolved **eagerly at parse time** against whatever is visible in scope then — across separate merged dictionaries this resolution is unreliable, so the shared (wrong-weight) defaults can win. Master's fix was to **also** declare the slot→weight `<StaticResource>` mappings in `Fonts.xaml` (which is merged *after* `SharedTypography.xaml` inside `BaseDictionaries.xaml`), making the correct weights win deterministically. A reshape of `Fonts.xaml` deleted those duplicated mappings, reintroducing the bug.

**How to apply:**
- When a per-design-system typography file maps font-family slots to weight-specific keys, keep the matching mappings in the **font dictionary that is merged after `SharedTypography.xaml`** (e.g. Simple's `Fonts.xaml`). Do not assume the aliases in `Typography.xaml` alone are sufficient — they are not, because of eager cross-dictionary `<StaticResource>` resolution in theme dictionaries.
- Treat the slot→weight mappings in `Fonts.xaml` as load-bearing, not redundant. The comment in that file explains why; preserve it on any refactor.

**Verification trap (the more important lesson):** these font tests **passed in the minimal dedicated `Uno.Themes.RuntimeTests` host but failed in `SimpleSampleApp`** (and therefore in CI). The dedicated host merges `<SimpleTheme/>` app-wide, which "warms" the ambient resolution scope so the fragile `<StaticResource>` aliases happen to resolve to the right weight — a **false positive**. The real consumer-like host (`SimpleSampleApp`, also what CI runs) exposed the bug.
- **Always verify font/typography/resource-precedence changes in `SimpleSampleApp` (the CI host), not only in a minimal host.** A minimal single-theme host can mask cross-dictionary resolution and merge-order bugs. If two hosts disagree, trust the one that matches CI.
