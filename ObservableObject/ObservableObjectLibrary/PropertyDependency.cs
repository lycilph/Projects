using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.ComponentModel;

namespace ObservableObjectLibrary
{
    public class PropertyDependency<T1, T2> : Dependency<T1, T2>
    {
        public ObservableObject obj;
        public PropertyChangedEventHandler handler;

        public PropertyDependency(Expression<Func<T1>> source, Expression<Func<T2>> target, ObservableObject obj, PropertyChangedEventHandler handler)
            : base(source, target)
        {
            this.obj = obj;
            this.handler = handler;
        }

        protected override void Remove()
        {
            obj.PropertyChanged -= handler;
        }
    }
}
