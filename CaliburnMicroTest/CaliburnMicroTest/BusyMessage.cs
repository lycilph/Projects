using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest
{
    public class BusyMessage
    {
        public bool IsBusy { get; set; }

        public BusyMessage(bool is_busy)
        {
            IsBusy = is_busy;
        }
    }
}
