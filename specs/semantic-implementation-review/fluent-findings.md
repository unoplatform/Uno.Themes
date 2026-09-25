# Fluent semantic implementation review

Review date: 2026-09-06. Scope: the current `Uno.Fluent.WinUI` implementation,
shared override integration, Fluent runtime tests, and published documentation.
This is a source audit. No runtime reproduction, build, or platform validation
was performed by this review lane. Line references identify the implementation
reviewed before documentation edits.

## Overall assessment

Fluent publishes the semantic control-style names and 19 typography styles.
Its use of built-in styles, nearest-match FAB/elevated aliases, and explicit
empty styles where no public platform style key exists is deliberate. Empty
styles preserve default-template resolution; they do not retrieve the platform's
actual default style object. These choices are not themselves defects.

Resource-name availability does not establish override parity. Native Fluent
templates consume their own resource names; only part of the semantic override
surface is translated. The gaps below separate direct source evidence from
runtime behavior that still needs a regression test.

## F1 — P2: merged override dictionaries are ignored by both bridges

Evidence:

- `src/library/Uno.Fluent.WinUI/FluentAccentPalette.cs:225–235` reads only a
  dictionary's own `PrimaryColor` and immediate appearance dictionaries.
- `FluentAccentPalette.cs:397–431` copies explicit accent-family overrides from
  those same locations. `ToOwnEntries` at line 486 never visits merged children.
- `src/library/Uno.Fluent.WinUI/FluentLightweightBridge.cs:313–320` follows the
  same pattern; its helper at line 372 also never visits merged children.
- `src/library/Uno.Themes/Extensions/ResourceDictionaryExtensions.cs:52–57`
  attaches the complete consumer dictionary graph for ordinary lookup.

Put a dictionary containing `PrimaryColor` or `FilledButtonBackground` in
`Colors.OverrideDictionary.MergedDictionaries`. Normal resource lookup can find
the override, but accent-basis resolution or lightweight repointing cannot.
When a seed is present, the later generated layer can also shadow an explicit
native accent resource contained in a merged child.

Required validation: flat and appearance-specific merged children, merged
children inside appearance dictionaries, competing siblings, and explicit
native accent keys with a seed. Assert direct resources and rendered controls.

## F2 — P2, runtime confirmation required: Dark-only accent overrides can fall back into Light

Evidence: `FluentAccentPalette.cs:265–278` creates a `Light` branch only when
there is a light driver, and creates a `Default` branch for a dark driver. With
only `ThemeDictionaries["Dark"]["PrimaryColor"]` and no seed,
`ResolveAccentBasis` returns `(null, dark)` and the generated dictionary therefore
has only `Default`. The comment that an undriven branch retains platform values
does not account for `Default` being the universal appearance fallback.

The generated graph can let Light lookups pick the Dark override's native accent
closure. `ApplyConsumerAccentOverrides` can create the same graph for an
appearance-specific explicit accent-family key. Existing tests supply both
appearances or a flat override. The test helper `FindBranchValue` at
`Given_FluentSeedAccent.cs:157` searches the requested branch literally and does
not model `Default` fallback, so a new test using only that helper could miss it.

Required validation: a Light app with a Dark-only override and no seed, asserting
both semantic `PrimaryColor` and native `AccentFillColorDefaultBrush` against the
platform baseline; repeat with the inverse case. This review has not rendered
the suspected leak and does not claim a runtime reproduction.

## F3 — P2: constructor color overrides bypass accent and lightweight processing

Evidence: `FluentTheme.cs:103–106,124–134` folds the constructor argument into the
palette supplied to `BaseTheme`. `FluentTheme.cs:189–199` reads only
`Colors.OverrideDictionary` when selecting the bridge input.

Consequently `new FluentTheme(colorOverride: overrides)` can change semantic
colors without driving built-in controls, whereas assigning the same dictionary
to `Colors.OverrideDictionary` reaches the bridge. A constructor-supplied
lightweight value can additionally be shadowed by a later Fluent default.

