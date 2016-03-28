using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace Registers
{
    class Registers
    {
        readonly Dictionary<Keys, ClipboardData> _registers = new Dictionary<Keys, ClipboardData>();

        Keys _currentRegister = Keys.D1;

        readonly List<ClipboardData> queue = new List<ClipboardData>();

        private int queueInd;

        private bool isQueueMode;

        private Form ui;

        private KeyboardMonitor keymon;

        public Registers(Form ui)
        {
            this.ui = ui;
        }

        public void Start()
        {
            //// 221 == Bight Bracket
            //// 219 == Left Bracket
            //// 186 == Semicolon
            //// 192 == Single Quote
            //// 191 == Forward Slash
            //_combinations.Add(new Combination(QueueStart, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)186));
            //_combinations.Add(new Combination(QueueEnd, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)192));
            //_combinations.Add(new Combination(QueueNext, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)221));
            //_combinations.Add(new Combination(QueuePrev, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)219));
            //_combinations.Add(new Combination(QueueLines, true, Keys.LShiftKey, Keys.RShiftKey, (Keys)191));
            //_combinations.Add(new Combination(QueuePush, false, Keys.RControlKey, Keys.C));
            //_combinations.Add(new Combination(QueuePush, false, Keys.RControlKey, Keys.X));
            //_combinations.Add(new Combination(QueuePull, false, Keys.RControlKey, Keys.V));
            //_combinations.Add(new Combination(QueuePush, false, Keys.LControlKey, Keys.C));
            //_combinations.Add(new Combination(QueuePush, false, Keys.LControlKey, Keys.X));
            //_combinations.Add(new Combination(QueuePull, false, Keys.LControlKey, Keys.V));

            CombinationMonitor.Instance.Create(QueueLines, true, Keys.LShiftKey, Keys.RShiftKey, (Keys) 191);
            CombinationMonitor.Instance.Create(QueuePush, false, Keys.LControlKey, Keys.C);
            CombinationMonitor.Instance.Create(QueuePush, false, Keys.RControlKey, Keys.C);
            CombinationMonitor.Instance.Create(QueuePush, false, Keys.LControlKey, Keys.X);
            CombinationMonitor.Instance.Create(QueuePush, false, Keys.RControlKey, Keys.X);
            CombinationMonitor.Instance.Create(QueuePull, false, Keys.LControlKey, Keys.V);
            CombinationMonitor.Instance.Create(QueuePull, false, Keys.RControlKey, Keys.V);

            var regSwitchCombo = new WildcardKeyCombination(SwitchReg, true, Keys.LShiftKey, Keys.RShiftKey);
            CombinationMonitor.Instance.Combinations.Add(regSwitchCombo);
        }

        public void QueueLines()
        {
            queue.Clear();
            var clipData = GetClip();
            if (clipData != null && clipData.Objects.Any(kvp => kvp.Value is string))
            {
                var stringData = (string)clipData.Objects.First(kvp => kvp.Value is string).Value;
                var lines = stringData.Replace("\r\n", "\n").Split('\n');
                foreach (var line in lines)
                {
                    queue.Add(new ClipboardData(DataFormats.StringFormat, line));
                }
                queueInd = 0;
                isQueueMode = true;
            }
        }

        public void QueueStart()
        {
            queueInd = 0;
            isQueueMode = true;
        }

        public void QueueEnd()
        {
            isQueueMode = false;
        }

        private void QueueNext()
        {
            queueInd++;
        }

        private void QueuePrev()
        {
            queueInd = (queueInd + queue.Count - 1) % queue.Count;
        }

        private void QueuePush()
        {
            var timer = new Timer();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                if (!isQueueMode) return;
                if (queueInd == 0)
                {
                    queue.Clear();
                }
                var clipData = GetClip();
                queue.Add(clipData);
                queueInd++;
            };
            timer.Interval = 10;
            timer.Start();
        }

        private void QueuePull()
        {
            ui.Invoke(new MethodInvoker(() =>
                {
                    if (!isQueueMode) return;
                    if (queue.Count == 0) return;
                    if (queueInd >= queue.Count)
                    {
                        queueInd = queueInd % queue.Count;
                    }
                    SetClip(queue[queueInd]);
                    queueInd++;
                }));

        }

        public void SwitchReg(Keys matched)
        {
            isQueueMode = false;
            var newRegister = matched;

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
            ui.Invoke(invoker);
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
                    ui.Invoke(invoker);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
