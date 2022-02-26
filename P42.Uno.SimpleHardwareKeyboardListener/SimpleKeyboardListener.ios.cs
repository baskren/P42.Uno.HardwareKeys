#if __IOS__

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

namespace P42.Uno.SimpleHardwareKeyboardListener
{
    public partial class SimpleKeyboardListener : TextBox
    {
        bool _isCapsLockOn;
        bool _isNumLockOn;

        partial void PlatformBuild()
        {
            //System.Diagnostics.Debug.WriteLine($"CAPS: {System.Console.CapsLock}  NUM:{System.Console.NumberLock}");

            Background = new SolidColorBrush(Colors.Pink);
            _platformIsNumLockEngaged = PlatformIsNumLockOn;
            _platformIsCapsLockEngaged = PlatformIsCapsLockOn;
        }

        bool PlatformIsCapsLockOn()
            => _isCapsLockOn;

        bool PlatformIsNumLockOn()
            => _isNumLockOn;

        /*
        protected override void OnKeyDown(KeyRoutedEventArgs args)
        {
            base.OnKeyDown(args);
            System.Diagnostics.Debug.WriteLine($"DOWN: {args.Key} {args.OriginalKey}");
        }

        protected override void OnKeyUp(KeyRoutedEventArgs args)
        {
            base.OnKeyUp(args);
            System.Diagnostics.Debug.WriteLine($"UP: {args.Key} {args.OriginalKey}");
        }
        */

        private void OnShown(object sender, UIKeyboardEventArgs e)
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }

        static Dictionary<string, VirtualKey> UIKeyCommands = new Dictionary<string, VirtualKey>
        {
            {"\b", VirtualKey.Back },
            {"\0x7F", VirtualKey.Delete},
            {"\t", VirtualKey.Tab },
            {"\r", VirtualKey.Enter },
            {"UIKeyInputPageUp", VirtualKey.PageUp },
            {"UIKeyInputPageDown", VirtualKey.PageDown},
            {"UIKeyInputHome", VirtualKey.Home },
            {"UIKeyInputEnd", VirtualKey.End },
            {"UIKeyInputInsert", VirtualKey.Insert },
            {"UIKeyInputDelete", VirtualKey.Delete },
            {"UIKeyInputUpArrow", VirtualKey.Up },
            {"UIKeyInputDownArrow", VirtualKey.Down },
            {"UIKeyInputLeftArrow", VirtualKey.Left },
            {"UIKeyInputRightArrow", VirtualKey.Right },
            {"UIKeyInputEscape", VirtualKey.Escape },
            {"UIKeyInputF1", VirtualKey.F1 },
            {"UIKeyInputF2", VirtualKey.F2 },
            {"UIKeyInputF3", VirtualKey.F3 },
            {"UIKeyInputF4", VirtualKey.F4 },
            {"UIKeyInputF5", VirtualKey.F5 },
            {"UIKeyInputF6", VirtualKey.F6 },
            {"UIKeyInputF7", VirtualKey.F7 },
            {"UIKeyInputF8", VirtualKey.F8 },
            {"UIKeyInputF9", VirtualKey.F9 },
            {"UIKeyInputF10", VirtualKey.F10 },
            {"UIKeyInputF11", VirtualKey.F11 },
            {"UIKeyInputF12", VirtualKey.F12 },
        };

