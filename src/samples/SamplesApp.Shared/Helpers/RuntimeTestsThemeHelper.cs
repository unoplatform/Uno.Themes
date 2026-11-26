namespace Uno.Themes.Samples.Helpers;

/// <summary>
/// Lets a headless runtime-test run pick the application theme (<c>UNO_RUNTIME_TESTS_THEME</c>).
/// </summary>
/// <remarks>
/// Built-in templates and the platform tokens resolve against the application theme only (an element's
/// <c>RequestedTheme</c> does not re-branch them, see specs/lessons.md), so the Dark branches of those
/// assertions are covered by a second CI run under a Dark application theme.
/// </remarks>
public static class RuntimeTestsThemeHelper
{
	/// <summary>
	/// Sets <paramref name="app"/>'s <see cref="Application.RequestedTheme"/> from <c>UNO_RUNTIME_TESTS_THEME</c>
	/// during a runtime-test run. Call from the <see cref="Application"/> constructor, before XAML resources initialize.
	/// </summary>
	public static void ApplyRequestedTheme(Application app)
	{
		if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("UNO_RUNTIME_TESTS_OUTPUT_PATH"))
			&& Enum.TryParse<ApplicationTheme>(Environment.GetEnvironmentVariable("UNO_RUNTIME_TESTS_THEME"), out var testTheme)
			&& Enum.IsDefined(testTheme))
		{
			app.RequestedTheme = testTheme;
		}
	}
}
