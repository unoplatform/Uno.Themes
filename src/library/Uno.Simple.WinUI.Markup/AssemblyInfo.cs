using System.Reflection;
using Uno.Extensions.Markup.Generator;

[assembly: GenerateMarkupForAssembly(typeof(Uno.Simple.SimpleTheme))]

[assembly: AssemblyMetadata("IsTrimmable", "True")]
