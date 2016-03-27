using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Utilities;

namespace Registers
{
    public partial class RegistersUI : Form
    {
        readonly Dictionary<Keys, ClipboardData> _registers = new Dictionary<Keys, ClipboardData>();
        Keys _currentRegister = Keys.D1;
        readonly List<ClipboardData> _queue = new List<ClipboardData>();
        private int _queueInd;
        private bool _isQueueMode;
        readonly List<Combination> _combinations = new List<Combination>(); 

        public RegistersUI()
        {
            InitializeComponent();
            for (var dKey = Keys.D0; dKey <= Keys.D9; dKey++)
            {
                _combinations.Add(new Combination(SwitchReg, true, Keys.LShiftKey, Keys.RShiftKey, dKey));
            }
            for (var ckey = Keys.A; ckey <= Keys.Z; ckey++)
            {
                _combinations.Add(new Combination(SwitchReg, true, Keys.LShiftKey, Keys.RShiftKey, ckey));
            }

            Console.WriteLine((int)Keys.A);

            // 221 == Bight Bracket
            // 219 == Left Bracket
            // 186 == Semicolon
            // 192 == Single Quote
            // 191 == Forward Slash
            _combinations.Add(new Combination(QueueStart, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)186));
            _combinations.Add(new Combination(QueueEnd, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)192));
            _combinations.Add(new Combination(QueueNext, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)221));
            _combinations.Add(new Combination(QueuePrev, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)219));
            _combinations.Add(new Combination(QueueLines, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)191));
            _combinations.Add(new Combination(QueuePush, false, Keys.RControlKey, Keys.C));
            _combinations.Add(new Combination(QueuePush, false, Keys.RControlKey, Keys.X));
            _combinations.Add(new Combination(QueuePull, false, Keys.RControlKey, Keys.V));
            _combinations.Add(new Combination(QueuePush, false, Keys.LControlKey, Keys.C));
            _combinations.Add(new Combination(QueuePush, false, Keys.LControlKey, Keys.X));
            _combinations.Add(new Combination(QueuePull, false, Keys.LControlKey, Keys.V));
        }

        public void QueueLines(List<Keys> keys)
        {
            _queue.Clear();
            var clipData = GetClip();
            if (clipData != null && clipData.Objects.Any(kvp => kvp.Value is string))
            {
                var stringData = (string)clipData.Objects.First(kvp => kvp.Value is string).Value;
                var lines = stringData.Replace("\r\n", "\n").Split('\n');
                foreach (var line in lines)
                {
                    _queue.Add(new ClipboardData(DataFormats.StringFormat, line));
                }
                _queueInd = 0;
                _isQueueMode = true;
            }
        }

        public void QueueStart(List<Keys> keys)
        {
            _queueInd = 0;
            _isQueueMode = true;
        }

        public void QueueEnd(List<Keys> keys)
        {
            _isQueueMode = false;
        }

        private void QueueNext(List<Keys> keys)
        {
            _queueInd++;
        }

        private void QueuePrev(List<Keys> keys)
        {
            _queueInd = (_queueInd + _queue.Count - 1) % _queue.Count;
        }

        private void QueuePush(List<Keys> keys)
        {
            var timer = new Timer();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                if (!_isQueueMode) return;
                if (_queueInd == 0)
                {
                    _queue.Clear();
                }
                var clipData = GetClip();
                _queue.Add(clipData);
                _queueInd++;
            };
            timer.Interval = 10;
            timer.Start();
        }

        private void QueuePull(List<Keys> keys)
        {
            if (!_isQueueMode) return;
            if (_queue.Count == 0) return;
            if (_queueInd >= _queue.Count)
            {
                _queueInd = _queueInd % _queue.Count;
            }
            SetClip(_queue[_queueInd]);
            _queueInd++;
        }

        public void SwitchReg(List<Keys> keys)
        {
            _isQueueMode = false;
            var newRegister = keys.FirstOrDefault(k =>
                (k >= Keys.D0 && k <= Keys.D9)
                || (k >= Keys.A && k <= Keys.Z));

            SaveReg(_currentRegister);
            ReadReg(newRegister);

            _currentRegister = newRegister;
        }

        public void SaveReg(Keys key)
        {
            MethodInvoker invoker = delegate
                {
                var clipData = GetClip();
                _registers[key] = clipData;
            };
            Invoke(invoker);
        }

        ClipboardData GetClip()
        {
            var dataObject = Clipboard.GetDataObject();
            if (dataObject == null) return null;
            return new ClipboardData(dataObject);
        }

        void SetClip(ClipboardData clipData)
        {
            try
            {
                Clipboard.Clear();
                foreach (var kvp in clipData.Objects)
                {
                    Clipboard.SetData(kvp.Key, kvp.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ReadReg(Keys key)
        {
            try
            {
                if (_registers.ContainsKey(key))
                {
                    MethodInvoker invoker = () => SetClip(_registers[key]);
                    Invoke(invoker);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
