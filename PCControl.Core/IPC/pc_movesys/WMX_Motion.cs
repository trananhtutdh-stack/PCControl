using WMX3ApiCLR;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCControl.Core.IPC;

namespace AutoVision.Service.IPC
{
    public class WMX_Motion : IMotion
    {
        private readonly WMX3Api _api;
        private readonly CoreMotion _cm;
        private CoreMotionStatus _status = new CoreMotionStatus();

        public WMX_Motion(WMX3Api api)
        {
            _api = api;
            _cm = new CoreMotion(api);
            setConfig();
        }

        //public CoreMotionStatus GetStatus()
        //{
        //    _cm.GetStatus(ref _status);

        //    return _status;
        //}

        public IAxis CreateAxis(int axisIndex)
        {
            return new WMXAxis(axisIndex, _cm);
        }
        private bool setConfig()
        {
            CoreMotion cm = new CoreMotion(_api);

            int ret = cm.Config.ImportAndSetAll(@"D:\wmx_parameters.xml");
            if (ret != ErrorCode.None)
            {
                return false;
            }
            return true;
        }

        public void SaveParameter()
        {

        }
        public void AxisErrClear(int idx) => _cm.AxisControl.ClearAxisAlarm(idx);
        public void AmpErrClear(int idx) => _cm.AxisControl.ClearAmpAlarm(idx);
        public double GetCurrentPos(int idx)
        {
            double curPos = 0;
            _cm.AxisControl.GetPosFeedback(idx, ref curPos);
            return curPos;
        }
        public double currentSpeed(int idx)
        {
            double curSpeed = 0;
            _cm.AxisControl.GetVelFeedback(idx, ref curSpeed);
            return curSpeed;

        }
        public void ServoOn(int idx) => _cm.AxisControl.SetServoOn(idx, 1);
        public void ServoOff(int idx) => _cm.AxisControl.SetServoOn(idx, 0);
        public void JOG_Positive(double speed, double Acc, double Dec, int idx)
        {
            Motion.JogCommand jogCommand = new Motion.JogCommand();
            jogCommand.Profile.Type = WMX3ApiCLR.ProfileType.Trapezoidal;
            jogCommand.Axis = idx;
            jogCommand.Profile.Velocity = speed;
            jogCommand.Profile.Acc = Acc;
            jogCommand.Profile.Dec = Dec;
            _cm.Motion.StartJog(jogCommand);
        }
        public void JOG_Nagative(double speed, double Acc, double Dec, int idx)
        {
            Motion.JogCommand jogCommand = new Motion.JogCommand();
            jogCommand.Profile.Type = WMX3ApiCLR.ProfileType.Trapezoidal;
            jogCommand.Axis = idx;
            jogCommand.Profile.Velocity = -speed;
            jogCommand.Profile.Acc = Acc;
            jogCommand.Profile.Dec = Dec;
            _cm.Motion.StartJog(jogCommand);
        }
        public void JOG_STOP(int idx) => _cm.Motion.Stop(idx);
        public void Homing(double homeSp, double creepSp, int idx)
        {
            WMX3ApiCLR.Config.HomeParam homePara = new WMX3ApiCLR.Config.HomeParam();
            homePara.HomeType = WMX3ApiCLR.Config.HomeType.CurrentPos;
            homePara.HomeDirection = WMX3ApiCLR.Config.HomeDirection.Positive;
            homePara.HomingVelocityFast = homeSp;
            homePara.HomingVelocitySlow = creepSp;
            _cm.Config.SetHomeParam(idx, homePara);
            _cm.Home.StartHome(idx);
        }
        public void MoveAbs(int idx, int Speed, double Pos, double Acc, double Dec)
        {
            Motion.PosCommand cmd = new Motion.PosCommand();
            cmd.Axis = idx;
            cmd.Target = Pos;
            cmd.Profile.Type = ProfileType.Trapezoidal;
            cmd.Profile.Velocity = Speed;
            cmd.Profile.Acc = Acc;
            cmd.Profile.Dec = Dec;
            _cm.Motion.StartMov(cmd);
        }
        public void MoveRel(int idx, int Speed, double Pos, double Acc, double Dec)
        {
            Motion.PosCommand cmd = new Motion.PosCommand();
            cmd.Axis = idx;
            cmd.Target = Pos;
            cmd.Profile.Type = ProfileType.Trapezoidal;
            cmd.Profile.Velocity = Speed;
            cmd.Profile.Acc = Acc;
            cmd.Profile.Dec = Dec;
            _cm.Motion.StartPos(cmd);
        }

    }

