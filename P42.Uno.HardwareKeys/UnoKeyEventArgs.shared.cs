using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;

namespace P42.Uno.HardwareKeys
{
    public class UnoKeyEventArgs : EventArgs
    {
        public string Characters { get; private set; }

        public VirtualKey VirtualKey { get; private set; }

        public VirtualKey[] Modifiers { get; private set; }

        internal UnoKeyEventArgs(string simpleKey, VirtualKey key, VirtualKey[] modifiers)
        {
            Characters = simpleKey;
            VirtualKey = key;
            Modifiers = modifiers;
        }
    }
}
