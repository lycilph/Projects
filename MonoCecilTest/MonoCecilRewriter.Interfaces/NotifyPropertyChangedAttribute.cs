using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCecilRewriter.Interfaces
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : Attribute
    {
        public bool WrapFields { get; private set; }

        public NotifyPropertyChangedAttribute(bool wrap_fields)
        {
            WrapFields = wrap_fields;
        }
    }
}
