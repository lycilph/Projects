using System;

namespace MonoCecilRewriter.Interfaces
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : Attribute {}
}
