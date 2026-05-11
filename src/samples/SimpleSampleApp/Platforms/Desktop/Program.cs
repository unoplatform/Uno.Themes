using System.Runtime.CompilerServices;
using Uno.UI.Hosting;

namespace Uno.Themes.Simple.Samples;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // Force the runtime-tests assembly's module initializer to run.
        // The engine's [ModuleInitializer] (RuntimeTestEmbeddedRunner.AutoStartTests)
        // reads UNO_RUNTIME_TESTS_* env vars and hooks into the app lifecycle.
        // Without this, the assembly is never loaded before the UI loop starts
        // and CI runtime-tests hang until timeout.
        RuntimeHelpers.RunModuleConstructor(
            typeof(Uno.Themes.Samples.RuntimeTests.Simple.Given_SemanticStyles).Module.ModuleHandle);

        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseX11()
            .UseLinuxFrameBuffer()
            .UseMacOS()
            .UseWin32()
            .Build();

        host.RunAsync();
    }
}
