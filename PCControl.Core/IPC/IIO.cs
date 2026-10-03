using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;

namespace AutoVision.Service.IPC
{
    public interface IIO
    {
        public bool ReadInput(int slave, int bit);
        public bool ReadInputManual(int slave, int bit);
        public bool ReadOutPutManual(int slave, int bit);
        public byte ReadInByteManual(int slave);
        public byte ReadOutByteManual(int slave);
        public bool ReadOutput(int slave, int bit);
        public void WriteOutput(int slave, int bit, bool value);
    }
}
