using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace P42.Uno.HardwareKeys.Demo
{
    public partial class Keys : Button
    {

        bool _quietModifiers = true;

        TextBox _textBox;

        public event EventHandler<string> SimpleKeyDown;
        public event EventHandler<string> SimpleKeyUp;

        public Keys()
        {
            Content = _textBox = new TextBox();
            _textBox.TextChanged += _textBox_TextChanged;
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            //FocusManager.TryFocusAsync(_textBox, FocusState.Programmatic);
        }


        Dictionary<VirtualKey, string> FatFingerBuffer = new Dictionary<VirtualKey, string>();
        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_waitingForText && !string.IsNullOrEmpty(_textBox.Text))
            {
                var key = _textBox.Text.Substring(_textBox.Text.Length-1);
                FatFingerBuffer[_lastKeyDown] = key;
                System.Diagnostics.Debug.WriteLine($"Text: [{_textBox.Text}] [{_lastKeyDown}]");
                SimpleKeyDown?.Invoke(this, $"[{key}] [{_lastKeyDown}]");
            }
        }

        VirtualKey _lastKeyDown;
        bool _waitingForText;
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            _lastKeyDown = e.Key;
            if (IsCharacterKey(e.Key))
            {
                _waitingForText = true;
                System.Diagnostics.Debug.Write(" ... ");
            }
            else
            {
                e.Handled = true;

                if (!_quietModifiers || !IsModifier(e.Key))
                {
                    System.Diagnostics.Debug.WriteLine($"OnKeyDown: [{e.Key}]");
                    SimpleKeyDown?.Invoke(this, $"[{e.Key}]");
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            base.OnKeyUp(e);
            if (!_quietModifiers || !IsModifier(e.Key))
            {
                System.Diagnostics.Debug.WriteLine($"OnKeyUp: [{_textBox.Text}] [{e.Key}]");
                var key = _textBox.Text;
                if (string.IsNullOrEmpty(key))
                {
                    if (FatFingerBuffer.TryGetValue(e.Key, out var txt))
                        key = txt;
                }
                else
                {
                    key = key.Substring(key.Length - 1);
                }

                SimpleKeyUp?.Invoke(this, string.IsNullOrEmpty(key) ? $"[{e.Key}]" : $"[{key}] [{e.Key}]");
            }
            _textBox.Text = String.Empty;
        }

            readonly static VirtualKey[] _modifierKeyBoundaries = new VirtualKey[] 
            { 
                VirtualKey.None, 
                VirtualKey.XButton2,
                VirtualKey.Enter,
                VirtualKey.Menu,
                VirtualKey.Pause,
                VirtualKey.Kanji,
                VirtualKey.Z,
                VirtualKey.RightWindows,
                VirtualKey.Scroll,
                VirtualKey.GoBack
        };

        bool IsModifier(VirtualKey key)
            => IsInGroup(key, _modifierKeyBoundaries, false);
        

        readonly static VirtualKey[] _charKeyBoundaries = new VirtualKey[]
        {
                VirtualKey.None,
                VirtualKey.Help,
                VirtualKey.Z,
                VirtualKey.Sleep,
                VirtualKey.Divide
        };

        bool IsNumpadNumKey(VirtualKey key)
        {
            if (key < VirtualKey.NumberPad0)
                return false;
            if (key < VirtualKey.Multiply)
                return true;
            return false;
        }

        bool IsCharacterKey(VirtualKey key)
        {
            if (IsNumpadNumKey(key))
                return (Windows.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.NumberKeyLock) & Windows.UI.Core.CoreVirtualKeyStates.Locked) != 0;

            return IsInGroup(key, _charKeyBoundaries, true);
        }

#if HAS_UNO_SKIA_WPF 
        static readonly List<VirtualKey> knownKeys = Enum.GetValues(typeof(VirtualKey)).Cast<VirtualKey>().ToList();
#else
        static readonly HashSet<VirtualKey> knownKeys = Enum.GetValues(typeof(VirtualKey)).Cast<VirtualKey>().ToHashSet();
#endif


        bool IsInGroup(VirtualKey key, VirtualKey[] boundaries, bool firstGroupState)
        {
            if (!knownKeys.Contains(key))
                return firstGroupState;

            var state = firstGroupState;
            foreach (var boundary in boundaries)
            {
                if (key <= boundary)
                    return state;
                state = !state;
            }
            return state;
        }
    }
}
