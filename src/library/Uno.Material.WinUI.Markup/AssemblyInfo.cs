using System.Reflection;
using Uno.Extensions.Markup.Generator;

[assembly: GenerateMarkupForAssembly(typeof(Uno.Material.MaterialResources))]
[assembly: GenerateMarkupForAssembly(typeof(Uno.Themes.BaseTheme))]
[assembly: AssemblyMetadata("IsTrimmable", "True")]
