using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionTesst
{
    public class A
    {
        public string Text { get; set; }
        public A(string t)
        {
            Text = t;
        }
    }

    public class B
    {
        public readonly Expression<Func<string>> expr;
        public readonly Func<string> func;
        public B(A a)
        {
            expr = () => a.Text;
            func = expr.Compile();
        }
        public override string ToString()
        {
            return func();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a = new A("test");

            WeakReference a_ref = new WeakReference(a);
            WeakReference b_ref = null;
            new Action(() =>
                           {
                               var b = new B(a);
                               b_ref = new WeakReference(b);
                               Console.WriteLine(b);
                               a.Text = "new test";
                               Console.WriteLine(b);
                           })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Expression<Func<A,string, string>> expr = (obj, str) => obj.Text + str;

            Console.WriteLine("a is " + (a_ref.IsAlive ? "alive" : "dead"));
            Console.WriteLine("b is " + (b_ref.IsAlive ? "alive" : "dead"));

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
