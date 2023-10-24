using System.Reflection;
using System.Text;
using Uno.Markup.Xaml.UI;
using Uno.Markup.Xaml.UI.Xaml.Media;

namespace Uno.Markup.Extensions;

public static class MarkdownHelper
{
	public static string ToMarkdownBlock(this Color color) => $"![](http://via.placeholder.com/50x25/{color.ToRgbText()}/{color.ToRgbText()})";
	public static string? ToMarkdownBlock(this SolidColorBrush brush) => brush.Color?.ToMarkdownBlock();

	public static string ToMarkdownTable<T>(this IEnumerable<T> source)
	{
		var properties = typeof(T).GetProperties();
		var buffer = new StringBuilder();

		// header
		buffer
			.AppendLine(string.Join("|", properties.Select(x => x.Name)))
			.AppendLine(string.Join("|", Enumerable.Repeat("-", properties.Length)));

		// content
		foreach (var item in source)
		{
			buffer.AppendLine(string.Join("|", properties
				.Select(p => p.GetValue(item))
			));
		}

		return buffer.ToString();
	}
	public static string ToPaddedMarkdownTable<T>(
		this IEnumerable<T> source,
		Func<PropertyInfo, string?>? headerFormatter = null,
		Func<object?, string?>? objectFormatter = null,
		int separatorPadding = 0,
		bool separatorOnSides = false)
	{
		var properties = typeof(T).GetProperties();
		var buffer = new List<string?[]>();

		headerFormatter ??= x => x.Name;
		objectFormatter ??= x => x?.ToString();

		buffer.Add(properties.Select(headerFormatter).ToArray());
		foreach (var item in source)
		{
			buffer.Add(properties.Select(p => objectFormatter(p.GetValue(item))).ToArray());
		}

		var columnWidths = properties.Select((_, i) => buffer.Max(x => x[i]?.Length ?? 0)).ToArray();

		var builder = new StringBuilder();
		var separator = string.Concat(new string(' ', separatorPadding), "|", new string(' ', separatorPadding));

		void WriteRow(IEnumerable<string?> values)
		{
			if (separatorOnSides) builder.Append('|').Append(' ', separatorPadding);
			builder.AppendJoin(separator, values.Select((x, i) => (x ?? string.Empty).PadRight(columnWidths[i])));
			if (separatorOnSides) builder.Append(' ', separatorPadding).Append('|');
			builder.AppendLine();
		}

		WriteRow(buffer[0]);
		WriteRow(columnWidths.Select(x => new string('-', x)));
		foreach (var row in buffer.Skip(1))
		{
			WriteRow(row);
		}

		return builder.ToString();
	}
}
