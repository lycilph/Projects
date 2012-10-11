using System.ComponentModel;
using MonoCecilRewriter.Interfaces;

namespace TestObjects
{
    [NotifyPropertyChanged(true)]
    public class BaseClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property_name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property_name));
        }
    }

    public class DerivedClass : BaseClass {}
}
