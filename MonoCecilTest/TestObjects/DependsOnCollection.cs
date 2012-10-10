using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TestObjects
{
    public class DependsOnCollection : INotifyPropertyChanged
    {
        private ObservableCollection<string> Items = new ObservableCollection<string>();
        public ObservableCollection<string> PropItems
        {
            get { return Items; }
            set
            {
                Items = value;
                Items.CollectionChanged += (sender, args) => NotifyPropertyChanged("Count");
            }
        }

        public int Count
        {
            get { return Items.Count; }
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
