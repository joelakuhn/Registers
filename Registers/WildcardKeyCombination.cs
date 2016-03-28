using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registers
{
    class WildcardKeyCombination : KeyCombination
    {
        private Action<Keys> Callback;

        public WildcardKeyCombination(Action<Keys> callback, bool stopPropogation, params Keys[] keys)
        {
            Keys = keys;
            StopPropogation = stopPropogation;
            Callback = callback;
        }

        public override bool Check(HashSet<Keys> pressed)
        {
            if (Keys.All(pressed.Contains) && Keys.Length == pressed.Count - 1)
            {
                Callback(pressed.First(key => !this.Keys.Contains(key)));
                return true;
            }
            return false;
        }
    }
}
