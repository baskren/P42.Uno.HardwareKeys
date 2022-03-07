using System;

namespace P42.Uno.HardwareKeys
{
    public static class ListenerExtensions
    {
        public static Listener AddOnHardwareKeyDown(this Listener listener, EventHandler<UnoKeyEventArgs> handler)
        { listener.HardwareKeyDown += handler; return listener; }

        public static Listener AddOnHardwareKeyUp(this Listener listener, EventHandler<UnoKeyEventArgs> handler)
        { listener.HardwareKeyUp += handler; return listener; }

        public static Listener AddOnIsCapsLockEnabledChanged(this Listener listener, EventHandler<KeyState> handler)
        { listener.IsCapsLockEngagedChanged += handler; return listener; }

        public static Listener AddOnIsShiftPressedChanged(this Listener listener, EventHandler<KeyState> handler)
        { listener.IsShiftPressedChanged += handler; return listener; }

        public static Listener AddOnIsControlPressedChanged(this Listener listener, EventHandler<KeyState> handler)
        { listener.IsControlPressedChanged += handler; return listener; }

        public static Listener AddOnIsWindowsPressedChanged(this Listener listener, EventHandler<KeyState> handler)
        { listener.IsWindowsPressedChanged += handler; return listener; }

        public static Listener AddOnIsMenuPressedChanged(this Listener listener, EventHandler<KeyState> handler)
        { listener.IsMenuPressedChanged += handler; return listener; }

        public static Listener AddOnIsNumLockEnabledChanged(this Listener listener, EventHandler<KeyState> handler)
        { listener.IsNumLockEngagedChanged += handler; return listener; }

        public static Listener IsNumLockEngaged(this Listener listener, bool engaged)
        { listener.IsNumLockEngaged = engaged ? KeyState.True : KeyState.False; return listener; }
    }
}