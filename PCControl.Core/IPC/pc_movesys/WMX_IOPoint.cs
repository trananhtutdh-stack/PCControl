using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;

namespace AutoVision.Service.IPC
{
    public enum IoModes
    {
        input = 1,
        output = 2
    }
    public class WMX_IOPoint : IIoPoint
    {
        protected readonly WMX _wmx;

        private int slave;
        private int bit;

        public IoModes IoMode { get; set; }

        public bool Value
        {
            get
            {
                if (IoMode == IoModes.input)
                {
                    return _wmx.IO.ReadInput(slave, bit);
                }
                return _wmx.IO.ReadOutput(slave, bit);
            }
            set
            {
                VerifyIoPoint(IoModes.output);
                _wmx.IO.WriteOutput(slave, bit, value);
            }
        }

        public (int a, int b) Point
        {
            get { return (slave, bit); }
        }

        public int Slave => slave;
        public int Bit => bit;
        public string AdressName
        {
            get; set;
        }
        public WMX_IOPoint(WMX wmx, int slave, int bit, IoModes iomodels, string adressName)
        {
            _wmx = wmx;
            IoMode = iomodels;
            this.slave = slave;
            this.bit = bit;
            AdressName = adressName;
        }
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyIoPoint(IoModes ioMode)
        {
            if (IoMode != ioMode)
            {
                throw new InvalidOperationException(string.Format("mode {0} mode {1} không hợp lệ：Board:{2} Index:{3}", ioMode, IoMode, slave, bit));
            }
        }

    }
}
