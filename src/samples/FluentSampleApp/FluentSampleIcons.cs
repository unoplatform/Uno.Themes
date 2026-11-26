namespace Uno.Themes.Samples;

/// <summary>
/// Segoe Fluent Icons glyphs for the Fluent head's navigation, as WinUI apps draw their NavigationView icons.
/// Code points and names from https://learn.microsoft.com/windows/apps/design/style/segoe-fluent-icons-font.
/// </summary>
internal static class FluentSampleIcons
{
	private static readonly Dictionary<string, string> Glyphs = new()
	{
		["Overview"] = "\uE80F", // Home
		["Runtime Tests"] = "\uE9D9", // Diagnostic
		["Styles"] = "\uE771", // Personalize
		["Controls"] = "\uE71D", // AllApps
		["Semantic Styling"] = "\uEB3C", // Design
		["Design Tokens"] = "\uED5E", // Ruler
		["Colors"] = "\uE790", // Color
		["Seed Color"] = "\uEF3C", // Eyedropper
		["AppBarButton"] = "\uE712", // More
		["AutoSuggestBox"] = "\uE721", // Search
		["Button"] = "\uE7C9", // TouchPointer
		["CalendarDatePicker"] = "\uE8BF", // CalendarDay
		["CalendarView"] = "\uE787", // Calendar
		["CheckBox"] = "\uE73A", // CheckboxComposite
		["ComboBox"] = "\uEDE3", // ButtonMenu
		["CommandBar"] = "\uE90E", // DockBottom
		["ContentDialog"] = "\uE8BD", // Message
		["DatePicker"] = "\uE8C0", // CalendarWeek
		["FAB"] = "\uE710", // Add
		["Flyout"] = "\uE82F", // ToolTip
		["HyperlinkButton"] = "\uE71B", // Link
		["IconButton"] = "\uE734", // FavoriteStar
		["Info Bar"] = "\uE946", // Info
		["ListView"] = "\uE8FD", // BulletedList
		["MediaPlayerElement"] = "\uE714", // Video
		["MenuFlyout"] = "\uE700", // GlobalNavButton
		["Navigation View (MUX)"] = "\uE90C", // DockLeft
		["Top Navigation View (MUX)"] = "\uE7C4", // TaskView
		["PasswordBox"] = "\uE9A8", // PasswordKeyShow
		["PersonPicture"] = "\uE77B", // Contact
		["PipsPager"] = "\uF127", // PaginationDotSolid10
		["Progress Bar"] = "\uF16A", // ProgressRingDots
		["Progress Ring"] = "\uF138", // StatusCircleRing
		["RadioButton"] = "\uECCB", // RadioBtnOn
		["RatingControl"] = "\uE735", // FavoriteStarFill
		["Slider"] = "\uE9E9", // Equalizer
		["TextBlock"] = "\uE8D2", // Font
		["TextBox"] = "\uE8AC", // Rename
		["TimePicker"] = "\uE917", // Clock
		["ToggleButton"] = "\uEC12", // ToggleBorder
		["ToggleSwitch"] = "\uEC11", // ToggleFilled
	};

	public static IconElement? Create(string entry)
		=> Glyphs.TryGetValue(entry, out var glyph) ? new FontIcon { Glyph = glyph } : null;
}
