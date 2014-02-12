using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GongDragDropTest
{
    public class Group : ObservableObject
    {
        private ObservableCollection<Category> _Categories = new ObservableCollection<Category>();
        public ObservableCollection<Category> Categories
        {
            get { return _Categories; }
            private set
            {
                if (_Categories == value) return;
                _Categories = value;
                NotifyPropertyChanged(() => Categories);
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

        public void Add(Category c)
        {
            c.Parent = this;
            Categories.Add(c);
        }
    }
}
