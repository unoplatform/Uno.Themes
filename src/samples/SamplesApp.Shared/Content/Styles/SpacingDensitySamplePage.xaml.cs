using System.Globalization;
using System.Text;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Uno.Themes.Samples.Content.Styles;

[SamplePage(
	SampleCategory.Styles,
	"Spacing & Density",
	Description = "Adjust the DefaultSpacing base unit and the DefaultDensity mode on the running theme and watch the whole app restyle live.",
	SortOrder = 21,
	SupportedDesigns = new[] { Design.Material, Design.Simple })]
public sealed partial class SpacingDensitySamplePage : Page
{
	// Tokens surfaced in the numeric readout, smallest to largest; matches the bars in the XAML.
	private static readonly string[] ReadoutTokens = { "Space050", "Space100", "Space150", "Space200", "Space300", "Space400", "Space600", "Space800" };

	private bool _initialized;

	public SpacingDensitySamplePage()
	{
		this.InitializeComponent();

		// The running theme is the single source of truth — the knobs reflect whatever it
		// currently carries, so the page state survives navigation without extra bookkeeping.
		var theme = SemanticThemeHelper.GetTheme();
		SpacingSlider.Value = theme?.DefaultSpacing ?? 4d;
		DensityCombo.SelectedIndex = (theme?.DefaultDensity ?? Density.Regular) switch
		{
			Density.Compact => 0,
			Density.Comfy => 2,
			_ => 1,
		};
		_initialized = true;
		UpdateReadouts();
	}

	private Density SelectedDensity => DensityCombo.SelectedIndex switch
	{
		0 => Density.Compact,
		2 => Density.Comfy,
		_ => Density.Regular,
	};

	private void OnSpacingChanged(object sender, RangeBaseValueChangedEventArgs e)
	{
		if (_initialized)
		{
			ApplyToTheme();
		}
	}

	private void OnDensityChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_initialized)
		{
			ApplyToTheme();
		}
	}

	private void ApplyToTheme()
	{
		if (SemanticThemeHelper.GetTheme() is not { } theme)
		{
			return;
		}

		var spacing = SpacingSlider.Value;
		var density = SelectedDensity;
		if (theme.DefaultSpacing == spacing && theme.DefaultDensity == density)
		{
			UpdateReadouts();
			return;
		}

		// Spacing tokens are values (double/Thickness) with no live instance to mutate, so —
		// unlike a seed-color change — restyling takes two extra steps beyond setting the DPs:
		//
		// 1. Reload the theme's static control-style layer. Its per-control alias keys
		//    (ButtonPadding → Space400HorizontalThickness, …) materialize lazily on first lookup
		//    and are then cached for the dictionary's lifetime; re-setting Source re-creates the
		//    layer so they re-resolve against the new scale. The DP setters below then rebuild
		//    the dynamic token layers via the theme's property-changed callbacks, keeping the
		//    theme's Colors/font configuration intact (nothing to copy).
		theme.Source = new Uri(theme.Source.OriginalString);
		theme.DefaultSpacing = spacing;
		theme.DefaultDensity = density;

		// 2. Force every live ThemeResource binding to re-resolve — the same machinery as a
		//    dark/light switch — so already-rendered controls pick the new values up.
		RefreshLiveTree();

		UpdateReadouts();
	}

	private void RefreshLiveTree()
	{
		if (XamlRoot?.Content is not FrameworkElement root)
		{
			return;
		}

		var original = root.RequestedTheme;
		root.RequestedTheme = root.ActualTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
		root.RequestedTheme = original;
	}

	private void UpdateReadouts()
	{
		var resources = Application.Current.Resources;
		var spacing = SpacingSlider.Value;
		var density = SelectedDensity;

		EffectiveBaseText.Text = resources.TryGetValue("Space100", out var space100) && space100 is double basePx
			? $"Effective base unit (Space100): {basePx:0.##} px = {spacing:0.##} × {DensityFactor(density):0.00} ({density})"
			: string.Empty;

		var values = new StringBuilder();
		foreach (var token in ReadoutTokens)
		{
			if (resources.TryGetValue(token, out var value) && value is double px)
			{
				if (values.Length > 0)
				{
					values.Append("   ");
				}

				values.Append(token).Append(" = ").Append(px.ToString("0.##", CultureInfo.CurrentCulture));
			}
		}

		TokenValuesText.Text = values.ToString();

		var themeName = SemanticThemeHelper.GetTheme()?.GetType().Name ?? nameof(BaseTheme);
		var spacingValue = spacing.ToString("0.##", CultureInfo.InvariantCulture);
		var densityAttribute = density == Density.Regular
			? string.Empty
			: $"\n{new string(' ', themeName.Length + 2)}DefaultDensity=\"{density}\"";
		XamlSnippet.Text = $"<{themeName} DefaultSpacing=\"{spacingValue}\"{densityAttribute} />";
	}

	private static double DensityFactor(Density density) => density switch
	{
		Density.Compact => 0.75,
		Density.Comfy => 1.25,
		_ => 1.0,
	};
}
