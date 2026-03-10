# Simple Theme → SDS Component Mapping

This document maps each XAML style in `src/library/Uno.Simple.WinUI/Styles/Controls/`
to its originating React/CSS component in `ref/sds/src/ui/primitives/`.

| XAML File | SDS Component | SDS Source Path | Notes |
|---|---|---|---|
| `Button.xaml` | **Button** | `Button/` | Variants: Primary, Neutral, Subtle, DangerPrimary, DangerSubtle |
| `ToggleButton.xaml` | **Button** | `Button/` | Neutral (unchecked) → Primary (checked) toggle behavior |
| `CheckBox.xaml` | **Checkbox** | `Checkbox/checkbox.css` | — |
| `RadioButton.xaml` | **Radio** | `Radio/radio.css` | — |
| `TextBox.xaml` | **Input** | `Input/input.css` | Outlined box, 8px radius, 1px border |
| `PasswordBox.xaml` | **Input** | `Input/input.css` | Same visual treatment as TextBox |
| `ComboBox.xaml` | **Select** | `Select/select.css` | — |
| `AutoSuggestBox.xaml` | **Search** | `Search/Search.tsx`, `search.css`, `Input/input.css` | Pill-shaped input (radius-full) with search/clear icon |
| `ToggleSwitch.xaml` | **Switch** | `Switch/switch.css` | — |
| `Slider.xaml` | **Slider** | `Slider/` | — |
| `Expander.xaml` | **Accordion** | `Accordion/` | Maps to AccordionItem behavior |
| `ListView.xaml` | **ListBox** | `ListBox/` | Maps to ListView + ListViewItem |
| `CalendarView.xaml` | **Calendar** | `Calendar/` | — |
| `CalendarDatePicker.xaml` | **DatePicker** | `DatePicker/` | Field + calendar flyout |
| `DatePicker.xaml` | **DatePicker** | `DatePicker/` | Field appearance inspired by CalendarDatePicker |
| `ContentDialog.xaml` | **Dialog** | `Dialog/` | Maps to Dialog type=card |
| `MenuFlyout.xaml` | **Menu** | `Menu/` | Maps to MenuFlyoutPresenter + MenuFlyoutItem |
| `ToolTip.xaml` | **Tooltip** | `Tooltip/` | — |
| `TextBlock.xaml` | **Text** | `Text/` | Maps SDS typography scale tokens |
| `PersonPicture.xaml` | **Avatar** | `Avatar/` | Sizes: small(24), medium(32), large(40) |
| `AppBarButton.xaml` | **Navigation** | `Navigation/` | Mapped from SDS NavigationButton component |

### Files without a direct SDS component mapping

| XAML File | Purpose |
|---|---|
| `_Resources.xaml` | Implicit style assignments (wires default styles to named styles) |

### SDS primitives with no current XAML counterpart

| SDS Component | Path |
|---|---|
| Fieldset | `Fieldset/` |
| Icon | `Icon/` |
| IconButton | `IconButton/` |
| Image | `Image/` |
| Link | `Link/` |
| Logo | `Logo/` |
| Notification | `Notification/` |
| Pagination | `Pagination/` |
| Tab | `Tab/` |
| Table | `Table/` |
| Tag | `Tag/` |
| Textarea | `Textarea/` |
