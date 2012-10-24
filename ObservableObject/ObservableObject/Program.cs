using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ObservableObject
{
    class Program
    {
        static void Main(string[] args)
        {
            Model m = new Model();
            m.PropertyChanged += (sender, event_args) => Console.WriteLine(string.Format("{0} was changed", event_args.PropertyName));
            m.ModelProp2.CollectionChanged += (sender, event_args) => Console.WriteLine(string.Format("ModelProp2 was changed (action = {0})", event_args.Action));

            ViewModel vm = new ViewModel(m);
            vm.PropertyChanged += (sender, event_args) => Console.WriteLine(string.Format("{0} was changed", event_args.PropertyName));

            Console.WriteLine("----------------------------------");
            m.ModelProp1 = 42;
            Console.WriteLine("----------------------------------");
            Console.WriteLine();

            Console.WriteLine("----------------------------------");
            m.ModelProp2.Add(42);
            Console.WriteLine("----------------------------------");
            Console.WriteLine();

            Console.WriteLine("----------------------------------");
            m.ModelProp2 = new ObservableCollection<int>();
            m.ModelProp2.Add(42);
            Console.WriteLine("----------------------------------");
            Console.WriteLine();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
