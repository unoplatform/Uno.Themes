using System.Text.RegularExpressions;

namespace Uno.Markup.Xaml.Helpers;

public static class ValueSimplifier
{
	public static string? SimplifyMarkup(string? value)
	{
		if (string.IsNullOrEmpty(value)) return value;
		if (!ShouldSimplify(value)) return value;

		if (Regex.Match(value, @"^{(?<type>Static|Theme)Resource (?<key>\w+)}$") is { Success: true } resourceMarkup)
		{
			return
				resourceMarkup.Groups["type"].Value[0] + // prefix with S/T, so we can still trace its original definition
				resourceMarkup.Result("[${key}]");
		}
		// fixme: naive parser
		if (Regex.Match(value, "{Binding(?<vargs>.+)?}") is { Success: true } bindingMarkup)
		{
			return bindingMarkup.Groups["vargs"].Value.Split(',')
				.Select(x => x.Trim().Split('=', 2))
				.ToDictionary(x => x.Length > 1 ? x[0] : "Path", x => x.Last())
				.TryGetValue("Path", out var path)
					? $"{{{path}}}" : "{this}";
		}

		return value;
	}
	public static string? SimplifyTimeSpan(string? value)
	{
		if (value == null) return null;
		if (!ShouldSimplify(value)) return value;

		return Regex.Replace(value, "^(00?:)+", "");
	}

	private static bool ShouldSimplify(string value) => true;
}
