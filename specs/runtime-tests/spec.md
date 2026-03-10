# Runtime Tests for Uno.Themes

## Overview

This spec defines a runtime UI testing strategy for the Uno.Themes repository using [Uno.UI.RuntimeTests.Engine](https://github.com/unoplatform/uno.ui.runtimetests.engine). Tests run inside a live XAML host, enabling validation of style resolution, template structure, theme resource binding, visual state transitions, and Light/Dark theme dictionary parity.

### Goals

- **Catch regressions** when XAML styles are refactored (missing template parts, broken resource chains, copy-paste drift between Light/Dark themes)
- **Validate design tokens** are applied correctly (MinHeight, CornerRadius, Padding, fonts)
- **Verify visual state behavior** (expanded/collapsed, hover, pressed, disabled transitions change the expected properties)
- **Run in CI** on both Desktop (Skia) and WASM targets via Azure DevOps

### Scope

| Phase | Theme | Controls | Status |
|-------|-------|----------|--------|
| 1 (this spec) | Simple | 21 controls + `_Resources.xaml` | Planned |
| 2 (future) | Material v2 | 31 controls | Deferred |
| 3 (future) | Cupertino | 17 controls | Deferred |

Simple theme is first because it has no v1/v2 split and the SimpleSampleApp uses the public `Uno.Sdk` (not `Uno.Sdk.Private`), making it the cleanest integration target.

---

## Architecture

### Test Engine

[Uno.UI.RuntimeTests.Engine](https://github.com/unoplatform/uno.ui.runtimetests.engine) is a source-package that embeds an MSTest-compatible test runner directly into the app. Tests execute in the live XAML tree with access to all registered theme resources.

Key APIs:
- `UnitTestsUIContentHelper.Content` — places a control into the test render zone
- `UnitTestsUIContentHelper.WaitForIdle()` / `WaitForLoaded(element)` — async waits for layout
- `[TestClass]`, `[TestMethod]`, `[DataRow]` — standard MSTest discovery attributes
- `[RunsOnUIThread]` — forces execution on the dispatcher (required for XAML operations)

### Hosting Strategy

Tests are **embedded in the SimpleSampleApp** — no separate test project. This ensures tests run against the exact same `SimpleTheme` registration and resource dictionaries that the sample app uses.

A new `RuntimeTestsPage` with `<UnitTestsControl />` is added to the app. Navigation is added to the Shell via a `SamplePageAttribute`-tagged page (the existing reflection-based menu system auto-discovers it).

For headless CI, the engine auto-runs all tests on startup when the `UNO_RUNTIME_TESTS_RUN_TESTS` environment variable is set.

---

## Simple Theme Control Inventory

The following 21 control style files exist under `src/library/Uno.Simple.WinUI/Styles/Controls/` (excluding `_Resources.xaml`):

| # | File | Control Type | Named Styles |
|---|------|-------------|--------------|
| 1 | `AppBarButton.xaml` | `AppBarButton` | `SimpleDefaultAppBarButtonStyle` |
| 2 | `AutoSuggestBox.xaml` | `AutoSuggestBox` | `SimpleDefaultAutoSuggestBoxStyle` |
| 3 | `Button.xaml` | `Button` | `SimplePrimaryButtonStyle`, `SimpleNeutralButtonStyle`, `SimpleSubtleButtonStyle`, `SimpleDangerPrimaryButtonStyle`, `SimpleDangerSubtleButtonStyle` + Small/Medium variants |
| 4 | `CalendarDatePicker.xaml` | `CalendarDatePicker` | `SimpleDefaultCalendarDatePickerStyle` |
| 5 | `CalendarView.xaml` | `CalendarView` | `SimpleDefaultCalendarViewStyle` |
| 6 | `CheckBox.xaml` | `CheckBox` | `SimpleDefaultCheckBoxStyle` |
| 7 | `ComboBox.xaml` | `ComboBox` | `SimpleDefaultComboBoxStyle` |
| 8 | `ContentDialog.xaml` | `ContentDialog` | `SimpleDefaultContentDialogStyle` |
| 9 | `DatePicker.xaml` | `DatePicker` | `SimpleDefaultDatePickerStyle` |
| 10 | `Expander.xaml` | `Expander` | `SimpleExpanderStyle`, `SimpleDefaultExpanderStyle` |
| 11 | `ListView.xaml` | `ListView` | `SimpleDefaultListViewStyle` |
| 12 | `MenuFlyout.xaml` | `MenuFlyoutPresenter` | `SimpleDefaultMenuFlyoutPresenterStyle` |
| 13 | `PasswordBox.xaml` | `PasswordBox` | `SimpleDefaultPasswordBoxStyle` |
| 14 | `PersonPicture.xaml` | `PersonPicture` | `SimpleDefaultPersonPictureStyle` |
| 15 | `RadioButton.xaml` | `RadioButton` | `SimpleDefaultRadioButtonStyle` |
| 16 | `Slider.xaml` | `Slider` | `SimpleDefaultSliderStyle` |
| 17 | `TextBlock.xaml` | `TextBlock` | Typography styles |
| 18 | `TextBox.xaml` | `TextBox` | `SimpleDefaultTextBoxStyle` |
| 19 | `ToggleButton.xaml` | `ToggleButton` | `SimpleDefaultToggleButtonStyle` |
| 20 | `ToggleSwitch.xaml` | `ToggleSwitch` | `SimpleDefaultToggleSwitchStyle` |
| 21 | `ToolTip.xaml` | `ToolTip` | `SimpleDefaultToolTipStyle` |

---

## Test Categories

### Category 1: Style Resolution (Bulk)

Validates that every control type resolves an implicit style when the Simple theme is active. This is the highest-value, lowest-effort test — it catches broken style registrations and missing entries in `mergedpages.xaml`.

```csharp
[TestClass]
[RunsOnUIThread]
public class Given_SimpleStyles
{
    [TestMethod]
    [DataRow(typeof(Button))]
    [DataRow(typeof(CheckBox))]
    [DataRow(typeof(ComboBox))]
    [DataRow(typeof(Expander))]
    [DataRow(typeof(PasswordBox))]
    [DataRow(typeof(RadioButton))]
    [DataRow(typeof(Slider))]
    [DataRow(typeof(TextBox))]
    [DataRow(typeof(ToggleButton))]
    [DataRow(typeof(ToggleSwitch))]
    // ... all 21 control types
    public async Task Control_HasImplicitStyle(Type controlType)
    {
        var control = (FrameworkElement)Activator.CreateInstance(controlType);
        UnitTestsUIContentHelper.Content = control;
        await UnitTestsUIContentHelper.WaitForIdle();

        Assert.IsNotNull(control.Style,
            $"No implicit style resolved for {controlType.Name}");
    }
}
```

### Category 2: Template Part Existence

Validates that expected named elements (`x:Name`) exist in control templates. Catches XAML refactoring regressions where a template part is renamed or removed.

Example for Expander:
- `ExpanderHeader` (ToggleButton)
- `ExpanderContent` (Border)
- `ExpanderContentClip` (Border)
- `ExpandCollapseChevron` (FontIcon — inside the header's template)

```csharp
[TestMethod]
public async Task Expander_TemplateParts_Exist()
{
    var expander = new Expander { Header = "Test", Content = new TextBlock { Text = "Content" } };
    UnitTestsUIContentHelper.Content = expander;
    await UnitTestsUIContentHelper.WaitForLoaded(expander);

    Assert.IsNotNull(expander.FindFirstDescendant<ToggleButton>(x => x.Name == "ExpanderHeader"),
        "ExpanderHeader template part missing");
    Assert.IsNotNull(expander.FindFirstDescendant<Border>(x => x.Name == "ExpanderContent"),
        "ExpanderContent template part missing");
    Assert.IsNotNull(expander.FindFirstDescendant<Border>(x => x.Name == "ExpanderContentClip"),
        "ExpanderContentClip template part missing");
}
```

### Category 3: Theme Resource Binding

Validates that lightweight-styling ThemeResource keys resolve to non-null brushes. This catches broken `StaticResource` → `ThemeResource` chains (e.g., referencing a `SimpleBackgroundDefaultSecondaryBrush` that doesn't exist).

```csharp
[TestMethod]
[DataRow("SimpleExpanderHeaderBackground")]
[DataRow("SimpleExpanderHeaderForeground")]
[DataRow("SimpleExpanderChevronForeground")]
[DataRow("SimpleExpanderContentBackground")]
[DataRow("SimpleExpanderContentBorderBrush")]
public void Expander_ThemeResource_ResolvesToBrush(string resourceKey)
{
    Assert.IsTrue(
        Application.Current.Resources.TryGetValue(resourceKey, out var value),
        $"ThemeResource '{resourceKey}' not found");
    Assert.IsInstanceOfType(value, typeof(Brush),
        $"ThemeResource '{resourceKey}' resolved to {value?.GetType().Name} instead of Brush");
}
```

### Category 4: Visual State Transitions

Validates that state changes actually modify the expected properties. This catches visual states that reference wrong target names or properties.

```csharp
[TestMethod]
public async Task Expander_WhenExpanded_ChevronRotates()
{
    var expander = new Expander { Header = "Test", IsExpanded = false };
    UnitTestsUIContentHelper.Content = expander;
    await UnitTestsUIContentHelper.WaitForLoaded(expander);

    var chevron = expander.FindFirstDescendant<FontIcon>(x => x.Name == "ExpandCollapseChevron");
    var transform = chevron.RenderTransform as RotateTransform;
    Assert.AreEqual(0d, transform.Angle, "Chevron should be 0° when collapsed");

    expander.IsExpanded = true;
    await UnitTestsUIContentHelper.WaitForIdle();

    Assert.AreEqual(180d, transform.Angle, "Chevron should be 180° when expanded");
}

[TestMethod]
public async Task Expander_ContentArea_IsCollapsedByDefault()
{
    var expander = new Expander { Header = "Test", Content = new TextBlock { Text = "Body" } };
    UnitTestsUIContentHelper.Content = expander;
    await UnitTestsUIContentHelper.WaitForLoaded(expander);

    var content = expander.FindFirstDescendant<Border>(x => x.Name == "ExpanderContent");
    Assert.AreEqual(Visibility.Collapsed, content.Visibility);
}
```

### Category 5: Theme Dictionary Parity (Light / Dark)

Validates that every style XAML file with `ThemeDictionaries` has the same set of resource keys in both `Light` and `Default` (Dark) dictionaries. This catches copy-paste drift — one of the most common errors when maintaining parallel theme dictionaries with ~50 keys each.

```csharp
[TestMethod]
[DataRow("ms-appx:///Uno.Simple.WinUI/Styles/Controls/Expander.xaml")]
[DataRow("ms-appx:///Uno.Simple.WinUI/Styles/Controls/Button.xaml")]
[DataRow("ms-appx:///Uno.Simple.WinUI/Styles/Controls/TextBox.xaml")]
// ... all 21 control xaml files
public void StyleFile_LightAndDark_HaveSameResourceKeys(string xamlUri)
{
    var dict = new ResourceDictionary { Source = new Uri(xamlUri) };

    if (dict.ThemeDictionaries.TryGetValue("Light", out var lightObj)
        && dict.ThemeDictionaries.TryGetValue("Default", out var darkObj)
        && lightObj is ResourceDictionary light
        && darkObj is ResourceDictionary dark)
    {
        var lightKeys = light.Keys.Cast<string>().OrderBy(k => k).ToList();
        var darkKeys = dark.Keys.Cast<string>().OrderBy(k => k).ToList();

        var missingInDark = lightKeys.Except(darkKeys).ToList();
        var missingInLight = darkKeys.Except(lightKeys).ToList();

        Assert.AreEqual(0, missingInDark.Count,
            $"Keys in Light but missing in Dark: {string.Join(", ", missingInDark)}");
        Assert.AreEqual(0, missingInLight.Count,
            $"Keys in Dark but missing in Light: {string.Join(", ", missingInLight)}");
    }
}
```

---

## Implementation Plan

### Phase 1: Infrastructure Setup

| Step | Action | File(s) |
|------|--------|---------|
| 1.1 | Add `Uno.UI.RuntimeTests.Engine` package version | `src/samples/Directory.Packages.props` |
| 1.2 | Add `PackageReference` to SimpleSampleApp | `src/samples/SimpleSampleApp/SimpleSampleApp.csproj` |
| 1.3 | Create test runner page | `src/samples/SimpleSampleApp/Content/RuntimeTestsPage.xaml` + `.xaml.cs` |
| 1.4 | Tag page with `SamplePageAttribute` so it auto-appears in Shell navigation | RuntimeTestsPage.xaml.cs |

**1.1** — Add to `src/samples/Directory.Packages.props`:
```xml
<PackageVersion Include="Uno.UI.RuntimeTests.Engine" Version="<latest>" />
```

**1.2** — Add to `SimpleSampleApp.csproj`:
```xml
<PackageReference Include="Uno.UI.RuntimeTests.Engine" />
```

**1.3** — Create `RuntimeTestsPage.xaml`:
```xml
<Page x:Class="Uno.Themes.Samples.Content.RuntimeTestsPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:runtimetests="using:Uno.UI.RuntimeTests">
    <runtimetests:UnitTestsControl />
</Page>
```

**1.4** — Tag the page for shell discovery:
```csharp
[SamplePage(SampleCategory.None, "Runtime Tests")]
public sealed partial class RuntimeTestsPage : Page
{
    public RuntimeTestsPage() => InitializeComponent();
}
```

### Phase 2: Shared Test Helpers

| Step | Action | File |
|------|--------|------|
| 2.1 | Create `StyleTestHelper` | `src/samples/SimpleSampleApp/RuntimeTests/Helpers/StyleTestHelper.cs` |
| 2.2 | Create `ThemeDictionaryTestHelper` | `src/samples/SimpleSampleApp/RuntimeTests/Helpers/ThemeDictionaryTestHelper.cs` |

**`StyleTestHelper.cs`** provides:
- `LoadAndWait(FrameworkElement element)` — sets content, waits for loaded
- `FindTemplatePart<T>(DependencyObject root, string name)` — finds a named descendant using `VisualTreeHelperEx`
- `AssertThemeResourceResolves(string key)` — resolves from `Application.Current.Resources` and asserts non-null
- `AssertBrushResource(string key)` — resolves and asserts it's a `Brush`

**`ThemeDictionaryTestHelper.cs`** provides:
- `AssertThemeParity(string xamlUri)` — loads dict, compares Light vs Default key sets
- `GetResourceDictionaryKeys(ResourceDictionary dict, string themeKey)` — extracts keys from a theme dictionary entry

### Phase 3: Test Classes

| Step | File | Tests | Category |
|------|------|-------|----------|
| 3.1 | `Given_SimpleStyles.cs` | `Control_HasImplicitStyle` × 21 types | Style Resolution |
| 3.2 | `Given_ExpanderStyle.cs` | 9 tests (parts, tokens, states) | Template Parts, Visual States |
| 3.3 | `Given_ButtonStyle.cs` | Named style resolution, variant parts, visual states | Template Parts, Visual States |
| 3.4 | `Given_ThemeDictionaryParity.cs` | Light/Dark key parity × 21 files | Theme Parity |
| 3.5 | `Given_ThemeResources.cs` | Key resource chains resolve for all controls | Resource Binding |

**Estimated test count**: ~80-100 individual test cases across all classes.

### Phase 4: CI Integration

| Step | Action | File |
|------|--------|------|
| 4.1 | Create runtime tests pipeline template | `build/stage-build-runtime-tests.yml` |
| 4.2 | Add stage to main pipeline | `azure-pipelines.yml` |

**Desktop job** (`stage-build-runtime-tests.yml`):
```yaml
- job: RuntimeTests_Desktop
  pool:
    vmImage: 'ubuntu-latest'
  steps:
  - template: templates/dotnet-install-linux.yml
    parameters:
      UnoCheckParameters: '--tfm net10.0-desktop'
  - script: dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -c Release -f net10.0-desktop
    displayName: Build SimpleSampleApp (Desktop)
  - script: |
      xvfb-run --auto-servernum --server-args='-screen 0 1280x1024x24' \
        dotnet src/samples/SimpleSampleApp/bin/Release/net10.0-desktop/SimpleSampleApp.dll
    displayName: Run Runtime Tests
    env:
      UNO_RUNTIME_TESTS_RUN_TESTS: '{}'
      UNO_RUNTIME_TESTS_OUTPUT_PATH: '$(Common.TestResultsDirectory)/runtime-tests.xml'
  - task: PublishTestResults@2
    condition: always()
    inputs:
      testRunTitle: 'Simple Theme Runtime Tests (Desktop)'
      testResultsFormat: 'NUnit'
      testResultsFiles: '$(Common.TestResultsDirectory)/runtime-tests.xml'
      failTaskOnFailedTests: true
```

**WASM job** (`stage-build-runtime-tests.yml`):
```yaml
- job: RuntimeTests_WASM
  pool:
    vmImage: 'ubuntu-latest'
  steps:
  - template: templates/dotnet-install-linux.yml
    parameters:
      UnoCheckParameters: '--tfm net10.0-browserwasm'
  - script: |
      dotnet publish src/samples/SimpleSampleApp/SimpleSampleApp.csproj \
        -c Release -f net10.0-browserwasm -p:PublishTrimmed=false
    displayName: Publish SimpleSampleApp (WASM)
  - script: dotnet tool install -g Uno.UI.RuntimeTests.Engine.Wasm.Runner
    displayName: Install WASM Test Runner
  - script: npx playwright install chromium
    displayName: Install Chromium
  - script: |
      uno-runtimetests-wasm \
        --app-path src/samples/SimpleSampleApp/bin/Release/net10.0-browserwasm/publish/wwwroot \
        --output $(Common.TestResultsDirectory)/runtime-tests-wasm.xml \
        --timeout 600
    displayName: Run Runtime Tests (WASM)
  - task: PublishTestResults@2
    condition: always()
    inputs:
      testRunTitle: 'Simple Theme Runtime Tests (WASM)'
      testResultsFormat: 'NUnit'
      testResultsFiles: '$(Common.TestResultsDirectory)/runtime-tests-wasm.xml'
      failTaskOnFailedTests: true
```

**Main pipeline** — add after `Build_Samples` in `azure-pipelines.yml`:
```yaml
- stage: Runtime_Tests
  displayName: Runtime Tests
  dependsOn: Determine_Changes
  condition: and(succeeded(), ne(dependencies.Determine_Changes.outputs['evaluate_changes.DetermineChanges.docsOnly'], 'true'))
  jobs:
  - template: build/stage-build-runtime-tests.yml
```

---

## File Inventory

### Files to create

| Path | Purpose |
|------|---------|
| `src/samples/SimpleSampleApp/Content/RuntimeTestsPage.xaml` | Test runner UI page |
| `src/samples/SimpleSampleApp/Content/RuntimeTestsPage.xaml.cs` | Page code-behind with `SamplePageAttribute` |
| `src/samples/SimpleSampleApp/RuntimeTests/Helpers/StyleTestHelper.cs` | Shared load/find/assert utilities |
| `src/samples/SimpleSampleApp/RuntimeTests/Helpers/ThemeDictionaryTestHelper.cs` | Theme dictionary parity utilities |
| `src/samples/SimpleSampleApp/RuntimeTests/Given_SimpleStyles.cs` | Bulk style resolution tests (21 controls) |
| `src/samples/SimpleSampleApp/RuntimeTests/Given_ExpanderStyle.cs` | Deep Expander tests (reference implementation) |
| `src/samples/SimpleSampleApp/RuntimeTests/Given_ButtonStyle.cs` | Button variant tests |
| `src/samples/SimpleSampleApp/RuntimeTests/Given_ThemeDictionaryParity.cs` | Light/Dark key parity tests |
| `src/samples/SimpleSampleApp/RuntimeTests/Given_ThemeResources.cs` | Resource chain resolution tests |
| `build/stage-build-runtime-tests.yml` | CI pipeline template |

### Files to modify

| Path | Change |
|------|--------|
| `src/samples/Directory.Packages.props` | Add `Uno.UI.RuntimeTests.Engine` package version |
| `src/samples/SimpleSampleApp/SimpleSampleApp.csproj` | Add `PackageReference` |
| `azure-pipelines.yml` | Add `Runtime_Tests` stage |

### Reference files (read-only)

| Path | Used for |
|------|----------|
| `src/library/Uno.Simple.WinUI/Styles/Controls/*.xaml` | All 21 control style files to test |
| `src/library/Uno.Simple.WinUI/SimpleTheme.cs` | Theme loading pattern |
| `src/library/Uno.Simple.WinUI/SimpleConstants.cs` | Resource URIs (`ms-appx:///Uno.Simple.WinUI/...`) |
| `src/samples/SamplesApp.Shared/Helpers/VisualTreeHelperEx.cs` | `FindFirstDescendant`, `EnumerateDescendants` |
| `src/samples/SamplesApp.Shared/Entities/SamplePageAttribute.cs` | Navigation auto-discovery attribute |
| `src/samples/SamplesApp.Shared/NavigationHelper.cs` | Shell menu population via reflection |

---

## Verification Plan

| # | Check | Method |
|---|-------|--------|
| 1 | Build succeeds | `dotnet build src/samples/SimpleSampleApp/SimpleSampleApp.csproj -f net10.0-desktop` |
| 2 | Tests run locally (headless) | `UNO_RUNTIME_TESTS_RUN_TESTS='{}' UNO_RUNTIME_TESTS_OUTPUT_PATH=results.xml dotnet run --project src/samples/SimpleSampleApp/SimpleSampleApp.csproj -f net10.0-desktop` |
| 3 | NUnit XML generated | Check `results.xml` exists and contains `<test-results>` |
| 4 | Tests run interactively | Launch app → navigate to "Runtime Tests" → run all → all green |
| 5 | CI Desktop job passes | Azure DevOps pipeline `Runtime_Tests` / `RuntimeTests_Desktop` |
| 6 | CI WASM job passes | Azure DevOps pipeline `Runtime_Tests` / `RuntimeTests_WASM` |
| 7 | No build impact on other sample apps | MaterialSampleApp and CupertinoSampleApp still build without changes |

---

## Decisions & Constraints

| Decision | Rationale |
|----------|-----------|
| Simple theme first | Clean SDK setup (`Uno.Sdk` not `Uno.Sdk.Private`), no v1/v2 split, 21 controls |
| Embedded in sample app | Reuses existing theme registration; no separate resource setup needed |
| Both Desktop + WASM CI | Desktop is fast (headless xvfb); WASM validates browser runtime behavior |
| Property assertions only (no screenshots) | Screenshot tests require golden-image maintenance; property tests are deterministic and low-maintenance |
| `SamplePageAttribute` for navigation | Leverages existing reflection-based menu system in `NavigationHelper.AddNavigationItems()` — zero Shell.xaml changes needed |
| Tests in `SimpleSampleApp/RuntimeTests/` folder | Keeps tests co-located with the app they validate; separate from shared sample code |

---

## Future Expansion

### Phase 2: Material Theme
- Add `Uno.UI.RuntimeTests.Engine` to `MaterialSampleApp.csproj`
- Create `RuntimeTestsPage` in MaterialSampleApp
- Port bulk style resolution tests for 31 v2 controls
- Add Material-specific tests (elevation, ripple, state layers with opacity)
- Add to CI pipeline matrix

### Phase 3: Cupertino Theme
- Same pattern for 17 Cupertino controls

### Phase 4: Design Token Value Assertions
- Parse SDS spec values and assert exact colors/sizes/spacing
- Example: verify `SimpleExpanderMinHeight` resolves to exactly `48` from `SimpleSpace1200`
- Could cross-reference with `specs/design-tokens/` documentation

### Phase 5: Screenshot Comparison Tests
- Use `RenderTargetBitmap` to capture control renders
- Compare against golden images stored in repo
- Higher maintenance burden — defer until test infrastructure is proven stable

---

## Test Authoring Guide

When adding tests for a new Simple control, follow this pattern:

1. **Add to bulk test** — Add a new `[DataRow(typeof(NewControl))]` in `Given_SimpleStyles.cs`
2. **Add theme parity test** — Add a new `[DataRow("ms-appx:///Uno.Simple.WinUI/Styles/Controls/NewControl.xaml")]` in `Given_ThemeDictionaryParity.cs`
3. **Create deep test class** (if the control has complex template/states):
   - File: `Given_NewControlStyle.cs`
   - Test template parts (`x:Name` elements)
   - Test design token values (MinHeight, CornerRadius, Padding)
   - Test visual state transitions (swap states, assert property changes)
   - Use `StyleTestHelper` for loading and tree traversal
