using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.System;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace P42.Uno.HardwareKeys
{
    /// <summary>
    /// UIElement that, when focused, captures hardware key events 
    /// </summary>
    [Bindable]
    public partial class Listener : Grid
    {
        #region Properties

        /// <summary>
        /// True if CONTROL key is pressed
        /// </summary>
        public KeyState IsControlPressed => _coreListener.IsControlPressed;

        /// <summary>
        /// True if SHIFT key is pressed
        /// </summary>
        public KeyState IsShiftPressed => _coreListener.IsShiftPressed;

        /// <summary>
        /// True if WINDOWS key is pressed
        /// </summary>
        public KeyState IsWindowsPressed => _coreListener.IsWindowsPressed;

        /// <summary>
        /// True if MENU key is pressed
        /// </summary>
        public KeyState IsMenuPressed => _coreListener.IsMenuPressed;

        /// <summary>
        /// True if NUMLOCK key is engaged
        /// </summary>
        public KeyState IsNumLockEngaged
        {
            get => _coreListener.IsNumLockEngaged;
            set => _coreListener.IsNumLockEngaged = value;
        }

        /// <summary>
        /// True if CAPSLOCK key is engaged
        /// </summary>
        public KeyState IsCapsLockEngaged => _coreListener.IsCapsLockEngaged;

        /// <summary>
        /// True if Listener is Active
        /// </summary>
        public bool IsActive
        {
            get => _coreListener.IsActive;
            set => _coreListener.IsActive = value;
        }

        /// <summary>
        /// True if modifier keys are not published
        /// </summary>
        public bool MuteModifiers
        {
            get => _coreListener.MuteModifiers;
            set => _coreListener.MuteModifiers = value;
        }

        public bool IsTabToMoveFocusEnabled
        {
            get => _coreListener.IsTabToMoveFocusEnabled;
            set => _coreListener.IsTabToMoveFocusEnabled = value;
        }

        public bool FocusManually
        {
            get => _coreListener.FocusManually;
            set => _coreListener.FocusManually = value;
        }
        #endregion


        #region Fields
        CoreListener _coreListener;
        #endregion


        #region Events
        /// <summary>
        /// Fired when CONTROL key is pressed
        /// </summary>
        public event EventHandler<KeyState> IsControlPressedChanged
        {
            add => _coreListener.IsControlPressedChanged += value;
            remove => _coreListener.IsControlPressedChanged -= value;
        }

        /// <summary>
        /// Fired when SHIFT key is pressed
        /// </summary>
        public event EventHandler<KeyState> IsShiftPressedChanged
        {
            add => _coreListener.IsShiftPressedChanged += value;
            remove => _coreListener.IsShiftPressedChanged -= value;
        }

        /// <summary>
        /// Fired when WINDOWS key is pressed
        /// </summary>
        public event EventHandler<KeyState> IsWindowsPressedChanged
        {
            add => _coreListener.IsWindowsPressedChanged += value;
            remove => _coreListener.IsWindowsPressedChanged -= value;
        }

        /// <summary>
        /// Fired when MENU key is pressed
        /// </summary>
        public event EventHandler<KeyState> IsMenuPressedChanged
        {
            add => _coreListener.IsMenuPressedChanged += value;
            remove => _coreListener.IsMenuPressedChanged -= value;
        }

        /// <summary>
        /// Fired when NUMLOCK key engagement has changed
        /// </summary>
        public event EventHandler<KeyState> IsNumLockEngagedChanged
        {
            add => _coreListener.IsNumLockEngagedChanged += value;
            remove => _coreListener.IsNumLockEngagedChanged -= value;
        }

        /// <summary>
        /// Fired when CAPSLOCK key engagement has changed
        /// </summary>
        public event EventHandler<KeyState> IsCapsLockEngagedChanged
        {
            add => _coreListener.IsCapsLockEngagedChanged += value;
            remove => _coreListener.IsCapsLockEngagedChanged -= value;
        }

        /// <summary>
        /// Fired when a hardware key is pressed down
        /// </summary>
        public event EventHandler<UnoKeyEventArgs> HardwareKeyDown
        {
            add => _coreListener.HardwareKeyDown += value;
            remove => _coreListener.HardwareKeyDown -= value;
        }

        /// <summary>
        /// Fired when a hardware key is released
        /// </summary>
        public event EventHandler<UnoKeyEventArgs> HardwareKeyUp
        {
            add => _coreListener.HardwareKeyUp += value;
            remove => _coreListener.HardwareKeyUp -= value;
        }
        #endregion


        #region Constructor / Disposer
        /// <summary>
        /// Constructor for HardwareKey.Listener
        /// </summary>
        public Listener()
        {
            Children.Add(new CoreListener().Assign(out _coreListener));
            Children.Add(new Microsoft.UI.Xaml.Shapes.Rectangle { Fill = new SolidColorBrush(Colors.Red) });
            Opacity = 0.001;
        }
        #endregion


        #region IDisposable Support

        bool _disposed;
#if WINDOWS
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#else
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }
#endif

#if __ANDROID__ 
        protected override void Dispose(bool disposing)
#elif __MACOS__ || __IOS__
        protected virtual new void Dispose(bool disposing)
#else
        protected virtual void Dispose(bool disposing)
#endif
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _coreListener?.Dispose();
            }
#if __ANDROID__ || __MACOS__ || __IOS__
            base.Dispose(disposing);
#endif
        }

        #endregion


    }
}
