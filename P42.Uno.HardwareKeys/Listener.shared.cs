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

namespace P42.Uno.HardwareKeys
{
    [Bindable]
    public partial class Listener
    {
        #region Properties


        KeyState _isControlPressed;
        public KeyState IsControlPressed
        {
            get => _isControlPressed;
            private set
            {
                if (_isControlPressed != value)
                {
                    _isControlPressed = value;
                    OnIsControlPressedChanged();
                    IsControlPressedChanged?.Invoke(this, value);
                }
            }
        }

        KeyState _isShiftPressed;
        public KeyState IsShiftPressed
        {
            get => _isShiftPressed;
            private set
            {
                if (_isShiftPressed != value)
                {
                    _isShiftPressed = value;
                    OnIsShiftPressedChanged();
                    IsShiftPressedChanged?.Invoke(this, value);
                }
            }
        }

        KeyState _isWindowsPressed;
        public KeyState IsWindowsPressed
        {
            get => _isWindowsPressed;
            private set
            {
                if (_isWindowsPressed != value)
                {
                    _isWindowsPressed = value;
                    OnIsWindowsPressedChanged();
                    IsWindowsPressedChanged?.Invoke(this, value);
                }
            }
        }

        KeyState _isMenuPressed;
        public KeyState IsMenuPressed
        {
            get => _isMenuPressed;
            private set
            {
                if (_isMenuPressed != value)
                {
                    _isMenuPressed = value;
                    OnIsMenuPressedChanged();
                    IsMenuPressedChanged?.Invoke(this, value);
                }
            }
        }

        KeyState _numLockEngaged;
        public KeyState IsNumLockEngaged
        {
            get => _numLockEngaged;
            set
            {
#if __IOS__ || __ANDROID__
                if (_numLockEngaged != value)
                {
                    _numLockEngaged = value;
                    OnNumLockStateChanged();
                    IsNumLockEngagedChanged?.Invoke(this, value);
                }
#endif
            }
        }

        KeyState _capsLockEngaged;
        public KeyState IsCapsLockEngaged
        {
            get => _capsLockEngaged;
            private set
            {
                if (_capsLockEngaged != value)
                {
                    _capsLockEngaged = value;
                    OnCapsLockStateChanged();
                    IsCapsLockEngagedChanged?.Invoke(this, value);
                }
            }
        }

        public bool IsActive
        {
            get => FocusManager.GetFocusedElement() == _platformCoreElement;
            set
            {
                if (value != IsActive)
                {
                    if (value)
                        _platformCoreElement.Focus(FocusState.Programmatic);
                    else
                    {
                        _tryingToDeactivate = true;
                        FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
                    }
                }
            }
        }

        public bool QuietModifiers { get; set; } = true;


        VirtualKey[] CurrentModifiers
        {
            get
            {
                var result = new List<VirtualKey>();

                if (IsShiftPressed == KeyState.True)
                    result.Add(VirtualKey.Shift);
                if (IsControlPressed == KeyState.True)
                    result.Add(VirtualKey.Control);
                if (IsWindowsPressed == KeyState.True)
                    result.Add(VirtualKey.LeftWindows);
                if (IsMenuPressed == KeyState.True)
                    result.Add(VirtualKey.Menu);
                if (IsCapsLockEngaged == KeyState.True)
                    result.Add(VirtualKey.CapitalLock);
                if (IsNumLockEngaged == KeyState.True)
                    result.Add(VirtualKey.NumberKeyLock);

                return result.ToArray();
            }
        }


        #endregion


        #region Fields
        bool _tryingToDeactivate;
        Control _platformCoreElement;
        #endregion


        #region Events
        public event EventHandler<KeyState> IsControlPressedChanged;
        public event EventHandler<KeyState> IsShiftPressedChanged;
        public event EventHandler<KeyState> IsWindowsPressedChanged;
        public event EventHandler<KeyState> IsMenuPressedChanged;
        public event EventHandler<KeyState> IsNumLockEngagedChanged;
        public event EventHandler<KeyState> IsCapsLockEngagedChanged;
        public event EventHandler<UnoKeyEventArgs> SimpleKeyDown;
        public event EventHandler<UnoKeyEventArgs> SimpleKeyUp;
        #endregion


        #region Construction / Disposal

        public Listener()
        {
            PlatformBuild();
            FocusManager.GotFocus += FocusManager_GotFocus;
            FocusManager.LosingFocus += FocusManager_LosingFocus;

        }

