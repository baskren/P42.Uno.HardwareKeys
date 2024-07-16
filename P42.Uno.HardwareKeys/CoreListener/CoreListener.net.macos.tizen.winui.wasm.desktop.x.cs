/*******************************************************************
 *
 * Common source for .net bait, mac, tizen, windows, wasm, and skia
 *
 * IF YOU MAKE A CHANGE HERE, BE SURE TO UPDATE ALL OF THE ABOVE!
 *
 **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

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
            //_textBox.TextChanged += _textBox_TextChanged;
            _textBox.BeforeTextChanging += _textBox_BeforeTextChanging;
            _textBox.Foreground = new SolidColorBrush(Color.FromArgb(1, 128, 128, 128));
        }

        partial void PlatformNumLockQuery()
            => IsNumLockEngaged = WinUIKeyEngaged(VirtualKey.NumberKeyLock, CoreVirtualKeyStates.Locked);

        partial void PlatformCapsLockQuery()
            => IsCapsLockEngaged = WinUIKeyEngaged(VirtualKey.CapitalLock, CoreVirtualKeyStates.Locked);

        partial void PlatformShiftPressedQuery()
            => IsShiftPressed = WinUIKeyEngaged(VirtualKey.Shift, CoreVirtualKeyStates.Down);

        partial void PlatformControlPressedQuery()
            => IsControlPressed = WinUIKeyEngaged(VirtualKey.Control, CoreVirtualKeyStates.Down);

        partial void PlatformMenuPressedQuery()
            => IsMenuPressed = WinUIKeyEngaged(VirtualKey.Menu, CoreVirtualKeyStates.Down);

        partial void PlatformWindowsPressedQuery()
            => IsWindowsPressed = WinUIKeyEngaged(VirtualKey.LeftWindows, VirtualKey.RightWindows, CoreVirtualKeyStates.Down);


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            //FocusManager.TryFocusAsync(_textBox, FocusState.Programmatic);
            TryFocusAsync(_textBox, FocusState.Programmatic).Forget();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        

        readonly Dictionary<VirtualKey, string> FatFingerBuffer = new Dictionary<VirtualKey, string>();
        private void _textBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (_waitingForText && !string.IsNullOrEmpty(args.NewText))
            {
                //var key = _textBox.Text.Substring(_textBox.Text.Length - 1);
                FatFingerBuffer[_lastKeyDown] = args.NewText;
                OnSimpleKeyDown(args.NewText, _lastKeyDown);
                _lastKeyDown = VirtualKey.None;
            }
            args.Cancel = true;
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
                    if (!MuteModifiers)
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
                if (!MuteModifiers)
                    OnSimpleKeyUp(key, e.Key, modifiers);
            }
            else
                OnSimpleKeyUp(key, e.Key, modifiers);

            //_textBox.Text = String.Empty;
        }

    }
}
