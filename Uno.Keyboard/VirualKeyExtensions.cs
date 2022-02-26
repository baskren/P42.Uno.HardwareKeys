using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;

#if __ANDROID__
using Android.Views;
#endif

namespace Uno.Keyboard
{
    public static class VirualKeyExtensions
    {
        internal static string Simplify(this VirtualKey key)
        {
            var txt = key.ToString();
            if (txt.StartsWith("NumberPad"))
                return txt.Substring("NumberPad".Length);
            if (txt.StartsWith("Number"))
                return txt.Substring("Number".Length);

            switch(key)
            {
                case VirtualKey.None:
                    return string.Empty;
                case VirtualKey.LeftWindows:
                case VirtualKey.RightWindows:
                    return "Windows";
                case VirtualKey.Add:
                    return "+";
                case VirtualKey.Subtract:
                    return "-";
                case VirtualKey.Multiply:
                    return "*";
                case VirtualKey.Divide:
                    return "/";
                case VirtualKey.Decimal:
                    return ".";
                case VirtualKey.LeftShift:
                case VirtualKey.RightShift:
                    return "Shift";
                case VirtualKey.LeftControl:
                case VirtualKey.RightControl:
                    return "Control";
                case VirtualKey.LeftMenu:
                case VirtualKey.RightMenu:
                    return "Menu";
            }

            return key.ToString();
        }


#if __ANDROID__


        internal static VirtualKey AsVirtualKey(this Keycode keycode)
        {
            if (KeyCodeMap.TryGetValue(keycode, out VirtualKey virtualKey))
                return virtualKey;
            return VirtualKey.None;
        }

