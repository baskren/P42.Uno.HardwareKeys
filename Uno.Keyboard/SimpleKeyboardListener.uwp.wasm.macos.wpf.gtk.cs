#if !__ANDROID__

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

namespace Uno.Keyboard
{
    public partial class SimpleKeyboardListener : Button
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

        partial void PlatformBuild()
        {
            Content = _textBox = new TextBox();
            _textBox.TextChanged += _textBox_TextChanged;
            _platformIsNumLockEngaged = PlatformNumLockOn;
            _platformIsCapsLockEngaged = PlatformCapsLockOn;
        }

        bool PlatformNumLockOn()
            => (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.NumberKeyLock) & Windows.UI.Core.CoreVirtualKeyStates.Locked) != 0;

        bool PlatformCapsLockOn()
            => (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.CapitalLock) & Windows.UI.Core.CoreVirtualKeyStates.Locked) != 0;

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            FocusManager.TryFocusAsync(_textBox, FocusState.Programmatic);
        }


        Dictionary<VirtualKey, string> FatFingerBuffer = new Dictionary<VirtualKey, string>();
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
                System.Diagnostics.Debug.Write(" ... ");
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

#endif