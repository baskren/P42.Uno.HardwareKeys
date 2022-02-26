using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Uno.Keyboard;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ButtonKeys
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SimpleKeyboardListener _listener;

        public MainPage()
        {
            this.InitializeComponent();

            _listener = new SimpleKeyboardListener();
            _listener.Background = new SolidColorBrush(Windows.UI.Colors.Pink);
            Grid.SetRow(_listener, 4);
            _grid.Children.Add(_listener);

            _listener.SimpleKeyDown += _listener_SimpleKeyDown;
            _listener.SimpleKeyUp += _listener_SimpleKeyUp;
        }

        private void _listener_SimpleKeyUp(object sender, UnoKeyEventArgs e)
        {
            if (e.Modifiers != null && e.Modifiers.Any())
                _upKeyTextBlock.Text = $"UP: [{string.Join("]+[", e.Modifiers)}] + [{e.SimpleKey}] ({e.Key})";
            else
                _upKeyTextBlock.Text = $"UP: [{e.SimpleKey}] ({e.Key})";
        }

        private void _listener_SimpleKeyDown(object sender, UnoKeyEventArgs e)
        {
            if (e.Modifiers != null && e.Modifiers.Any())
                _downKeyTextBlock.Text = $"DOWN: [{string.Join("]+[", e.Modifiers)}] + [{e.SimpleKey}] ({e.Key})";
            else
                _downKeyTextBlock.Text = $"DOWN: [{e.SimpleKey}] ({e.Key})";
        }
    }
}
