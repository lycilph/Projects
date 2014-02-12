using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest.ViewModels
{
    [Export(typeof(PageBase))]
    [ExportMetadata("Order", 1)]
    public class StartPageViewModel : PageBase
    {
        public StartPageViewModel() : base("Start")
        {
        }
    }
}
