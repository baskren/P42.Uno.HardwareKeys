using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace P42.Uno.HardwareKeys
{
    partial class CoreListener : Button
    {
        /*
        * 
        * NOTES:
        * 
        * numpad 5 : clear
        * 
        * Keys that don't OnKeyDown
        *      WASM: HOME, END, LEFT, RIGHT 
        *      UWP: INSERT, DELETE 
        *      WPF: HOME, END, LEFT, RIGHT, F10 (up[`]) 
        *      GtK: HOME, END, LEFT, RIGHT
        *      
        * Keys that are not right
        *      WASM:
        *      UWP:
        *      WPF: CONTROL+, WINDOWS+, MENU+
        *      
        * 
        */

        TextBox _textBox;

        void PlatformBuild()
        {
            Content = _platformCoreElement = _textBox = new TextBox();
            _textBox.Name = "HardwareKeys.CoreListener";
            _textBox.TextChanged += _textBox_TextChanged;

        }

        partial void PlatformNumLockQuery()
            => IsNumLockEngaged = (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.NumberKeyLock) & Windows.UI.Core.CoreVirtualKeyStates.Locked) != 0
            ? KeyState.True
            : KeyState.False;

        partial void PlatformCapsLockQuery()
            => IsCapsLockEngaged = (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.CapitalLock) & Windows.UI.Core.CoreVirtualKeyStates.Locked) != 0
            ? KeyState.True
            : KeyState.False;

        partial void PlatformShiftPressedQuery()
            => IsShiftPressed = (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) & Windows.UI.Core.CoreVirtualKeyStates.Down) != 0
            ? KeyState.True
            : KeyState.False;

        partial void PlatformControlPressedQuery()
            => IsControlPressed = (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.Control) & Windows.UI.Core.CoreVirtualKeyStates.Down) != 0
            ? KeyState.True
            : KeyState.False;

        partial void PlatformMenuPressedQuery()
            => IsMenuPressed = (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu) & Windows.UI.Core.CoreVirtualKeyStates.Down) != 0
            ? KeyState.True
            : KeyState.False;

        partial void PlatformWindowsPressedQuery()
            => IsWindowsPressed = ((Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.LeftWindows) | Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.RightWindows)) & Windows.UI.Core.CoreVirtualKeyStates.Down) != 0
            ? KeyState.True
            : KeyState.False;


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            FocusManager.TryFocusAsync(_textBox, FocusState.Programmatic);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }


        readonly Dictionary<VirtualKey, string> FatFingerBuffer = new Dictionary<VirtualKey, string>();
        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_waitingForText && !string.IsNullOrEmpty(_textBox.Text))
            {
                var key = _textBox.Text.Substring(_textBox.Text.Length - 1);
                FatFingerBuffer[_lastKeyDown] = key;
                OnSimpleKeyDown(key, _lastKeyDown);
                _lastKeyDown = VirtualKey.None;
            }
        }

        VirtualKey _lastKeyDown;
        bool _waitingForText;
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);

            if (IsCharacterKey(e.Key))
            {
                _waitingForText = true;
                //System.Diagnostics.Debug.Write(" ... ");
            }
            else
            {
                e.Handled = true;

                if (ProcessModifier(e.Key, true))
                {
                    if (!QuietModifiers)
                        OnSimpleKeyDown(string.Empty, e.Key);
                }
                else
                    OnSimpleKeyDown(string.Empty, e.Key);
            }

            _lastKeyDown = e.Key;
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            var modifiers = CurrentModifiers;
            base.OnKeyUp(e);

            var key = _textBox.Text;
            if (string.IsNullOrEmpty(key))
            {
                if (FatFingerBuffer.TryGetValue(e.Key, out var txt))
                    key = txt;
            }
            else
                key = key.Substring(key.Length - 1);

            if (ProcessModifier(e.Key, false))
            {
                if (!QuietModifiers)
                    OnSimpleKeyUp(key, e.Key, modifiers);
            }
            else
                OnSimpleKeyUp(key, e.Key, modifiers);

            _textBox.Text = String.Empty;
        }

    }
}
