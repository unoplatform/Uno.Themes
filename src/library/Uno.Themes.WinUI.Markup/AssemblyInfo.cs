using System.Reflection;
using Uno.Extensions.Markup.Generator;
using Uno.Extensions.Markup.Internals;

[assembly: AssemblyMetadata("IsTrimmable", "True")]
[assembly: ResourceDefinitionClass(typeof(Uno.Themes.Markup.Theme))]
[assembly: GenerateMarkupForAssembly(typeof(Uno.Themes.BaseTheme))]
