namespace Uno.Markup.Extensions;

public static class ExceptionExtensions
{
	/// <summary>Used to dump an object inside a throw-expression</summary>
	public static TException PreDump<TException>(this TException exception, object? dump) where TException : Exception
	{
#if LINQPAD
		dump.Dump();
		return exception;
#else
		exception.Data["Dump"] = dump;
		return exception;
#endif
	}
	/// <summary>Used to dump an object inside a throw-expression</summary>
	public static TException PreDump<TException>(this TException exception, object? dump, string description) where TException : Exception
	{
#if LINQPAD
		dump.Dump(description);
		return exception;
#else
		exception.Data["Dump"] = dump;
		exception.Data["DumpHeader"] = description;
		return exception;
#endif
	}
}
