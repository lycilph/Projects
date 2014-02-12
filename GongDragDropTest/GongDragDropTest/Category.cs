using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GongDragDropTest
{
    public class Category : ObservableObject
    {
        public Group Parent { get; set; }

        private ObservableCollection<Pattern> _Patterns = new ObservableCollection<Pattern>();
        public ObservableCollection<Pattern> Patterns
        {
            get { return _Patterns; }
            private set
            {
                if (_Patterns == value) return;
                _Patterns = value;
                NotifyPropertyChanged(() => Patterns);
            }
        }

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

        public void Add(Pattern p)
        {
            p.Parent = this;
            Patterns.Add(p);
        }
    }
}
