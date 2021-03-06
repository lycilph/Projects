﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using MonoCecilRewriter.Interfaces;

namespace TestObjects
{
    [NotifyPropertyChanged]
    public class DependsOnAutoCollectionProperty : INotifyPropertyChanged
    {
        public ObservableCollection<string> Items { get; set; }

        public int Count
        {
            get { return Items.Count; }
        }

        public DependsOnAutoCollectionProperty()
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
