using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoCecilRewriter
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Reading the assembly");
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(@"C:\Private\GitHub\Projects\MonoCecilTest\TestObjects\bin\Debug\TestObjects.dll");

            var notify_property_classes = assembly.GetNotifyPropertyChangedClasses();
            foreach (var notify_property_class in notify_property_classes)
            {
                var map = DependencyAnalyzer.Execute(notify_property_class);
                Console.WriteLine("Dependency map for " + notify_property_class.Name);
                map.Dump();
                Rewriter.Execute(notify_property_class, map);
            }

            Console.WriteLine("Writing the modified assembly");
            assembly.Name.Name = assembly.Name.Name.Replace("TestObjects", "TestObjectsModified");
            assembly.Write(@"C:\Private\GitHub\Projects\MonoCecilTest\TestObjects\bin\Debug\TestObjectsModified.dll");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
