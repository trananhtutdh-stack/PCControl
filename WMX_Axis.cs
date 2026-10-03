namespace AutoVision.Service.IPC
{
    public class WMX_Axis
    {
        protected readonly WMX _wmx;
        private readonly int _axisIndex;
        private IIpc ipc;
        private int index;

        public WMX_Axis(WMX wmx, int axisIndex, string name)
        {
            _wmx = wmx;
            _axisIndex = axisIndex;
            Name = name;
        }

        

        public DevAlarm DevAlarm
        {
            get
            {
                return null;
            }
        }

        public string Name { get; set; }

        public bool IsBusy
        {
            get
            {
                return false;
            }
        }

        public int ErrorCode
        {
            get
            {
                return 0;
            }
        }

        public bool IsHomeDone
        {
            get
            {
                return false;
            }
        }

        public bool IsMotionPause
        {
            get
            {
                return false;
            }
        }

        public bool IsServoON
        {
            get
            {
                return false;
            }
        }

        public double CurrentPos =>
            _wmx?.Motion.GetCurrentPos(_axisIndex) ?? 0;

        public double CurrentSpeed =>
            _wmx?.Motion.currentSpeed(_axisIndex) ?? 0;

        public bool ExternalNegativeLS
        {
            get
            {
                return false;
            }
        }

        public bool ExternalPositiveLS
        {
            get
            {
                return false;
            }
        }

        public bool HomeSW
        {
            get
            {
                return false;
            }
        }

        public double JogAcc { get; set; }

        public double JogDec { get; set; }

        public double PosAcc { get; set; }

        public double PosDec { get; set; }

        public double HomeSpeed { get; set; }

        public double CreepSpeed { get; set; }

        public double JogHSpeed { get; set; }

        public double JogMSpeed { get; set; }

        public double JogLSpeed { get; set; }

        public double JogSpeed { get; set; }

        public double JogLevel { get; set; }

        public void StatusJugger()
        {
        }

        public void AxisErrClear()
        {
            _wmx?.Motion.AxisErrClear(_axisIndex);
        }

        public void ServoOn()
        {
            _wmx?.Motion.ServoOn(_axisIndex);
        }

        public void ServoOff()
        {
            _wmx?.Motion.ServoOff(_axisIndex);
        }

        public void JOG_Positive()
        {
            _wmx?.Motion.JOG_Positive(JogSpeed, JogAcc, JogDec, _axisIndex);
        }

        public void JOG_Nagative()
        {
            _wmx?.Motion.JOG_Nagative(JogSpeed, JogAcc, JogDec, _axisIndex);
        }

        public void JOG_Positive(double jogSp)
        {
            _wmx?.Motion.JOG_Positive(jogSp, JogAcc, JogDec, _axisIndex);
        }

        public void JOG_Nagative(double jogSp)
        {
            _wmx?.Motion.JOG_Nagative(jogSp, JogAcc, JogDec, _axisIndex);
        }

        public void JOG_STOP()
        {
            _wmx?.Motion.JOG_STOP(_axisIndex);
        }

        public void Homing(double homeSp, double creepSp)
        {
            _wmx?.Motion.Homing(homeSp, creepSp, _axisIndex);
        }

        public void MoveAbs(double pos, double speed)
        {
            _wmx?.Motion.MoveAbs(_axisIndex, (int)speed, pos, PosAcc, PosDec);
        }

        public void MoveAbs(int pos)
        {
            MoveAbs(pos, JogSpeed);
        }

        public void MoveRel(double pos, double speed)
        {
            _wmx?.Motion.MoveRel(_axisIndex, (int)speed, pos, PosAcc, PosDec);
        }
    }
}
