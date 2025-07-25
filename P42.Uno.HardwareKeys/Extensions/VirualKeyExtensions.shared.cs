using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;

#if __ANDROID__
using Android.Views;
#elif __IOS__
using UIKit;
#endif

namespace P42.Uno.HardwareKeys;

/// <summary>
/// Extensions for Windows.System.VirtualKey
/// </summary>
public static class VirualKeyExtensions
{
    /// <summary>
    /// Is the key [Shift], [Control], [Menu], or [Windows]?
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsNonToggleModifier(this VirtualKey key)
    {
        switch (key)
        {
            case VirtualKey.Shift:
            case VirtualKey.LeftShift:
            case VirtualKey.RightShift:
            case VirtualKey.Control:
            case VirtualKey.LeftControl:
            case VirtualKey.RightControl:
            case VirtualKey.Menu:
            case VirtualKey.LeftMenu:
            case VirtualKey.RightMenu:
            case VirtualKey.LeftWindows:
            case VirtualKey.RightWindows:
                return true;
        }
        return false;
    }

    /// <summary>
    /// Does a IEnumerable of VirtualKey have a Non-toggle modifier?
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static bool HasNonToggleModifier(this IEnumerable<VirtualKey> keys)
        => keys?.Any(k => k.IsNonToggleModifier()) ?? false;

    /// <summary>
    /// Simplifies VirualKey to a more general interpretation
    /// </summary>
    /// <param name="key">VirtualKey</param>
    /// <returns></returns>
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

    /// <summary>
    /// Eguality test
    /// </summary>
    /// <param name="source">First list of VirtualKeys</param>
    /// <param name="other">Second list of VirtualKeys</param>
    /// <param name="ignore">VirutalKeys to ignore</param>
    /// <returns></returns>
    public static bool Equal(this IEnumerable<VirtualKey> source, IEnumerable<VirtualKey> other, IEnumerable<VirtualKey> ignore = null)
    {

        var sList = source?.ToList() ?? new List<VirtualKey>();
        var oList = other?.ToList() ?? new List<VirtualKey>();

        if (ignore != null)
        {
            foreach (var key in ignore)
            {
                sList.TryRemove(key);
                oList.TryRemove(key);
            }
        }

        if (sList.Empty() != oList.Empty())
            return false;

        if (sList.Count != oList.Count)
            return false;

        for (int i = 0; i < sList.Count; i++)
        {
            if (!oList.Contains(sList[i]))
                return false;
        }

        return true;

    }

    static bool Empty(this IEnumerable<VirtualKey> list)
    {
        if (list is null)
            return true;

        return !list.Any();
    }

