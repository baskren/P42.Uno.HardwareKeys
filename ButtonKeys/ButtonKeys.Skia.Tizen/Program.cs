using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace ButtonKeys.Skia.Tizen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new ButtonKeys.App(), args);
            host.Run();
        }
    }
}
