using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace P42.Uno.SimpleHardwareKeyboardListener
{
    [Bindable]
    public partial class SimpleKeyboardListener
    {
        #region Properties


        bool _isControlPressed;
        public bool IsControlPressed
        {
            get => _isControlPressed;
            set
            {
                if (_isControlPressed != value)
                {
                    _isControlPressed = value;
                    OnIsControlPressedChanged();
                    IsControlPressedChanged?.Invoke(this, value);
                }
            }
        }

        bool _isShiftPressed;
        public bool IsShiftPressed
        {
            get => _isShiftPressed;
            set
            {
                if (_isShiftPressed != value)
                {
                    _isShiftPressed = value;
                    OnIsShiftPressedChanged();
                    IsShiftPressedChanged?.Invoke(this, value);
                }
            }
        }

        bool _isWindowsPressed;
        public bool IsWindowsPressed
        {
            get => _isWindowsPressed;
            set
            {
                if (_isWindowsPressed != value)
                {
                    _isWindowsPressed = value;
                    OnIsWindowsPressedChanged();
                    IsWindowsPressedChanged?.Invoke(this, value);
                }
            }
        }

        bool _isMenuPressed;
        public bool IsMenuPressed
        {
            get => _isMenuPressed;
            set
            {
                if (!_isMenuPressed != value)
                {
                    _isWindowsPressed = value;
                    OnIsMenuPressedChanged();
                    IsMenuPressedChanged?.Invoke(this, value);
                }
            }
        }

        bool _isNumLockPressed;
        public bool IsNumLockPressed
        {
            get => _isNumLockPressed;
            set
            {
                if (_isNumLockPressed != value)
                {
                    _isNumLockPressed = value;
                    OnIsNumLockPressedChanged();
                    IsNumLockPressedChanged?.Invoke(this, value);
                }
            }
        }

        bool _isCapsLockPressed;
        public bool IsCapsLockPressed
        {
            get => _isCapsLockPressed;
            set
            {
                if (_isCapsLockPressed != value)
                {
                    _isCapsLockPressed = value;
                    OnIsCapsLockPressedChanged();
                    IsCapsLockPressedChanged?.Invoke(this, value);
                }
            }
        }

        public bool QuietModifiers { get; set; } = true;

        Func<bool> _platformIsNumLockEngaged;
        public bool IsNumLockEngaged => _platformIsNumLockEngaged.Invoke();

        Func<bool> _platformIsCapsLockEngaged;
        public bool IsCapsLockEngaged => _platformIsCapsLockEngaged.Invoke();

        VirtualKey[] CurrentModifiers
        {
            get
            {
                var result = new List<VirtualKey>();

                if (IsShiftPressed)
                    result.Add(VirtualKey.Shift);
                if (IsControlPressed)
                    result.Add(VirtualKey.Control);
                if (IsWindowsPressed)
                    result.Add(VirtualKey.LeftWindows);
                if (IsMenuPressed)
                    result.Add(VirtualKey.Menu);
                //if (IsCapsLockEngaged)
                //    result.Add(VirtualKey.CapitalLock);
                //if (IsNumLockEngaged)
                //    result.Add(VirtualKey.NumberKeyLock);

                return result.ToArray();
            }
        }


        #endregion


        #region Fields
        #endregion


        #region Events
        public event EventHandler<bool> IsControlPressedChanged;
        public event EventHandler<bool> IsShiftPressedChanged;
        public event EventHandler<bool> IsWindowsPressedChanged;
        public event EventHandler<bool> IsMenuPressedChanged;
        public event EventHandler<bool> IsNumLockPressedChanged;
        public event EventHandler<bool> IsCapsLockPressedChanged;
        public event EventHandler<UnoKeyEventArgs> SimpleKeyDown;
        public event EventHandler<UnoKeyEventArgs> SimpleKeyUp;
        #endregion


        #region Construction / Disposal

        public SimpleKeyboardListener()
        {
            PlatformBuild();
        }

        partial void PlatformBuild();
        #endregion


        #region Virtual Methods
        protected virtual void OnIsControlPressedChanged() { }
        protected virtual void OnIsShiftPressedChanged() { }
        protected virtual void OnIsWindowsPressedChanged() { }
        protected virtual void OnIsMenuPressedChanged() { }
        protected virtual void OnIsNumLockPressedChanged() { }
        protected virtual void OnIsCapsLockPressedChanged() { }

        protected virtual void OnSimpleKeyDown(string simpleKey, VirtualKey virtualKey, VirtualKey[] modifiers = null)
            => SimpleKeyDown?.Invoke(this, new UnoKeyEventArgs(simpleKey, virtualKey, modifiers ?? CurrentModifiers));

        protected virtual void OnSimpleKeyUp(string simpleKey, VirtualKey virtualKey, VirtualKey[] modifiers = null)
            => SimpleKeyUp?.Invoke(this, new UnoKeyEventArgs(simpleKey, virtualKey, modifiers ?? CurrentModifiers));

        #endregion


        #region Private Methods

        bool ProcessModifier(VirtualKey key, bool down)
        {
            bool isModifier = false;
            switch (key)
            {
                case VirtualKey.Control:
                case VirtualKey.LeftControl:
                case VirtualKey.RightControl:
                    isModifier = true;
                    IsControlPressed = down;
                    break;
                case VirtualKey.Shift:
                case VirtualKey.LeftShift:
                case VirtualKey.RightShift:
                    isModifier = true;
                    IsShiftPressed = down;
                    break;
                case VirtualKey.LeftWindows:
                case VirtualKey.RightWindows:
                    isModifier = true;
                    IsWindowsPressed = down;
                    break;
                case VirtualKey.Menu:
                case VirtualKey.LeftMenu:
                case VirtualKey.RightMenu:
                    isModifier = true;
                    IsMenuPressed = down;
                    break;
                case VirtualKey.NumberKeyLock:
                    isModifier = true;
                    IsNumLockPressed = down;
                    break;
                case VirtualKey.CapitalLock:
                    isModifier = true;
                    IsCapsLockPressed = down;
                    break;
            }

            return isModifier;
        }

        /* Shouldn't need because is handled by ProcessModifier
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
        */

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

#if NETSTANDARD 
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

        #endregion

    }
}
