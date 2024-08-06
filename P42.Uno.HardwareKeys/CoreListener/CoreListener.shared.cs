using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Input;
using System.Threading.Tasks;

namespace P42.Uno.HardwareKeys
{
    [Bindable]
    partial class CoreListener
    {
        #region Properties

        #region Modifier Keys
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

        KeyState _numLockEngaged = KeyState.Unknown;
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
        #endregion

        bool _isActive;
        public bool IsActive
        {
            get => _isActive; 
            set
            {
                if (value != _isActive)
                {
                    if (FocusManually)
                        _isActive = false;
                    else
                        _isActive= value;
                    if (_isActive)
                        //this.Focus(FocusState.Programmatic);
                        TryFocusAsync(this, FocusState.Programmatic).Forget();
                }
            }
        }

        public bool MuteModifiers { get; set; } = true;

        public bool IsTabToMoveFocusEnabled { get; set; }

        public bool GreedyFocus { get; set; } = true;

        bool _focusManually = true;
        public bool FocusManually 
        { 
            get => _focusManually;
            set
            {
                if (_focusManually != value)
                {
                    _isActive = false;
                    _focusManually = value;
                }
            }
        }

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
        Control _platformCoreElement;
        Control _lastFocusedControl;
        Control _tryingToFocusControl;
        #endregion


        #region Events
        public event EventHandler<KeyState> IsControlPressedChanged;
        public event EventHandler<KeyState> IsShiftPressedChanged;
        public event EventHandler<KeyState> IsWindowsPressedChanged;
        public event EventHandler<KeyState> IsMenuPressedChanged;
#pragma warning disable CS0067
        public event EventHandler<KeyState> IsNumLockEngagedChanged;
#pragma warning restore CS0067
        public event EventHandler<KeyState> IsCapsLockEngagedChanged;
        public event EventHandler<UnoKeyEventArgs> HardwareKeyDown;
        public event EventHandler<UnoKeyEventArgs> HardwareKeyUp;
        #endregion


        #region Construction / Disposal

