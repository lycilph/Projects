﻿using System.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mono.Cecil;
using NLog;
using NLog.Config;
using NLog.Targets;

// TODO: Make child objects (ie. INotifyPropertyChanged) and INotifyCollectionChanged use weak events
//       - Implement IWeakEventListener on objects
// TODO: Use new LogExtension in all classes
// TODO: Rename analyser to weaver
// TODO: Merge handling of "normal" and "auto" properties
//       - Add Try... finally for all properties
//       - Always trigger notifications
// TODO: Handle child objects (of type INotifyPropertyChanged)

// KNOWN ISSUES:
// - If setter has more than 1 return, only the last will call notification method
// - - Add Try... finally for all "normal" properties

namespace NotifyPropertyWeaver
{
    public class WeaverTask : Task
    {
        [Required]
        public ITaskItem[] Files { get; set; }

        public string LogFile { get; set; }

        public override bool Execute()
        {
            SetupLogging();

            var task_stopwatch = Stopwatch.StartNew(); 
            WriteMessage("Weaving INotifyPropertyChanged started");

            try
            {
                foreach (var file in Files)
                {
                    var file_stopwatch = Stopwatch.StartNew();
                    WriteMessage(string.Format("Processing {0}", file.ItemSpec));

                    ReaderParameters reader_parameters = new ReaderParameters() {ReadSymbols = true};
                    AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(file.ItemSpec, reader_parameters);

                    var analyzer = new DependencyAnalyzerV2();
                    analyzer.Run(assembly);

                    //var analyzer = new Analyzer(assembly);
                    //analyzer.Execute();

                    //WriterParameters writer_parameters = new WriterParameters() {WriteSymbols = true};
                    //assembly.Write(file.ItemSpec, writer_parameters);
                    
                    file_stopwatch.Stop();
                    WriteMessage(string.Format("Processing took {0} ms", file_stopwatch.ElapsedMilliseconds));
                }
            }
            finally
            {
                task_stopwatch.Stop();
                WriteMessage(string.Format("Weaving INotifyPropertyChanged done after {0} ms", task_stopwatch.ElapsedMilliseconds));
            }
            
            return true; 
        }

        private void WriteMessage(string message)
        {
            BuildMessageEventArgs args = new BuildMessageEventArgs(message, string.Empty, typeof(WeaverTask).ToString(), MessageImportance.High);
            BuildEngine.LogMessageEvent(args);

            LogManager.GetCurrentClassLogger().Trace(message);
        }

        private void SetupLogging()
        {
            LoggingConfiguration configuration = new LoggingConfiguration();

            NLogViewerTarget log_viewer_target = new NLogViewerTarget();
            configuration.AddTarget("viewer", log_viewer_target);

            log_viewer_target.Name = "viewer";
            log_viewer_target.Address = "udp://127.0.0.1:9999";

            LoggingRule log_viewer_rule = new LoggingRule("*", LogLevel.Trace, log_viewer_target);
            configuration.LoggingRules.Add(log_viewer_rule);

            if (!string.IsNullOrEmpty(LogFile))
            {
                FileTarget file_target = new FileTarget();
                configuration.AddTarget("file", file_target);

                file_target.FileName = LogFile;
                file_target.Layout = "${message}";
                file_target.DeleteOldFileOnStartup = true;

                LoggingRule log_file_rule = new LoggingRule("*", LogLevel.Trace, file_target);
                configuration.LoggingRules.Add(log_file_rule);
            }

            // Step 5. Activate the configuration
            LogManager.Configuration = configuration;
        }
    }
}