    static bool TryRemove(this ICollection<VirtualKey> list, VirtualKey key)
    {
        if (list.Contains(key))
        {
            list.Remove(key);
            return true;
        }

        return false;
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
        //{ Keycode.K11, VirtualKey. },
        //{ Keycode.K12, VirtualKey. },
        { Keycode.Num2, VirtualKey.Number2 },
        { Keycode.Num3, VirtualKey.Number3 },
        //{ Keycode.ThreeDMode, VirtualKey },
        { Keycode.Num4, VirtualKey.Number4 },
        { Keycode.Num5, VirtualKey.Number5 },
        { Keycode.Num6, VirtualKey.Number6 },
        { Keycode.Num7, VirtualKey.Number7 },
        { Keycode.Num8, VirtualKey.Number8 },
        { Keycode.Num9, VirtualKey.Number9 },
        { Keycode.A, VirtualKey.A },
        //{ Keycode.AllApps, VirtualKey. },
        { Keycode.AltLeft, VirtualKey.LeftMenu },
        { Keycode.AltRight, VirtualKey.RightMenu },
        //{ Keycode.Apostrophe, VirtualKey. },
        { Keycode.AppSwitch, VirtualKey.Application },
        //{ Keycode.Assist, VirtualKey },
        //{ Keycode.At, VirtualKey. },
        //{ Keycode.AvrInput, VirtualKey. },
        //{ Keycode.AvrPower, VirtualKey. },
        { Keycode.B, VirtualKey.B },
        { Keycode.Back, VirtualKey.Back },
        //{ Keycode.Backslash, VirtualKey. },
        //{ Keycode.Bookmark, VirtualKey. },
        //{ Keycode.Break, VirtualKey. },
        //{ Keycode.BrightnessDown, VirtualKey. },
        //{ Keycode.BrightnessUp, VirtualKey. },
        //{ Keycode.Button1, VirtualKey. },
        //{ Keycode.Button10, VirtualKey. },
        //{ Keycode.Button11, VirtualKey. },
        //{ Keycode.Button12, VirtualKey. },
        //{ Keycode.Button13, VirtualKey. },
        //{ Keycode.Button14, VirtualKey. },
        //{ Keycode.Button15, VirtualKey. },
        //{ Keycode.Button16, VirtualKey. },
        //{ Keycode.Button2, VirtualKey. },
        //{ Keycode.Button3, VirtualKey. },
        //{ Keycode.Button4, VirtualKey. },
        //{ Keycode.Button5, VirtualKey. },
        //{ Keycode.Button6, VirtualKey. },
        //{ Keycode.Button7, VirtualKey. },
        //{ Keycode.Button8, VirtualKey. },
        //{ Keycode.Button9, VirtualKey. },
        //{ Keycode.ButtonA, VirtualKey. },
        //{ Keycode.ButtonB, VirtualKey. },
        //{ Keycode.ButtonC, VirtualKey. },
        //{ Keycode.L1, VirtualKey. },
        //{ Keycode.L2, VirtualKey. },
        { Keycode.ButtonMode, VirtualKey.ModeChange },
        //{ Keycode.R1, VirtualKey. },
        //{ Keycode.R2, VirtualKey. },
        //{ Keycode.Select, VirtualKey. },
        //{ Keycode.ButtonStart, VirtualKey. },
        //{ Keycode.ButtonThumbl, VirtualKey. },
        //{ Keycode.ButtonThumbr, VirtualKey. },
        //{ Keycode.ButtonX, VirtualKey.GamepadX },
        //{ Keycode.ButtonY, VirtualKey.GamepadY },
        //{ Keycode.ButtonZ, VirtualKey. },
        { Keycode.C, VirtualKey.C },
        //{ Keycode.Calculator, VirtualKey. },
        //{ Keycode.Calendar, VirtualKey. },
        //{ Keycode.Call, VirtualKey. },
        //{ Keycode.Camera, VirtualKey. },
        { Keycode.CapsLock, VirtualKey.CapitalLock },
        //{ Keycode.Captions, VirtualKey. },
        //{ Keycode.ChannelDown, VirtualKey. },
        //{ Keycode.ChannelUp, VirtualKey. },
        { Keycode.Clear, VirtualKey.Clear },
        //{ Keycode.Comma, VirtualKey. },
        //{ Keycode.Contacts, VirtualKey. },
        //{ Keycode.Copy, VirtualKey. },
        { Keycode.CtrlLeft, VirtualKey.LeftControl },
        { Keycode.CtrlRight, VirtualKey.RightControl },
        //{ Keycode.Cut, VirtualKey. },
        { Keycode.D, VirtualKey.D },
        { Keycode.Del, VirtualKey.Back },
        { Keycode.DpadCenter, VirtualKey.Clear },
        { Keycode.DpadDown, VirtualKey.Down },
        { Keycode.DpadDownLeft, VirtualKey.End },
        { Keycode.DpadDownRight, VirtualKey.PageDown },
        { Keycode.DpadLeft, VirtualKey.Left },
        { Keycode.DpadRight, VirtualKey.Right },
        { Keycode.DpadUp, VirtualKey.Up },
        { Keycode.DpadUpLeft, VirtualKey.Home },
        { Keycode.DpadUpRight, VirtualKey.PageUp },
        //{ Keycode.Dvr, VirtualKey. },
        { Keycode.E, VirtualKey.E },
        //{ Keycode.Eisu, VirtualKey. },
        //{ Keycode.Endcall, VirtualKey. },
        { Keycode.Enter, VirtualKey.Enter },
        //{ Keycode.Envelope, VirtualKey. },
        //{ Keycode.Equals, VirtualKey. },
        { Keycode.Escape, VirtualKey.Escape },
        //{ Keycode.Explorer, VirtualKey. },
        { Keycode.F, VirtualKey.F },
        { Keycode.F1, VirtualKey.F1 },
        { Keycode.F10, VirtualKey.F10 },
        { Keycode.F11, VirtualKey.F11 },
        { Keycode.F12, VirtualKey.F12 },
        { Keycode.F2, VirtualKey.F2 },
        { Keycode.F3, VirtualKey.F3 },
        { Keycode.F4, VirtualKey.F4 },
        { Keycode.F5, VirtualKey.F5 },
        { Keycode.F6, VirtualKey.F6 },
        { Keycode.F7, VirtualKey.F7 },
        { Keycode.F8, VirtualKey.F8 },
        { Keycode.F9, VirtualKey.F9 },
        //{ Keycode.Focus, VirtualKey. },
        { Keycode.Forward, VirtualKey.GoForward },
        { Keycode.ForwardDel, VirtualKey.Delete },
        //{ Keycode.Function, VirtualKey },
        { Keycode.G, VirtualKey.G },
        //{ Keycode.Grave, VirtualKey. },
        //{ Keycode.Guide, VirtualKey. },
        { Keycode.H, VirtualKey.H },
        //{ Keycode.Headsethook, VirtualKey. },
        { Keycode.Help, VirtualKey.Help },
        //{ Keycode.Henkan, VirtualKey. },
        { Keycode.Home, VirtualKey.Home },
        { Keycode.I, VirtualKey.I },
        //{ Keycode.Info, VirtualKey. },
        { Keycode.Insert, VirtualKey.Insert },
        { Keycode.J, VirtualKey.J },
        { Keycode.K, VirtualKey.K },
        { Keycode.Kana, VirtualKey.Kana },
        //{ Keycode.KatakanaHiragana, VirtualKey. },
        { Keycode.L, VirtualKey.L },
        //{ Keycode.LanguageSwitch, VirtualKey. },
        //{ Keycode.LastChannel, VirtualKey. },
        //{ Keycode.LeftBracket, VirtualKey. },
        { Keycode.M, VirtualKey.M },
        //{ Keycode.MediaAudioTrack, VirtualKey. },
        //{ Keycode.MediaClose, VirtualKey. },
        //{ Keycode.MediaEject, VirtualKey. },
        //{ Keycode.MediaFastForward, VirtualKey. },
        //{ Keycode.MediaNext, VirtualKey. },
        //{ Keycode.MediaPause, VirtualKey. },
        //{ Keycode.MediaPlay, VirtualKey. },
        //{ Keycode.MediaPlayPause, VirtualKey. },
        //{ Keycode.MediaPrevious, VirtualKey. },
        //{ Keycode.MediaRecord, VirtualKey. },
        //{ Keycode.MediaRewind, VirtualKey. },
        //{ Keycode.MediaSkipBackward, VirtualKey. },
        //{ Keycode.MediaSkipForward, VirtualKey. },
        //{ Keycode.MediaStepBackward, VirtualKey. },
        //{ Keycode.MediaStepForward, VirtualKey. },
        //{ Keycode.MediaStop, VirtualKey. },
        //{ Keycode.MediaTopMenu, VirtualKey. },
        { Keycode.Menu, VirtualKey.Menu },
        { Keycode.MetaLeft, VirtualKey.LeftWindows },
        { Keycode.MetaRight, VirtualKey.RightWindows },
        { Keycode.Minus, VirtualKey.Subtract },
        { Keycode.MoveEnd, VirtualKey.End },
        { Keycode.MoveHome, VirtualKey.Home },
        //{ Keycode.Muhenkan, VirtualKey. },
        //{ Keycode.Music, VirtualKey. },
        //{ Keycode.Mute, VirtualKey. },
        { Keycode.N, VirtualKey.N },
        //{ Keycode.NavigateIn, VirtualKey. },
        //{ Keycode.NavigateNext, VirtualKey. },
        //{ Keycode.NavigateOut, VirtualKey. },
        //{ Keycode.NavigatePrevious, VirtualKey. },
        //{ Keycode.Notification, VirtualKey. },
        //{ Keycode.Num, VirtualKey. },
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
        //{ Keycode.NumpadComma, VirtualKey. },
        { Keycode.NumpadDivide, VirtualKey.Divide },
        { Keycode.NumpadDot, VirtualKey.Decimal },
        { Keycode.NumpadEnter, VirtualKey.Enter },
        //{ Keycode.Equals, VirtualKey. },
        //{ Keycode.NumpadLeftParen, VirtualKey. },
        { Keycode.NumpadMultiply, VirtualKey.Multiply },
        //{ Keycode.NumpadRightParen, VirtualKey. },
        { Keycode.NumpadSubtract, VirtualKey.Subtract },
        { Keycode.O, VirtualKey.O },
        { Keycode.P, VirtualKey.P },
        { Keycode.PageDown, VirtualKey.PageDown },
        { Keycode.PageUp, VirtualKey.PageUp },
        //{ Keycode.Pairing, VirtualKey. },
        //{ Keycode.Paste, VirtualKey. },
        { Keycode.Period, VirtualKey.Decimal },
        //{ Keycode.Pictsymbols, VirtualKey. },
        { Keycode.Plus, VirtualKey.Add },
        //{ Keycode.Pound, VirtualKey. },
        //{ Keycode.Power, VirtualKey. },
        //{ Keycode.ProfileSwitch, VirtualKey. },
        //{ Keycode.ProgBlue, VirtualKey. },
        //{ Keycode.ProgGreen, VirtualKey. },
        //{ Keycode.ProgRed, VirtualKey. },
        //{ Keycode.ProgYellow, VirtualKey. },
        { Keycode.Q, VirtualKey.Q },
        { Keycode.R, VirtualKey.R },
        //{ Keycode.Refresh, VirtualKey. },
        //{ Keycode.RightBracket, VirtualKey. },
        //{ Keycode.Ro, VirtualKey. },
        { Keycode.S, VirtualKey.S },
        { Keycode.ScrollLock, VirtualKey.Scroll },
        { Keycode.Search, VirtualKey.Search },
        //{ Keycode.Semicolon, VirtualKey. },
        //{ Keycode.Settings, VirtualKey. },
        { Keycode.ShiftLeft, VirtualKey.LeftShift },
        { Keycode.ShiftRight, VirtualKey.RightShift },
        //{ Keycode.Slash, VirtualKey. },
        { Keycode.Sleep, VirtualKey.Sleep },
        { Keycode.SoftLeft, VirtualKey.Left },
        { Keycode.SoftRight, VirtualKey.Right },
        { Keycode.SoftSleep, VirtualKey.Sleep },
        { Keycode.Space, VirtualKey.Space },
        { Keycode.Star, VirtualKey.Multiply },
        //{ Keycode.StbInput, VirtualKey. },
        //{ Keycode.StbPower, VirtualKey. },
        //{ Keycode.Stem1, VirtualKey. },
        //{ Keycode.Stem2, VirtualKey. },
        //{ Keycode.Stem3, VirtualKey. },
        //{ Keycode.StemPrimary, VirtualKey. },
        //{ Keycode.SwitchCharset, VirtualKey. },
        //{ Keycode.Sym, VirtualKey. },
        //{ Keycode.Sysrq, VirtualKey. },
        { Keycode.SystemNavigationDown, VirtualKey.NavigationDown },
        { Keycode.SystemNavigationLeft, VirtualKey.NavigationLeft },
        { Keycode.SystemNavigationRight, VirtualKey.NavigationRight },
        { Keycode.SystemNavigationUp, VirtualKey.NavigationUp },
        { Keycode.T, VirtualKey.T },
        { Keycode.Tab, VirtualKey.Tab },
        { Keycode.ThumbsDown, VirtualKey.Divide },
        //{ Keycode.ThumbsUp, VirtualKey. },
        //{ Keycode.Tv, VirtualKey. },
        //{ Keycode.TvAntennaCable, VirtualKey. },
        //{ Keycode.TvAudioDescription, VirtualKey. },
        //{ Keycode.TvAudioDescriptionMixDown, VirtualKey. },
        //{ Keycode.TvAudioDescriptionMixUp, VirtualKey. },
        //{ Keycode.TvContentsMenu, VirtualKey. },
        //{ Keycode.TvDataService, VirtualKey. },
        //{ Keycode.TvInput, VirtualKey. },
        //{ Keycode.TvInputComponent1, VirtualKey. },
        //{ Keycode.TvInputComponent2, VirtualKey. },
        //{ Keycode.TvInputComposite1, VirtualKey. },
        //{ Keycode.TvInputComposite2, VirtualKey. },
        //{ Keycode.TvInputHdmi1, VirtualKey. },
        //{ Keycode.TvInputHdmi2, VirtualKey. },
        //{ Keycode.TvInputHdmi3, VirtualKey. },
        //{ Keycode.TvInputHdmi4, VirtualKey. },
        //{ Keycode.TvInputVga1, VirtualKey. },
        //{ Keycode.TvMediaContextMenu, VirtualKey. },
        //{ Keycode.TvNetwork, VirtualKey. },
        //{ Keycode.TvNumberEntry, VirtualKey. },
        //{ Keycode.TvPower, VirtualKey. },
        //{ Keycode.TvRadioService, VirtualKey. },
        //{ Keycode.TvSatellite, VirtualKey. },
        //{ Keycode.TvSatelliteBs, VirtualKey. },
        //{ Keycode.TvSatelliteCs, VirtualKey. },
        //{ Keycode.TvSatelliteService, VirtualKey. },
        //{ Keycode.TvTeletext, VirtualKey. },
        //{ Keycode.TvTerrestrialAnalog, VirtualKey. },
        //{ Keycode.TvTerrestrialDigital, VirtualKey. },
        //{ Keycode.TvTimerProgramming, VirtualKey. },
        //{ Keycode.TvZoomMode, VirtualKey. },
        { Keycode.U, VirtualKey.U },
        //{ Keycode.Unknown, VirtualKey. },
        { Keycode.V, VirtualKey.V },
        //{ Keycode.VoiceAssist, VirtualKey. },
        //{ Keycode.VolumeDown, VirtualKey. },
        //{ Keycode.VolumeMute, VirtualKey. },
        //{ Keycode.VolumeUp, VirtualKey. },
        { Keycode.W, VirtualKey.W },
        //{ Keycode.Wakeup, VirtualKey. },
        //{ Keycode.Window, VirtualKey. },
        { Keycode.X, VirtualKey.X },
        { Keycode.Y, VirtualKey.Y },
        //{ Keycode.Yen, VirtualKey. },
        { Keycode.Z, VirtualKey.Z }
        //{ Keycode.ZenkakuHankaku, VirtualKey. },
        //{ Keycode.ZoomIn, VirtualKey. },
        //{ Keycode.ZoomOut, VirtualKey. },
    };

