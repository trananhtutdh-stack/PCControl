using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCControl.Core.IPC
{
    public class AxisStorage
    {
        public int AxisIndex { get; set; }
        public double HomeSpeed { get; set; }
        public double CreepSpeed { get; set; }
        public double JogSpeed { get; set; }
        public double JogHSpeed { get; set; }

        public double JogMSpeed { get; set; }
        public double JogLSpeed { get; set; }
        public double JogAcc { get; set; }
        public double JogDec { get; set; }
        public double JogLevel { get; set; }
        public double PosAcc { get; set; }
        public double PosDec { get; set; }
        public double Acceleration { get; set; }
        public double Deceleration { get; set; }
        public double MaxSpeed { get; set; }
        public double MinSpeed { get; set; }
        public double CurrentPosition { get; set; }
        public bool IsHomed { get; set; }
        public double Speed { get; set; }
        public double CurrentSpeed { get; }
        public AxisStorage(int axisIndex)
        {
            AxisIndex = axisIndex;
            HomeSpeed = 100; // Default home speed
            CreepSpeed = 10; // Default creep speed
            Acceleration = 50; // Default acceleration
            Deceleration = 50; // Default deceleration
            MaxSpeed = 200; // Default max speed
            MinSpeed = 5; // Default min speed
            CurrentPosition = 0; // Initial position
            IsHomed = false; // Initially not homed
            Speed = 1000; // Default speed
            CurrentSpeed = 1000;

        }
    }
}
