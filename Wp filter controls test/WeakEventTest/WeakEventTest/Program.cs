using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeakEventTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Data data = new Data();
            data.PropertyChanged += (sender, event_args) => Console.WriteLine(string.Format("{0} changed", event_args.PropertyName));
            data.items.Add(42);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
