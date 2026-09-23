using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Cupertino;
using Uno.UI.RuntimeTests;
using Windows.UI;

namespace Uno.Themes.Samples.RuntimeTests;

[TestClass]
public class Given_CupertinoSemanticColors
{
	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_SemanticTextColorChanges_Then_DefaultFieldsRepaintAndRestore(ElementTheme appearance)
	{
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var original = theme.Colors;
		var text = new TextBox { Text = "Name" };
		var password = new PasswordBox { Password = "Example" };
		var panel = new StackPanel { Children = { text, password }, RequestedTheme = appearance };
		try
		{
			UnitTestsUIContentHelper.Content = panel;
			await UnitTestsUIContentHelper.WaitForLoaded(password);
			var textBrush = (SolidColorBrush)text.Foreground;
			var passwordBrush = (SolidColorBrush)password.Foreground;
			var initial = textBrush.Color;
			var replacement = Color.FromArgb(255, 89, 31, 123);
			theme.Colors = new ThemeColors { OverrideDictionary = new ResourceDictionary { ["OnSurfaceColor"] = replacement } };
			await UnitTestsUIContentHelper.WaitForIdle();
			Assert.AreSame(textBrush, text.Foreground);
			Assert.AreEqual(replacement, textBrush.Color);
			Assert.AreEqual(replacement, passwordBrush.Color);
			var specific = Color.FromArgb(255, 17, 91, 33);
			theme.Colors = new ThemeColors { OverrideDictionary = new ResourceDictionary { ["OnSurfaceColor"] = replacement, ["LabelColor"] = specific } };
			Assert.AreEqual(specific, textBrush.Color, "The explicit Cupertino role keeps precedence.");
			theme.Colors = original;
			Assert.AreEqual(initial, textBrush.Color);
			Assert.AreEqual(initial, passwordBrush.Color);
		}
		finally
		{
			theme.Colors = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}

	[TestMethod]
	[RunsOnUIThread]
	[DataRow(ElementTheme.Light)]
	[DataRow(ElementTheme.Dark)]
	public async Task When_AllSemanticRolesAreOverridden_Then_EveryStateBrushRepaintsAndRestores(ElementTheme appearance)
	{
		// This is the complete public color-role/state contract, independent of Cupertino's mappings.
		string[] roles = { "Primary", "OnPrimary", "PrimaryContainer", "OnPrimaryContainer", "PrimaryInverse", "PrimaryVariantDark", "PrimaryVariantLight",
			"Secondary", "OnSecondary", "SecondaryContainer", "OnSecondaryContainer", "SecondaryVariantDark", "SecondaryVariantLight",
			"Tertiary", "OnTertiary", "TertiaryContainer", "OnTertiaryContainer", "Error", "OnError", "ErrorContainer", "OnErrorContainer",
			"Background", "OnBackground", "Surface", "OnSurface", "SurfaceVariant", "OnSurfaceVariant", "SurfaceInverse", "OnSurfaceInverse", "SurfaceTint", "Outline", "OutlineVariant" };
		string[] states = { "", "Hover", "Focused", "Pressed", "Dragged", "Selected", "Medium", "Low", "Disabled" };
		var theme = (CupertinoTheme)Application.Current.GetTheme();
		var original = theme.Colors;
		var panel = new StackPanel { RequestedTheme = appearance };
		var snapshots = new List<(SolidColorBrush Brush, Color Color, double Opacity, Border Target, Color Expected, double ExpectedOpacity)>();
		var overrides = new ResourceDictionary();
		try
		{
			for (var i = 0; i < roles.Length; i++)
			{
				var replacement = Color.FromArgb(255, (byte)(20 + i * 5), (byte)(190 - i * 3), (byte)(40 + i * 4));
				overrides[roles[i] + "Color"] = replacement;
				for (var j = 0; j < (roles[i] == "SurfaceTint" ? 1 : states.Length); j++)
				{
					var target = (Border)Microsoft.UI.Xaml.Markup.XamlReader.Load($"<Border xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Height='1' Width='10' Background='{{ThemeResource {roles[i]}{states[j]}Brush}}' />");
					panel.Children.Add(target);
				}
			}
			UnitTestsUIContentHelper.Content = panel;
			await UnitTestsUIContentHelper.WaitForLoaded(panel);
			await UnitTestsUIContentHelper.WaitForIdle();
			var targetIndex = 0;
			// SurfaceTint has only its base brush in the shared contract (no interaction ladder).
			for (var i = 0; i < roles.Length; i++)
			{
				for (var j = 0; j < (roles[i] == "SurfaceTint" ? 1 : states.Length); j++)
				{
					var target = (Border)panel.Children[targetIndex++];
					Assert.IsInstanceOfType<SolidColorBrush>(target.Background, roles[i] + states[j] + "Brush");
					var brush = (SolidColorBrush)target.Background;
					var opacity = j == 0 ? 1 : j / 10d;
					if (j > 0) overrides[states[j] + "Opacity"] = opacity;
					snapshots.Add((brush, brush.Color, brush.Opacity, target, (Color)overrides[roles[i] + "Color"], opacity));
				}
			}
			theme.Colors = new ThemeColors { OverrideDictionary = overrides };
			foreach (var snapshot in snapshots)
			{
				Assert.AreSame(snapshot.Brush, snapshot.Target.Background);
				Assert.AreEqual(snapshot.Expected, snapshot.Brush.Color);
				Assert.AreEqual(snapshot.ExpectedOpacity, snapshot.Brush.Opacity, 0.001);
			}
			theme.Colors = original;
			foreach (var snapshot in snapshots)
			{
				Assert.AreEqual(snapshot.Color, snapshot.Brush.Color);
				Assert.AreEqual(snapshot.Opacity, snapshot.Brush.Opacity, 0.001);
			}
		}
		finally
		{
			theme.Colors = original;
			UnitTestsUIContentHelper.Content = null;
		}
	}
}
