using System;

namespace NotifyPropertyWeaver.Interfaces
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : Attribute { }
}
