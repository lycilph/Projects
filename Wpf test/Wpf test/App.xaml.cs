using System.Windows;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Wpf_test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            SetupLogging();

            MainWindow main_window = new MainWindow();
            main_window.Show();
        }

        private void SetupLogging()
        {
            // Step 1. Create configuration object 
            LoggingConfiguration configuration = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            FileTarget fileTarget = new FileTarget();
            configuration.AddTarget("file", fileTarget);

            NLogViewerTarget log_viewer_target = new NLogViewerTarget();
            log_viewer_target.Name = "viewer";
            log_viewer_target.Address = "udp://127.0.0.1:9999";
            configuration.AddTarget("viewer", log_viewer_target);

            // Step 3. Set target properties 
            fileTarget.FileName = "${basedir}/file.txt";
            fileTarget.Layout = "${date} ${level} ${message}";

            // Step 4. Define rules
            LoggingRule rule1 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            configuration.LoggingRules.Add(rule1);

            LoggingRule rule2 = new LoggingRule("*", LogLevel.Trace, log_viewer_target);
            configuration.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = configuration;

            // Example usage
            log.Debug("Application started");
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            log.Debug("Application shutdown");
        }
    }
}
