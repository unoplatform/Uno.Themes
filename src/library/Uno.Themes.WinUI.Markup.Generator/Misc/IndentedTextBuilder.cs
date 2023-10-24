using System.Text;

namespace Uno.Markup.Misc;

public class IndentedTextBuilder
{
	public bool BlockConsecutiveEmptyLines { get; set; }
	public int IndentLevel { get; set; }

	private readonly string padding;
	private readonly StringBuilder builder = new();
	private bool wroteEmptyLinePreviously = false;

	public IndentedTextBuilder(char paddingChar, int count) : this(new string(paddingChar, count)) { }
	public IndentedTextBuilder(string padding) => this.padding = padding;

	private IndentedTextBuilder AppendJoinLine(string separator, string[] items)
	{
		builder
			.Append(GetCurrentIndentation())
			.AppendJoin(separator, items.Where(x => x != null))
			.AppendLine();
		return this;
	}
	public IndentedTextBuilder AppendLine(string line)
	{
		builder
			.Append(GetCurrentIndentation())
			.AppendLine(line);

		wroteEmptyLinePreviously = false;
		return this;
	}
	public IndentedTextBuilder AppendEmptyLine()
	{
		if (!BlockConsecutiveEmptyLines || !wroteEmptyLinePreviously)
		{
			builder.AppendLine();

			wroteEmptyLinePreviously = true;
		}
		return this;
	}

	private string GetCurrentIndentation() => string.Concat(Enumerable.Repeat(padding, IndentLevel));
	public IDisposable Block()
	{
		IndentLevel++;
		return Disposable.Create(() => IndentLevel--);
	}
	public IDisposable Block(string opening, string closing)
	{
		AppendLine(opening);
		IndentLevel++;

		return Disposable.Create(() =>
		{
			IndentLevel--;
			AppendLine(closing);
		});
	}
	public IndentedTextBuilder Indent()
	{
		IndentLevel++;
		return this;
	}
	public IndentedTextBuilder Unindent()
	{
		IndentLevel--;
		return this;
	}

	public override string ToString() => builder.ToString();
}
