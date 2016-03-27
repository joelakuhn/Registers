using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registers
{
    class ClipboardData
    {
        public Dictionary<string, object> Objects = new Dictionary<string, object>();

        public ClipboardData(IDataObject dataObject)
        {
            var dataFormats = dataObject.GetFormats();
            foreach (var format in dataFormats)
            {
                var specificObject = dataObject.GetData(format);
                Objects.Add(format, specificObject);
            }
        }

        public ClipboardData(string format, object value)
        {
            Objects.Add(format, value);
        }
    }
}
