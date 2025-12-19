namespace Uno.Themes.Samples.Helpers;

public static class HierarchyHelper
{
	public static IEnumerable<T> Flatten<T>(T element, Func<T, IEnumerable<T>> childrenSelector)
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

	public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
	{
		return source.SelectMany(x => Flatten(x, childrenSelector));
	}
}
