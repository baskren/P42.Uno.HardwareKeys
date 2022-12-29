using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using P42.Uno.HardwareKeys;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace P42.Uno.HardwareKeys.Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Listener _listener;
        Brush _gray = new SolidColorBrush(Color.FromArgb(0x7F, 0x7F, 0x7F, 0x7F));
        Brush _transparent = new SolidColorBrush(Colors.Transparent);
        Brush _unknown = new SolidColorBrush(Color.FromArgb(0x3F, 0x7F, 0x7F, 0x7F));

        public MainPage()
        {
            this.InitializeComponent();

            FocusManager.GotFocus += FocusManager_GotFocus;

            UpdateModifiers(null, false);

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

        private void _listener_KeyUp(object sender, UnoKeyEventArgs e)
        {
            if (e.Modifiers != null && e.Modifiers.Any())
                _upKeyTextBlock.Text = $"<<{e.Characters}>>  [{string.Join("]+[", e.Modifiers)}] + [{e.VirtualKey}]";
            else
                _upKeyTextBlock.Text = $"<<{e.Characters}>>  [{e.VirtualKey}]";
        }

        private void _listener_KeyDown(object sender, UnoKeyEventArgs e)
        {
            if (e.Modifiers != null && e.Modifiers.Any())
                _downKeyTextBlock.Text = $"<<{e.Characters}>>  [{string.Join("]+[", e.Modifiers)}] + [{e.VirtualKey}]";
            else
                _downKeyTextBlock.Text = $"<<{e.Characters}>>  [{e.VirtualKey}]";
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
