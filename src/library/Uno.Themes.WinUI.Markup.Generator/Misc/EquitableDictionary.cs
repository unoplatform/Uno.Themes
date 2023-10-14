using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Markup.Misc;

/// <summary>
/// Standard <see cref="Dictionary{TKey, TValue}"/> with overridden <see cref="Equals(object?)"/> that checks for sequential kvp equality.
/// </summary>
public class EquitableDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{
	public override int GetHashCode()
	{
		var hash = new HashCode();
		foreach (var pair in this)
		{
			hash.Add(pair);
		}

		return hash.ToHashCode();
	}
	public override bool Equals(object? obj)
	{
		if (obj == null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj is EquitableDictionary<TKey, TValue> other)
		{
			return
				this.Count == other.Count &&
				this.GetHashCode() == other.GetHashCode();
		}

		return false;
	}
}
