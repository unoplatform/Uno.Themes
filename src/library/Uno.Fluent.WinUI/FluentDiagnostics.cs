#nullable enable

namespace Uno.Fluent;

/// <summary>
/// Warning-log helper for FluentTheme's graceful-degradation paths — theme
/// initialization and rebuild passes never throw; they log and fall back.
/// </summary>
internal static class FluentDiagnostics
{
	internal static void LogWarning(string message)
	{
#if HAS_UNO
		var logger = global::Uno.Extensions.LogExtensionPoint.Log(typeof(FluentTheme));
		if (logger.IsEnabled(global::Microsoft.Extensions.Logging.LogLevel.Warning))
		{
			global::Microsoft.Extensions.Logging.LoggerExtensions.LogWarning(logger, message);
		}
#else
		global::System.Diagnostics.Debug.WriteLine(message);
#endif
	}
}
