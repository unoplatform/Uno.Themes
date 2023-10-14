using System.Text;

namespace Uno.Markup.Extensions;

public static class HierarchyExtensions
{
	public static string TreeGraph<T>(this T node, Func<T, IEnumerable<T>> childrenSelector, Func<T, string> formatter)
	{
		return node.TreeGraph(childrenSelector, (x, i) => new string(' ', i * 4) + formatter);
	}
	public static string TreeGraph<T>(this T node, Func<T, IEnumerable<T>> childrenSelector, Func<T, int, string> formatter)
	{
		return node.TreeGraph(childrenSelector, seq => seq, formatter);
	}
	public static string TreeGraph<T>(this T node,
		Func<T, IEnumerable<T>> childrenSelector,
		Func<IEnumerable<T>, IEnumerable<T>> sorter,
		Func<T, string> formatter)
	{
		return node.TreeGraph(childrenSelector, sorter, (x, i) => new string(' ', i * 4) + formatter);
	}
	public static string TreeGraph<T>(this T node,
		Func<T, IEnumerable<T>> childrenSelector,
		Func<IEnumerable<T>, IEnumerable<T>> sorter,
		Func<T, int, string> formatter)
	{
		var buffer = new StringBuilder();
		WriteNode(node, 0);

		void WriteNode(T node, int depth)
		{
			buffer.AppendLine(formatter(node, depth));
			foreach (var child in sorter(childrenSelector(node)))
			{
				WriteNode(child, depth + 1);
			}
		}

		return buffer.ToString();
	}

	public static IEnumerable<T> Flatten<T>(this T element, Func<T, IEnumerable<T>> childrenSelector)
	{
		yield return element;
		foreach (var child in childrenSelector(element) ?? Enumerable.Empty<T>())
		{
			foreach (var item in Flatten(child, childrenSelector))
			{
				yield return item;
			}
		}
	}
	public static IEnumerable<(string Path, T Node)> Flatten<T>(this T element,
		Func<T, IEnumerable<T>> childrenSelector,
		Func<T, string> nodeDescriptor,
		string separator = @"/")
		=> Flatten(element, childrenSelector, nodeDescriptor, separator, null);
	private static IEnumerable<(string Path, T Node)> Flatten<T>(this T element,
		Func<T, IEnumerable<T>> childrenSelector,
		Func<T, string> nodeDescriptor,
		string separator,
		string? path)
	{
		path += separator + nodeDescriptor(element);

		yield return (path, element);
		foreach (var child in childrenSelector(element) ?? Enumerable.Empty<T>())
		{
			foreach (var item in Flatten(child, childrenSelector, nodeDescriptor, separator, path))
			{
				yield return (item.Path, item.Node);
			}
		}
	}
	public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
	{
		return source.SelectMany(x => Flatten(x, childrenSelector));
	}
}
