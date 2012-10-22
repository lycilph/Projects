using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NotifyPropertyWeaver
{
    public class DependencyAnalyzerV2
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public void Run(AssemblyDefinition assembly)
        {
            var notify_property_classes = assembly.GetNotifyPropertyChangedClasses().ToList();
            foreach (var notify_property_class in notify_property_classes)
                AnalyzeClass(notify_property_class);
        }

        private void AnalyzeClass(TypeDefinition notify_property_class)
        {
            log.Write("Processing class {0}", notify_property_class.Name);

            var properties = notify_property_class.Properties.Where(p => p.GetMethod != null);
            foreach (var property in properties)
                AnalyzeProperty(property);
        }

        private void AnalyzeProperty(PropertyDefinition property)
        {
            log.Write(1, "Processing property {0}", property.Name);

            AnalyzeMethod(property, property.GetMethod);
        }

        private void AnalyzeMethod(PropertyDefinition property, MethodDefinition method)
        {
            var method_calls = method.Body.Instructions.Where(i => i.OpCode == OpCodes.Call);

            // for each method call
            // - if it is a call to a property (on this class)
            //   - if it is followed by a callvirt call
            //     - Add new dependency on property on child object (if it is a INotifyPropertyChanged object)
            //   - else
            //     - Add new dependency
            // - else
            //   - recursively analyze method

            // Find all "call" opcodes
            // - What about chained calls (ie. call followed by callvirt)
        }
    }
}
