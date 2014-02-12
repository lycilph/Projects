using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GongDragDropTest
{
    public class Pattern : ObservableObject
    {
        public Category Parent { get; set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                NotifyPropertyChanged(() => Name);
            }
        }
    }
}