        public CoreListener()
        {
            PlatformBuild();
            FocusManager.GotFocus += FocusManager_GotFocus;
            FocusManager.LosingFocus += FocusManager_LosingFocus;
            FocusManager.LostFocus += FocusManager_LostFocus;
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

        async Task<FocusMovementResult> TryFocusAsync(Control control, FocusState state)
        {
            _tryingToFocusControl = control;
            var result = await FocusManager.TryFocusAsync(control, state);
            _tryingToFocusControl = null;
            return result;
        }

        async Task MoveFocusToEditable()
        {
            try
            {
                if (FocusManager.FindNextFocusableElement(FocusNavigationDirection.Next) is Control control &&
                    control.IsTextEditable())
                    await TryFocusAsync(control, FocusState.Keyboard);
                else if (_lastFocusedControl != null)
                    //_lastFocusedControl?.Focus(FocusState.Keyboard);
                    await TryFocusAsync(_lastFocusedControl, FocusState.Keyboard);
            }
            catch (Exception) 
            {
                if (_lastFocusedControl != null)
                    await TryFocusAsync(_lastFocusedControl, FocusState.Keyboard);
            }

        }

        protected virtual void OnSimpleKeyDown(string simpleKey, VirtualKey virtualKey, VirtualKey[] modifiers = null)
        {
            if (!IsActive && !FocusManually)
            {
                MoveFocusToEditable().Forget();
                return;
            }

            if (IsTabToMoveFocusEnabled &&
                virtualKey == VirtualKey.Tab &&
                !modifiers.HasNonToggleModifier()
               )
            {
                MoveFocusToEditable().Forget();
                return;
            }
            

            HardwareKeyDown?.Invoke(this, new UnoKeyEventArgs(simpleKey, virtualKey, modifiers ?? CurrentModifiers));
        }

        protected virtual void OnSimpleKeyUp(string simpleKey, VirtualKey virtualKey, VirtualKey[] modifiers = null)
        {

            HardwareKeyUp?.Invoke(this, new UnoKeyEventArgs(simpleKey, virtualKey, modifiers ?? CurrentModifiers));
        }

        #endregion

        void Reset()
        {
            IsShiftPressed = KeyState.Unknown;
            IsControlPressed = KeyState.Unknown;
            IsWindowsPressed = KeyState.Unknown;
            IsMenuPressed = KeyState.Unknown;
            IsCapsLockEngaged = KeyState.Unknown;
#if !__IOS__
            IsNumLockEngaged = KeyState.Unknown;
#endif
        }

        private async void FocusManager_GotFocus(object sender, FocusManagerGotFocusEventArgs e)
        {

            //System.Diagnostics.Debug.WriteLine($"FocusManager_GotFocus({GetNameFor(sender)}, {GetNameFor(e.NewFocusedElement)})");

            if (e.NewFocusedElement == _platformCoreElement)
            {
                // We are now focused on our platform specific keyboard listener element!
                PlatformGotFocus();
                // Don't do the following here!  If using Menu to toggle between apps, this could create a false trigger!
                //PlatformShiftPressedQuery();
                //PlatformControlPressedQuery();
                //PlatformWindowsPressedQuery();
                //PlatformMenuPressedQuery(); 
                PlatformCapsLockQuery();
                PlatformNumLockQuery();
            }
            else if (FocusManually)
                return;
            else if (IsActive)
            {
                // We're wrongly focused on something else besides the platform specific keyboard listener element.
                if (e.NewFocusedElement == this)
                    // We're focused on the CoreListener element - which means we're actually focused on the platform specific listener element
                    return;

                if (e.NewFocusedElement == default || (GreedyFocus && !((Control)e.NewFocusedElement).IsTextEditable()))
                    // Yikes!  We're focused on something else - and it's not a text editable element - so let's get the focus back!
                    await TryFocusAsync(this, FocusState.Keyboard);
            }
            else
            {
                // we're not supposed to have focus - so let's just assure we don't
                if (e.NewFocusedElement == this && IsFocusEngaged)
                    Focus(FocusState.Unfocused);
                else if (e.NewFocusedElement == _platformCoreElement && _platformCoreElement.IsFocusEngaged)
                    _platformCoreElement.Focus(FocusState.Unfocused);
            }
                

        }

        LosingFocusEventArgs losingFocusEventArgs;
        private void FocusManager_LosingFocus(object sender, LosingFocusEventArgs e)
        {
            losingFocusEventArgs = e;

        }

        private void FocusManager_LostFocus(object sender, FocusManagerLostFocusEventArgs e)
        {
            if (e.OldFocusedElement != losingFocusEventArgs?.OldFocusedElement)
                return;

            // who is losing focus?
            if (e.OldFocusedElement is Control control &&
                control != this &&
                control != _platformCoreElement &&
                control.IsTextEditable()
                )
                _lastFocusedControl = control;

            //System.Diagnostics.Debug.WriteLine($"LosingFocus: ENTER sender:[{GetNameFor(sender)}] old:{GetNameFor(e.OldFocusedElement)} new:{GetNameFor(e.NewFocusedElement)} state:{e.FocusState} inDev:{e.InputDevice}");

            if (FocusManually)
            {
                if (e.OldFocusedElement != _platformCoreElement && losingFocusEventArgs?.NewFocusedElement == this)
                    Reset();

                return;
            }

            if (IsActive)
            {
                if (e.OldFocusedElement == _platformCoreElement)
                {
                    // hey!  We're supposed to have focus!
                    if (losingFocusEventArgs?.NewFocusedElement == default || (GreedyFocus && _tryingToFocusControl is null))
                    {
                        //if (!e.TryCancel())  // not working in iOS?
                        TryFocusAsync(_platformCoreElement, FocusState.Programmatic).Forget();
                        return;
                    }
                }
                else if (e.OldFocusedElement != null && losingFocusEventArgs?.NewFocusedElement == this)
                    Reset();
            }

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
                //return (Microsoft.UI.Xaml.Window.Current.CoreWindow.GetKeyState(VirtualKey.NumberKeyLock) & Windows.UI.Core.CoreVirtualKeyStates.Locked) != 0;
                return (WinUIKeyState(key) & Windows.UI.Core.CoreVirtualKeyStates.Locked) != 0;
            return IsInGroup(key, _charKeyBoundaries, true);
        }


        Windows.UI.Core.CoreVirtualKeyStates WinUIKeyState(VirtualKey key)
            => Microsoft.UI.Xaml.Window.Current?.CoreWindow.GetKeyState(key) ?? InputKeyboardSource.GetKeyStateForCurrentThread(key);

        KeyState WinUIKeyEngaged(VirtualKey key, Windows.UI.Core.CoreVirtualKeyStates state)
            => (WinUIKeyState(key) & state) != 0
            ? KeyState.True
            : KeyState.False;

        KeyState WinUIKeyEngaged(VirtualKey key1, VirtualKey key2, Windows.UI.Core.CoreVirtualKeyStates state)
            => ((WinUIKeyState(key1) & state) | (WinUIKeyState(key2)&state )) != 0
            ? KeyState.True
            : KeyState.False;

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
