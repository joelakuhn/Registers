using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registers
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();
        public static void ShowConsoleWindow()
        {
            AllocConsole();
        }


        static Mutex _m;
        private const string MUTEX_NAME = "Registers is running - e9811cb8-23c7-4e9e-9954-7ffcc99781aa";

        static bool IsAlreadyRunning()
        {
            try
            {
                Mutex.OpenExisting(MUTEX_NAME);
            }
            catch
            {
                _m = new Mutex(true, MUTEX_NAME);
                return false;
            }
            return true;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsAlreadyRunning()) return;
            //ShowConsoleWindow();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RegistersUI());
        }
    }
}
