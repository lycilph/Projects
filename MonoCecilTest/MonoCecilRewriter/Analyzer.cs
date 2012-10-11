using System;
using System.Linq;
using Mono.Cecil;
using System.Collections.Generic;

namespace MonoCecilRewriter
{
    // Analysis steps
    // - Find classes to process (ProcessClassTask)
    // - - Find fields to wrap (WrapFieldTask)
    // - - Find dependencies
    // - - Find auto properties to instrument (InstrumentAutoProperty)

    public class Analyzer
    {
        private AssemblyDefinition assembly;
        private List<TypeDefinition> notify_property_classes;

        public Analyzer(AssemblyDefinition assembly)
        {
            this.assembly = assembly;
        }

        public void Execute()
        {
            Console.WriteLine("Finding classes to process");
            notify_property_classes = assembly.GetNotifyPropertyChangedClasses().ToList();
            foreach (var notify_property_class in notify_property_classes)
                Console.WriteLine("   Found " + notify_property_class.Name);

            WrapFields();
        }

        private void WrapFields()
        {
            Console.WriteLine("Finding fields to wrap");
            foreach (var notify_property_class in notify_property_classes)
            {
                Console.WriteLine("   Processing " + notify_property_class.Name);

            }
        }
    }
}
