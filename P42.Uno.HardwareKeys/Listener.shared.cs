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
    public partial class Listener : Grid
    {
        #region Properties

        public KeyState IsControlPressed => _coreListener.IsControlPressed;

        public KeyState IsShiftPressed => _coreListener.IsShiftPressed;

        public KeyState IsWindowsPressed => _coreListener.IsWindowsPressed;

        public KeyState IsMenuPressed => _coreListener.IsMenuPressed;

        public KeyState IsNumLockEngaged
        {
            get => _coreListener.IsNumLockEngaged;
            set => _coreListener.IsNumLockEngaged = value;
        }

        public KeyState IsCapsLockEngaged => _coreListener.IsCapsLockEngaged;

        public bool IsActive
        {
            get => _coreListener.IsActive;
            set => _coreListener.IsActive = value;
        }

        public bool QuietModifiers
        {
            get => _coreListener.QuietModifiers;
            set => _coreListener.QuietModifiers = value;
        }

        #endregion


        #region Fields
        CoreListener _coreListener;
        #endregion


        #region Events
        public event EventHandler<KeyState> IsControlPressedChanged
        {
            add => _coreListener.IsControlPressedChanged += value;
            remove => _coreListener.IsControlPressedChanged -= value;
        }

        public event EventHandler<KeyState> IsShiftPressedChanged
        {
            add => _coreListener.IsShiftPressedChanged += value;
            remove => _coreListener.IsShiftPressedChanged -= value;
        }

        public event EventHandler<KeyState> IsWindowsPressedChanged
        {
            add => _coreListener.IsWindowsPressedChanged += value;
            remove => _coreListener.IsWindowsPressedChanged -= value;
        }

        public event EventHandler<KeyState> IsMenuPressedChanged
        {
            add => _coreListener.IsMenuPressedChanged += value;
            remove => _coreListener.IsMenuPressedChanged -= value;
        }

        public event EventHandler<KeyState> IsNumLockEngagedChanged
        {
            add => _coreListener.IsNumLockEngagedChanged += value;
            remove => _coreListener.IsNumLockEngagedChanged -= value;
        }

        public event EventHandler<KeyState> IsCapsLockEngagedChanged
        {
            add => _coreListener.IsCapsLockEngagedChanged += value;
            remove => _coreListener.IsCapsLockEngagedChanged -= value;
        }

        public event EventHandler<UnoKeyEventArgs> SimpleKeyDown
        {
            add => _coreListener.SimpleKeyDown += value;
            remove => _coreListener.SimpleKeyDown -= value;
        }

        public event EventHandler<UnoKeyEventArgs> SimpleKeyUp
        {
            add => _coreListener.SimpleKeyUp += value;
            remove => _coreListener.SimpleKeyUp -= value;
        }
        #endregion


        #region Constructor / Disposer
        public Listener()
        {
            Children.Add(new CoreListener().Assign(out _coreListener));
            Children.Add(new Windows.UI.Xaml.Shapes.Rectangle { Fill = new SolidColorBrush(Colors.Red) });
            Opacity = 0.001;
        }
        #endregion
    }
}