    public class WMXAxis : ObservableObject, IAxis
    {
        private readonly int _axisIndex;
        private readonly CoreMotion _cm;
        private AxisStorage _store;
        //private readonly Stopwatch _watchPos = new Stopwatch.StartNew();

        //private readonly System.Diagnostics.Stopwatch _watchPos = System.Diagnostics.Stopwatch.StartNew();
        public string Language { get; set; } = "";

        public WMXAxis(int axisIndex, CoreMotion cm)
        {
            _axisIndex = axisIndex;
            _cm = cm;
            _store = AxisConfigFile.Data[axisIndex];
        }

        public CoreMotionAxisStatus Status
        {
            get
            {
                CoreMotionStatus status = new CoreMotionStatus();
                _cm.GetStatus(ref status);
                return status.AxesStatus[_axisIndex];
            }
        }

        public void StatusJugger()
        {
            OnPropertyChanged(nameof(Status));

            // Position / Speed
            OnPropertyChanged(nameof(CurrentPos));
            OnPropertyChanged(nameof(CurrentSpeed));

            // Motion state
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsHomeDone));
            OnPropertyChanged(nameof(IsMotionPause));
            OnPropertyChanged(nameof(CanMove));
            OnPropertyChanged(nameof(State));

            // Servo
            OnPropertyChanged(nameof(IsServoON));

            // Alarm
            OnPropertyChanged(nameof(ErrorCode));
            OnPropertyChanged(nameof(DevAlarm));

