using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoVision.Service.IPC;
using CommunityToolkit.Mvvm.ComponentModel;
using PCControl.Service.Config;

namespace PCControl.Service.DeviceManager.Actuator
{
    public partial class TRAY: ObservableObject
    {
        [ObservableProperty] private int index;
        [ObservableProperty] private int status;
        [ObservableProperty] private string isTarget;


    }

    public partial class TRAY : ObservableObject
    {
        [ObservableProperty] private PANEL panel;
        [ObservableProperty] private int tool;
        [ObservableProperty] private int targetPos;
        private readonly IIoPoint trayDectSensor;
        public bool IsEmpty { get; private set; }
        public bool IsTrayDect => trayDectSensor.Value;
        //public List<PosData> PosDataCh1 { get; } = new();
        //public List<PosData> PosDataCh2 { get; } = new();
        //public Dictionary<int, PosIndex> Tray_Index { get; private set; } = new();
        public ObservableCollection<TRAY> Tray_Status { get; private set; } = new();
        private int GroupCount => Tool > 0 ? Tray_Status.Count / Tool : 0;
        public TRAY(PANEL panel,IIoPoint trayDectSensor, int tool)
        {
            Panel = panel;
            this.trayDectSensor = trayDectSensor;
            Tool = tool;
            TargetPos = 0;
            //RebuildIndex();
            //rebuildStatus();
            IsEmpty = false;
            panel.PropertyChanged += Panel_PropertyChanged;

        }

        //partial void OnTargetPosChanged(int value) => ApplyTargetHighLight(value);

        private void Panel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) 
        {
            UpdateTray();
        }

        private void UpdateTray()
        {
            int saveGroup = TargetPos;
            //RebuildIndex();
            //rebuildStatus(true);
            IsEmpty = false;
            //OnPanelChanged(nameof(Tray_Index));
            TargetPos = 0;
            if (saveGroup > 0)
            {
                TargetPos = saveGroup;
            }
        }
        public void ClearData()
        {

            foreach (var item in Tray_Status) 
            {
                //item.Status = true;
            }
            IsEmpty = false;
        }
        private (int start, int end) GetGroupRange(int groupIndex)
        {
            int start = (groupIndex - 1) * Tool + 1;
            int end = start + Tool - 1;
            return (start, end);
        }
        public void SellectGroup(int cellIndex)
        {
            if (Tray_Status == null || cellIndex < 1 || cellIndex > Tray_Status.Count)
            {
                return;
            }
            TargetPos = (cellIndex - 1) / Tool + 1;
        }

        public void CreatePos(double posX1st, double posY1st, double posXSpeed, double posYSpeed, int ch)
        {
            //var posDatas = ch == 1 ? PosDataCh1 : PosDataCh2;
            //posDatas.Clear();
            //int groupCols = Tool > 0 ? Panel.Col / Tool : 0;
            //for (int row = 8; row < Panel.Col; row++)
            //{
            //    for (int col = 0; col < groupCols; col++) 
            //    {
            //        double x = posX1st + col + Panel.PitchX;
            //        double y = posY1st + col + Panel.PitchY;
            //        posDatas.Add(new PosData(x, y,posXSpeed,posYSpeed));
            //    }
            //}
        }

        //public PosData FindPos(int groupindex, int ch)
        //{

        //}
    }
}
