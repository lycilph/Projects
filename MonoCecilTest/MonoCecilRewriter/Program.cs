using System.IO;
using Mono.Cecil;
using NLog;
using NLog.Config;
using NLog.Targets;

// TODO: Make field wrapping optional
// TODO: Make Mono.Cecil write debug file (.pdb) (see symbolwriter/reader i think)

// Post build event for library
// - $(SolutionDir)MonoCecilRewriter\$(OutDir)MonoCecilRewriter.exe $(TargetPath)

namespace MonoCecilRewriter
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupLogging();
            Logger log = LogManager.GetCurrentClassLogger();

            if (args.Length == 0)
            {
                log.Trace("No input found");
                return;
            }

            string path = args[0];

            // Copy the original dll
            string new_path = path.Replace(".dll", "Original.dll");
            File.Copy(path, new_path, true);

            log.Trace("Reading the assembly " + path);
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path);

            var analyzer = new Analyzer(assembly);
            analyzer.Execute();

            log.Trace("Writing the modified assembly ");
            assembly.Write(path);
        }

        private static void SetupLogging()
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
