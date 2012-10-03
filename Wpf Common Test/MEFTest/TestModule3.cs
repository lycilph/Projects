using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEFTest
{
    [ModuleType(Type = ModuleTypes.Debug)]
    public class TestModule3 : IModule
    {
        public string Name
        {
            get { return "Test module 3"; }
        }
    }
}
