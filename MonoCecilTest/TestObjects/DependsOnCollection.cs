using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace TestObjects
{
    public class DependsOnCollection : INotifyPropertyChanged
    {
        public ObservableCollection<string> Items { get; set; }

        public int Count
        {
            get { return Items.Count; }
        }

        public DependsOnCollection()
        {
            Items = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property_name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
