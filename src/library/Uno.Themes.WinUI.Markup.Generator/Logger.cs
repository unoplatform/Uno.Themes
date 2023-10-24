using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Uno.Markup;

internal static class Logger
{
	private static ILoggerFactory? _factory;
	private static ILoggerFactory Factory => _factory ?? throw new InvalidOperationException("The factory is not initialized.");

	public static void InitializeFactory(Action<ILoggingBuilder> configure)
	{
		if (_factory is { }) throw new InvalidOperationException("The factory was already initialized.");

		_factory = LoggerFactory.Create(b => b
			.Apply(configure)
			// always output to console, and additional to debug in debug
			.AddConsole()
#if DEBUG
			.AddDebug()
#endif
		);
	}

	public static ILogger<T> For<T>() => Factory.CreateLogger<T>();
	public static ILogger For(Type type) => Factory.CreateLogger(type);
	public static ILogger Log(this Type type) => Factory.CreateLogger(type);
}
