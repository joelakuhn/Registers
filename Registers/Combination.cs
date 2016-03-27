using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace Utilities
{
    class Combination
    {
        public delegate void EventFunction(List<Keys> keys);

        private globalKeyboardHook _gkh = new globalKeyboardHook();
        private EventFunction _f;
        private List<Keys> _combination;
        private List<bool> _pressed = new List<bool>();
        private bool _event = false;
        private bool stopPropogation;

        public Combination(EventFunction f, bool stopPropogation, params Keys[] keys)
        {
            _f = f;
            _combination = keys.ToList();
            foreach (Keys k in _combination)
            {
                _gkh.HookedKeys.Add(k);
                _pressed.Add(false);
            }
            _gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            _gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            this.stopPropogation = stopPropogation;
        }

        private void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            _event = false;
            _pressed[_combination.IndexOf(e.KeyCode)] = false;
        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if (_combination.IndexOf(e.KeyCode) == _combination.Count - 1)
            {
                bool before_pressed = true;
                for (int i = 0; i < _combination.IndexOf(e.KeyCode); i++)
                {
                    if (_pressed[i] == false)
                    {
                        before_pressed = false;
                    }
                }

                if (before_pressed == true && _event == false)
                {
                    _f(_combination);
                    e.Handled = stopPropogation;
                    _event = true;
                }
            }
            else
            {
                _pressed[_combination.IndexOf(e.KeyCode)] = true;
            }
        }
    }
}