        static Dictionary<UIKeyboardHidUsage, VirtualKey> ModifierKeys = new Dictionary<UIKeyboardHidUsage, VirtualKey>
        {
            { UIKeyboardHidUsage.KeyboardDeleteForward, VirtualKey.Delete },
            { UIKeyboardHidUsage.KeypadNumLock, VirtualKey.NumberKeyLock },
            { UIKeyboardHidUsage.KeyboardCapsLock, VirtualKey.CapitalLock },

            { UIKeyboardHidUsage.KeyboardLeftControl, VirtualKey.LeftControl }, //224
            { UIKeyboardHidUsage.KeyboardLeftShift, VirtualKey.LeftShift }, // 225
            //{ UIKeyboardHidUsage.KeyboardLeftAlt, VirtualKey.??? }, // Mac Cmd 226
            { UIKeyboardHidUsage.KeyboardLeftGui, VirtualKey.LeftWindows }, // Mac Cmd 227

            { UIKeyboardHidUsage.KeyboardRightControl, VirtualKey.RightControl }, //228
            { UIKeyboardHidUsage.KeyboardRightShift, VirtualKey.RightShift }, // 229
            //{ UIKeyboardHidUsage.KeyboardRightAlt, VirtualKey.??? }, // Mac Right Cmd 230
            { UIKeyboardHidUsage.KeyboardRightGui, VirtualKey.RightWindows }, // Mac Right Opt

            /*
            { UIKeyboardHidUsage.KeyboardHangul, VirtualKey.Hangul },
            { UIKeyboardHidUsage.KeyboardHanja, VirtualKey.Hanja },
            { UIKeyboardHidUsage.KeyboardKanaSwitch, VirtualKey.Kana },
            */
        };

        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            base.PressesBegan(presses, evt);
            if (presses.ToArray().FirstOrDefault()?.Key is UIKey key)
                ParseKey(key, true);
        }

        void ParseKey(UIKey key, bool down)
        {
            System.Diagnostics.Debug.WriteLine($"Parse Key: {key} {key.Characters.Length} {key.ModifierFlags}");

            bool shift = (key.ModifierFlags & UIKeyModifierFlags.Shift) > 0;
            bool mod = (key.ModifierFlags & (UIKeyModifierFlags.Command | UIKeyModifierFlags.Control | UIKeyModifierFlags.Alternate)) > 0;
            var chars = mod ? key.CharactersIgnoringModifiers : key.Characters;

            if (!IsNumLockEngaged && (key.ModifierFlags & UIKeyModifierFlags.NumericPad) > 0)
            {
                switch (key.Characters)
                {
                    case "7": chars = "UIKeyInputHome"; break;
                    case "8": chars = "UIKeyInputUpArrow"; break;
                    case "9": chars = "UIKeyInputPageUp"; break;
                    case "4": chars = "UIKeyInputLeftArrow"; break;
                    //case "5": newChars = "UIKeyInputHome"; break;
                    case "6": chars = "UIKeyInputRightArrow"; break;
                    case "1": chars = "UIKeyInputEnd"; break;
                    case "2": chars = "UIKeyInputDownArrow"; break;
                    case "3": chars = "UIKeyInputPageDown"; break;
                    case "0": chars = "UIKeyInputInsert"; break;
                    case ".": chars = "UIKeyInputDelete"; break;
                }
            }

            if (UIKeyCommands.TryGetValue(chars, out var v1))
                //_textBlock.Text = $"A Press [{v1}] + [{AsString(key.ModifierFlags)}]";
                OnSimpleKeyDown(string.Empty, v1);
            else if (ModifierKeys.TryGetValue(key.KeyCode, out var v2))
            {
                if (v2 == VirtualKey.NumberKeyLock)
                    _isNumLockOn = !_isNumLockOn;
                if (v2 == VirtualKey.CapitalLock)
                    _isCapsLockOn = !_isCapsLockOn;
                if (!QuietModifiers)
                    //_textBlock.Text = $"B Press [{v2}]+[{AsString(key.ModifierFlags)}]";
                    OnSimpleKeyDown(string.Empty, v2)
            }
            else if (shift && UIKeyCommands.TryGetValue(key.CharactersIgnoringModifiers, out var v3))
                _textBlock.Text = $"A Press [{v3}] + [{AsString(key.ModifierFlags)}]";
            else
                _textBlock.Text = $"C Press [{chars}]+[{AsString(key.ModifierFlags)}]";
            
        }

        IEnumerable<UIKeyModifierFlags> mods = Enum.GetValues(typeof(UIKeyModifierFlags)).Cast<UIKeyModifierFlags>();
        public string AsString(UIKeyModifierFlags flags)
        {
            var result = new List<string>();
            foreach (var flag in mods)
                if ((flag & flags) > 0)
                    result.Add(flag.ToString());
            return string.Join(",", result);
        }

        /*
        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Console.WriteLine($"_textBox_TextChanged {_textBox.Text}");
            _textBox.Text = string.Empty;
        }
        */

    }
}

#endif