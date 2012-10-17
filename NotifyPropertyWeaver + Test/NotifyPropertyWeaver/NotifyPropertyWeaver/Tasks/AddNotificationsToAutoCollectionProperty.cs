using System;
using System.Collections.Specialized;
using Mono.Cecil;
using Mono.Cecil.Cil;
//using NLog;

namespace NotifyPropertyWeaver.Tasks
{
    public class AddNotificationsToAutoCollectionProperty
    {
        //private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static void Execute(PropertyDefinition property_definition, MethodDefinition notify_method, MethodDefinition collection_notification_method)
        {
            //log.Trace(string.Format("\t\t\t\t\tRewriting {0} to raise collection notification method", property_definition.Name));

            // Find backing field
            FieldDefinition backing_field = property_definition.GetAutoPropertyBackingField();

            // Import CollectionChanged methods
            Type backing_field_type = backing_field.GetAssemblyQualifiedName();
            MethodReference collection_remove_handler = property_definition.Module.Import(backing_field_type.GetMethod("remove_CollectionChanged"));
            MethodReference collection_add_handler = property_definition.Module.Import(backing_field_type.GetMethod("add_CollectionChanged"));
            MethodReference event_constructor = property_definition.Module.Import(typeof(NotifyCollectionChangedEventHandler).GetConstructors()[0]);

            // Clear custom attributes
            backing_field.CustomAttributes.Clear();
            property_definition.GetMethod.CustomAttributes.Clear();
            property_definition.SetMethod.CustomAttributes.Clear();

            // Clear set method body
            property_definition.SetMethod.Body.Instructions.Clear();

            ILProcessor processor = property_definition.SetMethod.Body.GetILProcessor();
            Instruction ret = processor.Create(OpCodes.Ret);
            Instruction set_value_nop = processor.Create(OpCodes.Nop);
            Instruction notify_property_nop = processor.Create(OpCodes.Nop);

            // if (value != property)
            processor.Append(processor.Create(OpCodes.Ldarg_1));
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
            processor.Append(processor.Create(OpCodes.Ceq));
            processor.Append(processor.Create(OpCodes.Brtrue_S, ret));

            // If (property != null)
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
            processor.Append(processor.Create(OpCodes.Ldnull));
            processor.Append(processor.Create(OpCodes.Ceq));
            processor.Append(processor.Create(OpCodes.Brtrue_S, set_value_nop));

            // CollectionChanged -= notify method
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldftn, collection_notification_method));
            processor.Append(processor.Create(OpCodes.Newobj, event_constructor));
            processor.Append(processor.Create(OpCodes.Callvirt, collection_remove_handler));

            // property = value
            processor.Append(set_value_nop);
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldarg_1));
            processor.Append(processor.Create(OpCodes.Stfld, backing_field));

            // If (property != null)
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
            processor.Append(processor.Create(OpCodes.Ldnull));
            processor.Append(processor.Create(OpCodes.Ceq));
            processor.Append(processor.Create(OpCodes.Brtrue_S, notify_property_nop));

            // CollectionChanged -= notify method
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldftn, collection_notification_method));
            processor.Append(processor.Create(OpCodes.Newobj, event_constructor));
            processor.Append(processor.Create(OpCodes.Callvirt, collection_add_handler));

            // NotifyPropertyChanged(property)
            processor.Append(notify_property_nop);
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldstr, property_definition.Name));
            processor.Append(processor.Create(OpCodes.Call, notify_method));

            processor.Append(ret);
        }
    }
}
