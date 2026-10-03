using WMX3ApiCLR;
using wmx3Api;

namespace AutoVision.Service.IPC
{
    public class WMX : IIpc
    {
        public EngineStatus State = new EngineStatus();
        public IIO IO { get; private set; }
        public IMotion Motion { get; private set; }
        private WMX3Api Api { get; }
        private bool _disposed;

        public WMX()
        {
            Api = new WMX3Api();
        }

        public MAKER Maker => MAKER.WMX;

        public bool Initialize()
        {
            try
            {
                Api.CreateDevice(
                    "C:\\Program Files\\SoftServo\\WMX3\\",
                    DeviceType.DeviceTypeNormal,
                    0xFFFFFFFF);
                Api.SetDeviceName("PcControl");

                for (int i = 0; i < 100; i++)
                {
                    Api.StartCommunication(0xFFFFFFFF);
                    Api.GetEngineStatus(ref State);
                    if (State.State == EngineState.Communicating)
                    {
                        IO = (IIO)new WMX_IO(Api);
                        Motion = (IMotion)new WMX_Motion(Api);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                if (IO is IDisposable io)
                    io.Dispose();
            }
            catch (Exception ex)
            {
            }

            IO = null!;
            Motion = null!;

            try
            {
                Api.StopCommunication(0xFFFFFFFF);
                Api.CloseDevice();
                Api.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
