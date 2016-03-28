using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace Registers
{
    class CombinationMonitor
    {
        public readonly HashSet<Keys> PressedKeys = new HashSet<Keys>(); 

        public readonly List<KeyCombination> Combinations = new List<KeyCombination>(); 

        private readonly GlobalKeyboardHook keyboardHook;

        private static CombinationMonitor instance;

        public static CombinationMonitor Instance
        {
            get { return instance ?? (instance = new CombinationMonitor()); }
        }

        private CombinationMonitor()
        {
            keyboardHook = new GlobalKeyboardHook();
            keyboardHook.KeyDown += OnKeyDown;
            keyboardHook.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs args)
        {
            PressedKeys.Remove(args.KeyCode);
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            PressedKeys.Add(args.KeyCode);
            CheckCombinations(args);
        }

        private void CheckCombinations(KeyEventArgs args)
        {
            foreach (var combo in Combinations)
            {
                if (combo.Check(PressedKeys))
                {
                    args.Handled = combo.StopPropogation;
                    break;
                }
            }
        }

        public KeyCombination Create(Action callback, bool stopPropogation, params Keys[] keys)
        {
            var newCombo = new KeyCombination(callback, stopPropogation, keys);
            Combinations.Add(newCombo);
            return newCombo;
        }
    }
}
