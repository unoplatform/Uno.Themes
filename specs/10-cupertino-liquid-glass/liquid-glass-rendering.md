# Liquid Glass rendering on Uno Skia — technical design

Companion to `progress.md`. Scope: Skia-rendered Uno heads only (Desktop, WebAssembly, iOS, Android via
`SkiaRenderer`). The WinAppSDK head (`net10.0-windows10.0.19041`) must still *compile and run* the theme,
but only gets the solid fallback tier.

Confidence tags from the survey: **V-src** read in unoplatform/uno or mono/SkiaSharp source, **V-doc** read
in official docs, **Inf** inferred and to be confirmed by the Phase 0.5 spike.

Revision 2026-09-18 (post skeptic review): collapsed to **two tiers** (the composition "Frosted" tier was
unreachable on every in-scope platform by the tier-selection rule itself), removed the CPU normal-map path
for the same reason, dropped `IsBackdropFrozen` (undefined on a recording canvas), typed the tint as a
`Color`, added parameter clamping and a bounded cache, made the theme settings construction-time.

## 1. What Liquid Glass is, visually

Apple's iOS 26 / macOS 26 material, decomposed into the layers we have to reproduce:

| Layer | Effect | Mechanism here |
|---|---|---|
| Backdrop blur | Gaussian blur of what is behind the element; sigma grows with thickness (buttons ~12, popovers ~28, dialogs / sheets ~30) | `SaveLayer` backdrop image filter |
| Vibrancy | Saturation boost (1.15 bars, 1.5–1.8 popovers, ~4 dialogs) | colour-matrix filter |
| Adaptive tint | Translucent white or black veil chosen from backdrop luminance, plus the material tint (`#8CF2F2F5`-class for buttons, near-opaque for bars) | luminance sample + colour filter |
| Lensing / refraction | Content near the rim displaced along the surface normal, strongest at the edge and ~0 in the centre; mild interior magnification | displacement map from a rounded-rect SDF |
| Specular rim | Thin bright stroke along the lit edge, dimmer opposite | gradient stroke, 0.667 px |
| Inner shadow | Soft dark band inside the bottom edge | stroke + mask blur inside the clip |
| Drop shadow | Contact (tight) + ambient (wide, offset) | analytic shadow (`ElevatedView`) |
| Interactivity | Press scales the surface up (1.04 buttons, 1.3–1.5 knobs), brightens the light, springs back; adjacent glass morphs together | RenderTransform + animated parameters |
| Accessibility | Reduce Transparency → opaque fill; Reduce Motion → no elastic effects | Solid tier; zeroed durations |

Not in v2: chromatic dispersion (three GPU passes; follow-up), sibling morphing (`GlassEffectContainer`),
device-motion specular. Reference parameter sets per Apple material are in `reference-cupertino-avalonia.md` §6.

## 2. What Uno Skia gives us today (Uno 7.0-dev, SkiaSharp 4.151)

