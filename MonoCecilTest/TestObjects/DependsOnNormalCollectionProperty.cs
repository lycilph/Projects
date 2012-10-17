using System.Collections.ObjectModel;
using System.ComponentModel;
using MonoCecilRewriter.Interfaces;

namespace TestObjects
{
    [NotifyPropertyChanged]
    public class DependsOnNormalCollectionProperty : INotifyPropertyChanged
    {
        private ObservableCollection<string> _Items = new ObservableCollection<string>(); 
        public ObservableCollection<string> Items
        {
            get { return _Items; }
            set { _Items = value; }
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
