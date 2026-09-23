# Upstream issues — unoplatform/uno (6.7-dev secondary-ALC app support)

The four gaps found while building `ThemesSampleApp` (see `progress.md` → Phase 4 findings and
Review). All were reproduced against `Uno.Sdk.Private 6.7.0-dev.815` (`artifacts/uno` @
`21bf1ad6`) hosting the three theme sample heads through `AlcContentHost` /
`WindowHelper.ContentHostOverride`.

All four are now filed. Delete each wrapper-side sweep in
`src/samples/ThemesSampleApp/GuestHosting/GuestAppLoader.Sweeps.cs` when its fix ships.

| # | Issue | Wrapper-side sweep |
| --- | --- | --- |
| 1 | [unoplatform/uno#24073](https://github.com/unoplatform/uno/issues/24073) | clears `DependencyProperty._getPropertyCache` |
| 2 | [unoplatform/uno#24074](https://github.com/unoplatform/uno/issues/24074) | prunes `SystemNavigationManager` handlers |
| 3 | [unoplatform/uno#24075](https://github.com/unoplatform/uno/issues/24075) | re-invokes `Application.CleanupNonDefaultAlcCaches` |
| 4 | [unoplatform/uno#24076](https://github.com/unoplatform/uno/issues/24076) | none, not reachable host-side |

---

## 1. `RemoveNonDefaultAlcEntries` misses cross-ALC `_getPropertyCache` entries, pinning guest ALCs

**Area**: DependencyProperty / secondary-ALC app support
**Version**: 6.7.0-dev.815 (Skia desktop + wasm)

`DependencyProperty._getPropertyCache` memoizes `(targetType, "ns:Owner.Property") → DP`
lookups from style/VSM target paths. When a **guest** (secondary-ALC) style targets an
attached property declared in the guest on a **framework** element (e.g. `Button`), the cache
gains an entry whose *key* type lives in the default ALC but whose *value* (the DP, and its
owner type) lives in the guest ALC. `NameToPropertyDictionary.RemoveNonDefaultAlcEntries`
checks only the key's ALC, so the entry survives `ExitAlcApplication()`'s sweep and roots the
guest's `LoaderAllocator` — the collectible ALC can never be collected (verified with
`dotnet-dump` `gcroot`: the retention path enters through this cache entry).

**Suggested fix**: also check the cached DP value's owner-type ALC in
`RemoveNonDefaultAlcEntries` (or clear the pure memoization cache wholesale on ALC teardown —
it repopulates on demand).

**Workaround**: the hosting app clears `_getPropertyCache` via reflection after unload.

## 2. ALC teardown does not prune `SystemNavigationManager` event subscriptions

**Area**: SystemNavigationManager / secondary-ALC app support
**Version**: 6.7.0-dev.815 (Skia desktop + wasm)

A guest app that subscribes to the process-wide
`SystemNavigationManager.GetForCurrentView().BackRequested` (as the Uno.Themes sample `Shell`
does) is never unsubscribed by ALC teardown — `PruneCollectibleAlcEventSubscriptions` does not
cover this singleton's event fields (`_backRequested`, `InternalBackRequested`). The stale
handler roots the guest `Shell` and, through it, the guest's entire visual tree and ALC
(verified via heap dump).

**Suggested fix**: include the `SystemNavigationManager` event fields in the per-ALC event
subscription sweep (prune handlers whose target — or, for static handlers, declaring module —
lives in a collectible ALC).

**Workaround**: the hosting app prunes guest-ALC handlers via reflection after unload.

## 3. Guest finalizers re-populate swept caches during unload

**Area**: Application.CleanupNonDefaultAlcCaches / secondary-ALC app support
**Version**: 6.7.0-dev.815 (Skia desktop + wasm)

`ExitAlcApplication()` sweeps the per-ALC static caches, but guest `DependencyObject`
finalizers still run *afterwards*, during `AssemblyLoadContext.Unload()`, and can re-populate
the shared property-system caches — observed via heap dump as a guest `ControlExtensions`
attached-property entry re-rooting the dying ALC after the sweep had already run.

**Suggested fix**: re-run (or defer) the non-default-ALC cache sweep after the unloading
context's finalizers have drained, or make the caches ignore registrations originating from an
unloading ALC.

**Workaround**: the hosting app waits for finalizers and re-invokes
`Application.CleanupNonDefaultAlcCaches` via reflection.

## 4. Native X11 window/GL context leaks per ALC-guest window create/close cycle

**Area**: Skia X11 backend / secondary-ALC app support
**Version**: 6.7.0-dev.815 (Skia desktop, X11)

Each hosted-guest window create/close cycle leaks its native X11 GL context and render
threads: ~12–15 MB native per cycle, with the `llvmpipe` thread-group count growing 1:1 with
cycles (measured across a 16-cycle load/unload soak; the *managed* side is fully reclaimed —
15/15 guest ALCs collected, flat managed heap). Reproduces with and without an explicit
`Window.Close()` before `Application.Exit()`, so the leak sits in the ALC guest-window native
teardown rather than in anything the host can reach.

**Suggested fix**: release the GL context/render-thread resources in the X11 host's
ALC-guest-window teardown path.

**Workaround**: none available host-side; bounded in practice by the number of guest switches
in a session.
