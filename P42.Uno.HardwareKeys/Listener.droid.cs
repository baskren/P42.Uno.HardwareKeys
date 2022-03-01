using Android.Graphics;
using Android.Runtime;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class Listener : Button
    {
        void PlatformBuild()
        {
            Focusable = true;
            //ClickMode = ClickMode.Press;
            _platformCoreElement = this;

        }

        protected override void OnKeyUp(KeyRoutedEventArgs args) { }

        public override bool OnKeyUp([GeneratedEnum] Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            var text = e.UnicodeChar > 0
                ? ((char)e.UnicodeChar).ToString()
                : String.Empty;

            var key = keyCode.AsVirtualKey();
            var modifiers = CurrentModifiers;

            if (IsNumLockEngaged == KeyState.False)
            {
                switch (keyCode)
                {
                    case Android.Views.Keycode.Numpad0: key = VirtualKey.Insert; text = String.Empty; break;
                    case Android.Views.Keycode.NumpadDot: key = VirtualKey.Delete; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad1: key = VirtualKey.End; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad2: key = VirtualKey.Down; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad3: key = VirtualKey.PageDown; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad4: key = VirtualKey.Left; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad5: key = VirtualKey.Clear; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad6: key = VirtualKey.Right; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad7: key = VirtualKey.Home; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad8: key = VirtualKey.Up; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad9: key = VirtualKey.PageUp; text = String.Empty; break;
                }
            }

            if (ProcessModifier(key, false))
            {
                // TODO: Caps & Num Locks?!?!?!
                if (!QuietModifiers)
                    OnSimpleKeyUp(text, key, modifiers);
            }
            else
                OnSimpleKeyUp(text, key, modifiers);

            //System.Diagnostics.Debug.WriteLine($"OnKeyUp: [{text}] [{keyCode.AsVirtualKey()}] ");
            //return base.OnKeyUp(keyCode, e);
            return true;
        }

        protected override void OnKeyDown(KeyRoutedEventArgs args) { }

        public override bool OnKeyDown([GeneratedEnum] Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            System.Diagnostics.Debug.WriteLine($"Droid.OnKeyDown {keyCode}");
            var text = e.UnicodeChar > 0
                ? ((char)e.UnicodeChar).ToString()
                : String.Empty;

            var key = keyCode.AsVirtualKey();
            
            SyncModifiers(e);

            if (IsNumLockEngaged == KeyState.False)
            {
                switch (keyCode)
                {
                    case Android.Views.Keycode.Numpad0: key = VirtualKey.Insert; text = String.Empty; break;
                    case Android.Views.Keycode.NumpadDot: key = VirtualKey.Delete; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad1: key = VirtualKey.End; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad2: key = VirtualKey.Down; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad3: key = VirtualKey.PageDown; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad4: key = VirtualKey.Left; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad5: key = VirtualKey.Clear; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad6: key = VirtualKey.Right; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad7: key = VirtualKey.Home; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad8: key = VirtualKey.Up; text = String.Empty; break;
                    case Android.Views.Keycode.Numpad9: key = VirtualKey.PageUp; text = String.Empty; break;
                }
            }

            if (ProcessModifier(key, true))
            {
                if (!QuietModifiers)
                    OnSimpleKeyDown(text, key);
            }
            else
                OnSimpleKeyDown(text, key);

            //System.Diagnostics.Debug.WriteLine($"OnKeyDown: [{text}] [{keyCode.AsVirtualKey()}] ");
            //return base.OnKeyDown(keyCode, e);
            return true;
        }

        void SyncModifiers(Android.Views.KeyEvent e)
        {
            IsShiftPressed = e.IsShiftPressed ? KeyState.True : KeyState.False;
            IsControlPressed = e.IsCtrlPressed ? KeyState.True : KeyState.False;
            IsWindowsPressed = e.IsMetaPressed ? KeyState.True : KeyState.False;
            IsMenuPressed = e.IsAltPressed ? KeyState.True : KeyState.False;
            if (e.KeyCode == Keycode.CapsLock)
            {
                if (IsCapsLockEngaged == KeyState.False)
                    IsCapsLockEngaged = KeyState.True;
                else if (IsCapsLockEngaged == KeyState.True)
                    IsCapsLockEngaged = KeyState.False;
            }
            else if (e.KeyCode == Keycode.NumLock)
            {
                if (IsNumLockEngaged != KeyState.True)
                    IsNumLockEngaged = KeyState.True;
                else
                    IsNumLockEngaged = KeyState.False;
            }
            else
            {
                IsCapsLockEngaged = e.IsCapsLockOn ? KeyState.True : KeyState.False;
                IsNumLockEngaged = e.IsNumLockOn ? KeyState.True : KeyState.False;
            }
        }
    }

}
