using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace MEFTest
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleTypeAttribute : ExportAttribute
    {
        public ModuleTypeAttribute() : base(typeof(IModule)) {}

        public ModuleTypes Type { get; set; }
    }

    public enum ModuleTypes
    {
        Import,
        Debug
    }
}
