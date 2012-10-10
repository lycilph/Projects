using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace MonoCecilRewriter
{
    public class Rewriter
    {
        // TODO: Find notifypropertychanged method in a smarter way
        // TODO: Add input arguments to program
        // TODO: Add a log
        // TODO: Add a post build event to library

        private readonly DependencyMap dependencies;

        private Rewriter(DependencyMap map)
        {
            dependencies = map;
        }

        private void RewriteAutoProperty(PropertyDefinition property_definition)
        {
            Console.WriteLine(string.Format("   Rewriting {0} to call NotifyPropertyChanged", property_definition.Name));
            FieldDefinition backing_field = property_definition.DeclaringType.Fields.Single(f => f.Name == string.Format("<{0}>k__BackingField", property_definition.Name));
            MethodDefinition notify_method = property_definition.DeclaringType.Methods.Single(m => m.Name == "NotifyPropertyChanged");

            // Add local variable
            var bool_type = property_definition.Module.Import(typeof(bool));
            var local_variable = new VariableDefinition("v_0", bool_type);
            property_definition.SetMethod.Body.Variables.Add(local_variable);

            // Clear custom attributes
            backing_field.CustomAttributes.Clear();
            property_definition.GetMethod.CustomAttributes.Clear();
            property_definition.SetMethod.CustomAttributes.Clear();

            // Clear set method body
            property_definition.SetMethod.Body.Instructions.Clear();

            ILProcessor processor = property_definition.SetMethod.Body.GetILProcessor();
            Instruction ret = processor.Create(OpCodes.Ret);

            processor.Append(processor.Create(OpCodes.Nop));
            processor.Append(processor.Create(OpCodes.Ldarg_1));
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldfld, backing_field));
            processor.Append(processor.Create(OpCodes.Ceq));
            processor.Append(processor.Create(OpCodes.Stloc_0));
            processor.Append(processor.Create(OpCodes.Ldloc_0));
            processor.Append(processor.Create(OpCodes.Brtrue_S, ret));

            processor.Append(processor.Create(OpCodes.Nop));
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldarg_1));
            processor.Append(processor.Create(OpCodes.Stfld, backing_field));
            processor.Append(processor.Create(OpCodes.Ldarg_0));
            processor.Append(processor.Create(OpCodes.Ldstr, property_definition.Name));
            processor.Append(processor.Create(OpCodes.Call, notify_method));
            processor.Append(processor.Create(OpCodes.Nop));

            // Add notifications for dependent properties
            foreach (var target in dependencies.GetDependenciesFor(property_definition.Name))
            {
                Console.WriteLine("      Adding dependency " + target);
                processor.Append(processor.Create(OpCodes.Ldarg_0));
                processor.Append(processor.Create(OpCodes.Ldstr, target));
                processor.Append(processor.Create(OpCodes.Call, notify_method));
                processor.Append(processor.Create(OpCodes.Nop));
            }
            processor.Append(processor.Create(OpCodes.Nop));

            processor.Append(ret);
        }

        public static void Execute(TypeDefinition type_definition, DependencyMap map)
        {
            Rewriter rewriter = new Rewriter(map);

            Console.WriteLine("Rewriting " + type_definition.Name);
            foreach (var property in type_definition.GetAutoProperties())
                rewriter.RewriteAutoProperty(property);
        }
    }
}
