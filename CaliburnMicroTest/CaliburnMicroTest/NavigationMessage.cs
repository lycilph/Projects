using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest
{
    public class NavigationMessage
    {
        public enum NavigationType { First };

        public NavigationType Type { get; private set; }

        public NavigationMessage(NavigationType t)
        {
            Type = t;
        }
    }
}
