using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoVision.Service.IPC
{
    public interface IMotion
    {
        public void SaveParameter();
        public void AxisErrClear(int idx);
        public void AmpErrClear(int idx);
        public double GetCurrentPos(int idx);
        public double currentSpeed(int idx);
        public void ServoOn(int idx);
        public void ServoOff(int idx);
        public void JOG_Positive(double speed, double Acc, double Dec, int idx);
        public void JOG_Nagative(double speed, double Acc, double Dec, int idx);
        public void JOG_STOP(int idx);
        public void Homing(double homeSp, double creepSp, int idx);
        public void MoveRel(int idx, int Speed, double Pos, double Acc, double Dec);
        public void MoveAbs(int idx, int Speed, double Pos, double Acc, double Dec);
        IAxis CreateAxis(int axisIndex);
    }
}
