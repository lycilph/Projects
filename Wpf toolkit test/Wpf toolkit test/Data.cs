using System;
using System.ComponentModel;

namespace Wpf_toolkit_test
{
    public class Data
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }
        public Data(DateTime d, string t, double v)
        {
            Date = d;
            Text = t;
            Value = v;
        }
    }

    public class Sorter : ObservableObject
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
        private ListSortDirection? _Direction;
        public ListSortDirection? Direction
        {
            get { return _Direction; }
            set
            {
                if (_Direction == value)
                    return;
                _Direction = value;
                NotifyPropertyChanged("Direction");
            }
        }
        public Sorter(string n, ListSortDirection? d)
        {
            Name = n;
            Direction = d;
        }
    }
}
