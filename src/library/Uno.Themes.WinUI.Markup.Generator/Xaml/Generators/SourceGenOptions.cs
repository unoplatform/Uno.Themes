namespace Uno.Markup.Xaml.Generators;

public class SourceGenOptions
{
	// parser options
	public bool TrimControlName { get; set; } = true;
	public bool TrimMaterialPrefix { get; set; } = true;
	public bool PromoteDefaultStyleResources { get; set; } = true;
	public string? TargetType { get; set; } = null;
	public string[]? IgnoredResourceTypes { get; set; }
	public Dictionary<string, string?> ForcedGroupings { get; set; } = new();

	// generator options
	public string[]? NamespaceImports { get; set; }
	public string? Namespace { get; set; }
	public bool UseFileScopedNamespace { get; set; } = false;
	public Func<string?, string?>? XamlControlTypeResolver { get; set; }

	// options
	public bool Skip { get; set; }
	public bool Production { get; set; }
}
