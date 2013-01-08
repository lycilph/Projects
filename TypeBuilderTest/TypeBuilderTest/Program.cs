using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TypeBuilderTest
{
    public interface IStuff
    {
        string MockMethod();
    }

    public class Base
    {
        public string s()
        {
            return "abc";
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            AssemblyName assembly_name = new AssemblyName("MockAssembly");
            AssemblyBuilder assembly_builder = Thread.GetDomain().DefineDynamicAssembly(assembly_name, AssemblyBuilderAccess.Run);
            ModuleBuilder module_builder = assembly_builder.DefineDynamicModule("MockModule");

            TypeBuilder type_builder = module_builder.DefineType("MockType", TypeAttributes.Public | TypeAttributes.Class, typeof(Base));
            //MethodBuilder method_builder = type_builder.DefineMethod("MockMethod", MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, typeof(string), Type.EmptyTypes);

            //var il = method_builder.GetILGenerator();
            //il.Emit(OpCodes.Ldstr, "ABC");
            //il.Emit(OpCodes.Ret);

            //// Associate method to interface
            //type_builder.AddInterfaceImplementation(typeof(IStuff));
            //type_builder.DefineMethodOverride(method_builder, typeof(IStuff).GetMethod("MockMethod"));

            //// Generate type implementing interface IStuff
            //Type genereted_type = type_builder.CreateType();


            //var genereted_object = (IStuff)Activator.CreateInstance(genereted_type);
            //Console.WriteLine(genereted_object.MockMethod());

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
