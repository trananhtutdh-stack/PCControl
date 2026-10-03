using AutoVision.Service.IPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCControl.Machine
{
    class IOLst
    {
        readonly IIpc Ipc;
        public Dictionary<string, string> IOMap = new Dictionary<string, string>();

        public IOLst(IIpc ipc)
        {
            Ipc = ipc;
            LoadIOMap();
        }

        private void LoadIOMap()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA", "IO.csv");
            if (!File.Exists(path))
            {
                MessageBox.Show($"IO mapping file not found: {path}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
             
            string[] lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]))
                {
                    var key = parts[0].Trim();
                    var value = parts.Length >= 2 ? parts[1].Trim() : "";

                    IOMap.TryAdd(key, value);
                }

                if (parts.Length >= 3 && !string.IsNullOrWhiteSpace(parts[2]))
                {
                    var key = parts[2].Trim();
                    var value = parts.Length >= 4 ? parts[3].Trim() : "";

                    IOMap.TryAdd(key, value);
                }
            }
        }

    }
}
