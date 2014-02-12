using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnMicroTest.ViewModels
{
    public class PageBase : ObservableObject
    {
        private string _Header;
        public string Header
        {
            get { return _Header; }
            set
            {
                if (_Header == value) return;
                _Header = value;
                NotifyPropertyChanged();
            }
        }

        public PageBase(string header)
        {
            _Header = header;
        }
    }
}
