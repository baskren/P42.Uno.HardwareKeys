using System;

namespace P42.Uno.HardwareKeys;

/// <summary>
/// State of key engagement
/// </summary>
public enum KeyState
{
    /// <summary>
    /// Key is NOT pressed/engaged
    /// </summary>
    False,
    /// <summary>
    /// Key is pressed/engaged
    /// </summary>
    True,
    /// <summary>
    /// Cannot determine if key is pressed/engaged
    /// </summary>
    Unknown,
}