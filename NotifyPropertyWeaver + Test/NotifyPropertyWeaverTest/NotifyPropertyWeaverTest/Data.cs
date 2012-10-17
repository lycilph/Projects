using System.ComponentModel;
using NotifyPropertyWeaver.Interfaces;

namespace NotifyPropertyWeaverTest
{
    [NotifyPropertyChanged]
    public class Data : INotifyPropertyChanged
    {
        public int Count { get; set; }
        public string Text { get; set; }

        public Data()
        {
            Count = 42;
            Text = "Hello world!";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
