using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutoVision.Service.IPC
{

    public enum AxisState
    {
        Ready,
        Busy,
        Error,
        Unknown
    }
    public interface IAxis
    {
        public DevAlarm DevAlarm {  get; }
        string Name { get; set; }
        bool IsBusy {  get; }
        AxisState State { get; }
        bool CanMove { get; }
        double JogAcc { get; set; }
        double JogDec { get; set; }
        double PosAcc {  get; set; }
        double PosDec { get; set; }
        int ErrorCode {  get; }
        bool IsHomeDone { get; }
        bool IsMotionPause { get; }

        bool IsServoON {  get; }
        double CurrentPos { get; }
        double TargetPos { get; set; }
        double CurrentSpeed { get; }

        double HomeSpeed { get; }

        double CreepSpeed {  get; set; }
        double JogHSpeed { get; set; }
        double JogMSpeed { get; set; }
        double JogLSpeed { get; set; }
        double JogSpeed { get; set; }

        bool ExternalNegativeLS {  get; }
        bool ExternalPositiveLS {  get; }
        bool HomeSW {  get; }
        double JogLevel { get; set; }
        double Speed { get; set; }

        void StatusJugger();
        void AxisErrClear();
        void ServoOn();
        void ServoOff();

        void JOG_Positive();
        void JOG_Nagative();
        void JOG_Positive(double jogSp);
        void JOG_Nagative(double JogSp);
        void JOG_STOP();
        void Homing(double homeSp, double CreepSp);
        void MoveAbs(double Pos, double Speed);
        void MoveRel(double Pos, double Speed);
        void MoveAbs(int Pos);
    }
}
