using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Reflection;
using Mono.Cecil.Cil;

namespace MonoCecilRewriter
{
    public static class Extensions
    {
        public static bool ImplementsINotifyPropertyChanged(this TypeDefinition type_definition)
        {
            if (type_definition.Interfaces.Any(i => i.Name == "INotifyPropertyChanged"))
                return true;
            if (type_definition.BaseType != null && type_definition.BaseType.Resolve().ImplementsINotifyPropertyChanged())
                return true;
            return false;
        }

        public static bool HasNotifyPropertyChangedAttribute(this TypeDefinition type_definition)
        {
            if (type_definition.HasCustomAttributes && type_definition.CustomAttributes.Any(a => a.AttributeType.Name == "NotifyPropertyChangedAttribute"))
                return true;
            if (type_definition.BaseType != null && type_definition.BaseType.Resolve().HasNotifyPropertyChangedAttribute())
                return true;
            return false;
        }

        public static IEnumerable<TypeDefinition> GetNotifyPropertyChangedClasses(this AssemblyDefinition assembly)
        {
            return assembly.MainModule.Types.Where(t => t.ImplementsINotifyPropertyChanged() && t.HasNotifyPropertyChangedAttribute());
        }

        //public static IEnumerable<FieldDefinition> GetPublicNonBackingFields(this TypeDefinition notify_property_class)
        //{
        //    return notify_property_class.Fields.Where(f => f.IsPublic && !f.Name.Contains("k__BackingField"));
        //}



        //public static IEnumerable<TypeDefinition> GetNotifyPropertyChangedClasses(this AssemblyDefinition assembly)
        //{
        //    return assembly.MainModule.Types.Where(t => t.Interfaces.Any(it => it.Name == "INotifyPropertyChanged"));
        //}

        //public static IEnumerable<PropertyDefinition> GetAutoProperties(this TypeDefinition notify_property_class)
        //{
        //    return notify_property_class.Fields.Where(f => f.Name.Contains("k__BackingField"))
        //                                       .Select(f => f.Name.Substring(1, f.Name.IndexOf("k__BackingField", StringComparison.InvariantCulture) - 2))
        //                                       .Select(f => notify_property_class.Properties.Single(p => p.Name == f))
        //                                       .ToList();
        //}

        //public static IEnumerable<FieldDefinition> GetPublicNonBackingFields(this TypeDefinition notify_property_class)
        //{
        //    return notify_property_class.Fields.Where(f => f.IsPublic && !f.Name.Contains("k__BackingField"));
        //}

        //public static PropertyDefinition GetPropertyFromGetter(this MethodDefinition method)
        //{
        //    return method.DeclaringType.Properties.Single(p => p.GetMethod.Name == method.Name);
        //}

        //public static bool IsNotifyCollectionChangedProperty(this PropertyDefinition property)
        //{
        //    return property.PropertyType.Resolve().Interfaces.Any(i => i.Name == "INotifyCollectionChanged");
        //}

        //private static string GetAssemblyName(string type_name)
        //{
        //    foreach (Assembly current_assembly in AppDomain.CurrentDomain.GetAssemblies())
        //    {
        //        Type t = current_assembly.GetType(type_name, false, true);
        //        if (t != null)
        //            return current_assembly.FullName;
        //    }
        //    return string.Empty;
        //}

        //public static Type GetAssemblyQualifiedName(this FieldDefinition field)
        //{
        //    string fullname = field.FieldType.Namespace + "." + field.FieldType.Name;
        //    string assembly_name = GetAssemblyName(fullname);
        //    string assembly_qualified_name;

        //    var generic_instance = field.FieldType as GenericInstanceType;
        //    if (generic_instance != null)
        //    {
        //        if (generic_instance.GenericArguments.Count > 1)
        //            throw new ArgumentException("Only 1 generic argument is supported");
        //        string parameter = generic_instance.GenericArguments[0].FullName;
        //        string assembly_qualified_parameter = Type.GetType(parameter).AssemblyQualifiedName;

        //        assembly_qualified_name = string.Format("{0}[[{1}]],{2}", fullname, assembly_qualified_parameter, assembly_name);
        //    }
        //    else
        //    {
        //        assembly_qualified_name = string.Format("{0},{1}", fullname, assembly_name);
        //    }

        //    return Type.GetType(assembly_qualified_name);
        //}

        //public static MethodDefinition GetNotifyPropertyChangedMethod(this TypeDefinition notify_property_class)
        //{
        //    foreach (var method in notify_property_class.Methods)
        //    {
        //        foreach (var instruction in method.Body.Instructions)
        //        {
        //            if (instruction.OpCode == OpCodes.Callvirt)
        //            {
        //                MethodReference method_ref = instruction.Operand as MethodReference;
        //                if (method_ref != null && method_ref.DeclaringType.FullName == "System.ComponentModel.PropertyChangedEventHandler" && method_ref.Name == "Invoke")
        //                    return method;
        //            }
        //        }
        //    }

        //    return null;
        //}
    }
}
