using Microsoft.Extensions.Logging;
using Uno.Markup;
using Uno.Markup.Xaml.Generators;
using Uno.Markup.Xaml.Parsers;
using Uno.Markup.Xaml.UI.Xaml;

Logger.InitializeFactory(builder => builder
	.AddFilter("Microsoft", LogLevel.Warning)
	.AddFilter("System", LogLevel.Warning)
#if !DEBUG
	.AddFilter(typeof(Program).Namespace, LogLevel.Information)
#else
	.AddFilter(typeof(Program).Namespace, LogLevel.Debug)
#endif
);
var logger = typeof(Program).Log();

// should be set to src\library\Uno.Themes.WinUI.Markup
var cwd = Directory.GetCurrentDirectory().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
logger.LogInformation($"cwd: {cwd}");
if (!cwd.EndsWith(Path.Combine("src", "library", "Uno.Themes.WinUI.Markup")))
{
	throw new InvalidOperationException(@"Expecting to be run from: src\library\Uno.Themes.WinUI.Markup");
}

var paths = new
{
	XamlReference = Path.GetFullPath(Path.Combine(cwd, "..", "xaml-references")),
	Material = Path.GetFullPath(Path.Combine(cwd, "..", "Uno.Material")),
	ThemeMarkup = cwd,
	ThemeMarkupGenerator = Path.GetFullPath(Path.Combine(cwd, "..", "Uno.Themes.WinUI.Markup.Generator")),
};
logger.LogDebug($"paths:");
paths.GetType().GetProperties().ForEach(p =>
	logger.LogDebug($"- {p.Name}: {p.GetValue(paths)}")
);

// fixme: to be injected from Uno.Themes.WinUI.Markup when upgrading to msbuild task
var context = new[]
{
	// fixme: to be injected from Uno.Themes.WinUI.Markup when upgrading to msbuild task
	Path.Combine(paths.XamlReference, @"SystemResources.xaml"), // src\Uno.UI\UI\Xaml\Style\Generic\SystemResources.xaml
	Path.Combine(paths.XamlReference, @"themeresources_v2.xaml"), // src\Uno.UI.FluentTheme.v2\themeresources_v2.xaml
	Path.Combine(paths.Material, @"Styles\Application\Common\TextBoxVariables.xaml"),
	Path.Combine(paths.Material, @"Styles\Application\Common\Fonts.xaml"),
	Path.Combine(paths.Material, @"Styles\Application\v2\Typography.xaml"),
	Path.Combine(paths.Material, @"Styles\Application\v2\SharedColors.xaml"),
	Path.Combine(paths.Material, @"Styles\Application\v2\SharedColorPalette.xaml"),
}.Aggregate(new ResourceDictionary(), (acc, file) => acc.Merge(
	ScuffedXamlParser.Load<ResourceDictionary>(file) ?? throw new Exception($"Failed to parse: {file}")
));

var controls = new[]
{
	// fixme: to be injected from Uno.Themes.WinUI.Markup when upgrading to msbuild task
	Path.Combine(paths.Material, @"Styles\Controls\v2\Button.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\CalendarDatePicker.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\CalendarView.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\CheckBox.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\ComboBox.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\CommandBar.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\ContentDialog.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\DatePicker.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\FloatingActionButton.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\Flyout.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\HyperlinkButton.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\ListView.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\MediaPlayerElement.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\NavigationView.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\PasswordBox.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\PipsPager.Base.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\PipsPager.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\ProgressBar.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\ProgressRing.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\RadioButton.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\RatingControl.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\Ripple.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\Slider.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\TextBlock.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\TextBox.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\ToggleButton.xaml"),
	Path.Combine(paths.Material, @"Styles\Controls\v2\ToggleSwitch.xaml"),
};
foreach (var file in controls)
{
	var options = GetOptionsFor(file);
	var extension = false ? ".g.cs" : ".cs";
	var outputPath = options.Production
		? Path.Combine(paths.ThemeMarkup, "Theme", Path.GetFileNameWithoutExtension(file) + extension)
		: file.RegexReplace(@"\.xaml$", extension);

	if (!options.Skip)
	{
		logger.LogInformation($"processing: {file}");
		var source = LightWeightSourceGen.GenerateCsMarkup(file, context, options);

		File.WriteAllText(outputPath, source);
		logger.LogInformation($"{{ lines: {source?.Count(x => x == '\n') ?? 0}, length: {source?.Length ?? 0} }} => {outputPath}");
	}
	else
	{
		logger.LogWarning($"skipped: {file}");
		File.Delete(outputPath);
	}
}

// fixme: to be injected from Uno.Themes.WinUI.Markup when upgrading to msbuild task
SourceGenOptions GetOptionsFor(string path)
{
	var options = (SourceGenOptions)(Path.GetFileNameWithoutExtension(path) switch
	{
		"FloatingActionButton" => new() { TargetType = "Button" },
		"Slider" => new() { ForcedGroupings = "TickBar,Track".Split(',').ToDictionary(x => x, x => default(string)) },
		"ToggleSwitch" => new() { ForcedGroupings = "Knob,Thumb".Split(',').ToDictionary(x => x, x => default(string)) },
		
		"CommandBar" or
		"MediaPlayerElement" or
		"PipsPager.Base" or // todo
		"RatingControl" or
		"Flyout" or // todo
		"ListView" or // todo
		"NavigationView" or // todo
		"PipsPager" or
		"PipsPager.Base" or
		"Ripple"
			=> new() { Skip = true },

		_ => new(),
	});
	options.ForcedGroupings["Typography"] = "CharacterSpacing,FontFamily,FontSize,FontWeight";
	options.PromoteDefaultStyleResources = true;
	options.IgnoredResourceTypes = "ControlTemplate,LottieVisualSource".Split(',');
	options.NamespaceImports = new[]
	{
		"System",
		"Windows.UI",
		"Microsoft.UI.Text",
		"Microsoft.UI.Xaml",
		"Microsoft.UI.Xaml.Media",
		"Uno.Extensions.Markup",
		"Uno.Extensions.Markup.Internals",
	};
	options.Namespace = "Uno.Themes.Markup";
	options.Production = true;

	return options;
}
