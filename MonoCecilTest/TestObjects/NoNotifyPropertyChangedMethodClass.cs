using System.ComponentModel;
using MonoCecilRewriter.Interfaces;

namespace TestObjects
{
    [NotifyPropertyChanged]
    public class NoNotifyPropertyChangedMethodClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
