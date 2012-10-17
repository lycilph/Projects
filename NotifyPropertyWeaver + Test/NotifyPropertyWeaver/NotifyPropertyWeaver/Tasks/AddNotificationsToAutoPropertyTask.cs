using Mono.Cecil;
using Mono.Cecil.Cil;
//using NLog;

namespace NotifyPropertyWeaver.Tasks
{
    public static class AddNotificationsToAutoPropertyTask
    {
        //private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static void Execute(PropertyDefinition property_definition, MethodDefinition notify_method, DependencyMap map)
        {
            FieldDefinition backing_field = property_definition.GetAutoPropertyBackingField();

            // Clear custom attributes
            backing_field.CustomAttributes.Clear();
            property_definition.GetMethod.CustomAttributes.Clear();
            property_definition.SetMethod.CustomAttributes.Clear();

            // Clear set method body
            property_definition.SetMethod.Body.Instructions.Clear();

            ILProcessor processor = property_definition.SetMethod.Body.GetILProcessor();
            Instruction ret = processor.Create(OpCodes.Ret);

            // if (value != property)
            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldfld, backing_field);
            processor.Emit(OpCodes.Ceq);
            processor.Emit(OpCodes.Brtrue_S, ret);

            // property = value
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.Stfld, backing_field);

            // NotifyPropertyChanged(property)
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldstr, property_definition.Name);
            processor.Emit(OpCodes.Call, notify_method);

            // Add notifications for dependent properties
            foreach (var target in map.GetDependenciesFor(property_definition.Name))
            {
                //log.Trace("\t\t\t\t\tAdding dependency " + target);
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Ldstr, target);
                processor.Emit(OpCodes.Call, notify_method);
            }

            processor.Append(ret);
        }
    }
}
