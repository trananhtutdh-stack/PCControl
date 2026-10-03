using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCControl.Core.IPC
{
    public class PosSetting
    {
        public double Position { get; set; } = 100;
        public double Speed { get; set; } = 100;
        public double TimeOut { get; set; } = 10000;
        public double Acceleration { get; set; } = 50000;
        public double Deceleration { get; set; } = 50000;

        public void CopyFrom(PosSetting source)
        {
            Position = source.Position;
            Speed = source.Speed;
            TimeOut = source.TimeOut;
            Acceleration = source.Acceleration;
            Deceleration = source.Deceleration;
        }
    }
}
