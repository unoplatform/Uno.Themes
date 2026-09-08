# `Application.GetTheme()` walks nested MergedDictionaries (issue #1704)

Implements https://github.com/unoplatform/Uno.Themes/issues/1704.

> Numbered 10 because `specs/08-…` is claimed by PR #1702 and `specs/09-…` by PR #1707.

## The gap

`GetTheme(this Application)` (added in #1699) scanned only the top level:

```csharp
application?.Resources?.MergedDictionaries.OfType<BaseTheme>().FirstOrDefault();
```

An application that keeps its design system in a `ResourceDictionary` of its own was therefore
reported as having **no theme** — and that is the layout the hot-reload guidance steers apps toward,
because `App.xaml` yields no reloadable type, so a design system declared inline there can be written
to and reloaded with nothing re-rendering. `SemanticThemeHelper.GetTheme()` inherited the same reach,
being a pure delegation.

## Design

Breadth-first over `MergedDictionaries`, in `ApplicationExtensions.GetTheme` / `FindNestedTheme`:

- **Breadth-first, shallowest wins**, first match within a level. Chosen so the previous behaviour is
  preserved *exactly* for any application that already had a top-level theme — the walk only ever
  extends the search, never changes which theme an existing app resolves.
- **The top level is answered without allocating.** The inline-in-`App.xaml` layout is the common one
  and the one that already worked; only a miss pays for the queue and the visited set.
- **Visited set with reference semantics.** `ResourceDictionary` does not override equality, and
  identity is what makes a repeated dictionary cheap and a cycle finite. Resource graphs are diamonds
  by construction (several dictionaries merging one shared palette), so this is the normal case, not
  the pathological one.
- **`ThemeDictionaries` is not walked.** A design system is not an appearance-specific resource, and
  enumerating that collection throws on some platforms (see `specs/lessons.md`).
- **Depth bound of 32 levels**, where level 1 is "merged straight into `Application.Resources`". The
  visited set is what rules out a cycle; the bound keeps the cost predictable on a pathological graph.
  Mirrors the depth bound Uno's own theme-resolution walk carries for the same reason.

## What the walk does *not* do: cross an ALC boundary

The match is `is BaseTheme`, i.e. by type identity, so whether a host can reach a hosted guest's
theme is decided by the host's assembly-sharing policy and **not** by depth. Both policies are now
pinned by tests, having been measured rather than inferred:

| Host policy for `Uno.Themes.WinUI` | Guest theme is a host `BaseTheme`? | `hostSide.GetTheme()` over a nested guest theme |
|---|---|---|
| shared with the default context (Hot Design's shape) | yes | found — this walk is exactly what was missing |
| isolated per guest (`!Uno.Themes.WinUI`, the wrapper sample's policy) | no | `null` at any depth |

A guest theme constructs cleanly in a secondary collectible context that shares `Uno.UI`, and merges
into the host's resource graph under both policies; only the type match differs. So the isolated case
needs the guest to resolve its own `Application` from inside its own context (the pattern PR #1703
introduces via `NavigationHelper.CurrentApplication`) or a name-based match; the recursive walk is
necessary but not sufficient there, and the test pins that so the walk is never mistaken for
cross-ALC support. Recorded in the XML docs on `GetTheme` and in `doc/seed-colors.md`.

## Checklist

- [x] Red tests first, extending `Given_ApplicationExtensions`
  - [x] theme one and two levels down
  - [x] the same layout via a `Source`-loaded dictionary whose `MergedDictionaries` the XAML parser
        populated (fixture: `RuntimeTests/NestedThemeFixture.xaml`) — this repo has been bitten before
        by XAML-backed dictionaries behaving differently from code-built ones
  - [x] `SemanticThemeHelper.GetTheme()` inherits the reach
  - [x] a diamond the walk must traverse without revisiting or recursing forever
  - [x] both edges of the depth bound (31 dictionaries deep found, 32 not)
- [x] Red ALC tests in `Given_AlcApplicationExtensions`, one per sharing policy, built on a
      collectible guest context that shares `Uno.UI` and mirrors the wrapper's share/isolate markers
- [x] Breadth-first walk with visited set, allocation-free top level, depth bound
- [x] XML docs: order, cycle guard, why `ThemeDictionaries` is skipped, the ALC caveat
- [x] `doc/seed-colors.md`: the nested layout, the walk's order, the ALC note
- [x] `specs/03-seed-color-palette/seed-color-palette.md`: the extension's description corrected
- [x] Release build of the base library clean; full Simple suite green

## Review

Verification (`net10.0-desktop` Debug, headless `--runtime-tests`, 2026-08-31):

- Red before the fix: 6 failures — the five nested/diamond/helper cases and the shared-assembly ALC
  case — with the three pre-existing tests still passing, so the gap was depth and nothing else.
- Green after: full Simple suite **180 passed, 0 failed, 1 pre-existing `[Ignore]` skip**; Material
  head unchanged. The depth-bound rows land exactly where the doc says (31 found, 32 not), so the
  loop bound and the documented contract agree.
- `Uno.Themes.WinUI` Release build carries the same two pre-existing `CS0618` warnings as `master`.

Every test rearranges `Application.Current.Resources` and restores it exactly, detaching the theme
before re-parenting it (a `ResourceDictionary` may not be nested under two parents); the ~170 other
tests, most of which resolve through the application theme, are unaffected.

## Open items

- **Releasing it.** The issue also asks for the extension to ship in a released package so apps and
  tooling can rely on it instead of each writing their own walk; that is a release action, not a code
  change, and is not covered here.
- **Conflict with PR #1703.** That PR rewrites the same paragraph of
  `specs/03-seed-color-palette/seed-color-palette.md` (the ALC motivation) and adds the guest-side
  hand-off plus a host-side name-based check in the wrapper smoke. Whichever lands second should fold
  the two descriptions together; the facts recorded here and there agree.
