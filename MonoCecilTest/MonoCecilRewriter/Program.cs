using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using System.Diagnostics;

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
// TODO: What about classes derived from INotifyPropertyChanged classes???

namespace MonoCecilRewriter
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("One input found");
            //    return;
            //}

            //string path = args[0];
            const string path = @"C:\Private\GitHub\Projects\MonoCecilTest\TestObjects\bin\Debug\TestObjects.dll";
            string filename = Path.GetFileName(path);
            string folder = Path.GetDirectoryName(path);

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
        }
    }
}