        #endregion


        #region Private Methods

        #region Partial Methods
        partial void PlatformGotFocus();
        partial void PlatformShiftPressedQuery();
        partial void PlatformControlPressedQuery();
        partial void PlatformWindowsPressedQuery();
        partial void PlatformMenuPressedQuery();
        partial void PlatformNumLockQuery();
        partial void PlatformCapsLockQuery();
        #endregion

        #region Virtual Methods
        protected virtual void OnIsControlPressedChanged() { }
        protected virtual void OnIsShiftPressedChanged() { }
        protected virtual void OnIsWindowsPressedChanged() { }
        protected virtual void OnIsMenuPressedChanged() { }
        protected virtual void OnNumLockStateChanged() { }
        protected virtual void OnCapsLockStateChanged() { }

        protected virtual void OnSimpleKeyDown(string simpleKey, VirtualKey virtualKey, VirtualKey[] modifiers = null)
            => SimpleKeyDown?.Invoke(this, new UnoKeyEventArgs(simpleKey, virtualKey, modifiers ?? CurrentModifiers));

        protected virtual void OnSimpleKeyUp(string simpleKey, VirtualKey virtualKey, VirtualKey[] modifiers = null)
            => SimpleKeyUp?.Invoke(this, new UnoKeyEventArgs(simpleKey, virtualKey, modifiers ?? CurrentModifiers));

        #endregion



        private void FocusManager_GotFocus(object sender, FocusManagerGotFocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"FocusManager_GotFocus({sender}, {e.NewFocusedElement})");
            if (e.NewFocusedElement == _platformCoreElement)
            {
                PlatformGotFocus();
                PlatformShiftPressedQuery();
                PlatformControlPressedQuery();
                PlatformWindowsPressedQuery();
                PlatformMenuPressedQuery();
                PlatformCapsLockQuery();
                PlatformNumLockQuery();
            }
        }

        private void FocusManager_LosingFocus(object sender, LosingFocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"LosingFocus: ENTER sender:[{sender}] old:{e.OldFocusedElement} new:{e.NewFocusedElement} state:{e.FocusState} inDev:{e.InputDevice}");
            if (!_tryingToDeactivate && e.OldFocusedElement == _platformCoreElement)
            {
                System.Diagnostics.Debug.WriteLine($"LosingFocus: A");
                e.TryCancel();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"LosingFocus: B");
                IsShiftPressed = KeyState.Unknown;
                IsControlPressed = KeyState.Unknown;
                IsWindowsPressed = KeyState.Unknown;
                IsMenuPressed = KeyState.Unknown;
                IsCapsLockEngaged = KeyState.Unknown;
#if !__IOS__
                IsNumLockEngaged = KeyState.Unknown;
#endif
            }

            _tryingToDeactivate = false;
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"OnLostFocus");
            IsShiftPressed = KeyState.Unknown;
            IsControlPressed = KeyState.Unknown;
            IsWindowsPressed = KeyState.Unknown;
            IsMenuPressed = KeyState.Unknown;
            IsCapsLockEngaged = KeyState.Unknown;
#if !__IOS__
            IsNumLockEngaged = KeyState.Unknown;
#endif
        }


        bool ProcessModifier(VirtualKey key, bool down)
        {
            bool isModifier = false;
            switch (key)
            {
                case VirtualKey.Control:
                case VirtualKey.LeftControl:
                case VirtualKey.RightControl:
                    isModifier = true;
                    IsControlPressed = down ? KeyState.True : KeyState.False;
                    break;
                case VirtualKey.Shift:
                case VirtualKey.LeftShift:
                case VirtualKey.RightShift:
                    isModifier = true;
                    IsShiftPressed = down ? KeyState.True : KeyState.False;
                    break;
                case VirtualKey.LeftWindows:
                case VirtualKey.RightWindows:
                    isModifier = true;
                    IsWindowsPressed = down ? KeyState.True : KeyState.False;
                    break;
                case VirtualKey.Menu:
                case VirtualKey.LeftMenu:
                case VirtualKey.RightMenu:
                    isModifier = true;
                    IsMenuPressed = down ? KeyState.True : KeyState.False;
                    break;
                case VirtualKey.CapitalLock:
                case VirtualKey.NumberKeyLock:
                    isModifier = true;
                    break;
            }

            PlatformNumLockQuery();
            PlatformCapsLockQuery();

            return isModifier;
        }

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
