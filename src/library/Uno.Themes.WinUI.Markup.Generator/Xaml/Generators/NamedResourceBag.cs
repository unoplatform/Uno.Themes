using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Markup.Xaml.Generators;

public class NamedResourceBag
{
	public string? Name { get; set; }
	public string? LeftName { get; set; }
	public string? RightName { get; set; }
	public HashSet<IResourceRef> Resources { set; get; } = new();
	public HashSet<(IResourceRef Resource, string? Context)> ContextedResources { set; get; } = new();
	public HashSet<string> Paths { set; get; } = new();
	public List<NamedResourceBag> Children { set; get; } = new();

	public bool IsVSG { get; set; }
	public bool Sortable { get; set; } = true;

	public void Merge(NamedResourceBag other)
	{
		Resources.AddRange(other.Resources);
		ContextedResources.AddRange(other.ContextedResources);
		Paths.AddRange(other.Paths);
		Children.AddRange(other.Children);
	}
}
