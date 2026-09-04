namespace Uno.Themes.Samples.Content.Styles;

/// <summary>
/// Sets <see cref="BaseTheme.DefaultFontFamily"/> on the running application's theme, so a font
/// swap can be validated against the type scales on the page around it.
/// </summary>
public sealed partial class FontFamilyTunerControl : UserControl
{
	// Every entry is a font the sample heads actually carry (Inter, Roboto and Open Sans ship with
	// the sample packages), referenced through its single entry point so the per-scale *FontWeight
	// tokens resolve real weights. A null source leaves the property unset, which hands the type
	// scale back to the design system's own font.
	private static readonly (string Label, string? Source)[] Choices =
	{
		("(design system default)", null),
		("Inter", "ms-appx:///Uno.Fonts.Inter/Fonts/Inter.ttf#Inter"),
		("Roboto", "ms-appx:///Uno.Fonts.Roboto/Fonts/Roboto.ttf#Roboto"),
		("Open Sans", "ms-appx:///Uno.Fonts.OpenSans/Fonts/OpenSans.ttf"),
	};

	private bool _isInitializing;

	public FontFamilyTunerControl()
	{
		this.InitializeComponent();

		// Items are the labels themselves: a plain string list needs no item template and no
		// bindable metadata, and the selected index indexes Choices.
		FamilyCombo.ItemsSource = Choices.Select(choice => choice.Label).ToArray();

		Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		// The combo follows the theme rather than the reverse, so leaving the page and coming back
		// shows what the application is actually carrying.
		try
		{
			_isInitializing = true;

			var theme = SemanticThemeHelper.GetTheme();

			FamilyCombo.SelectedIndex = IndexOfSource(theme?.DefaultFontFamily?.Source);

			ReportState(theme);
		}
		finally
		{
			_isInitializing = false;
		}
	}

	/// <summary>
	/// The entry for <paramref name="source"/>, or the unset entry — which is also the answer for a
	/// source assigned outside this control (a declaration in App.xaml, say) that has no entry.
	/// </summary>
	/// <param name="source">The font source currently on the theme, or <c>null</c>.</param>
	/// <returns>An index into <c>Choices</c>.</returns>
	private static int IndexOfSource(string? source)
	{
		for (var i = 0; i < Choices.Length; i++)
		{
			if (string.Equals(Choices[i].Source, source, StringComparison.Ordinal))
			{
				return i;
			}
		}

		return 0;
	}

	private void OnFamilySelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_isInitializing)
		{
			return;
		}

		Apply();
	}

	private void Apply()
	{
		var theme = SemanticThemeHelper.GetTheme();
		if (theme is null)
		{
			ReportState(null);
			return;
		}

		theme.DefaultFontFamily = ToFontFamily(FamilyCombo.SelectedIndex);

		// Setting the family regenerates the resources, which covers everything laid out afterwards.
		// Text already on screen re-resolves its {ThemeResource *FontFamily} bindings only on a
		// theme-change pass, so drive one: flip the root's RequestedTheme away from its actual
		// theme and back, both writes in this frame.
		RefreshRenderedText();

		ReportState(theme);
	}

	private static FontFamily? ToFontFamily(int selectedIndex)
		=> selectedIndex >= 0 && Choices[selectedIndex].Source is { Length: > 0 } source
			? new FontFamily(source)
			: null;

	// Sample-only trick: two synchronous RequestedTheme writes drive a full ThemeResource
	// re-resolution pass over the tree (twice), and subscribers observe a transient wrong
	// theme for a frame. Fine for a demo page; do not copy this into library or app code.
	private void RefreshRenderedText()
	{
		if (XamlRoot?.Content is not FrameworkElement root)
		{
			return;
		}

		var requested = root.RequestedTheme;
		root.RequestedTheme = root.ActualTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
		root.RequestedTheme = requested;
	}

	private void ReportState(BaseTheme? theme)
	{
		if (theme is null)
		{
			StatusText.Text = "No BaseTheme in Application.Resources — nothing to set.";
			return;
		}

		StatusText.Text =
			$"Theme: {theme.GetType().Name}\n" +
			$"DefaultFontFamily → {(theme.DefaultFontFamily?.Source ?? "(unset)")}\n" +
			$"DefaultFontFamily resolves {Resolve("DefaultFontFamily")}";
	}

	/// <summary>
	/// What the application resolves the root token to, which is the generated value when the property
	/// is set and the design system's own declaration when it is not.
	/// </summary>
	/// <param name="tokenKey">The root token to resolve.</param>
	/// <returns>The resolved font source, or a marker when the key resolves to nothing.</returns>
	private static string Resolve(string tokenKey)
		=> Application.Current.Resources.TryGetValue(tokenKey, out var value) && value is FontFamily family
			? family.Source
			: "unresolved";
}
