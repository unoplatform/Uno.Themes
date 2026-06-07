# Lessons

Domain lessons and postmortems for the Uno.Themes repo. Append new entries at the top.

---

## Typography slot→weight font mappings must be duplicated in Fonts.xaml (not only Typography.xaml)

**Context:** PR #1680 (`dev/sb/themes-revert`) — reworking `BaseTheme` resource management. CI runtime tests failed with 5 `Given_Fonts` cases: Bold display slots (`DisplayLargeFontFamily`, `DisplayMediumFontFamily`) resolved to `Inter-Regular` instead of `Inter-Bold`, and SemiBold slots (`HeadlineMediumFontFamily`, `TitleMediumFontFamily`, `LabelLargeFontFamily`) resolved to `Inter-Regular`/`Inter-Medium` instead of `Inter-SemiBold`.

**Root cause:** Simple's `Typography.xaml` maps the semantic font-family slots via `<StaticResource ResourceKey="SimpleBoldFontFamily" />` etc. `SharedTypography.xaml` (Uno.Themes core) *also* defines the same `*FontFamily` keys, aliased to `TypefacePlain`/`TypefaceBrand` (the Segoe-derived defaults). `<StaticResource>` aliases inside `ResourceDictionary.ThemeDictionaries` are resolved **eagerly at parse time** against whatever is visible in scope then — across separate merged dictionaries this resolution is unreliable, so the shared (wrong-weight) defaults can win. Master's fix was to **also** declare the slot→weight `<StaticResource>` mappings in `Fonts.xaml` (which is merged *after* `SharedTypography.xaml` inside `BaseDictionaries.xaml`), making the correct weights win deterministically. A reshape of `Fonts.xaml` deleted those duplicated mappings, reintroducing the bug.

**How to apply:**
- When a per-design-system typography file maps font-family slots to weight-specific keys, keep the matching mappings in the **font dictionary that is merged after `SharedTypography.xaml`** (e.g. Simple's `Fonts.xaml`). Do not assume the aliases in `Typography.xaml` alone are sufficient — they are not, because of eager cross-dictionary `<StaticResource>` resolution in theme dictionaries.
- Treat the slot→weight mappings in `Fonts.xaml` as load-bearing, not redundant. The comment in that file explains why; preserve it on any refactor.

**Verification trap (the more important lesson):** these font tests **passed in the minimal dedicated `Uno.Themes.RuntimeTests` host but failed in `SimpleSampleApp`** (and therefore in CI). The dedicated host merges `<SimpleTheme/>` app-wide, which "warms" the ambient resolution scope so the fragile `<StaticResource>` aliases happen to resolve to the right weight — a **false positive**. The real consumer-like host (`SimpleSampleApp`, also what CI runs) exposed the bug.
- **Always verify font/typography/resource-precedence changes in `SimpleSampleApp` (the CI host), not only in a minimal host.** A minimal single-theme host can mask cross-dictionary resolution and merge-order bugs. If two hosts disagree, trust the one that matches CI.
