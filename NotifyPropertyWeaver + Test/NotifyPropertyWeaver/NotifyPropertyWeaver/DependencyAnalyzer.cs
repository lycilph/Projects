using Mono.Cecil;
using Mono.Cecil.Cil;
//using NLog;

namespace NotifyPropertyWeaver
{
    public class DependencyAnalyzer
    {
        //private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private DependencyMap map;

        public void Execute(TypeDefinition type_definition, DependencyMap dependency_map)
        {
            map = dependency_map;

            foreach (var property in type_definition.Properties)
            {
                //log.Trace(string.Format("\t\t\tChecking {0} for dependencies", property.Name));
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
                        //log.Trace(operand_reference != null
                        //                      ? "\t\t\t\tFound reference to " + operand_reference.Name + " (stopping search)"
                        //                      : "\t\t\t\tUnknown operand (stopping search)");
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
                                //log.Trace(string.Format("\t\t\t\tFound call to {0} (new dependency)", source.Name));
                                map.Add(source.Name, target.Name);
                            }
                            // Else analyse the property getter
                            else
                            {
                                //log.Trace("\t\t\t\tFound call to " + operand.Name + " (continuing search)");
                                AnalyseMethod(target, operand);
                            }
                        }
                        // Else analyse the method
                        else
                        {
                            //log.Trace("\t\t\t\tFound call to " + operand.Name + " (continuing search)");
                            AnalyseMethod(target, operand);
                        }
                    }
                }
            }
        }
    }
}
