# Cupertino sample app: full Cupertino chrome

Request (2026-09-24): "Update the cupertino sample app to be full cupertino, fix the navview icons, find any
other inconsistencies and fix. Especially the Overview page, it looks weird."

Evidence: every Cupertino sample page rendered through the real `Shell` (scratch capture test, 1280×1000,
Light and Dark) before and after.

## Findings (before)

- Sidebar: every control uses the same placeholder diamond; the category icons are filled while the rest
  are outlines.
- Sidebar labels, pane title and all unstyled sample text use the framework fallback font (Open Sans), not
  the theme's Inter: `CupertinoNavigationViewItemStyle` sets no font, and the sample's implicit TextBlock
  style sets none either.
- Chrome is Fluent: `ApplicationPageBackgroundThemeBrush` gray page, a 34 px light-weight title indented
  48 px past the content, a "SOURCE WinUI/Uno.UI" block, gray `SystemBaseMedium` boxes with a 1 px border
  around every sample, 10 px bordered "See Xaml" buttons, and the purple Material dark-mode toggle.
- Overview: Fluent cards (4 px radius, gray split column), `TitleTextBlockStyle`, uppercase "DEFAULT" /
  "CONTAINED" buttons, an outlined multiline TextBox with a paragraph of lorem ipsum.
- Pages: TimePicker has an empty Cupertino template (the theme has no TimePicker style); the bound ListView
  sample is empty in every design (`Data.Letters` with no `DataType`); CheckBox / RadioButton /
  ToggleSwitch / ComboBox use uppercase Material labels; the Colors page's label brushes are unreadable.

## Plan

- [x] Theme: sidebar rows use the body type slot (family, size, weight), red/fix/green runtime test.
- [x] Shared: a real outline icon per sample page and outline category icons (all heads benefit).
- [x] Shared: ListView bound sample gets its data; TimePicker drops Cupertino until the theme styles it.
- [x] Cupertino head: one chrome dictionary merged last (page layout, overview card, sample box, See XAML,
      appearance switch, implicit TextBlock font), built from semantic brushes and type slots.
- [x] Overview (Cupertino): inset-grouped sections, sentence-case iOS labels.
- [x] Pages: sentence-case labels, Colors page legible in both appearances.
- [x] Verify: captures Light/Dark, Cupertino runtime tests, formatters.

- [x] Theme (found while verifying): the gray fills vanished on a dark inset group, red/fix/green.

## Review

- **Sidebar.** Every page has an outline icon from `@mdi/svg` 7.4.47, translated to the origin like the existing
  ones and chosen close to square, because the item's Viewbox scales each icon to a fixed height (a 16×4 icon
  would have drawn 64 px wide). Category icons are outlines too. The shared attribute change reaches all heads.
- **Theme typography.** `CupertinoNavigationViewItemStyle` now sets the `BodyLarge*` slot and the pane title the
  `TitleMedium*` slot (`When_SidebarRealized_Then_TextUsesTheTypeSlots`, red on the fallback family first).
- **Theme fills.** The Gray button, text toggle, filled TextBox / PasswordBox and top-navigation hover / pressed
  read `SecondaryContainerBrush`, an opaque gray pre-composited over black: #1C1C1F on a #1C1C1E dark group,
  a luma lift of 0.1. They now read `CupertinoTertiarySystemFillBrush` (top navigation: the quaternary / system
  fills the sidebar already uses), Apple's translucent fill; Light renders unchanged over white.
  `When_GrayFillSitsOnADarkGroup_Then_ItStaysVisible` was red at 0.1 and passes; four pinned expectations in
  `Given_CupertinoControls` moved to the new brush.
- **Chrome.** `CupertinoSampleApp/Styles/CupertinoSampleChrome.xaml`, merged last, re-templates the page layout
  (grouped background, bold large title aligned with the content, description in the secondary label, no
  "SOURCE" block), each sample (inset group surface, footnote "See XAML" plain button, Cupertino popover),
  Overview entries (section header, group, "View component" row with a trailing chevron, footer), the
  appearance toggle (an iOS switch with a sun / moon knob) and unstyled text (theme typeface). The layout keeps
  the code-behind's template parts, collapsed.
- **Pages.** Overview: sentence-case Prominent / Tinted / Gray / Plain, labelled fields, settings rows.
  CheckBox / RadioButton / ToggleSwitch / ComboBox labels are sentence case; ToggleSwitch and ComboBox are iOS
  rows. Colors lists every brush as a chip plus key in grouped lists, legible in both appearances. The
  NavigationView sample opens its sidebar with real items. ListView's bound sample has data in every design.
- **Deferred.** TimePicker left the Cupertino app: the theme has no TimePicker style, and the page's Cupertino
  template was empty. A compact-capsule TimePicker like the DatePicker is the follow-up.
- **Verification.** Every Cupertino page captured through the real Shell, Light and Dark, before and after.
  Cupertino runtime tests 293/293, Simple 273 (1 existing skip), Material 63; XAML Styler and `dotnet format whitespace` verify clean; markdownlint and
  cSpell pass on the changed doc.
