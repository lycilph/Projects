using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Specialized;
using System.Reflection;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;
using PropertyAttributes = Mono.Cecil.PropertyAttributes;

namespace MonoCecilRewriter
{
    //public class Rewriter
    //{
    //    private readonly DependencyMap dependencies;
    //    private readonly TypeDefinition notify_property_class;
    //    private readonly AssemblyDefinition assembly;

    //    private Rewriter(AssemblyDefinition assembly, TypeDefinition notify_property_class, DependencyMap map)
    //    {
    //        this.assembly = assembly;
    //        this.notify_property_class = notify_property_class;
    //        dependencies = map;
    //    }

    //    private void RewriteAutoProperty(PropertyDefinition property_definition)
    //    {
    //        Console.WriteLine(string.Format("   Rewriting {0} to call NotifyPropertyChanged", property_definition.Name));
    //        FieldDefinition backing_field = notify_property_class.Fields.Single(f => f.Name == string.Format("<{0}>k__BackingField", property_definition.Name));
    //        MethodDefinition notify_method = notify_property_class.GetNotifyPropertyChangedMethod();

    //        // Clear custom attributes
    //        backing_field.CustomAttributes.Clear();
    //        property_definition.GetMethod.CustomAttributes.Clear();
    //        property_definition.SetMethod.CustomAttributes.Clear();

    //        // Clear set method body
    //        property_definition.SetMethod.Body.Instructions.Clear();

    //        ILProcessor processor = property_definition.SetMethod.Body.GetILProcessor();
    //        Instruction ret = processor.Create(OpCodes.Ret);

    //        // if (value != property)
    //        processor.Append(processor.Create(OpCodes.Ldarg_1));
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
    //        processor.Append(processor.Create(OpCodes.Ceq));
    //        processor.Append(processor.Create(OpCodes.Brtrue_S, ret));

