using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LateNightStupidities.IIImaBackupViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string TempPath => Path.Combine(Path.GetTempPath(), "LateNightStupidities.3maBackupTools");

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            try
            {
                if (Directory.Exists(App.TempPath))
                {
                    Directory.Delete(App.TempPath, true);
                }
            }
            catch
            {
                // As the app is exiting, do not do anything.
            }
        }
    }
}