    internal static VirtualKey[] GetModifiers(this Android.Views.KeyEvent e)
    {
        var result = new List<VirtualKey>();
        if (e.IsCtrlPressed)
            result.Add(VirtualKey.Control);
        if (e.IsShiftPressed)
            result.Add(VirtualKey.Shift);
        if (e.IsAltPressed)
            result.Add(VirtualKey.Menu);
        if (e.IsMetaPressed)
            result.Add(VirtualKey.Menu);
        if (e.IsCapsLockOn)
            result.Add(VirtualKey.CapitalLock);
        if (e.IsNumLockOn)
            result.Add(VirtualKey.NumberKeyLock);
        //if (e.IsFunctionPressed)
        //    result.Add(VirtualKey.);
        if (e.IsScrollLockOn)
            result.Add(VirtualKey.Scroll);
        
        return result.ToArray();
    }
    
#elif __IOS__
        internal static VirtualKey AsVirtualKey(this UIKeyboardHidUsage keycode)
        {
            if (KeyCodeMap.TryGetValue(keycode, out VirtualKey virtualKey))
                return virtualKey;
            return VirtualKey.None;
        }


        readonly static Dictionary<UIKeyboardHidUsage, VirtualKey> KeyCodeMap = new Dictionary<UIKeyboardHidUsage, VirtualKey>
        {
            { UIKeyboardHidUsage.KeyboardA, VirtualKey.A },
            { UIKeyboardHidUsage.KeyboardB, VirtualKey.B },
            { UIKeyboardHidUsage.KeyboardC, VirtualKey.C },
            { UIKeyboardHidUsage.KeyboardD, VirtualKey.D },
            { UIKeyboardHidUsage.KeyboardE, VirtualKey.E },
            { UIKeyboardHidUsage.KeyboardF, VirtualKey.F },
            { UIKeyboardHidUsage.KeyboardG, VirtualKey.G },
            { UIKeyboardHidUsage.KeyboardH, VirtualKey.H },
            { UIKeyboardHidUsage.KeyboardI, VirtualKey.I },
            { UIKeyboardHidUsage.KeyboardJ, VirtualKey.J },
            { UIKeyboardHidUsage.KeyboardK, VirtualKey.K },
            { UIKeyboardHidUsage.KeyboardL, VirtualKey.L },
            { UIKeyboardHidUsage.KeyboardM, VirtualKey.M },
            { UIKeyboardHidUsage.KeyboardN, VirtualKey.N },
            { UIKeyboardHidUsage.KeyboardO, VirtualKey.O },
            { UIKeyboardHidUsage.KeyboardP, VirtualKey.P },
            { UIKeyboardHidUsage.KeyboardQ, VirtualKey.Q },
            { UIKeyboardHidUsage.KeyboardR, VirtualKey.R },
            { UIKeyboardHidUsage.KeyboardS, VirtualKey.S },
            { UIKeyboardHidUsage.KeyboardT, VirtualKey.T },
            { UIKeyboardHidUsage.KeyboardU, VirtualKey.U },
            { UIKeyboardHidUsage.KeyboardV, VirtualKey.V },
            { UIKeyboardHidUsage.KeyboardW, VirtualKey.W },
            { UIKeyboardHidUsage.KeyboardX, VirtualKey.X },
            { UIKeyboardHidUsage.KeyboardY, VirtualKey.Y },
            { UIKeyboardHidUsage.KeyboardZ, VirtualKey.Z },
            { UIKeyboardHidUsage.Keyboard1, VirtualKey.Number1 },
            { UIKeyboardHidUsage.Keyboard2, VirtualKey.Number2 },
            { UIKeyboardHidUsage.Keyboard3, VirtualKey.Number3 },
            { UIKeyboardHidUsage.Keyboard4, VirtualKey.Number4 },
            { UIKeyboardHidUsage.Keyboard5, VirtualKey.Number5 },
            { UIKeyboardHidUsage.Keyboard6, VirtualKey.Number6 },
            { UIKeyboardHidUsage.Keyboard7, VirtualKey.Number7 },
            { UIKeyboardHidUsage.Keyboard8, VirtualKey.Number8 },
            { UIKeyboardHidUsage.Keyboard9, VirtualKey.Number9 },
            { UIKeyboardHidUsage.Keyboard0, VirtualKey.Number0 },
            { UIKeyboardHidUsage.KeyboardReturnOrEnter, VirtualKey.Enter },
            { UIKeyboardHidUsage.KeyboardEscape, VirtualKey.Escape },
            { UIKeyboardHidUsage.KeyboardDeleteOrBackspace, VirtualKey.Back },
            { UIKeyboardHidUsage.KeyboardTab, VirtualKey.Tab },
            { UIKeyboardHidUsage.KeyboardSpacebar, VirtualKey.Space },
            { UIKeyboardHidUsage.KeyboardHyphen, VirtualKey.Subtract },
            //{ UIKeyboardHidUsage.KeyboardEqualSign, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardOpenBracket, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardCloseBracket, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardBackslash, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardNonUSPound, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardSemicolon, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardQuote, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardGraveAccentAndTilde, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardComma, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardPeriod, VirtualKey. }, // Is this .Decimal ???????  <<<<<<<<
            //{ UIKeyboardHidUsage.KeyboardSlash, VirtualKey. },
            { UIKeyboardHidUsage.KeyboardCapsLock, VirtualKey.CapitalLock },
            { UIKeyboardHidUsage.KeyboardF1, VirtualKey.F1 },
            { UIKeyboardHidUsage.KeyboardF2, VirtualKey.F2 },
            { UIKeyboardHidUsage.KeyboardF3, VirtualKey.F3 },
            { UIKeyboardHidUsage.KeyboardF4, VirtualKey.F4 },
            { UIKeyboardHidUsage.KeyboardF5, VirtualKey.F5 },
            { UIKeyboardHidUsage.KeyboardF6, VirtualKey.F6 },
            { UIKeyboardHidUsage.KeyboardF7, VirtualKey.F7 },
            { UIKeyboardHidUsage.KeyboardF8, VirtualKey.F8 },
            { UIKeyboardHidUsage.KeyboardF9, VirtualKey.F9 },
            { UIKeyboardHidUsage.KeyboardF10, VirtualKey.F10 },
            { UIKeyboardHidUsage.KeyboardF11, VirtualKey.F11 },
            { UIKeyboardHidUsage.KeyboardF12, VirtualKey.F12 },
            { UIKeyboardHidUsage.KeyboardPrintScreen, VirtualKey.Print },
            { UIKeyboardHidUsage.KeyboardScrollLock, VirtualKey.Scroll },
            { UIKeyboardHidUsage.KeyboardPause, VirtualKey.Pause },
            { UIKeyboardHidUsage.KeyboardInsert, VirtualKey.Insert },
            { UIKeyboardHidUsage.KeyboardHome, VirtualKey.Home },
            { UIKeyboardHidUsage.KeyboardPageUp, VirtualKey.PageUp },
            { UIKeyboardHidUsage.KeyboardDeleteForward, VirtualKey.Delete },
            { UIKeyboardHidUsage.KeyboardEnd, VirtualKey.End },
            { UIKeyboardHidUsage.KeyboardPageDown, VirtualKey.PageDown },
            { UIKeyboardHidUsage.KeyboardRightArrow, VirtualKey.Right },
            { UIKeyboardHidUsage.KeyboardLeftArrow, VirtualKey.Left },
            { UIKeyboardHidUsage.KeyboardDownArrow, VirtualKey.Down },
            { UIKeyboardHidUsage.KeyboardUpArrow, VirtualKey.Up },
            { UIKeyboardHidUsage.KeypadNumLock, VirtualKey.NumberKeyLock },
            { UIKeyboardHidUsage.KeypadSlash, VirtualKey.Divide },
            { UIKeyboardHidUsage.KeypadAsterisk, VirtualKey.Multiply },
            { UIKeyboardHidUsage.KeypadHyphen, VirtualKey.Subtract },
            { UIKeyboardHidUsage.KeypadPlus, VirtualKey.Add },
            { UIKeyboardHidUsage.KeypadEnter, VirtualKey.Enter },
            { UIKeyboardHidUsage.Keypad1, VirtualKey.NumberPad1 },
            { UIKeyboardHidUsage.Keypad2, VirtualKey.NumberPad2 },
            { UIKeyboardHidUsage.Keypad3, VirtualKey.NumberPad3 },
            { UIKeyboardHidUsage.Keypad4, VirtualKey.NumberPad4 },
            { UIKeyboardHidUsage.Keypad5, VirtualKey.NumberPad5 },
            { UIKeyboardHidUsage.Keypad6, VirtualKey.NumberPad6 },
            { UIKeyboardHidUsage.Keypad7, VirtualKey.NumberPad7 },
            { UIKeyboardHidUsage.Keypad8, VirtualKey.NumberPad8 },
            { UIKeyboardHidUsage.Keypad9, VirtualKey.NumberPad9 },
            { UIKeyboardHidUsage.Keypad0, VirtualKey.NumberPad0 },
            { UIKeyboardHidUsage.KeypadPeriod, VirtualKey.Decimal },
            //{ UIKeyboardHidUsage.KeyboardNonUSBackslash, VirtualKey. },
            { UIKeyboardHidUsage.KeyboardApplication, VirtualKey.Application },
            //{ UIKeyboardHidUsage.KeyboardPower, VirtualKey. },
            { UIKeyboardHidUsage.KeypadEqualSign, VirtualKey.Enter },
            //{ UIKeyboardHidUsage.KeyboardF13, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF14, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF15, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF16, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF17, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF18, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF19, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF20, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF21, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF22, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF23, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardF24, VirtualKey. },
            { UIKeyboardHidUsage.KeyboardExecute, VirtualKey.Execute },
            { UIKeyboardHidUsage.KeyboardHelp, VirtualKey.Help },
            { UIKeyboardHidUsage.KeyboardMenu, VirtualKey.Menu },
            { UIKeyboardHidUsage.KeyboardSelect, VirtualKey.Select },
            { UIKeyboardHidUsage.KeyboardStop, VirtualKey.Stop },
            //{ UIKeyboardHidUsage.KeyboardAgain, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardUndo, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardCut, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardCopy, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardPaste, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardFind, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardMute, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardVolumeUp, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardVolumeDown, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLockingCapsLock, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLockingNumLock, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLockingScrollLock, VirtualKey. },
            //{ UIKeyboardHidUsage.KeypadComma, VirtualKey. },
            //{ UIKeyboardHidUsage.KeypadEqualSignAS400, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational1, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational2, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational3, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational4, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational5, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational6, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational7, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational8, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardInternational9, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang1, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang2, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang3, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang4, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang5, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang6, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang7, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang8, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardLang9, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardAlternateErase, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardSysReqOrAttention, VirtualKey. },
            { UIKeyboardHidUsage.KeyboardCancel, VirtualKey.Cancel },
            { UIKeyboardHidUsage.KeyboardClear, VirtualKey.Clear },
            //{ UIKeyboardHidUsage.KeyboardPrior, VirtualKey. },
            { UIKeyboardHidUsage.KeyboardReturn, VirtualKey.Enter },
            { UIKeyboardHidUsage.KeyboardSeparator, VirtualKey.Separator },
            //{ UIKeyboardHidUsage.KeyboardOut, VirtualKey },
            //{ UIKeyboardHidUsage.KeyboardOper, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardClearOrAgain, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardCrSelOrProps, VirtualKey. },
            //{ UIKeyboardHidUsage.KeyboardExSel, VirtualKey. },
            { UIKeyboardHidUsage.KeyboardLeftControl, VirtualKey.LeftControl },
            { UIKeyboardHidUsage.KeyboardLeftShift, VirtualKey.LeftShift },
            { UIKeyboardHidUsage.KeyboardLeftAlt, VirtualKey.LeftWindows },
            { UIKeyboardHidUsage.KeyboardLeftGui, VirtualKey.LeftMenu },
            { UIKeyboardHidUsage.KeyboardRightControl, VirtualKey.RightControl },
            { UIKeyboardHidUsage.KeyboardRightShift, VirtualKey.RightShift },
            { UIKeyboardHidUsage.KeyboardRightAlt, VirtualKey.RightWindows },
            { UIKeyboardHidUsage.KeyboardRightGui, VirtualKey.RightMenu },
            //{ UIKeyboardHidUsage.KeyboardReserved, VirtualKey },
            //{ UIKeyboardHidUsage.KeyboardHangul, VirtualKey.Hangul }, // ??? Exception thrown?
            { UIKeyboardHidUsage.KeyboardHanja, VirtualKey.Hanja },
            { UIKeyboardHidUsage.KeyboardKanaSwitch, VirtualKey.Kana },
            //{ UIKeyboardHidUsage.KeyboardAlphanumericSwitch, VirtualKey },
            //{ UIKeyboardHidUsage.KeyboardKatakana, VirtualKey },
            //{ UIKeyboardHidUsage.KeyboardHiragana, VirtualKey },
            //{ UIKeyboardHidUsage.KeyboardZenkakuHankakuKanji, VirtualKey },
        };
#endif

