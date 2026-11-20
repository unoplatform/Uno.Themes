#nullable enable

using System;
using Microsoft.Extensions.Logging;
using Uno.Extensions;

namespace Uno.Fluent;

/// <summary>
/// Warning-log helper for FluentTheme's graceful-degradation paths: theme initialization and
/// rebuild passes never throw; they log and fall back.
/// </summary>
internal static class FluentDiagnostics
{
	internal static void LogWarning(string message, Exception? exception = null)
	{
		var logger = typeof(FluentTheme).Log();
		if (logger.IsEnabled(LogLevel.Warning))
		{
			logger.LogWarning(exception, message);
		}
	}
}
