using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest.ViewModels
{
    [Export(typeof(PageBase))]
    [ExportMetadata("Order", 2)]
    public class ImagesPageViewModel : PageBase
    {
        public ImagesPageViewModel() : base("Images")
        {
        }
    }
}
