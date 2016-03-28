using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Registers.Properties;
using Utilities;

namespace Registers
{
    public partial class RegistersUI : Form
    {
        private Registers registers;

        public RegistersUI()
        {
            InitializeComponent();
            registers = new Registers(this);
            registers.Start();

            instructions_rtf.Rtf = Resources.Instructions;
        }

    }
}
