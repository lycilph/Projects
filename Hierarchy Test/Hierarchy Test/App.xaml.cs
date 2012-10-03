using System.Windows;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Hierarchy_Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            LogSetup.Default();
            log.Debug("Started");

            MainWindow main_window = new MainWindow();
            main_window.Show();
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            log.Debug("Shutdown");
        }

    }
}
