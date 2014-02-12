using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EditableTextBlockV2
{
    // Stolen from Caliburn Micro
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public bool IsNotifying { get; set; }
        
        public ObservableObject()
        {
            IsNotifying = true;
        }

        public void RefreshAll()
        {
            NotifyPropertyChanged(string.Empty);
        }

        public virtual void NotifyPropertyChanged([CallerMemberName] string property_name = "")
        {
            if (IsNotifying)
                OnPropertyChanged(new PropertyChangedEventArgs(property_name));
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
    }
}
