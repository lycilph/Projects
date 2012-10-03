using System.Collections.ObjectModel;

namespace Module_Interface
{
    public class Account : ObservableObject
    {
        private string _Name = "";
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public ObservableCollection<Post> Posts { get; set; }

        public Account(string Name)
        {
            _Name = Name;
            Posts = new ObservableCollection<Post>();
        }

        public void Clear()
        {
            Name = string.Empty;
            Posts.Clear();
        }
    }
}
