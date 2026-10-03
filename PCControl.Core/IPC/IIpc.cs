using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVision.Service.IPC
{
    public enum MAKER
    {
        WMX = 0,
        ZMC = 1,
        SIM = 2,
    }
    public interface IIpc : IDisposable
    {
        bool Initialize();
        MAKER Maker { get; }
        IIO IO{ get; }
        IMotion Motion { get; }
    }
}
