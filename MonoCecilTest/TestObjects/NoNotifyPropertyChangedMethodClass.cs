using System.ComponentModel;
using MonoCecilRewriter.Interfaces;

namespace TestObjects
{
    [NotifyPropertyChanged]
    public class NoNotifyPropertyChangedMethodClass : INotifyPropertyChanged
    {
        public int Prop { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
