using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wp_Checkbox_Test
{
    public class Item : ObservableObject
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                if (_Selected == value)
                    return;
                _Selected = value;
                NotifyPropertyChanged("Selected");
            }
        }

        public Item(string name, bool selected = false)
        {
            Name = name;
            Selected = selected;
        }
    }
}
