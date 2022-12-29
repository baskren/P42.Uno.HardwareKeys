using System;
using Microsoft.UI.Xaml;

namespace P42.Uno.HardwareKeys.Demo.Wasm
{
	public class Program
	{
		private static App _app;

		static int Main(string[] args)
		{
			Microsoft.UI.Xaml.Application.Start(_ => _app = new App());

			return 0;
		}
	}
}
