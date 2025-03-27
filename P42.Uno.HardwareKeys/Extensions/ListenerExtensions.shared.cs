using System;

namespace P42.Uno.HardwareKeys;

/// <summary>
/// C# Markup Extensions for HardwareKeys.Listener
/// </summary>
public static class ListenerExtensions
{
    /// <summary>
    /// Add a HardwareKeyDown event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddHardwareKeyDownHandler(this Listener listener, EventHandler<UnoKeyEventArgs> handler)
    { listener.HardwareKeyDown += handler; return listener; }

    /// <summary>
    /// Add a HardwareKeyUp event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddHardwareKeyUpHandler(this Listener listener, EventHandler<UnoKeyEventArgs> handler)
    { listener.HardwareKeyUp += handler; return listener; }

    /// <summary>
    /// Add a IsCapsLockEnabledChanged event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddIsCapsLockEnabledChangedHandler(this Listener listener, EventHandler<KeyState> handler)
    { listener.IsCapsLockEngagedChanged += handler; return listener; }

    /// <summary>
    /// Add a IsShiftPressedChanged event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddIsShiftPressedChangedHandler(this Listener listener, EventHandler<KeyState> handler)
    { listener.IsShiftPressedChanged += handler; return listener; }

    /// <summary>
    /// Add a IsControlPressedChanged event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddIsControlPressedChangedHandler(this Listener listener, EventHandler<KeyState> handler)
    { listener.IsControlPressedChanged += handler; return listener; }

    /// <summary>
    /// Add a IsWindowsPressedChanged event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddIsWindowsPressedChangedHandler(this Listener listener, EventHandler<KeyState> handler)
    { listener.IsWindowsPressedChanged += handler; return listener; }

    /// <summary>
    /// Add a IsMenuPressedChanged event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddIsMenuPressedChangedHandler(this Listener listener, EventHandler<KeyState> handler)
    { listener.IsMenuPressedChanged += handler; return listener; }

    /// <summary>
    /// Add a IsNumLockEnabledChanged event handler
    /// </summary>
    /// <param name="listener">a HardwareKeys.Listener instance</param>
    /// <param name="handler">an event handler</param>
    /// <returns></returns>
    public static Listener AddIsNumLockEnabledChangedHandler(this Listener listener, EventHandler<KeyState> handler)
    { listener.IsNumLockEngagedChanged += handler; return listener; }

    /// <summary>
    /// Add a IsNumLockEngaged event handler
    /// </summary>
    /// <param name="listener"></param>
    /// <param name="engaged"></param>
    /// <returns></returns>
    public static Listener IsNumLockEngaged(this Listener listener, bool engaged)
    { listener.IsNumLockEngaged = engaged ? KeyState.True : KeyState.False; return listener; }
}
