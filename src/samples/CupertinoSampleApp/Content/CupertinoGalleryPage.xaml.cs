namespace Uno.Themes.Samples.Content;

[SamplePage(
	SampleCategory.Styles,
	"Cupertino Gallery",
	Description = "Everyday iOS and iPadOS controls in context, including settings, forms, actions and navigation.",
	SortOrder = 0,
	SupportedDesigns = new[] { Design.Cupertino })]
public sealed partial class CupertinoGalleryPage : Page
{
	public CupertinoGalleryPage()
	{
		InitializeComponent();
	}

	// Pointer events on a knob will still move it out of the held state; keep the pointer off the controls.
	private void HoldKnobsChanged(object sender, RoutedEventArgs args)
	{
		var hold = HoldKnobs.IsChecked == true;
		KnobPositionPanel.Visibility = hold ? Visibility.Visible : Visibility.Collapsed;
		foreach (var control in Descendants(GalleryScroll))
		{
			if (control is ToggleSwitch or Microsoft.UI.Xaml.Controls.Primitives.Thumb)
			{
				VisualStateManager.GoToState(control, hold ? "Pressed" : "Normal", false);
			}
		}

		ApplyKnobPosition();
	}

	private void KnobPositionChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs args)
		=> ApplyKnobPosition();

	// The knob's own position comes from a held Duration-0 animation on its TranslateTransform (0 off, travel
	// on), which a plain property set cannot override, so the offset goes on the knob's Margin instead.
	private void ApplyKnobPosition()
	{
		var hold = HoldKnobs.IsChecked == true;
		foreach (var toggle in Descendants(GalleryScroll).OfType<ToggleSwitch>())
		{
			if (FindPart<Grid>(toggle, "SwitchKnob") is not { } knob || FindPart<Microsoft.UI.Xaml.Shapes.Shape>(toggle, "OuterBorder") is not { } track)
			{
				continue;
			}

			var travel = track.ActualWidth - knob.ActualWidth;
			var restOffset = toggle.IsOn ? travel : 0;
			var offset = hold ? (KnobPosition.Value / 100 * travel) - restOffset : 0;
			knob.Margin = new Thickness(offset, 0, 0, 0);
		}
	}

	private static IEnumerable<Control> Descendants(DependencyObject root)
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is Control control)
			{
				yield return control;
			}

			foreach (var nested in Descendants(child))
			{
				yield return nested;
			}
		}
	}

	private static T? FindPart<T>(DependencyObject root, string name) where T : FrameworkElement
	{
		for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
		{
			var child = VisualTreeHelper.GetChild(root, i);
			if (child is T match && match.Name == name)
			{
				return match;
			}

			if (FindPart<T>(child, name) is { } nested)
			{
				return nested;
			}
		}

		return null;
	}

	private async void ShowAlert(object sender, RoutedEventArgs args)
	{
		try
		{
			var dialog = new ContentDialog
			{
				Title = "Save your changes?",
				Content = "Your preferences will be available the next time you open the app.",
				PrimaryButtonText = "Save",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				XamlRoot = XamlRoot,
			};
			await dialog.ShowAsync();
		}
		catch (InvalidOperationException exception)
		{
			Console.Error.WriteLine($"Unable to open the gallery alert: {exception}");
		}
		catch (Exception exception)
		{
			Console.Error.WriteLine($"Gallery alert failed: {exception}");
		}
	}
}
