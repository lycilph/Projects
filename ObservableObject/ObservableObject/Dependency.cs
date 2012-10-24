using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ObservableObject
{
    public class Dependency<T1, T2>
    {
        public Expression<Func<T1>> source;
        public Expression<Func<T2>> target;

    }
}
