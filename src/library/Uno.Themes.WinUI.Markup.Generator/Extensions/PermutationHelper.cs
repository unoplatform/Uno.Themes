using System.Text.RegularExpressions;

namespace Uno.Markup.Extensions;

public static class PermutationHelper
{
	public static IEnumerable<string> PermuteWords(this string text)
	{
		return Regex.Split(text, "(?<!^)(?=[A-Z])")
			.Permute()
			.Select(arr => string.Concat(arr));
	}

	// credit: @user3277651 https://stackoverflow.com/a/21843611
	public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> source)
	{
		if (source == null || !source.Any())
			throw new ArgumentNullException("source");

		var array = source.ToArray();

		return Permute(array, 0, array.Length - 1);
	}
	private static IEnumerable<IEnumerable<T>> Permute<T>(T[] array, int i, int n)
	{
		if (i == n)
			yield return array.ToArray();
		else
		{
			for (int j = i; j <= n; j++)
			{
				array.Swap(i, j);
				foreach (var permutation in Permute(array, i + 1, n))
					yield return permutation.ToArray();
				array.Swap(i, j); //backtrack
			}
		}
	}
	private static void Swap<T>(this T[] array, int i, int j)
	{
		T temp = array[i];

		array[i] = array[j];
		array[j] = temp;
	}
}
