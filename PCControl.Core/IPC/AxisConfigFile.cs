using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCControl.Core.IPC
{
    public static class AxisConfigFile
    {
        private static readonly object _lock = new object();
        public static AxisStorage[] Data = new AxisStorage[50];
        public static string PathMain = "axisConfigMain.bin";
        public static bool IsLoading = false;

        static AxisConfigFile()
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = new AxisStorage(i);
            }
            Load();
        }

        public static void Load()
        {
            lock (_lock)
            {
                if (!File.Exists(PathMain))
                    return;

                IsLoading = true;

                try
                {
                    using var fs = new FileStream(
                        PathMain,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite);

                    using var br = new BinaryReader(fs);

                    foreach (var axis in Data)
                    {
                        axis.HomeSpeed = br.ReadDouble();
                        axis.CreepSpeed = br.ReadDouble();
                        axis.JogSpeed = br.ReadDouble();
                        axis.JogHSpeed = br.ReadDouble();
                        axis.JogMSpeed = br.ReadDouble();
                        axis.JogLSpeed = br.ReadDouble();
                        axis.JogAcc = br.ReadDouble();
                        axis.JogDec = br.ReadDouble();
                        
                        axis.PosAcc = br.ReadDouble();
                        axis.PosDec = br.ReadDouble();
                        //axis.Speed = br.ReadDouble();
                        
                    }
                }
                catch (EndOfStreamException)
                {
                    Console.WriteLine("Axis config file is incomplete.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading axis config: {ex.Message}");
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        public static void Save()
        {
            lock (_lock)
            {
                try
                {
                    using var fs = new FileStream(PathMain, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    using var bu = new BinaryWriter(fs);
                    foreach (var axis in Data)
                    {
                        bu.Write(axis.HomeSpeed);
                        bu.Write(axis.CreepSpeed);
                        bu.Write(axis.JogSpeed);
                        bu.Write(axis.JogHSpeed);
                        bu.Write(axis.JogMSpeed);
                        bu.Write(axis.JogLSpeed);
                        bu.Write(axis.JogAcc);
                        bu.Write(axis.JogDec);
                        
                        bu.Write(axis.PosAcc);
                        bu.Write(axis.PosDec);
                   

                        //for(int i = 0; i < 50; i++)
                        //{
                        //    bu.Write(Data[i].CurrentPosition);
                        //    bu.Write(Data[i].Speed);
                        //    bu.Write(Data[i].HomeSpeed);
                        //    bu.Write(Data[i].CreepSpeed);
                        //    bu.Write(Data[i].JogSpeed);
                        //    bu.Write(Data[i].JogHSpeed);
                        //    bu.Write(Data[i].JogMSpeed);
                        //    bu.Write(Data[i].JogLSpeed);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving axis config: {ex.Message}");
                }
            }
        }


    }
}