| Capability | Status | Evidence |
|---|---|---|
| `Compositor.CreateBackdropBrush()` + `CompositionEffectBrush` (GaussianBlur / Saturation / Tint / Blend) | Implemented on Skia; renders through `canvas.SaveLayer(new SKCanvasSaveLayerRec { Backdrop = filter })`; sets `RequiresRepaintOnEveryFrame` and `DamageRegionSamplingMargin = 3 sigma`. The Win2D effect classes are `internal` (consumers hand-implement `IGraphicsEffectD2D1Interop`). Kept here as the fact that proves backdrop reads are a supported pipeline contract; **not used** by this design (see §3) | V-src `CompositionEffectBrush.skia.cs`; V-doc composition.html |
| `AcrylicBrush` | Renders on Skia (backdrop source). `SkiaAcrylicBrush.skia.cs`: blur → Luminosity blend → Color blend with tint, 100 px padding, **downsamples by sigma/8 when sigma ≥ 16** | V-src |
| `CompositionCapabilities.GetForCurrentView().AreEffectsFast()` | `!IsSoftwareRenderer`. Runtime-effect based effects are identity pass-throughs on the software renderer | V-src |
| `SKCanvasElement` (`Uno.WinUI.Graphics2DSK`) | Abstract `FrameworkElement`, `RenderOverride(SKCanvas, Size)`, draws on the **shared frame canvas** (`SKCanvasVisual : ContainerVisual`; `Paint` clips to its size and invokes the callback). `IsSupportedOnCurrentPlatform()` is false on WinAppSDK (ctor throws) | V-doc controls/SKCanvasElement.html; V-src `SKCanvasVisual.cs` |
| Backdrop reads from `SKCanvasElement` | Uno records each visual's `Paint` into an `SKPicture` and replays it on the frame canvas; Skia records `SaveLayerRec` including `fBackdrop`, so a backdrop `SaveLayer` issued in `RenderOverride` is evaluated against the live destination at replay — the same contract `SkiaAcrylicBrush` relies on | V-src pipeline; **V-spike** on the Desktop software renderer (§8 items 1, 2, 4); GPU backends still open |
| `SKCanvasElement` damage handling | Declares neither `DamageRegionSamplingMargin` nor `RequiresRepaintOnEveryFrame`; a change behind the glass but outside its bounds may leave a stale halo (**open** — needs an on-screen capture, §8 item 2). `Opacity` does **not** modulate the backdrop layer (**V-spike**, §8 item 3) | V-spike / open |
| Recording canvas | `canvas.Surface` / `Snapshot()` are unavailable inside `RenderOverride` (the canvas is an `SKPictureRecorder` canvas) — no "freeze the backdrop" primitive | V-src `Visual.skia.cs` `PaintStep` |
| `SKImageFilter.CreateDisplacementMapEffect(xChannel, yChannel, scale, displacement, input, crop)` | Available (SkiaSharp 3.119+ / 4.x). `liquid-glass-react` does refraction exactly this way (SDF → displacement map) | V-src mono/SkiaSharp `SKImageFilter.cs` |
| `SKRuntimeEffect` (SkSL) | Production-used by Uno (`ToColorFilter`, `ToShader`) on every GPU backend incl. WASM WebGL2. **Not** available as an image filter with a backdrop child (`SKRuntimeEffect.ToImageFilter` = open PR mono/SkiaSharp#3778, target 4.153-preview) | V-src / V-doc |
| `UIElement.Shadow` / `ThemeShadow`, `ElevatedView` | Analytic drop shadows on Skia (`Visual.ShadowState`); drop only. Material v2 already uses `ElevatedView` | V-src |
| Toolkit `ShadowContainer` | `SKCanvasElement`-based; **inner shadows** = wide stroke + `SKMaskFilter.CreateBlur` inside `ClipRoundRect` | V-src uno.toolkit.ui |
| `UISettings.AdvancedEffectsEnabled` | `[NotImplemented]`, returns `true`; no OS Reduce Transparency signal on Skia | V-src |
| `RenderTargetBitmap.RenderAsync` | Works on Skia but forces the software path during capture; it **does** replay a descendant's backdrop `SaveLayer` against the offscreen target, including the popup layer when the topmost visual is captured | **V-spike** (§8 item 7) |
| WASM renderer | WebGL2 (Ganesh) by default; `putImageData` software fallback when WebGL is missing; `IsSoftwareRenderer` set accordingly | V-src `BrowserRenderer.cs` |
| ALC hosting | `Uno.WinUI.Graphics2DSK` is already on `ThemesSampleApp`'s shared-assembly list, so a backplate type in the guest is a collectible-ALC subclass of a host type | V-src `GuestHosting/GuestSharedAssemblies.txt` |
| Forward risk | PR unoplatform/uno#24153 makes Skia one pluggable backend and adds WebGPU without `SKCanvas`. An `SKCanvasElement` backplate is unsupported there; a composition-brush tier would be the answer *then* | V-doc (unmerged) |

## 3. Decision: two-tier `GlassPanel`

One public primitive, `Uno.Cupertino.GlassPanel`, picks a renderer at load time and degrades to a solid
fill. Every Cupertino template that shows glass places a `GlassPanel` as its bottom layer and puts ordinary
XAML on top. Nothing else in the theme knows how glass is drawn. Glass is used only where Apple uses it —
bars, floating controls, popovers, dialogs, sheets, opt-in buttons — and **never inside item templates**.

```
GlassPanel : Control                          (public, Uno.Cupertino.WinUI)
  Material       : GlassMaterial   (Regular | Prominent | Thin | Thick | Clear)   explicit underlying type
  TintColor      : Color           (alpha = tint strength; defaults from the material preset)
  CornerRadius   : CornerRadius    (inherited; 9999 = capsule)
  IsInteractive  : bool            (press scale + light response)
  RenderingMode  : GlassRenderingMode (Auto | Liquid | Solid)   default Auto
  -- advanced overrides, NaN = preset: BlurRadius, Saturation, Refraction, Thickness, LightIntensity,
     Fresnel, Magnification. Clamped in the PCC (sigma ≤ 64, refraction ≤ 32, saturation 0–4,
     magnification 1–1.5); non-finite → preset. PCCs never throw.
  -- template part chosen once, on Loaded:
     Tier "Liquid" : SkiaGlassBackplate : SKCanvasElement     (GPU Skia heads)
     Tier "Solid"  : Border { Background = tint over SurfaceBrush } + 0.5 px OutlineBrush hairline
                     + ElevatedView shadow                     (software renderer, WinAppSDK,
                                                                Reduce Transparency, RenderingMode=Solid)
```

Tier selection in `Auto`, evaluated on `Loaded`:

```
if RenderingMode == Solid                                          -> Solid
else if ThemeResource CupertinoReduceTransparency == true           -> Solid
else if SKCanvasElement.IsSupportedOnCurrentPlatform()
        && CompositionCapabilities.GetForCurrentView().AreEffectsFast()   -> Liquid
else                                                                -> Solid
```

`ReduceTransparency`, `ReduceMotion` and `GlassRenderingMode` are DPs on `CupertinoTheme`, written as
resources (`CupertinoReduceTransparency`, `CupertinoReduceMotion`, `CupertinoGlassRenderingMode`) through
`AddThemeSpecificResources`. They are **construction-time** settings, like `DefaultCornerRadius`: a
`GlassPanel` reads them when it loads; changing them later affects panels created afterwards, and content
already on screen re-resolves on a theme-change pass or when the root content is recreated. This avoids a
process-wide static that two theme instances (app-level plus a scoped one, the `Given_Fonts` scenario) would
fight over.

### Tier "Liquid" — `SkiaGlassBackplate : SKCanvasElement`

Partial class excluded on the Windows TFM. `RenderOverride(canvas, size)`:

1. Build the `SKRoundRect` (or capsule) from `CornerRadius` and size.
2. Get the backdrop filter chain from a small cache keyed by `(quantised size (4 px), scale, preset)`,
   LRU-capped at 32 entries — buttons come in arbitrary widths and `WebAssembly.Memory.grow()` is
   irreversible (AGENTS.md §2). Chain:
   - **Refraction**: `CreateDisplacementMapEffect(R, G, refraction × scale, displacement, null, bounds)`
     where `displacement` = `CreateShader(normalMapEffect.ToShader(uniforms))` — an `SKRuntimeEffect` with
     no children (available today) that evaluates the rounded-rect SDF gradient and encodes the outward
     normal × edge band `1 − sqrt(1 − band²)` into R / G. GPU only; the software renderer never reaches this
     tier, so there is no CPU map path.
   - **Blur**: downsample when sigma ≥ 16 (`CreateMatrix(1/k)` → `CreateBlur(sigma/k, sigma/k, Clamp)` →
     `CreateMatrix(k)`, `k = sigma/8`, mirroring `SkiaAcrylicBrush`). Always `SKShaderTileMode.Clamp` so the
     blur never samples outside its own bounds.
   - **Vibrancy + tint**: `CreateColorFilter(saturationMatrix ∘ tintMatrix)`; adaptive veil from a 9-tap
     luminance estimate of the blurred layer through a `ToColorFilter` runtime effect.
3. `canvas.Save()`, `canvas.ClipRoundRect(rrect, antialias: true)`, **then**
   `canvas.SaveLayer(new SKCanvasSaveLayerRec { Bounds = rrect bounds, Backdrop = chain, Paint = opacityPaint })`.
   The order is load-bearing: the layer is filled with the filtered backdrop at `SaveLayer` time and is
   composited through the clip that was active *then*, so a clip issued after it only trims the rim and tint
   and the blurred backdrop leaks into the square corners (measured, §8). `opacityPaint` carries the
   effective opacity of the panel and its ancestors — the pipeline does not apply `Opacity` to the layer
   (§8 item 3).
4. Inside the layer: specular rim (gradient stroke 0.667 px along the lit edge, `Fresnel × LightIntensity`),
   inner shadow (wide stroke + `SKMaskFilter.CreateBlur` after the clip, Toolkit's recipe).
5. `canvas.Restore()` twice (layer, then clip). The drop shadow is drawn by `ElevatedView` on the `GlassPanel`, not here.

Repaint model: `Invalidate()` on `SizeChanged` and on any parameter DP change. While a panel animates
(press / release, parameter animation) it subscribes to `CompositionTarget.Rendering` and calls
`Invalidate()` per frame **for the animation duration only**; the subscription is dropped on completion and
in `Unloaded` (a `Loaded → Unloaded → Loaded` cycle must not double-subscribe — guarded by a field). Static
glass over static content is recorded once and replayed by Uno's picture cache. A glass bar over a
*scrolling* list is the canonical hard case and is spike item 5.

Upgrade path: when `SKRuntimeEffect.ToImageFilter` ships, the chain collapses into one runtime image
filter (Kyant0/AndroidLiquidGlass style). The chain lives in one method so the swap is local.

### Tier "Solid"

Tint composited over `SurfaceBrush` + 0.5 px `OutlineBrush` hairline + `ElevatedView` shadow. This is the
Reduce Transparency rendering, the WinAppSDK rendering, the software-renderer rendering, and what every
CI assertion checks first. Identical in spirit to Cupertino.Avalonia's `RenderPlain`.

### Why no composition-brush tier

The survey's Option 1 (a `XamlCompositionBrushBase` over `CreateBackdropBrush` + hand-built effect nodes)
was dropped: with the selection rule above it would only ever run on a GPU WinAppSDK head, which is out of
scope, while costing ~30 lines per effect node and forcing `RequiresRepaintOnEveryFrame` on every surface.
It returns as a follow-up if the WebGPU (non-`SKCanvas`) backend in uno#24153 lands.

## 4. Package and TFM impact (decision D-5, taken after the spike)

- Preferred: `Uno.Cupertino.WinUI` references `Uno.WinUI.Graphics2DSK` (brings SkiaSharp transitively);
  `SkiaGlassBackplate` compiled out on `net10.0-windows10.0.19041`. Hidden cost the spike must quantify:
  SkiaSharp native binaries are pulled into `net10.0-ios` / `net10.0-android` consumers that use the
  native renderer, and into the Windows TFM where the backplate cannot render.
- Alternative: a separate `Uno.Cupertino.WinUI.Skia` package (Toolkit's `Uno.Toolkit.Skia.WinUI` model).
  One more package to explain, but zero cost for non-Skia consumers.
- Either way: `ThemesSampleApp` hosting smoke must stay green with a guest-ALC subclass of the host's
  `SKCanvasElement` (Phase 2b gate).
- Independent of D-5: drop `UnoFeatures=Lottie` (D-7); the activity indicator becomes an 8-spoke XAML
  storyboard. Breaking: `CupertinoDeterminateAnimation_Uno` / `CupertinoIndeterminateAnimation_Uno` keys
  are removed and consumers lose the transitive `Uno.WinUI.Lottie` reference (§8 of the mapping).

## 5. Performance rules (WASM first)

- A glass surface costs one readback of `bounds + padding`, one separable blur (area × sigma) and one
  recomposite every frame it is *repainted*; displacement adds one full-area pass. Budget: a handful of
  large surfaces (navigation bar, tab bar, one popover or sheet) at 60 fps on WebGL2. **Never** per-item
  glass inside lists; rows use solid fills.
- Downsample before blurring (sigma ≥ 16 → k = sigma/8). Bounded, quantised filter cache (above).
- Per-frame `Invalidate()` only for the duration of an animation; static panels never force repaints.
- Reduce Motion zeroes every Cupertino `Duration` resource at construction time and `GlassPanel` skips its
  press animation when `CupertinoReduceMotion` is true.

## 6. Verification plan for the primitive

Runtime tests live in `src/samples/CupertinoSampleApp/RuntimeTests/` (the Simple host cannot reference
Uno.Cupertino). Desktop Skia under Xvfb is the CI target; its software renderer means CI exercises tier
selection, the API contract and the Solid tier, while Liquid pixels are verified on a GPU locally and in
the WASM smoke.

- `Given_GlassPanel.When_ModeAuto_Then_TierMatchesCapabilities` — asserts the chosen template part against
  `IsSupportedOnCurrentPlatform()` / `AreEffectsFast()`.
- `When_ReduceTransparency_Then_SolidTier` — theme created with the flag; panel loads Solid.
- `When_ParameterOutOfRange_Then_Clamped` — negative / huge / NaN values never reach the filter chain.
- `When_Unloaded_MidAnimation_Then_RenderingUnsubscribed` — leak guard per AGENTS.md §2: `WeakReference`
  fields, GC in a `[MethodImpl(NoInlining)]` method.
- `When_SizeChanges_Then_FilterChainCacheBounded` — cache count never exceeds the cap across 100 sizes.
- Liquid-tier pixel test: a panel with `RenderingMode=Liquid` over a hard black / white edge shows lower
  edge variance inside than outside, captured with `RenderTargetBitmap` on an ancestor (spike item 7). The
  software renderer executes the whole chain including the SkSL normal map, so this runs on CI under Xvfb
  with **no** capability gate; `Auto` still selects Solid there, which is why the test forces the mode.
- Visual: `LiquidGlassSamplePage` renders every material over a photo wallpaper in light and dark;
  screenshots from Desktop GPU, WASM WebGL2 and Android attached to the PR.

## 7. Spike (Phase 0.5) — questions the prototype answers before D-5 is taken and before any control uses glass

Throwaway prototype in `CupertinoSampleApp`, not the library.

1. Does a backdrop `SaveLayer` inside `SKCanvasElement.RenderOverride` read the content beneath on Desktop
   (OpenGL and software), WASM (WebGL2 and software) and Android? Screenshots per target.
2. Stale-halo behaviour under damage-region repaint when content *behind* the panel changes; is `Clamp`
   tile mode enough, or does the panel need per-frame invalidation whenever an ancestor scrolls?
3. Does `Opacity` on the `GlassPanel` modulate the custom drawing?
4. Does the backplate render correctly **inside a `Popup` / `Flyout` / `ContentDialog`** (overlay layer) —
   the heaviest glass users are popovers, menus and alerts.
5. Glass bar over a scrolling `ListView`: frame time and correctness while the list scrolls under it.
6. WASM WebGL2 frame time: 5 glass surfaces at 1280×800, with and without downsampling.
7. Can a test capture the blurred result? Try `RenderTargetBitmap` on an ancestor, then the runtime-test
   engine's screenshot helper; record which one sees the backdrop `SaveLayer`.
8. Does `Uno.WinUI.Graphics2DSK` restore for the plain `net10.0` library TFM; does the Windows TFM compile
   with the backplate excluded; what does it add to an iOS / Android native-renderer consumer (D-5)?
9. Which family name Uno Skia resolves for the system font on macOS / iOS (`.AppleSystemUIFont`,
   `SF Pro Text`, `SF Pro`) — needed for the one-line SF opt-in in the docs (D-3).

Gate: findings recorded here, every **Inf** tag replaced by a verified result or a design change.

## 8. Spike results — 2026-09-18, Desktop (Win32) software renderer only

Prototype: `src/samples/CupertinoSampleApp/Spike/GlassSpike.cs`, launched with `--glass-spike` (interactive)
or `--glass-spike=<dir>` (scripted; writes `RenderTargetBitmap` captures of the topmost visual and
`info.txt`). `--glass-spike-noglass` hides every panel for a baseline. Uno.Sdk 7.0.0-dev.701,
SkiaSharp 4.151.1, Release, `net10.0-desktop`.

**Environment limit, stated up front:** the machine this ran on has no GPU (the only adapter is an ASPEED
BMC) and the session was a disconnected RDP session. Uno therefore selected the software renderer
(`AreEffectsFast() == false`), the screen DC did not exist (no window screenshots), and the frame clock was
throttled to ~4 fps **with and without glass** (3.9 vs 3.9–4.0), so no frame-time number from this run
means anything. Everything below is the software path, observed through `RenderTargetBitmap`.

| # | Question | Result |
|---|---|---|
| 1 | Backdrop `SaveLayer` inside `RenderOverride` reads what is beneath | **Yes on Desktop software.** Blur, saturation and the SkSL displacement map all render (the runtime effect executes on the CPU backend — it is not an identity pass-through). **Open:** Desktop OpenGL, WASM WebGL2 / software, Android |
| 2 | Stale content when the backdrop changes | `RenderOverride` ran 6–8 times in a 12 s run while a bar animated behind five panels and a list scrolled under a sixth, and every capture showed the blur tracking the live content — the recorded picture is replayed against the current destination with no `Invalidate()`. **Open:** the on-screen damage-region path (a `RenderTargetBitmap` capture is a full fresh render and cannot show a stale halo) |
| 3 | `Opacity` modulates the custom drawing | **No.** A panel under `Opacity="0.3"` produced byte-identical pixels to the same panel at 1.0 while its sibling `TextBlock` faded. **Design change:** the backplate multiplies its own and its ancestors' `Opacity` into the layer `Paint` (§3 step 3), re-evaluated on `Invalidate()` |
| 4 | Inside `Flyout` / `ContentDialog` | **Works** (software): both read the content beneath the overlay layer; in a dialog the backdrop is the already-dimmed content, which is the desired look |
| 5 | Glass bar over scrolling content | **Correct** at every capture with no per-frame invalidation. **Open:** frame time |
| 6 | WASM WebGL2 frame time, five surfaces | **Open** — no GPU, no usable browser session |
| 7 | Capture method for the pixel test | **`RenderTargetBitmap` on an ancestor** sees the backdrop layer, down to the `XamlIslandRoot` (popups included). §6 updated: the pixel test is no longer GPU-gated |
| 8 | Package / TFM cost (D-5) | From the 7.0.0-dev.701 nuspec, not a build: `lib/net10.0`, `lib/net10.0-windows10.0.19041.0`, `lib/net11.0`, so it restores for the plain library TFM and for Windows. **Every** dependency group — Windows, iOS, Android included — takes `SkiaSharp 4.151.1`. Sample heads already receive the package through `UnoFeatures=SkiaRenderer`, so for Skia consumers the reference adds nothing; the cost lands only on native-renderer and WinAppSDK consumers |
| 9 | System-font family name on Apple hosts | **Open** — needs a macOS / iOS host |

Additional finding — **clip order.** The draw order this document originally specified (`SaveLayer`, then
`ClipRoundRect`) leaks the blurred backdrop into the corners: pixels outside the arc over a pure black
backdrop measured 2–8 / 255 against 0 for clip-then-`SaveLayer`. §3 step 3 is corrected.

Gate status: **provisional go.** Correctness of the Liquid tier is established on the one backend this
machine has, and it holds in the overlay layer. What is still unmeasured is exactly what the tier exists
for — GPU behaviour and cost. Items 1 / 2 / 5 / 6 on a GPU desktop and in a WebGL2 browser, and item 9 on an
Apple host, need a run of `CupertinoSampleApp --glass-spike` on real hardware before Phase 2 starts; the
spike file stays in the sample head until then. Phase 1 does not depend on any of it.
