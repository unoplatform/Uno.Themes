using Microsoft.Extensions.Logging;
using Uno.Markup;
using Uno.Markup.Xaml.Generators;
using Uno.Markup.Xaml.Parsers;
using Uno.Markup.Xaml.UI.Xaml;

Logger.InitializeFactory(builder => builder
	.AddFilter("Microsoft", LogLevel.Warning)
	.AddFilter("System", LogLevel.Warning)
	.AddFilter(typeof(Program).Namespace, LogLevel.Information)
);
var logger = typeof(Program).Log();

// should be set to src\library\Uno.Themes.WinUI.Markup
var cwd = Directory.GetCurrentDirectory().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
logger.LogInformation($"cwd: {cwd}");
if (!cwd.EndsWith(Path.Combine("src", "library", "Uno.Themes.WinUI.Markup")))
{
	throw new InvalidOperationException(@"Expecting to be run from: src\library\Uno.Themes.WinUI.Markup");
}

// todo: parameterize these variables to be injected from Uno.Themes.WinUI.Markup
var context = new[]
{
	@"D:\code\uno\framework\Uno\src\Uno.UI\UI\Xaml\Style\Generic\SystemResources.xaml",
	@"D:\code\uno\framework\Uno\src\Uno.UI.FluentTheme.v2\themeresources_v2.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\TextBoxVariables.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\Common\Fonts.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\Typography.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColors.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Application\v2\SharedColorPalette.xaml",
}.Aggregate(new ResourceDictionary(), (acc, file) => acc.Merge(
	ScuffedXamlParser.Load<ResourceDictionary>(file) ?? throw new Exception($"Failed to parse: {file}")
));

var controls = new[]
{
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\Button.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\CalendarDatePicker.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\CalendarView.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\CheckBox.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ComboBox.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\CommandBar.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ContentDialog.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\DatePicker.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\FloatingActionButton.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\Flyout.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\HyperlinkButton.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ListView.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\MediaPlayerElement.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\NavigationView.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\PasswordBox.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\PipsPager.Base.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\PipsPager.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ProgressBar.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ProgressRing.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\RadioButton.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\RatingControl.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\Ripple.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\Slider.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBlock.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\TextBox.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ToggleButton.xaml",
	@"D:\code\uno\platform\Uno.Themes\src\library\Uno.Material\Styles\Controls\v2\ToggleSwitch.xaml",
};
foreach (var file in controls)
{
	var options = GetOptionsFor(file);
	var extension = false ? ".g.cs" : ".cs";
	var outputPath = options.Production
		? Path.Combine(cwd, "Theme", Path.GetFileNameWithoutExtension(file) + extension)
		: file.RegexReplace(@"\.xaml$", extension);

	if (!options.Skip)
	{
		logger.LogWarning($"processing: {file}");
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

SourceGenOptions GetOptionsFor(string path)
{
	var options = (SourceGenOptions)(Path.GetFileNameWithoutExtension(path) switch
	{
		"FloatingActionButton" => new() { TargetType = "Button" },
		"Slider" => new() { ForcedGroupings = "TickBar,Track".Split(',').ToDictionary(x => x, x => default(string)) },
		"ToggleSwitch" => new() { ForcedGroupings = "Knob,Thumb".Split(',').ToDictionary(x => x, x => default(string)) },
		
		"CommandBar" or
		"MediaPlayerElement" or // fixme: not being skipped
		"PipsPager.Base" or // low prio
		"RatingControl" or
		"Flyout" or // fixme: crash
		"ListView" or
		"NavigationView" or // fixme: very very slow
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
