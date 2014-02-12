using System.ComponentModel;

namespace FilesystemControlsTest
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void PreNotifyPropertyChanged(string property_name) { }

        protected void NotifyPropertyChanged(string property_name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                PreNotifyPropertyChanged(property_name);
                handler(this, new PropertyChangedEventArgs(property_name));
            }
        }
    }
}
