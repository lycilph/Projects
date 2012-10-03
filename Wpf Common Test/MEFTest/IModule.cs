using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace MEFTest
{
    [InheritedExport(typeof(IModule))]
    public interface IModule
    {
        string Name { get; }
    }
}
