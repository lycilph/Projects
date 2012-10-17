using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCecilRewriter.Tasks
{
    class WrapFieldsTask
    {
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

    }
}
