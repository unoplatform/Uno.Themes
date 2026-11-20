using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Uno.Extensions.Markup;
using Uno.Extensions.Markup.Internals;
using Uno.Simple;
using Uno.UI.RuntimeTests;
using Windows.UI;
using Windows.UI.Text;
using Semantic = Uno.Themes.Markup.Theme;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_MarkupSemanticResources
{
	[TestMethod]
	[DataRow("AppBarButton", typeof(AppBarButton))]
	[DataRow("CommandBar", typeof(CommandBar))]
	[DataRow("ContentDialog", typeof(ContentDialog))]
	[DataRow("FlyoutPresenter", typeof(FlyoutPresenter))]
	[DataRow("ListView", typeof(ListView))]
	[DataRow("ListViewItem", typeof(ListViewItem))]
	[DataRow("MenuFlyoutItem", typeof(MenuFlyoutItem))]
	[DataRow("MenuFlyoutPresenter", typeof(MenuFlyoutPresenter))]
	[DataRow("NavigationView", typeof(NavigationView))]
	[DataRow("NavigationViewItem", typeof(NavigationViewItem))]
	[DataRow("PipsPager", typeof(PipsPager))]
	public void When_StyleMetadataIsRead_Then_TargetIsTheActualXamlControl(string helperName, Type controlType)
	{
		var property = typeof(Semantic).GetNestedType(helperName)?.GetNestedType("Styles")?.GetProperty("Default");
		Assert.IsNotNull(property);
		var definition = property.GetCustomAttribute<ResourceKeyDefinitionAttribute>();
		Assert.IsNotNull(definition);
		Assert.AreEqual(controlType, definition.TargetType,
			"Style metadata must identify the XAML control, not the nested resource helper class.");
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_TypographyHelpersAreUsed_Then_TypedConsumersResolveScopedOverrides(ElementTheme appearance)
	{
		// Typed arrays deliberately make a wrong public generic argument a compiler error.
		(string Slot, ThemeResourceKey<FontWeight> Weight, ThemeResourceKey<FontFamily> Family)[] slots =
		[
			("DisplayLarge", Semantic.Typography.DisplayLarge.FontWeight, Semantic.Typography.DisplayLarge.FontFamily),
			("DisplayMedium", Semantic.Typography.DisplayMedium.FontWeight, Semantic.Typography.DisplayMedium.FontFamily),
			("DisplaySmall", Semantic.Typography.DisplaySmall.FontWeight, Semantic.Typography.DisplaySmall.FontFamily),
			("HeadlineLarge", Semantic.Typography.HeadlineLarge.FontWeight, Semantic.Typography.HeadlineLarge.FontFamily),
			("HeadlineMedium", Semantic.Typography.HeadlineMedium.FontWeight, Semantic.Typography.HeadlineMedium.FontFamily),
			("HeadlineSmall", Semantic.Typography.HeadlineSmall.FontWeight, Semantic.Typography.HeadlineSmall.FontFamily),
			("TitleLarge", Semantic.Typography.TitleLarge.FontWeight, Semantic.Typography.TitleLarge.FontFamily),
			("TitleMedium", Semantic.Typography.TitleMedium.FontWeight, Semantic.Typography.TitleMedium.FontFamily),
			("TitleSmall", Semantic.Typography.TitleSmall.FontWeight, Semantic.Typography.TitleSmall.FontFamily),
			("LabelLarge", Semantic.Typography.LabelLarge.FontWeight, Semantic.Typography.LabelLarge.FontFamily),
			("LabelMedium", Semantic.Typography.LabelMedium.FontWeight, Semantic.Typography.LabelMedium.FontFamily),
			("LabelSmall", Semantic.Typography.LabelSmall.FontWeight, Semantic.Typography.LabelSmall.FontFamily),
			("LabelExtraSmall", Semantic.Typography.LabelExtraSmall.FontWeight, Semantic.Typography.LabelExtraSmall.FontFamily),
			("BodyLarge", Semantic.Typography.BodyLarge.FontWeight, Semantic.Typography.BodyLarge.FontFamily),
			("BodyMedium", Semantic.Typography.BodyMedium.FontWeight, Semantic.Typography.BodyMedium.FontFamily),
			("BodySmall", Semantic.Typography.BodySmall.FontWeight, Semantic.Typography.BodySmall.FontFamily),
			("CaptionLarge", Semantic.Typography.CaptionLarge.FontWeight, Semantic.Typography.CaptionLarge.FontFamily),
			("CaptionMedium", Semantic.Typography.CaptionMedium.FontWeight, Semantic.Typography.CaptionMedium.FontFamily),
			("CaptionSmall", Semantic.Typography.CaptionSmall.FontWeight, Semantic.Typography.CaptionSmall.FontFamily),
		];
		var host = new StackPanel { RequestedTheme = appearance };
		var themeRoot = new Grid { RequestedTheme = appearance };
		themeRoot.Resources.MergedDictionaries.Add(new SimpleTheme());
		themeRoot.Children.Add(host);
		var family = new FontFamily("monospace");
		foreach (var (slot, weight, fontFamily) in slots)
		{
			Assert.AreEqual(slot + "FontWeight", weight.Key);
			Assert.AreEqual(slot + "FontFamily", fontFamily.Key);
			host.Resources[weight.Key] = "Bold";
			host.Resources[fontFamily.Key] = family;
			var text = new TextBlock().Text(slot);
			host.Children.Add(text);
			// Markup resolves resources immediately: establish the nearest override scope first.
			text.FontWeight(weight).FontFamily(fontFamily);
		}
		ThemeResourceKey<FontFamily> rootFamily = Semantic.Typography.DefaultFontFamily;
		Assert.AreEqual("DefaultFontFamily", rootFamily.Key);
		host.Resources[rootFamily.Key] = family;
		var rootText = new TextBlock().Text("root");
		host.Children.Add(rootText);
		rootText.FontFamily(rootFamily);
		try
		{
			UnitTestsUIContentHelper.Content = themeRoot;
			await UnitTestsUIContentHelper.WaitForLoaded(rootText);
			await UnitTestsUIContentHelper.WaitForIdle();
			foreach (var text in host.Children.OfType<TextBlock>().Take(slots.Length))
			{
				Assert.AreEqual(Microsoft.UI.Text.FontWeights.Bold, text.FontWeight, text.Text);
				Assert.AreEqual(family.Source, text.FontFamily.Source, text.Text);
			}
			Assert.AreEqual(family.Source, rootText.FontFamily.Source);
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_PipsPathHelpersAreUsed_Then_StringConsumersResolvePathData(ElementTheme appearance)
	{
		ThemeResourceKey<string>[] keys =
		[
			Semantic.PipsPager.Resources.Default.PreviousPageButtonData,
			Semantic.PipsPager.Resources.Default.NextPageButtonData,
		];
		var host = new StackPanel { RequestedTheme = appearance };
		host.Resources.MergedDictionaries.Add(new SimpleTheme());
		const string path = "M 0,0 L 4,4 L 0,8";
		foreach (var key in keys)
		{
			// These Material path keys can also be supplied by a consumer dictionary.
			// Their Material defaults are verified in the Material runtime host.
			host.Resources[key.Key] = path;
			var text = new TextBlock();
			host.Children.Add(text);
			text.Text(key);
		}
		try
		{
			UnitTestsUIContentHelper.Content = host;
			await UnitTestsUIContentHelper.WaitForLoaded(host);
			await UnitTestsUIContentHelper.WaitForIdle();
			foreach (var text in host.Children.OfType<TextBlock>())
			{
				Assert.AreEqual(path, text.Text);
				Assert.IsInstanceOfType(Microsoft.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), text.Text), typeof(Geometry));
			}
		}
		finally
		{
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public void When_AdditionalSemanticStyleHelpersAreUsed_Then_KeysResolveToExpectedControls(ElementTheme appearance)
	{
		(StaticResourceKey<Style> Key, Type Target)[] styles =
		[
			(Semantic.ComboBoxItem.Styles.Default, typeof(ComboBoxItem)),
			(Semantic.DatePickerFlyoutPresenter.Styles.Default, typeof(DatePickerFlyoutPresenter)),
			(Semantic.MediaTransportControls.Styles.Default, typeof(MediaTransportControls)),
			(Semantic.MenuFlyoutSeparator.Styles.Default, typeof(MenuFlyoutSeparator)),
			(Semantic.MenuFlyoutSubItem.Styles.Default, typeof(MenuFlyoutSubItem)),
			(Semantic.RadioMenuFlyoutItem.Styles.Default, typeof(RadioMenuFlyoutItem)),
			(Semantic.ToggleMenuFlyoutItem.Styles.Default, typeof(ToggleMenuFlyoutItem)),
		];
		var host = new Grid { RequestedTheme = appearance };
		host.Resources.MergedDictionaries.Add(new SimpleTheme());
		foreach (var (key, target) in styles)
		{
			Assert.AreEqual(target.Name + "Style", key.Key);
			Assert.IsInstanceOfType(host.Resources[key.Key], typeof(Style), key.Key);
			var style = (Style)host.Resources[key.Key];
			Assert.AreEqual(target, style.TargetType, key.Key);
		}
		ThemeResourceKey<Color> shadow = Semantic.Colors.Shadow.Default;
		Assert.AreEqual("ShadowColor", shadow.Key);
		Assert.IsInstanceOfType(host.Resources[shadow.Key], typeof(Color));
	}
}
