using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using P42.Uno.HardwareKeys;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace P42.Uno.HardwareKeys.Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Listener _listener;
        Brush _gray = new SolidColorBrush(Color.FromArgb(0x7F, 0x7F, 0x7F, 0x7F));
        Brush _transparent = new SolidColorBrush(Colors.Transparent);
        Brush _unknown = new SolidColorBrush(Color.FromArgb(0x3F, 0x7F, 0x7F, 0x7F));
        DependencyObject _lastFocusedElement;

        public MainPage()
        {
            this.InitializeComponent();

            Windows.UI.Xaml.Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            _listener = new Listener();
            _listener.Name = "HardwareKeys.Listener";
            Grid.SetRow(_listener, 6);
            _grid.Children.Add(_listener);

            _listener.SimpleKeyDown += _listener_SimpleKeyDown;
            _listener.SimpleKeyUp += _listener_SimpleKeyUp;

            //_toggleFocusButton.Click += _toggleFocusButton_Click;

            FocusManager.GotFocus += FocusManager_GotFocus;
            FocusManager.LosingFocus += FocusManager_LosingFocus;


            _listener.IsCapsLockEngagedChanged += UpdateModifiers;
            _listener.IsShiftPressedChanged += UpdateModifiers;
            _listener.IsControlPressedChanged += UpdateModifiers;
            _listener.IsWindowsPressedChanged += UpdateModifiers;
            _listener.IsMenuPressedChanged += UpdateModifiers;
            _listener.IsNumLockEngagedChanged += UpdateModifiers;

            UpdateModifiers(null, false);

        }


        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
#if __IOS__ || __ANDROID__
            System.Diagnostics.Debug.WriteLine($"CoreWindow_KeyDown: {args.VirtualKey} {args.KeyStatus}");
#endif
        }

        void UpdateModifiers(object sender, bool state)
            => UpdateModifiers(null, KeyState.Unknown);

        void UpdateModifiers(object sender, KeyState state)
        {
            _capsLockBorder.Background = _listener.IsCapsLockEngaged == KeyState.True ? _gray : _listener.IsCapsLockEngaged == KeyState.False ? _transparent : _unknown;
            _shiftBorder.Background = _listener.IsShiftPressed == KeyState.True ? _gray : _listener.IsShiftPressed == KeyState.False ? _transparent : _unknown;
            _controlBorder.Background = _listener.IsControlPressed == KeyState.True ? _gray : _listener.IsControlPressed == KeyState.False ? _transparent : _unknown;
            _windowsBorder.Background = _listener.IsWindowsPressed == KeyState.True ? _gray : _listener.IsWindowsPressed == KeyState.False ? _transparent : _unknown;
            _menuBorder.Background = _listener.IsMenuPressed == KeyState.True ? _gray : _listener.IsMenuPressed == KeyState.False ? _transparent : _unknown;
            _numLockBorder.Background = _listener.IsNumLockEngaged == KeyState.True ? _gray : _listener.IsNumLockEngaged == KeyState.False ? _transparent : _unknown;
        }

        private void _listener_SimpleKeyUp(object sender, UnoKeyEventArgs e)
        {
            if (e.Modifiers != null && e.Modifiers.Any())
                _upKeyTextBlock.Text = $"<<{e.Characters}>>  [{string.Join("]+[", e.Modifiers)}] + [{e.VirtualKey}]";
            else
                _upKeyTextBlock.Text = $"<<{e.Characters}>>  [{e.VirtualKey}]";
        }

        private void _listener_SimpleKeyDown(object sender, UnoKeyEventArgs e)
        {
            if (e.Modifiers != null && e.Modifiers.Any())
                _downKeyTextBlock.Text = $"<<{e.Characters}>>  [{string.Join("]+[", e.Modifiers)}] + [{e.VirtualKey}]";
            else
                _downKeyTextBlock.Text = $"<<{e.Characters}>>  [{e.VirtualKey}]";
        }

        private void FocusManager_LosingFocus(object sender, LosingFocusEventArgs e)
        {
            _lastFocusedElement = e.OldFocusedElement;
        }


        private void FocusManager_GotFocus(object sender, FocusManagerGotFocusEventArgs e)
        {
            if (e.NewFocusedElement is FrameworkElement element)
                _currentFocusTextBlock.Text = element.Name;
            else
                _currentFocusTextBlock.Text = e.NewFocusedElement?.ToString() ?? "NO FOCUS";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var element = FocusManager.GetFocusedElement();
            if (element is FrameworkElement focusedElement)
                _currentFocusTextBlock.Text = focusedElement.Name;
            else
                _currentFocusTextBlock.Text = element?.ToString() ?? "NO FOCUS";
        }


        private void _hwKeysActiveToggle_Toggled(object sender, RoutedEventArgs e)
        {
            _listener.IsActive = _hwKeysActiveToggle.IsOn;
        }

    }
}
