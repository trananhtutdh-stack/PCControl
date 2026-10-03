using AutoVision.Service.IPC.Factory;

namespace AutoVision.Service.IPC
{
    public sealed class IpcFactory : IIpcFactory
    {

        public IpcFactory()
        {
        }

        public IIpc Create(MAKER maker)
        {
            var ipc = CreateByMaker(maker);
            ipc.Initialize();
            return ipc;
        }

        private IIpc CreateByMaker(MAKER maker) => maker switch
        {
            MAKER.WMX => new WmxIpcFactory().Create(),
            MAKER.ZMC => new ZmcIpcFactory().Create(),
            _ => throw new ArgumentOutOfRangeException(nameof(maker), maker, "Unsupported IPC maker."),
        };
    }
}
