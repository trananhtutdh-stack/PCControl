using System;

namespace AutoVision.Service.IPC.Factory
{
    public sealed class ZmcIpcFactory : IIpcMakerFactory
    {
        public MAKER Maker => MAKER.ZMC;

        public IIpc Create() =>
            throw new NotSupportedException("ZMC IPC is not implemented yet.");
    }
}