Required validation: compare constructor, modern dictionary/source, and obsolete
dictionary/source channels using `PrimaryColor` and `FilledButtonBackground`.

## F4 — P2: text-button state/background resources have no consumers

Evidence:

- `Styles/Controls/Button.xaml:21–23` sets literal transparent background/border
  and consumes only `TextButtonForeground`.
- `FluentLightweightBridge.cs:180–189` mirrors the pointer-over and pressed
  foregrounds, background family, and border brush as semantic resources only.
- Neither supplied button style reads those state/background/border keys; the
  built-in template uses native Button interaction resources.

`TextButtonForegroundPointerOver`, `TextButtonForegroundPressed`,
`TextButtonBackground*`, and `TextButtonBorderBrush` can resolve to consumer
values without affecting their supposed visual parts. The rendered test at
`Given_FluentLightweightStyling.cs:191–207` covers only rest foreground; resource
presence/value assertions do not prove state behavior.

Documentation corrections: `doc/styles/Button.md:8` and
`doc/lightweight-styling.md:257` previously advertised the entire
`TextButtonForeground*` family. Only rest `TextButtonForeground` and
`IconButtonForeground` are directly consumed by the supplied styles.

Required validation: realize text buttons, drive rest/hover/pressed states,
and assert template visual brushes after app- and page-scoped overrides.

## F5 — P2: the shared root font resource does not cascade in Fluent

Evidence: `Styles/Application/Fonts.xaml:35–66,74–105` aliases both the root and
every slot directly to `ContentControlThemeFontFamily`. The header explicitly
explains that this avoids alias chaining on Uno.

A `FontOverrideDictionary` containing `DefaultFontFamily` changes that resource
but not the slots. Material/Simple slots reference `DefaultFontFamily` and thus
support that app-level override route. The `FluentTheme.DefaultFontFamily`
property works because the generated layer writes concrete values for every
slot; `BuildPlatformTokenOverrides` also writes the native font token.

Required validation: apply the same app-level root override dictionary under
all three themes, then test per-slot overrides, the property, and clear/restore.
Current Fluent typography tests cover the property, not root dictionary parity.

## F6 — P2: semantic brush/color overrides do not generally reach native controls

Evidence: `FluentAccentPalette.cs:232–235` selects only `PrimaryColor` as a driver;
`WriteClosure` at lines 385–388 computes on-accent text without reading semantic
`OnPrimaryColor` or `OnPrimaryBrush`. `PrimaryBrush` is also not a driver.

For comparison, Material `Styles/Controls/v2/Button.xaml:35–45` and Simple
`Styles/Controls/Button.xaml:38–46` alias filled-button resources directly to
`OnPrimaryBrush` and `PrimaryBrush`.

An explicit shared `OnPrimaryColor` or `OnPrimaryBrush` foreground override can
therefore change semantic lookup and Material/Simple buttons while Fluent
buttons keep the stock/generated foreground. An explicit `PrimaryBrush` fill
override similarly does not drive native Fluent accent fill. Native default
visual differences are intentional; ignoring the same explicit semantic
override is still a portability limitation against the requested contract.

Required validation: the same color- and brush-level overrides applied to
realized filled buttons under each theme, including brush instances with opacity.

## F7 — P2: the semantic palette captures the system accent once

Evidence: `FluentColorPalette.cs:80–108` reads platform resources into concrete
colors. `FluentTheme.cs:127` populates them at construction; the retry at
`FluentTheme.cs:161` runs only while the palette has zero appearance dictionaries.
There is no system accent-change subscription in the Fluent library.

Once populated, the semantic palette does not reread a changed Windows system
accent, even on an unrelated theme-property rebuild. Built-in controls retain
their platform accent behavior while semantic colors can become stale. Published
wording that semantic roles "track" the system accent overstated this lifecycle.

