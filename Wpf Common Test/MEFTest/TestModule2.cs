﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEFTest
{
    [ModuleType(Type = ModuleTypes.Import)]
    public class TestModule2 : IModule
    {
        public string Name
        {
            get { return "Test module 2"; }
        }
    }
}
