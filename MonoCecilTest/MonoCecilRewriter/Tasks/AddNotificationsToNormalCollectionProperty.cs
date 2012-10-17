using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Specialized;
using NLog;

namespace MonoCecilRewriter.Tasks
{
    public class AddNotificationsToNormalCollectionProperty
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static void Execute(PropertyDefinition property_definition, MethodDefinition notify_method, MethodDefinition collection_notification_method)
        {
            // Check if notifications are already call, if so bail out
            foreach (var instruction in property_definition.SetMethod.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Call)
                {
                    var method = instruction.Operand as MethodDefinition;
                    if (method != null && method == notify_method)
                    {
                        log.Trace("\t\t\t\t\tBailing out, notification found in property");
                        return;
                    }
                }
            }

            AddNotifications(property_definition, notify_method, collection_notification_method);
            AddConstructorInitialization(property_definition);
        }

        private static void AddNotifications(PropertyDefinition property_definition, MethodDefinition notify_method, MethodDefinition collection_notification_method)
        {
            // Find backing field
            FieldDefinition backing_field = property_definition.GetNormalPropertyBackingField().Resolve();

            // Import CollectionChanged methods
            Type backing_field_type = backing_field.GetAssemblyQualifiedName();
            MethodReference collection_remove_handler = property_definition.Module.Import(backing_field_type.GetMethod("remove_CollectionChanged"));
            MethodReference collection_add_handler = property_definition.Module.Import(backing_field_type.GetMethod("add_CollectionChanged"));
            MethodReference event_constructor = property_definition.Module.Import(typeof(NotifyCollectionChangedEventHandler).GetConstructors()[0]);

            // Add notifications
            ILProcessor processor = property_definition.SetMethod.Body.GetILProcessor();
            var first = property_definition.SetMethod.Body.Instructions.First();
            var ret = property_definition.SetMethod.Body.Instructions.Last(i => i.OpCode == OpCodes.Ret);
            var notify_property_nop = processor.Create(OpCodes.Nop);

            // If (property != null)
            processor.InsertBefore(first, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(first, processor.Create(OpCodes.Ldfld, backing_field));
            processor.InsertBefore(first, processor.Create(OpCodes.Ldnull));
            processor.InsertBefore(first, processor.Create(OpCodes.Ceq));
            processor.InsertBefore(first, processor.Create(OpCodes.Brtrue_S, first));

            // CollectionChanged -= notify method
            processor.InsertBefore(first, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(first, processor.Create(OpCodes.Ldfld, backing_field));
            processor.InsertBefore(first, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(first, processor.Create(OpCodes.Ldftn, collection_notification_method));
            processor.InsertBefore(first, processor.Create(OpCodes.Newobj, event_constructor));
            processor.InsertBefore(first, processor.Create(OpCodes.Callvirt, collection_remove_handler));

            // Existing code is here

            // If (property != null)
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldfld, backing_field));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldnull));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ceq));
            processor.InsertBefore(ret, processor.Create(OpCodes.Brtrue_S, notify_property_nop));

            // CollectionChanged -= notify method
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldfld, backing_field));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldftn, collection_notification_method));
            processor.InsertBefore(ret, processor.Create(OpCodes.Newobj, event_constructor));
            processor.InsertBefore(ret, processor.Create(OpCodes.Callvirt, collection_add_handler));

            // NotifyPropertyChanged(property)
            processor.InsertBefore(ret, notify_property_nop);
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldstr, property_definition.Name));
            processor.InsertBefore(ret, processor.Create(OpCodes.Call, notify_method));

            // Return from method here            
        }

        private static void AddConstructorInitialization(PropertyDefinition property_definition)
        {
            // Find backing field
            FieldDefinition backing_field = property_definition.GetNormalPropertyBackingField().Resolve();

            // Fix constructor setup of property (ie. add event handler to property, do it by setting property to field)
            var constructor = property_definition.DeclaringType.GetConstructor();

            // Add initialization
            ILProcessor processor = constructor.Body.GetILProcessor();
            var ret = constructor.Body.Instructions.Last(i => i.OpCode == OpCodes.Ret);

            processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldarg_0));
            processor.InsertBefore(ret, processor.Create(OpCodes.Ldfld, backing_field));
            processor.InsertBefore(ret, processor.Create(OpCodes.Call, property_definition.SetMethod));
        }
    }
}
