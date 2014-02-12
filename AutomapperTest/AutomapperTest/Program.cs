using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Mapper.CreateMap<Data, DataViewModel>();

            var d = new Data { Name = "ABC" };
            var dvm = Mapper.Map<DataViewModel>(d);

            Console.WriteLine(dvm.Name);
            Console.ReadKey();
        }
    }
}
