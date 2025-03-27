using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;

namespace P42.Uno.HardwareKeys;

/// <summary>
/// EventArgs for HardwareKey events
/// </summary>
public class UnoKeyEventArgs : EventArgs
{
    /// <summary>
    /// Localized (when possible) representation of key during key event
    /// </summary>
    public string Characters { get; private set; }

    /// <summary>
    /// The Windows.System.VirtualKey for the key
    /// </summary>
    public VirtualKey VirtualKey { get; private set; }

    /// <summary>
    /// Array of Windows.System.VirtualKey modifier keys that were active during key event
    /// </summary>
    public VirtualKey[] Modifiers { get; private set; }

    internal UnoKeyEventArgs(string simpleKey, VirtualKey key, VirtualKey[] modifiers)
    {
        Characters = simpleKey;
        VirtualKey = key;
        Modifiers = modifiers;
    }
}