    //        // property = value
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldarg_1));
    //        processor.Append(processor.Create(OpCodes.Stfld, backing_field));

    //        // NotifyPropertyChanged(property)
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldstr, property_definition.Name));
    //        processor.Append(processor.Create(OpCodes.Call, notify_method));

    //        // Add notifications for dependent properties
    //        foreach (var target in dependencies.GetPropertyDependenciesFor(property_definition.Name))
    //        {
    //            Console.WriteLine("      Adding dependency " + target);
    //            processor.Append(processor.Create(OpCodes.Ldarg_0));
    //            processor.Append(processor.Create(OpCodes.Ldstr, target));
    //            processor.Append(processor.Create(OpCodes.Call, notify_method));
    //        }

    //        processor.Append(ret);
    //    }

    //    private void RewriteCollectionAutoProperty(PropertyDefinition property_definition)
    //    {
    //        Console.WriteLine(string.Format("   Rewriting {0} to raise collection notification method", property_definition.Name));
    //        FieldDefinition backing_field = notify_property_class.Fields.Single(f => f.Name == string.Format("<{0}>k__BackingField", property_definition.Name));
    //        MethodDefinition notify_method = notify_property_class.GetNotifyPropertyChangedMethod();
    //        MethodDefinition collection_changed_method = notify_property_class.Methods.Single(m => m.Name == property_definition.Name + "CollectionNotification");
    //        MethodReference event_constructor = assembly.MainModule.Import(typeof(NotifyCollectionChangedEventHandler).GetConstructors()[0]);

    //        // Import remove_CollectionChanged method
    //        Type backing_field_type = backing_field.GetAssemblyQualifiedName();
    //        MethodReference collection_remove_handler = assembly.MainModule.Import(backing_field_type.GetMethod("remove_CollectionChanged"));
    //        MethodReference collection_add_handler = assembly.MainModule.Import(backing_field_type.GetMethod("add_CollectionChanged"));

    //        // Clear custom attributes
    //        backing_field.CustomAttributes.Clear();
    //        property_definition.GetMethod.CustomAttributes.Clear();
    //        property_definition.SetMethod.CustomAttributes.Clear();

    //        // Clear set method body
    //        property_definition.SetMethod.Body.Instructions.Clear();

    //        ILProcessor processor = property_definition.SetMethod.Body.GetILProcessor();
    //        Instruction ret = processor.Create(OpCodes.Ret);
    //        Instruction set_value_nop = processor.Create(OpCodes.Nop);
    //        Instruction notify_property_nop = processor.Create(OpCodes.Nop);

    //        // if (value != property)
    //        processor.Append(processor.Create(OpCodes.Ldarg_1));
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
    //        processor.Append(processor.Create(OpCodes.Ceq));
    //        processor.Append(processor.Create(OpCodes.Brtrue_S, ret));

    //        // If (property != null)
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
    //        processor.Append(processor.Create(OpCodes.Ldnull));
    //        processor.Append(processor.Create(OpCodes.Ceq));
    //        processor.Append(processor.Create(OpCodes.Brtrue_S, set_value_nop));

    //        // CollectionChanged -= notify method
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldftn, collection_changed_method));
    //        processor.Append(processor.Create(OpCodes.Newobj, event_constructor));
    //        processor.Append(processor.Create(OpCodes.Callvirt, collection_remove_handler));

    //        // property = value
    //        processor.Append(set_value_nop);
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldarg_1));
    //        processor.Append(processor.Create(OpCodes.Stfld, backing_field));

    //        // If (property != null)
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
    //        processor.Append(processor.Create(OpCodes.Ldnull));
    //        processor.Append(processor.Create(OpCodes.Ceq));
    //        processor.Append(processor.Create(OpCodes.Brtrue_S, notify_property_nop));

    //        // CollectionChanged -= notify method
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldftn, collection_changed_method));
    //        processor.Append(processor.Create(OpCodes.Newobj, event_constructor));
    //        processor.Append(processor.Create(OpCodes.Callvirt, collection_add_handler));

    //        // NotifyPropertyChanged(property)
    //        processor.Append(notify_property_nop);
    //        processor.Append(processor.Create(OpCodes.Ldarg_0));
    //        processor.Append(processor.Create(OpCodes.Ldstr, property_definition.Name));
    //        processor.Append(processor.Create(OpCodes.Call, notify_method));

    //        processor.Append(ret);
    //    }

    //    private void RewriteAutoProperties()
    //    {
    //        Console.WriteLine("Rewriting " + notify_property_class.Name);
    //        foreach (var property in notify_property_class.GetAutoProperties())
    //        {
    //            if (property.IsNotifyCollectionChangedProperty())
    //                RewriteCollectionAutoProperty(property);
    //            else
    //                RewriteAutoProperty(property);
    //        }
    //    }

    //    private void AddCollectionNotifications()
    //    {
    //        foreach (var source in dependencies.GetCollectionSources())
    //        {
    //            Console.WriteLine("Adding collection notification method for " + source);

    //            // Create method
    //            TypeReference return_type = assembly.MainModule.Import(typeof (void));
    //            MethodDefinition collection_notification = new MethodDefinition(source + "CollectionNotification", Mono.Cecil.MethodAttributes.Private | Mono.Cecil.MethodAttributes.HideBySig, return_type);

    //            // Add parameters
    //            TypeReference sender_type = assembly.MainModule.Import(typeof (object));
    //            TypeReference args_type = assembly.MainModule.Import(typeof (NotifyCollectionChangedEventArgs));

    //            ParameterDefinition sender = new ParameterDefinition("sender", Mono.Cecil.ParameterAttributes.None, sender_type);
    //            ParameterDefinition args = new ParameterDefinition("args", Mono.Cecil.ParameterAttributes.None, args_type);

    //            collection_notification.Parameters.Add(sender);
    //            collection_notification.Parameters.Add(args);

    //            // Find the notify method for the class
    //            MethodDefinition notify_method = notify_property_class.Methods.Single(m => m.Name == "NotifyPropertyChanged");

    //            // Add notifications for dependent properties
    //            ILProcessor processor = collection_notification.Body.GetILProcessor();
    //            foreach (var target in dependencies.GetCollectionDependenciesFor(source))
    //            {
    //                Console.WriteLine("   Adding dependency " + target);
    //                processor.Append(processor.Create(OpCodes.Ldarg_0));
    //                processor.Append(processor.Create(OpCodes.Ldstr, target));
    //                processor.Append(processor.Create(OpCodes.Call, notify_method));
    //                processor.Append(processor.Create(OpCodes.Nop));
    //            }
    //            processor.Append(processor.Create(OpCodes.Ret));

    //            // Add method to class
    //            notify_property_class.Methods.Add(collection_notification);
    //        }
    //    }

    //    private void WrapFields()
    //    {
    //        Console.WriteLine("Wrapping public non-backing fields in properties for " + notify_property_class.Name);
    //        var fields = notify_property_class.GetPublicNonBackingFields();
    //        foreach (var field in fields)
    //        {
    //            var old_field_name = field.Name;
    //            var new_field_name = string.Format("<{0}>k__BackingField", field.Name);
    //            Console.WriteLine("   Renaming " + old_field_name + " to " + new_field_name);
    //            field.Name = new_field_name;
    //            field.IsPrivate = true;

    //            Console.WriteLine("   Adding getter method");
    //            var get_method = new MethodDefinition("get_" + old_field_name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, field.FieldType);
    //            get_method.SemanticsAttributes = MethodSemanticsAttributes.Getter;
    //            get_method.Body.InitLocals = true;

    //            var getter = get_method.Body.GetILProcessor();
    //            getter.Emit(OpCodes.Ldarg_0);
    //            getter.Emit(OpCodes.Ldfld, field);
    //            getter.Emit(OpCodes.Ret);

    //            notify_property_class.Methods.Add(get_method);

    //            Console.WriteLine("   Adding setter method");
    //            var void_type = assembly.MainModule.Import(typeof(void));
    //            var set_method = new MethodDefinition("set_" + old_field_name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, void_type);
    //            set_method.SemanticsAttributes = MethodSemanticsAttributes.Setter;
    //            set_method.Body.InitLocals = true;

    //            var value = new ParameterDefinition("value", ParameterAttributes.None, field.FieldType);
    //            set_method.Parameters.Add(value);

    //            var setter = set_method.Body.GetILProcessor();
    //            setter.Emit(OpCodes.Ldarg_0);
    //            setter.Emit(OpCodes.Ldarg_1);
    //            setter.Emit(OpCodes.Stfld, field);
    //            setter.Emit(OpCodes.Ret);

    //            notify_property_class.Methods.Add(set_method);

    //            Console.WriteLine("   Adding property for field " + old_field_name);
    //            var property = new PropertyDefinition(old_field_name, PropertyAttributes.None, field.FieldType);
    //            property.HasThis = true; // This makes it an "instance property"

    //            property.GetMethod = get_method;
    //            property.SetMethod = set_method;

    //            notify_property_class.Properties.Add(property);

    //        }
    //    }

    //    public static void Execute(AssemblyDefinition assembly, TypeDefinition notify_property_class, DependencyMap map)
    //    {
    //        Rewriter rewriter = new Rewriter(assembly, notify_property_class, map);

    //        rewriter.WrapFields();
    //        rewriter.AddCollectionNotifications();
    //        rewriter.RewriteAutoProperties();
    //    }
    //}
}
