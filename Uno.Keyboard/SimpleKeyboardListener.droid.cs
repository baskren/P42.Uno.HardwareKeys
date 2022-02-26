#if __ANDROID__

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

namespace Uno.Keyboard
{
    public partial class SimpleKeyboardListener : ContentControl
    {
        bool _isCapsLockOn;
        bool _isNumLockOn;

        partial void PlatformBuild()
        {
            Focusable = true;
            _platformIsCapsLockEngaged = PlatformIsCapsLockOn;
            _platformIsNumLockEngaged = PlatformIsNumLockOn;
        }

        bool PlatformIsCapsLockOn()
            => _isCapsLockOn;

        bool PlatformIsNumLockOn()
            => _isNumLockOn;

        public override bool OnKeyUp([GeneratedEnum] Android.Views.Keycode keyCode, Android.Views.KeyEvent e)
        {
            var text = e.UnicodeChar > 0
                ? ((char)e.UnicodeChar).ToString()
                : String.Empty;

            var key = keyCode.AsVirtualKey();

            if (ProcessModifier(key, false))
            {
                if (!QuietModifiers)
                    OnSimpleKeyUp(text, key);
            }
            else
                OnSimpleKeyUp(text, key);

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
            IsShiftPressed = e.IsShiftPressed;
            IsControlPressed = e.IsCtrlPressed;
            IsWindowsPressed = e.IsMetaPressed;
            IsMenuPressed = e.IsAltPressed;
            _isCapsLockOn = e.IsCapsLockOn;
            _isNumLockOn = e.IsNumLockOn;
        }
    }
}

#endif