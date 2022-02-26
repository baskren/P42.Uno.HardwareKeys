using System;
using System.Collections.Generic;
using System.Text;
using Windows.System;

namespace Uno.Keyboard
{
    public class UnoKeyEventArgs : EventArgs
    {
        public string SimpleKey { get; private set; }

        public VirtualKey Key { get; private set; }

        public VirtualKey[] Modifiers { get; private set; }

        internal UnoKeyEventArgs(string simpleKey, VirtualKey key, VirtualKey[] modifiers)
        {
            SimpleKey = simpleKey;
            Key = key;
            Modifiers = modifiers;
        }
    }
}
