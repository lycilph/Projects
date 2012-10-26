using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ObservableObjectLibrary
{
    public class Dependency<T1, T2> : DependencyBase
    {
        public Expression<Func<T1>> source;
        public Expression<Func<T2>> target;

        public Dependency(Expression<Func<T1>> source, Expression<Func<T2>> target) : base(ExpressionHelper.FindPropertyName(source))
        {
            this.source = source;
            this.target = target;
        }

        public override void Add(ObservableObject obj)
        {
            obj.AddDependency(source, target);
        }

        public override bool DependensOn(string property)
        {
            return ExpressionHelper.DependensOn(source, property);
        }
    }
}
