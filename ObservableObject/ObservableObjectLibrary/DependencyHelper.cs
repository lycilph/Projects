using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableObjectLibrary
{
    public static class DependencyHelper
    {
        public static CollectionDependency<T1, T2> CreateCollectionDependency<T1, T2>(Expression<Func<T1>> source, Expression<Func<T2>> target, INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler handler)
        {
           return new CollectionDependency<T1, T2>(source, target, obj, handler); 
        }

        public static PropertyDependency<T1, T2> CreatePropertyDependency<T1, T2>(Expression<Func<T1>> source, Expression<Func<T2>> target, ObservableObject obj, PropertyChangedEventHandler handler)
        {
            return new PropertyDependency<T1, T2>(source, target, obj, handler);
        }
    }
}
