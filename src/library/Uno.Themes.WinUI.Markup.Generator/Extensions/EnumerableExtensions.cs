namespace Uno.Markup.Extensions;

public static class EnumerableExtensions
{
	public static IEnumerable<T> Safe<T>(this IEnumerable<T>? source) => source is not null ? source : Array.Empty<T>();
	public static T? JustOneOrDefault<T>(this IEnumerable<T> source) where T : class
	{
		T? first = null;
		foreach (var item in source)
		{
			if (first != null) return null;
			first = item;
		}

		return first;
	}
	public static T? JustOneOrNull<T>(this IEnumerable<T> source) where T : struct
	{
		T? first = null;
		foreach (var item in source)
		{
			if (first != null) return null;
			first = item;
		}

		return first;
	}
	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (var item in source)
		{
			action(item);
		}
	}
	public static string JoinBy<T>(this IEnumerable<T> source, string separator)
	{
		return string.Join(separator, source);
	}
	public static IEnumerable<T> MustAll<T>(this IEnumerable<T> source, Func<T, bool> predicate, Func<T, string>? formatError = null)
	{
		foreach (var item in source)
		{
			if (!predicate(item))
				throw new ArgumentOutOfRangeException(nameof(item), item, formatError?.Invoke(item) ?? "Invalid value").PreDump(item);

			yield return item;
		}
	}
	public static IEnumerable<T>? EmptyAsNull<T>(this IEnumerable<T> source) => source.ToArray() is { Length: > 0 } array ? array : null;
	public static bool IsSubsetBy<T, TKey>(this IEnumerable<T> source, IEnumerable<T> superset, Func<T, TKey> keySelector)
	{
		return !source.Select(keySelector).IsSubset(superset.Select(keySelector));
	}
	public static bool IsSubset<T>(this IEnumerable<T> source, IEnumerable<T> superset)
	{
		return !source.Except(superset).Any();
	}
	public static T? FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : struct
	{
		foreach (var element in source)
		{
			if (predicate(element)) return element;
		}

		return null;
	}
}
