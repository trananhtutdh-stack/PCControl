using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoVision.Service.IPC;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PCControl.Service.DeviceManager.Actuator
{
    public class Tool : ObservableObject
    {
        private string name;
        private IIoPoint vaccum;
        private IIoPoint blow;
        private IIoPoint vaccumSS;
        private IIoPoint blowSS;
        private int index;

        private bool isVacuumOld = false;
        private bool isBlowOld = false;

        public string Name => name;
        public bool IsVaccumSS => vaccumSS.Value;
        public bool IsBlowSS => blowSS.Value;
        public bool IsVacuum => vaccum.Value;
        public bool IsBlow => blow.Value;
        public int Index => index;

        public void Vaccum()
        {
            blow.Value = false;
            vaccum.Value = true;
            OnPropertyChanged(nameof(IsVacuum));
            OnPropertyChanged(nameof(IsBlow));

        }

        public void Blow()
        {
            vaccum.Value = false;
            blow.Value = true;
            OnPropertyChanged(nameof(IsVacuum));
            OnPropertyChanged(nameof(IsBlow));
        }

        public void Off()
        {
            vaccum.Value = false;
            blow.Value = false;
            OnPropertyChanged(nameof(IsVacuum));
            OnPropertyChanged(nameof(IsBlow));
        }

        public Tool(IIoPoint vaccumSS, IIoPoint blowSS, IIoPoint vaccum, IIoPoint blow, string name, int index)
        {
            this.vaccumSS = vaccumSS;
            this.blowSS = blowSS;
            this.vaccum = vaccum;
            this.blow = blow;
            this.name = name;
            this.index = index;
        }

        public void StatusJugger()
        {
            if(vaccumSS.Value != isVacuumOld)
            {
                
                OnPropertyChanged(nameof(IsVaccumSS));
                isVacuumOld = vaccumSS.Value;
            }
            if (blowSS.Value != isBlowOld)
            {
                OnPropertyChanged(nameof(IsBlowSS));
                isBlowOld = blowSS.Value;
            }
        }
    }
}
