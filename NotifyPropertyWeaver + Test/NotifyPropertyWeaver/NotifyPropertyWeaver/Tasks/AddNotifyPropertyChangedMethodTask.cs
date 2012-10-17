using System.ComponentModel;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NotifyPropertyWeaver.Tasks
{
    public static class AddNotifyPropertyChangedMethodTask
    {
        public static MethodDefinition Execute(TypeDefinition notify_property_changed_class)
        {
            // Import types and methods
            TypeReference return_type = notify_property_changed_class.Module.Import(typeof (void));
            TypeReference property_parameter_type = notify_property_changed_class.Module.Import(typeof(string));
            TypeReference property_changed_handler_type = notify_property_changed_class.Module.Import(typeof(PropertyChangedEventHandler));
            MethodReference event_constructor = notify_property_changed_class.Module.Import(typeof(PropertyChangedEventArgs).GetConstructors()[0]);
            MethodReference invoke_event = notify_property_changed_class.Module.Import(typeof (PropertyChangedEventHandler).GetMethod("Invoke"));

            // Create method
            MethodDefinition notify_method = new MethodDefinition("NotifyPropertyChanged", MethodAttributes.Family | MethodAttributes.HideBySig, return_type);

            // Add parameters
            ParameterDefinition property_parameter = new ParameterDefinition("property_name", ParameterAttributes.None, property_parameter_type);
            notify_method.Parameters.Add(property_parameter);

            // Get the property changed event handler
            FieldReference property_changed_handler = notify_property_changed_class.GetEventHandler("PropertyChangedEventHandler");

            // Add variable
            VariableDefinition handler = new VariableDefinition(property_changed_handler_type);

            // Add local method variable
            notify_method.Body.Variables.Add(handler);
            notify_method.Body.InitLocals = true;

            // Add method body
            ILProcessor processor = notify_method.Body.GetILProcessor();
            Instruction ret = processor.Create(OpCodes.Ret);

            processor.Emit(OpCodes.Ldarg_0); // Load "this" variable
            processor.Emit(OpCodes.Ldfld, property_changed_handler);
            processor.Emit(OpCodes.Stloc_0);
            processor.Emit(OpCodes.Ldloc_0);
            processor.Emit(OpCodes.Ldnull);
            processor.Emit(OpCodes.Ceq);
            processor.Emit(OpCodes.Brtrue_S, ret);

            processor.Emit(OpCodes.Ldloc_0);
            processor.Emit(OpCodes.Ldarg_0); // Load "this" variable
            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.Newobj, event_constructor);
            processor.Emit(OpCodes.Callvirt, invoke_event);

            processor.Append(ret);

            // Add method to class
            notify_property_changed_class.Methods.Add(notify_method);

            return notify_method;
        }
    }
}
