using System.Collections.Specialized;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NLog;

namespace MonoCecilRewriter.Tasks
{
    public class AddCollectionNotificationMethodTask
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static MethodDefinition Execute(PropertyDefinition property_definition, MethodDefinition notify_method, DependencyMap map)
        {
            log.Trace("\t\t\t\t\tAdding collection notification method for " + property_definition.Name);

            // Create method
            TypeReference return_type = property_definition.Module.Import(typeof (void));
            MethodDefinition collection_notification = new MethodDefinition(property_definition.Name + "CollectionNotification", Mono.Cecil.MethodAttributes.Private | Mono.Cecil.MethodAttributes.HideBySig, return_type);

            // Add parameters
            TypeReference sender_type = property_definition.Module.Import(typeof(object));
            TypeReference args_type = property_definition.Module.Import(typeof(NotifyCollectionChangedEventArgs));

            ParameterDefinition sender = new ParameterDefinition("sender", Mono.Cecil.ParameterAttributes.None, sender_type);
            ParameterDefinition args = new ParameterDefinition("args", Mono.Cecil.ParameterAttributes.None, args_type);

            collection_notification.Parameters.Add(sender);
            collection_notification.Parameters.Add(args);

            // Add notifications for dependent properties
            ILProcessor processor = collection_notification.Body.GetILProcessor();
            foreach (var target in map.GetDependenciesFor(property_definition.Name))
            {
                log.Trace("\t\t\t\t\t\tAdding dependency " + target);
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Ldstr, target);
                processor.Emit(OpCodes.Call, notify_method);
            }
            processor.Emit(OpCodes.Ret);

            // Add method to class
            property_definition.DeclaringType.Methods.Add(collection_notification);
            return collection_notification;
        }
    }
}
