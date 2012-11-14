using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ObservableObject
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = new Model();
            var vm = new ViewModel(m);

            vm.PropertyChanged += (sender, eventArgs) =>
                                      {
                                          if (eventArgs.PropertyName == "AllCategories")
                                          {
                                              foreach (var category in vm.AllCategories)
                                                  Console.WriteLine(category);
                                          }
                                          else if (eventArgs.PropertyName == "AllCategoriesCount")
                                              Console.WriteLine("(VM) New count " + vm.AllCategoriesCount);
                                      };
            vm.WrappedModel.PropertyChanged += (sender, eventArgs) =>
                                                   {
                                                       if (eventArgs.PropertyName == "CategoriesCount")
                                                           Console.WriteLine("(M) New count " + vm.WrappedModel.CategoriesCount);
                                                   };

            vm.WrappedModel.Categories.Add("Item 1");
            vm.WrappedModel.Categories.Add("Item 2");
            vm.WrappedModel.Categories.Add("Item 3");
            vm.WrappedModel.Categories.Add("Item 4");
            vm.WrappedModel.Categories.Add("Item 5");

            Console.WriteLine("Press any key to continues...");
            Console.ReadKey();
        }
    }
}
