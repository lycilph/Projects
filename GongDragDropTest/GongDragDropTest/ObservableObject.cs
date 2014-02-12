using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace GongDragDropTest
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

        public virtual void NotifyPropertyChanged(string property_name)
        {
            if (IsNotifying)
                OnPropertyChanged(new PropertyChangedEventArgs(property_name));
        }

        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyPropertyChanged(property.GetMemberInfo().Name);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
    }
}
