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

// TODO:
// - Normal properties should call NotifyPropertyChanged for dependent properties

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
            string path = @"C:\Private\GitHub\Projects\MonoCecilTest\TestObjects\bin\Debug\TestObjects.dll";
            string filename = Path.GetFileName(path);
            string folder = Path.GetDirectoryName(path);

            Console.WriteLine("Reading the assembly " + path);
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path);

            var notify_property_classes = assembly.GetNotifyPropertyChangedClasses();
            foreach (var notify_property_class in notify_property_classes)
            {
                var map = DependencyAnalyzer.Execute(notify_property_class);
                map.Dump();
                Rewriter.Execute(assembly, notify_property_class, map);
            }

            string modified_path = path.Replace(".dll", "Modified.dll");
            assembly.Name.Name = assembly.Name.Name + "Modified";

            Console.WriteLine("Writing the modified assembly " + modified_path);
            assembly.Write(modified_path);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
