using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Wpf_test
{
    public class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class Category : ObservableObject
    {
        private string _Name;
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
        public Category(string n) { _Name = n; }
    }

    public enum CategoryMatchType { Auto, Manual };

    public class Post : ObservableObject
    {
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (_Text != value)
                {
                    _Text = value;
                    NotifyPropertyChanged("Text");
                }
            }
        }
        private Category _Match = null;
        public Category Match
        {
            get { return _Match; }
            set
            {
                if (_Match != value)
                {
                    _Match = value;
                    NotifyPropertyChanged("Match");
                }
            }
        }
        private CategoryMatchType _MatchType = CategoryMatchType.Auto;
        public CategoryMatchType MatchType
        {
            get { return _MatchType; }
            set
            {
                if (_MatchType != value)
                {
                    _MatchType = value;
                    NotifyPropertyChanged("MatchType");
                }
            }
        }
        public Post(string s) : this(s, null, CategoryMatchType.Auto) { }
        public Post(string s, Category m, CategoryMatchType mt) { _Text = s; _Match = m; _MatchType = mt; }
    }

    public class Account : ObservableObject
    {
        private string _Name;
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
        public ObservableCollection<Post> Posts { get; private set; }
        public Account(string n)
        {
            _Name = n;
            Posts = new ObservableCollection<Post>();
        }
    }
}
