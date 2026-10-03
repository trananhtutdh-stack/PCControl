using System;
using AutoVision.Service.IPC;
using PCControl.Service;
using PCControl.Service.DeviceManager.Actuator;

namespace PCControl.Machine
{
    public sealed class MachineDevices : IDisposable
    {
        public IIpc Ipc { get; }
        public Dictionary<string, Cylinder> Cylinders { get; } = new();
        public Dictionary<string, IAxis> Axis { get; } = new();

        public Dictionary<string, Tool> Tools { get; } = new();
        public MachineDevices(IIpcFactory ipcFactory)
        {
            Ipc = ipcFactory.Create(MAKER.WMX);

            Cylinders.Add("CYL_JIG_CH1", new Cylinder("CYL_JIG_CH1", "Y0", "Y1", "X0", "X1", Ipc));
            Cylinders.Add("CYL_JIG_CH2", new Cylinder("CYL_JIG_CH2", "Y2", "Y3", "X2", "X3", Ipc));
            Cylinders.Add("CYL_TRAY", new Cylinder("CYL_TRAY", "Y4", "Y5", "X4", "X5", Ipc));
            Cylinders.Add("CYL_TOOL1_CH1", new Cylinder("CYL_TOOL1_CH1", "Y6", "Y7", "X6", "X7", Ipc));
            Cylinders.Add("CYL_TOOL2_CH1", new Cylinder("CYL_TOOL2_CH1", "Y10", "Y11", "X10", "X11", Ipc));
            Cylinders.Add("CYL_TOOL3_CH1", new Cylinder("CYL_TOOL3_CH1", "Y12", "Y13", "X12", "X13", Ipc));
            Cylinders.Add("CYL_TOOL4_CH1", new Cylinder("CYL_TOOL4_CH1", "Y14", "Y15", "X14", "X15", Ipc));

            AddAxis("CH1_X_AXIS", 1);
            AddAxis("CH1_Y_AXIS", 2);
            AddAxis("CH1_Z_AXIS", 3);
            AddAxis("CH2_X_AXIS", 4);
            AddAxis("CH2_Y_AXIS", 5);
            AddAxis("CH2_Z_AXIS", 6);



        }

        private void AddAxis(string name,int axisNo)
        {
            IAxis axis =Ipc.Motion.CreateAxis(axisNo);
            axis.Name = name;
            Axis.Add(name, axis);
        }

        public void Dispose()
        {
            Ipc?.Dispose();
        }
    }
}
