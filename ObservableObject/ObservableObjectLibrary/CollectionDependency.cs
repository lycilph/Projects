﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace ObservableObjectLibrary
{
    public class CollectionDependency<T1, T2> : Dependency<T1, T2>
    {
        public INotifyCollectionChanged obj;
        public NotifyCollectionChangedEventHandler handler;

        public CollectionDependency(Expression<Func<T1>> source, Expression<Func<T2>> target, INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler handler)
            : base(source, target)
        {
            this.obj = obj;
            this.handler = handler;
        }

        protected override void Remove()
        {
            obj.CollectionChanged -= handler;
        }
    }
}
