using NLog;
using NLog.Config;
using NLog.Targets;

namespace Hierarchy_Test
{
    public static class LogSetup
    {
        public static void Default()
        {
            // Step 1. Create configuration object 
            LoggingConfiguration configuration = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            NLogViewerTarget log_viewer_target = new NLogViewerTarget();
            configuration.AddTarget("viewer", log_viewer_target);

            // Step 3. Set target properties 
            log_viewer_target.Name = "viewer";
            log_viewer_target.Address = "udp://127.0.0.1:9999";

            // Step 4. Define rules
            LoggingRule log_viewer_rule = new LoggingRule("*", LogLevel.Trace, log_viewer_target);
            configuration.LoggingRules.Add(log_viewer_rule);

            // Step 5. Activate the configuration
            LogManager.Configuration = configuration;
        }
    }
}
