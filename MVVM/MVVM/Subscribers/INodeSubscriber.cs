using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVM.Subscribers
{
    internal interface INodeSubscriber
    {
        void DumpToLog();
    }
}
