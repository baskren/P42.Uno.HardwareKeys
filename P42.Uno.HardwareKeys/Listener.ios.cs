using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace P42.Uno.HardwareKeys
{
    public partial class Listener : TextBox
    {
        bool _isCapsLockOn;
        bool _isNumLockOn;

        void PlatformBuild()
        {
            _platformCoreElement = this;
        }


        private void OnShown(object sender, UIKeyboardEventArgs e)
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }

        

        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesBegan(presses, evt);
            if (presses.ToArray().FirstOrDefault()?.Key is UIKey uiKey)
            {
                var text = uiKey.Characters;
                var key = uiKey.KeyCode.AsVirtualKey();

                SyncModifiers(uiKey);
                SyncLocks(uiKey);

                if (ProcessModifier(key, true))
                    SyncModifiers(uiKey);

                if (ProcessModifier(key, true))
                {
                    //TODO: Caps and NumLock!?!?!
                    if (!QuietModifiers)
                        OnSimpleKeyDown(text, key);
                }
                else
                    OnSimpleKeyDown(text, key);
            }
        }

        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesEnded(presses, evt);
            if (presses.ToArray().FirstOrDefault()?.Key is UIKey uiKey)
            {
                var text = uiKey.Characters;
                var key = uiKey.KeyCode.AsVirtualKey();
                var modifiers = CurrentModifiers;

                //SyncLocks(uiKey);

                if (ProcessModifier(key, false))
                {
                    if (!QuietModifiers)
                        OnSimpleKeyUp(text, key, modifiers);
                }
                else
                    OnSimpleKeyUp(text, key, modifiers);
            }

            // Do we need fat finger support?
            Text = String.Empty;
        }

        void SyncLocks(UIKey uiKey)
        {
            if (uiKey.KeyCode == UIKeyboardHidUsage.KeyboardCapsLock)
            {
                if (IsCapsLockEngaged == KeyState.True)
                    IsCapsLockEngaged = KeyState.False;
                else if (IsCapsLockEngaged == KeyState.False)
                    IsCapsLockEngaged = KeyState.True;
            }
            else if (uiKey.KeyCode == UIKeyboardHidUsage.KeypadNumLock)
            {
                if (IsNumLockEngaged == KeyState.True)
                    IsNumLockEngaged = KeyState.False;
                else if (IsNumLockEngaged == KeyState.False)
                    IsNumLockEngaged = KeyState.True;
            }
            else
            {
                IsCapsLockEngaged = (uiKey.ModifierFlags & UIKeyModifierFlags.AlphaShift) != 0
                    ? KeyState.True : KeyState.False;
                IsNumLockEngaged = (uiKey.ModifierFlags & UIKeyModifierFlags.NumericPad) != 0
                    ? KeyState.True : KeyState.False;
            }
            System.Diagnostics.Debug.WriteLine($"UIKey.KeyCode: [{uiKey.KeyCode}] IsCapsLockEngaged[{IsCapsLockEngaged}] IsNumLockEngaged[{IsNumLockEngaged}]");
        }

        void SyncModifiers(UIKey uiKey)
        {
            IsShiftPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Shift) != 0 ? KeyState.True : KeyState.False;
            IsControlPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Control) != 0 ? KeyState.True : KeyState.False;
            //IsWindowsPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.)
            IsMenuPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Alternate) != 0 ? KeyState.True : KeyState.False;


        }
    }
}
