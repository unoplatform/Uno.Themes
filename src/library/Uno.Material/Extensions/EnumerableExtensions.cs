using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Uno.Material.Extensions
{
	internal static class EnumerableExtensions
	{
		public static bool Any(this IEnumerable items)
		{
			var collection = items as ICollection;

			if (collection != null)
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

			var list = items as IList;
			if (list != null)
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
}
