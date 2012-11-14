using System;
using System.ComponentModel;

namespace MVVM.Expressions
{
    public static class TypeHelper
    {
        public static bool DoesTypeImplementINotifyPropertyChangedAndChanging(Type type)
        {
            return typeof(INotifyPropertyChanged).IsAssignableFrom(type) &&
                   typeof(INotifyPropertyChanging).IsAssignableFrom(type);
        }
    }
}