            // Limit / Home switch
            OnPropertyChanged(nameof(ExternalPositiveLS));
            OnPropertyChanged(nameof(ExternalNegativeLS));
            OnPropertyChanged(nameof(HomeSW));

        }

        public DevAlarm DevAlarm
        {
            get
            {
                DevAlarm al = null;
                if ((ErrorCode != 0))
                {
                    al = new DevAlarm(() => true)
                    {
                        AlarmLevel = AlarmLevels.Error,
                        Message = (Language == "en") ? String.Format("Axis {0} Error Code: {1}", _axisIndex, ErrorCode) : String.Format("轴 {0} 错误代码: {1}", _axisIndex, ErrorCode),
                        Solution = (Language == "en") ? "Please check the axis and clear the error." : "请检查轴并清除错误。"
                    };
                }
                if(PosNoComple())
                {
                    al = new DevAlarm(() => true)
                    {
                        AlarmLevel = AlarmLevels.Error,
                        Message = (Language == "en") ? String.Format("Axis {0} Position Not Complete", _axisIndex) : String.Format("轴 {0} 位置未完成", _axisIndex),
                        Solution = (Language == "en") ? "Please check the axis position." : "请检查轴位置。"
                    };
                }
                return al;

            }
        }

        private bool PosNoComple()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public bool IsBusy => Status.OpState != OperationState.Idle;
        public AxisState State
        {
            get
            {
                if (ErrorCode != 0)
                    return AxisState.Error;

                if (IsBusy)
                    return AxisState.Busy;
                if(!IsBusy && ErrorCode == 0 && IsServoON)
                    return AxisState.Ready;
                return AxisState.Unknown;
            }
        }
        public bool CanMove
        {
            get
            {
                return IsServoON
                       && !IsBusy
                       && !ExternalPositiveLS
                       && !ExternalNegativeLS
                       && ErrorCode == 0;
            }
        }

        public double JogAcc
        {
            get => _store.JogAcc;
            set
            {
                if ((value != _store.JogAcc))
                {
                    _store.JogAcc = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(JogAcc));
                    
                }
                
            }
        }
        public double JogDec
        {
            get => _store.JogDec; 
            set { 
                if ((value != _store.JogDec)) 
                { 
                    _store.JogDec = value; 
                    AxisConfigFile.Save(); 
                    OnPropertyChanged(nameof(JogDec)); 
                } 
            }
        }
        public double PosAcc
        {
            get => _store.PosAcc;
            set
            {
                if ((value != _store.PosAcc))
                {
                    _store.PosAcc = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(PosAcc));
                }
            }
        }
        public double PosDec
        {
            get => _store.PosDec;
            set
            {
                if ((value != _store.PosDec))
                {
                    _store.PosDec = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(PosDec));
                }
            }
        }
        public int ErrorCode => Status.AmpAlarmCode;
        public bool IsHomeDone => Status.HomeDone&&(Status.ActualPos==0);
        public bool IsMotionPause => Status.MotionPaused;

        public bool IsServoON => Status.ServoOn;
        public double CurrentPos => Status.ActualPos;
        public double CurrentSpeed => Status.ActualVelocity;

        public double HomeSpeed => Status.ActualVelocity;
        public double CreepSpeed
        {
            get => _store.CreepSpeed;
            set
            {
                if ((value != _store.CreepSpeed))
                {
                    _store.CreepSpeed = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(CreepSpeed));
                }
            }
        }
        public double JogHSpeed
        {
            get => _store.JogHSpeed;
            set
            {
                if ((value != _store.JogHSpeed))
                {
                    _store.JogHSpeed = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(JogHSpeed));
                }
            }
        }
        public double JogMSpeed
        {
            get => _store.JogMSpeed;
            set
            {
                if ((value != _store.JogMSpeed))
                {
                    _store.JogMSpeed = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(JogMSpeed));
                }
            }
        }
        public double JogLSpeed
        {
            get => _store.JogLSpeed;
            set
            {
                if ((value != _store.JogLSpeed))
                {
                    _store.JogLSpeed = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(JogLSpeed));
                }
            }
        }
        public double JogSpeed
        {
            get => _store.JogSpeed;
            set
            {
                if ((value != _store.JogSpeed))
                {
                    _store.JogSpeed = value;
                    AxisConfigFile.Save();
                    OnPropertyChanged(nameof(JogSpeed));
                }
            }
        }

        public bool ExternalNegativeLS => Status.ExternalNegativeLS;
        public bool ExternalPositiveLS => Status.ExternalPositiveLS;
        public bool HomeSW => Status.HomeSwitch;
        private double _jogLevel;
        public double JogLevel
        {
            get => _jogLevel;
            set
            {
                if (_jogLevel != value)
                {
                    _jogLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _targetPos;

        public double TargetPos
        {
            get => _targetPos;
            set
            {
                if (_targetPos != value)
                {
                    _targetPos = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Speed { get; set; }


        public void AxisErrClear()
        {
            _cm.AxisControl.ClearAxisAlarm(_axisIndex);
        }

        public void AmpErrClear()
        {
            _cm.AxisControl.ClearAmpAlarm(_axisIndex);
        }

        public void Homing(double homeSp, double CreepSp)
        {
            WMX3ApiCLR.Config.HomeParam homePara = new WMX3ApiCLR.Config.HomeParam();
            homePara.HomeType = WMX3ApiCLR.Config.HomeType.ZPulse;
            homePara.HomeDirection = WMX3ApiCLR.Config.HomeDirection.Positive;
            homePara.HomingVelocityFast = homeSp;
            homePara.HomingVelocitySlow = CreepSp;
            _cm.Config.SetHomeParam(_axisIndex, homePara);
            _cm.Home.StartHome(_axisIndex);
        }

        public void JOG_Nagative()
        {
            if(!IsServoON) return;
            Motion.JogCommand jogCommand = new Motion.JogCommand();
            jogCommand.Profile.Type = ProfileType.Trapezoidal;
            jogCommand.Axis = _axisIndex;
            jogCommand.Profile.Velocity = -JogSpeed;
            jogCommand.Profile.Acc = JogAcc;
            jogCommand.Profile.Dec = JogDec;
            _cm.Motion.StartJog(jogCommand);
        }

        public void JOG_Nagative(double JogSp)
        {
            if (!IsServoON) return;
            Motion.JogCommand jogCommand = new Motion.JogCommand();
            jogCommand.Profile.Type = ProfileType.Trapezoidal;
            jogCommand.Axis = _axisIndex;
            jogCommand.Profile.Velocity = -JogSp;
            jogCommand.Profile.Acc = JogAcc;
            jogCommand.Profile.Dec = JogDec;
            _cm.Motion.StartJog(jogCommand);
        }

        public void JOG_Positive()
        {
            if (!IsServoON) return;
            Motion.JogCommand jogCommand = new Motion.JogCommand();
            jogCommand.Profile.Type = ProfileType.Trapezoidal;
            jogCommand.Axis = _axisIndex;
            jogCommand.Profile.Velocity = JogSpeed;
            jogCommand.Profile.Acc = JogAcc;
            jogCommand.Profile.Dec = JogDec;
            _cm.Motion.StartJog(jogCommand);
        }

        public void JOG_Positive(double jogSp)
        {
            if (!IsServoON) return;
            Motion.JogCommand jogCommand = new Motion.JogCommand();
            jogCommand.Profile.Type = ProfileType.Trapezoidal;
            jogCommand.Axis = _axisIndex;
            jogCommand.Profile.Velocity = jogSp;
            jogCommand.Profile.Acc = JogAcc;
            jogCommand.Profile.Dec = JogDec;
            _cm.Motion.StartJog(jogCommand);
        }

        public void JOG_STOP()
        {
            if(Status.OpState == OperationState.Jog||Status.OpState == OperationState.Pos)
            {
                _cm.Motion.Stop(_axisIndex);
            }
        }

        public void MoveAbs(double Pos, double Speed)
        {
            Motion.PosCommand cmd = new Motion.PosCommand();
            cmd.Axis = _axisIndex;
            cmd.Target = Pos;
            cmd.Profile.Type = ProfileType.Trapezoidal;
            cmd.Profile.Velocity = Speed;
            cmd.Profile.Acc = PosAcc;
            cmd.Profile.Dec = PosDec;
            _cm.Motion.StartPos(cmd);
        }

        public void MoveAbs(int Pos)
        {
           Motion.PosCommand cmd = new Motion.PosCommand();
            cmd.Axis = _axisIndex;
            cmd.Target = Pos;
            cmd.Profile.Type = ProfileType.Trapezoidal;
            cmd.Profile.Velocity = JogSpeed;
            cmd.Profile.Acc = PosAcc;
            cmd.Profile.Dec = PosDec;
            _cm.Motion.StartPos(cmd);
        }

        public void MoveRel(double Pos, double Speed)
        {
            Motion.PosCommand cmd = new Motion.PosCommand();
            cmd.Axis = _axisIndex;
            cmd.Target = Pos;
            cmd.Profile.Type = ProfileType.Trapezoidal;
            cmd.Profile.Velocity = Speed;
            cmd.Profile.Acc = PosAcc;
            cmd.Profile.Dec = PosDec;
            
            _cm.Motion.StartMov(cmd);
        }

        public void MoveRel(int Pos)
        {
            Motion.PosCommand cmd = new Motion.PosCommand();
            cmd.Axis = _axisIndex;
            cmd.Target = Pos;
            cmd.Profile.Type = ProfileType.Trapezoidal;
            cmd.Profile.Velocity = JogSpeed;
            cmd.Profile.Acc = PosAcc;
            cmd.Profile.Dec = PosDec;

            _cm.Motion.StartMov(cmd);
        }

        public void ServoOff()
        {
            _cm.AxisControl.SetServoOn(_axisIndex, 0);
        }

        public void ServoOn()
        {
            _cm.AxisControl.SetServoOn(_axisIndex, 1);
        }

    }
}
