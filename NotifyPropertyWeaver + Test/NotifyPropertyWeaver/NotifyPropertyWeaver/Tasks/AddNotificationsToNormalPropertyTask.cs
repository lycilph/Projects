using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
//using NLog;

namespace NotifyPropertyWeaver.Tasks
{
    public class AddNotificationsToNormalPropertyTask
    {
        //private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static void Execute(PropertyDefinition property_definition, MethodDefinition notify_method, DependencyMap map)
        {
            // Check if notifications are already call, if so bail out
            foreach (var instruction in property_definition.SetMethod.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Call)
                {
                    var method = instruction.Operand as MethodDefinition;
                    if (method != null && method == notify_method)
                    {
                        //log.Trace("\t\t\t\t\tBailing out, notification found in property");
                        return;
                    }
                }
            }

            // Add notifications
            var ret = property_definition.SetMethod.Body.Instructions.Last(i => i.OpCode == OpCodes.Ret);
            ILProcessor processor = property_definition.SetMethod.Body.GetILProcessor();

            // NotifyPropertyChanged(property)
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldstr, property_definition.Name));
            processor.InsertBefore(ret, processor.Create(OpCodes.Call, notify_method));

            // Add notifications for dependent properties
            foreach (var target in map.GetDependenciesFor(property_definition.Name))
            {
                //log.Trace("\t\t\t\t\tAdding dependency " + target);
                processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
                processor.InsertBefore(ret, processor.Create(OpCodes.Ldstr, target));
                processor.InsertBefore(ret, processor.Create(OpCodes.Call, notify_method));
            }
        }
    }
}
