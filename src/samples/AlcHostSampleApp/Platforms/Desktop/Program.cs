using Uno.UI.Hosting;

namespace Uno.Themes.AlcHost;

internal class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseX11()
            .UseLinuxFrameBuffer()
            .UseMacOS()
            .UseWin32()
            .Build();

        await host.RunAsync();
    }
}
