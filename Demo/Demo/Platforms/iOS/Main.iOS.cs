using UIKit;
using Uno.UI.Hosting;

namespace P42.Uno.HardwareKeys.Demo.iOS;

public class EntryPoint
{
    // This is the main entry point of the application.
    public static void Main(string[] args)
    {
        App.InitializeLogging();

        var host = UnoPlatformHostBuilder.Create()
            .App(() => new App())
            .UseAppleUIKit()
            .Build();

        host.Run();
    }
}
