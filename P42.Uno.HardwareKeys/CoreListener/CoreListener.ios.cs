#if !__MACCATALYST__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Windows.System;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

#pragma warning disable CA1422 // Valzidate platform compatibility
namespace P42.Uno.HardwareKeys
{
    partial class CoreListener : TextBox
    {
        void PlatformBuild()
        {
            Name = "HardwareKeys.CoreListener";
            _platformCoreElement = this;
            UIKeyboard.Notifications.ObserveWillShow(OnShown);
            IsNumLockEngaged = KeyState.True;
            IsSpellCheckEnabled = false;
            IsTextPredictionEnabled = false;
            CharacterCasing = CharacterCasing.Normal;
            PreventKeyboardDisplayOnProgrammaticFocus = true;
        }

        
        private void OnShown(object sender, UIKeyboardEventArgs e)
        {
            // this only happens if the hardware keyboard is not enabled and, thus, the software keyboard appears
            //IsActive = false;
#if __IOS__
            if (IsActive || FocusManually)
            {
                var focused = FocusManager.GetFocusedElement();

                if (this == focused)
                    UIApplication.SharedApplication.KeyWindow.EndEditing(true);

                IsActive = false;
            }

#endif
        }

        (VirtualKey, string) MapUiKey(UIKey uiKey)
        {
            var text = uiKey.Characters;
            var key = uiKey.KeyCode.AsVirtualKey();

            if (IsNumLockEngaged == KeyState.False && (uiKey.ModifierFlags & UIKeyModifierFlags.NumericPad) != 0)
            {
                switch (key)
                {
                    case VirtualKey.NumberPad0: key = VirtualKey.Insert; text = String.Empty; break;
                    case VirtualKey.Decimal: key = VirtualKey.Delete; text = String.Empty; break;
                    case VirtualKey.NumberPad1: key = VirtualKey.End; text = String.Empty; break;
                    case VirtualKey.NumberPad2: key = VirtualKey.Down; text = String.Empty; break;
                    case VirtualKey.NumberPad3: key = VirtualKey.PageDown; text = String.Empty; break;
                    case VirtualKey.NumberPad4: key = VirtualKey.Left; text = String.Empty; break;
                    case VirtualKey.NumberPad5: key = VirtualKey.Clear; text = String.Empty; break;
                    case VirtualKey.NumberPad6: key = VirtualKey.Right; text = String.Empty; break;
                    case VirtualKey.NumberPad7: key = VirtualKey.Home; text = String.Empty; break;
                    case VirtualKey.NumberPad8: key = VirtualKey.Up; text = String.Empty; break;
                    case VirtualKey.NumberPad9: key = VirtualKey.PageUp; text = String.Empty; break;
                }
            }

            // make sure we have uniform handling across all platforms
            if (text.Length == 1 && char.IsControl(text[0]))
                text = string.Empty;

            return (key, text);
        }

        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (presses.ToArray().FirstOrDefault()?.Key is UIKey uiKey)
            {
                var (key, text) = MapUiKey(uiKey);

                SyncModifiers(uiKey);

                if (ProcessModifier(key, true))
                {
                    if (!MuteModifiers)
                        OnSimpleKeyDown(text, key);
                }
                else
                    OnSimpleKeyDown(text, key);
            }
        }


        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (presses.ToArray().FirstOrDefault()?.Key is UIKey uiKey)
            {
                var modifiers = CurrentModifiers;
                var (key, text) = MapUiKey(uiKey);

                if (ProcessModifier(key, false))
                {
                    if (!MuteModifiers)
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
            if (uiKey.KeyCode == UIKeyboardHidUsage.KeyboardCapsLock)
            {
                if (IsCapsLockEngaged == KeyState.False)
                    IsCapsLockEngaged = KeyState.True;
                else if (IsCapsLockEngaged == KeyState.True)
                    IsCapsLockEngaged = KeyState.False;
            }
            else if (uiKey.KeyCode == UIKeyboardHidUsage.KeypadNumLock)
            {
                if (IsNumLockEngaged != KeyState.True)
                    IsNumLockEngaged = KeyState.True;
                else 
                    IsNumLockEngaged = KeyState.False;
            }
            else
            {
                IsCapsLockEngaged = (uiKey.ModifierFlags & UIKeyModifierFlags.AlphaShift) != 0 ? KeyState.True : KeyState.False;
            }
            IsShiftPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Shift) != 0 ? KeyState.True : KeyState.False;
            IsControlPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Control) != 0 ? KeyState.True : KeyState.False;
            IsWindowsPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Alternate) != 0 ? KeyState.True : KeyState.False;
            //IsMenuPressed = (uiKey.ModifierFlags & UIKeyModifierFlags.Alternate) != 0 ? KeyState.True : KeyState.False;

            var flags = new List<UIKeyModifierFlags>();
            foreach (var flag in Enum.GetValues(typeof(UIKeyModifierFlags)).Cast<UIKeyModifierFlags>().ToArray())
                if ((flag & uiKey.ModifierFlags) != 0)
                    flags.Add(flag);
            //System.Diagnostics.Debug.WriteLine($"[{uiKey.KeyCode}] Modifiers: [{string.Join(",", flags)}]");
        }
    }
}
#pragma warning restore CA1422 // Validate platform compatibility


#endif

