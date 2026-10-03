using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using AutoVision.Service.IPC;
using Microsoft.Extensions.DependencyInjection;
using PCControl.Composition;
using PCControl.Machine;
using PCControl.Sequence;
using PCControl.Service.Config;
using PCControl.Service.Navigation;
using ShareLibrary.Localizations;


namespace PCControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;
        //public static IAppNavigationService AppNavigate { get; set; }
        //public static MachineDevices Devices { get; set; }
        //public static SeqThread seqThread;
        //public static Config Config { get; set; }

        
        //static App()
        //{
        //    //AppNavigate = new AutoNavigationService();
        //    //Devices = new MachineDevices(new IpcFactory());
        //    //seqThread = new SeqThread();
        //    //Config = new Config();
        //}
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            services.RegisterPcControlServices();
            Services = services.BuildServiceProvider();
            LanguageManager.LoadLanguage("en");
          
            
            //Config.appSettingJson.CurrentCultureName = "zh";
            //Config.SaveAppSettingJson();
            //Config.SaveRecipeSettingJson();
            Services.GetRequiredService<MainWindow>().Show();
            Services.GetRequiredService<SeqThread>();
            Services.GetRequiredService<Config>();
        }
    }
}
