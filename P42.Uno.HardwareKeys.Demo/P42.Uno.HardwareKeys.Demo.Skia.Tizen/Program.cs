using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace P42.Uno.HardwareKeys.Demo.Skia.Tizen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new P42.Uno.HardwareKeys.Demo.App(), args);
            host.Run();
        }
    }
}
