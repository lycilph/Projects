using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WeakEventTest
{
    public class WeakEventNotifier : IWeakEventListener
    {
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
