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

        void SyncModifiers(UIKey uiKey)
        {
            IsShiftPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Shift) != 0;
            IsControlPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Control) != 0;
            //IsWindowsPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.)
            IsMenuPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Alternate) != 0;

            IsCapsLockEngaged = (uiKey.ModifierFlags & UIKeyModifierFlags.AlphaShift) != 0;
            IsNumLockEngaged = (uiKey.ModifierFlags & UIKeyModifierFlags.NumericPad) != 0;

            System.Diagnostics.Debug.WriteLine($"{uiKey.KeyCode} {IsCapsLockEngaged}");
        }
    }
}
