using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using System.Diagnostics;
using NLog.Config;
using NLog.Targets;
using NLog;

// Supported cases
// - Autoproperty calls NotifyPropertyChanged on itself
// - Autoproperty calls NotifyPropertyChanged for dependent properties
// - Collection properties calls NotifyPropertyChanged for dependent properties
// - Wrap fields in properties

// TODO: Make field wrapping optional
// BUG: Fix rewriting collection autoproperty with no dependencies
// TODO: Add Attribute to control rewriting
// TODO: OPTIONAL: Add NotifyPropertyChanged if not present
// TODO: Normal properties should call NotifyPropertyChanged for dependent properties
// TODO: Add a log
// TODO: Add a post build event to library

namespace MonoCecilRewriter
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupLogging();
            Logger log = LogManager.GetCurrentClassLogger();
            log.Trace("Starting program");

            //if (args.Length == 0)
            //{
            //    Console.WriteLine("One input found");
            //    return;
            //}

            //string path = args[0];
            const string path = @"C:\Private\GitHub\Projects\MonoCecilTest\TestObjects\bin\Debug\TestObjects.dll";

            Console.WriteLine("Reading the assembly " + path);
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path);

            var analyzer = new Analyzer(assembly);
            analyzer.Execute();

            //var notify_property_classes = assembly.GetNotifyPropertyChangedClasses();
            ////var notify_property_classes = assembly.MainModule.Types.Where(t => t.Name == "WrapField");
            //foreach (var notify_property_class in notify_property_classes)
            //{
            //    var map = DependencyAnalyzer.Execute(notify_property_class);
            //    map.Dump();
            //    Rewriter.Execute(assembly, notify_property_class, map);
            //}

            //string modified_path = path.Replace(".dll", "Modified.dll");
            //assembly.Name.Name = assembly.Name.Name + "Modified";

            //Console.WriteLine("Writing the modified assembly " + modified_path);
            //assembly.Write(modified_path);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            log.Trace("Exiting program");
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
