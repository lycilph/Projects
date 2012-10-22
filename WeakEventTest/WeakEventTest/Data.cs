using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WeakEventTest
{
    public class Data : IWeakEventListener, INotifyPropertyChanged
    {
        public readonly ObservableCollection<int> items = new ObservableCollection<int>();

        public Data()
        {
            CollectionChangedEventManager.AddListener(items, this);
        }

        public bool ReceiveWeakEvent(Type manager_type, object sender, EventArgs e)
        {
            if (manager_type == typeof(CollectionChangedEventManager))
                NotifyPropertyChanged("Items");
            else
                return false;

            return true;
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
