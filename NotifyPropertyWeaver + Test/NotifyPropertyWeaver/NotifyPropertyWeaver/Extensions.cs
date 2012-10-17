using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NotifyPropertyWeaver
{
    public static class Extensions
    {
        public static bool ImplementsInterface(this TypeDefinition type_definition, string interface_name)
        {
            if (type_definition.Interfaces.Any(i => i.Name == interface_name))
                return true;
            if (type_definition.BaseType != null && type_definition.BaseType.Resolve().ImplementsInterface(interface_name))
                return true;
            return false;
        }

        public static bool HasAttribute(this TypeDefinition type_definition, string attribute_name)
        {
            if (type_definition.HasCustomAttributes && type_definition.CustomAttributes.Any(a => a.AttributeType.Name == attribute_name))
                return true;
            if (type_definition.BaseType != null && type_definition.BaseType.Resolve().HasAttribute(attribute_name))
                return true;
            return false;
        }

        public static IEnumerable<TypeDefinition> GetNotifyPropertyChangedClasses(this AssemblyDefinition assembly)
        {
            return assembly.MainModule.Types.Where(t => t.ImplementsInterface("INotifyPropertyChanged") && t.HasAttribute("NotifyPropertyChangedAttribute"));
        }

        public static MethodDefinition FindNotifyPropertyChangedMethod(this TypeDefinition type_definition)
        {
            foreach (var method in type_definition.Methods)
            {
                if (method.HasBody)
                {
                    foreach (var instruction in method.Body.Instructions)
                    {
                        if (instruction.OpCode == OpCodes.Callvirt)
                        {
                            MethodReference method_ref = instruction.Operand as MethodReference;
                            if (method_ref != null &&
                                method_ref.DeclaringType.FullName == "System.ComponentModel.PropertyChangedEventHandler" &&
                                method_ref.Name == "Invoke")
                                return method;
                        }
                    }
                }
            }

            // No method found try base class
            if (type_definition.BaseType != null)
                return type_definition.BaseType.Resolve().FindNotifyPropertyChangedMethod();
            
            return null;
        }

        public static PropertyDefinition FindPropertyFromGetterMethod(this MethodDefinition method)
        {
            return method.DeclaringType.Properties.Single(p => p.GetMethod.Name == method.Name);
        }

        public static FieldReference GetEventHandler(this TypeDefinition type_definition, string event_type)
        {
            return type_definition.Fields.Single(e => e.FieldType.Name == event_type);
        }

        public static MethodDefinition GetConstructor(this TypeDefinition type_definition)
        {
            return type_definition.Methods.Single(m => m.Name == ".ctor");
        }

        public static FieldDefinition GetAutoPropertyBackingField(this PropertyDefinition property_definition)
        {
            return property_definition.DeclaringType.Fields.Single(f => f.Name == string.Format("<{0}>k__BackingField", property_definition.Name));
        }

        public static bool IsAutoProperty(this PropertyDefinition property_definition)
        {
            string backing_field_name = string.Format("<{0}>k__BackingField", property_definition.Name);
            return property_definition.DeclaringType.Fields.Any(f => f.Name == backing_field_name);
        }

        public static bool IsCollectionProperty(this PropertyDefinition property_definition)
        {
            return property_definition.PropertyType.Resolve().ImplementsInterface("INotifyCollectionChanged");
        }

        public static bool IsCollectionAutoProperty(this PropertyDefinition property_definition)
        {
            return property_definition.IsAutoProperty() && property_definition.IsCollectionProperty();
        }

        private static string GetAssemblyName(string type_name)
        {
            foreach (Assembly current_assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = current_assembly.GetType(type_name, false, true);
                if (t != null)
                    return current_assembly.FullName;
            }
            return string.Empty;
        }

        public static Type GetAssemblyQualifiedName(this FieldDefinition field)
        {
            string fullname = field.FieldType.Namespace + "." + field.FieldType.Name;
            string assembly_name = GetAssemblyName(fullname);
            string assembly_qualified_name;

            var generic_instance = field.FieldType as GenericInstanceType;
            if (generic_instance != null)
            {
                if (generic_instance.GenericArguments.Count > 1)
                    throw new ArgumentException("Only 1 generic argument is supported");
                string parameter = generic_instance.GenericArguments[0].FullName;
                string assembly_qualified_parameter = Type.GetType(parameter).AssemblyQualifiedName;

                assembly_qualified_name = string.Format("{0}[[{1}]],{2}", fullname, assembly_qualified_parameter, assembly_name);
            }
            else
            {
                assembly_qualified_name = string.Format("{0},{1}", fullname, assembly_name);
            }

            return Type.GetType(assembly_qualified_name);
        }

        public static FieldReference GetNormalPropertyBackingField(this PropertyDefinition property_definition)
        {
            if (property_definition.GetMethod == null)
                return null;

            // Find first field of the same type as the property in the get method, assume this is the backing field
            foreach (var instruction in property_definition.GetMethod.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Ldfld)
                {
                    var field_reference = instruction.Operand as FieldReference;
                    if (field_reference != null && field_reference.FieldType.FullName == property_definition.PropertyType.FullName)
                        return field_reference;
                }
            }

            return null;
        }
    }
}
