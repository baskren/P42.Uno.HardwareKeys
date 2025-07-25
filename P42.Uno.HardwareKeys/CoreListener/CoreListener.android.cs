using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace P42.Uno.HardwareKeys;

partial class CoreListener : Button
{
    private static WeakReference<CoreListener>? _activeWeakRef = null;
    
    public static CoreListener? Active
    {
        get
        {
            if (_activeWeakRef is null || !_activeWeakRef.TryGetTarget(out var listener))
                return null;

            return listener;
        }
    }
    
    void PlatformBuild()
    {
        Name = "HardwareKeys.CoreListener";
        _platformCoreElement = this;
#if __IOS__
        UIKeyboard.Notifications.ObserveWillShow(OnShown);
#endif
        //AcceptsReturn = false;
        //IsReadOnly = false;
        IsNumLockEngaged = KeyState.Unknown;
        //IsSpellCheckEnabled = false;
        //IsTextPredictionEnabled = false;
        //CharacterCasing = CharacterCasing.Normal;
        //PreventKeyboardDisplayOnProgrammaticFocus = true;
        FocusState = FocusState.Unfocused;
        HorizontalAlignment = HorizontalAlignment.Stretch;

        //TextChanged += OnTextChanged;
        
        IsShiftPressed = KeyState.Unknown;
        IsControlPressed = KeyState.Unknown;
        IsWindowsPressed = KeyState.Unknown;
        IsMenuPressed = KeyState.Unknown;
        IsCapsLockEngaged = KeyState.Unknown;
        IsNumLockEngaged = KeyState.Unknown;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        //Text = string.Empty; // Clear text to prevent showing the keyboard
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
        _activeWeakRef = new WeakReference<CoreListener>(this);
        base.OnGotFocus(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        _activeWeakRef = null;
        base.OnLostFocus(e);
    }

    public bool DispatchKeyEvent(Android.Views.KeyEvent e)
    {
        var text = e.UnicodeChar > 0
            ? ((char)e.UnicodeChar).ToString()
            : String.Empty;

        var key = e.KeyCode.AsVirtualKey();
        var modifiers = e.GetModifiers();

        if (e.ScanCode == 111) // ScanCode 111 is the "Back" button
        {
            // Handle the back button press
            OnSimpleKeyDown(string.Empty, VirtualKey.Delete, CurrentModifiers);
            OnSimpleKeyUp(string.Empty, VirtualKey.Delete, CurrentModifiers);
            return true; // Indicate that the event was handled
        }

        
        if (IsNumLockEngaged == KeyState.False || e.IsNumLockOn)
        {
            switch (e.KeyCode)
            {
                case Android.Views.Keycode.Numpad0: key = VirtualKey.Insert; text = string.Empty; break;
                case Android.Views.Keycode.NumpadDot: key = VirtualKey.Delete; text = string.Empty; break;
                case Android.Views.Keycode.Numpad1: key = VirtualKey.End; text = string.Empty; break;
                case Android.Views.Keycode.Numpad2: key = VirtualKey.Down; text = string.Empty; break;
                case Android.Views.Keycode.Numpad3: key = VirtualKey.PageDown; text = string.Empty; break;
                case Android.Views.Keycode.Numpad4: key = VirtualKey.Left; text = string.Empty; break;
                case Android.Views.Keycode.Numpad5: key = VirtualKey.Clear; text = string.Empty; break;
                case Android.Views.Keycode.Numpad6: key = VirtualKey.Right; text = string.Empty; break;
                case Android.Views.Keycode.Numpad7: key = VirtualKey.Home; text = string.Empty; break;
                case Android.Views.Keycode.Numpad8: key = VirtualKey.Up; text = string.Empty; break;
                case Android.Views.Keycode.Numpad9: key = VirtualKey.PageUp; text = string.Empty; break;
            }
        }

        if (ProcessModifier(key, false))
        {
            // TODO: Caps & Num Locks?!?!?!
            if (!MuteModifiers)
                SendKeyEvents(text, key, modifiers);
        }
        else
            SendKeyEvents(text, key, modifiers);

        //System.Diagnostics.Debug.WriteLine($"OnKeyUp: [{text}] [{keyCode.AsVirtualKey()}] ");
        //return base.OnKeyUp(keyCode, e);
        return true;
    }
    
    private void SendKeyEvents(string simpleKey, VirtualKey virtualKey, VirtualKey[] modifiers = null)
    {
        OnSimpleKeyDown(simpleKey, virtualKey, modifiers);
        OnSimpleKeyUp(simpleKey, virtualKey, modifiers);
    }
}

