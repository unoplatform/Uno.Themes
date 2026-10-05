#nullable enable

using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Material;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Guards the lightweight-styling keys of the Material ComboBox placeholders (unoplatform/Uno.Themes#1739):
/// the floating label's transform and margin, and the text style shared by both placeholder TextBlocks.
/// The keys only exist in the Material template, so the test lives in the Material sample app.
/// </summary>
[TestClass]
public class Given_ComboBox
{
	private const string StyleKey = "MaterialComboBoxStyle";

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_Default_Then_PlaceholdersKeepMaterialValues(ElementTheme appearance)
	{
		var (container, comboBox) = CreateThemedComboBox(appearance);

		await LoadAsync(container, comboBox);

		var (placeholder, upperPlaceholder) = GetPlaceholders(comboBox);
		var transform = GetTransform(upperPlaceholder);
		Assert.AreEqual(0.7, transform.ScaleX, 0.001, $"ScaleX under {appearance}");
		Assert.AreEqual(0.7, transform.ScaleY, 0.001, $"ScaleY under {appearance}");
		Assert.AreEqual(-11.0, transform.TranslateY, 0.001, $"TranslateY under {appearance}");
		Assert.AreEqual(new Thickness(0), upperPlaceholder.Margin, $"Margin under {appearance}");

		var bodyLarge = container.Resources["MaterialBodyLarge"];
		Assert.AreSame(bodyLarge, placeholder.Style, $"PlaceholderElement style under {appearance}");
		Assert.AreSame(bodyLarge, upperPlaceholder.Style, $"UpperPlaceholderElement style under {appearance}");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light, false)]
	[DataRow(ElementTheme.Dark, false)]
	[DataRow(ElementTheme.Light, true)]
	[DataRow(ElementTheme.Dark, true)]
	public async Task When_LightweightResourcesOverridden_Then_PlaceholdersUseLocalValues(ElementTheme appearance, bool onComboBox)
	{
		var (container, comboBox) = CreateThemedComboBox(appearance);
		var textStyle = SetOverrides(onComboBox ? comboBox.Resources : container.Resources);

		await LoadAsync(container, comboBox);

		AssertOverridesApplied(comboBox, textStyle, $"under {appearance}");
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeChangesAfterLoad_Then_LocalMarginAndStyleAreKept()
	{
		// Not asserted: the transform keys. On Uno a theme change re-resolves the {ThemeResource}
		// on the CompositeTransform (not a FrameworkElement) from Application.Resources only, so a
		// local transform override reverts to the default; see the app-level test below.
		var (container, comboBox) = CreateThemedComboBox(ElementTheme.Light);
		var textStyle = SetOverrides(comboBox.Resources);

		await LoadAsync(container, comboBox);

		foreach (var appearance in new[] { ElementTheme.Dark, ElementTheme.Light })
		{
			container.RequestedTheme = appearance;
			await UnitTestsUIContentHelper.WaitForIdle();

			var (placeholder, upperPlaceholder) = GetPlaceholders(comboBox);
			Assert.AreEqual(new Thickness(0, 0, 0, 6), upperPlaceholder.Margin, $"Margin after switching to {appearance}");
			Assert.AreSame(textStyle, placeholder.Style, $"PlaceholderElement style after switching to {appearance}");
			Assert.AreSame(textStyle, upperPlaceholder.Style, $"UpperPlaceholderElement style after switching to {appearance}");
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	public async Task When_ThemeChangesAfterLoad_Then_AppLevelTransformOverridesAreKept()
	{
		// Mirrors an override declared in App.xaml: the theme comes from the sample app's
		// Application.Resources rather than a local MaterialTheme, so the app-level keys win.
		var app = Application.Current.Resources;
		app["ComboBoxUpperPlaceHolderScaleX"] = 0.5;
		app["ComboBoxUpperPlaceHolderScaleY"] = 0.6;
		app["ComboBoxUpperPlaceHolderTranslateY"] = -14.0;
		try
		{
			var container = new Grid { RequestedTheme = ElementTheme.Light };
			var comboBox = new ComboBox
			{
				Style = (Style)app[StyleKey],
				PlaceholderText = "Label",
				ItemsSource = new[] { "A", "B" },
				SelectedIndex = 0,
			};
			container.Children.Add(comboBox);

			await LoadAsync(container, comboBox);

			foreach (var appearance in new[] { ElementTheme.Light, ElementTheme.Dark, ElementTheme.Light })
			{
				container.RequestedTheme = appearance;
				await UnitTestsUIContentHelper.WaitForIdle();

				var transform = GetTransform(GetPlaceholders(comboBox).UpperPlaceholder);
				Assert.AreEqual(0.5, transform.ScaleX, 0.001, $"ScaleX under {appearance}");
				Assert.AreEqual(0.6, transform.ScaleY, 0.001, $"ScaleY under {appearance}");
				Assert.AreEqual(-14.0, transform.TranslateY, 0.001, $"TranslateY under {appearance}");
			}
		}
		finally
		{
			app.Remove("ComboBoxUpperPlaceHolderScaleX");
			app.Remove("ComboBoxUpperPlaceHolderScaleY");
			app.Remove("ComboBoxUpperPlaceHolderTranslateY");
		}
	}

	private static Style SetOverrides(ResourceDictionary resources)
	{
		var textStyle = new Style(typeof(TextBlock)) { Setters = { new Setter(TextBlock.FontSizeProperty, 22d) } };
		resources["ComboBoxUpperPlaceHolderScaleX"] = 0.5;
		resources["ComboBoxUpperPlaceHolderScaleY"] = 0.6;
		resources["ComboBoxUpperPlaceHolderTranslateY"] = -14.0;
		resources["ComboBoxUpperPlaceHolderMargin"] = new Thickness(0, 0, 0, 6);
		resources["ComboBoxPlaceholderTextStyle"] = textStyle;
		return textStyle;
	}

	private static void AssertOverridesApplied(ComboBox comboBox, Style textStyle, string context)
	{
		var (placeholder, upperPlaceholder) = GetPlaceholders(comboBox);
		var transform = GetTransform(upperPlaceholder);
		Assert.AreEqual(0.5, transform.ScaleX, 0.001, $"ScaleX {context}");
		Assert.AreEqual(0.6, transform.ScaleY, 0.001, $"ScaleY {context}");
		Assert.AreEqual(-14.0, transform.TranslateY, 0.001, $"TranslateY {context}");
		Assert.AreEqual(new Thickness(0, 0, 0, 6), upperPlaceholder.Margin, $"Margin {context}");
		Assert.AreSame(textStyle, placeholder.Style, $"PlaceholderElement style {context}");
		Assert.AreSame(textStyle, upperPlaceholder.Style, $"UpperPlaceholderElement style {context}");
	}

	private static (Grid Container, ComboBox ComboBox) CreateThemedComboBox(ElementTheme appearance)
	{
		var container = new Grid { RequestedTheme = appearance };
		container.Resources.MergedDictionaries.Add(new MaterialTheme());

		var style = container.Resources[StyleKey] as Style;
		Assert.IsNotNull(style, $"{StyleKey} should be resolvable from the theme.");

		// A selected item shows the floating label (UpperPlaceholderElement).
		var comboBox = new ComboBox
		{
			Style = style,
			PlaceholderText = "Label",
			ItemsSource = new[] { "A", "B" },
			SelectedIndex = 0,
		};
		container.Children.Add(comboBox);
		return (container, comboBox);
	}

	private static async Task LoadAsync(Grid container, ComboBox comboBox)
	{
		UnitTestsUIContentHelper.Content = container;
		await UnitTestsUIContentHelper.WaitForLoaded(comboBox);
		await UnitTestsUIContentHelper.WaitForIdle();
	}

	private static (TextBlock Placeholder, TextBlock UpperPlaceholder) GetPlaceholders(ComboBox comboBox)
	{
		TextBlock? placeholder = comboBox.FindFirstDescendant<TextBlock>(x => x.Name == "PlaceholderElement");
		TextBlock? upperPlaceholder = comboBox.FindFirstDescendant<TextBlock>(x => x.Name == "UpperPlaceholderElement");
		Assert.IsNotNull(placeholder, "PlaceholderElement should be part of the ComboBox template.");
		Assert.IsNotNull(upperPlaceholder, "UpperPlaceholderElement should be part of the ComboBox template.");
		return (placeholder, upperPlaceholder);
	}

	private static CompositeTransform GetTransform(TextBlock upperPlaceholder)
	{
		var transform = upperPlaceholder.RenderTransform as CompositeTransform;
		Assert.IsNotNull(transform, "UpperPlaceholderElement should use a CompositeTransform.");
		return transform;
	}
}
