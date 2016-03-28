using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Registers
{
    class KeyCombination
    {
        public Keys[] Keys;

        public bool StopPropogation;

        private readonly Action Callback;

        public KeyCombination() { }
        
        public KeyCombination(Action callback, bool stopPropogation, params Keys[] keys)
        {
            Callback = callback;
            StopPropogation = stopPropogation;
            Keys = keys;
        }

        public virtual bool Check(HashSet<Keys> pressed)
        {
            if (Keys.All(pressed.Contains))
            {
                Callback();
                return true;
            }
            return false;
        }
    }
}
