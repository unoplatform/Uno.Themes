using System.Dynamic;

namespace Uno.Markup.Extensions;

public static class PivotHelper
{
	public static IEnumerable<ExpandoObject> Pivot<T, TKey>(this IEnumerable<T> source,
		Func<T, TKey> keySelector,
		Func<T, IDictionary<string, object>> columnSelectors)
		where TKey : notnull
	{
		var results = new Dictionary<TKey, ExpandoObject>();
		foreach (var item in source)
		{
			var key = keySelector(item);
			if (!results.TryGetValue(key, out var data))
			{
				results[key] = data = new ExpandoObject();
				(data as dynamic).Key = key;
			}

			var columns = columnSelectors(item);
			columns.Populate(data);
		}

		return results.Values;
	}

	private static ExpandoObject Populate(this IDictionary<string, object> source, ExpandoObject target)
	{
		var targetImpl = target as IDictionary<string, object>;
		foreach (var kvp in source)
		{
			targetImpl[kvp.Key] = kvp.Value;
		}

		return target;
	}
}
