using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace MonoCecilRewriter
{
    public static class Extensions
    {
        public static IEnumerable<TypeDefinition> GetNotifyPropertyChangedClasses(this AssemblyDefinition assembly)
        {
            return assembly.MainModule.Types.Where(t => t.Interfaces.Any(it => it.Name == "INotifyPropertyChanged"));
        }

        public static IEnumerable<PropertyDefinition> GetAutoProperties(this TypeDefinition notify_property_class)
        {
            return notify_property_class.Fields.Where(f => f.Name.Contains("k__BackingField"))
                                               .Select(f => f.Name.Substring(1, f.Name.IndexOf("k__BackingField", StringComparison.InvariantCulture) - 2))
                                               .Select(f => notify_property_class.Properties.Single(p => p.Name == f))
                                               .ToList();
        }

        public static PropertyDefinition GetPropertyFromGetter(this MethodDefinition method)
        {
            return method.DeclaringType.Properties.Single(p => p.GetMethod.Name == method.Name);
        }
    }
}
