namespace Uno.Markup.Misc;

public class SequentialEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
{
	public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
	{
		return GetHashCode(x) == GetHashCode(y);
	}

	public int GetHashCode(IEnumerable<T>? obj)
	{
		if (obj is null) return 0;

		// note: Enumerable.Aggregate(new HashCode(), ...) is passed around by value...
		var hash = new HashCode();
		foreach (var item in obj.Order())
		{
			hash.Add(item);
		}

		return hash.ToHashCode();
	}
}
