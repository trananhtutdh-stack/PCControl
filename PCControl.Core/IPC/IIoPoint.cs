using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVision.Service.IPC
{
    public interface IIoPoint
    {
        public bool Value {  get; set; }
        public (int a, int b) Point { get; }
        public IoModes IoMode { get; set; }
        string AdressName { get; set; }
    }
}
