using System;
using System.Linq;
using Mono.Cecil;
using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace MonoCecilRewriter
{
    // Analysis steps
    // - Find classes to process (ProcessClassTask)
    // - - Find NotifyPropertyChanged method (FindOrAddNotifyPropertyChangedMethodTask)
    // - - Find fields to wrap (WrapFieldTask)
    // - - Find dependencies
    // - - Find auto properties to instrument (InstrumentAutoPropertyTask)
    // - - Find collection properties to instrument (InstrumentCollectionPropertyTask)
    // - - Find collection properties to generate notification methods for (AddCollectionNotificationMethodTask)

    public class Analyzer
    {
        private readonly AssemblyDefinition assembly;
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

            FindNotifyPropertyChangedMethod();
            FindFieldsToWrap();
            FindDependencies();
            // Create notification methods for AutoCollectionProperties
        }

        private void FindNotifyPropertyChangedMethod()
        {
            Console.WriteLine("Finding notify property changed method");
            foreach (var notify_property_class in notify_property_classes)
            {
                Console.WriteLine("   Processing " + notify_property_class.Name);
                MethodDefinition notify_property_changed_method = notify_property_class.FindNotifyPropertyChangedMethod();
                if (notify_property_changed_method == null)
                    Console.WriteLine("      Adding NotifyPropertyChanged method");
                else
                    Console.WriteLine("      Found " + notify_property_changed_method.Name);
            }
        }

        private void FindFieldsToWrap()
        {
            Console.WriteLine("Finding fields to wrap");
            foreach (var notify_property_class in notify_property_classes)
            {
                Console.WriteLine("   Processing " + notify_property_class.Name);
                var fields_to_wrap = notify_property_class.GetPublicNonBackingFields();
                foreach (var field in fields_to_wrap)
                    Console.WriteLine("      Found " + field.Name);
            }
        }

        private void FindDependencies()
        {
            DependencyAnalyzer dependency_analyzer = new DependencyAnalyzer();

            Console.WriteLine("Finding property dependencies");
            foreach (var notify_property_class in notify_property_classes)
            {
                Console.WriteLine("   Processing " + notify_property_class.Name);
                DependencyMap map = new DependencyMap();
                dependency_analyzer.Execute(notify_property_class, map);
            }
        }
    }
}
