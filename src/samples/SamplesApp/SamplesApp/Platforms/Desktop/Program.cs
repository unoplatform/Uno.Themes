using Uno.UI.Hosting;

namespace Uno.Themes.Samples;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // InitializeLogging is called from App's static constructor

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseX11()
            .UseLinuxFrameBuffer()
            .UseMacOS()
            .UseWin32()
            .Build();

        host.Run();
    }
}
