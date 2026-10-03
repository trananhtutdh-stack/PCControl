

namespace AutoVision.Service.IPC.Factory
{
    public sealed class WmxIpcFactory : IIpcMakerFactory
    {
        public MAKER Maker => MAKER.WMX;

        public IIpc Create() => new WMX();
    }
}
