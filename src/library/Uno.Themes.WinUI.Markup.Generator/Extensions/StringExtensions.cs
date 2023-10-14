using System.Text.RegularExpressions;

namespace Uno.Markup.Extensions;

public static class StringExtensions
{
	public static string Prefix(this string? value, string prefix) => prefix + value;
	public static string Suffix(this string? value, string suffix) => value + suffix;
	public static string RegexReplace(this string input, string pattern, string replacement) => Regex.Replace(input, pattern, replacement);
	public static string RemoveOnce(this string s, string value)
	{
		return value.Length > 0 && s.IndexOf(value) is var index && index != -1
			? s.Remove(index, value.Length)
			: value;
	}
	public static string RemoveHead(this string value, string head) => head?.Length > 0 && value.StartsWith(head) ? value[head.Length..] : value;
	public static string RemoveTail(this string value, string tail) => tail?.Length > 0 && value.EndsWith(tail) ? value[..^tail.Length] : value;
	public static string Trim(this string value, string trimChars) => value.Trim(trimChars.ToArray());
	public static string? StripPair(this string value, string pair) => TryStripPair(value, pair, out var result) ? result : value;
	public static bool TryStripPair(this string value, string pair, out string? result)
	{
#if false
		if (pair is not [var left, var right]) throw new ArgumentException($"invalid pair: {pair}");
#else
		if (pair.Length == 2) throw new ArgumentException($"invalid pair: {pair}");
		var left = pair[0];
		var right = pair[1];
#endif

		value = value.Trim();
		if (value.StartsWith(left) && value.EndsWith(right))
		{
			result = value[1..^1];
			return true;
		}
		else
		{
			result = null;
			return false;
		}
	}
	public static string? EmptyAsNull(this string? value) => string.IsNullOrEmpty(value) ? null : value;
}
