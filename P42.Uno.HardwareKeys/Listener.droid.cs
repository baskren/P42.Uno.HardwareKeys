using Android.Runtime;
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
    public partial class Listener : ContentControl
    {
        void PlatformBuild()
        {
            Focusable = true;
            _platformCoreElement = this;
        }

        public override bool OnKeyUp([GeneratedEnum] Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            var text = e.UnicodeChar > 0
                ? ((char)e.UnicodeChar).ToString()
                : String.Empty;

            var key = keyCode.AsVirtualKey();
            var modifiers = CurrentModifiers;

            if (ProcessModifier(key, false))
            {
                // TODO: Caps & Num Locks?!?!?!
                if (!QuietModifiers)
                    OnSimpleKeyUp(text, key, modifiers);
            }
            else
                OnSimpleKeyUp(text, key, modifiers);

            //System.Diagnostics.Debug.WriteLine($"OnKeyUp: [{text}] [{keyCode.AsVirtualKey()}] ");
            return base.OnKeyUp(keyCode, e);
        }


        public override bool OnKeyDown([GeneratedEnum] Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            var text = e.UnicodeChar > 0
                ? ((char)e.UnicodeChar).ToString()
                : String.Empty;

            var key = keyCode.AsVirtualKey();

            SyncModifiers(e);

            if (ProcessModifier(key, true))
            {
                if (!QuietModifiers)
                    OnSimpleKeyDown(text, key);
            }
            else
                OnSimpleKeyDown(text, key);

            //System.Diagnostics.Debug.WriteLine($"OnKeyDown: [{text}] [{keyCode.AsVirtualKey()}] ");
            return base.OnKeyDown(keyCode, e);
        }

        void SyncModifiers(Android.Views.KeyEvent e)
        {
            IsShiftPressed = e.IsShiftPressed ? KeyState.True : KeyState.False;
            IsControlPressed = e.IsCtrlPressed ? KeyState.True : KeyState.False;
            IsWindowsPressed = e.IsMetaPressed ? KeyState.True : KeyState.False;
            IsMenuPressed = e.IsAltPressed ? KeyState.True : KeyState.False;
            IsCapsLockEnabled = e.IsCapsLockOn ? KeyState.True : KeyState.False;
            IsNumLockEnabled = e.IsNumLockOn ? KeyState.True : KeyState.False;
        }
    }
}
