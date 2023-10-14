namespace Uno.Markup.Extensions;

public static class HashSetExtensions
{
	public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			set.Add(item);
		}
	}
}
