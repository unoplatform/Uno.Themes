#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Themes.Samples.Helpers;
using Uno.UI.RuntimeTests;

namespace Uno.Themes.Samples.RuntimeTests;

/// <summary>
/// Opens every sample page the head lists, the way the shell does. Uno tolerates XAML and code-behind
/// that WinUI rejects (undefined resource keys, unconverted resource types, events raised mid-parse,
/// dialogs without a XamlRoot), so a page that renders fine on Skia can still take the WinAppSDK head
/// down the moment it is opened. Shared by every sample head, since the pages are.
/// </summary>
[TestClass]
public class Given_AllSamplePages
{
	[TestMethod]
	[RunsOnUIThread]
	public async Task When_EverySamplePageIsOpened_Then_NoneFails()
	{
		var failures = new List<string>();

		// Some pages (Seed Color) seed the application's theme; put it back for the tests that follow.
		// Heads without a BaseTheme (Cupertino) have no seed to restore.
		using (SampleThemeHelper.GetTheme() is null ? null : new ThemeSeedSnapshot())
		{
			// RuntimeTestRunner hosts a UnitTestsControl, whose constructor takes over the engine's test
			// content root: opening it here would detach every page that follows from the tree.
			foreach (var sample in NavigationHelper.GetSamples().Where(x => x.ViewType != typeof(Content.RuntimeTestRunner)))
			{
				try
				{
					var page = (Page)Activator.CreateInstance(sample.ViewType)!;
					page.DataContext = sample;

					UnitTestsUIContentHelper.Content = page;
					await UnitTestsUIContentHelper.WaitForLoaded(page);
					await UnitTestsUIContentHelper.WaitForIdle();
				}
				catch (Exception ex)
				{
					failures.Add($"{sample.ViewType.Name}: {ex.GetBaseException().Message}");
				}
			}
		}

		Assert.AreEqual(0, failures.Count, "Sample pages failed to open:\n" + string.Join("\n", failures));
	}
}

/// <summary>
/// Restores the application theme's seed on dispose. Local values are restored as such, so "unset"
/// stays distinct from "set to the default".
/// </summary>
internal sealed class ThemeSeedSnapshot : IDisposable
{
	private readonly ThemeColors _colors = SampleThemeHelper.GetColorsOrThrow();
	private readonly object _seed;
	private readonly object _mode;

	public ThemeSeedSnapshot()
	{
		_seed = _colors.ReadLocalValue(ThemeColors.PrimarySeedProperty);
		_mode = _colors.ReadLocalValue(ThemeColors.SeedColorModeProperty);
	}

	public ThemeColors Colors => _colors;

	public void Dispose()
	{
		Restore(ThemeColors.PrimarySeedProperty, _seed);
		Restore(ThemeColors.SeedColorModeProperty, _mode);
	}

	private void Restore(DependencyProperty property, object value)
	{
		if (value == DependencyProperty.UnsetValue)
		{
			_colors.ClearValue(property);
		}
		else
		{
			_colors.SetValue(property, value);
		}
	}
}
