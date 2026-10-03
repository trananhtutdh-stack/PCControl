using System;
using System.ComponentModel;
using System.Threading;
using WMX3ApiCLR;

namespace AutoVision.Service.IPC
{
    public class WMX_IO : IIO, IDisposable
    {
        private readonly WMX3Api _api;
        private readonly Io _io;
        
        private readonly bool[] xVals = new bool[160];
        private readonly bool[] yVals = new bool[160];

        private readonly object _lock = new object();
        private volatile bool _running = true;
        private Thread? _scanThread;
        private CancellationTokenSource? _cs = new CancellationTokenSource();
        private bool _disposed;
        
        public WMX_IO(WMX3Api api)
        {
            _api = api;
            _io = new Io(_api); 

            // init
            for (int i = 0; i < 160; i++)
            {
                xVals[i] = false;
                yVals[i] = false;
            }

            _scanThread = new Thread(() => ScanLoop(_cs.Token));
            _scanThread.IsBackground = true;
            _scanThread.Start();
        }


        private int Index(int slave, int bit) => slave * 16 + bit;

        public bool ReadInput(int slave, int bit)
        {
            int idx = Index(slave, bit);
            lock (_lock) return xVals[idx];
        }
        public bool ReadInputManual(int slave, int bit)
        {
            byte v = 0;
            _io.GetInBit(slave, bit, ref v);
            bool val = v != 0;
            return val;              
        }
        public bool ReadOutPutManual(int slave, int bit)
        {
            byte v = 0;
            _io.GetOutBit(slave, bit, ref v);
            bool val = v != 0;
            return val;
        }

        public byte ReadInByteManual(int slave)
        {
            byte v = 0;
            _io.GetInByte(slave, ref v);
            return v;
        }
        public byte ReadOutByteManual(int slave)
        {
            byte v = 0;
            _io.GetOutByte(slave, ref v);
            return v;
        }

        public bool ReadOutput(int slave, int bit)
        {
            int idx = Index(slave, bit);
            lock (_lock) return yVals[idx];
        }

        public void WriteOutput(int slave, int bit, bool value)
        {
            byte v = (byte)(value ? 1 : 0);
            _io.SetOutBit((byte)slave, (byte)bit, v); 
            int idx = Index(slave, bit);
            lock (_lock)
            {
                if (yVals[idx] != value)
                {
                    yVals[idx] = value;
                }
            }
        }

        private void ScanLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                 EngineStatus State = new EngineStatus();
                 _api.GetEngineStatus(ref State);
                if(State.State == EngineState.Communicating)
                {
                    for (int slave = 0; slave < 10; slave++)
                    {
                        for (int bit = 0; bit < 16; bit++)
                        {
                            byte v = 0;
                            try { _io.GetInBit(slave, bit, ref v); }
                            catch { }

                            bool val = v != 0;
                            int idx = Index(slave, bit);
                            lock (_lock)
                            {
                                if (xVals[idx] != val)
                                {
                                    xVals[idx] = val;
                                }
                            }
                            byte v1 = 0;
                            try { _io.GetOutBit(slave, bit, ref v1); }
                            catch { }

                            bool val1 = v1 != 0;
                            int idx1 = Index(slave, bit);

                            lock (_lock)
                            {
                                if (yVals[idx1] != val1)
                                {
                                    yVals[idx1] = val1;
                                }
                            }
                        }
                    }
                }                
                Thread.Sleep(10);
            }
        }   
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _running = false;

            try
            {
                _cs?.Cancel();
            }
            catch
            {
            }

            try
            {
                if (_scanThread?.IsAlive == true)
                    _scanThread.Join(TimeSpan.FromSeconds(2));
            }
            catch
            {
            }

            try
            {
                _cs?.Dispose();
            }
            catch
            {
            }

            _cs = null;
            _scanThread = null;
        }
    }
}