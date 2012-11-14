using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Sandbox
{
    public class A
    {
        public int Prop1 { get; set; }
        public int Prop2 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();

            Expression<Func<int>> e1 = () => a.Prop1;
            Expression<Func<A, int>> e2 = obj => obj.Prop1;
        }
    }
}
