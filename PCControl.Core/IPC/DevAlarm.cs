using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVision.Service.IPC
{
    public enum AlarmLevels
    {
        None,
        Error,
        Warrning
    }
    public class DevAlarm
    {
        private readonly Func<bool> _condition;

        public AlarmLevels AlarmLevel { get; set; }

        public bool IsAlarm
        {
            get
            {
                try
                {
                    return _condition();
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }

        public string Message { get; set; }
        public string Solution { get; set; }

        public DevAlarm(Func<bool> condition)
        {
            _condition = condition;
        }
    }
}
