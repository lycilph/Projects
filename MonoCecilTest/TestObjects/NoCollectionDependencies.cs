using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MonoCecilRewriter.Interfaces;
using System.Collections.ObjectModel;

namespace TestObjects
{
    [NotifyPropertyChanged]
    public class NoCollectionDependencies : INotifyPropertyChanged
    {
        public ObservableCollection<string> Items { get; set; }

        public int Prop { get; set; }

        public NoCollectionDependencies()
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