        readonly static Dictionary<Keycode, VirtualKey> KeyCodeMap = new Dictionary<Keycode, VirtualKey>
        {
            { Keycode.Num0, VirtualKey.Number0 },
            { Keycode.Num1, VirtualKey.Number1 },
            { Keycode.Num2, VirtualKey.Number2 },
            { Keycode.Num3, VirtualKey.Number3 },
            { Keycode.Num4, VirtualKey.Number4 },
            { Keycode.Num5, VirtualKey.Number5 },
            { Keycode.Num6, VirtualKey.Number6 },
            { Keycode.Num7, VirtualKey.Number7 },
            { Keycode.Num8, VirtualKey.Number8 },
            { Keycode.Num9, VirtualKey.Number9 },
            { Keycode.A, VirtualKey.A },
            { Keycode.AltLeft, VirtualKey.LeftMenu },
            { Keycode.AltRight, VirtualKey.RightMenu },
            { Keycode.AppSwitch, VirtualKey.Application },
            { Keycode.B, VirtualKey.B },
            { Keycode.Back, VirtualKey.Back },
            { Keycode.ButtonA, VirtualKey.GamepadA },
            { Keycode.ButtonB, VirtualKey.GamepadB },
            { Keycode.ButtonMode, VirtualKey.ModeChange },
            { Keycode.ButtonX, VirtualKey.GamepadX },
            { Keycode.ButtonY, VirtualKey.GamepadY },
            { Keycode.C, VirtualKey.C },
            { Keycode.CapsLock, VirtualKey.CapitalLock },
            { Keycode.Clear, VirtualKey.Clear },
            { Keycode.CtrlLeft, VirtualKey.LeftControl },
            { Keycode.CtrlRight, VirtualKey.RightControl },
            { Keycode.D, VirtualKey.D },
            { Keycode.Del, VirtualKey.Back },
            { Keycode.DpadDown, VirtualKey.Down },
            { Keycode.DpadUp, VirtualKey.Up },
            { Keycode.DpadCenter, VirtualKey.Clear },
            { Keycode.DpadLeft, VirtualKey.Left },
            { Keycode.DpadRight, VirtualKey.Right },
            { Keycode.E, VirtualKey.E },
            { Keycode.Enter, VirtualKey.Enter },
            { Keycode.Escape, VirtualKey.Escape },
            { Keycode.F, VirtualKey.F },
            { Keycode.F1, VirtualKey.F1 },
            { Keycode.F2, VirtualKey.F2 },
            { Keycode.F3, VirtualKey.F3 },
            { Keycode.F4, VirtualKey.F4 },
            { Keycode.F5, VirtualKey.F5 },
            { Keycode.F6, VirtualKey.F6 },
            { Keycode.F7, VirtualKey.F7 },
            { Keycode.F8, VirtualKey.F8 },
            { Keycode.F9, VirtualKey.F9 },
            { Keycode.F10, VirtualKey.F10 },
            { Keycode.F11, VirtualKey.F11 },
            { Keycode.F12, VirtualKey.F12 },
            { Keycode.Forward, VirtualKey.GoForward },
            { Keycode.ForwardDel, VirtualKey.Delete },
            { Keycode.G, VirtualKey.G },
            { Keycode.H, VirtualKey.H },
            { Keycode.Help, VirtualKey.Help },
            { Keycode.Home, VirtualKey.Home },
            { Keycode.I, VirtualKey.I },
            { Keycode.Insert, VirtualKey.Insert },
            { Keycode.J, VirtualKey.J },
            { Keycode.K, VirtualKey.K },
            { Keycode.Kana, VirtualKey.Kana },
            { Keycode.L, VirtualKey.L },
            { Keycode.M, VirtualKey.M },
            { Keycode.Menu, VirtualKey.Menu },
            { Keycode.MetaLeft, VirtualKey.LeftWindows },
            { Keycode.MetaRight, VirtualKey.RightWindows },
            { Keycode.N, VirtualKey.N },
            { Keycode.NumLock, VirtualKey.NumberKeyLock },
            { Keycode.Numpad0, VirtualKey.NumberPad0 },
            { Keycode.Numpad1, VirtualKey.NumberPad1 },
            { Keycode.Numpad2, VirtualKey.NumberPad2 },
            { Keycode.Numpad3, VirtualKey.NumberPad3 },
            { Keycode.Numpad4, VirtualKey.NumberPad4 },
            { Keycode.Numpad5, VirtualKey.NumberPad5 },
            { Keycode.Numpad6, VirtualKey.NumberPad6 },
            { Keycode.Numpad7, VirtualKey.NumberPad7 },
            { Keycode.Numpad8, VirtualKey.NumberPad8 },
            { Keycode.Numpad9, VirtualKey.NumberPad9 },
            { Keycode.NumpadAdd, VirtualKey.Add },
            { Keycode.NumpadDivide, VirtualKey.Divide },
            { Keycode.NumpadDot, VirtualKey.Decimal },
            { Keycode.NumpadEnter, VirtualKey.Enter },
            { Keycode.NumpadMultiply, VirtualKey.Multiply },
            { Keycode.NumpadSubtract, VirtualKey.Subtract },
            { Keycode.O, VirtualKey.O },
            { Keycode.P, VirtualKey.P },
            { Keycode.PageDown, VirtualKey.PageDown },
            { Keycode.PageUp, VirtualKey.PageUp },
            { Keycode.Plus, VirtualKey.Add },
            { Keycode.Q, VirtualKey.Q },
            { Keycode.R, VirtualKey.R },
            { Keycode.S, VirtualKey.S },
            { Keycode.ScrollLock, VirtualKey.Scroll },
            { Keycode.Search, VirtualKey.Search },
            { Keycode.ShiftLeft, VirtualKey.LeftShift },
            { Keycode.ShiftRight, VirtualKey.RightShift },
            { Keycode.Sleep, VirtualKey.Sleep },
            { Keycode.SystemNavigationDown, VirtualKey.NavigationDown },
            { Keycode.SystemNavigationLeft, VirtualKey.NavigationLeft },
            { Keycode.SystemNavigationRight, VirtualKey.NavigationRight },
            { Keycode.SystemNavigationUp, VirtualKey.NavigationUp },
            { Keycode.T, VirtualKey.T },
            { Keycode.Tab, VirtualKey.Tab },
            { Keycode.U, VirtualKey.U },
            { Keycode.V, VirtualKey.V },
            { Keycode.W, VirtualKey.W },
            { Keycode.X, VirtualKey.X },
            { Keycode.Y, VirtualKey.Y },
            { Keycode.Z, VirtualKey.Z }
        };
#endif
    }
}