Required validation: change the real Windows accent with an unseeded Fluent
theme alive and compare semantic resources and native controls; verify any
future event subscription teardown.

## F8 — P2 validation gap: live seed updates replace native brushes

The replacement mechanism is confirmed in source:

- `FluentTheme.cs:212–221` creates a new accent dictionary when the seed or mode
  changes; `FluentAccentPalette.cs:363–388` allocates new closure brushes.
- `FluentTheme.cs:234` rebuilds the lightweight layer every pass;
  `FluentLightweightBridge.cs:270–294` creates new default brushes.
- `BaseTheme.cs:570–577` removes previous dynamic dictionaries. The persistent
  shared semantic brush updater does not update references to old Fluent accent
  or lightweight brushes, and the Fluent hook requests no control refresh.

Whether each target re-resolves existing controls automatically on these nested
dictionary replacements is not established by this audit. An already realized
control can retain its previous brush unless the framework refreshes it. Do not
present that as a newly reproduced failure without a rendered test.

Coverage inspected:

- `Given_FluentSeedAccent.When_SeedSet_RenderedAccentButtonFollowsSeed`
  (`:450`) sets the seed before creating the button.
- `When_SeedCleared_PlatformAccentRestored` (`:486`) clears while attached but
  asserts resource values, then unmerges the theme and creates a fresh button.
  It expressly acknowledges native materialized-brush caching at lines 500–503.
- There is no seed-A to seed-B test asserting the same rendered button before
  and after. Rendered lightweight tests likewise configure overrides first.

Required validation: realize native accent and semantic filled buttons under
seed A; change to seed B, change generation mode, and clear the seed; assert the
same controls' fill/foreground and held brush references after idle, without
navigation/theme toggles. Test both Desktop and WebAssembly. Repeat for a
lightweight override replacement. The known seed-clear cache limitation is
already described in `doc/seed-colors.md` and is now cross-linked from Fluent
getting started.

## Intentional limits that still matter for portability

- Divergent page/subtree semantic keys require native Fluent resource names;
  the app-wide bridge only processes `Colors.OverrideDictionary`.
- Filled-tonal/elevated buttons share standard Fluent resources and cannot be
  customized independently through their semantic resource families.
- Some semantic resources have no native equivalent. Some disabled/state keys
  are override-only and have no declared Fluent default.
- Spacing/density tokens exist but do not affect native template metrics.
- A few native templates use static corner-radius resources and therefore do
  not follow the property bridge.
- The implemented seed fill is Dark1 in Light and Light2 in Dark. This differs
  from semantic Primary in Light by design. An explicit PrimaryColor override
  follows a different rule: it becomes the native fill verbatim.
- Existing source/spec records leave native Windows and WebAssembly behavior
  to additional validation; Desktop tests alone are not proof of parity.

## Documentation work in this review lane

Updated only `doc/fluent-getting-started.md`: removed the universal override
implication, clarified construction-time accent capture, explained explicit
primary versus seeded fill, and documented supported override shapes/channels,
text-button limits, typography-root differences, and native brush lifecycle.
No product behavior was changed. Other central pages are owned by the primary
reviewer.

Additional corrections passed to the primary reviewer:

- Semantic-style notes should describe empty styles preserving native default
  templates, not runtime retrieval of built-in style objects.
- Lightweight descriptions should distinguish declared defaults, override-only
  keys, and actual rendered consumers.
- `doc/design-tokens.md` says FontOverrideSource is reread on every rebuild;
  `BaseTheme.cs:623` onward now caches it across unrelated rebuilds and reloads
  after invalidation.

## Verification

The evidence above comes from repository source and test inspection. No test
was added, removed, skipped, or modified. The lane did not run builds or runtime
tests. Runtime confirmation remains explicitly outstanding for F2 and F8;
rendered regression coverage is recommended for every override finding.
