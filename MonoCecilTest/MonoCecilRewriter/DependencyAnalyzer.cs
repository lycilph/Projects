using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoCecilRewriter
{
    public class DependencyAnalyzer
    {
        private readonly DependencyMap dependencies = new DependencyMap();

        private void AnalyseDependency(PropertyDefinition source, PropertyDefinition target)
        {
            if (source.IsNotifyCollectionChangedProperty())
            {
                Console.WriteLine("         Dependency is a INotifyCollectionChanged");
                dependencies.AddCollectionDependency(source.Name, target.Name);
            }
            else
            {
                Console.WriteLine("         Dependency is a \"normal\" property");
                dependencies.AddPropertyDependency(source.Name, target.Name);
            }
        }

        private void AnalyseMethod(PropertyDefinition target, MethodDefinition method)
        {
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Call)
                {
                    var operand = instruction.Operand as MethodDefinition;
                    if (operand == null)
                    {
                        var operand_reference = instruction.Operand as MethodReference;
                        if (operand_reference != null)
                            Console.WriteLine("      Found method reference to " + operand_reference.Name + " (stopping search)");
                        else
                            Console.WriteLine("      Unknown operand (stopping search)");
                    }
                    else
                    {
                        // Is this a property we are calling?
                        if (operand.IsGetter)
                        {
                            // Can this property raise events? (ie. does it have a setter)
                            var source = operand.GetPropertyFromGetter();
                            if (source.SetMethod != null)
                            {
                                Console.WriteLine("      Found method call to " + source.Name + " (new dependency)");
                                AnalyseDependency(source, target);
                            }
                            // Else analyse the property getter
                            else
                            {
                                Console.WriteLine("      Found method call to " + operand.Name + " (continuing search)");
                                AnalyseMethod(target, operand);
                            }
                        }
                        // Else analyse the method
                        else
                        {
                            Console.WriteLine("      Found method call to " + operand.Name + " (continuing search)");
                            AnalyseMethod(target, operand);
                        }
                    }
                }
            }
        }

        private void AnalyzeProperty(PropertyDefinition property)
        {
            Console.WriteLine(string.Format("   Checking {0} for dependencies", property.Name));
            AnalyseMethod(property, property.GetMethod);
        }

        public static DependencyMap Execute(TypeDefinition type_definition)
        {
            DependencyAnalyzer analyzer = new DependencyAnalyzer();

            Console.WriteLine("Analyzing " + type_definition.Name);
            foreach (var property in type_definition.Properties)
                analyzer.AnalyzeProperty(property);

            return analyzer.dependencies;
        }
    }
}
