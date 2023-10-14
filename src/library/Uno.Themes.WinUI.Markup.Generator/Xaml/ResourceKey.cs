namespace Uno.Markup.Xaml;

public record ResourceKey(string? Key, string? TargetType = null) : IComparable
{
	public static implicit operator ResourceKey(string key) => new(key);

#if LINQPAD
	private object ToDump() => ToString();
#endif
	public override string ToString() => Key?.ToString() ?? $"TargetType={TargetType}";
	public static explicit operator string(ResourceKey x) => x.ToString();

	public int CompareTo(object? obj)
	{
		if (obj is ResourceKey other)
		{
			int? Compare(string? a, string? b) => (a, b) switch
			{
				(null, null) => null,
				(_, null) => 1,
				(null, _) => -1,
				(_, _) => a.CompareTo(b),
			};

			return Compare(TargetType, other.TargetType) ?? Compare(Key, other.Key) ?? 0;
		}

		return 1;
	}
}