    internal static VirtualKey AsVirtualKey(this string  key)
    {
        if (KeyMap.TryGetValue(key.ToUpper(), out var mappedKey))
            return mappedKey;
        return VirtualKey.None;
    }

    readonly static Dictionary<string, VirtualKey> KeyMap = new Dictionary<string, VirtualKey>
    {
        { "A", VirtualKey.A },
        { "B", VirtualKey.B },
        { "C", VirtualKey.C },
        { "D", VirtualKey.D },
        { "E", VirtualKey.E },
        { "F", VirtualKey.F },
        { "G", VirtualKey.G },
        { "H", VirtualKey.H },
        { "I", VirtualKey.I },
        { "J", VirtualKey.J },
        { "K", VirtualKey.K },
        { "L", VirtualKey.L },
        { "M", VirtualKey.M },
        { "N", VirtualKey.N },
        { "O", VirtualKey.O },
        { "P", VirtualKey.P },
        { "Q", VirtualKey.Q },
        { "R", VirtualKey.R },
        { "S", VirtualKey.S },
        { "T", VirtualKey.T },
        { "U", VirtualKey.U },
        { "V", VirtualKey.V },
        { "W", VirtualKey.W },
        { "X", VirtualKey.X },
        { "Y", VirtualKey.Y },
        { "Z", VirtualKey.Z },
        { "1", VirtualKey.Number1 },
        { "2", VirtualKey.Number2 },
        { "3", VirtualKey.Number3 },
        { "4", VirtualKey.Number4 },
        { "5", VirtualKey.Number5 },
        { "6", VirtualKey.Number6 },
        { "7", VirtualKey.Number7 },
        { "8", VirtualKey.Number8 },
        { "9", VirtualKey.Number9 },
        { "0", VirtualKey.Number0 },
        { "\n", VirtualKey.Enter },
        { "\t", VirtualKey.Tab },
        { " ", VirtualKey.Space },
        { "-", VirtualKey.Subtract },
        { ".", VirtualKey.Decimal }, // Is this .Decimal ???????  <<<<<<<<
        { "/", VirtualKey.Divide },
        { "*", VirtualKey.Multiply },
        { "+", VirtualKey.Add },
 
    };
}
