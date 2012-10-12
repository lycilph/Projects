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
        private DependencyMap map;

        public void Execute(TypeDefinition type_definition, DependencyMap dependency_map)
        {
            map = dependency_map;

            foreach (var property in type_definition.Properties)
            {
                Console.WriteLine(string.Format("      Checking {0} for dependencies", property.Name));
                AnalyseMethod(property, property.GetMethod);
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
                        Console.WriteLine(operand_reference != null
                                              ? "         Found reference to " + operand_reference.Name + " (stopping search)"
                                              : "         Unknown operand (stopping search)");
                    }
                    else
                    {
                        // Is this a property we are calling?
                        if (operand.IsGetter)
                        {
                            // Can this property raise events? (ie. does it have a setter)
                            var source = operand.FindPropertyFromGetterMethod();
                            if (source.SetMethod != null)
                            {
                                var property_type = source.ImplementsInterface("INotifyCollectionChanged")
                                                        ? PropertyType.AutoCollectionProperty
                                                        : PropertyType.AutoProperty;
                                Console.WriteLine(string.Format("         Found call to {0} (new {1} dependency)", source.Name, property_type));
                                map.Add(source.Name, target.Name, property_type);
                            }
                            // Else analyse the property getter
                            else
                            {
                                Console.WriteLine("         Found call to " + operand.Name + " (continuing search)");
                                AnalyseMethod(target, operand);
                            }
                        }
                        // Else analyse the method
                        else
                        {
                            Console.WriteLine("         Found call to " + operand.Name + " (continuing search)");
                            AnalyseMethod(target, operand);
                        }
                    }
                }
            }
        }
    }
}
