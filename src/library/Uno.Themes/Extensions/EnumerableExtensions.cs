using System.Collections;

namespace Uno.Themes;

internal static class EnumerableExtensions
{
	public static bool Any(this IEnumerable items)
	{
		if (items is ICollection collection)
		{
			return collection.Count > 0;
		}

		var enumerator = items.GetEnumerator();

		return enumerator.MoveNext();
	}

	public static int IndexOf(this IEnumerable items, object item)
	{
		if (items == null)
		{
			return -1;
		}

		if (items is IList list)
		{
			return list.IndexOf(item);
		}

		var enumerator = items.GetEnumerator();
		for (var i = 0; ; i++)
		{
			if (!enumerator.MoveNext())
			{
				return -1;
			}

			if (enumerator.Current?.Equals(item) ?? item == null)
			{
				return i;
			}
		}
	}
}
