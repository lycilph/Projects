using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest.ViewModels
{
    public class ObservableObject : PropertyChangedBase
    {
        public void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            NotifyOfPropertyChange(property);
        }
    }
}
